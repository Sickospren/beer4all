using SpreadsheetLight;
using System;
using System.Windows.Forms;

namespace AppCervezasV6
{
    public partial class InsertarFila : Form
    {
        String ruta;
        public InsertarFila(String rutaArchivo)
        {
            InitializeComponent();
            ruta = rutaArchivo;
        }

        private Boolean comprobar() {

            int contador = 0;

            if (txtNombre.Text.Equals(""))
            {
                MessageBox.Show("NOMBRE VACIO");
            }
            else {

                contador++;
            }

            if (txtMarca.Text.Equals(""))
            {
                MessageBox.Show("MARCA VACIO");
            }
            else
            {

                contador++;
            }

            if (txtTipo.Text.Equals(""))
            {
                MessageBox.Show("TIPO VACIO");
            }
            else
            {

                contador++;
            }

            if (txtEnvase.Text.Equals(""))
            {
                MessageBox.Show("ENVASE VACIO");
            }
            else
            {

                contador++;
            }

            if (txtCapacidad.Text.Equals(""))
            {
                MessageBox.Show("CAPACIDAD VACIO");
            }
            else
            {

                contador++;
            }

            if (txtPrecio.Text.Equals(""))
            {
                MessageBox.Show("PRECIO VACIO");
            }
            else
            {

                contador++;
            }

            if (txtUnidades.Text.Equals(""))
            {
                MessageBox.Show("UNIDADES VACIO");
            }
            else
            {

                contador++;
            }

            if (contador == 7)
            {
                return true;
            }
            else 
            { 
                return false; 
            }

        }

        private void vaciar()
        {
            txtNombre.Text = "";
            txtMarca.Text = "";
            txtTipo.Text = "";
            txtEnvase.Text = "";
            txtCapacidad.Text = "";
            txtPrecio.Text = "";
            txtUnidades.Text = "";

        }

        private void insertar()
        {
            if (comprobar() == true)
            {
                using (SLDocument sl = new SLDocument(ruta))
                {
                    sl.SelectWorksheet("INVENTARIO");
                    int ultimaFila = sl.GetWorksheetStatistics().EndRowIndex;
                    int id = sl.GetCellValueAsInt32("A" + ultimaFila) + 1;
                    int disponible = ultimaFila + 1;


                    sl.SetCellValue("A" + disponible, id);
                    sl.SetCellValue("B" + disponible, txtNombre.Text.ToString());
                    sl.SetCellValue("C" + disponible, txtMarca.Text.ToString());
                    sl.SetCellValue("D" + disponible, txtTipo.Text.ToString());
                    sl.SetCellValue("E" + disponible, txtEnvase.Text.ToString());
                    sl.SetCellValue("F" + disponible, int.Parse(txtCapacidad.Text));
                    sl.SetCellValue("G" + disponible, double.Parse(txtPrecio.Text));
                    sl.SetCellValue("H" + disponible, int.Parse(txtUnidades.Text));
                    sl.Save();


                }

                using (SLDocument sl = new SLDocument(ruta))
                {

                    sl.SelectWorksheet("TOTAL VENTAS");
                    int ultimaFilaT = sl.GetWorksheetStatistics().EndRowIndex;
                    int idT = sl.GetCellValueAsInt32("A" + ultimaFilaT) + 1;
                    int disponibleT = ultimaFilaT + 1;
                    int total = 0;

                    sl.SetCellValue("A" + disponibleT, idT);
                    sl.SetCellValue("B" + disponibleT, txtNombre.Text.ToString());
                    sl.SetCellValue("C" + disponibleT, txtMarca.Text.ToString());
                    sl.SetCellValue("D" + disponibleT, double.Parse(txtPrecio.Text));
                    sl.SetCellValue("E" + disponibleT, total);
                    sl.SetCellValue("F" + disponibleT, total);
                    sl.Save();
                }
                vaciar();
            }
        }

  

        private void btnInsertar_Click(object sender, EventArgs e)
        {
            insertar();
        }

        public static void validarLetras(KeyPressEventArgs v)
        {
            if (Char.IsLetter(v.KeyChar))
            {
                v.Handled = false;
            }
            else if (Char.IsSeparator(v.KeyChar))
            {
                v.Handled = false;
            }
            else if (Char.IsControl(v.KeyChar))
            {
                v.Handled = false;
            }
            else
            {
                v.Handled = true;
                MessageBox.Show("");
            }
        }

        public static void validarNumeros(KeyPressEventArgs v)
        {
            if (Char.IsDigit(v.KeyChar))
            {
                v.Handled = false;
            }
            else if (Char.IsSeparator(v.KeyChar))
            {
                v.Handled = false;
            }
            else if (Char.IsControl(v.KeyChar))
            {
                v.Handled = false;
            }
            else
            {
                v.Handled = true;
                MessageBox.Show("Solo se permiten numeros");
            }
        }

        public static void validarDecimales(KeyPressEventArgs v)
        {
            if (Char.IsDigit(v.KeyChar))
            {
                v.Handled = false;
            }
            else if (Char.IsSeparator(v.KeyChar))
            {
                v.Handled = false;
            }
            else if (Char.IsControl(v.KeyChar))
            {
                v.Handled = false;
            }
            else if (v.KeyChar.ToString().Equals(","))
            {
                v.Handled = false;
            }
            else
            {
                v.Handled = true;
                MessageBox.Show("Solo se permiten numeros y numeros con decimal");
            }
        }

        private void txtCapacidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            validarNumeros(e);
        }

        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            validarDecimales(e);
        }

        private void txtUnidades_KeyPress(object sender, KeyPressEventArgs e)
        {
            validarNumeros(e);
        }
    }
}
