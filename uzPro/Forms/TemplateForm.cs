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
    public partial class TemplateForm : Form
    {

        public int TemplateId = 0;

        public TemplateForm()
        {
            InitializeComponent();
        }

        private void TemplateForm_Load(object sender, EventArgs e)
        {
            dgvTemplate.DefaultCellStyle.Font = new Font("Arial", 14);
            dgvTemplate.DataSource = TemplateFacade.GetTemplates();
            dgvTemplate.Columns[0].Visible = false;
            dgvTemplate.Columns[1].HeaderText = "N";
            dgvTemplate.Columns[2].HeaderText = "Название";
            dgvTemplate.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTemplate.Columns[1].Width = 40;           
        }

        private void menuNewTemplate_Click(object sender, EventArgs e)
        {
            NewTemplateForm newTemplate = new NewTemplateForm(null);
            if (newTemplate.ShowDialog(this) == DialogResult.OK)
            {
                dgvTemplate.DataSource = TemplateFacade.GetTemplates();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (dgvTemplate.CurrentCell != null)
            {
                int cellValue = Convert.ToInt32(dgvTemplate.Rows[dgvTemplate.CurrentCell.RowIndex].Cells[0].Value.ToString());
                this.TemplateId = cellValue;
                this.DialogResult = DialogResult.OK;
            }
        }

        private void menuEditTemplate_Click(object sender, EventArgs e)
        {
            if (dgvTemplate.CurrentCell != null)
            {
                int cellValue = Convert.ToInt32(dgvTemplate.Rows[dgvTemplate.CurrentCell.RowIndex].Cells[0].Value.ToString());
                NewTemplateForm newTemplate = new NewTemplateForm(cellValue);
                if (newTemplate.ShowDialog() == DialogResult.OK)
                {
                    dgvTemplate.DataSource = TemplateFacade.GetTemplates();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void menuDeleteTemplate_Click(object sender, EventArgs e)
        {
            if (dgvTemplate.CurrentCell != null)
            {
                int cellValue = Convert.ToInt32(dgvTemplate.Rows[dgvTemplate.CurrentCell.RowIndex].Cells[0].Value.ToString());


                TemplateFacade.Delete(cellValue);

                dgvTemplate.DataSource = TemplateFacade.GetTemplates();
                
            }
        }
    }
}
