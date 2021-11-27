using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using Grafos.Models;

namespace Grafos.Controllers
{
    class AristaController
    {
        public AristaController()
        {

        }

        public Arista CrearAristas(double x1, double y1, double x2, double y2, int idNodo1, int idNodo2)
        {
            Line line = new Line();
            line.X1 = x1;
            line.Y1 = y1;
            line.X2 = x2;
            line.Y2 = y2;
            line.Stroke = Brushes.Black;
            line.StrokeThickness = 1.5;
            line.Name = "n"+ idNodo1 + "_" + "n" + idNodo2;

            return new Arista() { NombreArista = line.Name, CanvasObjeto = line, IdNodo1 = idNodo1, IdNodo2 = idNodo2};
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
