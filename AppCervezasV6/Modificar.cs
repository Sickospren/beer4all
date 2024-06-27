using DocumentFormat.OpenXml.Drawing;
using SpreadsheetLight;
using System;
using System.Data;
using System.Windows.Forms;

namespace AppCervezasV6
{
    public partial class Modificar : Form
    {
        String ruta;
        Boolean casillaEncontrada = false;
        Boolean cbxSeleccionado = false;
        Boolean modificado = false;
        public Modificar(string rutaArchivo)
        {
            InitializeComponent();
            ruta = rutaArchivo;
            leerArchivo();
            txtModificar.Enabled = false;
            btnModificar.Enabled = false;
            cbxColumnas.DropDownStyle = ComboBoxStyle.DropDownList;
            llenarCbx();
        }

        private void leerArchivo()
        {

            DataTable dataTable = new DataTable();
            using (SLDocument sl = new SLDocument(ruta))
            {
                sl.SelectWorksheet("INVENTARIO");
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
                dataGridView1.DataSource = dataTable;

            }

        }


        private void filtrarPalabra(String filtro)
        {

            using (SLDocument sl = new SLDocument(ruta))
            {
                sl.SelectWorksheet("INVENTARIO");
                DataTable dataTable = new DataTable();
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


                        for (int x = 2; x <= estadisticas.EndColumnIndex; x++)
                        {
                            String valorCelda = sl.GetCellValueAsString(i, x);
                            if (valorCelda.Equals(filtro, StringComparison.OrdinalIgnoreCase))
                            {
                                DataRow row = dataTable.NewRow();
                                for (int j = 1; j <= estadisticas.EndColumnIndex; j++)
                                {

                                    row[j - 1] = sl.GetCellValueAsString(i, j);

                                }
                                dataTable.Rows.Add(row);
                            }
                        }
                    }
                }
                dataGridView1.DataSource = dataTable;
            }
        }

        private void btnFiltro_Click(object sender, EventArgs e)
        {
            if (txtFiltro.Text.Equals(""))
            {
                leerArchivo();
            }
            else
            {
                filtrarPalabra(txtFiltro.Text);
            }
        }

        int filaMod;


        private void seleccionarFila()
        {


            using (SLDocument sl = new SLDocument(ruta))
            {
                sl.SelectWorksheet("INVENTARIO");
                string txt = txtSelectId.Text;
                int seleccionado = int.Parse(txt);
                SLWorksheetStatistics estadisticas = sl.GetWorksheetStatistics();

                for (int fila = 1; fila <= estadisticas.EndRowIndex; fila++)
                {
                    int valor = sl.GetCellValueAsInt32(fila, 1);

                    if (valor == seleccionado)
                    {

                        casillaEncontrada = true;
                        filaMod = fila;
                        break;

                    }

                }

                if (!casillaEncontrada)
                {
                    MessageBox.Show("No se ha encontrado la id", "notFound", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        }

        private void llenarCbx()
        {
            cbxColumnas.Items.Add("NOMBRE");
            cbxColumnas.Items.Add("MARCA");
            cbxColumnas.Items.Add("TIPO");
            cbxColumnas.Items.Add("ENVASE");
            cbxColumnas.Items.Add("CAPACIDAD");
            cbxColumnas.Items.Add("PRECIO");
            cbxColumnas.Items.Add("UNIDADES");
        }

        private void comprobarCBX()
        {

            if (cbxColumnas.SelectedIndex == -1)
            {

                MessageBox.Show("No se ha seleccionado columna", "notFound", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {
                cbxSeleccionado = true;
            }

        }

        private Boolean comprobarCajaId() {
            if (txtSelectId.Text.Equals(""))
            {
                MessageBox.Show("no has seleccionado ninguna id");
                return false;
            }
            else {
                return true;
            }
        }

        private void btnSeleccionarMod_Click(object sender, EventArgs e)
        {
            if (comprobarCajaId()) {

                seleccionarFila();
                comprobarCBX();
                if (casillaEncontrada && cbxSeleccionado)
                {
                    txtModificar.Enabled = true;
                    btnModificar.Enabled = true;

                }

            }
            
        }

        private void modificar()
        {
            int seleccionado = cbxColumnas.SelectedIndex;
            String columna = "";
            int tipo = 0;
            Boolean comun = false;

            switch (seleccionado)
            {
                case 0:
                    columna = "B";
                    comun = true;
                    break;
                case 1:
                    columna = "C";
                    comun = true;
                    break;
                case 2:
                    columna = "D";
                    break;
                case 3:
                    columna = "E";
                    break;
                case 4:
                    columna = "F";
                    tipo = 1;
                    break;
                case 5:
                    columna = "G";
                    comun = true;
                    tipo = 2;
                    break;
                case 6:
                    columna = "H";
                    tipo = 1;
                    break;
            }

            String modificar = columna + filaMod.ToString();

            Console.WriteLine("fila a modificar: " + modificar);
            Console.WriteLine("tipo:   " + tipo);

            using (SLDocument sl = new SLDocument(ruta))
            {
                sl.SelectWorksheet("INVENTARIO");
                if (txtModificar.Text.Equals(""))
                {
                    MessageBox.Show("caja de texto vacia", "empty", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (tipo == 0)
                    {
                        sl.SetCellValue(modificar, txtModificar.Text);

                    }
                    else if (tipo == 1)
                    {
                        sl.SetCellValue(modificar, int.Parse(txtModificar.Text));
                    }
                    else if (tipo == 2)
                    {
                        sl.SetCellValue(modificar, double.Parse(txtModificar.Text));
                    }
                    modificado = true;


                }

                sl.Save();

            }

            if (comun)
            {
                switch (seleccionado)
                {

                    case 0:
                        columna = "B";
                        break;
                    case 1:
                        columna = "C";
                        break;
                    case 5:
                        columna = "D";
                        tipo = 2;
                        break;

                }
                modificar = columna + filaMod.ToString();
                using (SLDocument sl = new SLDocument(ruta))
                {
                    sl.SelectWorksheet("TOTAL VENTAS");
                    if (txtModificar.Text.Equals(""))
                    {
                        MessageBox.Show("caja de texto vacia", "empty", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        if (tipo == 0)
                        {
                            sl.SetCellValue(modificar, txtModificar.Text);
                        }
                        else if (tipo == 2)
                        {
                            sl.SetCellValue(modificar, double.Parse(txtModificar.Text));
                        }

                    }
                    sl.Save();

                }
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            modificar();
            if (modificado)
            {
                modificado = false;
                txtModificar.Text = "";
                txtModificar.Enabled = false;
                txtSelectId.Text = "";
                cbxColumnas.SelectedIndex = -1;
                btnModificar.Enabled = false;
                leerArchivo();

            }
        }


        private void txtSelectId_KeyPress(object sender, KeyPressEventArgs e)
        {
            InsertarFila.validarNumeros(e);
        }

        private int verTipo()
        {
            int seleccionado = cbxColumnas.SelectedIndex;
            int salida = 0;
            switch (seleccionado)
            {

                case 4:
                    salida = 1;
                    break;
                case 5:
                    salida = 2;
                    break;
                case 6:
                    salida = 1;
                    break;
            }
            return salida;


        }

        private void txtModificar_KeyPress(object sender, KeyPressEventArgs e)
        {
            int tipo = verTipo();
            if (tipo == 1)
            {
                InsertarFila.validarNumeros(e);

            }
            else if (tipo == 2)
            {
                InsertarFila.validarDecimales(e);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
