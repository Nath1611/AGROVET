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
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void gestiónDeUsuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Commons.ObjetosGlobales.FormularioGestionUsuarios = new FrmGestionUsuarios();

            Commons.ObjetosGlobales.FormularioGestionUsuarios.Show();

            Commons.ObjetosGlobales.FormularioGestionProductos.Show();
        }

        private void gestiónDeProveedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Commons.ObjetosGlobales.FormularioGestionProveedores.Show();
        }

        private void gestiónDeTipoDeProductoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Commons.ObjetosGlobales.FormularioGestionProductos.Show();

            Commons.ObjetosGlobales.FormularioGestionTipoProductos.Show();
        }
    }
}
