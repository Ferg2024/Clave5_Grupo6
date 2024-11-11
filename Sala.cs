using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clave5_Grupo6
{
    public class Sala : Entidad
    {
        // Atributos específicos de Sala
        public int Capacidad { get; set; }
        public string Ubicacion { get; set; }
        public string Distribucion { get; set; }
        public bool Disponibilidad { get; set; }  // true si está disponible
        public bool Proyector { get; set; }
        public bool Oasis { get; set; }
        public bool Cafetera { get; set; }

        // Constructor que llama al constructor de la clase base (Entidad)
        public Sala(int id, string nombre, int capacidad, string ubicacion, string distribucion, bool disponibilidad, bool proyector, bool oasis, bool cafetera)
            : base(id, nombre)
        {
            Capacidad = capacidad;
            Ubicacion = ubicacion;
            Distribucion = distribucion;
            Disponibilidad = disponibilidad;
            Proyector = proyector;
            Oasis = oasis;
            Cafetera = cafetera;
        }
    }
}
