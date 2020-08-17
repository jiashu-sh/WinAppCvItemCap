namespace WinAppCvItemCap01
{
    partial class FormCvItemCap
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
            this.btnStartDetection = new System.Windows.Forms.Button();
            this.cbShowPic = new System.Windows.Forms.CheckBox();
            this.cbShowMoving = new System.Windows.Forms.CheckBox();
            this.cbDetectOn = new System.Windows.Forms.CheckBox();
            this.tbMessage = new System.Windows.Forms.TextBox();
            this.btnCapture = new System.Windows.Forms.Button();
            this.btnOpenFile1 = new System.Windows.Forms.Button();
            this.tbImgFile1 = new System.Windows.Forms.TextBox();
            this.tbImgFile2 = new System.Windows.Forms.TextBox();
            this.btnOpenFile2 = new System.Windows.Forms.Button();
            this.btnSaveText = new System.Windows.Forms.Button();
            this.btnCountBlack = new System.Windows.Forms.Button();
            this.tbResult = new System.Windows.Forms.TextBox();
            this.btnCompare = new System.Windows.Forms.Button();
            this.pbImg2 = new System.Windows.Forms.PictureBox();
            this.pbImg1 = new System.Windows.Forms.PictureBox();
            this.pbCameraView = new System.Windows.Forms.PictureBox();
            this.tbDmCodes = new System.Windows.Forms.TextBox();
            this.cbStrartDecode = new System.Windows.Forms.CheckBox();
            this.cbDetectLines = new System.Windows.Forms.CheckBox();
            this.cbCheckUnCheckAll = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbImg2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbImg1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCameraView)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStartDetection
            // 
            this.btnStartDetection.Location = new System.Drawing.Point(668, 12);
            this.btnStartDetection.Name = "btnStartDetection";
            this.btnStartDetection.Size = new System.Drawing.Size(204, 31);
            this.btnStartDetection.TabIndex = 1;
            this.btnStartDetection.Text = "Start Detection";
            this.btnStartDetection.UseVisualStyleBackColor = true;
            this.btnStartDetection.Click += new System.EventHandler(this.btnStartDetection_Click);
            // 
            // cbShowPic
            // 
            this.cbShowPic.AutoSize = true;
            this.cbShowPic.Location = new System.Drawing.Point(790, 54);
            this.cbShowPic.Name = "cbShowPic";
            this.cbShowPic.Size = new System.Drawing.Size(78, 21);
            this.cbShowPic.TabIndex = 8;
            this.cbShowPic.Text = "Show Pic";
            this.cbShowPic.UseVisualStyleBackColor = true;
            this.cbShowPic.CheckedChanged += new System.EventHandler(this.cbShowPic_CheckedChanged);
            // 
            // cbShowMoving
            // 
            this.cbShowMoving.AutoSize = true;
            this.cbShowMoving.Location = new System.Drawing.Point(668, 81);
            this.cbShowMoving.Name = "cbShowMoving";
            this.cbShowMoving.Size = new System.Drawing.Size(106, 21);
            this.cbShowMoving.TabIndex = 7;
            this.cbShowMoving.Text = "Show Moving";
            this.cbShowMoving.UseVisualStyleBackColor = true;
            this.cbShowMoving.CheckedChanged += new System.EventHandler(this.cbShowMoving_CheckedChanged);
            // 
            // cbDetectOn
            // 
            this.cbDetectOn.AutoSize = true;
            this.cbDetectOn.Location = new System.Drawing.Point(668, 54);
            this.cbDetectOn.Name = "cbDetectOn";
            this.cbDetectOn.Size = new System.Drawing.Size(85, 21);
            this.cbDetectOn.TabIndex = 6;
            this.cbDetectOn.Text = "Detect On";
            this.cbDetectOn.UseVisualStyleBackColor = true;
            this.cbDetectOn.CheckedChanged += new System.EventHandler(this.cbDetectOn_CheckedChanged);
            // 
            // tbMessage
            // 
            this.tbMessage.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbMessage.Location = new System.Drawing.Point(12, 499);
            this.tbMessage.Multiline = true;
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.Size = new System.Drawing.Size(560, 50);
            this.tbMessage.TabIndex = 9;
            // 
            // btnCapture
            // 
            this.btnCapture.Location = new System.Drawing.Point(578, 499);
            this.btnCapture.Name = "btnCapture";
            this.btnCapture.Size = new System.Drawing.Size(74, 50);
            this.btnCapture.TabIndex = 10;
            this.btnCapture.Text = "Capture";
            this.btnCapture.UseVisualStyleBackColor = true;
            this.btnCapture.Click += new System.EventHandler(this.btnCapture_Click);
            // 
            // btnOpenFile1
            // 
            this.btnOpenFile1.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOpenFile1.Location = new System.Drawing.Point(668, 137);
            this.btnOpenFile1.Name = "btnOpenFile1";
            this.btnOpenFile1.Size = new System.Drawing.Size(100, 23);
            this.btnOpenFile1.TabIndex = 11;
            this.btnOpenFile1.Text = "Open Img 1";
            this.btnOpenFile1.UseVisualStyleBackColor = true;
            this.btnOpenFile1.Click += new System.EventHandler(this.btnOpenFile1_Click);
            // 
            // tbImgFile1
            // 
            this.tbImgFile1.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbImgFile1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tbImgFile1.Location = new System.Drawing.Point(668, 272);
            this.tbImgFile1.Multiline = true;
            this.tbImgFile1.Name = "tbImgFile1";
            this.tbImgFile1.Size = new System.Drawing.Size(204, 35);
            this.tbImgFile1.TabIndex = 12;
            this.tbImgFile1.Text = "file 1 path";
            // 
            // tbImgFile2
            // 
            this.tbImgFile2.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbImgFile2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tbImgFile2.Location = new System.Drawing.Point(668, 313);
            this.tbImgFile2.Multiline = true;
            this.tbImgFile2.Name = "tbImgFile2";
            this.tbImgFile2.Size = new System.Drawing.Size(204, 36);
            this.tbImgFile2.TabIndex = 14;
            this.tbImgFile2.Text = "file 2 path";
            // 
            // btnOpenFile2
            // 
            this.btnOpenFile2.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOpenFile2.Location = new System.Drawing.Point(772, 137);
            this.btnOpenFile2.Name = "btnOpenFile2";
            this.btnOpenFile2.Size = new System.Drawing.Size(100, 23);
            this.btnOpenFile2.TabIndex = 13;
            this.btnOpenFile2.Text = "Open Img 2";
            this.btnOpenFile2.UseVisualStyleBackColor = true;
            this.btnOpenFile2.Click += new System.EventHandler(this.btnOpenFile2_Click);
            // 
            // btnSaveText
            // 
            this.btnSaveText.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSaveText.Location = new System.Drawing.Point(668, 355);
            this.btnSaveText.Name = "btnSaveText";
            this.btnSaveText.Size = new System.Drawing.Size(100, 23);
            this.btnSaveText.TabIndex = 13;
            this.btnSaveText.Text = "Save Text （file1)";
            this.btnSaveText.UseVisualStyleBackColor = true;
            this.btnSaveText.Click += new System.EventHandler(this.btnSaveText_Click);
            // 
            // btnCountBlack
            // 
            this.btnCountBlack.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCountBlack.Location = new System.Drawing.Point(668, 469);
            this.btnCountBlack.Name = "btnCountBlack";
            this.btnCountBlack.Size = new System.Drawing.Size(100, 23);
            this.btnCountBlack.TabIndex = 13;
            this.btnCountBlack.Text = "Count Is Black";
            this.btnCountBlack.UseVisualStyleBackColor = true;
            this.btnCountBlack.Click += new System.EventHandler(this.btnCountBlack_Click);
            // 
            // tbResult
            // 
            this.tbResult.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbResult.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tbResult.Location = new System.Drawing.Point(668, 498);
            this.tbResult.Multiline = true;
            this.tbResult.Name = "tbResult";
            this.tbResult.Size = new System.Drawing.Size(204, 51);
            this.tbResult.TabIndex = 14;
            // 
            // btnCompare
            // 
            this.btnCompare.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCompare.Location = new System.Drawing.Point(772, 469);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(100, 23);
            this.btnCompare.TabIndex = 13;
            this.btnCompare.Text = "Compare";
            this.btnCompare.UseVisualStyleBackColor = true;
            this.btnCompare.Click += new System.EventHandler(this.btnCompare_Click);
            // 
            // pbImg2
            // 
            this.pbImg2.BackColor = System.Drawing.Color.White;
            this.pbImg2.Image = global::WinAppCvItemCap01.Properties.Resources.bg_2;
            this.pbImg2.Location = new System.Drawing.Point(772, 166);
            this.pbImg2.Name = "pbImg2";
            this.pbImg2.Size = new System.Drawing.Size(100, 100);
            this.pbImg2.TabIndex = 15;
            this.pbImg2.TabStop = false;
            // 
            // pbImg1
            // 
            this.pbImg1.BackColor = System.Drawing.Color.White;
            this.pbImg1.Image = global::WinAppCvItemCap01.Properties.Resources.bg_1;
            this.pbImg1.Location = new System.Drawing.Point(668, 166);
            this.pbImg1.Name = "pbImg1";
            this.pbImg1.Size = new System.Drawing.Size(100, 100);
            this.pbImg1.TabIndex = 15;
            this.pbImg1.TabStop = false;
            // 
            // pbCameraView
            // 
            this.pbCameraView.Location = new System.Drawing.Point(12, 12);
            this.pbCameraView.Name = "pbCameraView";
            this.pbCameraView.Size = new System.Drawing.Size(640, 480);
            this.pbCameraView.TabIndex = 0;
            this.pbCameraView.TabStop = false;
            this.pbCameraView.Paint += new System.Windows.Forms.PaintEventHandler(this.pbCameraView_Paint);
            // 
            // tbDmCodes
            // 
            this.tbDmCodes.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbDmCodes.ForeColor = System.Drawing.Color.Red;
            this.tbDmCodes.Location = new System.Drawing.Point(668, 384);
            this.tbDmCodes.Multiline = true;
            this.tbDmCodes.Name = "tbDmCodes";
            this.tbDmCodes.Size = new System.Drawing.Size(204, 79);
            this.tbDmCodes.TabIndex = 16;
            this.tbDmCodes.Text = "DataMatrix Codes ...";
            // 
            // cbStrartDecode
            // 
            this.cbStrartDecode.AutoSize = true;
            this.cbStrartDecode.ForeColor = System.Drawing.Color.Blue;
            this.cbStrartDecode.Location = new System.Drawing.Point(790, 81);
            this.cbStrartDecode.Name = "cbStrartDecode";
            this.cbStrartDecode.Size = new System.Drawing.Size(72, 21);
            this.cbStrartDecode.TabIndex = 8;
            this.cbStrartDecode.Text = "Decode";
            this.cbStrartDecode.UseVisualStyleBackColor = true;
            this.cbStrartDecode.CheckedChanged += new System.EventHandler(this.cbStrartDecode_CheckedChanged);
            // 
            // cbDetectLines
            // 
            this.cbDetectLines.AutoSize = true;
            this.cbDetectLines.Location = new System.Drawing.Point(668, 109);
            this.cbDetectLines.Name = "cbDetectLines";
            this.cbDetectLines.Size = new System.Drawing.Size(91, 21);
            this.cbDetectLines.TabIndex = 17;
            this.cbDetectLines.Text = "Detect Line";
            this.cbDetectLines.UseVisualStyleBackColor = true;
            this.cbDetectLines.CheckedChanged += new System.EventHandler(this.cbDetectLines_CheckedChanged);
            // 
            // cbCheckUnCheckAll
            // 
            this.cbCheckUnCheckAll.AutoSize = true;
            this.cbCheckUnCheckAll.ForeColor = System.Drawing.Color.Red;
            this.cbCheckUnCheckAll.Location = new System.Drawing.Point(790, 108);
            this.cbCheckUnCheckAll.Name = "cbCheckUnCheckAll";
            this.cbCheckUnCheckAll.Size = new System.Drawing.Size(88, 21);
            this.cbCheckUnCheckAll.TabIndex = 17;
            this.cbCheckUnCheckAll.Text = "Chk/Un All";
            this.cbCheckUnCheckAll.UseVisualStyleBackColor = true;
            this.cbCheckUnCheckAll.CheckedChanged += new System.EventHandler(this.cbCheckUnCheckAll_CheckedChanged);
            // 
            // FormCvItemCap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 561);
            this.Controls.Add(this.cbCheckUnCheckAll);
            this.Controls.Add(this.cbDetectLines);
            this.Controls.Add(this.tbDmCodes);
            this.Controls.Add(this.pbImg2);
            this.Controls.Add(this.pbImg1);
            this.Controls.Add(this.tbResult);
            this.Controls.Add(this.tbImgFile2);
            this.Controls.Add(this.btnSaveText);
            this.Controls.Add(this.btnCompare);
            this.Controls.Add(this.btnCountBlack);
            this.Controls.Add(this.btnOpenFile2);
            this.Controls.Add(this.tbImgFile1);
            this.Controls.Add(this.btnOpenFile1);
            this.Controls.Add(this.btnCapture);
            this.Controls.Add(this.tbMessage);
            this.Controls.Add(this.cbStrartDecode);
            this.Controls.Add(this.cbShowPic);
            this.Controls.Add(this.cbShowMoving);
            this.Controls.Add(this.cbDetectOn);
            this.Controls.Add(this.btnStartDetection);
            this.Controls.Add(this.pbCameraView);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormCvItemCap";
            this.Text = "FormCvItemCap";
            this.Load += new System.EventHandler(this.FormCvItemCap_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbImg2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbImg1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCameraView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbCameraView;
        private System.Windows.Forms.Button btnStartDetection;
        private System.Windows.Forms.CheckBox cbShowPic;
        private System.Windows.Forms.CheckBox cbShowMoving;
        private System.Windows.Forms.CheckBox cbDetectOn;
        private System.Windows.Forms.TextBox tbMessage;
        private System.Windows.Forms.Button btnCapture;
        private System.Windows.Forms.Button btnOpenFile1;
        private System.Windows.Forms.TextBox tbImgFile1;
        private System.Windows.Forms.TextBox tbImgFile2;
        private System.Windows.Forms.Button btnOpenFile2;
        private System.Windows.Forms.PictureBox pbImg1;
        private System.Windows.Forms.PictureBox pbImg2;
        private System.Windows.Forms.Button btnSaveText;
        private System.Windows.Forms.Button btnCountBlack;
        private System.Windows.Forms.TextBox tbResult;
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.TextBox tbDmCodes;
        private System.Windows.Forms.CheckBox cbStrartDecode;
        private System.Windows.Forms.CheckBox cbDetectLines;
        private System.Windows.Forms.CheckBox cbCheckUnCheckAll;
    }
}

