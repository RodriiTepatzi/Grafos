using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows;

namespace Grafos.Resources
{
    class Nodo
    {

        int posicion = 0;

        /// <sumary>
        /// Logica del nodo
        /// </sumary>
        
        public Nodo()
        {

        }
        
        public Ellipse CrearNodo(int node_id)
        {
            SolidColorBrush stroke_color = new SolidColorBrush(Colors.Black);
            SolidColorBrush fill_color = new SolidColorBrush(Colors.White);
            Ellipse nodo = new Ellipse();
            nodo.Fill = fill_color;
            nodo.Height = 50;
            nodo.Width = 50;
            nodo.Cursor = Cursors.SizeAll;
            nodo.Stroke = stroke_color;
            nodo.StrokeThickness = 1;
            nodo.Name = "nodo_" + node_id;
            nodo.SetValue(Canvas.TopProperty, 50.0);
            nodo.SetValue(Canvas.LeftProperty, 50.0);

            return nodo;
        }
        public TextBox GenerarTextoNodo(int id)
        {
            TextBox tb = new TextBox();
            tb.Name = "txt_" + id;
            tb.Text = "0";
            tb.Width = 30;
            tb.Height = 16;
            tb.BorderBrush = Brushes.Transparent;
            tb.SelectionBrush = Brushes.Green;
            tb.TextAlignment = System.Windows.TextAlignment.Center;
            tb.SetValue(Canvas.TopProperty, 67.0);
            tb.SetValue(Canvas.LeftProperty, 60.0);

            return tb;
        }

        public Rectangle[] GenerarConexiones(int id)
        {
            Rectangle[] conectores = new Rectangle[4];

            for (int i = 0; i < 4; i++)
            {
                conectores[i] = new Rectangle()
                {
                    Width = 10,
                    Height = 10,
                    Fill = Brushes.Transparent,
                    Stroke = Brushes.Gray,
                    StrokeThickness = 0.5,
                    Cursor = Cursors.Hand
                };
            }

            conectores[0].Name = "cone_t_" + id;
            conectores[1].Name = "cone_r_" + id;
            conectores[2].Name = "cone_b_" + id;
            conectores[3].Name = "cone_l_" + id;

            conectores[0].SetValue(Canvas.TopProperty, 45.0);
            conectores[1].SetValue(Canvas.TopProperty, 70.0);
            conectores[2].SetValue(Canvas.TopProperty, 95.0);
            conectores[3].SetValue(Canvas.TopProperty, 70.0);

            conectores[0].SetValue(Canvas.LeftProperty, 70.0);
            conectores[1].SetValue(Canvas.LeftProperty, 95.0);
            conectores[2].SetValue(Canvas.LeftProperty, 70.0);
            conectores[3].SetValue(Canvas.LeftProperty, 45.0);

            return conectores;
        }

        public string ObtenerId(Ellipse nodo)
        {
            string nodo_id = nodo.Name;
            string s_rem = "nodo_";
            string id = nodo_id.Replace(s_rem, "");

            return id;
        }


        public int generarID(int[] ids)
        {
            int new_id = 0;

            if (ids[0] == 0)
            {
                return new_id = 1;
            }
            else
            {
                new_id = 1;
                for (int i = 0; i < ids.Length; i++)
                {
                    if (ids[i] == 0)
                    {
                        break;
                    }
                    else
                    {
                        new_id++;
                    }
                }
                return new_id;
            }
        }

        public int[] FijarEnArreglo(int[] ids,int id)
        {
            int counter = 0;

            if (ids[0] == 0)
            {
                ids[0] = id;
            }
            else
            {
                for (int i = 0; i < ids.Length; i++)
                {
                    if (ids[i] != 0)
                    {
                        counter++;
                    }
                }

                ids[counter] = id;
            }

            return ids;
        }

        public int[] EliminarEnArreglo(int[] ids, int id)
        {

            for (int i = 0; i < ids.Length; i++)
            {
                if (ids[i] == id)
                {
                    ids[i] = 0;
                    posicion = i;
                    break;
                }
            }

            int total = TotalNodos(ids);
            int[] ids_ordenados = ReOrdenarArreglo(ids, posicion, total);
            

            return ids_ordenados;
        }

        private int[] ReOrdenarArreglo(int[] ids, int posi, int total)
        {

            for (int i = posi; i <= total; i++)
            {
                if (i == total)
                {
                    ids[i] = ids[i + 1];
                }
                else
                {
                    ids[i] = ids[i + 1] - 1;
                }
            }

            return ids;
        }

        public int TotalNodos(int[] ids)
        {
            int total = 0;

            for (int i = 0; i < ids.Length; i++)
            {
                if (ids[i] != 0)
                {
                    total++;
                }
            }

            return total;

        }


        public int getPosicion()
        {
            return this.posicion;
        }

        public string getConID(string id)
        {
            string[] s_rem = {"cone_t_", "cone_r_", "cone_b_", "cone_l_" };
            string idd = "";

            for (int i=0; i < 4; i++)
            {
                if (id.Contains(s_rem[i]))
                {
                    idd = id.Replace(s_rem[i], "");
                }
            }

            return idd;
        }

        public char ObtenerTipoConector(string id)
        {
            if (id.Contains("t"))
            {
                return 't';
            }
            else if (id.Contains("r"))
            {
                return 'r';
            }
            else if(id.Contains("b"))
            {
                return 'b';
            }
            else if (id.Contains("l"))
            {
                return 'l';
            }
            return '0';
        }
    }
}
