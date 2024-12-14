namespace YOLODetectionApp
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.btnSelectImage = new System.Windows.Forms.Button();
            this.btnDetect = new System.Windows.Forms.Button();
            this.numericUpDownConfidence = new System.Windows.Forms.NumericUpDown();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.BtnLoadConfig = new System.Windows.Forms.Button();
            this.BtnLoadWeights = new System.Windows.Forms.Button();
            this.BtnLoadClassNames = new System.Windows.Forms.Button();
            this.txtConfigPath = new System.Windows.Forms.TextBox();
            this.txtWeightsPath = new System.Windows.Forms.TextBox();
            this.txtClassNamesPath = new System.Windows.Forms.TextBox();
            this.numericUpDownFontSize = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDetectionResults = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownNMS = new System.Windows.Forms.NumericUpDown();
            this.BtnSelectFolder = new System.Windows.Forms.Button();
            this.BtnBatchDetect = new System.Windows.Forms.Button();
            this.txtFolderPath = new System.Windows.Forms.TextBox();
            this.txtImagePath = new System.Windows.Forms.TextBox();
            this.btnRtspDetect = new System.Windows.Forms.Button();
            this.btnRtspLiveDetect = new System.Windows.Forms.Button();
            this.txtRtspPath = new System.Windows.Forms.TextBox();
            this.videoView1 = new LibVLCSharp.WinForms.VideoView();
            this.btnRtspConnect = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDownDelayTime = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownConfidence)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFontSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNMS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.videoView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDelayTime)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(800, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(1008, 570);
            this.pictureBox.TabIndex = 1;
            this.pictureBox.TabStop = false;
            // 
            // btnSelectImage
            // 
            this.btnSelectImage.Location = new System.Drawing.Point(12, 243);
            this.btnSelectImage.Name = "btnSelectImage";
            this.btnSelectImage.Size = new System.Drawing.Size(150, 50);
            this.btnSelectImage.TabIndex = 2;
            this.btnSelectImage.Text = "选择图像";
            this.btnSelectImage.UseVisualStyleBackColor = true;
            this.btnSelectImage.Click += new System.EventHandler(this.BtnSelectImage_Click);
            // 
            // btnDetect
            // 
            this.btnDetect.Location = new System.Drawing.Point(601, 243);
            this.btnDetect.Name = "btnDetect";
            this.btnDetect.Size = new System.Drawing.Size(150, 50);
            this.btnDetect.TabIndex = 3;
            this.btnDetect.Text = "开始检测";
            this.btnDetect.UseVisualStyleBackColor = true;
            this.btnDetect.Click += new System.EventHandler(this.BtnDetect_Click);
            // 
            // numericUpDownConfidence
            // 
            this.numericUpDownConfidence.Location = new System.Drawing.Point(631, 24);
            this.numericUpDownConfidence.Name = "numericUpDownConfidence";
            this.numericUpDownConfidence.Size = new System.Drawing.Size(120, 25);
            this.numericUpDownConfidence.TabIndex = 4;
            this.numericUpDownConfidence.Tag = "";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(553, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "置信度";
            // 
            // BtnLoadConfig
            // 
            this.BtnLoadConfig.Location = new System.Drawing.Point(12, 11);
            this.BtnLoadConfig.Name = "BtnLoadConfig";
            this.BtnLoadConfig.Size = new System.Drawing.Size(150, 50);
            this.BtnLoadConfig.TabIndex = 6;
            this.BtnLoadConfig.Text = "选择配置文件";
            this.BtnLoadConfig.UseVisualStyleBackColor = true;
            this.BtnLoadConfig.Click += new System.EventHandler(this.BtnLoadConfig_Click);
            // 
            // BtnLoadWeights
            // 
            this.BtnLoadWeights.Location = new System.Drawing.Point(12, 69);
            this.BtnLoadWeights.Name = "BtnLoadWeights";
            this.BtnLoadWeights.Size = new System.Drawing.Size(150, 50);
            this.BtnLoadWeights.TabIndex = 7;
            this.BtnLoadWeights.Text = "选择权重文件";
            this.BtnLoadWeights.UseVisualStyleBackColor = true;
            this.BtnLoadWeights.Click += new System.EventHandler(this.BtnLoadWeights_Click);
            // 
            // BtnLoadClassNames
            // 
            this.BtnLoadClassNames.Location = new System.Drawing.Point(12, 127);
            this.BtnLoadClassNames.Name = "BtnLoadClassNames";
            this.BtnLoadClassNames.Size = new System.Drawing.Size(150, 50);
            this.BtnLoadClassNames.TabIndex = 8;
            this.BtnLoadClassNames.Text = "选择类别文件";
            this.BtnLoadClassNames.UseVisualStyleBackColor = true;
            this.BtnLoadClassNames.Click += new System.EventHandler(this.BtnLoadClassNames_Click);
            // 
            // txtConfigPath
            // 
            this.txtConfigPath.Location = new System.Drawing.Point(168, 24);
            this.txtConfigPath.Name = "txtConfigPath";
            this.txtConfigPath.Size = new System.Drawing.Size(333, 25);
            this.txtConfigPath.TabIndex = 9;
            // 
            // txtWeightsPath
            // 
            this.txtWeightsPath.Location = new System.Drawing.Point(168, 82);
            this.txtWeightsPath.Name = "txtWeightsPath";
            this.txtWeightsPath.Size = new System.Drawing.Size(333, 25);
            this.txtWeightsPath.TabIndex = 10;
            // 
            // txtClassNamesPath
            // 
            this.txtClassNamesPath.Location = new System.Drawing.Point(168, 140);
            this.txtClassNamesPath.Name = "txtClassNamesPath";
            this.txtClassNamesPath.Size = new System.Drawing.Size(333, 25);
            this.txtClassNamesPath.TabIndex = 11;
            // 
            // numericUpDownFontSize
            // 
            this.numericUpDownFontSize.Location = new System.Drawing.Point(631, 100);
            this.numericUpDownFontSize.Name = "numericUpDownFontSize";
            this.numericUpDownFontSize.Size = new System.Drawing.Size(120, 25);
            this.numericUpDownFontSize.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(538, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 13;
            this.label2.Text = "文字大小";
            // 
            // txtDetectionResults
            // 
            this.txtDetectionResults.Location = new System.Drawing.Point(0, 418);
            this.txtDetectionResults.Multiline = true;
            this.txtDetectionResults.Name = "txtDetectionResults";
            this.txtDetectionResults.ReadOnly = true;
            this.txtDetectionResults.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDetectionResults.Size = new System.Drawing.Size(794, 152);
            this.txtDetectionResults.TabIndex = 14;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(574, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 15);
            this.label3.TabIndex = 16;
            this.label3.Text = "NMS";
            // 
            // numericUpDownNMS
            // 
            this.numericUpDownNMS.Location = new System.Drawing.Point(631, 62);
            this.numericUpDownNMS.Name = "numericUpDownNMS";
            this.numericUpDownNMS.Size = new System.Drawing.Size(120, 25);
            this.numericUpDownNMS.TabIndex = 15;
            this.numericUpDownNMS.Tag = "";
            // 
            // BtnSelectFolder
            // 
            this.BtnSelectFolder.Location = new System.Drawing.Point(12, 185);
            this.BtnSelectFolder.Name = "BtnSelectFolder";
            this.BtnSelectFolder.Size = new System.Drawing.Size(150, 50);
            this.BtnSelectFolder.TabIndex = 17;
            this.BtnSelectFolder.Text = "选择文件夹";
            this.BtnSelectFolder.UseVisualStyleBackColor = true;
            this.BtnSelectFolder.Click += new System.EventHandler(this.BtnSelectFolder_Click);
            // 
            // BtnBatchDetect
            // 
            this.BtnBatchDetect.Location = new System.Drawing.Point(601, 185);
            this.BtnBatchDetect.Name = "BtnBatchDetect";
            this.BtnBatchDetect.Size = new System.Drawing.Size(150, 50);
            this.BtnBatchDetect.TabIndex = 18;
            this.BtnBatchDetect.Text = "批量检测";
            this.BtnBatchDetect.UseVisualStyleBackColor = true;
            this.BtnBatchDetect.Click += new System.EventHandler(this.BtnBatchDetect_Click);
            // 
            // txtFolderPath
            // 
            this.txtFolderPath.Location = new System.Drawing.Point(168, 198);
            this.txtFolderPath.Name = "txtFolderPath";
            this.txtFolderPath.Size = new System.Drawing.Size(333, 25);
            this.txtFolderPath.TabIndex = 19;
            // 
            // txtImagePath
            // 
            this.txtImagePath.Location = new System.Drawing.Point(168, 256);
            this.txtImagePath.Name = "txtImagePath";
            this.txtImagePath.Size = new System.Drawing.Size(333, 25);
            this.txtImagePath.TabIndex = 20;
            // 
            // btnRtspDetect
            // 
            this.btnRtspDetect.Location = new System.Drawing.Point(601, 301);
            this.btnRtspDetect.Name = "btnRtspDetect";
            this.btnRtspDetect.Size = new System.Drawing.Size(150, 51);
            this.btnRtspDetect.TabIndex = 21;
            this.btnRtspDetect.Text = "RTSP 检测";
            this.btnRtspDetect.UseVisualStyleBackColor = true;
            this.btnRtspDetect.Click += new System.EventHandler(this.BtnRtspDetect_Click);
            // 
            // btnRtspLiveDetect
            // 
            this.btnRtspLiveDetect.Location = new System.Drawing.Point(601, 360);
            this.btnRtspLiveDetect.Name = "btnRtspLiveDetect";
            this.btnRtspLiveDetect.Size = new System.Drawing.Size(150, 51);
            this.btnRtspLiveDetect.TabIndex = 22;
            this.btnRtspLiveDetect.Text = "RTSP live";
            this.btnRtspLiveDetect.UseVisualStyleBackColor = true;
            this.btnRtspLiveDetect.Click += new System.EventHandler(this.BtnRtspLiveDetect_Click);
            // 
            // txtRtspPath
            // 
            this.txtRtspPath.Location = new System.Drawing.Point(168, 314);
            this.txtRtspPath.Name = "txtRtspPath";
            this.txtRtspPath.Size = new System.Drawing.Size(333, 25);
            this.txtRtspPath.TabIndex = 23;
            this.txtRtspPath.Text = "rtsp://zephyr.rtsp.stream/movie?streamKey=504e8fc8f56843d79fe691271f603daf";
            // 
            // videoView1
            // 
            this.videoView1.BackColor = System.Drawing.Color.Black;
            this.videoView1.Location = new System.Drawing.Point(12, 359);
            this.videoView1.MediaPlayer = null;
            this.videoView1.Name = "videoView1";
            this.videoView1.Size = new System.Drawing.Size(150, 51);
            this.videoView1.TabIndex = 24;
            this.videoView1.Text = "videoView1";
            this.videoView1.Visible = false;
            // 
            // btnRtspConnect
            // 
            this.btnRtspConnect.Location = new System.Drawing.Point(12, 301);
            this.btnRtspConnect.Name = "btnRtspConnect";
            this.btnRtspConnect.Size = new System.Drawing.Size(150, 50);
            this.btnRtspConnect.TabIndex = 25;
            this.btnRtspConnect.Text = "连接 RTSP";
            this.btnRtspConnect.UseVisualStyleBackColor = true;
            this.btnRtspConnect.Click += new System.EventHandler(this.BtnRtspConnect_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(538, 143);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 15);
            this.label4.TabIndex = 27;
            this.label4.Text = "任务间隔";
            // 
            // numericUpDownDelayTime
            // 
            this.numericUpDownDelayTime.Location = new System.Drawing.Point(631, 138);
            this.numericUpDownDelayTime.Name = "numericUpDownDelayTime";
            this.numericUpDownDelayTime.Size = new System.Drawing.Size(120, 25);
            this.numericUpDownDelayTime.TabIndex = 26;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1813, 601);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numericUpDownDelayTime);
            this.Controls.Add(this.btnRtspConnect);
            this.Controls.Add(this.videoView1);
            this.Controls.Add(this.txtRtspPath);
            this.Controls.Add(this.btnRtspLiveDetect);
            this.Controls.Add(this.btnRtspDetect);
            this.Controls.Add(this.txtImagePath);
            this.Controls.Add(this.txtFolderPath);
            this.Controls.Add(this.BtnBatchDetect);
            this.Controls.Add(this.BtnSelectFolder);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numericUpDownNMS);
            this.Controls.Add(this.txtDetectionResults);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numericUpDownFontSize);
            this.Controls.Add(this.txtClassNamesPath);
            this.Controls.Add(this.txtWeightsPath);
            this.Controls.Add(this.txtConfigPath);
            this.Controls.Add(this.BtnLoadClassNames);
            this.Controls.Add(this.BtnLoadWeights);
            this.Controls.Add(this.BtnLoadConfig);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDownConfidence);
            this.Controls.Add(this.btnDetect);
            this.Controls.Add(this.btnSelectImage);
            this.Controls.Add(this.pictureBox);
            this.Name = "MainForm";
            this.Text = "VisionInspection";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownConfidence)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFontSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNMS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.videoView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDelayTime)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button btnSelectImage;
        private System.Windows.Forms.Button btnDetect;
        private System.Windows.Forms.NumericUpDown numericUpDownConfidence;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnLoadConfig;
        private System.Windows.Forms.Button BtnLoadWeights;
        private System.Windows.Forms.Button BtnLoadClassNames;
        private System.Windows.Forms.TextBox txtConfigPath;
        private System.Windows.Forms.TextBox txtWeightsPath;
        private System.Windows.Forms.TextBox txtClassNamesPath;
        private System.Windows.Forms.NumericUpDown numericUpDownFontSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDetectionResults;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownNMS;
        private System.Windows.Forms.Button BtnSelectFolder;
        private System.Windows.Forms.Button BtnBatchDetect;
        private System.Windows.Forms.TextBox txtFolderPath;
        private System.Windows.Forms.TextBox txtImagePath;
        private System.Windows.Forms.Button btnRtspDetect;
        private System.Windows.Forms.Button btnRtspLiveDetect;
        private System.Windows.Forms.TextBox txtRtspPath;
        private LibVLCSharp.WinForms.VideoView videoView1;
        private System.Windows.Forms.Button btnRtspConnect;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDownDelayTime;
    }
}

