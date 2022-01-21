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
using uzPro.Utils;
using uzPro.Forms;

namespace uzPro
{
    public partial class MainForm : Form
    {

        private int clientId;
        public MainForm()
        {
            InitializeComponent();
        }
                    
        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                var protocols = ProtocolFacade.GetProtocolByClietnId(0);
            }
            catch  {
                MessageBox.Show("Нет подключения к БД");
            }
        }
              
       
        private void menuSelectClient_Click(object sender, EventArgs e)
        {
            ClientForm clientForm = new ClientForm();
            if (clientForm.ShowDialog(this) == DialogResult.OK) {
                if (clientForm.clientId > 0)
                {
                    clientId = clientForm.clientId;
                    LoadClientProtocol();
                }
            }
        }

        private void LoadClientProtocol()
        {
            Client currentClietn = ClientFacade.GetClientById(clientId);
            NameLabel.Text = string.Format("{0} ({1})", currentClietn.Name, currentClietn.BirthDate.ToShortDateString());
            dataGridView1.DefaultCellStyle.Font = new Font("Arial", 14);
            dataGridView1.DataSource = ProtocolFacade.GetProtocolByClietnId(currentClietn.Id);
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "Дата";            
            dataGridView1.Columns[2].HeaderText = "Название исследования";
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; 
            dataGridView1.Columns[1].Width = 150;
        }
                                       
        private void menuNewProtocol_Click(object sender, EventArgs e)
        {
            if (clientId >0)
            {
                Protocol protocol = new Protocol();
                protocol.ClientId = clientId;
                ProtocolForm protocolForm = new ProtocolForm(protocol);
                if (protocolForm.ShowDialog(this) == DialogResult.OK)
                {
                    dataGridView1.DataSource = ProtocolFacade.GetProtocolByClietnId(clientId);
                }
            }
            else {
                ClientForm clientForm = new ClientForm();
                if (clientForm.ShowDialog() == DialogResult.OK)
                {
                    if (clientForm.clientId >0)
                    {
                        clientId = clientForm.clientId;
                        LoadClientProtocol();
                    }
                }
            }
            
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int cellValue = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            Protocol protocol = ProtocolFacade.GetProtocolById(cellValue);
            if (protocol != null)
            {
                ProtocolForm protocolForm = new ProtocolForm(protocol);
                protocolForm.ShowDialog(this);
            }
            
        }

        private void menuTemplates_Click(object sender, EventArgs e)
        {
            TemplateForm templatesForm = new TemplateForm();
            templatesForm.ShowDialog(this);
        }

        private void menuBackUp_Click(object sender, EventArgs e)
        {
            
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string result = ServiceDB.BackUp(dialog.SelectedPath);
                    if (!string.IsNullOrEmpty(result)) {
                        MessageBox.Show(result);
                    }
                }                
            }
        }

        private void menuHeader_Click(object sender, EventArgs e)
        {
            HeaderForm headerForm = new HeaderForm();
            headerForm.Show();
        }

        private void menuServices_Click(object sender, EventArgs e)
        {
            ServicesForm services = new ServicesForm();
            services.ShowDialog(this);
        }

        private void menuTemplates_Click_1(object sender, EventArgs e)
        {
            TemplateForm templatesForm = new TemplateForm();
            templatesForm.ShowDialog(this);
        }

        
    }
}
