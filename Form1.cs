using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient; //Para conectar la BD a Visual


namespace Clave5_Grupo6
{
    public partial class FormClientes : Form

    {

        //Creacion de variables estaticas de clase 

        static string Servidor = "localhost";               //Nombre del servidor de MySQL
        static string BD = "clave5_grupodetrabajodb6";      //Nombre de la base de datos
        static string Usuario = "root";                     //Usuario de acceso a MySQL
        static string Paswoord = "root";                    //Contraseña de usuario de acceso a MySQL


        //Cadena de conexión concatenando las variables
        static string cadenaConexion = "Database=" + BD + "; Data Source=" + Servidor + "; User Id = " + Usuario + "; Password=" + Paswoord + "";

        //Instancia para conexión a MySQL, recibe la cadena de conexión
        static MySqlConnection conexionBD = new MySqlConnection(cadenaConexion);

        //Lista para captar los nombres de asistentes 
        private List<string> asistentes;

        public FormClientes()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            CargarDatosSalas();     //Muestra la informacion de Salasa
        }

        //Botones 

        /*Boton para poder comprobar la conexion a la base de datos */
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                conexionBD.Open(); //se abre la conexion de la variable global declara en la parte superior del formulario
                MessageBox.Show("Conexión exitosa!", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);    // se manda un mensaje de estado de conexion
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

        //Boton para cerrar el Formulario de Registro Clientes 
        private void btnCerrar_Click(object sender, EventArgs e)

        {   
            DialogResult resultado = MessageBox.Show("¿Estás seguro de que quieres salir?", "Confirmar sálida", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resultado == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
            
        //Boton para llamar a Formulario Reserva
        private void button2_Click(object sender, EventArgs e)
        {
            // Crear una nueva instancia del formulario 'Reservas'
            Reservas reservasForm = new Reservas();

            // Cerrar el formulario actual (Form1)
            this.Hide();

            // Mostrar el formulario 'Reservas'
            reservasForm.Show();
        }

        //Boton para realizar la Reservacion 
        private void btnReserva_Click(object sender, EventArgs e)
        {
            // Variable para almacenar los mensajes de error
            StringBuilder errores = new StringBuilder();

            //Validacion de Datos Personales 

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

            // Validación de salas
            if (cmbSalas.SelectedIndex == -1)  // Si no se ha seleccionado ninguna sala
            {
                errores.AppendLine("Por favor, selecciona una sala para realizar la reserva.");
            }

            // Validación de las fecha y horas
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
                errores.AppendLine("Por favor, ingresa una cantidad válida de asistentes.");
            }

            //Validacion de Menú 

            int menu1Personas = (int)numeric1.Value;
            int menu2Personas = (int)numeric2.Value;
            int menu3Personas = (int)numeric3.Value;

            int totalSeleccionado = menu1Personas + menu2Personas + menu3Personas;

            if (totalSeleccionado != totalPersona)  // Si la suma no es igual al total de personas
            {
                errores.AppendLine("La cantidad de personas que han elegido los menús no coincide con la cantidad total de personas que han sido ingresadas.");
            }

            // Si hay errores, mostramos todos

            if (errores.Length > 0)
            {
                MessageBox.Show(errores.ToString(), "Errores de datos de entrada", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Si el error está en la cantidad de personas, borrar el dato y enfocar el campo
                if (string.IsNullOrWhiteSpace(txtPersonas.Text) || !int.TryParse(txtPersonas.Text, out totalPersona) || totalPersona <= 0)
                {
                    txtPersonas.Clear(); 
                    txtPersonas.Focus();  
                }
                return;
            }

            // Crear la reserva con los datos del formulario

            int salaSeleccionada = Convert.ToInt32(cmbSalas.SelectedItem);
            DateTime fechaReserva = Fecha.Value;
            TimeSpan horaInicio = HoraInicio.Value.TimeOfDay;
            TimeSpan horaFin = HoraFin.Value.TimeOfDay;

            // Llamar a la función para verificar disponibilidad de salas
            if (!ActualizarDisponibilidadSala(salaSeleccionada, fechaReserva, horaInicio, horaFin))
            {
                MessageBox.Show("La sala no está disponible en el horario seleccionado. Por favor, elija otro horario.");
                return; // Detener el proceso si la sala está ocupada
            }


            // Crear el cliente con los datos ingresados
            Cliente cliente = new Cliente(txtClienteNombre.Text, txtApellido.Text, txtCorreoel.Text, TxtTelefonos.Text);

            // Insertar el cliente y obtener su ID
            int clienteId = InsertarCliente(cliente);

            // Verificar si el ID es válido
            if (clienteId == -1)
            {
                MessageBox.Show("No se pudo insertar el cliente. La operación fue cancelada.");
                return;
            }

            // Asignar el ID al cliente después de la inserción
            cliente.ID = clienteId;

            //Crear la cadena de los menús seleccionados
            string menuSeleccionado = "";
            if (menu1Personas > 0)
            {
                menuSeleccionado += $"Menú 1: {menu1Personas} persona(s)\n";
            }
            if (menu2Personas > 0)
            {
                menuSeleccionado += $"Menú 2: {menu2Personas} persona(s)\n";
            }
            if (menu3Personas > 0)
            {
                menuSeleccionado += $"Menú 3: {menu3Personas} persona(s)\n";
            }

            //Convertir la lista de asistentes a una cadena separada por comas

            string asistentesString = string.Join(",", asistentes);  // Lista de asistentes convertida a una cadena

            //Crear la reserva con los datos del formulario, incluyendo los menús seleccionados y los asistentes

            Reserva reserva = new Reserva(
                0,  // ID de la reserva 
                cliente,  // El cliente recién insertado
                new Sala(salaSeleccionada, clienteId, "", "", true, false, false, false),  // Sala seleccionada
                fechaReserva,  // Fecha de la reserva
                horaInicio,  // Hora de inicio
                horaFin,  // Hora de fin
                menuSeleccionado,  // Asignar la cadena de menús seleccionados
                totalSeleccionado,  // Total de personas para todos los menús
                asistentes  // Lista de asistentes
            );

            // Asignar precios a los menús según el número de personas que los seleccionaron
            decimal precioMenu1 = 10.00m;  // Precio por persona para el Menú 1
            decimal precioMenu2 = 12.00m;  // Precio por persona para el Menú 2
            decimal precioMenu3 = 15.00m;  // Precio por persona para el Menú 3

            decimal total = (menu1Personas * precioMenu1) + (menu2Personas * precioMenu2) + (menu3Personas * precioMenu3);

            // Asignar el total calculado a la reserva
            reserva.TotalPago = total;

            // Mostrar el total en el txtTotal
            txtTotal.Text = reserva.TotalPago.ToString("C");

           // Verifica si el ID del cliente es válido antes de insertar la reserva
            if (reserva.Cliente.ID == 0)
            {
                MessageBox.Show("Error: El cliente no tiene un ID válido.");
                return;
            }

            InsertarReserva(reserva);

            LimpiarFormulario();
        }


        //Validaciones 

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

        private void txtPersonas_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo números enteros positivos
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8) // 8 es la tecla "Backspace"
            {
                e.Handled = true;
            }
        }

        //Metodos necesarios 

        private int InsertarCliente(Cliente cliente)
        {
            try
            {
                // Crear la consulta SQL para insertar el cliente
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

                    // Ejecutar la consulta para insertar el cliente
                    cmd.ExecuteNonQuery();

                    // Recuperar el ID del cliente recién insertado
                    cmd.CommandText = "SELECT LAST_INSERT_ID()";  // Obtener el último ID insertado
                    int clienteId = Convert.ToInt32(cmd.ExecuteScalar()); // Ejecuta el query y obtiene el ID

                    // Asignar el ID al objeto cliente
                    cliente.ID = clienteId;

                    // Confirmación de inserción exitosa
                    MessageBox.Show("Cliente insertado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Retornar el ID del cliente recién insertado
                    return clienteId;
                }
            }
            catch (Exception ex)
            {
                // Mostrar un mensaje de error si algo falla
                MessageBox.Show("Error al insertar el cliente: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;  // Retornar -1 si ocurre un error
            }
            finally
            {
                // Cerrar la conexión si está abierta
                if (conexionBD.State == ConnectionState.Open)
                {
                    conexionBD.Close();
                }
            }
        }

        private void InsertarReserva(Reserva reserva)
        {
            try
            {
                // Consulta SQL para insertar la reserva
                string query = "INSERT INTO reservas (IdClientes, IdSala, FechaReserva, FechaInicio, FechaFin, MenuSeleccionado, CantidadAsistentes, Asistentes, TotalPago) " +
                               "VALUES (@IdClientes, @IdSala, @FechaReserva, @FechaInicio, @FechaFin, @MenuSeleccionado, @CantidadAsistentes, @Asistentes, @TotalPago)";

                using (MySqlCommand cmd = new MySqlCommand(query, conexionBD))
                {
                    // Parámetros para la consulta
                    cmd.Parameters.AddWithValue("@IdClientes", reserva.Cliente.ID);
                    cmd.Parameters.AddWithValue("@IdSala", reserva.Sala.ID);
                    cmd.Parameters.AddWithValue("@FechaReserva", reserva.FechaReserva);
                    cmd.Parameters.AddWithValue("@FechaInicio", reserva.FechaReserva.Add(reserva.HoraInicio));
                    cmd.Parameters.AddWithValue("@FechaFin", reserva.FechaReserva.Add(reserva.HoraFin));
                    cmd.Parameters.AddWithValue("@MenuSeleccionado", reserva.MenuSeleccionado);
                    cmd.Parameters.AddWithValue("@CantidadAsistentes", reserva.CantidadAsistentes);
                    cmd.Parameters.AddWithValue("@Asistentes", string.Join(",", reserva.Asistentes)); // Unir los asistentes en una cadena
                    cmd.Parameters.AddWithValue("@TotalPago", reserva.TotalPago);

                    // Abrir la conexión
                    conexionBD.Open();

                    // Ejecutar la consulta
                    cmd.ExecuteNonQuery();

                    // Confirmar el éxito
                    MessageBox.Show("Reserva registrada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al insertar la reserva: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conexionBD.State == ConnectionState.Open)
                    conexionBD.Close();
            }
        }
        private bool ActualizarDisponibilidadSala(int idSala, DateTime fechaReserva, TimeSpan horaInicio, TimeSpan horaFin)
        {
            try
            {
                // Convertir TimeSpan a DateTime (solo para la hora, conservando la fecha original)

                DateTime fechaHoraInicio = fechaReserva.Date.Add(horaInicio);  // Combina la fecha con la hora de inicio
                DateTime fechaHoraFin = fechaReserva.Date.Add(horaFin);  // Combina la fecha con la hora de fin

                // Consulta SQL ajustada para verificar horas
                string query = @"SELECT COUNT(*) 
                 FROM reservas 
                 WHERE IdSala = @idSala 
                 AND ((FechaReserva = @fechaReserva 
                       AND (FechaInicio < @horaFin AND FechaFin > @horaInicio)) 
                      OR FechaReserva > @fechaReserva)";

                using (MySqlCommand cmd = new MySqlCommand(query, conexionBD))
                {
                    // Parámetros de la consulta
                    cmd.Parameters.AddWithValue("@idSala", idSala);
                    cmd.Parameters.AddWithValue("@fechaReserva", fechaReserva.Date);  // Solo la fecha sin la hora
                    cmd.Parameters.AddWithValue("@horaInicio", fechaHoraInicio);  // Usamos DateTime con hora
                    cmd.Parameters.AddWithValue("@horaFin", fechaHoraFin);  // Usamos DateTime con hora

                    // Abrir la conexión
                    using (conexionBD)  // Usar 'using' para asegurar que la conexión se cierre automáticamente
                    {
                        conexionBD.Open();

                        // Ejecutar la consulta
                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        // Si count > 0, significa que hay una reserva con horario ocupado 
                        return count == 0;  // Si no hay solapamiento, devolver true
                    }
                }
            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error si hay un problema al verificar la disponibilidad
                MessageBox.Show("Error al verificar la disponibilidad de la sala: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
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

                // Llenar el DataGridView con los datos de las salas
                da.Fill(dt);

                // Llenar el ComboBox con los números de las salas (1, 2, 3)
                cmbSalas.Items.Clear(); // Limpiar primero el ComboBox
                foreach (DataRow row in dt.Rows)
                {
                    // Agregar solo el número de sala (IDSalas)
                    cmbSalas.Items.Add(row["IDSalas"]);
                }

                // Llenar el DataGridView con los datos de todas las salas
                InfoSalas.DataSource = dt;

                // Ajustar el tamaño de las columnas automáticamente 
                InfoSalas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                // Ajustar el tamaño de las filas automáticamente
                InfoSalas.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar las salas: " + ex.Message);
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
                object result = cmd.ExecuteScalar(); // Obtener la capacidad
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
                string nombreAsistente = Microsoft.VisualBasic.Interaction.InputBox($"Ingrese el nombre del asistente {i + 1}:", "Asistentes", "");

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


        private void LimpiarFormulario()
        {
            // Limpiar los TextBox
            txtClienteNombre.Clear();
            txtApellido.Clear();
            txtCorreoel.Clear();
            TxtTelefonos.Clear();
            txtPersonas.Clear();
            txtTotal.Clear();

            // Limpiar ComboBox
            cmbSalas.SelectedIndex = -1;

            // Limpiar DateTimePicker
            Fecha.Value = DateTime.Now;
            HoraInicio.Value = DateTime.Now;
            HoraFin.Value = DateTime.Now;

            // Limpiar NumericUpDown
            numeric1.Value = 0;
            numeric2.Value = 0;
            numeric3.Value = 0;

            // Limpiar ListBox 
            asistentes.Clear();

        }
    }
}