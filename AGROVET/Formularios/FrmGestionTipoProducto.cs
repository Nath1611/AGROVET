using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AGROVET.Formularios
{
    public partial class FrmGestionTipoProducto : Form
    {
        private Logica.Models.TipoProducto TipoPrLocal { get; set; }

        private DataTable ListaTiposProductos { get; set; }

        public FrmGestionTipoProducto()
        {
            InitializeComponent();

            TipoPrLocal = new Logica.Models.TipoProducto();

            ListaTiposProductos = new DataTable();
        }

        private void FrmGestionTipoProducto_Load(object sender, EventArgs e)
        {
            LlenarListaTipoProductos();

            LimpiarFormulario();
        }

        private void LlenarListaTipoProductos(string FiltroBusqueda = "")
        {
            string Filtro = "";

            if (!string.IsNullOrEmpty(FiltroBusqueda) && FiltroBusqueda != "Buscar")
            {
                Filtro = FiltroBusqueda;
            }

            ListaTiposProductos = TipoPrLocal.ListarTipoProducto(Filtro);
            DgvListaTipoProductos.DataSource = ListaTiposProductos;
            DgvListaTipoProductos.ClearSelection();
        }

        private bool ValidarDatosRequeridos()
        {
            bool R = false;

            if (!string.IsNullOrEmpty(TipoPrLocal.Nombre))
            {
                R = true;
            }
            else
            {
                if (string.IsNullOrEmpty(TipoPrLocal.Nombre))
                {
                    MessageBox.Show("Debe digitar el nombre", "Error de validación", MessageBoxButtons.OK);
                    TxtNombre.Focus();
                    return false;
                }
            }
            return R;
        }

        private void LimpiarFormulario(bool LimpiarBusqueda = true)
        {
            TxtNombre.Clear();

            if (LimpiarBusqueda)
            {
                TxtBuscador.Text = "Buscar";
            }

            TipoPrLocal = new Logica.Models.TipoProducto();

            AgregarTipoP();
        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            if (ValidarDatosRequeridos())
            {
                bool OkNombre = TipoPrLocal.ConsultarPorNombre(TipoPrLocal.Nombre);

                if (!OkNombre)
                {
                    string Mensaje = string.Format("¿Desea continuar y agregar este tipo de producto {0}?", "Confirmación", MessageBoxButtons.YesNo);
                    DialogResult Continuar = MessageBox.Show(Mensaje, "Confirmación", MessageBoxButtons.YesNo);

                    if (Continuar == DialogResult.Yes)
                    {
                        if (TipoPrLocal.Agregar())
                        {
                            MessageBox.Show("Se ha logrado agregar el tipo de producto", "Aviso", MessageBoxButtons.OK);
                            LimpiarFormulario();
                            LlenarListaTipoProductos();
                        }
                        else
                        {
                            MessageBox.Show("No se logró guardar el tipo de producto", "Aviso", MessageBoxButtons.OK);
                        }
                    }
                }
                else
                {
                    if (OkNombre)
                    {
                        MessageBox.Show("Ya existe un tipo de producto con ese nombre", "Aviso", MessageBoxButtons.OK);
                    }
                }
            }
        }

        private void TxtNombre_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtNombre.Text.Trim()))
            {
                TipoPrLocal.Nombre = TxtNombre.Text.Trim();
            }
            else
            {
                TipoPrLocal.Nombre = "";
            }
        }

        private void BtnLimpiarFormulario_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            LlenarListaTipoProductos();
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            if (ValidarDatosRequeridos())
            {
                Logica.Models.TipoProducto ObjTipoProd = TipoPrLocal.ConsultarPorID(TipoPrLocal.IDTipoProducto);

                if (ObjTipoProd.IDTipoProducto > 0)
                {
                    string Mensaje = string.Format("¿Desea continuar con la modificación de este tipo de producto?", "Confirmación", MessageBoxButtons.YesNo);
                    DialogResult Continuar = MessageBox.Show(Mensaje, "Confirmación", MessageBoxButtons.YesNo);

                    if (Continuar == DialogResult.Yes)
                    {
                        if (TipoPrLocal.Editar())
                        {
                            MessageBox.Show("El tipo del producto ha sido modificaco correctamente", "Aviso", MessageBoxButtons.OK);
                            LimpiarFormulario();
                            LlenarListaTipoProductos();
                        }
                        else
                        {
                            MessageBox.Show("No se pudo modificar este tipo de producto", "Aviso", MessageBoxButtons.OK);
                        }
                    }
                }
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (ValidarDatosRequeridos())
            {
                Logica.Models.TipoProducto ObjTipoProd = TipoPrLocal.ConsultarPorID(TipoPrLocal.IDTipoProducto);

                if (ObjTipoProd.IDTipoProducto > 0)
                {
                    string Mensaje = string.Format("¿Desea continuar con la modificación de este tipo de producto?", "Confirmación", MessageBoxButtons.YesNo);
                    DialogResult Continuar = MessageBox.Show(Mensaje, "Confirmación", MessageBoxButtons.YesNo);

                    if (Continuar == DialogResult.Yes)
                    {
                        if (TipoPrLocal.Eliminar())
                        {
                            MessageBox.Show("El tipo del producto ha sido eliminado correctamente", "Aviso", MessageBoxButtons.OK);
                        }
                        else
                        {
                            MessageBox.Show("No se pudo eliminar este tipo de producto", "Aviso", MessageBoxButtons.OK);
                        }
                    }
                    LimpiarFormulario();
                    LlenarListaTipoProductos();
                }
            }
        }

        private void TxtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Commons.ObjetosGlobales.CaracteresTexto(e, true);
        }

        private void AgregarTipoP()
        {
            BtnAgregar.Enabled = true;
            BtnEditar.Enabled = false;
            BtnEliminar.Enabled = false;
        }
        private void ActivarEditarEliminar()
        {
            BtnAgregar.Enabled = false;
            BtnEditar.Enabled = true;
            BtnEliminar.Enabled = true;
        }
        private void TxtBuscador_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtBuscador.Text.Trim()) && TxtBuscador.Text.Count() >= 2)
            {
                LlenarListaTipoProductos(TxtBuscador.Text.Trim());
            }
            else
            {
                LlenarListaTipoProductos();
            }
        }
    }
}


