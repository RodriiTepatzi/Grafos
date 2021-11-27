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
using EncontrarElemento = Grafos.Resources.EncontrarElemento;
using NodoH = Grafos.Controllers.NodoController;

namespace Grafos
{
    public partial class ArbolGenerador : Window 
    {
        public int[,] matriz { get; set; }
        public int inicio { get; set; }
        EncontrarElemento IHelper = new EncontrarElemento();
        NodoH NodoHelper = new NodoH();
        private List<int> analizados = new List<int>();

        public ArbolGenerador(int[,]matriz, int inicio)
        {
            this.matriz = matriz;
            this.inicio = inicio;
            InitializeComponent();
            GenerarGraficos();
        }

        private void GenerarGraficos()
        {
            //Obtenemos los nodos del arbol
            for (int i = 0; i < matriz.GetLength(0); i++)
            {
                for (int j = 0; j < matriz.GetLength(1); j++)
                {
                    if (matriz[i,j] != 0)
                    {
                        if (!analizados.Exists(x => x == i))
                        {
                            analizados.Add(i);
                        }
                    }
                }
            }

            //Dibujamos los nodos obteniendolos del canvas de la ventana principal
            for (int i = 0; i < analizados.Count; i++)
            {
                Console.WriteLine(analizados[i]);
                Ellipse nodo_padre = IHelper.FindChild<Ellipse>(Application.Current.MainWindow, "nodo_" + (analizados[i] + 1));
                /*Ellipse nodo_hijo = NodoHelper.CrearNodo(analizados[i]+1);
                nodo_hijo.SetValue(Canvas.LeftProperty, nodo_padre.GetValue(Canvas.LeftProperty));
                nodo_hijo.SetValue(Canvas.TopProperty, nodo_padre.GetValue(Canvas.TopProperty));
                
                canvass.Children.Add(nodo_hijo); */
            }
        }
    }
}
