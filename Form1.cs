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
            CargarDatosSalas();
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
        {   // Mostrar un cuadro de mensaje de confirmación con botones Yes y No
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

        private void txtClienteNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir letras, espacios, y caracteres especiales como á, é, í, ó, ú, ñ
            if (!Char.IsLetter(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != 32 &&
                e.KeyChar != 'á' && e.KeyChar != 'é' && e.KeyChar != 'í' && e.KeyChar != 'ó' && e.KeyChar != 'ú' &&
                e.KeyChar != 'Á' && e.KeyChar != 'É' && e.KeyChar != 'Í' && e.KeyChar != 'Ó' && e.KeyChar != 'Ú' &&
                e.KeyChar != 'ñ' && e.KeyChar != 'Ñ')
            {
                e.Handled = true; // Cancela el carácter no permitido
            }
        }

        private void txtApellido_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir letras, espacios, y caracteres especiales como á, é, í, ó, ú, ñ
            if (!Char.IsLetter(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != 32 &&
                e.KeyChar != 'á' && e.KeyChar != 'é' && e.KeyChar != 'í' && e.KeyChar != 'ó' && e.KeyChar != 'ú' &&
                e.KeyChar != 'Á' && e.KeyChar != 'É' && e.KeyChar != 'Í' && e.KeyChar != 'Ó' && e.KeyChar != 'Ú' &&
                e.KeyChar != 'ñ' && e.KeyChar != 'Ñ')
            {
                e.Handled = true; // Cancela el carácter no permitido
            }
        }

        private bool ValidarCorreo(string correo)
        {
            // Expresión regular para validar el formato del correo electrónico
            var regex = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");

            // Retorna true si el correo cumple con el formato, falso si no
            return regex.IsMatch(correo);
        }


        private void btnReserva_Click(object sender, EventArgs e)
        {
            // Variable para almacenar los mensajes de error
            // Variable para almacenar los mensajes de error
            StringBuilder errores = new StringBuilder();

            // Validación de nombre
            if (string.IsNullOrWhiteSpace(txtClienteNombre.Text))
            {
                errores.AppendLine("El nombre no puede estar vacío.");
            }

            // Validación de apellido
            if (string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                errores.AppendLine("El apellido no puede estar vacío.");
            }

            // Validación de correo electrónico
            if (string.IsNullOrWhiteSpace(txtCorreoel.Text))
            {
                errores.AppendLine("El correo electrónico no puede estar vacío.");
            }
            else if (!ValidarCorreo(txtCorreoel.Text))  // Validar el formato del correo
            {
                errores.AppendLine("Por favor ingrese un correo electrónico válido.");
            }

            // Validación de teléfono (MaskedTextBox)
            if (string.IsNullOrWhiteSpace(TxtTelefonos.Text) || !TxtTelefonos.MaskFull)
            {
                errores.AppendLine("El teléfono debe estar completo y no puede estar vacío.");
            }

            // Validación de la selección de sala
            if (cmbSalas.SelectedIndex == -1)  // Si no se ha seleccionado ninguna sala
            {
                errores.AppendLine("Por favor, selecciona una sala para realizar la reserva.");
            }

            // Validación de las fechas y horas
            if (Fecha.Value.Date < DateTime.Now.Date)  // Fecha debe ser hoy o futura
            {
                errores.AppendLine("La fecha seleccionada no puede ser anterior a la fecha actual.");
            }

            if (HoraInicio.Value >= HoraFin.Value)  // La hora de inicio no puede ser mayor o igual a la hora de fin
            {
                errores.AppendLine("La hora de inicio no puede ser igual o mayor que la hora de fin.");
            }

            // Si hay errores, mostramos todos
            if (errores.Length > 0)
            {
                MessageBox.Show(errores.ToString(), "Errores de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Si todo está correcto, continuar con el proceso de reserva
            int salaSeleccionada = Convert.ToInt32(cmbSalas.SelectedItem);
            DateTime fechaReserva = Fecha.Value;
            DateTime horaInicio = HoraInicio.Value;
            DateTime horaFin = HoraFin.Value;

        }

        private void CargarDatosSalas()
        {
            try
            {
                // Crear la consulta SQL para obtener las salas
                string query = "SELECT IDSalas, Ubicacion, Capacidad, Distribucion, Disponibilidad, Proyector, Oasis, Cafetera FROM salas";

                // Crear el comando SQL
                MySqlCommand cmd = new MySqlCommand(query, conexionBD);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                // Llenar el DataTable con los datos de las salas
                da.Fill(dt);

                // Llenar el ComboBox con los números de las salas (1, 2, 3)
                cmbSalas.Items.Clear(); // Limpiar primero el ComboBox
                foreach (DataRow row in dt.Rows)
                {
                    // Agregar solo el número de sala (IDSalas)
                    cmbSalas.Items.Add(row["IDSalas"]);
                }

                // Llenar el DataGridView con los datos de todas las salas
                // No necesitamos hacer conversiones adicionales, solo cargar los datos tal cual están en la base de datos
                InfoSalas.DataSource = dt;

                // Ajustar el tamaño de las columnas automáticamente (este es el paso clave)
                InfoSalas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                // Ajustar el tamaño de las filas automáticamente
                InfoSalas.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar las salas: " + ex.Message);
            }

        }

        private void txtPersonas_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo números enteros positivos
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8) // 8 es la tecla "Backspace"
            {
                e.Handled = true;
            }
        }

        private void btnPersonas_Click(object sender, EventArgs e)
        {
          
        } 
    }
}