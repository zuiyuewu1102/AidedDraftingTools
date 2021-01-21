namespace BF_CustomTools
{
    partial class ReBlockNameForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxOldBlockName = new System.Windows.Forms.TextBox();
            this.textBoxNewBlockName = new System.Windows.Forms.TextBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "原块名：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "新块名：";
            // 
            // textBoxOldBlockName
            // 
            this.textBoxOldBlockName.Enabled = false;
            this.textBoxOldBlockName.Location = new System.Drawing.Point(85, 17);
            this.textBoxOldBlockName.Name = "textBoxOldBlockName";
            this.textBoxOldBlockName.Size = new System.Drawing.Size(196, 21);
            this.textBoxOldBlockName.TabIndex = 2;
            // 
            // textBoxNewBlockName
            // 
            this.textBoxNewBlockName.Enabled = false;
            this.textBoxNewBlockName.Location = new System.Drawing.Point(85, 59);
            this.textBoxNewBlockName.Name = "textBoxNewBlockName";
            this.textBoxNewBlockName.Size = new System.Drawing.Size(196, 21);
            this.textBoxNewBlockName.TabIndex = 3;
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(206, 95);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 5;
            this.buttonOk.Text = "确定";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.ButtonOk_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(28, 95);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(120, 16);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.Text = "是否使用自动命名";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.CheckBox1_CheckedChanged);
            // 
            // ReBlockNameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(308, 136);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.textBoxNewBlockName);
            this.Controls.Add(this.textBoxOldBlockName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ReBlockNameForm";
            this.Text = "百福工具箱——修改块名";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxOldBlockName;
        private System.Windows.Forms.TextBox textBoxNewBlockName;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}