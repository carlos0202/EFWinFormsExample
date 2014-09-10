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
    public partial class FrmAddClient : Form
    {
        private Client client;
        private ClientViewModel viewModel;

        public FrmAddClient()
        {
            InitializeComponent();
            client = new Client();
            viewModel = new ClientViewModel();
        }

        private void FrmAddClient_Load(object sender, EventArgs e)
        {
            clientBindingSource.DataSource = viewModel;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (viewModel.IsValid())
            {
                using (var db = new DBContext())
                {
                    DataHelpers.Transfer<ClientViewModel, Client>(viewModel, client);
                    db.Clients.Add(client);
                    db.SaveChanges();
                }
                MessageBox.Show("Cliente registrado satisfactoriamente.");
                this.Close();
                this.Dispose();
            }
        }
    }
}
