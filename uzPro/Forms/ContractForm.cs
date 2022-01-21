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

namespace uzPro.Forms
{
    public partial class ContractForm : Form
    {

        private int _protocolId;
        private int _contractId;

        public ContractForm()
        {
            InitializeComponent();
        }

        public ContractForm(int? protocolId)
        {
            InitializeComponent();
                        
            dgvServices.Columns.Add("Kod", "Код");
            dgvServices.Columns.Add("Name", "Название");
            dgvServices.Columns.Add("Price", "Цена");
            dgvServices.Columns.Add("Count", "Количество");
            dgvServices.Columns.Add("Sum", "Общая сумма");
            dgvServices.Columns.Add("Sale", "Сумма скидки");
            dgvServices.Columns.Add("Total", "Всего");            
            dgvServices.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvServices.Columns[0].Width = 40;


            if (protocolId != null)
            {
                _protocolId = Convert.ToInt32(protocolId);
            }

            if (_protocolId > 0)
            {

                Contract contract = ContractFacade.GetContractByProtocolId(_protocolId);
                Protocol protocol = ProtocolFacade.GetProtocolById(_protocolId);

                if (contract != null)
                {
                    tbNumContract.Text = contract.NumContract;
                    tbDateContract.Text = protocol.Date.ToShortDateString();
                    tbNumAct.Text = contract.NumAct;
                    tbName.Text = contract.Name;
                    tbBirthDate.Text = contract.BirthDate;
                    tbPasport.Text = contract.Pasport;
                    tbReg.Text = contract.Reg;
                    tbTel.Text = contract.Tel;

                    _contractId = contract.id;
                    FillServices(contract.id);
                }
                else
                {                    
                    Client client = ClientFacade.GetClientById(protocol.ClientId);
                    tbDateContract.Text = DateTime.Now.ToShortDateString();
                    tbName.Text = client.Name;
                    tbBirthDate.Text = client.BirthDate.ToShortDateString();
                    tbPasport.Text = client.Pasport;
                    tbReg.Text = client.Reg;
                    tbTel.Text = client.Tel;
                }
                

               
            }
            
        }

        private void FillServices(int contractId)
        {
            if (contractId > 0)
            {
                DataTable dt = ContractValuesFacade.GetValuesByContractId(contractId);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dgvServices.Rows.Add(dt.Rows[i][0], dt.Rows[i][1], dt.Rows[i][2], dt.Rows[i][3], dt.Rows[i][4], dt.Rows[i][5], dt.Rows[i][6]);
                }
            }
            
        }

        private void btnAddService_Click(object sender, EventArgs e)
        {
            ServicesForm services = new ServicesForm();

            if(services.ShowDialog(this) == DialogResult.OK){
                Service service = ServiceFacade.GetServiceById(services.ServiceId);
                dgvServices.Rows.Add(service.Kod, service.Name, service.Price, 1, service.Price, 0, service.Price);
                
            }

        }

        private void ContractForm_Load(object sender, EventArgs e)
        {

        }

        private void btnContractCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnContractSave_Click(object sender, EventArgs e)
        {
            SaveContract();
            MessageBox.Show("Догвор сохранен");
        }

        private void btnContractPrint_Click(object sender, EventArgs e)
        {

            SaveContract();

            Dictionary<string, string> pacient = new Dictionary<string, string>();
            pacient.Add("NumContract", tbNumContract.Text);
            pacient.Add("NumAct", tbNumAct.Text);
            pacient.Add("Date", tbDateContract.Text);
            pacient.Add("Name", tbName.Text);
            pacient.Add("Pasp", tbPasport.Text);
            pacient.Add("Address", tbReg.Text);
            pacient.Add("Tel", tbTel.Text);

            List<Service> servList = new List<Service>();

            DataTable dt = new DataTable();
            foreach (DataGridViewColumn col in dgvServices.Columns)
            {
                dt.Columns.Add(col.Name);
            }

            foreach (DataGridViewRow row in dgvServices.Rows)
            {
                DataRow dRow = dt.NewRow();
                foreach (DataGridViewCell cell in row.Cells)
                {
                    dRow[cell.ColumnIndex] = cell.Value;
                }
                dt.Rows.Add(dRow);
            }

            System.Diagnostics.Process.Start(PDFManager.GetContract(pacient, dt));
        }

        private void SaveContract()
        {
            Contract contract = new Contract();
            contract.id = _contractId;
            contract.ProtocolID = _protocolId;
            contract.NumContract = tbNumContract.Text;
            contract.NumAct = tbNumAct.Text;
            contract.Name = tbName.Text;
            contract.BirthDate = tbBirthDate.Text;
            contract.Pasport = tbPasport.Text;
            contract.Reg = tbReg.Text;
            contract.Tel = tbTel.Text;


            if (contract.id == 0)
            {
                ContractFacade.Create(contract);

                SaveContractValues(contract);
            }
            else
            {
                ContractFacade.Update(contract);

                ContractValuesFacade.DeleteContractValuesByContractId(contract.id);

                SaveContractValues(contract);
            }
        }

        private void SaveContractValues(Contract contract)
        {
            foreach (DataGridViewRow row in dgvServices.Rows)
            {
                ContractValue contractValue = new ContractValue();

                contractValue.ContractID = contract.id;
                contractValue.Kod = row.Cells[0].Value.ToString();
                contractValue.Name = row.Cells[1].Value.ToString();
                contractValue.CostOne = row.Cells[2].Value.ToString();
                contractValue.Count = row.Cells[3].Value.ToString();
                contractValue.CostAll = row.Cells[4].Value.ToString();
                contractValue.Sale = row.Cells[5].Value.ToString();
                contractValue.Total = row.Cells[6].Value.ToString();

                ContractValuesFacade.Create(contractValue);
            }
        }

        private void btnContractCancel_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.dgvServices.SelectedRows.Count > 0)
            {
                dgvServices.Rows.RemoveAt(this.dgvServices.SelectedRows[0].Index);
            }      
        }
    }
}
