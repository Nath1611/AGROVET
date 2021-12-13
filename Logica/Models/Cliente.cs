using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.Models
{
    public class Cliente : ICrudBase, IPersona
    {
        //IPersona
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }

        //ICrudBase
        public bool Agregar()
        {
            throw new NotImplementedException();
        }

        public bool Editar()
        {
            throw new NotImplementedException();
        }

        public bool Eliminar()
        {
            throw new NotImplementedException();
        }
        //Atributos específicos
        public string IDCliente { get; set; }
        public string Direccion { get; set; }

    }
}
