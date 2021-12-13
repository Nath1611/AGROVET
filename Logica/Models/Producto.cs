using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Logica.Models
{
    public class Producto : ICrudBase
    {
        public int IDProducto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Funcion { get; set; }
        public string FechaCaducidad { get; set; }
        public string NumExistencias { get; set; }

        public bool Agregar()
        {
            bool R = false;

            Conexion MiCnnAdd = new Conexion();

            MiCnnAdd.ListadoDeParametros.Add(new SqlParameter ("@Nombre", this.Nombre));
            MiCnnAdd.ListadoDeParametros.Add(new SqlParameter ("@Funcion", this.Funcion));
            MiCnnAdd.ListadoDeParametros.Add(new SqlParameter ("@Descripcion", this.Descripcion));
            MiCnnAdd.ListadoDeParametros.Add(new SqlParameter ("@FechaCaducidad", this.FechaCaducidad));
            MiCnnAdd.ListadoDeParametros.Add(new SqlParameter ("@CantExistencias", this.NumExistencias));

            MiCnnAdd.ListadoDeParametros.Add(new SqlParameter("@IDTipoProducto", this.MisTiposP.IDTipoProducto));

            int resultado = MiCnnAdd.DMLUpdateDeleteInsert("SPProductoAgregar");

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

            MiCnn.ListadoDeParametros.Add(new SqlParameter("@Nombre", this.Nombre));
            MiCnn.ListadoDeParametros.Add(new SqlParameter("@Funcion", this.Funcion));
            MiCnn.ListadoDeParametros.Add(new SqlParameter("@Descripcion", this.Descripcion));
            MiCnn.ListadoDeParametros.Add(new SqlParameter("@FechaCaducidad", this.FechaCaducidad));
            MiCnn.ListadoDeParametros.Add(new SqlParameter("@CantExistencias", this.NumExistencias));

            MiCnn.ListadoDeParametros.Add(new SqlParameter("@IDTipoProducto", this.MisTiposP.IDTipoProducto));
            MiCnn.ListadoDeParametros.Add(new SqlParameter("@IDProducto", this.IDProducto));

            int resultado = MiCnn.DMLUpdateDeleteInsert("SPProductoEditar");

            if (resultado == 1)
            {
                R = true;
            }

            return R;
        }

        public bool Eliminar()
        {
            bool R = false;

            Conexion conexion = new Conexion();

            conexion.ListadoDeParametros.Add(new SqlParameter("@ID", IDProducto));

            int retorno = conexion.DMLUpdateDeleteInsert("SPProductoEliminar");

            if (retorno == 1)
            {
                R = true;
            }

            return R;
        }

        public TipoProducto MisTiposP { get; set; }

        public Producto()
        {
            MisTiposP = new TipoProducto();
        }

        public Producto ConsultarPorID(int ID)
        {
            Producto R = new Producto();

            Conexion MiCnn = new Conexion();

            MiCnn.ListadoDeParametros.Add(new SqlParameter("@ID", ID));

            DataTable DatosProducto = new DataTable();

            DatosProducto = MiCnn.DMLSelect("SPProductoConsultarPorID");

            if (DatosProducto != null && DatosProducto.Rows.Count == 1)
            {
                DataRow Fila = DatosProducto.Rows[0];

                R.IDProducto = ID;
                R.Nombre = Convert.ToString(Fila["Nombre"]);
                R.Funcion = Convert.ToString(Fila["Funcion"]);
                R.Descripcion = Convert.ToString(Fila["Descripcion"]);
                R.FechaCaducidad = Convert.ToString(Fila["FechaCaducidad"]);
                R.NumExistencias = Convert.ToString(Fila ["NumExistencias"]);
                R.MisTiposP.IDTipoProducto = Convert.ToInt32(Fila["IDTipoProducto"]);
            }
            return R;   
        }

        public bool ConsultarPorNombre(string Nombre)
        {
            bool R = false;

            Conexion Conexion = new Conexion();

            Conexion.ListadoDeParametros.Add(new SqlParameter("@Nombre", this.Nombre));

            DataTable resultado = Conexion.DMLSelect("SPProductoConsultarPorNombre");

            if (resultado != null && resultado.Rows.Count > 0)
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

            R = MiCnn.DMLSelect("SPProductoListar");

            return R;
        }
    }
}
