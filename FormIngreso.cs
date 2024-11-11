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
                MessageBox.Show("Bienvenid@, presione en Aceptar", "Work Office S.A de C.V", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.Hide();

                FormClientes formClientes = new FormClientes();
                formClientes.Show();
            }
            else
            {
                MessageBox.Show("Los datos que ha ingresado no son los correctos.", "Intente Nuevamente", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                txtUser.Text = "";
                txtPassword.Text = "";
                txtUser.Focus();
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            // Mostrar un cuadro de mensaje de confirmación con botones Yes y No
            DialogResult resultado = MessageBox.Show("¿Estás seguro de que quieres salir?", "Confirmar salida",MessageBoxButtons.YesNo,MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void FormIngreso_Load(object sender, EventArgs e)
        {

        }

        private void txtUser_TextChanged(object sender, EventArgs e)
        {
            // Obtener el texto del TextBox
            string texto = txtUser.Text;

            // Validar que no esté vacío ni contenga solo espacios
            if (string.IsNullOrWhiteSpace(texto))
            {
      
                errorUsuario.SetError(txtUser, "El campo no puede contener espacios vacíos.");
            }
            else
            {
                // Si pasa la validación, quitar el mensaje de error
                errorUsuario.SetError(txtUser, string.Empty);
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
