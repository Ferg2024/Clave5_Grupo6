using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clave5_Grupo6
{
    public class Cliente : Entidad
    {
        // Atributos específicos de Cliente
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }

        // Constructor que llama al constructor de la clase base (Entidad)
        public Cliente(string nombre, string apellido, string correo, string telefono): base(0, nombre)
        {
            Apellido = apellido;
            Correo = correo;
            Telefono = telefono;
        }
    }
}
