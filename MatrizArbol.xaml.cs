using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Grafos
{
    /// <summary>
    /// Lógica de interacción para MatrizArbol.xaml
    /// </summary>
    public partial class MatrizArbol : Window
    {
        public int[,] matriz;
        public int[] nodos;

        public MatrizArbol(int [,] matriz, int[] nodos)
        {
            this.matriz = matriz;
            this.nodos = nodos;
            InitializeComponent();
            ElaborarColumnasLV(nodos);
        }
        private void LimpiarColumansLV()
        {
            lv.Items.Clear();
        }

        private void ElaborarColumnasLV(int[] nodos)
        {
            var gridView = new GridView();
            lv.View = gridView;


            gridView.Columns.Add(new GridViewColumn
            {
                Width = 60,
                Header = "Nodos",
                DisplayMemberBinding = new Binding("[" + 0.ToString() + "]")
            });


            for (int i = 1; i <= matriz.GetLength(0); i++)
            {

                gridView.Columns.Add(new GridViewColumn
                {
                    Width = 80,
                    Header = "nodo_" + nodos[i - 1],
                    DisplayMemberBinding = new Binding("[" + i.ToString() + "]")
                });
            }

            RellenarCampos(nodos, matriz.GetLength(0));
        }

        private void RellenarCampos(int[] nodos, int size)
        {
            List<string> item;

            for (int i = 0; i < size; i++)
            {
                item = new List<string>();
                item.Add("Nodo" + (i + 1).ToString());

                for (int j = 0; j < size; j++)
                {
                    item.Add(matriz[i, j].ToString());
                }
                lv.Items.Add(item);
            }
        }
    }
}
