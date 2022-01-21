using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace uzPro.Forms
{
    public partial class HeaderForm : Form
    {
        public HeaderForm()
        {
            InitializeComponent();
        }

        private void HeaderForm_Load(object sender, EventArgs e)
        {
            rtbText.Rtf = Properties.Settings.Default.header;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (rtbText.TextLength > 0) {
                string s = rtbText.Rtf.ToString();
                Properties.Settings.Default["header"] = s;
                Properties.Settings.Default.Save();
            }
            this.Close();
        }



        private void tbrFont_Click(object sender, EventArgs e)
        {
            SelectFontToolStripMenuItem_Click(this, e);
        }

        private void SelectFontToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (!(rtbText.SelectionFont == null))
                {
                    FontDialog1.Font = rtbText.SelectionFont;
                }
                else
                {
                    FontDialog1.Font = null;
                }
                FontDialog1.ShowApply = true;
                if (FontDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    rtbText.SelectionFont = FontDialog1.Font;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }




        private void tbrBold_Click(object sender, System.EventArgs e)
        {
            BoldToolStripMenuItem_Click(this, e);
        }


        private void tbrItalic_Click(object sender, System.EventArgs e)
        {
            ItalicToolStripMenuItem_Click(this, e);
        }


        private void tbrUnderline_Click(object sender, System.EventArgs e)
        {
            UnderlineToolStripMenuItem_Click(this, e);
        }


        private void tbrLeft_Click(object sender, System.EventArgs e)
        {
            rtbText.SelectionAlignment = HorizontalAlignment.Left;
        }


        private void tbrCenter_Click(object sender, System.EventArgs e)
        {
            rtbText.SelectionAlignment = HorizontalAlignment.Center;
        }


        private void tbrRight_Click(object sender, System.EventArgs e)
        {
            rtbText.SelectionAlignment = HorizontalAlignment.Right;
        }


        private void BoldToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (!(rtbText.SelectionFont == null))
                {
                    System.Drawing.Font currentFont = rtbText.SelectionFont;
                    System.Drawing.FontStyle newFontStyle;

                    newFontStyle = rtbText.SelectionFont.Style ^ FontStyle.Bold;

                    rtbText.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, newFontStyle);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }




        private void ItalicToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (!(rtbText.SelectionFont == null))
                {
                    System.Drawing.Font currentFont = rtbText.SelectionFont;
                    System.Drawing.FontStyle newFontStyle;

                    newFontStyle = rtbText.SelectionFont.Style ^ FontStyle.Italic;

                    rtbText.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, newFontStyle);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }


        private void UnderlineToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (!(rtbText.SelectionFont == null))
                {
                    System.Drawing.Font currentFont = rtbText.SelectionFont;
                    System.Drawing.FontStyle newFontStyle;

                    newFontStyle = rtbText.SelectionFont.Style ^ FontStyle.Underline;

                    rtbText.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, newFontStyle);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }

    }
}
