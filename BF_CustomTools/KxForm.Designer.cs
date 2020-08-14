namespace BF_CustomTools
{
    partial class KxForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KxForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ArearadioButton = new System.Windows.Forms.RadioButton();
            this.BlockradioButton = new System.Windows.Forms.RadioButton();
            this.DimradioButton = new System.Windows.Forms.RadioButton();
            this.HatchradioButton = new System.Windows.Forms.RadioButton();
            this.MtextradioButton = new System.Windows.Forms.RadioButton();
            this.TextradioButton = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Colorbtn = new System.Windows.Forms.Button();
            this.BlockcomboBox = new System.Windows.Forms.ComboBox();
            this.LayercomboBox = new System.Windows.Forms.ComboBox();
            this.BlocknamecheckBox = new System.Windows.Forms.CheckBox();
            this.ColorcheckBox = new System.Windows.Forms.CheckBox();
            this.LayercheckBox = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.OKbtn = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ArearadioButton);
            this.groupBox1.Controls.Add(this.BlockradioButton);
            this.groupBox1.Controls.Add(this.DimradioButton);
            this.groupBox1.Controls.Add(this.HatchradioButton);
            this.groupBox1.Controls.Add(this.MtextradioButton);
            this.groupBox1.Controls.Add(this.TextradioButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(112, 242);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选项";
            // 
            // ArearadioButton
            // 
            this.ArearadioButton.AutoSize = true;
            this.ArearadioButton.Location = new System.Drawing.Point(20, 205);
            this.ArearadioButton.Name = "ArearadioButton";
            this.ArearadioButton.Size = new System.Drawing.Size(71, 16);
            this.ArearadioButton.TabIndex = 5;
            this.ArearadioButton.Text = "相同面积";
            this.ArearadioButton.UseVisualStyleBackColor = true;
            // 
            // BlockradioButton
            // 
            this.BlockradioButton.AutoSize = true;
            this.BlockradioButton.Location = new System.Drawing.Point(20, 168);
            this.BlockradioButton.Name = "BlockradioButton";
            this.BlockradioButton.Size = new System.Drawing.Size(47, 16);
            this.BlockradioButton.TabIndex = 4;
            this.BlockradioButton.Text = "图块";
            this.BlockradioButton.UseVisualStyleBackColor = true;
            this.BlockradioButton.CheckedChanged += new System.EventHandler(this.BlockradioButton_CheckedChanged);
            // 
            // DimradioButton
            // 
            this.DimradioButton.AutoSize = true;
            this.DimradioButton.Checked = true;
            this.DimradioButton.Location = new System.Drawing.Point(20, 131);
            this.DimradioButton.Name = "DimradioButton";
            this.DimradioButton.Size = new System.Drawing.Size(47, 16);
            this.DimradioButton.TabIndex = 3;
            this.DimradioButton.TabStop = true;
            this.DimradioButton.Text = "标注";
            this.DimradioButton.UseVisualStyleBackColor = true;
            this.DimradioButton.CheckedChanged += new System.EventHandler(this.DimradioButton_CheckedChanged);
            // 
            // HatchradioButton
            // 
            this.HatchradioButton.AutoSize = true;
            this.HatchradioButton.Location = new System.Drawing.Point(20, 94);
            this.HatchradioButton.Name = "HatchradioButton";
            this.HatchradioButton.Size = new System.Drawing.Size(47, 16);
            this.HatchradioButton.TabIndex = 2;
            this.HatchradioButton.Text = "填充";
            this.HatchradioButton.UseVisualStyleBackColor = true;
            // 
            // MtextradioButton
            // 
            this.MtextradioButton.AutoSize = true;
            this.MtextradioButton.Location = new System.Drawing.Point(20, 57);
            this.MtextradioButton.Name = "MtextradioButton";
            this.MtextradioButton.Size = new System.Drawing.Size(71, 16);
            this.MtextradioButton.TabIndex = 1;
            this.MtextradioButton.Text = "多行文字";
            this.MtextradioButton.UseVisualStyleBackColor = true;
            // 
            // TextradioButton
            // 
            this.TextradioButton.AutoSize = true;
            this.TextradioButton.Location = new System.Drawing.Point(20, 20);
            this.TextradioButton.Name = "TextradioButton";
            this.TextradioButton.Size = new System.Drawing.Size(71, 16);
            this.TextradioButton.TabIndex = 0;
            this.TextradioButton.Text = "单行文字";
            this.TextradioButton.UseVisualStyleBackColor = true;
            this.TextradioButton.CheckedChanged += new System.EventHandler(this.TextradioButton_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Colorbtn);
            this.groupBox2.Controls.Add(this.BlockcomboBox);
            this.groupBox2.Controls.Add(this.LayercomboBox);
            this.groupBox2.Controls.Add(this.BlocknamecheckBox);
            this.groupBox2.Controls.Add(this.ColorcheckBox);
            this.groupBox2.Controls.Add(this.LayercheckBox);
            this.groupBox2.Location = new System.Drawing.Point(130, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(330, 126);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "过滤条件";
            // 
            // Colorbtn
            // 
            this.Colorbtn.BackColor = System.Drawing.Color.Black;
            this.Colorbtn.Location = new System.Drawing.Point(304, 55);
            this.Colorbtn.Name = "Colorbtn";
            this.Colorbtn.Size = new System.Drawing.Size(20, 17);
            this.Colorbtn.TabIndex = 5;
            this.Colorbtn.UseVisualStyleBackColor = false;
            this.Colorbtn.Click += new System.EventHandler(this.Colorbtn_Click);
            // 
            // BlockcomboBox
            // 
            this.BlockcomboBox.Enabled = false;
            this.BlockcomboBox.FormattingEnabled = true;
            this.BlockcomboBox.Location = new System.Drawing.Point(87, 90);
            this.BlockcomboBox.Name = "BlockcomboBox";
            this.BlockcomboBox.Size = new System.Drawing.Size(237, 20);
            this.BlockcomboBox.TabIndex = 4;
            // 
            // LayercomboBox
            // 
            this.LayercomboBox.Enabled = false;
            this.LayercomboBox.FormattingEnabled = true;
            this.LayercomboBox.Location = new System.Drawing.Point(87, 19);
            this.LayercomboBox.Name = "LayercomboBox";
            this.LayercomboBox.Size = new System.Drawing.Size(237, 20);
            this.LayercomboBox.TabIndex = 3;
            this.LayercomboBox.SelectedIndexChanged += new System.EventHandler(this.LayercomboBox_SelectedIndexChanged);
            // 
            // BlocknamecheckBox
            // 
            this.BlocknamecheckBox.AutoSize = true;
            this.BlocknamecheckBox.Enabled = false;
            this.BlocknamecheckBox.Location = new System.Drawing.Point(20, 92);
            this.BlocknamecheckBox.Name = "BlocknamecheckBox";
            this.BlocknamecheckBox.Size = new System.Drawing.Size(60, 16);
            this.BlocknamecheckBox.TabIndex = 2;
            this.BlocknamecheckBox.Text = "块名 =";
            this.BlocknamecheckBox.UseVisualStyleBackColor = true;
            this.BlocknamecheckBox.CheckedChanged += new System.EventHandler(this.BlocknamecheckBox_CheckedChanged);
            // 
            // ColorcheckBox
            // 
            this.ColorcheckBox.AutoSize = true;
            this.ColorcheckBox.Location = new System.Drawing.Point(20, 56);
            this.ColorcheckBox.Name = "ColorcheckBox";
            this.ColorcheckBox.Size = new System.Drawing.Size(60, 16);
            this.ColorcheckBox.TabIndex = 1;
            this.ColorcheckBox.Text = "颜色 =";
            this.ColorcheckBox.UseVisualStyleBackColor = true;
            // 
            // LayercheckBox
            // 
            this.LayercheckBox.AutoSize = true;
            this.LayercheckBox.Location = new System.Drawing.Point(20, 20);
            this.LayercheckBox.Name = "LayercheckBox";
            this.LayercheckBox.Size = new System.Drawing.Size(60, 16);
            this.LayercheckBox.TabIndex = 0;
            this.LayercheckBox.Text = "图层 =";
            this.LayercheckBox.UseVisualStyleBackColor = true;
            this.LayercheckBox.CheckedChanged += new System.EventHandler(this.LayercheckBox_CheckedChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(130, 144);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(108, 108);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // OKbtn
            // 
            this.OKbtn.Location = new System.Drawing.Point(288, 231);
            this.OKbtn.Name = "OKbtn";
            this.OKbtn.Size = new System.Drawing.Size(75, 21);
            this.OKbtn.TabIndex = 3;
            this.OKbtn.Text = "确定";
            this.OKbtn.UseVisualStyleBackColor = true;
            this.OKbtn.Click += new System.EventHandler(this.OKbtn_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(385, 231);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 21);
            this.button2.TabIndex = 4;
            this.button2.Text = "取消";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Gray;
            this.label1.Location = new System.Drawing.Point(265, 145);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "制作人：醉月武";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Gray;
            this.label2.Location = new System.Drawing.Point(265, 170);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(143, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "交流群：325959696(QQ群)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Gray;
            this.label3.Location = new System.Drawing.Point(265, 195);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(137, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "<---：微信公众号二维码";
            // 
            // KxForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 261);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.OKbtn);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "KxForm";
            this.Text = "ADT-快速选择";
            this.Load += new System.EventHandler(this.KxForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton TextradioButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton ArearadioButton;
        private System.Windows.Forms.RadioButton BlockradioButton;
        private System.Windows.Forms.RadioButton DimradioButton;
        private System.Windows.Forms.RadioButton HatchradioButton;
        private System.Windows.Forms.RadioButton MtextradioButton;
        private System.Windows.Forms.ComboBox BlockcomboBox;
        private System.Windows.Forms.ComboBox LayercomboBox;
        private System.Windows.Forms.CheckBox BlocknamecheckBox;
        private System.Windows.Forms.CheckBox ColorcheckBox;
        private System.Windows.Forms.CheckBox LayercheckBox;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button OKbtn;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button Colorbtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}