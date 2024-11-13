using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clave5_Grupo6
{
    public class Reserva : Entidad
    {
    // Atributos específicos de Reserva
    public Cliente Cliente { get; set; }
    public Sala Sala { get; set; }
    public DateTime FechaReserva { get; set; }
    public TimeSpan HoraInicio { get; set; }  // Cambiado a TimeSpan para trabajar solo con horas
    public TimeSpan HoraFin { get; set; }     // Cambiado a TimeSpan para trabajar solo con horas
    public string MenuSeleccionado { get; set; } 
    public int CantidadAsistentes { get; set; }
    public List<string> Asistentes { get; set; }
    public decimal TotalPago { get; set; }

    // Constructor
    public Reserva(int id, Cliente cliente, Sala sala, DateTime fechaReserva, TimeSpan horaInicio, TimeSpan horaFin, string menuSeleccionado, int cantidadAsistentes, List<string> asistentes)
        : base(id, cliente.Nombre)
    {
        Cliente = cliente;
        Sala = sala;
        FechaReserva = fechaReserva;
        HoraInicio = horaInicio;
        HoraFin = horaFin;
        MenuSeleccionado = menuSeleccionado;
        CantidadAsistentes = cantidadAsistentes;
        Asistentes = asistentes;
        TotalPago = 0.0m; 
    }

        public void CalcularTotal()
        {
            decimal precioMenu = 0;

            // Convertimos el string de menús seleccionados en un arreglo de strings
            string[] menues = MenuSeleccionado.Split(','); // Split para separar por coma

            foreach (var menu in menues)
            {
                switch (menu.Trim()) // Trim para quitar espacios extra
                {
                    case "1":
                        precioMenu += 10.00m; // Precio por persona para el Menú 1
                        break;
                    case "2":
                        precioMenu += 12.00m; // Precio por persona para el Menú 2
                        break;
                    case "3":
                        precioMenu += 15.00m; // Precio por persona para el Menú 3
                        break;
                    default:
                        // Si el menú no está definido (por ejemplo, un valor inesperado), podríamos manejarlo con un mensaje de error.
                        throw new Exception("Menú seleccionado no válido.");
                }
            }

            // Calcular el total a pagar
            TotalPago = precioMenu * CantidadAsistentes;
        }
    }
}
