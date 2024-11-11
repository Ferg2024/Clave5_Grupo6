using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clave5_Grupo6
{
   public abstract class Entidad
    {
        // Atributos comunes para todas las entidades
        public int ID { get; set; }
        public string Nombre { get; set; }

        // Constructor
        public Entidad(int id, string nombre)
        {
            ID = id;
            Nombre = nombre;
        }
    }
}
