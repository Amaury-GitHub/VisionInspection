using LibVLCSharp.Shared;
using OpenCvSharp;
using OpenCvSharp.Dnn;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ookii.Dialogs.WinForms;

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
        private string imagePath; 
        private string snapshotPath; 


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

        // 循环任务延迟时间
        private int delayMs = 500;

        // 存储每个类别的颜色和对比色
        private readonly Dictionary<int, (Scalar labelColor, Scalar textColor)> ColorMap = new Dictionary<int, (Scalar, Scalar)>();

        // 类别数，可以根据实际情况调整
        private readonly int numClasses = 80;

        // 定义一个全局的随机数生成器
        private readonly Random rand = new Random();


        private LibVLC _libVLC;
        private MediaPlayer _mediaPlayer;
        private bool _isConnected = false;
        private bool _isPlaying = false;
        private System.Timers.Timer _connectionCheckTimer;
        private CancellationTokenSource cancellationTokenSource;



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
            // 设置循环任务延迟时间控件
            numericUpDownDelayTime.Value = delayMs / 1000.0M; // 将毫秒转换为秒
            numericUpDownDelayTime.Minimum = 0.1M;
            numericUpDownDelayTime.Maximum = 10.0M; // 设置最大值为10秒
            numericUpDownDelayTime.DecimalPlaces = 1;
            numericUpDownDelayTime.Increment = 0.1M;
            numericUpDownDelayTime.ValueChanged += (sender, e) =>
            {
                delayMs = (int)(numericUpDownDelayTime.Value * 1000); // 将秒转换为毫秒
            };

            // 设置路径文本框为只读
            txtConfigPath.ReadOnly = true;
            txtWeightsPath.ReadOnly = true;
            txtClassNamesPath.ReadOnly = true;
            txtFolderPath.ReadOnly = true;
            txtImagePath.ReadOnly = true;
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
            // 选择文件
            imagePath = SelectPath(false, "图像文件|*.jpg;*.jpeg;*.png;*.bmp|所有文件|*.*", txtImagePath);

            if (!string.IsNullOrEmpty(imagePath))
            {
                // 使用 OpenCV 读取图像
                inputImage = Cv2.ImRead(imagePath);

                // 更新 PictureBox 显示图像
                RenewPictureBox(inputImage, pictureBox);
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

            if (inputImage == null)
            {
                MessageBox.Show("请先选择一张图像！");
                return;
            }

            // 加载模型（如果还未加载）
            LoadYOLOModel();

            // 进行物体检测
            var detectedImage = DetectObjects(inputImage, imagePath);

            // 将推理结果图像保存到 exe 目录下
            try
            {
                Cv2.ImWrite(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "predictions.jpg"), detectedImage);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存图像时发生错误: {ex.Message}");
            }

            RenewPictureBox(detectedImage, pictureBox);
        }



        // 选择并加载模型配置文件
        private void BtnLoadConfig_Click(object sender, EventArgs e)
        {
            // 选择文件
            modelConfig = SelectPath(false, "配置文件|*.cfg|所有文件|*.*", txtConfigPath);

        }

        // 选择并加载模型权重文件
        private void BtnLoadWeights_Click(object sender, EventArgs e)
        {
            // 选择文件
            modelWeights = SelectPath(false, "权重文件|*.weights|所有文件|*.*", txtWeightsPath);

        }

        // 选择并加载类别文件
        private void BtnLoadClassNames_Click(object sender, EventArgs e)
        {
            // 选择文件
            classNamesFile = SelectPath(false, "类别文件|*.names|所有文件|*.*", txtClassNamesPath);


        }

        // 选择文件夹按钮事件
        private void BtnSelectFolder_Click(object sender, EventArgs e)
        {
            FolderPath = SelectPath(true, null, txtFolderPath);  // 选择文件夹

        }

        // 批量检测按钮事件
        private async void BtnBatchDetect_Click(object sender, EventArgs e)
        {
            UpdateButtonText(BtnBatchDetect, "批量检测中");

            // 清空txtDetectionResults内容
            txtDetectionResults.Clear();

            if (string.IsNullOrEmpty(FolderPath))
            {
                MessageBox.Show("请选择一个文件夹！");
                UpdateButtonText(BtnBatchDetect, "批量检测");
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
                UpdateButtonText(BtnBatchDetect, "批量检测");
                return;
            }

            // 开始批量检测
            foreach (var imagePath in imageFiles)
            {
                try
                {
                    await ProcessImage(imagePath);  // 处理每张图片
                    await Task.Delay(delayMs); // 每处理完一张图片后等待，减少CPU占用
                }
                catch (Exception)
                {
                }
            }

            // 生成检测结果文本
            string resultTxtPath = Path.Combine(resultFolderPath, "result.txt");
            using (StreamWriter writer = new StreamWriter(resultTxtPath))
            {
                writer.WriteLine(txtDetectionResults.Text); // 将TextBox中的结果写入文件
            }
            UpdateButtonText(BtnBatchDetect, "批量检测");
        }

        // 处理每张图片并保存结果
        private Task ProcessImage(string imagePath)
        {
            // 读取图片
            inputImage = Cv2.ImRead(imagePath);

            if (inputImage.Empty())
            {
                MessageBox.Show($"无法读取图片：{imagePath}");
                return Task.CompletedTask;
            }

            // 使用YOLO模型进行检测
            var detectedImage = DetectObjects(inputImage, imagePath);

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
                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }

        private void BtnRtspConnect_Click(object sender, EventArgs e)
        {
            if (!_isConnected)
            {
                try
                {
                    // 创建 LibVLC 实例和 MediaPlayer
                    _libVLC = new LibVLC();
                    _mediaPlayer = new MediaPlayer(_libVLC);

                    // 监听播放错误事件
                    _mediaPlayer.EncounteredError += (s, args) =>
                    {
                        // 播放失败时弹窗提示
                        BeginInvoke(new Action(() =>
                        {
                            MessageBox.Show("播放失败，请检查地址是否正确或网络连接！");
                            Disconnect();
                        }));
                    };

                    // 创建并加载 RTSP 流媒体
                    var media = new Media(_libVLC, txtRtspPath.Text, FromType.FromLocation);
                    _mediaPlayer.Play(media);

                    // 禁用音频
                    _mediaPlayer.Mute = true;  // 静音，防止音频播放

                    // 绑定 MediaPlayer 到 VideoView 控件以显示视频
                    videoView1.MediaPlayer = _mediaPlayer;

                    // 设置截图路径
                    snapshotPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "snapshot.jpg");

                    // 启动定时器进行连接验证
                    _isPlaying = false;
                    _connectionCheckTimer = new System.Timers.Timer(1000); // 每秒检查一次
                    _connectionCheckTimer.Elapsed += CheckConnection;
                    _connectionCheckTimer.Start();

                    // 更新按钮状态
                    UpdateButtonText(btnRtspConnect, "连接中...");
                    txtRtspPath.Enabled = false;
                    _isConnected = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("连接失败：" + ex.Message);
                }
            }
            else
            {
                Disconnect();
            }
        }

        private void Disconnect()
        {
            try
            {
                // 取消实时检测任务
                cancellationTokenSource?.Cancel();
                cancellationTokenSource = null;

                // 停止播放并释放资源
                if (_mediaPlayer != null)
                {
                    _mediaPlayer.Stop();
                    _mediaPlayer.Dispose();
                    _mediaPlayer = null;
                }

                _libVLC?.Dispose();
                _libVLC = null;

                // 停止连接检查定时器
                _connectionCheckTimer?.Stop();
                _connectionCheckTimer?.Dispose();

                // 更新按钮状态
                UpdateButtonText(btnRtspConnect, "连接 RTSP");
                txtRtspPath.Enabled = true;
                _isConnected = false;
                _isPlaying = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("断开时发生错误：" + ex.Message);
            }
        }

        // 检查连接状态的定时器回调
        private void CheckConnection(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_mediaPlayer != null && !_isPlaying)
            {
                bool success = _mediaPlayer.TakeSnapshot(0, snapshotPath, 0, 0);
                if (success)
                {
                    UpdateButtonText(btnRtspConnect, "断开连接");
                    _isPlaying = true;
                    _connectionCheckTimer.Stop(); // 停止检查
                }
            }
        }

        private void BtnRtspDetect_Click(object sender, EventArgs e)
        {
            if (!_isConnected)
            {
                MessageBox.Show("请先连接 RTSP！");
                return;
            }
            if (!_isPlaying)
            {
                MessageBox.Show("请等待 RTSP 连接完成！");
                return;
            }
            try
            {
                    // 尝试截图并检查是否成功
                    bool success = _mediaPlayer.TakeSnapshot(0, snapshotPath, 0, 0);

                    if (success)
                    {
                        // 检查配置文件和图像是否有效
                        if (string.IsNullOrEmpty(modelConfig) || string.IsNullOrEmpty(modelWeights) || string.IsNullOrEmpty(classNamesFile))
                        {
                            MessageBox.Show("配置文件、权重文件或类别文件尚未加载，请先选择文件！");
                            return;
                        }

                        // 加载模型（如果还未加载）
                        LoadYOLOModel();

                        inputImage = Cv2.ImRead(snapshotPath);

                        // 进行物体检测
                        var detectedImage = DetectObjects(inputImage, null);

                        // 将推理结果图像保存到 exe 目录下
                        try
                        {
                            Cv2.ImWrite(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "predictions.jpg"), detectedImage);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"保存图像时发生错误: {ex.Message}");
                        }

                        RenewPictureBox(detectedImage, pictureBox);
                    }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("检测失败：" + ex.Message);
            }

        }


        private async void BtnRtspLiveDetect_Click(object sender, EventArgs e)
        {
            if (!_isConnected)
            {
                MessageBox.Show("请先连接 RTSP！");
                return;
            }
            if (!_isPlaying)
            {
                MessageBox.Show("请等待 RTSP 连接完成！");
                return;
            }

            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource = null;
                UpdateButtonText(btnRtspLiveDetect, "RTSP live");
                return;
            }

            cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            UpdateButtonText(btnRtspLiveDetect, "停止检测");

            try
            {
                await Task.Run(async () =>
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        // 尝试截图并进行检测
                        bool success = _mediaPlayer.TakeSnapshot(0, snapshotPath, 0, 0);
                        if (success)
                        {
                            if (string.IsNullOrEmpty(modelConfig) || string.IsNullOrEmpty(modelWeights) || string.IsNullOrEmpty(classNamesFile))
                            {
                                MessageBox.Show("配置文件、权重文件或类别文件尚未加载，请先选择文件！");
                                break;
                            }

                            LoadYOLOModel();

                            inputImage = Cv2.ImRead(snapshotPath);

                            var detectedImage = await Task.Run(() => DetectObjects(inputImage, null));

                            RenewPictureBox(detectedImage, pictureBox);
                        }

                        await Task.Delay(delayMs);
                    }
                }, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // 检测任务被取消
            }
            finally
            {
                UpdateButtonText(btnRtspLiveDetect, "RTSP live");
            }
        }



        private void RenewPictureBox(Mat inputImage, PictureBox pictureBox)
        {
            // 获取 PictureBox 的宽度和高度
            int pictureBoxWidth = pictureBox.Width;
            int pictureBoxHeight = pictureBox.Height;

            // 调整图像大小以适应 PictureBox
            Mat resizedImage = ResizeImage(inputImage, pictureBoxWidth, pictureBoxHeight);

            // 将调整后的图像转换为 Bitmap 并显示在 PictureBox 中
            pictureBox.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(resizedImage);
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
            Cv2.Resize(image, resizedImage, new OpenCvSharp.Size(newWidth, newHeight));
            return resizedImage;
        }
        // 文件加载通用方法
        private string SelectPath(bool isFolder, string filter = null, TextBox txtPath = null)
        {
            string selectedPath = null;

            if (isFolder)
            {
                // 选择文件夹
                using (var folderDialog = new VistaFolderBrowserDialog())
                {
                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        selectedPath = folderDialog.SelectedPath;  // 获取选择的文件夹路径
                    }
                }
            }
            else
            {
                // 选择文件
                using (var fileDialog = new VistaOpenFileDialog())
                {
                    fileDialog.Filter = filter;

                    if (fileDialog.ShowDialog() == DialogResult.OK)
                    {
                        selectedPath = fileDialog.FileName;  // 获取选择的文件路径
                    }
                }
            }

            // 如果选择了路径，更新 TextBox 显示路径
            if (txtPath != null)
            {
                if (!string.IsNullOrEmpty(selectedPath))
                {
                    txtPath.Text = selectedPath;  // 显示选择的路径
                }
                else
                {
                    // 如果用户取消选择，恢复之前的路径
                    selectedPath = txtPath.Text;
                }
            }

            return selectedPath;  // 返回选择的路径（可能是文件夹或文件路径）
        }
        private void AppendTextToTextbox(string text)
        {
            if (txtDetectionResults.InvokeRequired)
            {
                // 如果当前线程不是 UI 线程，使用 Invoke 进行调用
                txtDetectionResults.Invoke(new Action(() => AppendTextToTextbox(text)));
            }
            else
            {
                // 在 UI 线程上直接追加文本
                txtDetectionResults.AppendText(text + Environment.NewLine);
            }
        }

        private void UpdateButtonText(Button button, string text)
        {
            if (button.InvokeRequired)
            {
                button.Invoke(new Action(() => button.Text = text));
            }
            else
            {
                button.Text = text;
            }
        }


        // 检测物体
        private Mat DetectObjects(Mat image, string imagePath)
        {
            AppendTextToTextbox(imagePath);

            int width = image.Width;
            int height = image.Height;

            // 图像预处理
            var blob = CvDnn.BlobFromImage(image, 1.0 / 255.0, new OpenCvSharp.Size(416, 416), new Scalar(0, 0, 0), true, false);
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
                    AppendTextToTextbox(resultText);


                    // 获取预先计算的颜色和对比色
                    var (labelColor, textColor) = ColorMap[classId];

                    // 绘制带背景的文字
                    var labelSize = Cv2.GetTextSize(label, HersheyFonts.HersheySimplex, fontSize, 1, out int baseline);

                    // 计算文字位置
                    OpenCvSharp.Point labelOrigin;
                    OpenCvSharp.Point backgroundOrigin;

                    if (box.Y - labelSize.Height - 10 < 0)
                    {
                        // 如果顶部超出，将文字显示在标注框内部
                        labelOrigin = new OpenCvSharp.Point(box.X + 5, box.Y + labelSize.Height - 17);
                        backgroundOrigin = labelOrigin;
                    }
                    else
                    {
                        // 默认显示在标注框上方
                        labelOrigin = new OpenCvSharp.Point(box.X + 5, box.Y - labelSize.Height - 10);
                        backgroundOrigin = labelOrigin;

                    }

                    // 确保文字背景不会超出图像边界
                    int backgroundHeight = labelSize.Height + 17;
                    int backgroundWidth = Math.Min(labelSize.Width + 10, image.Width - labelOrigin.X);

                    // 背景框位置调整
                    backgroundOrigin.X -= 6;
                    backgroundOrigin.Y -= 5;

                    Cv2.Rectangle(outputImage, new Rect(backgroundOrigin, new OpenCvSharp.Size(backgroundWidth, backgroundHeight)), labelColor, Cv2.FILLED);

                    Cv2.PutText(
                        outputImage,
                        label,
                        new OpenCvSharp.Point(labelOrigin.X, labelOrigin.Y + labelSize.Height),
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
                OpenCvSharp.Point textOrg = new OpenCvSharp.Point((width - textSize.Width) / 2, (height + textSize.Height) / 2);
                Rect backgroundRect = new Rect(
                    new OpenCvSharp.Point(textOrg.X - 10, textOrg.Y - textSize.Height - 5), // 背景左上角
                    new OpenCvSharp.Size(textSize.Width + 20, textSize.Height + 17) // 背景大小
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
                AppendTextToTextbox(noDetectionText);

            }

            // 获取TextBox的宽度
            int textBoxWidth = txtDetectionResults.ClientSize.Width;

            // 计算分割线长度
            string separatorLine = new string('-', textBoxWidth / 10);

            // 向TextBox中添加分割线
            AppendTextToTextbox(separatorLine);

            return outputImage;
        }
    }
}
