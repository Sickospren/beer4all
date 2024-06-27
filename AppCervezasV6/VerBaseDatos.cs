using SpreadsheetLight;
using System;
using System.Collections;
using System.Data;
using System.Windows.Forms;

namespace AppCervezasV6
{
    public partial class VerBaseDatos : Form
    {
        String ruta;
        String hoja;
        public VerBaseDatos(String rutaArchivo)
        {

            ruta = rutaArchivo;
            hoja = "INVENTARIO";
            InitializeComponent();
            leerArchivo(hoja);
        }

        private void leerArchivo(String hoja)
        {

            DataTable dataTable = new DataTable();
            using (SLDocument sl = new SLDocument(ruta))
            {
                sl.SelectWorksheet(hoja);
                SLWorksheetStatistics estadisticas = sl.GetWorksheetStatistics();
                for (int i = 1; i <= estadisticas.EndRowIndex; i++)
                {

                    if (i == 1)
                    {

                        for (int j = 1; j <= estadisticas.EndColumnIndex; j++)
                        {
                            dataTable.Columns.Add(sl.GetCellValueAsString(i, j));
                        }

                    }
                    else
                    {
                        DataRow row = dataTable.NewRow();
                        for (int x = 1; x <= estadisticas.EndColumnIndex; x++)
                        {
                            row[x - 1] = sl.GetCellValueAsString(i, x);

                        }
                        dataTable.Rows.Add(row);

                    }

                }

                dataGridView.DataSource = dataTable;





            }

        }

        public void ventasTotales()
        {
            ArrayList ventasTotales = new ArrayList();
            int totalCervezas;
            //paso 1 sacar numero de productos en la bd
            using (SLDocument sl = new SLDocument(ruta))
            {
                sl.SelectWorksheet("TOTAL VENTAS");
                totalCervezas = sl.GetWorksheetStatistics().EndRowIndex - 1;
            }
            //ahora voy a ir sacando cuantas cervezas he venido de cada
            using (SLDocument sl = new SLDocument(ruta))
            {
                sl.SelectWorksheet("VENTAS");
                int totalFilas = sl.GetWorksheetStatistics().EndRowIndex;
                int totalVendidas = 0;
                for (int cerveza = 1; cerveza <= totalCervezas; cerveza++)
                {
                    Console.WriteLine("indice cervezas " + cerveza);
                    for (int row = 2; row <= totalFilas; row++)
                    {
                        String celdaCerve = "C" + row;
                        String celdaUnidades = "D" + row;

                        int idCerve = sl.GetCellValueAsInt32(celdaCerve);
                        if (idCerve == cerveza)
                        {
                            totalVendidas += sl.GetCellValueAsInt32(celdaUnidades);
                        }

                    }
                    ventasTotales.Add(totalVendidas);
                    totalVendidas = 0;
                }

            }

            Console.WriteLine("total de birras en la bd: " + totalCervezas);

            Console.WriteLine("total de birras en el arl: " + ventasTotales.Count);
            Console.WriteLine("----------");
            foreach (int x in ventasTotales)
            {
                Console.WriteLine("valo dentro de arl:  " + x);
            }

            using (SLDocument sl = new SLDocument(ruta))
            {

                sl.SelectWorksheet("TOTAL VENTAS");
                int indice = 2;

                foreach (int x in ventasTotales)
                {
                    String celda = "E" + indice;
                    sl.SetCellValue(celda, x);
                    indice++;
                }
                sl.Save();

            }

            using (SLDocument sl = new SLDocument(ruta))
            {
                sl.SelectWorksheet("TOTAL VENTAS");
                int numFilas = sl.GetWorksheetStatistics().EndRowIndex;
                for (int row = 2; row <= numFilas; row++)
                {
                    String celdaPrecio = "D" + row;
                    String celdaTotalV = "E" + row;
                    String ganancias = "F" + row;
                    double precio = sl.GetCellValueAsDouble(celdaPrecio);
                    int totalVendidas = sl.GetCellValueAsInt32(celdaTotalV);

                    double totalprecios = totalVendidas * precio;

                    sl.SetCellValue(ganancias, totalprecios);

                }
                sl.Save();



            }

        }

        private void btnInventario_Click(object sender, EventArgs e)
        {
            hoja = "INVENTARIO";
            dataGridView.DataSource = null;
            leerArchivo(hoja);
        }

        private void btnVentas_Click(object sender, EventArgs e)
        {
            hoja = "VENTAS";
            dataGridView.DataSource = null;
            leerArchivo(hoja);
        }

        private void btnTotal_Click(object sender, EventArgs e)
        {
            hoja = "TOTAL VENTAS";
            dataGridView.DataSource = null;
            ventasTotales();
            leerArchivo(hoja);
        }
    }
}
