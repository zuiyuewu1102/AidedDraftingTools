﻿
namespace BF_CustomTools
{
    partial class SetBasePlateThickness
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
            this.label1 = new System.Windows.Forms.Label();
            this.TxtCurThickness = new System.Windows.Forms.TextBox();
            this.Btn3 = new System.Windows.Forms.Button();
            this.Btn5 = new System.Windows.Forms.Button();
            this.Btn12 = new System.Windows.Forms.Button();
            this.Btn9 = new System.Windows.Forms.Button();
            this.Btn18 = new System.Windows.Forms.Button();
            this.Btn15 = new System.Windows.Forms.Button();
            this.BtnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "基板厚度：";
            // 
            // TxtCurThickness
            // 
            this.TxtCurThickness.Enabled = false;
            this.TxtCurThickness.Location = new System.Drawing.Point(83, 6);
            this.TxtCurThickness.Name = "TxtCurThickness";
            this.TxtCurThickness.Size = new System.Drawing.Size(65, 21);
            this.TxtCurThickness.TabIndex = 1;
            // 
            // Btn3
            // 
            this.Btn3.Location = new System.Drawing.Point(12, 33);
            this.Btn3.Name = "Btn3";
            this.Btn3.Size = new System.Drawing.Size(65, 23);
            this.Btn3.TabIndex = 2;
            this.Btn3.Text = "3厘板";
            this.Btn3.UseVisualStyleBackColor = true;
            this.Btn3.Click += new System.EventHandler(this.Btn3_Click);
            // 
            // Btn5
            // 
            this.Btn5.Location = new System.Drawing.Point(83, 33);
            this.Btn5.Name = "Btn5";
            this.Btn5.Size = new System.Drawing.Size(65, 23);
            this.Btn5.TabIndex = 3;
            this.Btn5.Text = "5厘板";
            this.Btn5.UseVisualStyleBackColor = true;
            this.Btn5.Click += new System.EventHandler(this.Btn5_Click);
            // 
            // Btn12
            // 
            this.Btn12.Location = new System.Drawing.Point(83, 62);
            this.Btn12.Name = "Btn12";
            this.Btn12.Size = new System.Drawing.Size(65, 23);
            this.Btn12.TabIndex = 5;
            this.Btn12.Text = "12厘板";
            this.Btn12.UseVisualStyleBackColor = true;
            this.Btn12.Click += new System.EventHandler(this.Btn12_Click);
            // 
            // Btn9
            // 
            this.Btn9.Location = new System.Drawing.Point(12, 62);
            this.Btn9.Name = "Btn9";
            this.Btn9.Size = new System.Drawing.Size(65, 23);
            this.Btn9.TabIndex = 4;
            this.Btn9.Text = "9厘板";
            this.Btn9.UseVisualStyleBackColor = true;
            this.Btn9.Click += new System.EventHandler(this.Btn9_Click);
            // 
            // Btn18
            // 
            this.Btn18.Location = new System.Drawing.Point(83, 91);
            this.Btn18.Name = "Btn18";
            this.Btn18.Size = new System.Drawing.Size(65, 23);
            this.Btn18.TabIndex = 7;
            this.Btn18.Text = "18厘板";
            this.Btn18.UseVisualStyleBackColor = true;
            this.Btn18.Click += new System.EventHandler(this.Btn18_Click);
            // 
            // Btn15
            // 
            this.Btn15.Location = new System.Drawing.Point(12, 91);
            this.Btn15.Name = "Btn15";
            this.Btn15.Size = new System.Drawing.Size(65, 23);
            this.Btn15.TabIndex = 6;
            this.Btn15.Text = "15厘板";
            this.Btn15.UseVisualStyleBackColor = true;
            this.Btn15.Click += new System.EventHandler(this.Btn15_Click);
            // 
            // BtnOK
            // 
            this.BtnOK.Location = new System.Drawing.Point(83, 120);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(65, 23);
            this.BtnOK.TabIndex = 8;
            this.BtnOK.Text = "确定";
            this.BtnOK.UseVisualStyleBackColor = true;
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // SetBasePlateThickness
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(159, 148);
            this.Controls.Add(this.BtnOK);
            this.Controls.Add(this.Btn18);
            this.Controls.Add(this.Btn15);
            this.Controls.Add(this.Btn12);
            this.Controls.Add(this.Btn9);
            this.Controls.Add(this.Btn5);
            this.Controls.Add(this.Btn3);
            this.Controls.Add(this.TxtCurThickness);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetBasePlateThickness";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设置基板厚度";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtCurThickness;
        private System.Windows.Forms.Button Btn3;
        private System.Windows.Forms.Button Btn5;
        private System.Windows.Forms.Button Btn12;
        private System.Windows.Forms.Button Btn9;
        private System.Windows.Forms.Button Btn18;
        private System.Windows.Forms.Button Btn15;
        private System.Windows.Forms.Button BtnOK;
    }
}