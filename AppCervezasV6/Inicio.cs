using System;
using System.Windows.Forms;

namespace AppCervezasV6
{
    public partial class Inicio : Form
    {

        String ruta;
        public Inicio(string rutaArchivo)
        {
            ruta = rutaArchivo;
            InitializeComponent();
            ventanaPanel(new VerBaseDatos(ruta));
        }

        private void ventanaPanel(Form ventana)
        {
            panelVentana.Controls.Clear();

            ventana.TopLevel = false;
            ventana.FormBorderStyle = FormBorderStyle.None;
            ventana.ClientSize = panelVentana.Size;


            panelVentana.Controls.Add(ventana);
            ventana.Show();

        }

        private void btnVer_Click(object sender, EventArgs e)
        {
            ventanaPanel(new VerBaseDatos(ruta));
        }

        private void btnInsertar_Click(object sender, EventArgs e)
        {
            ventanaPanel(new InsertarFila(ruta));
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            ventanaPanel(new Modificar(ruta));
        }

        private void btnVenta_Click(object sender, EventArgs e)
        {
            ventanaPanel(new Venta(ruta));
        }
    }
}
