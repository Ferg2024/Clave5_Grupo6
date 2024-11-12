using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Clave5_Grupo6
{
    /// <summary>
    /// Autores: 
    /// González Sánchez, Ferdinand Gerard GS24015
    /// Melendez Arteaga, Mateo Alejandro MA21077
    /// Fecha de entrega: 13/11/2024
    /// descripcion del formulario: 
    /*Este es un formulario de registro de clientes.*/
    /// </summary>
    
    public partial class FormClientes : Form
    {
        //Creacion de variables estaticas de clase 

        static string Servidor = "localhost"; //Nombre del servidor de MySQL
        static string BD = "clave5_grupodetrabajodb6"; //Nombre de la base de datos 
        static string Usuario = "root"; //Usuario de acceso a MySQL
        static string Paswoord = "root"; //Contraseña de usuario de acceso a MySQL

        //Crearemos la cadena de conexión concatenando las variables

        static string cadenaConexion = "Database=" + BD + "; Data Source=" + Servidor + "; User Id = " + Usuario + "; Password=" + Paswoord + "";

        //Instancia para conexión a MySQL, recibe la cadena de conexión
        static MySqlConnection conexionBD = new MySqlConnection(cadenaConexion);

        public FormClientes()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                conexionBD.Open(); //se abre la conexion de la variable global declara enla parte superior del formulario
                MessageBox.Show("Conexión exitosa!", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);// se manda un mensaje de estado de conexion

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
            finally
            {
                conexionBD.Close(); // se cierra la conexion
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            // Mostrar un cuadro de mensaje de confirmación con botones Yes y No
            DialogResult resultado = MessageBox.Show("¿Estás seguro de que quieres salir?", "Confirmar salida", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                Application.Exit();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Crear una nueva instancia del formulario 'Reservas'
            Reservas reservasForm = new Reservas();

            // Cerrar el formulario actual (Form1)
            this.Hide();

            // Mostrar el formulario 'Reservas'
            reservasForm.Show();

        }
    }
}
