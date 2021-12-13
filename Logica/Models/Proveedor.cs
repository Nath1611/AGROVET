using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Logica.Models
{
    public class Proveedor : ICrudBase, IPersona
    {
        public int IDProveedor { get; set; }
        public string Cedula { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Nombre { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Telefono { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Email { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string TipoEmpresa { get; set; }
        public bool Agregar()
        {
            bool R = false;

            Conexion MiCnnAdd = new Conexion();

            MiCnnAdd.ListadoDeParametros.Add(new SqlParameter("@Nombre", this.Nombre));
            MiCnnAdd.ListadoDeParametros.Add(new SqlParameter("@Empresa", this.TipoEmpresa));
            MiCnnAdd.ListadoDeParametros.Add(new SqlParameter("@Telefono", this.Telefono));
            MiCnnAdd.ListadoDeParametros.Add(new SqlParameter("@Email", this.Email));

            int resultado = MiCnnAdd.DMLUpdateDeleteInsert("SPProveedorAgregar");

            if (resultado > 0)
            {
                R = true;
            }

            return R;
        }

        public bool Editar()
        {
            bool R = false;

            Conexion MiCnnAdd = new Conexion();

            MiCnnAdd.ListadoDeParametros.Add(new SqlParameter("@Nombre", this.Nombre));
            MiCnnAdd.ListadoDeParametros.Add(new SqlParameter("@Empresa", this.TipoEmpresa));
            MiCnnAdd.ListadoDeParametros.Add(new SqlParameter("@Telefono", this.Telefono));
            MiCnnAdd.ListadoDeParametros.Add(new SqlParameter("@Email", this.Email));

            int resultado = MiCnnAdd.DMLUpdateDeleteInsert("SPProveedorEditar");

            if (resultado > 0)
            {
                R = true;
            }

            return R;
        }

        public bool Eliminar()
        {
            bool R = false;

            Conexion MiCnnAdd = new Conexion();

            MiCnnAdd.ListadoDeParametros.Add(new SqlParameter("@ID", this.IDProveedor));
           
            int resultado = MiCnnAdd.DMLUpdateDeleteInsert("SPProveedorEliminar");

            if (resultado == 0)
            {
                R = true;
            }

            return R;
        }
        public DataTable ListarProveedor()
        {
            DataTable R = new DataTable();

            Conexion MiConexion = new Conexion();

            R = MiConexion.DMLSelect("SPProveedorListar");

            return R;
        }

        public Proveedor ConsultarPorID(int ID)
        {
            Proveedor R = new Proveedor();

            Conexion MiCnn = new Conexion();

            MiCnn.ListadoDeParametros.Add(new SqlParameter("@ID", ID));

            DataTable DatosProveedor = new DataTable();

            DatosProveedor = MiCnn.DMLSelect("SPProveedorConsultarPorID");

            if (DatosProveedor != null && DatosProveedor.Rows.Count == 1)
            {
                DataRow Fila = DatosProveedor.Rows[0];

                R.IDProveedor = ID;
                R.Nombre = Convert.ToString(Fila["Nombre"]);
                R.TipoEmpresa = Convert.ToString(Fila["Empresa"]);
                R.Email = Convert.ToString(Fila["Email"]);
                R.Telefono = Convert.ToString(Fila["Telefono"]);
            }
            return R;
        }

        public bool ConsultarPorEmail(string Email)
        {
            bool R = false;

            Conexion MiConexion = new Conexion();

            MiConexion.ListadoDeParametros.Add(new SqlParameter("@Email", Email));

            DataTable DatosConexion = MiConexion.DMLSelect("SPProveedorConsultarPorEmail");

            if (DatosConexion != null && DatosConexion.Rows.Count > 0)
            {
                R = true;
            }

            return R;
        }

        public bool ConsultarPorEmpresa(string TipoEmpresa)
        {
            bool R = false;

            Conexion MiConexion = new Conexion();

            MiConexion.ListadoDeParametros.Add(new SqlParameter("@Empresa", TipoEmpresa));

            DataTable DatosConexion = MiConexion.DMLSelect("SPProveedorConsultarPorEmpresa");

            if (DatosConexion != null && DatosConexion.Rows.Count > 0)
            {
                R = true;
            }

            return R;
        }

        public DataTable Listar(string Filtro = "")
        {
            DataTable R = new DataTable();

            Conexion Conexion = new Conexion();

            Conexion.ListadoDeParametros.Add(new SqlParameter("@Filtro", Filtro));

            R = Conexion.DMLSelect("SPProveedorListar");

            return R;
        }

        
    }
}
