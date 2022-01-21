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
    public partial class ServicesForm : Form
    {

        public int ServiceId = 0;

        public ServicesForm()
        {
            InitializeComponent();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            NewServicesForm newService = new NewServicesForm(null);
            if (newService.ShowDialog(this) == DialogResult.OK)
            {
                dgvServices.DataSource = ServiceFacade.GetServices();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvServices.CurrentCell != null)
            {
                int cellValue = Convert.ToInt32(dgvServices.Rows[dgvServices.CurrentCell.RowIndex].Cells[0].Value.ToString());
                NewServicesForm newService = new NewServicesForm(cellValue);
                if (newService.ShowDialog(this) == DialogResult.OK)
                {
                    dgvServices.DataSource = ServiceFacade.GetServices();
                }
            }
        }

        private void BtnDel_Click(object sender, EventArgs e)
        {
            if (this.dgvServices.SelectedRows.Count > 0)
            {
                int cellValue = Convert.ToInt32(dgvServices.Rows[dgvServices.CurrentCell.RowIndex].Cells[0].Value.ToString());

                ServiceFacade.Delete(cellValue);

                dgvServices.DataSource = ServiceFacade.GetServices();
                
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tbnOk_Click(object sender, EventArgs e)
        {
            if (dgvServices.CurrentCell != null)
            {
                int cellValue = Convert.ToInt32(dgvServices.Rows[dgvServices.CurrentCell.RowIndex].Cells[0].Value.ToString());
                this.ServiceId = cellValue;
                this.DialogResult = DialogResult.OK;
            }
        }

        private void ServicesForm_Load(object sender, EventArgs e)
        {
            dgvServices.DataSource = ServiceFacade.GetServices();
            if (dgvServices.ColumnCount > 0)
            {
                dgvServices.Columns[0].Visible = false;
                dgvServices.Columns[1].HeaderText = "Код";
                dgvServices.Columns[2].HeaderText = "Название";
                dgvServices.Columns[3].HeaderText = "Цена";
                dgvServices.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvServices.Columns[1].Width = 40;
            }
        }
    }
}
