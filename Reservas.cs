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

        private void btnMostrar_Click(object sender, EventArgs e)
        {
            MostrarDatos(); // Llamamos al método que carga los datos en el DataGridView
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            // Verificar si se ha seleccionado una fila en el DataGridView
            if (MostrarReservas.SelectedRows.Count > 0)
            {
                // Obtener el ID de la reserva de la fila seleccionada
                int idReserva = Convert.ToInt32(MostrarReservas.SelectedRows[0].Cells["IDReservas"].Value);

                // Confirmar la eliminación
                DialogResult dialogResult = MessageBox.Show("¿Estás seguro de que deseas eliminar esta reserva?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    EliminarReserva(idReserva);  // Llamar al método para eliminar la reserva
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una fila para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void EliminarReserva(int idReserva)
        {
            MySqlConnection conexionBD = new MySqlConnection(cadenaConexion); // Nueva conexión

            try
            {
                // Abrir la conexión
                conexionBD.Open();

                // Consulta SQL para eliminar la reserva
                string query = "DELETE FROM reservas WHERE IDReservas = @IDReservas";

                // Crear el comando para ejecutar la consulta
                MySqlCommand cmd = new MySqlCommand(query, conexionBD);

                // Agregar el parámetro para el ID de la reserva
                cmd.Parameters.AddWithValue("@IDReservas", idReserva);

                // Ejecutar la consulta
                cmd.ExecuteNonQuery();

                // Confirmar que la reserva fue eliminada
                MessageBox.Show("Reserva eliminada correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Recargar los datos en el DataGridView
                MostrarDatos();  // Volvemos a cargar los datos actualizados
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar la reserva: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conexionBD.Close();  // Cerramos la conexión
            }
        }

        private void MostrarDatos()
        {
            MySqlConnection conexionBD = new MySqlConnection(cadenaConexion); // Nueva conexión

            try
            {
                // Abrir la conexión
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
                MostrarReservas.DataSource = Datos;

                // Ajustamos las columnas y filas para que se ajusten automáticamente
                MostrarReservas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                MostrarReservas.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                //Ajustar el ancho de las columnas para que no se corten las cabeceras
                foreach (DataGridViewColumn columnas in MostrarReservas.Columns)
                {
                    columnas.SortMode = DataGridViewColumnSortMode.NotSortable;
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

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            // Verificar si el valor del NumericUpDown es válido (por ejemplo, no es cero)
            if (numericBuscar.Value == 0)
            {
                MessageBox.Show("Por favor, ingrese un ID de reserva válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Buscar la reserva en la base de datos
            BuscarReserva((int)numericBuscar.Value);
        }
        private void BuscarReserva(int idReserva)
        {
            try
            {
                // Abrir la conexión
                using (MySqlConnection conexionBD = new MySqlConnection(cadenaConexion))
                {
                    conexionBD.Open();

                    // Consulta SQL para buscar la reserva por ID
                    string query = @"
                SELECT c.Nombre, c.Apellido
                FROM reservas r
                JOIN clientes c ON r.IdClientes = c.IDClientes
                WHERE r.IDReservas = @IDReservas";

                    MySqlCommand cmd = new MySqlCommand(query, conexionBD);
                    cmd.Parameters.AddWithValue("@IDReservas", idReserva);

                    // Ejecutar la consulta y obtener el resultado
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        // Si se encuentra la reserva, mostrar el mensaje
                        reader.Read(); // Leer la primera fila
                        string nombreCliente = reader.GetString("Nombre");
                        string apellidoCliente = reader.GetString("Apellido");

                        // Mostrar el mensaje con el nombre del cliente
                        MessageBox.Show($"Registro encontrado: Reserva hecha por {nombreCliente} {apellidoCliente}", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // Si no se encuentra la reserva
                        MessageBox.Show("No se encontró ninguna reserva con ese ID.", "No encontrado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar la reserva: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}