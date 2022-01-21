using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using uzPro.Concrete;
using ExtendedRichTextBox;
using uzPro.Properties;


namespace uzPro.Forms
{
    public partial class ProtocolForm : Form
    {

        private int _clientId;
        private int _protocolId;
        private Client currentClient;
        private int checkPrint;
        RichTextBoxPrintCtrl rtbPrint = new RichTextBoxPrintCtrl();
        private string old_text;

        public ProtocolForm(Protocol protocol)
        {
            InitializeComponent();

            rtbPrint.Visible = false;

            if (protocol != null)
            {
                if (protocol.ClientId > 0)
                {
                    _clientId = Convert.ToInt32(protocol.ClientId);
                }

                currentClient = ClientFacade.GetClientById(_clientId);
                if (currentClient != null && currentClient.Id > 0)
                {
                    this.Text = string.Format("Протокол УЗИ - {0} {1}", currentClient.Name, currentClient.BirthDate.ToShortDateString());
                }

                if (protocol.Id > 0)
                {
                    _protocolId = protocol.Id;
                    dtpDate.Value = protocol.Date;
                    tbTitle.Text = protocol.Title;
                    rtbText.Rtf = protocol.Text;
                    old_text = protocol.Text;
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

        private void menuSave_Click(object sender, EventArgs e)
        {
            SaveProtocol();
            MessageBox.Show("Протокол сохранен.");
        }

        private void SaveProtocol()
        {
            Protocol protocol = new Protocol();
            if (currentClient != null && currentClient.Id > 0)
            {
                protocol.ClientId = currentClient.Id;
                protocol.Date = dtpDate.Value;
                protocol.Title = tbTitle.Text;
                protocol.Text = rtbText.Rtf;
                old_text = rtbText.Rtf;
                if (_protocolId > 0)
                {
                    protocol.Id = _protocolId;
                    ProtocolFacade.Update(protocol);
                }
                else
                {
                    ProtocolFacade.Create(protocol);
                    _protocolId = protocol.Id;
                }
            }
            else
            {
                MessageBox.Show("Не выбран клиент.");
            }

        }

        private void menuSelectTemplate_Click(object sender, EventArgs e)
        {
            TemplateForm templateForm = new TemplateForm();

            if (templateForm.ShowDialog(this) == DialogResult.OK)
            {
                if (templateForm.TemplateId > 0)
                {
                    Template template = TemplateFacade.GetTemplateById(templateForm.TemplateId);
                    if (template != null)
                    {
                        tbTitle.Text += template.Title;
                        rtbText.SelectedRtf = template.Text;
                    }
                }
            }
        }

        private void menuPrint_Click(object sender, EventArgs e)
        {

            rtbPrint.SelectedRtf = Properties.Settings.Default.header;
            rtbPrint.DeselectAll();
            rtbPrint.SelectionFont = new Font("Times New Roman", 12.0f, FontStyle.Bold);
            rtbPrint.AppendText("\r\n " + tbTitle.Text.Trim());            
            rtbPrint.AppendText("\r\n ФИО: " + string.Format("{0} ({1})", currentClient.Name, currentClient.BirthDate.ToShortDateString()));
            rtbPrint.SelectionFont = new Font("Times New Roman", 12.0f);
            rtbPrint.AppendText("\r\n Дата обследования: " + dtpDate.Value.ToShortDateString());
            rtbPrint.AppendText("\r\n");
            rtbPrint.DeselectAll();
            rtbPrint.SelectionFont = new Font("Times New Roman", 14.0f, FontStyle.Bold);
            rtbPrint.SelectionAlignment = HorizontalAlignment.Center;
            rtbPrint.AppendText("\r\n Протокол обследования");
            rtbPrint.AppendText("\r\n");
            rtbPrint.AppendText("\r\n");
            rtbPrint.SelectionAlignment = HorizontalAlignment.Left;
            rtbPrint.SelectedRtf = rtbText.Rtf;
            rtbPrint.DeselectAll();
            rtbPrint.SelectionFont = new Font("Times New Roman", 12.0f);
            string sign = Properties.Settings.Default.sign;
            rtbPrint.AppendText("\r\n Врач: " + sign + "________________________________");

            try
            {
                printDialog1.Document = printDocument1;
                if (printDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Margins margins = new Margins(50, 40, 50, 40);
                    printDocument1.DefaultPageSettings.Margins = margins;
                    printDocument1.Print();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            checkPrint = rtbPrint.Print(checkPrint, rtbPrint.TextLength, e);

            if (checkPrint < rtbPrint.TextLength)
            {
                e.HasMorePages = true;
            }
            else
            {
                e.HasMorePages = false;
            }
        }

        private void ProtocolForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (rtbText.Rtf != old_text)
            {
                DialogResult result = MessageBox.Show("Cохранить изменения?", "Сохранение протокола", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    SaveProtocol();
                    this.DialogResult = DialogResult.OK;
                }
                else if (result == DialogResult.No)
                {
                    this.DialogResult = DialogResult.OK;
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }
            else {
                this.DialogResult = DialogResult.OK;           
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

        private void rtbText_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                
            }
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

        private void menuContract1_Click(object sender, EventArgs e)
        {

            if (_protocolId == 0) {
                SaveProtocol();
            }



            ContractForm contract = new ContractForm(_protocolId);

            contract.ShowDialog(this);

            //Client client = ClientFacade.GetClientById(currentClient.Id);
            //Dictionary<string, string> pacient = new Dictionary<string, string>();
            //pacient.Add("Date", dtpDate.Value.ToShortDateString());
            //pacient.Add("Name", currentClient.Name);
            //pacient.Add("Pasp", currentClient.Pasport);
            //pacient.Add("Address", currentClient.Reg);
            //pacient.Add("Tel", currentClient.Tel);            
            //System.Diagnostics.Process.Start(PDFManager.GetContract(pacient));
        }
    }
}
