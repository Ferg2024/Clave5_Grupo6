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
    public int MenuSeleccionado { get; set; }  // 1, 2 o 3
    public int CantidadAsistentes { get; set; }
    public List<string> Asistentes { get; set; }
    public decimal TotalPago { get; set; }

    // Constructor que ya no recibe el totalPago
    public Reserva(int id, Cliente cliente, Sala sala, DateTime fechaReserva, TimeSpan horaInicio, TimeSpan horaFin, int menuSeleccionado, int cantidadAsistentes, List<string> asistentes)
        : base(id, cliente.Nombre)  // Usamos el nombre del cliente como nombre de la reserva
    {
        Cliente = cliente;
        Sala = sala;
        FechaReserva = fechaReserva;
        HoraInicio = horaInicio;
        HoraFin = horaFin;
        MenuSeleccionado = menuSeleccionado;
        CantidadAsistentes = cantidadAsistentes;
        Asistentes = asistentes;
        TotalPago = 0.0m;  // Lo inicializas en 0.0M porque lo vamos a calcular después
    }

    // Método para calcular el total a pagar por la reserva
    public void CalcularTotal()
    {
        decimal precioMenu = 0;

        // Definir los precios según el menú seleccionado
        switch (MenuSeleccionado)
        {
            case 1:
                precioMenu = 10.00m; // Precio por persona para el Menú 1
                break;
            case 2:
                precioMenu = 12.00m; // Precio por persona para el Menú 2
                break;
            case 3:
                precioMenu = 15.00m; // Precio por persona para el Menú 3
                break;
        }

        // Calcular el total
        TotalPago = precioMenu * CantidadAsistentes;
    }
    }
}
