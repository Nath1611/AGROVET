using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.Models
{
    public class Usuario : ICrudBase, IPersona
    {
        public int IDUsuario { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }

        

        public Usuario ConsultarPorID(int ID)
        {
            Usuario R = new Usuario();

            Conexion MiCnn = new Conexion();

            MiCnn.ListadoDeParametros.Add(new SqlParameter("@ID", ID));

            DataTable DatosUsuario = new DataTable();

            DatosUsuario = MiCnn.DMLSelect("SPUsuarioConsultarPorID");

            if (DatosUsuario != null && DatosUsuario.Rows.Count == 1)
            {
                DataRow Fila = DatosUsuario.Rows[0];

                R.IDUsuario = ID;
                R.Nombre = Convert.ToString(Fila["Nombre"]);
                R.Cedula = Convert.ToString(Fila["Cedula"]);
                R.Telefono = Convert.ToString(Fila["Telefono"]);
                R.Email = Convert.ToString(Fila["Email"]);
            }
            return R;
        }
        public bool ConsultarPorCedula(string cedula)
        {
            bool R = false;

            //SDUsuarioAgregar 1.3.1 y 1.3.2
            Conexion MiConexion = new Conexion();

            //En este caso y de forma didactica, se implementa un parametro para la cédula
            //este valor debe agregarse como parámetro que debe llegar hasta el SP (Procedimiento almacenado)
            MiConexion.ListadoDeParametros.Add(new SqlParameter("@Cedula", cedula));

            //SDUsuarioAgregar 1.3.3 y 1.3.4
            DataTable retorno = MiConexion.DMLSelect("SPUsuarioConsultarPorCedula");

            //SDUsuarioAgregar 1.3.5
            if (retorno != null && retorno.Rows.Count > 0)
            {
                R = true;
            }

            return R;
        }
        public bool ConsultarPorEmail(string email)
        {
            bool R = false;

            Conexion MiCnn = new Conexion();

            MiCnn.ListadoDeParametros.Add(new SqlParameter("@Email", this.Email));

            DataTable resultado = MiCnn.DMLSelect("SPUsuarioConsultarPorEmail");

            if (resultado != null && resultado.Rows.Count < 0)
            {
                R = true;
            }

            return R;
        }

        public bool Agregar()
        {
            bool R = false;

            Conexion MiCnnAdd = new Conexion();

            //PARAMETROS PARA EL SP
            MiCnnAdd.ListadoDeParametros.Add(new SqlParameter("@Cedula", this.Cedula));
            MiCnnAdd.ListadoDeParametros.Add(new SqlParameter("@Nombre", this.Nombre));
            MiCnnAdd.ListadoDeParametros.Add(new SqlParameter("@Telefono", this.Telefono));
            MiCnnAdd.ListadoDeParametros.Add(new SqlParameter("@Email", this.Email));

            int resultado = MiCnnAdd.DMLUpdateDeleteInsert("SPUsuarioAgregar");

            if (resultado > 0)
            {
                R = true;
            }

            return R;
        }

        public bool Editar()
        {
            bool R = false;

            Conexion MiCnn = new Conexion();

            MiCnn.ListadoDeParametros.Add(new SqlParameter("@Cedula", this.Cedula));
            MiCnn.ListadoDeParametros.Add(new SqlParameter("@Nombre", this.Nombre));
            MiCnn.ListadoDeParametros.Add(new SqlParameter("@Telefono", this.Telefono));
            MiCnn.ListadoDeParametros.Add(new SqlParameter("@Email", this.Email));
            MiCnn.ListadoDeParametros.Add(new SqlParameter("@ID", this.IDUsuario));

            int retorno = MiCnn.DMLUpdateDeleteInsert("SPUsuarioEditar");

            if (retorno == 1)
            {
                R = true;
            }

            return R;
        }

        public bool Eliminar()
        {
            bool R = false;

            Conexion MiCnn = new Conexion();

            MiCnn.ListadoDeParametros.Add(new SqlParameter("@ID", this.IDUsuario));

            int retorno = MiCnn.DMLUpdateDeleteInsert("SPUsuarioEliminar");

            if (retorno == 1)
            {
                R = true;
            }

            return R;
        }
        public DataTable Listar(string Filtro = "")
        {
            DataTable R = new DataTable();

            Conexion MiCnn = new Conexion();

            MiCnn.ListadoDeParametros.Add(new SqlParameter("@Filtro", Filtro));

            R = MiCnn.DMLSelect("SPUsuariosListar");

            return R;

        }
        

    }
}
