using Grafos.Models;
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
    /// Lógica de interacción para ExpandirMatrizWindow.xaml
    /// </summary>
    public partial class ExpandirMatrizWindow : Window
    {
        private readonly int[,] matriz;
        public ExpandirMatrizWindow(int[,] matrizz, List<Nodo> nodos)
        {
            InitializeComponent();
            matriz = matrizz;
            LimpiarColumansLV();
            ElaborarColumnasLV(nodos);
            RellenarCampos(matriz, nodos.Count);
        }

        private void LimpiarColumansLV()
        {
            lv.Items.Clear();
        }
        private void ElaborarColumnasLV(List<Nodo> nodos)
        {
            var gridView = new GridView();

            lv.View = gridView;


            gridView.Columns.Add(new GridViewColumn
            {
                Width = 60,
                Header = "Nodos",
                DisplayMemberBinding = new Binding("[" + 0.ToString() + "]")
            });


            for (int i = 1; i <= nodos.Count; i++)
            {

                gridView.Columns.Add(new GridViewColumn
                {
                    Width = 80,
                    Header = "Nodo " + nodos[i - 1].Id,
                    DisplayMemberBinding = new Binding("[" + i.ToString() + "]")
                });
            }


        }
        private void RellenarCampos(int[,] nodos, int size)
        {

            //ReemplazarNulls(size);
            List<string> item;

            for (int i = 0; i < size; i++)
            {
                item = new List<string>();
                item.Add("Nodo " + (i + 1).ToString());

                for (int j = 0; j < size; j++)
                {
                    item.Add(matriz[i, j].ToString());
                }
                lv.Items.Add(item);
            }
        }

        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) this.Close();
        }
    }
}
