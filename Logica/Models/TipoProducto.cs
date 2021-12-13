using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.Models
{
    public class TipoProducto : ICrudBase
    {
        public int IDTipoProducto { get; set; }
        public string Nombre { get; set; }

        public bool Agregar()
        {
            bool R = false;

            Conexion MiCnnAdd = new Conexion();

            MiCnnAdd.ListadoDeParametros.Add(new SqlParameter("@Nombre", this.Nombre));
           
            MiCnnAdd.ListadoDeParametros.Add(new SqlParameter("@IDTipoProducto", this.IDTipoProducto));

            int resultado = MiCnnAdd.DMLUpdateDeleteInsert("SPTipoProductoAgregar");

            if (resultado > 0)
            {
                R = true;
            }

            return R;
        }

        public bool Editar()
        {
            throw new NotImplementedException();
        }

        public bool Eliminar()
        {
            bool R = false;

            Conexion MiCnn = new Conexion();

            MiCnn.ListadoDeParametros.Add(new SqlParameter("@Nombre", this.Nombre));

            MiCnn.ListadoDeParametros.Add(new SqlParameter("@IDTipoProducto", this.IDTipoProducto));

            int resultado = MiCnn.DMLUpdateDeleteInsert("SPTipoProductoEditar");

            if (resultado == 1)
            {
                R = true;
            }

            return R;
        }
        public DataTable ListarTipoProducto(string Filtro = "")
        {
            DataTable R = new DataTable();

            Conexion MiConexion = new Conexion();

            R = MiConexion.DMLSelect("SPTipoProductoListar");

            return R;
        }

        public TipoProducto ConsultarPorID(int ID)
        {
            TipoProducto R = new TipoProducto();

            Conexion MiCnn = new Conexion();

            MiCnn.ListadoDeParametros.Add(new SqlParameter("@ID", ID));

            DataTable DatosTipoProducto = new DataTable();

            DatosTipoProducto = MiCnn.DMLSelect("SPTipoProductoConsultarPorID");

            if (DatosTipoProducto != null && DatosTipoProducto.Rows.Count == 1)
            {
                DataRow Fila = DatosTipoProducto.Rows[0];

                R.IDTipoProducto = ID;
                R.Nombre = Convert.ToString(Fila["Nombre"]);
            }

            return R;
        }
        public bool ConsultarPorNombre(string Nombre)
        {
            bool R = false;

            Conexion MiCnn = new Conexion();

            MiCnn.ListadoDeParametros.Add(new SqlParameter("@Nombre", Nombre));

            DataTable DatosTipoProducto = new DataTable();

            DatosTipoProducto = MiCnn.DMLSelect("SPTipoProductoConsultarPorNombre");

            if (DatosTipoProducto != null && DatosTipoProducto.Rows.Count == 0)
            {
                R = true;
            }

            return R;
        }
    }
}
