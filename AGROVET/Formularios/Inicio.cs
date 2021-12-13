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
    public partial class Inicio : Form
    {
        public Inicio()
        {
            InitializeComponent();
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            //Salir de la aplicación
            Application.Exit();
        }

        private void BtnContinuar_Click(object sender, EventArgs e)
        {
            Commons.ObjetosGlobales.MiFormPrincipal.Show();
        }
    }
}
