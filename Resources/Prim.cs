using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafos.Resources
{
    class Prim
    {
        public int[,] InitAlgPrim(int[,] Matriz, List<string> ListaVertices, int inicio)
        {  //Llega la matriz a la que le vamos a aplicar el algoritmo
            bool[] marcados = new bool[ListaVertices.Count]; //Creamos un vector booleano, para saber cuales están marcados
            string vertice = ListaVertices.ElementAt(inicio-1); //Le introducimos un nodo aleatorio, o el primero
            return AlgPrim(Matriz, marcados, vertice, new int[Matriz.GetLength(0), Matriz.GetLength(1)], ListaVertices); //Llamamos al método recursivo mandándole 
        }                                                                                     //un matriz nueva para que en ella nos 
                                                                                              //devuelva el árbol final
        private int[,] AlgPrim(int[,] Matriz, bool[] marcados, string vertice, int[,] Final, List <string> ListaVertices)
        {
            marcados[ListaVertices.IndexOf(vertice)] = true;//marcamos el primer nodo
            int aux = -1;
            if (!TodosMarcados(marcados))
            { //Mientras que no todos estén marcados
                for (int i = 0; i < marcados.Length; i++)
                { //Recorremos sólo las filas de los nodos marcados
                    if (marcados[i])
                    {
                        for (int j = 0; j < Matriz.GetLength(1); j++)
                        {
                            if (Matriz[i, j] != 0)
                            {        //Si la arista existe
                                if (!marcados[j])
                                {         //Si el nodo no ha sido marcado antes
                                    if (aux == -1)
                                    {        //Esto sólo se hace una vez
                                        aux = Matriz[i, j];
                                    }
                                    else
                                    {
                                        aux = Math.Min(aux, Matriz[i, j]); //Encontramos la arista mínima
                                    }
                                }
                            }
                        }
                    }
                }
                //Aquí buscamos el nodo correspondiente a esa arista mínima (aux)
                for (int i = 0; i < marcados.Length; i++)
                {
                    if (marcados[i])
                    {
                        for (int j = 0; j < Matriz.GetLength(1); j++)
                        {
                            if (Matriz[i,j] == aux)
                            {
                                if (!marcados[j])
                                { //Si no ha sido marcado antes
                                    Final[i, j] = aux; //Se llena la matriz final con el valor
                                    Final[j, i] = aux;//Se llena la matriz final con el valor
                                    return Remarcar(Matriz, marcados, ListaVertices.Find(x => x.Contains(vertice)), Final, ListaVertices); //se llama de nuevo al método con
                                                                                                   //el nodo a marcar
                                }
                            }
                        }
                    }
                }
            }
            return Final;
        }

        private int[,] Remarcar(int[,] Matriz, bool[] marcados, string vertice, int[,] Final, List<string> ListaVertices)
        {
            marcados[ListaVertices.IndexOf(vertice)] = true;//marcamos el primer nodo
            int aux = -1;
            if (!TodosMarcados(marcados))
            { //Mientras que no todos estén marcados
                for (int i = 0; i < marcados.Length; i++)
                { //Recorremos sólo las filas de los nodos marcados
                    if (marcados[i])
                    {
                        for (int j = 0; j < Matriz.GetLength(1); j++)
                        {
                            if (Matriz[i, j] != 0)
                            {        //Si la arista existe
                                if (!marcados[j])
                                {         //Si el nodo no ha sido marcado antes
                                    if (aux == -1)
                                    {        //Esto sólo se hace una vez
                                        aux = Matriz[i, j];
                                    }
                                    else
                                    {
                                        aux = Math.Min(aux, Matriz[i, j]); //Encontramos la arista mínima
                                    }
                                }
                            }
                        }
                    }
                }
                //Aquí buscamos el nodo correspondiente a esa arista mínima (aux)
                for (int i = 0; i < marcados.Length; i++)
                {
                    if (marcados[i])
                    {
                        for (int j = 0; j < Matriz.GetLength(1); j++)
                        {
                            if (Matriz[i, j] == aux)
                            {
                                if (!marcados[j])
                                { //Si no ha sido marcado antes
                                    Final[i, j] = aux; //Se llena la matriz final con el valor
                                    Final[j, i] = aux;//Se llena la matriz final con el valor
                                                      //return AlgPrim(Matriz, marcados, ListaVertices.Find(x => x.Contains(vertice)), Final, ListaVertices); //se llama de nuevo al método con
                                                      //el nodo a marcar
                                }
                            }
                        }
                    }
                }
            }
            return Final;
        }

        public bool TodosMarcados(bool[] vertice)
        { //Método para saber si todos están marcados
            foreach (bool b in vertice)
            {
                if (!b)
                {
                    return b;
                }
            }
            return true;
        }
    }
}
