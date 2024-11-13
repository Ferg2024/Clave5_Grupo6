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
        private List<string> asistentes;

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

            // Validación de la cantidad de personas
            int totalPersona = 0;
            if (string.IsNullOrWhiteSpace(txtPersonas.Text) || !int.TryParse(txtPersonas.Text, out totalPersona) || totalPersona <= 0)
            {
                errores.AppendLine("Por favor, ingresa un número válido de personas.");
            }

            //
            int menu1Personas = (int)numeric1.Value;
            int menu2Personas = (int)numeric2.Value;
            int menu3Personas = (int)numeric3.Value;

            int totalSeleccionado = menu1Personas + menu2Personas + menu3Personas;

            if (totalSeleccionado != totalPersona)  // Si la suma no es igual al total de personas
            {
                errores.AppendLine("La suma de las personas seleccionadas para los menús no coincide con la cantidad total de personas.");
            }

            // Si hay errores, mostramos todos

            if (errores.Length > 0)
            {
                MessageBox.Show(errores.ToString(), "Errores de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Si el error está en la cantidad de personas, borrar el dato y enfocar el campo
                if (string.IsNullOrWhiteSpace(txtPersonas.Text) || !int.TryParse(txtPersonas.Text, out totalPersona) || totalPersona <= 0)
                {
                    txtPersonas.Clear();  // Borrar el dato
                    txtPersonas.Focus();  // Establecer el foco en el campo
                }

                return;
            }

            // Crear la lista de asistentes
            List<string> asistentes = new List<string>();
            for (int i = 0; i < totalPersona; i++)
            {
                string asistente = Microsoft.VisualBasic.Interaction.InputBox($"Ingrese el nombre del asistente {i + 1}:", "Nombre del Asistente");
                if (string.IsNullOrWhiteSpace(asistente))
                {
                    MessageBox.Show("El nombre del asistente no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                asistentes.Add(asistente);
            }


            // Crear el cliente con los datos ingresados
            Cliente cliente = new Cliente(txtClienteNombre.Text, txtApellido.Text, txtCorreoel.Text, TxtTelefonos.Text);

            // Insertar el cliente en la base de datos
            InsertarCliente(cliente);


        }

        private void InsertarCliente(Cliente cliente)
        {
            try
            {
                // Crear la consulta SQL
                string query = "INSERT INTO Clientes (Nombre, Apellido, Correo, Telefono) " +
                               "VALUES (@Nombre, @Apellido, @Correo, @Telefono)";

                // Crear el comando SQL con la conexión a la base de datos
                using (MySqlCommand cmd = new MySqlCommand(query, conexionBD))
                {
                    // Añadir los parámetros con los valores del cliente
                    cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", cliente.Apellido);
                    cmd.Parameters.AddWithValue("@Correo", cliente.Correo);
                    cmd.Parameters.AddWithValue("@Telefono", cliente.Telefono);

                    // Abrir la conexión
                    conexionBD.Open();

                    // Ejecutar la consulta
                    cmd.ExecuteNonQuery();

                    // Mostrar un mensaje indicando que la inserción fue exitosa
                    MessageBox.Show("Cliente insertado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // Mostrar un mensaje de error si algo falla
                MessageBox.Show("Error al insertar el cliente: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Cerrar la conexión, si está abierta
                if (conexionBD.State == ConnectionState.Open)
                {
                    conexionBD.Close();
                }
            }
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


        // Método para obtener la capacidad de la sala seleccionada desde la base de datos
        private int ObtenerCapacidadSala(int idSala)
        {
            // Consulta SQL para obtener la capacidad de la sala seleccionada
            string query = "SELECT Capacidad FROM salas WHERE IDSalas = @idSala";
            MySqlCommand cmd = new MySqlCommand(query, conexionBD);
            cmd.Parameters.AddWithValue("@idSala", idSala);

            try
            {
                conexionBD.Open();
                object result = cmd.ExecuteScalar(); // Obtener solo un valor (la capacidad)
                return result != null ? Convert.ToInt32(result) : 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener la capacidad: " + ex.Message);
                return 0;
            }
            finally
            {
                conexionBD.Close();
            }
        }

        private void btnPersonas_Click(object sender, EventArgs e)
        {
            // Validar que se haya seleccionado una sala
            if (cmbSalas.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor, selecciona una sala.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validar que la cantidad de personas sea un número entero mayor que 0
            if (string.IsNullOrWhiteSpace(txtPersonas.Text) || Convert.ToInt32(txtPersonas.Text) <= 0)
            {
                MessageBox.Show("Por favor, ingresa un número válido de personas.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int cantidadPersonas = Convert.ToInt32(txtPersonas.Text);

            // Obtener la sala seleccionada
            int salaSeleccionada = Convert.ToInt32(cmbSalas.SelectedItem);

            // Obtener la capacidad de la sala
            int capacidadSala = ObtenerCapacidadSala(salaSeleccionada);

            // Validar que la cantidad de personas no exceda la capacidad de la sala
            if (cantidadPersonas > capacidadSala)
            {
                MessageBox.Show($"La capacidad máxima de esta sala es {capacidadSala} personas.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Si la validación pasó, pedimos los nombres de los asistentes
            asistentes = new List<string>();  // Limpiamos la lista antes de empezar

            for (int i = 0; i < cantidadPersonas; i++)
            {
                string nombreAsistente = Microsoft.VisualBasic.Interaction.InputBox($"Ingrese el nombre del asistente {i + 1}:", "Nombre del Asistente", "");

                // Validar que el nombre no esté vacío y que solo contenga letras y acentos
                if (string.IsNullOrWhiteSpace(nombreAsistente) || !nombreAsistente.All(c => Char.IsLetter(c) || "áéíóúÁÉÍÓÚñÑ".Contains(c)))
                {
                    MessageBox.Show("Por favor, ingrese un nombre válido (solo letras y acentos).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    i--;  // Volver a pedir el nombre
                    continue;
                }

                asistentes.Add(nombreAsistente);  // Agregar el nombre a la lista de asistentes
            }

            MessageBox.Show("La lista de asistentes se ha guardado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

        } 
    }
}