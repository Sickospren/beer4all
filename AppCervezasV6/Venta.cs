using DocumentFormat.OpenXml.Wordprocessing;
using SpreadsheetLight;
using System;
using System.Collections;
using System.Data;
using System.Windows.Forms;


namespace AppCervezasV6
{
    public partial class Venta : Form
    {
        String ruta;
        ArrayList arrayVenta = new ArrayList();
        int stockDisponible = 0;
        int valorFila;
        DataTable dataTable;
        double precio;
        double precioVentana = 0;

        public Venta(String rutaArchivo)
        {
            InitializeComponent();
            ruta = rutaArchivo;
            txtCantCompra.Enabled = false;
            btnCantidad.Enabled = false;
            btnProcesar.Enabled = false;
            txtIdVenta.Text = "ID VENTA: " + (ultimaVenta() + 1).ToString();
        }

        private Boolean comprobarCajaID() {

            if (txtID.Text.Equals(""))
            {
                MessageBox.Show("ID VACIO");
                return false;
            }
            else {
                return true;
            }
        }

        private void comprobar()
        {

            using (SLDocument sl = new SLDocument(ruta))
            {
                sl.SelectWorksheet("INVENTARIO");
                Boolean encontrado = false;
                if (comprobarCajaID()) { 
                 
                    String valorCaja = txtID.Text;
                    int comprobar = int.Parse(valorCaja);
                

                    SLWorksheetStatistics estadisticas = sl.GetWorksheetStatistics();

                    for (int fila = 2; fila <= estadisticas.EndRowIndex; fila++)
                    {
                        int valor = sl.GetCellValueAsInt32(fila, 1);

                        if (valor == comprobar)
                        {
                            String celda = "H" + fila;
                            String celdaP = "G" + fila;
                            stockDisponible = sl.GetCellValueAsInt32(celda);
                            precio = sl.GetCellValueAsDouble(celdaP);
                            txtExistencias.Text = "Stock: "+stockDisponible.ToString();
                            valorFila = fila;
                            encontrado = true;
                            sl.Save();
                            txtCantCompra.Enabled = true;
                            btnCantidad.Enabled = true;
                            break;

                        }

                    }
                    if (!encontrado)
                    {
                        MessageBox.Show("No existe ese id", "notFound404", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }


        }

        private int ultimaVenta()
        {

            using (SLDocument sl = new SLDocument(ruta))
            {

                sl.SelectWorksheet("VENTAS");

                int ultimaFila = sl.GetWorksheetStatistics().EndRowIndex;

                string celda = "A" + ultimaFila;

                int ultimaIdVenta = sl.GetCellValueAsInt32(celda);

                return ultimaIdVenta;
            }

        }

        private Boolean comprobarCajaUnidades()
        {

            if (txtCantCompra.Text.Equals(""))
            {
                MessageBox.Show("UNIDADES VACIO");
                return false;
            }
            else
            {
                return true;
            }
        }

        private void llenarListaVentas()
        {

            int unidades = int.Parse(txtCantCompra.Text);

            if (unidades <= stockDisponible)
            {
                stockDisponible = stockDisponible - unidades;
                using (SLDocument sl = new SLDocument(ruta))
                {
                    sl.SelectWorksheet("INVENTARIO");
                    String filaUnidades = "H" + valorFila;
                    String filaProducto = "A" + valorFila;
                    sl.SetCellValue(filaUnidades, stockDisponible);
                    double precioT = unidades * precio;

                    precioVentana += precioT;

                    String cliente = txtCliente.Text;

                    if (cliente.Equals(""))
                    {
                        cliente = "Sin Registar";
                    }


                    String[] registroVenta = new string[5];
                    registroVenta[0] = (ultimaVenta() + 1).ToString();
                    registroVenta[1] = cliente;
                    registroVenta[2] = sl.GetCellValueAsString(filaProducto);
                    registroVenta[3] = unidades.ToString();
                    registroVenta[4] = precioT.ToString();
                    arrayVenta.Add(registroVenta);
                    sl.Save();

                }
                cargarLista(arrayVenta);
                txtPrecio.Text = precioVentana.ToString()+"€";

            }
            else
            {

               MessageBox.Show("No hay unidades suficientes", "Sin Stock", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }


        }

        private void cargarLista(ArrayList lista)
        {

            int precioTotal;

            dataTable = new DataTable();

            String[] cabecera = { "IDVENTA", "CLIENTE", "IDPRODUCTO", "CANTIDAD", "PRECIO" };

            foreach (String i in cabecera)
            {
                dataTable.Columns.Add(i.ToString());

            }

            foreach (String[] s in lista)
            {
                int columnas = dataTable.Columns.Count;
                DataRow row = dataTable.NewRow();
                Console.WriteLine("arrrayyyy " + s.Length);
                for (int z = 0; z < s.Length; z++)
                {
                    row[z] = s[z];
                }

                dataTable.Rows.Add(row);
            }
            dataGridViewVenta.DataSource = dataTable;
            txtCantCompra.Text = "";
            txtID.Text = "";
            txtCantCompra.Enabled = false;
            btnCantidad.Enabled = false;
            btnProcesar.Enabled = true;


        }


        private void guardarVenta(ArrayList lista)
        {

            using (SLDocument sl = new SLDocument(ruta))
            {
                sl.SelectWorksheet("VENTAS");
                SLWorksheetStatistics stats = sl.GetWorksheetStatistics();
                int ultimaFila = stats.EndRowIndex + 1;
                String IDVENTA, CLIENTE, IDPRODUCTO, CANTIDAD, PRECIO;



                foreach (String[] s in lista)
                {
                    IDVENTA = "A" + ultimaFila;
                    CLIENTE = "B" + ultimaFila;
                    IDPRODUCTO = "C" + ultimaFila;
                    CANTIDAD = "D" + ultimaFila;
                    PRECIO = "E" + ultimaFila;

                    int i = 0;

                    sl.SetCellValue(IDVENTA, int.Parse(s[i]));
                    sl.SetCellValue(CLIENTE, s[i + 1]);
                    sl.SetCellValue(IDPRODUCTO, int.Parse(s[i + 2]));
                    sl.SetCellValue(CANTIDAD, int.Parse(s[i + 3]));
                    sl.SetCellValue(PRECIO, double.Parse(s[i + 4]));

                    ultimaFila++;
                    i = 0;
                }
                sl.Save();
                btnProcesar.Enabled = false;
                txtCliente.Text = "";
            }
        }

        private void btnComprobarID_Click(object sender, EventArgs e)
        {
            comprobar();

        }

        private void btnCantidad_Click(object sender, EventArgs e)
        {

            if (comprobarCajaUnidades()) 
            {
                llenarListaVentas();
                cargarLista(arrayVenta); 
            }
            
        }
        private void btnProcesar_Click(object sender, EventArgs e)
        {
            guardarVenta(arrayVenta);
        }

        private void txtID_KeyPress(object sender, KeyPressEventArgs e)
        {
            InsertarFila.validarNumeros(e);
        }

        private void txtCantCompra_KeyPress(object sender, KeyPressEventArgs e)
        {
            InsertarFila.validarNumeros(e);
        }
    }
}
