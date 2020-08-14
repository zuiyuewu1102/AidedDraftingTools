using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BF_CustomTools
{
    public partial class TitleForm : Form
    {
        public TitleForm()
        {
            InitializeComponent();
            if (PublicValue.newBlockName != null)
            {
                switch (PublicValue.newBlockName)
                {
                    case "BF-立面标题":
                        radioButton2.Checked = true;
                        textBox3.Text = "立面图";
                        textBox3.Enabled = false;
                        listBox3.Enabled = false;
                        textBox1.Enabled = true;
                        listBox1.Enabled = true;
                        break;
                    case "BF-大样标题":
                        radioButton3.Checked = true;
                        textBox3.Text = "大样图";
                        textBox3.Enabled = false;
                        listBox3.Enabled = false;
                        textBox1.Enabled = true;
                        listBox1.Enabled = true;
                        break;
                    default:
                        PublicValue.newBlockName = "BF-平面标题";
                        listBox1.Enabled = false;
                        textBox1.Enabled = false;
                        textBox3.Enabled = true;
                        listBox3.Enabled = true;
                        break;
                }
            }
            else
                PublicValue.newBlockName = "BF-平面标题";

            if (PublicValue.scale != null)
            {
                for (int i = 0; i < listBox2.Items.Count; i++)
                {
                    if (PublicValue.scale == listBox2.Items[i].ToString()) 
                    {
                        listBox2.SelectedIndex = i;
                        textBox2.Text = PublicValue.scale;
                        break;
                    }
                }
            }
            else
            {
                listBox2.SelectedIndex = 0;
                textBox2.Text = listBox2.Items.ToString();
            }

            if (PublicValue.syNo != null)
            {
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    if (PublicValue.syNo == listBox1.Items[i].ToString())
                    {
                        listBox1.SelectedIndex = i;
                        textBox1.Text = PublicValue.syNo;
                        break;
                    }
                }
            }
            else
            {
                listBox1.SelectedIndex = 0;
                textBox1.Text = listBox1.Items.ToString();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            PublicValue.syNo = textBox1.Text;
            PublicValue.scale = textBox2.Text;
            PublicValue.DwgName = textBox3.Text;            
            this.Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = listBox1.SelectedItem.ToString();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox2.Text = listBox2.SelectedItem.ToString();
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox3.Text = listBox3.SelectedItem.ToString();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton2.Checked == true)
            {
                PublicValue.newBlockName = "BF-立面标题";
                textBox3.Text = "立面图";
                textBox3.Enabled = false;
                listBox3.Enabled = false;
                textBox1.Enabled = true;
                listBox1.Enabled = true;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked == true)
            {

                PublicValue.newBlockName = "BF-大样标题";
                textBox3.Text = "大样图";
                textBox3.Enabled = false;
                listBox3.Enabled = false;
                textBox1.Enabled = true;
                listBox1.Enabled = true;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                PublicValue.newBlockName = "BF-平面标题";
                listBox1.Enabled = false;
                textBox1.Enabled = false;
                listBox3.Enabled = true;
                textBox3.Enabled = true;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            radioButton1.Checked = true;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            radioButton2.Checked = true;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            radioButton3.Checked = true;
        }
    }
}
