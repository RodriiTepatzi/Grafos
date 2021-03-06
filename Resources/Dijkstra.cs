using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Grafos.Resources
{
    class Dijkstra
    {
        private int contador = 0;
        private int aristas = 0;
        private int[,] matrizRecorrido;
        private int[] nodos = new int[30];//Arreglo donde se almacenaran los nodos en los recorridos
        private string nombreGrafo = "";

        public void setNodoInicio(int nodoInicio)
        {
            this.nodos[0] = nodoInicio;
        }
        /*Captura el tamaño del archivo y retorna una matriz con las dimensiones: "tamaño X 3", donde se 
         * almacenaran los datos del archivo
         */
        public int[,] tamanioMatriz(int[,] matrizTemporal, string nombre)
        {
            int tamanio = 0;//contador que va incrementando con cada recorrido para encontrar la cantidad de aristas
            string linea;//Variable donde se almacenara cada linea del archivo de texto
            this.nombreGrafo = nombre;//variable que contendra el nombre del archivo de texto sin extension

            System.IO.StreamReader file =
                new System.IO.StreamReader(@"c:\Grafos\" + nombre + ".txt"); //Linea que sirve para leer el archivo de texto

            while ((linea = file.ReadLine()) != null)
            {
                tamanio++;
            }
            aristas = tamanio;// variable que contendra el numero de aristas en el grafo
            matrizTemporal = new int[aristas, 3]; // se establece las dimensiones de la matriz
            file.Close();

            return matrizTemporal;
        }


        public void imprimirMatriz(int[,] matriz)
        {

            Console.Write("   ");
            for (int i = 0; i < matriz.GetLength(0); i++)
            {
                Console.Write(" " + (char)+(65 + i) + " ");
            }

            Console.WriteLine();
            for (int i = 0; i < matriz.GetLength(0); i++)
            {
                Console.Write(" " + (char)+(65 + i) + " ");
                for (int j = 0; j < matriz.GetLength(0); j++)
                {
                    Console.Write(" " + matriz[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        public void imprimirRecorrido(int[,] matriz)
        {
            for (int i = 0; i < matriz.GetLength(0); i++)
            {
                for (int j = 0; j < matriz.GetLength(1); j++)
                {
                    Console.Write(" " + matriz[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        public void algoritmoDijkstra(int[,] matriz, int nodo, int pesoNodoInicial, int bandera, int nodoTemp)
        {
            matrizRecorrido = matriz;
            int temp = 0;

            //Con este if se valida que las aristas existentes sean mayores a el numero de columnas diferentes
            //de cero en la matriz recorrido y que el nodo sea diferente al nodo anterior
            if (aristas > tamanioRecorrido(matrizRecorrido) && nodo != nodoTemp)
            {
                nodoTemp = nodo;//Si es diferente entonces se iguala el nodo temporal al nodo para que posteriormente
                                //sea utilizado para compararse
                for (int i = 0; i < matriz.GetLength(1); i++)
                {
                    //Si en la matriz en la columna del nodo no existe algun cero(contiene algun peso) entonces deja pasar 
                    if (matriz[nodo, i] != 0)
                    {
                        matrizRecorrido[contador, 0] = nodo;
                        matrizRecorrido[contador, 1] = matriz[nodo, i] + pesoNodoInicial;
                        matrizRecorrido[contador, 2] = i;
                        contador++;
                    }
                }


                //Estos for anidados recorren ambos las columnas
                for (int i = 0; i < matrizRecorrido.GetLength(0); i++)
                {
                    for (int j = 0; j < matrizRecorrido.GetLength(0); j++)
                    {
                        //Si la columna "i" es diferente a la "j" significa que estamos comparando diferentes columnas y se cumple la condicion.
                        //Se compara la matrizRecorrido en la posicion 2 con "i" y "j" verificando que sean diferentes de 0 ya que se espera que exista un valor
                        //Se compara la matrizRecorrido en la posicion 3 con "i" y "j" verificando que sean diferentes de 100 ya que este es un valor fuera del rango
                        if (i != j && matrizRecorrido[i, 2] != 0 && matrizRecorrido[j, 2] != 0 && matrizRecorrido[i, 3] != 100 && matrizRecorrido[j, 3] != 100)
                        {
                            //Se compara la matrizRecorrido en la posicion 2 con "i" y "j" para saber si son iguales y poder bloquear el que tenga un peso mayor
                            if (matrizRecorrido[i, 2] == matrizRecorrido[j, 2])
                            {
                                //en dado caso que el peso del primero sea menor o igual al segundo, entonces se bloquea el segundo con el numero 100
                                if (matrizRecorrido[i, 1] <= matrizRecorrido[j, 1])
                                {
                                    matrizRecorrido[j, 3] = 100;
                                }
                                else
                                {
                                    //Si el primer valor es mayor entonces se bloquea este
                                    matrizRecorrido[i, 3] = 100;
                                }
                            }
                        }
                    }
                }

                //Ahora se buscara el menor de todos los nodos hijos para continuar con el recorrido
                pesoNodoInicial = 0;
                for (int i = 0; i < matrizRecorrido.GetLength(0); i++)
                {
                    //Se recorre matrizRecorrido en cada columna en su posicion 3 para verificar que no aya sido bloqueada y se verifica que el peso inicial sea de cero
                    //si se cumple lo anterior asigna el peso inicial de la primera letra y la pone en el peso inicial para tomarla como base en los recorridos
                    for (int k = 0; k < matrizRecorrido.GetLength(0); k++)
                    {
                        if (matrizRecorrido[k, 3] == 0 && pesoNodoInicial == 0)
                        {
                            pesoNodoInicial = matrizRecorrido[k, 1];

                        }
                    }
                    //Se verifica que en la matrizRecorrido en la posicion 1 sea menor al peso inicial, tambien se verifica que la bandera sea igual a 0 y que 
                    //en la posicion de la letra inicial de la matrizRecorrido sea diferente de cero, osea que exista!
                    if (matrizRecorrido[i, 1] <= pesoNodoInicial && matrizRecorrido[i, 3] == 0 && matrizRecorrido[i, 1] != 0)
                    {
                        //De cumplir las condiciones se asigna el nuevo peso a la variable pesoInicial, el nodo inicial se guarda en temp y 
                        //tambien se guarda el nodo al que llega en nodo
                        pesoNodoInicial = matrizRecorrido[i, 1];
                        nodo = matrizRecorrido[i, 2];
                        temp = i;
                    }

                }
                bandera++;//la bandera se incrementa
                matrizRecorrido[temp, 3] = bandera;// se guarda el valor de la bandera en la matrizRecorrido en la posicion del nodo menor para que se bloquee

                algoritmoDijkstra(matriz, nodo, pesoNodoInicial, bandera, nodoTemp);//Se recursa mandando a llamar el metodo mandando como parametros la matriz
                                                                                    //de un principio, el nuevo nodo(es el menor de los hijos), el peso anterior, la bandera autoincrementada y el nodo que se 
                                                                                    //utilizo en el recorrido
                                                                                    //Si el recorrido se inicia desde un valor que no sea el "A" o "0" entonces quedan recorridos vacios en la matrizRecorrido, entonces se coloca
                                                                                    //el ultimo valor que tomo la bandera para bloquearlos
                for (int i = 0; i < matrizRecorrido.GetLength(0); i++)
                {
                    if (matrizRecorrido[i, 3] == 0)
                    {
                        matrizRecorrido[i, 3] = bandera;
                    }
                }

            }//Fin condicion

            contador = 0;
            aristas = 0;
        }
        //Se busca el tamaño de la matrizRecorrido cada vez que se inserta uno o varios valores para la valdacion 
        //del metodo algoritmoDijkstra
        public int tamanioRecorrido(int[,] recorrido)
        {
            int temp = 0;
            for (int i = 0; i < recorrido.GetLength(0); i++)
            {
                if (recorrido[i, 1] != 0)
                {
                    temp++;
                }
            }
            return temp;
        }

        //Metodo para imprimir la matriz recorrido desde la clase Program donde esta el menu
        public void imprimir() { imprimirRecorrido(matrizRecorrido); }

        //Este metodo se encarga de hacer el recorrido una vez que ya se aplico el algoritmoDijkstra, 
        //recive el nodo al que se quiere llegar
        public void recorridoMinimo(int nodoFinal, int[,] matriz)
        {
            matrizRecorrido = matriz;

            //Verifica que el nodoFinal sea diferente al arreglo de nodos en su posicion 0 ya que en esa se encuentra el valor con el que se inicio
            //el recorrido en el algoritmo dijkstra, y en cuanto se parescan el recorrido se termina
            if (nodoFinal != nodos[0])
            {
                for (int i = 0; i < matrizRecorrido.GetLength(0); i++)
                {
                    //Se valida si la matrizRecorrido es menor a 100 ya que este significa que el nodo es invalido y verifica tambien que la matrizRecorrido en su
                    //posicion 2 con cada recorrido de i sea igual al nodoFinal, si esto cumple pasa a la recursividad
                    if (matrizRecorrido[i, 3] < 100 && matrizRecorrido[i, 2] == nodoFinal)
                    {
                        //Si entra manda a llamar este mismo metodo pero mandando la letra anterior al nodoFinal
                        recorridoMinimo(matrizRecorrido[i, 0], matriz);
                    }
                }
            }
            //Por ultimo se imprimen cada uno de los valores que fueron nodoFinal antes de mandarlos por el metodo recursivo
            Console.Write((char)+(nodoFinal + 65) + " ");

        }
    }
}
