using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clave5_Grupo6
{
    public partial class FormIngreso : Form
    {
        public FormIngreso()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            String Users, Password;
            Users = txtUser.Text.TrimEnd();
            Password = txtPassword.Text.TrimEnd();

            if (Users == "admin" && Password == "1111")
            {
                MessageBox.Show("Bienvenido, espere un momento...", "Iniciando.....", MessageBoxButtons.OK);
                this.Hide();

                FormClientes formClientes = new FormClientes();
                formClientes.Show();
            }
            else
            {
                MessageBox.Show("Datos Incorrectos.", "Intente Nuevamente", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                txtUser.Text = "";
                txtPassword.Text = "";
                txtUser.Focus();
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
