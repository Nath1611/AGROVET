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
    public partial class FrmGestionUsuarios : Form
    {
        private Logica.Models.Usuario UsuarioLocal { get; set; }
        private DataTable ListaUsuarios { get; set; }
        public FrmGestionUsuarios()
        {
            InitializeComponent();

            UsuarioLocal = new Logica.Models.Usuario();
            ListaUsuarios = new DataTable();
        }
        

        public void LlenarListaUsuarios(string Filtro = "")
        {
            ListaUsuarios = UsuarioLocal.Listar(Filtro);

            DgvListaUsuarios.DataSource = ListaUsuarios;

            DgvListaUsuarios.ClearSelection();
        }

        private void DgvListaUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DgvListaUsuarios.SelectedRows.Count == 1)
            {
                LimpiarFormulario(false);

                DataGridViewRow MiFila = DgvListaUsuarios.SelectedRows[0];

                int CodigoUsuario = Convert.ToInt32(MiFila.Cells["CIDUsuario"].Value);

                UsuarioLocal = UsuarioLocal.ConsultarPorID(CodigoUsuario);

                TxtIDUsuario.Text = UsuarioLocal.IDUsuario.ToString();
                TxtNombre.Text = UsuarioLocal.Nombre;
                TxtCedula.Text = UsuarioLocal.Cedula;
                TxtTelefono.Text = UsuarioLocal.Telefono;
                TxtEmail.Text = UsuarioLocal.Email;

                ActivarEditarEliminar();
            }
        }

        private void FrmGestionUsuarios_Load(object sender, EventArgs e)
        {
            LlenarListaUsuarios();
            LimpiarFormulario();
        }

        private bool ValidarDatosRequeridos()
        {
            bool R = false;

            if (!string.IsNullOrEmpty(UsuarioLocal.Nombre) &&
                !string.IsNullOrEmpty(UsuarioLocal.Cedula) &&
                !string.IsNullOrEmpty(UsuarioLocal.Email) &&
                !string.IsNullOrEmpty(UsuarioLocal.Telefono))
            {
                R = true;
            }
            else
            {
                if (string.IsNullOrEmpty(UsuarioLocal.Nombre))
                {
                    MessageBox.Show("Debe digitar el nombre", "Error de validación", MessageBoxButtons.OK);
                    TxtNombre.Focus();
                    return false;
                }
                if (string.IsNullOrEmpty(UsuarioLocal.Cedula))
                {
                    MessageBox.Show("Debe digitar la cédula", "Error de validación", MessageBoxButtons.OK);
                    TxtCedula.Focus();
                    return false;
                }
                if (string.IsNullOrEmpty(UsuarioLocal.Email))
                {
                    MessageBox.Show("Debe digitar el email del usuario", "Error de validación", MessageBoxButtons.OK);
                    TxtEmail.Focus();
                    return false;
                }
                if (string.IsNullOrEmpty(UsuarioLocal.Telefono))
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
            TxtIDUsuario.Clear();
            TxtNombre.Clear();
            TxtCedula.Clear();
            TxtEmail.Clear();
            TxtTelefono.Clear();

            if (LimpiarBusqueda)
            {
                TxtBuscador.Text = "Buscar";
            }

            UsuarioLocal = new Logica.Models.Usuario();

            AgregarU();
        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            if (ValidarDatosRequeridos())
            {
                bool OkCedula = UsuarioLocal.ConsultarPorCedula(UsuarioLocal.Cedula);
                bool OkEmail = UsuarioLocal.ConsultarPorEmail(UsuarioLocal.Email);

                if (!OkCedula && !OkEmail)
                {
                    string Message = string.Format("¿Desea agregar al usuario {0}?", "Confirmación", MessageBoxButtons.YesNo);
                    DialogResult Continuar = MessageBox.Show(Message, "Confrimación", MessageBoxButtons.YesNo);

                    if (Continuar == DialogResult.Yes)
                    {
                        if (UsuarioLocal.Agregar())
                        {
                            MessageBox.Show("Se ha agregado al usuario correctamente", "Confirmación", MessageBoxButtons.OK);
                            LimpiarFormulario();
                            LlenarListaUsuarios();
                        }
                        else
                        {
                            MessageBox.Show("No se logró agregar al usuario", "Confirmación", MessageBoxButtons.OK);
                        }
                    }
                }
                else
                {
                    if (OkCedula)
                    {
                        MessageBox.Show("Ya existe un usuario con esa cédula", "Aviso", MessageBoxButtons.OK);
                    }
                    if (OkEmail)
                    {
                        MessageBox.Show("Ya existe un usuario con ese email", "Aviso", MessageBoxButtons.OK);
                    }
                }
            }
        }

        private void TxtNombre_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtNombre.Text.Trim()))
            {
                UsuarioLocal.Nombre = TxtNombre.Text.Trim();
            }
            else
            {
                UsuarioLocal.Nombre = "";
            }
        }
        private void TxtCedula_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtCedula.Text.Trim()))
            {
                UsuarioLocal.Cedula = TxtCedula.Text.Trim();
            }
            else
            {
                UsuarioLocal.Cedula = "";
            }
        }
        private void TxtTelefono_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtTelefono.Text.Trim()))
            {
                UsuarioLocal.Telefono = TxtTelefono.Text.Trim();
            }
            else
            {
                UsuarioLocal.Telefono = "";
            }
        }

        private void TxtEmail_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtEmail.Text.Trim()))
            {
                if (Commons.ObjetosGlobales.ValidarEmail((TxtEmail.Text.Trim())))
                {
                    UsuarioLocal.Email = TxtEmail.Text.Trim();
                }
                else
                {
                    MessageBox.Show("El formato del correo no es correcto", "Error de validación", MessageBoxButtons.OK);
                    TxtEmail.Focus();
                    TxtEmail.SelectAll();
                }
            }
            else
            {
                UsuarioLocal.Email = "";
            }
        }

        private void BtnLimpiarFormulario_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            LlenarListaUsuarios();
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {

            if (ValidarDatosRequeridos())
            {
                Logica.Models.Usuario ObjUsuario = UsuarioLocal.ConsultarPorID(UsuarioLocal.IDUsuario);
                
                if (ObjUsuario.IDUsuario > 0)
                {
                    string Mensaje = string.Format("¿Desea continaur con la modificación del usuario?", "Confirmación", MessageBoxButtons.YesNo);
                    DialogResult Continuar = MessageBox.Show(Mensaje, "Confirmación", MessageBoxButtons.YesNo);

                    if (Continuar == DialogResult.Yes)
                    {
                        if (UsuarioLocal.Editar())
                        {
                            MessageBox.Show("El usuario ha sido actualizado correctamente", "Confirmación", MessageBoxButtons.OK);

                            LimpiarFormulario();
                            LlenarListaUsuarios();
                        }
                        else
                        {
                            MessageBox.Show("El usuario no pudo ser actualizado", "Confirmación", MessageBoxButtons.OK);
                        }
                    }
                }
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            Logica.Models.Usuario ObjUsuarioTemporal = UsuarioLocal.ConsultarPorID(UsuarioLocal.IDUsuario);

            if (ObjUsuarioTemporal.IDUsuario > 0)
            {
                string Mensaje = string.Format("¿Desea continuar con la eliminación del usuario {0}?", UsuarioLocal.Nombre);
                DialogResult Continuar = MessageBox.Show(Mensaje, "Confirmación", MessageBoxButtons.YesNo);

                if (Continuar == DialogResult.Yes)
                {
                    if (UsuarioLocal.Eliminar())
                    {
                        MessageBox.Show("El usuario ha sido eliminado correctamente", "Confirmación", MessageBoxButtons.OK);
                    }
                    else
                    {
                        MessageBox.Show("El usuario no pudo ser eliminado", "Confirmación", MessageBoxButtons.OK);
                    }
                }
                LimpiarFormulario();
                LlenarListaUsuarios();
            }
        }

        private void TxtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Commons.ObjetosGlobales.CaracteresTexto(e, true);
        }

        private void TxtCedula_KeyPress(object sender, KeyPressEventArgs e)
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

        //SEGURIDAD PARA EL USUARIO
        private void AgregarU()
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
                LlenarListaUsuarios();
            }
        }
    }

}
