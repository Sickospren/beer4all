using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppCervezasV6
{
    public partial class Selector : Form
    {

        String ruta;
        public Selector()
        {
            InitializeComponent();
            txtSelector.ReadOnly = true;
            btnInicio.Enabled = false;
        }

        private void btnSeleccionar_Click(object sender, EventArgs e)
        {
            OpenFileDialog seleccion = new OpenFileDialog();
            seleccion.Filter = "Libro de excel|*.xlsx";
            seleccion.Title = "Seleccion de plantilla";
            seleccion.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (seleccion.ShowDialog() == DialogResult.OK)
            {
                ruta = seleccion.FileName;
                txtSelector.Text = ruta;

                if (!string.IsNullOrEmpty(txtSelector.Text))
                {
                    btnInicio.Enabled = true;

                }

            }

        }

        private void txtSelecctor_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnInicio_Click(object sender, EventArgs e)
        {
            Inicio i = new Inicio(ruta);
            this.Hide();
            i.Show();
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            string esctitorio = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            ruta = Path.Combine(esctitorio, "cervezas.xlsx");

            using (SLDocument sl = new SLDocument())
            {

                sl.SetCellValue("A1", "ID");
                sl.SetCellValue("B1", "NOMBRE");
                sl.SetCellValue("C1", "MARCA");
                sl.SetCellValue("D1", "TIPO");
                sl.SetCellValue("E1", "ENVASE");
                sl.SetCellValue("F1", "CAPACIDAD");
                sl.SetCellValue("G1", "PRECIO");
                sl.SetCellValue("H1", "UNIDADES");
                sl.RenameWorksheet(SLDocument.DefaultFirstSheetName, "INVENTARIO");

                sl.AddWorksheet("VENTAS");
                sl.SelectWorksheet("VENTAS");
                sl.SetCellValue("A1", "IDVENTA");
                sl.SetCellValue("B1", "CLIENTE");
                sl.SetCellValue("C1", "IDPRODUCTO");
                sl.SetCellValue("D1", "CANTIDAD");
                sl.SetCellValue("E1", "PRECIO");

                sl.AddWorksheet("TOTAL VENTAS");
                sl.SelectWorksheet("TOTAL VENTAS");
                sl.SetCellValue("A1", "IDPRODUCTO");
                sl.SetCellValue("B1", "NOMBRE");
                sl.SetCellValue("C1", "MARCA");
                sl.SetCellValue("D1", "PRECIO");
                sl.SetCellValue("E1", "TOTALVENTAS");
                sl.SetCellValue("F1", "GANANCIAS TOTALES");

                sl.SelectWorksheet("INVENTARIO");

                sl.SaveAs(ruta);
                txtSelector.Text = ruta;

                if (!string.IsNullOrEmpty(txtSelector.Text))
                {
                    btnInicio.Enabled = true;

                }


            }
        }
    }
}
