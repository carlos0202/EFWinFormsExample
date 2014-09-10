using EFWinFormsExample.Helpers;
using EFWinFormsExample.Models;
using EFWinFormsExample.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EFWinFormsExample.Views
{
    public partial class FrmEditClient : Form
    {
        private Client client;
        private ClientViewModel viewModel;

        public FrmEditClient(int clientId)
        {
            InitializeComponent();
            client = new Client();
            viewModel = new ClientViewModel();
            using(DBContext db = new DBContext())
                client = db.Clients.Find(clientId);
            DataHelpers.Transfer<Client, ClientViewModel>(client, viewModel);
            clientBindingSource.DataSource = viewModel;
        }

        private void FrmEditClient_Load(object sender, EventArgs e)
        {
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (viewModel.IsValid())
            {
                using (DBContext db = new DBContext())
                {
                    DataHelpers.Transfer<ClientViewModel, Client>(viewModel, client);
                    db.Entry(client).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                MessageBox.Show("Datos de cliente actualizados satisfactoriamente.");
                this.Close();
                this.Dispose();
            }
        }
    }
}
