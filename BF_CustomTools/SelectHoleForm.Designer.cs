
namespace BF_CustomTools
{
    partial class SelectHoleForm
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
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.RadioBtnH5 = new System.Windows.Forms.RadioButton();
            this.RadioBtnH510S = new System.Windows.Forms.RadioButton();
            this.RadioBtnH510X = new System.Windows.Forms.RadioButton();
            this.BtnOK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::BF_CustomTools.Properties.Resources.H510X;
            this.pictureBox3.Location = new System.Drawing.Point(256, 17);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(100, 100);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 2;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::BF_CustomTools.Properties.Resources.H510S;
            this.pictureBox2.Location = new System.Drawing.Point(134, 17);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(100, 100);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::BF_CustomTools.Properties.Resources.H5;
            this.pictureBox1.Location = new System.Drawing.Point(9, 17);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 100);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.RadioBtnH510X);
            this.groupBox1.Controls.Add(this.RadioBtnH510S);
            this.groupBox1.Controls.Add(this.RadioBtnH5);
            this.groupBox1.Controls.Add(this.pictureBox2);
            this.groupBox1.Controls.Add(this.pictureBox3);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(366, 151);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "开孔方式";
            // 
            // RadioBtnH5
            // 
            this.RadioBtnH5.AutoSize = true;
            this.RadioBtnH5.Checked = true;
            this.RadioBtnH5.Location = new System.Drawing.Point(9, 123);
            this.RadioBtnH5.Name = "RadioBtnH5";
            this.RadioBtnH5.Size = new System.Drawing.Size(65, 16);
            this.RadioBtnH5.TabIndex = 3;
            this.RadioBtnH5.TabStop = true;
            this.RadioBtnH5.Text = "直径5mm";
            this.RadioBtnH5.UseVisualStyleBackColor = true;
            this.RadioBtnH5.CheckedChanged += new System.EventHandler(this.RadioBtnH5_CheckedChanged);
            // 
            // RadioBtnH510S
            // 
            this.RadioBtnH510S.AutoSize = true;
            this.RadioBtnH510S.Location = new System.Drawing.Point(134, 123);
            this.RadioBtnH510S.Name = "RadioBtnH510S";
            this.RadioBtnH510S.Size = new System.Drawing.Size(101, 16);
            this.RadioBtnH510S.TabIndex = 4;
            this.RadioBtnH510S.Text = "直径上10下5mm";
            this.RadioBtnH510S.UseVisualStyleBackColor = true;
            this.RadioBtnH510S.CheckedChanged += new System.EventHandler(this.RadioBtnH510S_CheckedChanged);
            // 
            // RadioBtnH510X
            // 
            this.RadioBtnH510X.AutoSize = true;
            this.RadioBtnH510X.Location = new System.Drawing.Point(256, 123);
            this.RadioBtnH510X.Name = "RadioBtnH510X";
            this.RadioBtnH510X.Size = new System.Drawing.Size(101, 16);
            this.RadioBtnH510X.TabIndex = 5;
            this.RadioBtnH510X.Text = "直径上5下10mm";
            this.RadioBtnH510X.UseVisualStyleBackColor = true;
            this.RadioBtnH510X.CheckedChanged += new System.EventHandler(this.RadioBtnH510X_CheckedChanged);
            // 
            // BtnOK
            // 
            this.BtnOK.Location = new System.Drawing.Point(303, 169);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(75, 23);
            this.BtnOK.TabIndex = 4;
            this.BtnOK.Text = "确定";
            this.BtnOK.UseVisualStyleBackColor = true;
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // SelectHoleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 201);
            this.Controls.Add(this.BtnOK);
            this.Controls.Add(this.groupBox1);
            this.Name = "SelectHoleForm";
            this.Text = "[百福展柜]-选择开孔方式";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton RadioBtnH510X;
        private System.Windows.Forms.RadioButton RadioBtnH510S;
        private System.Windows.Forms.RadioButton RadioBtnH5;
        private System.Windows.Forms.Button BtnOK;
    }
}