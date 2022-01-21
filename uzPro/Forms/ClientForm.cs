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
    public partial class ClientForm : Form
    {

        public int clientId = 0;

        public ClientForm()
        {
            InitializeComponent();
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            dgvClients.DefaultCellStyle.Font = new Font("Arial", 14);
            dgvClients.DataSource = ClientFacade.GetClients();
            dgvClients.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvClients.Columns[0].Visible = false;
            dgvClients.Columns[1].HeaderText = "ФИО";
            dgvClients.Columns[1].Width = 500;
            dgvClients.Columns[2].HeaderText = "Дата рождения";
            dgvClients.Columns[2].Width = 150;
            dgvClients.Columns[3].HeaderText = "Телефон";
            dgvClients.Columns[3].Width = 250;
        }

        private void menuNewClient_Click(object sender, EventArgs e)
        {
            NewClientForm newClient = new NewClientForm(null);
            if (newClient.ShowDialog(this) == DialogResult.OK) {
                dgvClients.DataSource = ClientFacade.GetClients();
            }
        }

        private void dgvClients_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {            
            int cellValue = Convert.ToInt32(dgvClients.Rows[e.RowIndex].Cells[0].Value.ToString());
            this.clientId = cellValue;
            this.DialogResult = DialogResult.OK;
        }

        private void menuEditClient_Click(object sender, EventArgs e)
        {
            if (dgvClients.CurrentCell != null) { 
                int cellValue = Convert.ToInt32(dgvClients.Rows[dgvClients.CurrentCell.RowIndex].Cells[0].Value.ToString());
                NewClientForm newClient = new NewClientForm(cellValue);
                if (newClient.ShowDialog(this) == DialogResult.OK)
                {
                    dgvClients.DataSource = ClientFacade.GetClients();
                }
            }            
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbName.Text))
            {
                dgvClients.DataSource = ClientFacade.GetClientsByName(tbName.Text.Trim());
            }
            else {
                dgvClients.DataSource = ClientFacade.GetClients();
            }

        }

        private void tbName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(sender, e);
            }
        }

        private void tb_Clear_Click(object sender, EventArgs e)
        {
            tbName.Text = "";
            dgvClients.DataSource = ClientFacade.GetClients();
        }

        private void btnNewClient_Click(object sender, EventArgs e)
        {
            NewClientForm newClient = new NewClientForm(null);
            if (newClient.ShowDialog(this) == DialogResult.OK)
            {
                dgvClients.DataSource = ClientFacade.GetClients();
            }
        }

        private void btnEditClient_Click(object sender, EventArgs e)
        {
            if (dgvClients.CurrentCell != null)
            {
                int cellValue = Convert.ToInt32(dgvClients.Rows[dgvClients.CurrentCell.RowIndex].Cells[0].Value.ToString());
                NewClientForm newClient = new NewClientForm(cellValue);
                if (newClient.ShowDialog(this) == DialogResult.OK)
                {
                    dgvClients.DataSource = ClientFacade.GetClients();
                }
            }        
        }
    }
}
