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
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.btnSelectImage = new System.Windows.Forms.Button();
            this.btnDetect = new System.Windows.Forms.Button();
            this.numericUpDownConfidence = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.btnLoadConfig = new System.Windows.Forms.Button();
            this.btnLoadWeights = new System.Windows.Forms.Button();
            this.btnLoadClassNames = new System.Windows.Forms.Button();
            this.txtConfigPath = new System.Windows.Forms.TextBox();
            this.txtWeightsPath = new System.Windows.Forms.TextBox();
            this.txtClassNamesPath = new System.Windows.Forms.TextBox();
            this.numericUpDownFontSize = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDetectionResults = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownNMS = new System.Windows.Forms.NumericUpDown();
            this.btnSelectFolder = new System.Windows.Forms.Button();
            this.btnBatchDetect = new System.Windows.Forms.Button();
            this.txtFolderPath = new System.Windows.Forms.TextBox();
            this.txtImagePath = new System.Windows.Forms.TextBox();
            this.btnStreamDetect = new System.Windows.Forms.Button();
            this.btnStreamLiveDetect = new System.Windows.Forms.Button();
            this.txtStreamPath = new System.Windows.Forms.TextBox();
            this.videoView1 = new LibVLCSharp.WinForms.VideoView();
            this.btnStreamConnect = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDownDelayTime = new System.Windows.Forms.NumericUpDown();
            this.enableOPENCL = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.numericUpDownImageSize = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownConfidence)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFontSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNMS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.videoView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDelayTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownImageSize)).BeginInit();
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
            this.btnSelectImage.Font = new System.Drawing.Font("Consolas", 9F);
            this.btnSelectImage.Location = new System.Drawing.Point(12, 243);
            this.btnSelectImage.Name = "btnSelectImage";
            this.btnSelectImage.Size = new System.Drawing.Size(150, 52);
            this.btnSelectImage.TabIndex = 2;
            this.btnSelectImage.Text = "选择图像";
            this.btnSelectImage.UseVisualStyleBackColor = true;
            this.btnSelectImage.Click += new System.EventHandler(this.BtnSelectImage_Click);
            // 
            // btnDetect
            // 
            this.btnDetect.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDetect.Location = new System.Drawing.Point(601, 243);
            this.btnDetect.Name = "btnDetect";
            this.btnDetect.Size = new System.Drawing.Size(150, 52);
            this.btnDetect.TabIndex = 3;
            this.btnDetect.Text = "单次检测";
            this.btnDetect.UseVisualStyleBackColor = true;
            this.btnDetect.Click += new System.EventHandler(this.BtnDetect_Click);
            // 
            // numericUpDownConfidence
            // 
            this.numericUpDownConfidence.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownConfidence.Location = new System.Drawing.Point(631, 25);
            this.numericUpDownConfidence.Name = "numericUpDownConfidence";
            this.numericUpDownConfidence.Size = new System.Drawing.Size(120, 25);
            this.numericUpDownConfidence.TabIndex = 4;
            this.numericUpDownConfidence.Tag = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Consolas", 9F);
            this.label1.Location = new System.Drawing.Point(547, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 18);
            this.label1.TabIndex = 5;
            this.label1.Text = "置信度";
            // 
            // btnLoadConfig
            // 
            this.btnLoadConfig.Font = new System.Drawing.Font("Consolas", 9F);
            this.btnLoadConfig.Location = new System.Drawing.Point(12, 11);
            this.btnLoadConfig.Name = "btnLoadConfig";
            this.btnLoadConfig.Size = new System.Drawing.Size(150, 52);
            this.btnLoadConfig.TabIndex = 6;
            this.btnLoadConfig.Text = "选择配置文件";
            this.btnLoadConfig.UseVisualStyleBackColor = true;
            this.btnLoadConfig.Click += new System.EventHandler(this.BtnLoadConfig_Click);
            // 
            // btnLoadWeights
            // 
            this.btnLoadWeights.Font = new System.Drawing.Font("Consolas", 9F);
            this.btnLoadWeights.Location = new System.Drawing.Point(12, 69);
            this.btnLoadWeights.Name = "btnLoadWeights";
            this.btnLoadWeights.Size = new System.Drawing.Size(150, 52);
            this.btnLoadWeights.TabIndex = 7;
            this.btnLoadWeights.Text = "选择权重文件";
            this.btnLoadWeights.UseVisualStyleBackColor = true;
            this.btnLoadWeights.Click += new System.EventHandler(this.BtnLoadWeights_Click);
            // 
            // btnLoadClassNames
            // 
            this.btnLoadClassNames.Font = new System.Drawing.Font("Consolas", 9F);
            this.btnLoadClassNames.Location = new System.Drawing.Point(12, 127);
            this.btnLoadClassNames.Name = "btnLoadClassNames";
            this.btnLoadClassNames.Size = new System.Drawing.Size(150, 52);
            this.btnLoadClassNames.TabIndex = 8;
            this.btnLoadClassNames.Text = "选择类别文件";
            this.btnLoadClassNames.UseVisualStyleBackColor = true;
            this.btnLoadClassNames.Click += new System.EventHandler(this.BtnLoadClassNames_Click);
            // 
            // txtConfigPath
            // 
            this.txtConfigPath.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConfigPath.Location = new System.Drawing.Point(168, 25);
            this.txtConfigPath.Name = "txtConfigPath";
            this.txtConfigPath.Size = new System.Drawing.Size(333, 25);
            this.txtConfigPath.TabIndex = 9;
            // 
            // txtWeightsPath
            // 
            this.txtWeightsPath.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWeightsPath.Location = new System.Drawing.Point(168, 82);
            this.txtWeightsPath.Name = "txtWeightsPath";
            this.txtWeightsPath.Size = new System.Drawing.Size(333, 25);
            this.txtWeightsPath.TabIndex = 10;
            // 
            // txtClassNamesPath
            // 
            this.txtClassNamesPath.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtClassNamesPath.Location = new System.Drawing.Point(168, 141);
            this.txtClassNamesPath.Name = "txtClassNamesPath";
            this.txtClassNamesPath.Size = new System.Drawing.Size(333, 25);
            this.txtClassNamesPath.TabIndex = 11;
            // 
            // numericUpDownFontSize
            // 
            this.numericUpDownFontSize.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownFontSize.Location = new System.Drawing.Point(631, 101);
            this.numericUpDownFontSize.Name = "numericUpDownFontSize";
            this.numericUpDownFontSize.Size = new System.Drawing.Size(120, 25);
            this.numericUpDownFontSize.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Consolas", 9F);
            this.label2.Location = new System.Drawing.Point(531, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 18);
            this.label2.TabIndex = 13;
            this.label2.Text = "文字大小";
            // 
            // txtDetectionResults
            // 
            this.txtDetectionResults.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.label3.Font = new System.Drawing.Font("Consolas", 9F);
            this.label3.Location = new System.Drawing.Point(571, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 18);
            this.label3.TabIndex = 16;
            this.label3.Text = "NMS";
            // 
            // numericUpDownNMS
            // 
            this.numericUpDownNMS.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownNMS.Location = new System.Drawing.Point(631, 63);
            this.numericUpDownNMS.Name = "numericUpDownNMS";
            this.numericUpDownNMS.Size = new System.Drawing.Size(120, 25);
            this.numericUpDownNMS.TabIndex = 15;
            this.numericUpDownNMS.Tag = "";
            // 
            // btnSelectFolder
            // 
            this.btnSelectFolder.Font = new System.Drawing.Font("Consolas", 9F);
            this.btnSelectFolder.Location = new System.Drawing.Point(12, 185);
            this.btnSelectFolder.Name = "btnSelectFolder";
            this.btnSelectFolder.Size = new System.Drawing.Size(150, 52);
            this.btnSelectFolder.TabIndex = 17;
            this.btnSelectFolder.Text = "选择文件夹";
            this.btnSelectFolder.UseVisualStyleBackColor = true;
            this.btnSelectFolder.Click += new System.EventHandler(this.BtnSelectFolder_Click);
            // 
            // btnBatchDetect
            // 
            this.btnBatchDetect.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBatchDetect.Location = new System.Drawing.Point(601, 185);
            this.btnBatchDetect.Name = "btnBatchDetect";
            this.btnBatchDetect.Size = new System.Drawing.Size(150, 52);
            this.btnBatchDetect.TabIndex = 18;
            this.btnBatchDetect.Text = "批量检测";
            this.btnBatchDetect.UseVisualStyleBackColor = true;
            this.btnBatchDetect.Click += new System.EventHandler(this.BtnBatchDetect_Click);
            // 
            // txtFolderPath
            // 
            this.txtFolderPath.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFolderPath.Location = new System.Drawing.Point(168, 199);
            this.txtFolderPath.Name = "txtFolderPath";
            this.txtFolderPath.Size = new System.Drawing.Size(333, 25);
            this.txtFolderPath.TabIndex = 19;
            // 
            // txtImagePath
            // 
            this.txtImagePath.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtImagePath.Location = new System.Drawing.Point(168, 257);
            this.txtImagePath.Name = "txtImagePath";
            this.txtImagePath.Size = new System.Drawing.Size(333, 25);
            this.txtImagePath.TabIndex = 20;
            // 
            // btnStreamDetect
            // 
            this.btnStreamDetect.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStreamDetect.Location = new System.Drawing.Point(601, 301);
            this.btnStreamDetect.Name = "btnStreamDetect";
            this.btnStreamDetect.Size = new System.Drawing.Size(150, 52);
            this.btnStreamDetect.TabIndex = 21;
            this.btnStreamDetect.Text = "Stream\r\n单次检测";
            this.btnStreamDetect.UseVisualStyleBackColor = true;
            this.btnStreamDetect.Click += new System.EventHandler(this.BtnStreamDetect_Click);
            // 
            // btnStreamLiveDetect
            // 
            this.btnStreamLiveDetect.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStreamLiveDetect.Location = new System.Drawing.Point(601, 359);
            this.btnStreamLiveDetect.Name = "btnStreamLiveDetect";
            this.btnStreamLiveDetect.Size = new System.Drawing.Size(150, 52);
            this.btnStreamLiveDetect.TabIndex = 22;
            this.btnStreamLiveDetect.Text = "Stream\r\n连续检测";
            this.btnStreamLiveDetect.UseVisualStyleBackColor = true;
            this.btnStreamLiveDetect.Click += new System.EventHandler(this.BtnStreamLiveDetect_Click);
            // 
            // txtStreamPath
            // 
            this.txtStreamPath.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStreamPath.Location = new System.Drawing.Point(168, 315);
            this.txtStreamPath.Name = "txtStreamPath";
            this.txtStreamPath.Size = new System.Drawing.Size(333, 25);
            this.txtStreamPath.TabIndex = 23;
            this.txtStreamPath.Text = "rtsp://admin:1qaz!QAZ@192.168.1.108:554";
            // 
            // videoView1
            // 
            this.videoView1.BackColor = System.Drawing.Color.Black;
            this.videoView1.Font = new System.Drawing.Font("Consolas", 9F);
            this.videoView1.Location = new System.Drawing.Point(12, 359);
            this.videoView1.MediaPlayer = null;
            this.videoView1.Name = "videoView1";
            this.videoView1.Size = new System.Drawing.Size(150, 52);
            this.videoView1.TabIndex = 24;
            this.videoView1.Text = "videoView1";
            this.videoView1.Visible = false;
            // 
            // btnStreamConnect
            // 
            this.btnStreamConnect.Font = new System.Drawing.Font("Consolas", 9F);
            this.btnStreamConnect.Location = new System.Drawing.Point(12, 301);
            this.btnStreamConnect.Name = "btnStreamConnect";
            this.btnStreamConnect.Size = new System.Drawing.Size(150, 52);
            this.btnStreamConnect.TabIndex = 25;
            this.btnStreamConnect.Text = "连接 Stream";
            this.btnStreamConnect.UseVisualStyleBackColor = true;
            this.btnStreamConnect.Click += new System.EventHandler(this.BtnStreamConnect_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Consolas", 9F);
            this.label4.Location = new System.Drawing.Point(531, 143);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 18);
            this.label4.TabIndex = 27;
            this.label4.Text = "任务间隔";
            // 
            // numericUpDownDelayTime
            // 
            this.numericUpDownDelayTime.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownDelayTime.Location = new System.Drawing.Point(630, 141);
            this.numericUpDownDelayTime.Name = "numericUpDownDelayTime";
            this.numericUpDownDelayTime.Size = new System.Drawing.Size(120, 25);
            this.numericUpDownDelayTime.TabIndex = 26;
            // 
            // enableOPENCL
            // 
            this.enableOPENCL.AutoSize = true;
            this.enableOPENCL.Font = new System.Drawing.Font("Consolas", 9F);
            this.enableOPENCL.Location = new System.Drawing.Point(187, 373);
            this.enableOPENCL.Name = "enableOPENCL";
            this.enableOPENCL.Size = new System.Drawing.Size(78, 22);
            this.enableOPENCL.TabIndex = 28;
            this.enableOPENCL.Text = "OPENCL";
            this.enableOPENCL.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Consolas", 9F);
            this.label5.Location = new System.Drawing.Point(302, 375);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 18);
            this.label5.TabIndex = 29;
            this.label5.Text = "分辨率";
            // 
            // numericUpDownImageSize
            // 
            this.numericUpDownImageSize.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownImageSize.Location = new System.Drawing.Point(381, 373);
            this.numericUpDownImageSize.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownImageSize.Name = "numericUpDownImageSize";
            this.numericUpDownImageSize.Size = new System.Drawing.Size(120, 25);
            this.numericUpDownImageSize.TabIndex = 30;
            this.numericUpDownImageSize.Tag = "";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1813, 601);
            this.Controls.Add(this.numericUpDownImageSize);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.enableOPENCL);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numericUpDownDelayTime);
            this.Controls.Add(this.btnStreamConnect);
            this.Controls.Add(this.videoView1);
            this.Controls.Add(this.txtStreamPath);
            this.Controls.Add(this.btnStreamLiveDetect);
            this.Controls.Add(this.btnStreamDetect);
            this.Controls.Add(this.txtImagePath);
            this.Controls.Add(this.txtFolderPath);
            this.Controls.Add(this.btnBatchDetect);
            this.Controls.Add(this.btnSelectFolder);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numericUpDownNMS);
            this.Controls.Add(this.txtDetectionResults);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numericUpDownFontSize);
            this.Controls.Add(this.txtClassNamesPath);
            this.Controls.Add(this.txtWeightsPath);
            this.Controls.Add(this.txtConfigPath);
            this.Controls.Add(this.btnLoadClassNames);
            this.Controls.Add(this.btnLoadWeights);
            this.Controls.Add(this.btnLoadConfig);
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
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownImageSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button btnSelectImage;
        private System.Windows.Forms.Button btnDetect;
        private System.Windows.Forms.NumericUpDown numericUpDownConfidence;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnLoadConfig;
        private System.Windows.Forms.Button btnLoadWeights;
        private System.Windows.Forms.Button btnLoadClassNames;
        private System.Windows.Forms.TextBox txtConfigPath;
        private System.Windows.Forms.TextBox txtWeightsPath;
        private System.Windows.Forms.TextBox txtClassNamesPath;
        private System.Windows.Forms.NumericUpDown numericUpDownFontSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDetectionResults;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownNMS;
        private System.Windows.Forms.Button btnSelectFolder;
        private System.Windows.Forms.Button btnBatchDetect;
        private System.Windows.Forms.TextBox txtFolderPath;
        private System.Windows.Forms.TextBox txtImagePath;
        private System.Windows.Forms.Button btnStreamDetect;
        private System.Windows.Forms.Button btnStreamLiveDetect;
        private System.Windows.Forms.TextBox txtStreamPath;
        private LibVLCSharp.WinForms.VideoView videoView1;
        private System.Windows.Forms.Button btnStreamConnect;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDownDelayTime;
        private System.Windows.Forms.CheckBox enableOPENCL;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numericUpDownImageSize;
    }
}

