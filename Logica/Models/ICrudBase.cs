using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.Models
{
    //Encapsula los métodos que comparten las clases
    public interface ICrudBase
    {
        bool Agregar();
        bool Editar();
        bool Eliminar();
    }
}
