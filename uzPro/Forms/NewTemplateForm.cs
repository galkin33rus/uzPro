using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using uzPro.Concrete;

namespace uzPro.Forms
{
    public partial class NewTemplateForm : Form
    {

        private int templateId;

        public NewTemplateForm(int? id)
        {
            InitializeComponent();

            if (id != null)
            {
                templateId = Convert.ToInt32(id);

                Template template = TemplateFacade.GetTemplateById(templateId);
                if (template != null)
                {
                    tbTitle.Text = template.Title;
                    rtbText.Rtf = template.Text;
                    
                }
            }

            ContextMenu contextMenu = new System.Windows.Forms.ContextMenu();
            MenuItem menuItem = new MenuItem("Вырезать");
            menuItem.Click += new EventHandler(CutAction);
            contextMenu.MenuItems.Add(menuItem);
            menuItem = new MenuItem("Копировать");
            menuItem.Click += new EventHandler(CopyAction);
            contextMenu.MenuItems.Add(menuItem);
            menuItem = new MenuItem("Вставить");
            menuItem.Click += new EventHandler(PasteAction);
            contextMenu.MenuItems.Add(menuItem);

            rtbText.ContextMenu = contextMenu;
        }

        void CutAction(object sender, EventArgs e)
        {
            rtbText.Cut();
        }

        void CopyAction(object sender, EventArgs e)
        {
            Clipboard.Clear();
            rtbText.Copy();
        }

        void PasteAction(object sender, EventArgs e)
        {
            rtbText.Paste();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Template template = new Template();

            template.Id = templateId;
            template.Title = tbTitle.Text;
            template.Text = rtbText.Rtf;


            if (templateId > 0)
            {
                TemplateFacade.Update(template);
            }
            else
            {
                TemplateFacade.Create(template);
            }

            this.DialogResult = DialogResult.OK;
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
