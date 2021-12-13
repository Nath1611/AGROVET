using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystalDecisions.CrystalReports.Engine;

namespace Logica.Models
{
    public class Factura
    {
        public int IDFactura { get; set; }
        public int Total { get; set; }
        public int Impuesto { get; set; }
        public DateTime Fecha { get; set; }
        public string Cliente { get; set; }
        public int IDProducto { get; set; }

        //constructor
        public Factura()
        {
            MiProducto = new Producto();
            MiCliente = new Cliente();

        }

        public Producto MiProducto { get; set; }
        public Cliente MiCliente { get; set; }

        //Funciones
        public bool Agregar()
        {
            bool R = false;

            Conexion MiCnn = new Conexion();

            MiCnn.ListadoDeParametros.Add(new SqlParameter("@IDCliente", this.MiCliente.IDCliente));
            MiCnn.ListadoDeParametros.Add(new SqlParameter("@IDProducto", this.MiProducto.IDProducto));


            Object i = MiCnn.DMLConRetornoEscalar("SPFacturaAgregar");

            if (i != null)
            {
                //hay que asignar al ID del ticket el valor del ID creado en el SP
                //ya que es fundamental para la visualizacion del reporte
                this.IDFactura = Convert.ToInt32(i.ToString());


                R = true;
            }

            return R;
        }
        public ReportDocument Imprimir(ReportDocument reporte)
        {
            ReportDocument R = reporte;

            Crystal OCrystal = new Crystal(R);

            DataTable Datos = new DataTable();

            Conexion MiCnn = new Conexion();

            MiCnn.ListadoDeParametros.Add(new SqlParameter("@IDFactura", this.IDFactura));

            Datos = MiCnn.DMLSelect("SPFacturaReporte");

            if (Datos != null && Datos.Rows.Count > 0)
            {
                //se le asigna al reporte (que se diseñó) los datos que provienen del SP
                OCrystal.Datos = Datos;

                R = OCrystal.GenerarReporte();
            }
            return R;
        }
    }
}
