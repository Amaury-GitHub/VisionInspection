﻿namespace YOLODetectionApp
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
            this.btnRtsp = new System.Windows.Forms.Button();
            this.btnRtspLive = new System.Windows.Forms.Button();
            this.txtRtspPath = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownConfidence)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFontSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNMS)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(800, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(1024, 768);
            this.pictureBox.TabIndex = 1;
            this.pictureBox.TabStop = false;
            // 
            // btnSelectImage
            // 
            this.btnSelectImage.Location = new System.Drawing.Point(12, 235);
            this.btnSelectImage.Name = "btnSelectImage";
            this.btnSelectImage.Size = new System.Drawing.Size(150, 50);
            this.btnSelectImage.TabIndex = 2;
            this.btnSelectImage.Text = "选择图像";
            this.btnSelectImage.UseVisualStyleBackColor = true;
            this.btnSelectImage.Click += new System.EventHandler(this.BtnSelectImage_Click);
            // 
            // btnDetect
            // 
            this.btnDetect.Location = new System.Drawing.Point(601, 236);
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
            this.BtnLoadWeights.Location = new System.Drawing.Point(12, 67);
            this.BtnLoadWeights.Name = "BtnLoadWeights";
            this.BtnLoadWeights.Size = new System.Drawing.Size(150, 50);
            this.BtnLoadWeights.TabIndex = 7;
            this.BtnLoadWeights.Text = "选择权重文件";
            this.BtnLoadWeights.UseVisualStyleBackColor = true;
            this.BtnLoadWeights.Click += new System.EventHandler(this.BtnLoadWeights_Click);
            // 
            // BtnLoadClassNames
            // 
            this.BtnLoadClassNames.Location = new System.Drawing.Point(12, 123);
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
            this.txtWeightsPath.Location = new System.Drawing.Point(168, 80);
            this.txtWeightsPath.Name = "txtWeightsPath";
            this.txtWeightsPath.Size = new System.Drawing.Size(333, 25);
            this.txtWeightsPath.TabIndex = 10;
            // 
            // txtClassNamesPath
            // 
            this.txtClassNamesPath.Location = new System.Drawing.Point(168, 136);
            this.txtClassNamesPath.Name = "txtClassNamesPath";
            this.txtClassNamesPath.Size = new System.Drawing.Size(333, 25);
            this.txtClassNamesPath.TabIndex = 11;
            // 
            // numericUpDownFontSize
            // 
            this.numericUpDownFontSize.Location = new System.Drawing.Point(631, 80);
            this.numericUpDownFontSize.Name = "numericUpDownFontSize";
            this.numericUpDownFontSize.Size = new System.Drawing.Size(120, 25);
            this.numericUpDownFontSize.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(538, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 13;
            this.label2.Text = "文字大小";
            // 
            // txtDetectionResults
            // 
            this.txtDetectionResults.Location = new System.Drawing.Point(0, 421);
            this.txtDetectionResults.Multiline = true;
            this.txtDetectionResults.Name = "txtDetectionResults";
            this.txtDetectionResults.ReadOnly = true;
            this.txtDetectionResults.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDetectionResults.Size = new System.Drawing.Size(794, 347);
            this.txtDetectionResults.TabIndex = 14;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(574, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 15);
            this.label3.TabIndex = 16;
            this.label3.Text = "NMS";
            // 
            // numericUpDownNMS
            // 
            this.numericUpDownNMS.Location = new System.Drawing.Point(631, 52);
            this.numericUpDownNMS.Name = "numericUpDownNMS";
            this.numericUpDownNMS.Size = new System.Drawing.Size(120, 25);
            this.numericUpDownNMS.TabIndex = 15;
            this.numericUpDownNMS.Tag = "";
            // 
            // BtnSelectFolder
            // 
            this.BtnSelectFolder.Location = new System.Drawing.Point(12, 179);
            this.BtnSelectFolder.Name = "BtnSelectFolder";
            this.BtnSelectFolder.Size = new System.Drawing.Size(150, 50);
            this.BtnSelectFolder.TabIndex = 17;
            this.BtnSelectFolder.Text = "选择文件夹";
            this.BtnSelectFolder.UseVisualStyleBackColor = true;
            this.BtnSelectFolder.Click += new System.EventHandler(this.BtnSelectFolder_Click);
            // 
            // BtnBatchDetect
            // 
            this.BtnBatchDetect.Location = new System.Drawing.Point(601, 180);
            this.BtnBatchDetect.Name = "BtnBatchDetect";
            this.BtnBatchDetect.Size = new System.Drawing.Size(150, 50);
            this.BtnBatchDetect.TabIndex = 18;
            this.BtnBatchDetect.Text = "批量检测";
            this.BtnBatchDetect.UseVisualStyleBackColor = true;
            this.BtnBatchDetect.Click += new System.EventHandler(this.BtnBatchDetect_Click);
            // 
            // txtFolderPath
            // 
            this.txtFolderPath.Location = new System.Drawing.Point(168, 192);
            this.txtFolderPath.Name = "txtFolderPath";
            this.txtFolderPath.Size = new System.Drawing.Size(333, 25);
            this.txtFolderPath.TabIndex = 19;
            // 
            // txtImagePath
            // 
            this.txtImagePath.Location = new System.Drawing.Point(168, 248);
            this.txtImagePath.Name = "txtImagePath";
            this.txtImagePath.Size = new System.Drawing.Size(333, 25);
            this.txtImagePath.TabIndex = 20;
            // 
            // btnRtsp
            // 
            this.btnRtsp.Location = new System.Drawing.Point(12, 359);
            this.btnRtsp.Name = "btnRtsp";
            this.btnRtsp.Size = new System.Drawing.Size(150, 51);
            this.btnRtsp.TabIndex = 21;
            this.btnRtsp.Text = "RTSP";
            this.btnRtsp.UseVisualStyleBackColor = true;
            this.btnRtsp.Click += new System.EventHandler(this.BtnRtspDetect_Click);
            // 
            // btnRtspLive
            // 
            this.btnRtspLive.Location = new System.Drawing.Point(351, 359);
            this.btnRtspLive.Name = "btnRtspLive";
            this.btnRtspLive.Size = new System.Drawing.Size(150, 51);
            this.btnRtspLive.TabIndex = 22;
            this.btnRtspLive.Text = "RTSP live";
            this.btnRtspLive.UseVisualStyleBackColor = true;
            this.btnRtspLive.Click += new System.EventHandler(this.BtnRtspLiveDetect_Click);
            // 
            // txtRtspPath
            // 
            this.txtRtspPath.Location = new System.Drawing.Point(12, 316);
            this.txtRtspPath.Name = "txtRtspPath";
            this.txtRtspPath.Size = new System.Drawing.Size(489, 25);
            this.txtRtspPath.TabIndex = 23;
            this.txtRtspPath.Text = "rtsp://admin:1qaz!QAZ@192.168.1.108:554/cam/realmonitor?channel=1&subtype=0\n";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1836, 791);
            this.Controls.Add(this.txtRtspPath);
            this.Controls.Add(this.btnRtspLive);
            this.Controls.Add(this.btnRtsp);
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
        private System.Windows.Forms.Button btnRtsp;
        private System.Windows.Forms.Button btnRtspLive;
        private System.Windows.Forms.TextBox txtRtspPath;
    }
}

