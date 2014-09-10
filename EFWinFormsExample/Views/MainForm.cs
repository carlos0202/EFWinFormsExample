using EFWinFormsExample.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EFWinFormsExample.Helpers;
using EFWinFormsExample.Models.ViewModels;

namespace EFWinFormsExample.Views
{
    public partial class MainForm : Form
    {

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void RefreshData()
        {
            var source = new List<ClientViewModel>();
            var clientes = new List<Client>();
            using(DBContext db = new DBContext())
                clientes = db.Clients.ToList();
            clientes.ForEach(c => 
                {
                    c.DNI = c.DNI.CFormat("dni");
                    source.Add(DataHelpers.Transfer<Client, ClientViewModel>(c));
                });

            dgvClientes.DataSource = source;
            dgvClientes.Refresh();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            new FrmAddClient().ShowDialog();
            RefreshData();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var cliente = this.dgvClientes.SelectedRows[0].DataBoundItem as ClientViewModel;
            new FrmEditClient(cliente.Id).ShowDialog();
            RefreshData();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                "¿Esta seguro de eliminar este cliente?",
                "Eliminar Cliente",
                MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
            {
                var cliente = this.dgvClientes.SelectedRows[0].DataBoundItem as ClientViewModel;
                using (DBContext db = new DBContext())
                {
                    var clienteDlt = db.Clients.Find(cliente.Id);
                    db.Clients.Remove(clienteDlt);
                    db.SaveChanges();
                    MessageBox.Show("Cliente eliminado satisfactoriamente.");
                    RefreshData();
                }
            }
            else
            {
                MessageBox.Show("Operación cancelada");
            }
        }
    }
}
