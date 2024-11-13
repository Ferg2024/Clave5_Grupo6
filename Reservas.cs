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
    public partial class Reservas : Form
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
        public Reservas()
        {
            InitializeComponent();
        }

        private void btnConexionBD_Click(object sender, EventArgs e)
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

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            // Crear una nueva instancia de FormClientes
            FormClientes formClientes = new FormClientes();

            // Mostrar FormClientes
            formClientes.Show();

            // Cerrar el formulario actual (Reservas)
            this.Close();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
          
                

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            //Recupera el id seleccionado a eliminar
            int id = Convert.ToInt32(MostrarReservas.CurrentRow.Cells["IDClientes"].Value);
            MySqlCommand consulta = new MySqlCommand();
            conexionBD.Open();
            consulta.Connection = conexionBD;
            consulta.CommandText = "DELETE FROM CLIENTE WHERE IDClientes = '" + id + "'";
            try
            {
                MySqlDataAdapter adaptadorMySQL = new MySqlDataAdapter();
                adaptadorMySQL.SelectCommand = consulta;
                DataTable tabla = new DataTable();

                adaptadorMySQL.Fill(tabla); //ejecutar el DELETE
                MessageBox.Show("Elemento eliminado!!");
                //SE CONSULTA LA TABLA NUEVAMENTE
                consulta.CommandText = ("select * from clientes"); //realizar una consulta de la tabla
                adaptadorMySQL.Fill(tabla);
                MostrarReservas.DataSource = tabla;
            }
            catch (MySqlException ex) //manejo de excepciones MySQL
            {
                MessageBox.Show("Error: " + ex);
            }
            finally
            {
                conexionBD.Close();
            }
            //Fin botón eliminar
        }

        private void btnMostrar_Click(object sender, EventArgs e)
        {
            //Inicializa una nueva instancia de la clase MySqlCommand.
            MySqlCommand consulta = new MySqlCommand();
            conexionBD.Open(); //se abre la conexion de la variable declarada en la superior del formulario

            //Instancia para conexión a MySQL, recibe la cadena de conexión
            consulta.Connection = conexionBD;
            consulta.CommandText = ("select * from clientes");

            try
            {

                //Inicializa una nueva instancia de la clase MySqlDataAdapter con
                //el MySqlCommand especificado como propiedad SelectCommand.
                MySqlDataAdapter adaptadorMySQL = new MySqlDataAdapter();
                adaptadorMySQL.SelectCommand = consulta;
                DataTable tabla = new DataTable();
                adaptadorMySQL.Fill(tabla);
                MostrarReservas.DataSource = tabla;
            }
            catch (MySqlException ex) //manejo de excepciones MySQL
            {
                MessageBox.Show("Error: " + ex);
            }
            finally
            {
                conexionBD.Close();
            }
        }
    }
}