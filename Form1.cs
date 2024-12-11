using OpenCvSharp;
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
        private string FolderPath;
        private string resultFolderPath;

        private static string imagePath;


        private string[] classNames;

        private Net net;
        private Mat inputImage;

        private bool modelLoaded = false; // 用于判断模型是否已经加载

        // 置信度阈值
        private float confidenceThreshold = 0.8f;

        // NMS阈值
        private float nmsThreshold = 0.0f;

        // 字体大小
        private double fontSize = 1.0;

        // 存储每个类别的颜色和对比色
        private readonly Dictionary<int, (Scalar labelColor, Scalar textColor)> ColorMap = new Dictionary<int, (Scalar, Scalar)>();

        // 类别数，可以根据实际情况调整
        private readonly int numClasses = 80;

        // 定义一个全局的随机数生成器
        private readonly Random rand = new Random();
        public MainForm()
        {
            InitializeComponent();
            InitializeControls();
            TryLoadDefaultFiles();
            InitializeColors();
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

            // 设置NMS阈值控件
            numericUpDownNMS.Value = (decimal)nmsThreshold;
            numericUpDownNMS.Minimum = 0;
            numericUpDownNMS.Maximum = 1;
            numericUpDownNMS.DecimalPlaces = 1;
            numericUpDownNMS.Increment = 0.1M;
            numericUpDownNMS.ValueChanged += (sender, e) =>
            {
                // 更新NMS阈值
                nmsThreshold = (float)numericUpDownNMS.Value;
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
            txtFolderPath.ReadOnly = true;
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
        // 启动时计算颜色
        private void InitializeColors()
        {
            for (int i = 0; i < numClasses; i++)
            {
                Scalar color = GenerateRandomColor();
                Scalar complementaryColor = CalculateComplementaryColor(color);
                ColorMap[i] = (color, complementaryColor);
            }
        }
        // 随机生成颜色
        private Scalar GenerateRandomColor()
        {
            return new Scalar(rand.Next(200, 255), rand.Next(200, 255), rand.Next(200, 255));
        }
        // 计算互补色
        private Scalar CalculateComplementaryColor(Scalar originalColor)
        {
            // 获取原色的 R、G、B 值
            byte r = (byte)originalColor.Val0;
            byte g = (byte)originalColor.Val1;
            byte b = (byte)originalColor.Val2;

            // 计算补色：通过与255的异或运算来确保强烈的对比
            byte compR = (byte)(255 - r);
            byte compG = (byte)(255 - g);
            byte compB = (byte)(255 - b);

            return new Scalar(compR, compG, compB);
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
            using (System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog())
            {
                openFileDialog.Filter = "图像文件|*.jpg;*.jpeg;*.png;*.bmp|所有文件|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    inputImage = Cv2.ImRead(openFileDialog.FileName);

                    imagePath = openFileDialog.FileName;

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

            // 将推理结果图像保存到 exe 目录下
            try
            {
                Cv2.ImWrite(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "predictions.jpg"), detectedImage);
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

        // 选择文件夹按钮事件
        private void BtnSelectFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    FolderPath = folderDialog.SelectedPath;
                    txtFolderPath.Text = FolderPath; // 显示选择的文件夹路径
                }
            }
        }
        // 批量检测按钮事件
        private void BtnBatchDetect_Click(object sender, EventArgs e)
        {
            // 清空txtDetectionResults内容
            txtDetectionResults.Clear();

            if (string.IsNullOrEmpty(FolderPath))
            {
                MessageBox.Show("请选择一个文件夹！");
                return;
            }

            // 获取当前时间作为文件夹名
            string timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            resultFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, timeStamp);

            // 创建一个以当前时间命名的文件夹
            Directory.CreateDirectory(resultFolderPath);

            // 获取文件夹中的所有图像文件
            var imageFiles = Directory.GetFiles(FolderPath, "*.jpg")
                .Concat(Directory.GetFiles(FolderPath, "*.jpeg"))
                .Concat(Directory.GetFiles(FolderPath, "*.png"))
                .Concat(Directory.GetFiles(FolderPath, "*.bmp"))
                .ToArray();

            if (imageFiles.Length == 0)
            {
                MessageBox.Show("该文件夹中没有支持的图像文件！");
                return;
            }

            // 开始批量检测
            foreach (var imagePath in imageFiles)
            {
                ProcessImage(imagePath);
            }
            // 生成检测结果文本
            string resultTxtPath = Path.Combine(resultFolderPath, "result.txt");
            using (StreamWriter writer = new StreamWriter(resultTxtPath))
            {
                writer.WriteLine(txtDetectionResults.Text); // 将TextBox中的结果写入文件
            }
            MessageBox.Show("批量检测完成！");
        }

        // 处理每张图片并保存结果
        private void ProcessImage(string imagePath)
        {
            // 读取图片
            Mat inputImage = Cv2.ImRead(imagePath);
            MainForm.imagePath = imagePath;
            if (inputImage.Empty())
            {
                MessageBox.Show($"无法读取图片：{imagePath}");
                return;
            }

            // 使用YOLO模型进行检测
            var detectedImage = DetectObjects(inputImage);

            // 获取原始文件名
            string fileName = Path.GetFileName(imagePath);

            // 保存推理后的图像
            string resultImagePath = Path.Combine(resultFolderPath, fileName);
            try
            {
                Cv2.ImWrite(resultImagePath, detectedImage);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存图像时发生错误: {ex.Message}");
                return;
            }
        }
        // 文件加载通用方法
        private string LoadFile(string filter)
        {
            using (System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog())
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
            txtDetectionResults.AppendText(imagePath + Environment.NewLine);

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
                nmsThreshold,   // NMS 阈值
                out int[] indices
            );

            var outputImage = image.Clone();

            if (indices.Length > 0)
            {
                foreach (var idx in indices)
                {
                    var box = boxes[idx];
                    int classId = classIds[idx];
                    string label = $"{classNames[classId]}: {confidences[idx]:0.00}";
                    string resultText = $"Label: {classNames[classId]}, Confidence: {confidences[idx]:0.00}";

                    txtDetectionResults.AppendText(resultText + Environment.NewLine);

                    // 获取预先计算的颜色和对比色
                    var (labelColor, textColor) = ColorMap[classId];

                    // 绘制带背景的文字
                    var labelSize = Cv2.GetTextSize(label, HersheyFonts.HersheySimplex, fontSize, 1, out int baseline);

                    // 计算文字位置
                    Point labelOrigin;
                    Point backgroundOrigin;

                    if (box.Y - labelSize.Height - 10 < 0)
                    {
                        // 如果顶部超出，将文字显示在标注框下方
                        labelOrigin = new Point(box.X + 5, box.Y + box.Height + 10);
                        backgroundOrigin = labelOrigin;
                    }
                    else
                    {
                        // 默认显示在标注框上方
                        labelOrigin = new Point(box.X + 5, box.Y - labelSize.Height - 10);
                        backgroundOrigin = labelOrigin;

                    }

                    // 确保文字背景不会超出图像边界
                    int backgroundHeight = labelSize.Height + 17;
                    int backgroundWidth = Math.Min(labelSize.Width + 10, image.Width - labelOrigin.X);

                    // 背景框位置调整
                    backgroundOrigin.X -= 6;
                    backgroundOrigin.Y -= 5;

                    Cv2.Rectangle(outputImage, new Rect(backgroundOrigin, new Size(backgroundWidth, backgroundHeight)), labelColor, Cv2.FILLED);

                    Cv2.PutText(
                        outputImage,
                        label,
                        new Point(labelOrigin.X, labelOrigin.Y + labelSize.Height),
                        HersheyFonts.HersheySimplex,
                        fontSize,
                        textColor,
                        1
                        );

                    // 绘制检测框
                    Cv2.Rectangle(outputImage, box, labelColor, 2);
                }
            }
            else
            {
                // 未检测到物体时添加提示文字
                string noDetectionText = "No objects detected";
                double fontSize = 1.0;
                Scalar backgroundColor = new Scalar(0, 0, 255); // 红色背景
                Scalar textColor = new Scalar(0, 0, 0); // 黑色文字

                // 获取文字大小
                var textSize = Cv2.GetTextSize(noDetectionText, HersheyFonts.HersheySimplex, fontSize, 1, out int baseline);

                // 计算文字和背景的位置
                Point textOrg = new Point((width - textSize.Width) / 2, (height + textSize.Height) / 2);
                Rect backgroundRect = new Rect(
                    new Point(textOrg.X - 10, textOrg.Y - textSize.Height - 5), // 背景左上角
                    new Size(textSize.Width + 20, textSize.Height + 17) // 背景大小
                );

                // 绘制背景
                Cv2.Rectangle(outputImage, backgroundRect, backgroundColor, Cv2.FILLED);

                // 绘制文字
                Cv2.PutText(
                    outputImage,
                    noDetectionText,
                    textOrg,
                    HersheyFonts.HersheySimplex,
                    fontSize,
                    textColor,
                    1
                );
                txtDetectionResults.AppendText(noDetectionText + Environment.NewLine);

            }

            // 获取TextBox的宽度
            int textBoxWidth = txtDetectionResults.ClientSize.Width;

            // 计算分割线长度
            string separatorLine = new string('-', textBoxWidth / 10);

            // 向TextBox中添加分割线
            txtDetectionResults.AppendText(separatorLine + Environment.NewLine);


            return outputImage;
        }
    }
}
