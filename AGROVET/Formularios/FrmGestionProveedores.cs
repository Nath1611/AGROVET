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
    public partial class FrmGestionProveedores : Form
    {
        private Logica.Models.Proveedor ProveedorLocal { get; set; }
        private DataTable ListaProveedores { get; set; }

        public FrmGestionProveedores()
        {
            InitializeComponent();

            ProveedorLocal = new Logica.Models.Proveedor();
            ListaProveedores = new DataTable();
        }
        public void LlenarListaProveedores(string Filtro = ".")
        {
            ListaProveedores = ProveedorLocal.Listar(Filtro);

            DgvListaProveedores.DataSource = ListaProveedores;

            DgvListaProveedores.ClearSelection();
        }

        

        private void FrmGestionProveedores_Load(object sender, EventArgs e)
        {
            LlenarListaProveedores();
            LimpiarFormulario();
        }

        private bool ValidarDatosRequeridos()
        {
            bool R = false;

            if (!string.IsNullOrEmpty(ProveedorLocal.Nombre) &&
                !string.IsNullOrEmpty(ProveedorLocal.TipoEmpresa) &&
                !string.IsNullOrEmpty(ProveedorLocal.Email) &&
                !string.IsNullOrEmpty(ProveedorLocal.Telefono))
            {
                R = true;
            }
            else
            {
                if (string.IsNullOrEmpty(ProveedorLocal.Nombre))
                {
                    MessageBox.Show("Debe digitar el nombre", "Error de validación", MessageBoxButtons.OK);
                    TxtNombre.Focus();
                    return false;
                }
                if (string.IsNullOrEmpty(ProveedorLocal.TipoEmpresa))
                {
                    MessageBox.Show("Debe digitar la empresa", "Error de validación", MessageBoxButtons.OK);
                    TxtEmpresa.Focus();
                    return false;
                }
                if (string.IsNullOrEmpty(ProveedorLocal.Email))
                {
                    MessageBox.Show("Debe digitar el email", "Error de validación", MessageBoxButtons.OK);
                    TxtEmail.Focus();
                    return false;
                }
                if (string.IsNullOrEmpty(ProveedorLocal.Telefono))
                {
                    MessageBox.Show("Debe digitar el teléfono", "Error de validación", MessageBoxButtons.OK);
                    TxtTelefono.Focus();
                    return false;
                }
            }
            return R;
        }

        private void LimpiarFormulario(bool LimpiarBusqueda = true)
        {
            TxtIDProveedor.Clear();
            TxtNombre.Clear();
            TxtEmpresa.Clear();
            TxtTelefono.Clear();
            TxtEmail.Clear();

            if (LimpiarBusqueda)
            {
                TxtBuscador.Text = "Buscar";
            }

            ProveedorLocal = new Logica.Models.Proveedor();

            AgregarP();
        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            if (ValidarDatosRequeridos())
            {
                bool OkEmail = ProveedorLocal.ConsultarPorEmail(ProveedorLocal.Email);

                if (!OkEmail)
                {
                    string Message = string.Format("¿Desea agregar al proveedor {0}?", "Confirmación", MessageBoxButtons.YesNo);
                    DialogResult Continuar = MessageBox.Show(Message, "Confrimación", MessageBoxButtons.YesNo);

                    if (Continuar == DialogResult.Yes)
                    {
                        if (ProveedorLocal.Agregar())
                        {
                            MessageBox.Show("Se ha agregado al proveedor correctamente", "Confirmación", MessageBoxButtons.OK);
                            LimpiarFormulario();
                            LlenarListaProveedores();
                        }
                        else
                        {
                            MessageBox.Show("No se logró agregar al proveedor", "Confirmación", MessageBoxButtons.OK);
                        }
                    }
                }
                else
                {
                    if (OkEmail)
                    {
                        MessageBox.Show("Ya existe un proveedor con ese email", "Aviso", MessageBoxButtons.OK);
                    }
                }
            }
        }

        private void TxtNombre_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtNombre.Text.Trim()))
            {
                ProveedorLocal.Nombre = TxtNombre.Text.Trim();
            }
            else
            {
                ProveedorLocal.Nombre = "";
            }
        }

        private void TxtEmpresa_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtEmpresa.Text.Trim()))
            {
                ProveedorLocal.TipoEmpresa = TxtEmpresa.Text.Trim();
            }
            else
            {
                ProveedorLocal.TipoEmpresa = "";
            }
        }

        private void TxtTelefono_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtTelefono.Text.Trim()))
            {
                ProveedorLocal.Telefono = TxtTelefono.Text.Trim();
            }
            else
            {
                ProveedorLocal.Telefono = "";
            }
        }

        private void TxtEmail_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtEmail.Text.Trim()))
            {
                ProveedorLocal.Email = TxtEmail.Text.Trim();
            }
            else
            {
                ProveedorLocal.Email = "";
            }
        }

        private void BtnLimpiarFormulario_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            LlenarListaProveedores();
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            if (ValidarDatosRequeridos())
            {
                Logica.Models.Proveedor ObjProveedor = ProveedorLocal.ConsultarPorID(ProveedorLocal.IDProveedor);

                if (ObjProveedor.IDProveedor > 0)
                {
                    string Mensaje = string.Format("¿Desea continaur con la modificación del proveedor?", "Confirmación", MessageBoxButtons.YesNo);
                    DialogResult Continuar = MessageBox.Show(Mensaje, "Confirmación", MessageBoxButtons.YesNo);

                    if (Continuar == DialogResult.Yes)
                    {
                        if (ProveedorLocal.Editar())
                        {
                            MessageBox.Show("El proveedor ha sido actualizado correctamente", "Confirmación", MessageBoxButtons.OK);

                            LimpiarFormulario();
                            LlenarListaProveedores();
                        }
                        else
                        {
                            MessageBox.Show("El proveedor no pudo ser actualizado", "Confirmación", MessageBoxButtons.OK);
                        }
                    }
                }
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (ValidarDatosRequeridos())
            {
                Logica.Models.Proveedor ObjProveedorTemporal = ProveedorLocal.ConsultarPorID(ProveedorLocal.IDProveedor);

                if (ObjProveedorTemporal.IDProveedor > 0)
                {
                    string Mensaje = string.Format("¿Desea continaur con la eliminación del proveedor?", "Confirmación", MessageBoxButtons.YesNo);
                    DialogResult Continuar = MessageBox.Show(Mensaje, "Confirmación", MessageBoxButtons.YesNo);

                    if (Continuar == DialogResult.Yes)
                    {
                        if (ProveedorLocal.Eliminar())
                        {
                            MessageBox.Show("El proveedor ha sido eliminado correctamente", "Confirmación", MessageBoxButtons.OK);

                            LimpiarFormulario();
                            LlenarListaProveedores();
                        }
                        else
                        {
                            MessageBox.Show("El proveedor no pudo ser eliminado", "Confirmación", MessageBoxButtons.OK);
                        }
                    }
                }
            }
        }

        private void DgvListaProveedores_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DgvListaProveedores.SelectedRows.Count == 1)
            {
                LimpiarFormulario(false);

                DataGridViewRow MiFila = DgvListaProveedores.SelectedRows[0];

                int CodigoProveedor = Convert.ToInt32(MiFila.Cells["CIDProveedor"].Value);

                ProveedorLocal = ProveedorLocal.ConsultarPorID(CodigoProveedor);

                TxtIDProveedor.Text = ProveedorLocal.IDProveedor.ToString();
                TxtNombre.Text = ProveedorLocal.Nombre;
                TxtEmpresa.Text = ProveedorLocal.TipoEmpresa;
                TxtTelefono.Text = ProveedorLocal.Telefono;
                TxtEmail.Text = ProveedorLocal.Email;

                ActivarEditarEliminar();
            }
        }

        private void TxtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Commons.ObjetosGlobales.CaracteresTexto(e, true);
        }

        private void TxtEmpresa_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Commons.ObjetosGlobales.CaracteresTexto(e, true);
        }

        private void TxtTelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Commons.ObjetosGlobales.CaracteresTexto(e, true);
        }

        private void TxtEmail_KeyPress(object sender, KeyPressEventArgs e)
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
                LlenarListaProveedores();
            }
        }
    }
}
