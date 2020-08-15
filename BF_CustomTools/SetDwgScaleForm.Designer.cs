namespace BF_CustomTools
{
    partial class SetDwgScaleForm
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
            this.listBoxGlobalScale = new System.Windows.Forms.ListBox();
            this.listBoxLocalScale = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listBoxGlobalScale
            // 
            this.listBoxGlobalScale.FormattingEnabled = true;
            this.listBoxGlobalScale.ItemHeight = 12;
            this.listBoxGlobalScale.Items.AddRange(new object[] {
            "1:1",
            "1:2",
            "1:3",
            "1:4",
            "1:5",
            "1:8",
            "1:10",
            "1:12",
            "1:15",
            "1:20",
            "1:25",
            "1:30",
            "1:35",
            "1:40",
            "1:45",
            "1:50",
            "1:55",
            "1:60",
            "1:65",
            "1:70",
            "1:75",
            "1:80",
            "1:85",
            "1:90",
            "1:95",
            "1:100"});
            this.listBoxGlobalScale.Location = new System.Drawing.Point(12, 26);
            this.listBoxGlobalScale.Name = "listBoxGlobalScale";
            this.listBoxGlobalScale.Size = new System.Drawing.Size(141, 232);
            this.listBoxGlobalScale.TabIndex = 0;
            this.listBoxGlobalScale.SelectedIndexChanged += new System.EventHandler(this.ListBoxGlobalScale_SelectedIndexChanged);
            // 
            // listBoxLocalScale
            // 
            this.listBoxLocalScale.Enabled = false;
            this.listBoxLocalScale.FormattingEnabled = true;
            this.listBoxLocalScale.ItemHeight = 12;
            this.listBoxLocalScale.Items.AddRange(new object[] {
            "1:2",
            "1:3",
            "1:4",
            "1:5",
            "1:6",
            "1:7",
            "1:8",
            "1:9",
            "1:10",
            "1:12",
            "1:15"});
            this.listBoxLocalScale.Location = new System.Drawing.Point(161, 27);
            this.listBoxLocalScale.Name = "listBoxLocalScale";
            this.listBoxLocalScale.Size = new System.Drawing.Size(130, 160);
            this.listBoxLocalScale.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 277);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "全局比例 1 :";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(93, 274);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(60, 21);
            this.textBox1.TabIndex = 3;
            // 
            // textBox2
            // 
            this.textBox2.Enabled = false;
            this.textBox2.Location = new System.Drawing.Point(242, 199);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(49, 21);
            this.textBox2.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(159, 202);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "全局比例 1 :";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(161, 241);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(84, 16);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.Text = "有局部比例";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.CheckBox1_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(216, 272);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "确定";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "全局比例列表";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(159, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "局部比例列表";
            // 
            // SetDwgScaleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(303, 310);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBoxLocalScale);
            this.Controls.Add(this.listBoxGlobalScale);
            this.Name = "SetDwgScaleForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "[百福工具箱]—设置图纸比例";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxGlobalScale;
        private System.Windows.Forms.ListBox listBoxLocalScale;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}