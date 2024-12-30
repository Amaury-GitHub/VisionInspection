using LibVLCSharp.Shared;
using Ookii.Dialogs.WinForms;
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

namespace YOLODetectionApp
{
    public partial class MainForm : Form
    {
        // 存储文件路径的变量
        private string modelConfig;

        private string modelWeights;

        private string classNamesFile;

        private string modelConfigLoad;

        private string modelWeightsLoad;

        private string classNamesFileLoad;

        private string FolderPath;

        private string resultFolderPath;

        private string imagePath;

        private string snapshotPath;

        private string[] classNames;

        private Net net;

        private Mat inputImage;

        private bool modelLoaded = false; // 用于判断模型是否已经加载

        private bool OPENCL = false; // 判断是否启用OPENCL

        // 置信度阈值
        private float confidenceThreshold = 0.8f;

        // NMS阈值
        private float nmsThreshold = 0.0f;

        // 字体大小
        private double fontSize = 1.0;

        // 循环任务延迟时间
        private int delayMs = 500;

        // 推理图片分辨率
        private int imageSize = 608;

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

        private bool _isStreamLive = false;

        // 构造函数
        public MainForm()
        {
            // 初始化组件
            InitializeComponent();

            // 初始化控件
            InitializeControls();

            // 尝试加载默认文件
            TryLoadDefaultFiles();

            // 初始化颜色
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
            numericUpDownConfidence.ReadOnly = true;
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
            numericUpDownNMS.ReadOnly = true;
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
            numericUpDownFontSize.ReadOnly = true;
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
            numericUpDownDelayTime.ReadOnly = true;
            numericUpDownDelayTime.ValueChanged += (sender, e) =>
            {
                delayMs = (int)(numericUpDownDelayTime.Value * 1000); // 将秒转换为毫秒
            };

            // 设置推理分辨率控件
            numericUpDownImageSize.Value = (decimal)imageSize;
            numericUpDownImageSize.Minimum = 416.0M;
            numericUpDownImageSize.Maximum = 608.0M;
            numericUpDownImageSize.DecimalPlaces = 0;
            numericUpDownImageSize.Increment = 32M;
            numericUpDownImageSize.ReadOnly = true;
            numericUpDownImageSize.ValueChanged += (sender, e) =>
            {
                imageSize = (int)(numericUpDownImageSize.Value);
            };

            // 设置路径文本框为只读
            txtConfigPath.ReadOnly = true;
            txtWeightsPath.ReadOnly = true;
            txtClassNamesPath.ReadOnly = true;
            txtFolderPath.ReadOnly = true;
            txtImagePath.ReadOnly = true;
        }

        // 自动加载程序目录下的 YOLO 模型
        private void TryLoadDefaultFiles()
        {
            // 获取当前应用程序的目录
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
        }

        // 启动时计算颜色 初始化颜色
        private void InitializeColors()
        {
            // 遍历所有类别
            for (int i = 0; i < numClasses; i++)
            {
                // 生成随机颜色
                Scalar color = GenerateRandomColor();

                // 计算互补颜色
                Scalar complementaryColor = CalculateComplementaryColor(color);

                // 将颜色和互补颜色存入ColorMap
                ColorMap[i] = (color, complementaryColor);
            }
        }

        // 随机生成颜色 生成一个随机颜色
        private Scalar GenerateRandomColor()
        {
            // 返回一个Scalar对象，包含三个随机数，范围在200到255之间
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

        // 加载 YOLO 模型
        private void LoadYOLOModel()
        {
            // 检查配置文件和图像是否有效
            if (string.IsNullOrEmpty(modelConfig) || string.IsNullOrEmpty(modelWeights) || string.IsNullOrEmpty(classNamesFile))
            {
                MessageBox.Show("配置文件、权重文件或类别文件尚未加载，请先选择文件！");
                return;
            }

            // 检查是否需要重新加载模型
            if (IsModelLoaded())
            {
                return;
            }

            try
            {
                // 加载类别名称
                LoadClassNames();

                // 加载网络
                net = CvDnn.ReadNetFromDarknet(modelConfig, modelWeights);
                if (net.Empty())
                {
                    MessageBox.Show("YOLO模型加载失败，请检查配置和权重文件！");
                    return;
                }

                net.SetPreferableBackend(Backend.OPENCV);
                if (enableOPENCL.Checked)
                {
                    net.SetPreferableTarget(Target.OPENCL);
                    OPENCL = true;
                }
                else
                {
                    net.SetPreferableTarget(Target.CPU);
                    OPENCL = false;
                }

                classNamesFileLoad = classNamesFile;
                modelConfigLoad = modelConfig;
                modelWeightsLoad = modelWeights;
                modelLoaded = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载YOLO模型时发生错误：{ex.Message}");
                return;
            }
        }

        // 检查模型是否已经加载且路径没有变化 判断模型是否已加载
        private bool IsModelLoaded()
        {
            // 如果模型已加载，并且模型配置、模型权重和类别名称文件路径与文本框中的路径一致，以及是否启用OPENCL, 则返回true
            return modelLoaded && modelConfigLoad == modelConfig && modelWeightsLoad == modelWeights && classNamesFileLoad == classNamesFile && OPENCL == enableOPENCL.Checked;
        }

        // 加载类别文件 加载类别文件
        private void LoadClassNames()
        {
            // 如果类别文件为空或者文件不存在，则弹出提示框并返回false
            if (string.IsNullOrEmpty(classNamesFile) || !File.Exists(classNamesFile))
            {
                MessageBox.Show("类别文件未找到，请检查路径！");
            }

            try
            {
                // 读取类别文件中的所有行
                classNames = File.ReadAllLines(classNamesFile);
            }
            catch (Exception ex)
            {
                // 如果读取文件时发生错误，则弹出提示框并返回false
                MessageBox.Show($"读取类别文件时发生错误：{ex.Message}");
            }
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

        // 当点击检测按钮时触发的事件
        private void BtnDetect_Click(object sender, EventArgs e)
        {
            // 加载 YOLO 模型
            LoadYOLOModel();
            if (!modelLoaded)
            {
                return;
            }

            // 更新按钮状态
            UpdateButtonStatus(btnBatchDetect, "批量检测", false);
            UpdateButtonStatus(btnDetect, "单次检测中...", false);
            UpdateButtonStatus(btnStreamDetect, "Stream\r\n单次检测", false);
            UpdateButtonStatus(btnStreamLiveDetect, "Stream\r\n连续检测", false);

            try
            {
                if (string.IsNullOrEmpty(imagePath))
                {
                    MessageBox.Show("请先选择一张图像！");
                    return;
                }

                // 使用 OpenCV 读取图像
                inputImage = Cv2.ImRead(txtImagePath.Text);

                // 进行物体检测
                var detectedImage = DetectObjects(inputImage, imagePath);

                // 将推理结果图像保存到 exe 目录下
                Cv2.ImWrite(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "predictions.jpg"), detectedImage);

                RenewPictureBox(detectedImage, pictureBox);
            }
            catch (Exception)
            {
            }
            finally
            {
                // 更新按钮状态
                UpdateButtonStatus(btnBatchDetect, "批量检测", true);
                UpdateButtonStatus(btnDetect, "单次检测", true);
                UpdateButtonStatus(btnStreamDetect, "Stream\r\n单次检测", true);
                UpdateButtonStatus(btnStreamLiveDetect, "Stream\r\n连续检测", true);
            }
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

        // 选择文件夹按钮事件 点击选择文件夹按钮时触发的事件
        private void BtnSelectFolder_Click(object sender, EventArgs e)
        {
            // 调用SelectPath方法，选择文件夹，并将返回的文件夹路径赋值给FolderPath变量
            FolderPath = SelectPath(true, null, txtFolderPath);  // 选择文件夹
        }

        // 批量检测按钮事件
        private async void BtnBatchDetect_Click(object sender, EventArgs e)
        {
            // 加载 YOLO 模型
            LoadYOLOModel();
            if (!modelLoaded)
            {
                return;
            }

            // 更新按钮状态
            UpdateButtonStatus(btnBatchDetect, "批量检测中...", false);
            UpdateButtonStatus(btnDetect, "单次检测", false);
            UpdateButtonStatus(btnStreamDetect, "Stream\r\n单次检测", false);
            UpdateButtonStatus(btnStreamLiveDetect, "Stream\r\n连续检测", false);

            try
            {
                // 判断FolderPath是否为空，如果为空，则弹出提示框，并返回
                if (string.IsNullOrEmpty(FolderPath))
                {
                    MessageBox.Show("请选择一个文件夹！");
                    return;
                }

                // 获取当前时间作为文件夹名
                string timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

                // 将当前时间命名的文件夹路径赋值给resultFolderPath
                resultFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, timeStamp);

                // 创建一个以当前时间命名的文件夹
                Directory.CreateDirectory(resultFolderPath);

                // 获取文件夹中的所有图像文件
                var imageFiles = Directory.GetFiles(FolderPath, "*.jpg")
                    .Concat(Directory.GetFiles(FolderPath, "*.jpeg"))
                    .Concat(Directory.GetFiles(FolderPath, "*.png"))
                    .Concat(Directory.GetFiles(FolderPath, "*.bmp"))
                    .ToArray();

                // 判断文件夹中是否有图像文件，如果没有，则弹出提示框，并返回
                if (imageFiles.Length == 0)
                {
                    MessageBox.Show("该文件夹中没有支持的图像文件！");

                    return;
                }

                // 清空txtDetectionResults内容
                txtDetectionResults.Clear();

                // 开始批量检测
                foreach (var imagePath in imageFiles)
                {
                    try
                    {
                        // 处理每张图片
                        await ProcessImage(imagePath);

                        // 每处理完一张图片后等待，减少CPU占用
                        await Task.Delay(delayMs);
                    }
                    catch (Exception)
                    {
                    }
                }

                // 生成检测结果文本
                using (StreamWriter writer = new StreamWriter(Path.Combine(resultFolderPath, "result.txt")))
                {
                    // 将TextBox中的结果写入文件
                    writer.WriteLine(txtDetectionResults.Text);
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                // 更新按钮状态
                UpdateButtonStatus(btnBatchDetect, "批量检测", true);

                UpdateButtonStatus(btnDetect, "单次检测", true);

                UpdateButtonStatus(btnStreamDetect, "Stream\r\n单次检测", true);

                UpdateButtonStatus(btnStreamLiveDetect, "Stream\r\n连续检测", true);
            }
        }

        // 处理每张图片并保存结果
        private Task ProcessImage(string imagePath)
        {
            // 读取图片
            inputImage = Cv2.ImRead(imagePath);

            // 使用YOLO模型进行检测
            var detectedImage = DetectObjects(inputImage, imagePath);

            // 获取原始文件名
            string fileName = Path.GetFileName(imagePath);

            // 保存推理后的图像
            Cv2.ImWrite(Path.Combine(resultFolderPath, fileName), detectedImage);

            return Task.CompletedTask;
        }

        // Stream 连接
        private void BtnStreamConnect_Click(object sender, EventArgs e)
        {
            // 如果没有连接，则尝试连接
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
                            if (_isPlaying)
                                return;
                            AppendTextToTextbox("连接失败");
                            Disconnect();
                        }));
                    };
                    _mediaPlayer.Stopped += (s, args) =>
                    {
                        BeginInvoke(new Action(() =>
                        {
                            if (!_isPlaying)
                                return;
                            AppendTextToTextbox("连接中断，尝试重新连接...");
                            Reconnect(); // 尝试重连
                        }));
                    };

                    // 初始化并播放 Stream
                    PlayStream(txtStreamPath.Text);

                    // 设置截图路径
                    snapshotPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "snapshot.jpg");

                    // 启动定时器进行连接验证
                    _isPlaying = false;

                    // 更新按钮状态
                    UpdateButtonStatus(btnStreamConnect, "连接中...", true);
                    txtStreamPath.Enabled = false;
                    _isConnected = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("连接失败：" + ex.Message);
                }
            }
            else
            {
                // 如果已经连接，则断开连接
                Disconnect();
            }
        }

        // Stream 断开
        private void Disconnect()
        {
            try
            {
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
                UpdateButtonStatus(btnStreamConnect, "连接 Stream", true);
                txtStreamPath.Enabled = true;
                _isConnected = false;
                _isPlaying = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("断开时发生错误：" + ex.Message);
            }
        }

        // Stream 重连 尝试重新连接
        private void Reconnect()
        {
            if (!_isPlaying)
                return; // 如果已手动断开，不执行重连

            // 延迟2秒后执行
            Task.Delay(2000).ContinueWith(t =>
            {
                // 在UI线程中执行
                BeginInvoke(new Action(() =>
                {
                    // 尝试重新播放 Stream
                    PlayStream(txtStreamPath.Text);
                }));
            });
        }

        // Stream 播放
        private void PlayStream(string StreamUrl)
        {
            try
            {
                if (_mediaPlayer != null)
                {
                    // 创建并加载 Stream
                    var media = new Media(_libVLC, StreamUrl, FromType.FromLocation);

                    // 禁用音频
                    _mediaPlayer.Mute = true; // 静音，防止音频播放

                    // 绑定 MediaPlayer 到 VideoView 控件以显示视频
                    videoView1.MediaPlayer = _mediaPlayer;

                    // 开始播放
                    _mediaPlayer.Play(media);

                    AppendTextToTextbox("连接中...");

                    _connectionCheckTimer = new System.Timers.Timer(1000); // 每秒检查一次
                    _connectionCheckTimer.Elapsed += CheckConnection;
                    _connectionCheckTimer.Start();
                }
            }
            catch (Exception ex)
            {
                AppendTextToTextbox($"连接时发生错误：{ex.Message}");
            }
        }

        // 检查 stream 的连接状态
        private void CheckConnection(object sender, System.Timers.ElapsedEventArgs e)
        {
            // 如果媒体播放器不为空
            if (_mediaPlayer != null)
            {
                // 拍摄快照
                bool success = _mediaPlayer.TakeSnapshot(0, snapshotPath, 0, 0);

                // 如果拍摄成功
                if (success)
                {
                    // 设置正在播放
                    _isPlaying = true;

                    // 停止检查
                    _connectionCheckTimer.Stop();

                    // 添加文本到文本框
                    AppendTextToTextbox("连接完成");

                    // 如果流是直播
                    if (_isStreamLive)
                    {
                        // 更新按钮状态
                        UpdateButtonStatus(btnStreamConnect, "断开连接", false);
                    }
                    else
                    {
                        // 更新按钮状态
                        UpdateButtonStatus(btnStreamConnect, "断开连接", true);
                    }
                }
            }
        }

        // stream 的单张检测
        private void BtnStreamDetect_Click(object sender, EventArgs e)
        {
            // 加载 YOLO 模型
            LoadYOLOModel();
            if (!modelLoaded)
            {
                return;
            }

            // 更新按钮状态
            UpdateButtonStatus(btnBatchDetect, "批量检测", false);
            UpdateButtonStatus(btnDetect, "单次检测", false);
            UpdateButtonStatus(btnStreamDetect, "Stream\r\n单次检测中...", false);
            UpdateButtonStatus(btnStreamLiveDetect, "Stream\r\n连续检测", false);

            try
            {
                // 检查是否已连接 Stream
                if (!_isConnected)
                {
                    MessageBox.Show("请先连接 Stream！");
                    return;
                }

                // 检查 Stream 是否正在播放
                if (!_isPlaying)
                {
                    MessageBox.Show("请等待 Stream 连接完成！");
                    return;
                }
                try
                {
                    // 尝试截图并检查是否成功
                    bool success = _mediaPlayer.TakeSnapshot(0, snapshotPath, 0, 0);

                    if (success)
                    {
                        inputImage = Cv2.ImRead(snapshotPath);

                        // 进行物体检测
                        var detectedImage = DetectObjects(inputImage, null);

                        // 将推理结果图像保存到 exe 目录下
                        Cv2.ImWrite(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "predictions.jpg"), detectedImage);

                        RenewPictureBox(detectedImage, pictureBox);
                    }
                }
                catch (Exception)
                {
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                // 更新按钮状态
                UpdateButtonStatus(btnBatchDetect, "批量检测", true);
                UpdateButtonStatus(btnDetect, "单次检测", true);
                UpdateButtonStatus(btnStreamDetect, "Stream\r\n单次检测", true);
                UpdateButtonStatus(btnStreamLiveDetect, "Stream\r\n连续检测", true);
            }
        }

        // stream 的实时检测
        private async void BtnStreamLiveDetect_Click(object sender, EventArgs e)
        {
            // 加载 YOLO 模型
            LoadYOLOModel();
            if (!modelLoaded)
            {
                return;
            }

            // 更新按钮状态
            UpdateButtonStatus(btnBatchDetect, "批量检测", false);
            UpdateButtonStatus(btnDetect, "单次检测", false);
            UpdateButtonStatus(btnStreamDetect, "Stream\r\n单次检测", false);
            UpdateButtonStatus(btnStreamLiveDetect, "Stream\r\n连续检测中...", true);
            UpdateButtonStatus(btnStreamConnect, null, false);

            try
            {
                // 检查是否已连接 Stream
                if (!_isConnected)
                {
                    MessageBox.Show("请先连接 Stream！");
                    return;
                }

                // 检查 Stream 是否已连接完成
                if (!_isPlaying)
                {
                    MessageBox.Show("请等待 Stream 连接完成！");
                    return;
                }

                // 检查是否已取消检测
                if (cancellationTokenSource != null)
                {
                    cancellationTokenSource.Cancel();
                    cancellationTokenSource = null;
                    _isStreamLive = false;

                    return;
                }

                // 创建新的 CancellationTokenSource
                cancellationTokenSource = new CancellationTokenSource();
                var cancellationToken = cancellationTokenSource.Token;
                _isStreamLive = true;

                try
                {
                    // 在新的线程中执行检测
                    await Task.Run(async () =>
                    {
                        while (!cancellationToken.IsCancellationRequested)
                        {
                            // 尝试截图并进行检测
                            bool success = _mediaPlayer.TakeSnapshot(0, snapshotPath, 0, 0);

                            if (success)
                            {
                                // 读取截图
                                inputImage = Cv2.ImRead(snapshotPath);

                                // 进行检测
                                var detectedImage = await Task.Run(() => DetectObjects(inputImage, null));

                                // 将推理结果图像保存到 exe 目录下
                                Cv2.ImWrite(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "predictions.jpg"), detectedImage);

                                // 更新显示图像
                                RenewPictureBox(detectedImage, pictureBox);
                            }

                            // 延迟一段时间
                            await Task.Delay(delayMs);
                        }
                    }, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                // 更新按钮状态
                UpdateButtonStatus(btnBatchDetect, "批量检测", true);
                UpdateButtonStatus(btnDetect, "单次检测", true);
                UpdateButtonStatus(btnStreamDetect, "Stream\r\n单次检测", true);
                UpdateButtonStatus(btnStreamLiveDetect, "Stream\r\n连续检测", true);
                UpdateButtonStatus(btnStreamConnect, null, true);
            }
        }

        // 刷新图片显示
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

        // 路径选择 选择文件夹或文件路径
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

        // 刷新日志 向文本框中追加文本
        private void AppendTextToTextbox(string text)
        {
            // 判断当前线程是否为 UI 线程
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

        // 更新按钮状态 更新按钮状态
        private void UpdateButtonStatus(Button button, string text, bool isEnable)
        {
            // 如果按钮需要调用
            if (button.InvokeRequired)
            {
                // 如果文本不为空
                if (!string.IsNullOrEmpty(text))
                {
                    // 调用按钮的Invoke方法，更新按钮文本
                    button.Invoke(new Action(() => button.Text = text));
                }

                // 调用按钮的Invoke方法，更新按钮是否可用
                button.Invoke(new Action(() => button.Enabled = isEnable));
            }
            else
            {
                // 如果文本不为空
                if (!string.IsNullOrEmpty(text))
                {
                    // 更新按钮文本
                    button.Text = text;
                }

                // 更新按钮是否可用
                button.Enabled = isEnable;
            }
        }

        // 检测物体
        private Mat DetectObjects(Mat image, string imagePath)
        {
            // 将图像路径添加到TextBox中
            AppendTextToTextbox(imagePath);

            // 获取图像的宽度和高度
            int width = image.Width;
            int height = image.Height;

            // 图像预处理
            var blob = CvDnn.BlobFromImage(image, 1.0 / 255.0, new OpenCvSharp.Size(imageSize, imageSize), new Scalar(0, 0, 0), true, false);
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