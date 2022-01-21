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
    public partial class NewServicesForm : Form
    {
        private int serviceId;
       
        public NewServicesForm(int? id)
        {
            InitializeComponent();

            if (id != null) {
                serviceId = Convert.ToInt32(id);

                Service service = ServiceFacade.GetServiceById(serviceId);
                if (service != null)
                {
                    tbKod.Text = service.Kod;
                    tbName.Text = service.Name;
                    tbPrice.Text = service.Price.ToString();
                }
            }



        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Service service = new Service();

            service.Id = serviceId;
            service.Name = tbName.Text;
            service.Kod = tbKod.Text;

            service.Price = float.Parse(tbPrice.Text);

            if (serviceId > 0)
            {
                ServiceFacade.Update(service);
            }
            else
            {
                ServiceFacade.Create(service);
            }

            this.DialogResult = DialogResult.OK;
        }
    }
}
