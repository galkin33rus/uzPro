using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using uzPro.Concrete;

namespace uzPro.Forms
{
    public partial class NewClientForm : Form
    {

        private int clietnId = 0;
                        
        public NewClientForm(int? id)
        {
            InitializeComponent();

            if (id != null) {
                clietnId = Convert.ToInt32(id);

                Client client = ClientFacade.GetClientById(clietnId);
                if (client != null) {
                    tbName.Text = client.Name;
                    tbBirthDate.Text = client.BirthDate.ToShortDateString();
                    tbPasport.Text = client.Pasport;
                    tbReg.Text = client.Reg;
                    tbTel.Text = client.Tel;
                }
            }



        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbName.Text))
            {
                MessageBox.Show("Поле ФИО не должно быть пустым");
                return;
            }

            if (string.IsNullOrWhiteSpace(tbBirthDate.Text))
            {
                MessageBox.Show("Поле Дата рождения не должно быть пустым");
                return;
            }

            DateTime dt;
            if (!DateTime.TryParse(tbBirthDate.Text.Trim(), out dt))
            {
                MessageBox.Show("Не верно заполнено поле 'Дата рождения'");
                return;
            }

            Client client = new Client();

            client.Id = clietnId;
            client.Name = tbName.Text;
            client.BirthDate = dt;
            client.Pasport = tbPasport.Text;
            client.Reg = tbReg.Text;
            client.Tel = tbTel.Text;

            if (clietnId > 0)
            {
                ClientFacade.Update(client);
            }
            else
            {
                ClientFacade.Create(client);
            }

           

            this.DialogResult = DialogResult.OK;
        }

        private void tbBirthDate_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void tbBirthDate_Leave(object sender, EventArgs e)
        {
            tbBirthDate.Text = tbBirthDate.Text.Replace(',', '.');
        }
    }
}
