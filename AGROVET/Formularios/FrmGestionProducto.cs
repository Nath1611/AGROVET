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
    public partial class FrmGestionProducto : Form
    {
        private Logica.Models.Producto ProductoLocal { get; set; }
        private DataTable ListaProductos { get; set; }

        public FrmGestionProducto()
        {
            InitializeComponent();

            ProductoLocal = new Logica.Models.Producto();

            ListaProductos = new DataTable();
        }

        private void FrmGestionProducto_Load(object sender, EventArgs e)
        {
            CargarComboTiposP();

            LlenarListaProductos();

            LimpiarFormulario();
        }

        private void LlenarListaProductos(string FiltroBusqueda = "")
        {
            string Filtro = "";

            if (!string.IsNullOrEmpty(FiltroBusqueda) && FiltroBusqueda != "Buscar")
            {
                Filtro = FiltroBusqueda;
            }

            ListaProductos = ProductoLocal.Listar(Filtro);
            DgvListaProductos.DataSource = ListaProductos;
            DgvListaProductos.ClearSelection();
        }

        private void CargarComboTiposP()
        {
            DataTable DatosDeTiposP = new DataTable();

            DatosDeTiposP = ProductoLocal.MisTiposP.ListarTipoProducto();

            CbTipoProducto.ValueMember = "ID";
            CbTipoProducto.DisplayMember = "Descrip";

            CbTipoProducto.DataSource = DatosDeTiposP;

            CbTipoProducto.SelectedIndex = -1;
        }

        private bool ValidarDatosRequeridos()
        {
            bool R = false;

            if (!string.IsNullOrEmpty(ProductoLocal.Nombre) &&
                !string.IsNullOrEmpty(ProductoLocal.Funcion) &&
                !string.IsNullOrEmpty(ProductoLocal.Descripcion) &&
                !string.IsNullOrEmpty(ProductoLocal.FechaCaducidad) &&
                !string.IsNullOrEmpty(Convert.ToString(ProductoLocal.NumExistencias)) &&
                    ProductoLocal.MisTiposP.IDTipoProducto > 0)
            {
                R = true;
            }
            else
            {
                if (string.IsNullOrEmpty(ProductoLocal.Nombre))
                {
                    MessageBox.Show("Debe digitar el nombre", "Error de validación", MessageBoxButtons.OK);
                    TxtNombre.Focus();
                    return false;
                }
                if (string.IsNullOrEmpty(ProductoLocal.Funcion))
                {
                    MessageBox.Show("Debe digitar la función del producto", "Error de validación", MessageBoxButtons.OK);
                    TxtFuncion.Focus();
                    return false;
                }
                if (string.IsNullOrEmpty(ProductoLocal.Descripcion))
                {
                    MessageBox.Show("Debe digitar la descripción del producto", "Error de validación", MessageBoxButtons.OK);
                    TxtDescripcion.Focus();
                    return false;
                }
                if (string.IsNullOrEmpty(ProductoLocal.FechaCaducidad))
                {
                    MessageBox.Show("Debe digitar la caducidad del producto", "Error de validación", MessageBoxButtons.OK);
                    TxtFechaCaducidad.Focus();
                    return false;
                }
                if (string.IsNullOrEmpty(Convert.ToString(ProductoLocal.NumExistencias)))
                {
                    MessageBox.Show("Debe digitar el número de productos disponibles", "Error de validación", MessageBoxButtons.OK);
                    TxtNumExistencias.Focus();
                    return false;
                }
                if (ProductoLocal.MisTiposP.IDTipoProducto <= 0)
                {
                    MessageBox.Show("Debe seleccionar un tipo de producto", "Error de validación", MessageBoxButtons.OK);
                    CbTipoProducto.Focus();
                    return false;
                }
            }
            return R;
        }

        private void LimpiarFormulario(bool LimpiarBusqueda = true)
        {
            TxtIDProducto.Clear();
            TxtNombre.Clear();
            TxtFuncion.Clear();
            TxtDescripcion.Clear();
            TxtFechaCaducidad.Clear();
            TxtNumExistencias.Clear();
            CbTipoProducto.SelectedIndex = -1;

            if (LimpiarBusqueda)
            {
                TxtBuscador.Text = "Buscar";
            }

            ProductoLocal = new Logica.Models.Producto();

            AgregarP();
        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            if (ValidarDatosRequeridos())
            {
                bool OkNombre = ProductoLocal.ConsultarPorNombre(ProductoLocal.Nombre);

                if (!OkNombre)
                {
                    string Mensaje = string.Format("¿Desea continuar y agregar el producto {0}?", "Confirmación", MessageBoxButtons.YesNo);
                    DialogResult Continuar = MessageBox.Show(Mensaje, "Confirmación", MessageBoxButtons.YesNo);

                    if (Continuar == DialogResult.Yes)
                    {
                        if (ProductoLocal.Agregar())
                        {
                            MessageBox.Show("Se ha logrado agregar el producto", "Aviso", MessageBoxButtons.OK);
                            LimpiarFormulario();
                            LlenarListaProductos();
                        }
                        else
                        {
                            MessageBox.Show("No se logró guardar el producto", "Aviso", MessageBoxButtons.OK);
                        }
                    }
                }
                else
                {
                    if (OkNombre)
                    {
                        MessageBox.Show("Ya existe un producto con ese nombre", "Aviso", MessageBoxButtons.OK);
                    }
                }
            }
        }

        private void TxtNombre_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtNombre.Text.Trim()))
            {
                ProductoLocal.Nombre = TxtNombre.Text.Trim();
            }
            else
            {
                ProductoLocal.Nombre = "";
            }
        }

        private void TxtFuncion_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtFuncion.Text.Trim()))
            {
                ProductoLocal.Funcion = TxtFuncion.Text.Trim();
            }
            else
            {
                ProductoLocal.Funcion = "";
            }
        }

        private void TxtDescripcion_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtDescripcion.Text.Trim()))
            {
                ProductoLocal.Descripcion = TxtDescripcion.Text.Trim();
            }
            else
            {
                ProductoLocal.Descripcion = "";
            }
        }

        private void TxtFechaCaducidad_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtFechaCaducidad.Text.Trim()))
            {
                ProductoLocal.FechaCaducidad = TxtFechaCaducidad.Text.Trim();
            }
            else
            {
                ProductoLocal.Funcion = "";
            }
        }

        private void TxtNumExistencias_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtNumExistencias.Text.Trim()))
            {
                ProductoLocal.NumExistencias = TxtNumExistencias.Text.Trim();
            }
            else
            {
                ProductoLocal.NumExistencias = "";
            }
        }

        private void CbTipoProducto_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (CbTipoProducto.SelectedIndex >= 0)
            {
                ProductoLocal.MisTiposP.IDTipoProducto = Convert.ToInt32(CbTipoProducto.SelectedValue);
            }
        }

        private void BtnLimpiarFormulario_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            LlenarListaProductos();
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            if (ValidarDatosRequeridos())
            {
                Logica.Models.Producto ObjProducto = ProductoLocal.ConsultarPorID(ProductoLocal.IDProducto);

                if (ObjProducto.IDProducto > 0)
                {
                    string Mensaje = string.Format("¿Desea continuar con la modificación del producto?", "Confirmación", MessageBoxButtons.YesNo);
                    DialogResult Continuar = MessageBox.Show(Mensaje, "Confirmación", MessageBoxButtons.YesNo);

                    if (Continuar == DialogResult.Yes)
                    {
                        if (ProductoLocal.Editar())
                        {
                            MessageBox.Show("El producto se ha modificado correctamente", "Aviso", MessageBoxButtons.OK);
                            LimpiarFormulario();
                            LlenarListaProductos();
                        }
                        else
                        {
                            MessageBox.Show("No se pudo modificar el producto", "Aviso", MessageBoxButtons.OK);
                        }
                    }
                }
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            Logica.Models.Producto ObjProductoTemporal = ProductoLocal.ConsultarPorID(ProductoLocal.IDProducto);

            if (ObjProductoTemporal.IDProducto > 0)
            {
                string Mensaje = string.Format("¿Desea continuar con la eliminación del producto?", "Confirmación", MessageBoxButtons.YesNo);
                DialogResult Continuar = MessageBox.Show(Mensaje, "Confirmación", MessageBoxButtons.YesNo);

                if (Continuar == DialogResult.Yes)
                {
                    if (ProductoLocal.Eliminar())
                    {
                        MessageBox.Show("El producto se ha eliminado correctamente", "Aviso", MessageBoxButtons.OK);
                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar el producto", "Aviso", MessageBoxButtons.OK);
                    }
                }
                LimpiarFormulario();
                LlenarListaProductos();
            }
        }

        private void DgvListaProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DgvListaProductos.SelectedRows.Count == 1)
            {
                LimpiarFormulario();

                DataGridViewRow MiFila = DgvListaProductos.SelectedRows[0];

                int CodigoProducto = Convert.ToInt32(MiFila.Cells["CIDProducto"].Value);

                ProductoLocal = ProductoLocal.ConsultarPorID(CodigoProducto);

                TxtIDProducto.Text = ProductoLocal.IDProducto.ToString();
                TxtNombre.Text = ProductoLocal.Nombre;
                TxtFuncion.Text = ProductoLocal.Funcion;
                TxtDescripcion.Text = ProductoLocal.Descripcion;
                TxtFechaCaducidad.Text = ProductoLocal.FechaCaducidad;
                TxtNumExistencias.Text = ProductoLocal.NumExistencias;
                CbTipoProducto.SelectedValue = ProductoLocal.MisTiposP.IDTipoProducto;

                ActivarEditarEliminar();
            }
        }

        private void TxtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Commons.ObjetosGlobales.CaracteresTexto(e, true);
        }

        private void TxtFuncion_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Commons.ObjetosGlobales.CaracteresTexto(e, true);
        }

        private void TxtDescripcion_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Commons.ObjetosGlobales.CaracteresTexto(e, true);
        }

        private void TxtFechaCaducidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Commons.ObjetosGlobales.CaracteresTexto(e, true);
        }

        private void TxtNumExistencias_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Commons.ObjetosGlobales.CaracteresTexto(e, true);
        }

        private void AgregarP()
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
                LlenarListaProductos(TxtBuscador.Text.Trim());
            }
            else
            {
                LlenarListaProductos();
            }
        }
    }
}
