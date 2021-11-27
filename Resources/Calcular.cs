using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Grafos.Controllers;
using Grafos.Models;

namespace Grafos.Resources
{
    class Calcular
    {

        EncontrarElemento IHelper = new EncontrarElemento();
        NodoController NodoHelper = new NodoController();
        AristaController AristaHelper = new AristaController();
        int[,] matriz;
        int V = 0;

        // ===VISTA DE LA TABLA===

        //        nodo_1 nodo _2 nodo_3 nodo_n
        // nodo_1   0       0      0      . 
        // nodo_2   1       0      1      .
        // nodo_3   0       1      0      .
        // nodo_n   .       .      .      .


        // ===VISTA DE LA MATRIZ===

        //  0   1   2   3   4
        //  1   0   1   0   0
        //  2   1   0   1   0   
        //  3   0   1   0   0
        //  4   0   0   0   0


        public int[,] CalcularMatriz(List<Nodo> nodos, List<Arista> aristas)
        {
            int TotalNodos = nodos.Count;
            int TotalAristas = aristas.Count;
            int idNodo1 = 0;
            int idNodo2 = 0;

            matriz = new int[TotalNodos, TotalNodos];

            foreach (var arista in aristas)
            {
                idNodo1 = arista.IdNodo1 - 1;
                idNodo2 = arista.IdNodo2 - 1;

                if (arista.Peso == 0)
                {
                    matriz[idNodo1, idNodo2] = 1;
                    matriz[idNodo2, idNodo1] = 1;
                }
                else
                {
                    matriz[idNodo1, idNodo2] = arista.Peso;
                    matriz[idNodo2, idNodo1] = arista.Peso;
                }
            }

            LimpiarColumansLV();
            ElaborarColumnasLV(nodos);
            RellenarCampos(matriz, nodos.Count);
            return matriz;
        }

        private void LimpiarColumansLV()
        {
            ListView lv = IHelper.FindChild<ListView>(Application.Current.MainWindow, "lv_matriz");
            lv.Items.Clear();
        }
        private void ElaborarColumnasLV(List<Nodo> nodos)
        {
            var gridView = new GridView();

            ListView lv = IHelper.FindChild<ListView>(Application.Current.MainWindow, "lv_matriz");
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

            ListView lv = IHelper.FindChild<ListView>(Application.Current.MainWindow, "lv_matriz");
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

        private void ReemplazarNulls(int size)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (matriz[i, j] != 1)
                    {
                        matriz[i, j] = 0;
                    }

                }
            }
        }

        /*  Paso 1. S ← {vinicial} //Inicialmente S contendrá el vértice //origen
       Paso 2. Para cada v∈V, v ≠ vinicial, hacer
           2.1. D[v] ← C[vinicial, v] //Inicialmente el costo del //camino mínimo de vinicial a v es lo contenido en //la matriz de costos
           2.2. P[v] ← vinicial //Inicialmente, el //predecesor de v en el camino mínimo construido //hasta el momento es vinicial
       Paso 3. Mientras (V – S ≠ ∅) hacer //Mientras existan vértices para //los cuales no se ha determinado el //camino mínimo
           3.1. Elegir un vértice w∈(V-S) tal que D[w] sea el mínimo.
           3.2. S ← S ∪ {w} //Se agrega w al conjunto S, pues ya se //tiene el camino mínimo hacia w 
           3.3. Para cada v∈(V-S) hacer
           3.3.1. D[v] ← min(D[v],D[w]+C[w,v]) //Se escoge, entre //el camino mínimo hacia v que se tiene //hasta el momento, y el camino hacia v //pasando por w mediante su camino mínimo, //el de menor costo.
           3.3.2. Si min(D[v],D[w]+C[w,v]) = D[w]+C[w,v] entonces P[v] ← w //Si se escoge ir por w entonces //el predecesor de v por el momento es w
       Paso 4. Fin */

        public void ResolverGrafo (int[,] matriz, int v_inicial, int nodos_total)
        {
            V = nodos_total;
            dijkstra(matriz, v_inicial);

        }

        public int[,] AgregarPesoMatriz (int[,] matriz, int nodo_1, int nodo_2, int peso, List<Nodo> nodos)
        {
            nodo_1 = nodo_1 - 1;
            nodo_2 = nodo_2 - 1;

            matriz[nodo_1, nodo_2] = peso;

            LimpiarColumansLV();
            ElaborarColumnasLV(nodos);

            return matriz;
        }

        private int minDistance(int[] dist,
                        bool[] sptSet)
        {
            int min = int.MaxValue, min_index = -1;

            for (int v = 0; v < V; v++)
                if (sptSet[v] == false && dist[v] <= min)
                {
                    min = dist[v];
                    min_index = v;
                }

            return min_index;
        }

        private void printSolution(int[] dist, int n, int inicial)
        {
            TextBlock tb = IHelper.FindChild<TextBlock>(Application.Current.MainWindow, "salida_text");

            tb.Text = "Nodo     Distancia "
                          + "desde " + inicial +"\n";

            for (int i = 0; i < V; i++)
            {
                if (dist[i] != int.MaxValue) { tb.Text += (i + 1 + " \t\t " + dist[i] + "\n"); }
                
            }
        }

        private void dijkstra(int[,] grafo, int inicial)
        {
            inicial = inicial - 1;
            //Contiene la distancia desde el vertice inicial a i
            int[] dist = new int[V]; 
            // sptSet[i] guarada como verdadero si i esta incluido en recorrido o si del inicial a i ya esta finalizado
            bool[] sptSet = new bool[V];

            // inicializamos todas las distancias como infinito y sptSet en falso
            for (int i = 0; i < V; i++)
            {
                dist[i] = int.MaxValue;
                sptSet[i] = false;
            }

            // distancia desde el nodo inicial siempre sera 0
            dist[inicial] = 0;

            
            for (int count = 0; count < V - 1; count++)
            {
                // Pick the minimum distance vertex
                // from the set of vertices not yet
                // processed. u is always equal to
                // src in first iteration.
                int u = minDistance(dist, sptSet);

                // Mark the picked vertex as processed
                sptSet[u] = true;

                // Update dist value of the adjacent
                // vertices of the picked vertex.
                for (int v = 0; v < V; v++)

                    // Update dist[v] only if is not in
                    // sptSet, there is an edge from u
                    // to v, and total weight of path
                    // from src to v through u is smaller
                    // than current value of dist[v]
                    if (!sptSet[v] && grafo[u, v] != 0 &&
                         dist[u] != int.MaxValue && dist[u] + grafo[u, v] < dist[v])
                        dist[v] = dist[u] + grafo[u, v];
            }

            printSolution(dist, V, inicial+1);
        }
    }
}