﻿using OpenCvSharp;
using OpenCvSharp.Dnn;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace YOLODetectionApp
{
    public partial class MainForm : Form
    {
        // 存储文件路径的变量
        private string modelConfig;
        private string modelWeights;
        private string classNamesFile;

        private string[] classNames;
        private Net net;
        private Mat inputImage;
        private bool modelLoaded = false; // 用于判断模型是否已经加载

        // 置信度阈值，默认设置为0.5
        private float confidenceThreshold = 0.5f;

        private double fontSize = 1.0;
        private readonly Dictionary<int, Scalar> labelColors = new Dictionary<int, Scalar>(); // 存储类别对应的颜色
        private readonly Random rand = new Random();
        private string resultImagePath;

        public MainForm()
        {
            InitializeComponent();
            InitializeControls();
            TryLoadDefaultFiles();
        }

        // 初始化控件设置
        private void InitializeControls()
        {
            // 设置置信度阈值控件
            numericUpDownConfidence.Value = (decimal)confidenceThreshold;
            numericUpDownConfidence.Minimum = 0;
            numericUpDownConfidence.Maximum = 1;
            numericUpDownConfidence.DecimalPlaces = 2;
            numericUpDownConfidence.Increment = 0.01M;
            numericUpDownConfidence.ValueChanged += (sender, e) =>
            {
                // 更新置信度阈值
                confidenceThreshold = (float)numericUpDownConfidence.Value;
            };

            // 设置字体大小控件
            numericUpDownFontSize.Value = (decimal)fontSize;
            numericUpDownFontSize.Minimum = 0.1M;
            numericUpDownFontSize.Maximum = 9.0M;
            numericUpDownFontSize.DecimalPlaces = 1;
            numericUpDownFontSize.Increment = 0.1M;
            numericUpDownFontSize.ValueChanged += (sender, e) =>
            {
                fontSize = (double)numericUpDownFontSize.Value;
            };

            // 设置路径文本框为只读
            txtConfigPath.ReadOnly = true;
            txtWeightsPath.ReadOnly = true;
            txtClassNamesPath.ReadOnly = true;
        }

        private void TryLoadDefaultFiles()
        {
            string exeDir = AppDomain.CurrentDomain.BaseDirectory;

            // 查找目录中的文件
            var configFiles = Directory.GetFiles(exeDir, "*.cfg");
            var weightFiles = Directory.GetFiles(exeDir, "*.weights");
            var namesFiles = Directory.GetFiles(exeDir, "*.names");

            // 自动加载找到的文件（如存在）
            if (configFiles.Length > 0)
                modelConfig = configFiles[0];
            if (weightFiles.Length > 0)
                modelWeights = weightFiles[0];
            if (namesFiles.Length > 0)
                classNamesFile = namesFiles[0];

            // 更新UI
            txtConfigPath.Text = modelConfig;
            txtWeightsPath.Text = modelWeights;
            txtClassNamesPath.Text = classNamesFile;

            // 加载模型
            if (!string.IsNullOrEmpty(modelConfig) && !string.IsNullOrEmpty(modelWeights) && !string.IsNullOrEmpty(classNamesFile))
                LoadYOLOModel();
        }

        // 加载YOLO模型
        private void LoadYOLOModel()
        {
            // 检查是否需要重新加载模型
            if (IsModelLoaded())
            {
                return;
            }

            // 加载类别文件
            if (!LoadClassNames())
            {
                return;
            }

            try
            {
                // 加载网络
                net = CvDnn.ReadNetFromDarknet(modelConfig, modelWeights);
                if (net.Empty())
                {
                    MessageBox.Show("YOLO模型加载失败，请检查配置和权重文件！");
                    return;
                }

                net.SetPreferableBackend(Backend.OPENCV);
                net.SetPreferableTarget(Target.CPU);

                modelLoaded = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载YOLO模型时发生错误：{ex.Message}");
            }
        }

        // 检查模型是否已经加载且路径没有变化
        private bool IsModelLoaded()
        {
            return modelLoaded && modelConfig == txtConfigPath.Text && modelWeights == txtWeightsPath.Text && classNamesFile == txtClassNamesPath.Text;
        }

        // 加载类别文件
        private bool LoadClassNames()
        {
            if (string.IsNullOrEmpty(classNamesFile) || !File.Exists(classNamesFile))
            {
                MessageBox.Show("类别文件未找到，请检查路径！");
                return false;
            }

            try
            {
                classNames = File.ReadAllLines(classNamesFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"读取类别文件时发生错误：{ex.Message}");
                return false;
            }

            return true;
        }

        // 选择图像并显示
        private void BtnSelectImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "图像文件|*.jpg;*.jpeg;*.png;*.bmp|所有文件|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    inputImage = Cv2.ImRead(openFileDialog.FileName);

                    // 获取图片和PictureBox的大小
                    int pictureBoxWidth = pictureBoxInput.Width;
                    int pictureBoxHeight = pictureBoxInput.Height;

                    // 调整图像大小以适应PictureBox
                    Mat resizedImage = ResizeImage(inputImage, pictureBoxWidth, pictureBoxHeight);

                    // 将调整后的图像显示在PictureBox中
                    pictureBoxInput.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(resizedImage);
                }
            }
        }

        // 检测图像中的物体
        private void BtnDetect_Click(object sender, EventArgs e)
        {
            // 检查配置文件和图像是否有效
            if (string.IsNullOrEmpty(modelConfig) || string.IsNullOrEmpty(modelWeights) || string.IsNullOrEmpty(classNamesFile))
            {
                MessageBox.Show("配置文件、权重文件或类别文件尚未加载，请先选择文件！");
                return;
            }

            if (inputImage.Empty())
            {
                MessageBox.Show("请先选择一张图像！");
                return;
            }

            // 加载模型（如果还未加载）
            LoadYOLOModel();

            // 进行物体检测
            var detectedImage = DetectObjects(inputImage);
            // 获取应用程序的目录路径
            string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // 设置保存路径为 "predictions.jpg"
            resultImagePath = Path.Combine(exeDirectory, "predictions.jpg");

            // 将推理结果图像保存到 exe 目录下
            try
            {
                Cv2.ImWrite(resultImagePath, detectedImage);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存图像时发生错误: {ex.Message}");
            }

            // 获取图片和PictureBox的大小
            int pictureBoxWidth = pictureBoxOutput.Width;
            int pictureBoxHeight = pictureBoxOutput.Height;

            // 调整图像大小以适应PictureBox
            Mat resizedImage = ResizeImage(detectedImage, pictureBoxWidth, pictureBoxHeight);

            // 将调整后的图像显示在PictureBox中
            pictureBoxOutput.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(resizedImage);

            // 计算分隔线的长度（基于文本框的宽度）
            string separatorLine = new string('-', txtDetectionResults.ClientSize.Width / 10);
            // 在TextBox中添加分隔线
            txtDetectionResults.AppendText(separatorLine + Environment.NewLine);
        }

        // 图像缩放方法
        private Mat ResizeImage(Mat image, int maxWidth, int maxHeight)
        {
            // 计算缩放比例
            double aspectRatio = (double)image.Width / image.Height;
            int newWidth = maxWidth;
            int newHeight = (int)(maxWidth / aspectRatio);

            // 如果新高度超过PictureBox高度，调整宽度和高度
            if (newHeight > maxHeight)
            {
                newHeight = maxHeight;
                newWidth = (int)(maxHeight * aspectRatio);
            }

            // 使用OpenCV进行图像缩放
            Mat resizedImage = new Mat();
            Cv2.Resize(image, resizedImage, new Size(newWidth, newHeight));
            return resizedImage;
        }

        // 选择并加载模型配置文件
        private void BtnLoadConfig_Click(object sender, EventArgs e)
        {
            // 记录之前的路径
            string previousConfigPath = modelConfig;

            // 选择文件
            modelConfig = LoadFile("配置文件|*.cfg|所有文件|*.*");

            // 如果文件选择不是取消（路径不为空），才更新文本框和变量
            if (!string.IsNullOrEmpty(modelConfig))
            {
                txtConfigPath.Text = modelConfig;
            }
            else
            {
                // 如果点击取消，恢复之前的路径
                modelConfig = previousConfigPath;
            }
        }

        // 选择并加载模型权重文件
        private void BtnLoadWeights_Click(object sender, EventArgs e)
        {
            // 记录之前的权重文件路径
            string previousWeightsPath = modelWeights;

            // 选择文件
            modelWeights = LoadFile("权重文件|*.weights|所有文件|*.*");

            // 如果文件选择不是取消（路径不为空），才更新文本框和变量
            if (!string.IsNullOrEmpty(modelWeights))
            {
                txtWeightsPath.Text = modelWeights;
            }
            else
            {
                // 如果点击取消，恢复之前的路径
                modelWeights = previousWeightsPath;
            }
        }

        // 选择并加载类别文件
        private void BtnLoadClassNames_Click(object sender, EventArgs e)
        {
            // 记录之前的类别文件路径
            string previousClassNamesPath = classNamesFile;

            // 选择文件
            classNamesFile = LoadFile("类别文件|*.names|所有文件|*.*");

            // 如果文件选择不是取消（路径不为空），才更新文本框和变量
            if (!string.IsNullOrEmpty(classNamesFile))
            {
                txtClassNamesPath.Text = classNamesFile;
            }
            else
            {
                // 如果点击取消，恢复之前的路径
                classNamesFile = previousClassNamesPath;
            }
        }

        // 文件加载通用方法
        private string LoadFile(string filter)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = filter;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return openFileDialog.FileName;
                }
            }
            return null;
        }

        // 检测物体
        private Mat DetectObjects(Mat image)
        {
            int width = image.Width;
            int height = image.Height;

            // 图像预处理
            var blob = CvDnn.BlobFromImage(image, 1.0 / 255.0, new Size(416, 416), new Scalar(0, 0, 0), true, false);
            net.SetInput(blob);

            // 获取输出层
            var outputLayerNames = net.GetUnconnectedOutLayersNames();
            var outputs = outputLayerNames.Select(name => new Mat()).ToArray();

            // 前向传播
            net.Forward(outputs, outputLayerNames);

            // 解析检测结果
            var boxes = new List<Rect>();
            var confidences = new List<float>();
            var classIds = new List<int>();

            foreach (var output in outputs)
            {
                for (int i = 0; i < output.Rows; i++)
                {
                    float[] detection = new float[output.Cols];
                    Marshal.Copy(output.Ptr(i), detection, 0, output.Cols);

                    float confidence = detection[4];
                    if (confidence > confidenceThreshold) // 置信度阈值
                    {
                        int centerX = (int)(detection[0] * width);
                        int centerY = (int)(detection[1] * height);
                        int boxWidth = (int)(detection[2] * width);
                        int boxHeight = (int)(detection[3] * height);
                        int x = centerX - boxWidth / 2;
                        int y = centerY - boxHeight / 2;

                        boxes.Add(new Rect(x, y, boxWidth, boxHeight));
                        confidences.Add(confidence);

                        float[] scores = detection.Skip(5).ToArray();
                        classIds.Add(Array.IndexOf(scores, scores.Max()));
                    }
                }
            }

            // 使用 NMSBoxes 进行非极大值抑制
            CvDnn.NMSBoxes(
                boxes.ToArray(),
                confidences.ToArray(),
                confidenceThreshold,  // 置信度阈值
                0.4f,   // NMS 阈值
                out int[] indices
            );

            var outputImage = image.Clone();
            foreach (var idx in indices)
            {
                var box = boxes[idx];
                int classId = classIds[idx];
                string label = $"{classNames[classId]}: {confidences[idx]:0.00}";
                string resultText = $"Label: {classNames[classId]}, Confidence: {confidences[idx]:0.00}";

                txtDetectionResults.AppendText(resultText + Environment.NewLine);

                // 获取颜色
                if (!labelColors.ContainsKey(classId))
                {
                    labelColors[classId] = GenerateRandomColor();
                }
                Scalar color = labelColors[classId];

                // 绘制带背景的文字
                var labelSize = Cv2.GetTextSize(label, HersheyFonts.HersheyDuplex, fontSize, 1, out int baseline);

                // 计算文字位置
                Point labelOrigin;
                if (box.Y - labelSize.Height - 5 < 0)
                {
                    // 如果顶部超出，将文字显示在标注框下方
                    labelOrigin = new Point(box.X, box.Y + box.Height + 5);
                }
                else
                {
                    // 默认显示在标注框上方
                    labelOrigin = new Point(box.X, box.Y - labelSize.Height - 5);
                }

                // 确保文字背景不会超出图像边界
                int backgroundHeight = labelSize.Height + 5;
                int backgroundWidth = Math.Min(labelSize.Width, image.Width - labelOrigin.X);

                Cv2.Rectangle(outputImage, new Rect(labelOrigin, new Size(backgroundWidth, backgroundHeight)), color, Cv2.FILLED);
                Cv2.PutText(outputImage, label, new Point(labelOrigin.X, labelOrigin.Y + labelSize.Height), HersheyFonts.HersheyDuplex, fontSize, new Scalar(0, 0, 0), 1);

                // 绘制检测框
                Cv2.Rectangle(outputImage, box, color, 2);
            }

            return outputImage;
        }

        // 随机生成颜色
        private Scalar GenerateRandomColor()
        {
            return new Scalar(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255));
        }
    }
}
