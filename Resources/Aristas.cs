using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;



namespace Grafos.Resources
{
    class Aristas
    {
        public Aristas()
        {

        }

        public Line CrearAristas(double x1, double y1, double x2, double y2, string id_1, string id_2, char tipo_1, char tipo_2)
        {
            Line arista = new Line();
            arista.X1 = x1;
            arista.Y1 = y1;
            arista.X2 = x2;
            arista.Y2 = y2;
            arista.Stroke = Brushes.Black;
            arista.StrokeThickness = 1.5;
            arista.Name = tipo_1 + id_1 + "_" + tipo_2+ id_2;

            return arista;
        }

        public int TotalAristas(string[] aristas)
        {
            int total = 0;

            for (int i = 0; i < aristas.Length; i++)
            {
                if (!string.IsNullOrEmpty(aristas[i]))
                {
                    total++;
                }
            }

            return total;
        }
        
        public string[] AristasAMover(string[] aristas, string id)
        {
            int counter = 0;
            for (int i = 0; i < aristas.Length; i++)
            {
                if (!string.IsNullOrEmpty(aristas[i]))
                {
                    if (aristas[i].Contains(id))
                    {
                        counter++;
                    }
                }
            }
            string[] a_mover = new string[counter];
            int j = 0;
            for (int i = 0; i < aristas.Length; i++)
            {
                if (!string.IsNullOrEmpty(aristas[i]))
                {
                    if (aristas[i].Contains(id))
                    {
                        a_mover[j] = aristas[i];
                        j++;
                    }
                }
            }
            return a_mover;
        }

        public string[] AristasAEliminar(string[] aristas, string id)
        {
            int counter = 0;
            for (int i = 0; i < aristas.Length; i++)
            {
                if (!string.IsNullOrEmpty(aristas[i]))
                {
                    if (aristas[i].Contains(id))
                    {
                        counter++;
                    }
                }
            }
            string[] a_eliminar = new string[counter];
            int j = 0;
            for (int i = 0; i < aristas.Length; i++)
            {
                if (!string.IsNullOrEmpty(aristas[i]))
                {
                    if (aristas[i].Contains(id))
                    {
                        a_eliminar[j] = aristas[i];
                        j++;
                    }
                }
            }
            return a_eliminar;
        }

        public string[] EliminarRegistro(string[] aristas, string[] eliminar)
        {
            int counter = 0;
            int posicion = 0;

            for (int i = 0; i < aristas.Length; i++)
            {
                for (int j = 0; j < eliminar.Length; j++)
                {
                    if (aristas[i] == eliminar[j])
                    {
                        if (counter == 0 && posicion == 0)
                        {
                            posicion = i;
                        }
                        aristas[i] = "";
                    }
                }
            }

            return ReOrdenarRegistro(aristas, posicion);
        }

        public string ObtenerNombre(Line arista)
        {
            //t1_t2 -> 5 caracteres -> 1_t2 -> 4 caracteres -> 1-t2 -> 1-2
            //t11_t22 -> 7 caracteres ->  11_t22 -> 6 caracteres -> 11-t22 ->  11-22
            string nombre = arista.Name;
            string temp = nombre;

            if (nombre.Length == 5)
            {
                //t1_t2
                temp = nombre.Remove(0,1);
                temp = temp.Replace("_", "-");
                //1-t2
                temp = temp.Remove(2,1);
                // = 1-2
            }
            else if (nombre.Length == 7)
            {
                //t11_t22
                temp = nombre.Remove(0,1);
                temp = temp.Replace("_", "-");
                //11-t22
                temp = temp.Remove(3,1);
                // = 11-22
            }

            return temp;
        }

        private string[] ReOrdenarRegistro(string[] aristas, int posicion)
        {
            int aux = 0;
            int counter = 0;
            string[] new_aristas = new string[128];

            for (int i = 0; i < aristas.Length; i++)
            {
                if (!string.IsNullOrEmpty(aristas[i]))
                {
                    new_aristas[counter] = aristas[i];
                    counter++;
                }
            }

            return new_aristas;
        }

        public string RenombrarID(Line arista, int posi)
        {
            //t1_t2 -> 5 caracteres -> 1_t2 -> 4 caracteres -> 1-t2 -> 1-2
            //t11_t22 -> 7 caracteres ->  11_t22 -> 6 caracteres -> 11-t22 ->  11-22
            string nombre = arista.Name;
            string temp = nombre;
            char dir1 = '.';
            char dir2 = '.';
            int aux = 0;
            int aux2 = 0;
            string nuevo = "";

            if (nombre.Length == 5)
            {
                //t1_t2
                dir1 = nombre[0];
                temp = nombre.Remove(0, 1);
                temp = temp.Replace("_", "-");
                //1-t2
                dir2 = temp[2];
                temp = temp.Remove(2, 1);
                // = 1-2
                aux = int.Parse(temp[0].ToString());
                aux2 = int.Parse(temp[2].ToString());

                if (aux > posi)
                {
                    aux = int.Parse(temp[0].ToString()) - 1;
                }
                
                if (aux2 > posi)
                {
                    aux2 = int.Parse(temp[2].ToString()) - 1;
                }

                nuevo = dir1.ToString() + aux.ToString() + "_" + dir2.ToString() + aux2.ToString();

            }
            else if (nombre.Length == 7)
            {

                //t11_t22
                dir1 = nombre[0];
                temp = nombre.Remove(0, 1);
                temp = temp.Replace("_", "-");
                //11-t22
                dir2 = nombre[3];
                temp = temp.Remove(3, 1);
                // = 11-22
                aux = int.Parse((temp[0] + temp[1]).ToString());
                aux2 = int.Parse((temp[0] + temp[1]).ToString());

                if (aux > posi)
                {
                    aux = int.Parse(temp[0].ToString()) - 1;
                }
                else if (aux2 > posi)
                {
                    aux2 = int.Parse(temp[2].ToString()) - 1;
                }

                nuevo = dir1 + aux + "_" + dir2 + aux2;
            }

            return nuevo;
        }
    }
}
