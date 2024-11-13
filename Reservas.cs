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

        private void btnMostrar_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Abrimos la conexión a la base de datos
                conexionBD.Open();

                // Crear el comando con la consulta SQL
                string query = @"
            SELECT r.IDReservas, r.FechaReserva, r.FechaInicio, r.FechaFin, r.MenuSeleccionado, r.CantidadAsistentes, r.TotalPago, 
                   c.Nombre, c.Apellido, c.Correo, c.Telefono
            FROM reservas r
            JOIN clientes c ON r.IdClientes = c.IDClientes;
        ";

                MySqlDataAdapter Mostrar = new MySqlDataAdapter(query, conexionBD);
                DataTable Datos = new DataTable(); // Creamos una tabla donde guardaremos los resultados

                // Llenamos el DataTable con los datos de la consulta
                Mostrar.Fill(Datos);

                // Asignamos los datos al DataGridView
                MostrarReservas.DataSource = Datos; // 'MostrarReservas' es el DataGridView

                // Ajustamos las columnas y filas para que se ajusten automáticamente
                MostrarReservas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells; // Ajusta las columnas según el contenido
                MostrarReservas.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;     // Ajusta las filas según el contenido

                // (Opcional) Ajustar el ancho de las columnas para que no se corten las cabeceras
                foreach (DataGridViewColumn columnas in MostrarReservas.Columns)
                {
                    columnas.SortMode = DataGridViewColumnSortMode.NotSortable; // Deshabilita la capacidad de ordenar si no se necesita
                }

                MessageBox.Show("Datos cargados correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conexionBD.Close(); // Cerramos la conexión
            }
        }
    }
}