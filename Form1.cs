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

        // Lista temporal de asistentes
        private List<string> asistentes = new List<string>();

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
        // Validar que los campos no estén vacíos y que el formato sea correcto
        private bool ValidarCampos()
        {
            // Validar Nombre
            if (string.IsNullOrWhiteSpace(txtClienteNombre.Text))
            {
                MessageBox.Show("El nombre no puede estar vacío.");
                return false;
            }
            if (!txtClienteNombre.Text.All(c => Char.IsLetter(c) || Char.IsWhiteSpace(c) || "áéíóúÁÉÍÓÚ".Contains(c)))
            {
                MessageBox.Show("El nombre solo debe contener letras y acentos.");
                return false;
            }

            // Validar Apellido
            if (string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                MessageBox.Show("El apellido no puede estar vacío.");
                return false;
            }
            if (!txtApellido.Text.All(c => Char.IsLetter(c) || Char.IsWhiteSpace(c) || "áéíóúÁÉÍÓÚ".Contains(c)))
            {
                MessageBox.Show("El apellido solo debe contener letras y acentos.");
                return false;
            }

            // Validar Correo
            if (string.IsNullOrWhiteSpace(txtCorreoel.Text))
            {
                MessageBox.Show("El correo electrónico no puede estar vacío.");
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(txtCorreoel.Text);
                if (addr.Address != txtCorreoel.Text)
                {
                    MessageBox.Show("El correo electrónico no es válido.");
                    return false;
                }
            }
            catch
            {
                MessageBox.Show("El correo electrónico no es válido.");
                return false;
            }

            // Validar Teléfono
            if (string.IsNullOrWhiteSpace(TxtTelefonos.Text))
            {
                MessageBox.Show("El teléfono no puede estar vacío.");
                return false;
            }

            // Asegurarse de que el formato sea 1212-1212 (MaskedTextBox)
            if (TxtTelefonos.MaskFull == false)
            {
                MessageBox.Show("El formato del teléfono no es válido.");
                return false;
            }

            return true;
        }

        // Validar si la hora de fin es posterior a la hora de inicio
        private bool ValidarHoras()
        {
            if (HoraInicio.Value >= HoraFin.Value)
            {
                MessageBox.Show("La hora de fin debe ser posterior a la hora de inicio.");
                return false;
            }
            return true;
        }

        // Validar que la cantidad de asistentes no exceda la capacidad de la sala
        private bool ValidarCantidadAsistentes()
        {
            int cantidadAsistentes = Convert.ToInt32(txtPersonas.Text);
            Sala salaSeleccionada = (Sala)cmbSalas.SelectedItem;

            if (cantidadAsistentes < 0)
            {
                MessageBox.Show("La cantidad de asistentes no puede ser negativa.");
                return false;
            }
            if (cantidadAsistentes > salaSeleccionada.Capacidad)
            {
                MessageBox.Show("La cantidad de asistentes excede la capacidad de la sala.");
                return false;
            }
            return true;
        }

        // Validar que el total de asistentes por los menús coincida con la cantidad de asistentes
        private void ValidarMenus()
        {
            int cantidadAsistentes = Convert.ToInt32(txtPersonas.Text);
            int menu1 = (int)numeric1.Value;
            int menu2 = (int)numeric2.Value;
            int menu3 = (int)numeric3.Value;

            // Verificar que la cantidad total de personas en los menús coincida con la cantidad de asistentes
            if (menu1 + menu2 + menu3 != cantidadAsistentes)
            {
                MessageBox.Show("El total de personas seleccionadas para los menús debe coincidir con la cantidad de asistentes.");
                numeric1.Value = 0;
                numeric2.Value = 0;
                numeric3.Value = 0;
                return;
            }

            // Si todo está bien, el total se calculará al crear la reserva
            txtTotal.Text = ""; // Limpiar el campo de total hasta que todo esté validado
        }

        // Capturar los nombres de los asistentes
        private void btnPersonas_Click(object sender, EventArgs e)
        {
            asistentes.Clear(); // Limpiar lista de asistentes al comenzar un nuevo proceso

            int cantidadAsistentes = Convert.ToInt32(txtPersonas.Text);

            // Validar que la cantidad de asistentes sea un número entero positivo
            if (cantidadAsistentes <= 0)
            {
                MessageBox.Show("La cantidad de asistentes debe ser mayor a cero.");
                return;
            }

            // Pedir los nombres de los asistentes
            for (int i = 0; i < cantidadAsistentes; i++)
            {
                string nombreAsistente = Microsoft.VisualBasic.Interaction.InputBox($"Ingrese el nombre del asistente {i + 1}:");

                // Validar nombre
                if (string.IsNullOrWhiteSpace(nombreAsistente) || !nombreAsistente.All(c => Char.IsLetter(c) || "áéíóúÁÉÍÓÚ".Contains(c)))
                {
                    MessageBox.Show("El nombre del asistente solo debe contener letras y acentos.");
                    return;
                }

                asistentes.Add(nombreAsistente); // Agregar el nombre a la lista temporal
            }

            MessageBox.Show("Nombres de asistentes guardados temporalmente.");
        }

        // Método para guardar la reserva en la base de datos
        private void GuardarReservaEnBaseDeDatos(Reserva reserva)
        {
            using (MySqlConnection conn = new MySqlConnection("TuCadenaDeConexion"))
            {
                conn.Open();

                // Insertar Cliente
                string queryCliente = "INSERT INTO clientes (Nombre, Apellido, Correo, Telefono) VALUES (@Nombre, @Apellido, @Correo, @Telefono)";
                using (MySqlCommand cmd = new MySqlCommand(queryCliente, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", reserva.Cliente.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", reserva.Cliente.Apellido);
                    cmd.Parameters.AddWithValue("@Correo", reserva.Cliente.Correo);
                    cmd.Parameters.AddWithValue("@Telefono", reserva.Cliente.Telefono);
                    cmd.ExecuteNonQuery();
                }

                // Insertar Reserva
                string queryReserva = "INSERT INTO reservas (IdClientes, IdSala, FechaReserva, FechaInicio, FechaFin, MenuSeleccionado, CantidadAsistentes, Asistentes, TotalPago) VALUES (@IdClientes, @IdSala, @FechaReserva, @FechaInicio, @FechaFin, @MenuSeleccionado, @CantidadAsistentes, @Asistentes, @TotalPago)";
                using (MySqlCommand cmd = new MySqlCommand(queryReserva, conn))
                {
                    cmd.Parameters.AddWithValue("@IdClientes", reserva.Cliente.ID); // Se debe recuperar el ID del cliente después de insertarlo
                    cmd.Parameters.AddWithValue("@IdSala", reserva.Sala.ID);
                    cmd.Parameters.AddWithValue("@FechaReserva", reserva.FechaReserva);
                    cmd.Parameters.AddWithValue("@FechaInicio", reserva.HoraInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", reserva.HoraFin);
                    cmd.Parameters.AddWithValue("@MenuSeleccionado", reserva.MenuSeleccionado);
                    cmd.Parameters.AddWithValue("@CantidadAsistentes", reserva.CantidadAsistentes);
                    cmd.Parameters.AddWithValue("@Asistentes", string.Join(",", reserva.Asistentes)); // Convertir lista de asistentes a un string
                    cmd.Parameters.AddWithValue("@TotalPago", reserva.TotalPago); // Se inserta el total calculado en la clase Reserva
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Evento de clic para reservar
        private void btnReserva_Click(object sender, EventArgs e)
        {
            // Validar todos los campos antes de proceder
            if (ValidarCampos() && ValidarHoras() && ValidarCantidadAsistentes())
            {
                // Validar menús
                ValidarMenus();

                // Verificar que los valores de los menús sean correctos
                int asistentesMenu1 = (int)numeric1.Value;
                int asistentesMenu2 = (int)numeric2.Value;
                int asistentesMenu3 = (int)numeric3.Value;

                if (asistentesMenu1 + asistentesMenu2 + asistentesMenu3 != Convert.ToInt32(txtPersonas.Text))
                {
                    MessageBox.Show("La suma de los asistentes de cada menú no coincide con la cantidad de asistentes.");
                    return;
                }

                // Crear el objeto Cliente con los datos ingresados
                Cliente cliente = new Cliente
                {
                    Nombre = txtClienteNombre.Text,
                    Apellido = txtApellido.Text,
                    Correo = txtCorreoel.Text,
                    Telefono = TxtTelefonos.Text
                };

                // Crear el objeto Sala
                Sala salaSeleccionada = (Sala)cmbSalas.SelectedItem;

                // Crear la reserva
                Reserva reserva = new Reserva
                {
                    Cliente = cliente,
                    Sala = salaSeleccionada,
                    FechaReserva = Fecha.Value,
                    HoraInicio = HoraInicio.Value.TimeOfDay,
                    HoraFin = HoraFin.Value.TimeOfDay,
                    MenuSeleccionado = GetMenuSeleccionado(), // Aquí debes establecer cómo obtener el menú seleccionado
                    CantidadAsistentes = Convert.ToInt32(txtPersonas.Text),
                    Asistentes = asistentes, // Lista de asistentes recogida anteriormente
                };

                // Calcular el total usando el método de la clase Reserva
                reserva.CalcularTotal();

                // Mostrar el total calculado en el TextBox
                txtTotal.Text = reserva.TotalPago.ToString("C2"); // Mostrar total en formato moneda

                // Guardar en la base de datos
                try
                {
                    GuardarReservaEnBaseDeDatos(reserva);
                    MessageBox.Show("Reserva registrada con éxito.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al registrar la reserva: " + ex.Message);
                }
            }
        }

        // Mostrar la información de la sala seleccionada en el DataGridView
        private void cmbSalas_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Obtén la sala seleccionada
            Sala salaSeleccionada = (Sala)cmbSalas.SelectedItem;

            // Muestra la información de la sala en el DataGridView
            var salaInfo = new List<Sala> { salaSeleccionada };
            InfoSalas.DataSource = salaInfo; // Asumiendo que InfoSalas tiene como fuente las salas
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
