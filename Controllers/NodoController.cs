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
using Grafos.Models;

namespace Grafos.Controllers
{
    class NodoController
    {

        int posicion = 0;

        /// <sumary>
        /// Logica del nodo
        /// </sumary>
        
        public Nodo CrearNodo(int idNodo)
        {
            SolidColorBrush stroke_color = new SolidColorBrush(Colors.Black);
            SolidColorBrush fill_color = new SolidColorBrush(Colors.White);
            Ellipse canvasObjeto = new Ellipse();
            canvasObjeto.Fill = fill_color;
            canvasObjeto.Height = 50;
            canvasObjeto.Width = 50;
            canvasObjeto.Cursor = Cursors.SizeAll;
            canvasObjeto.Stroke = stroke_color;
            canvasObjeto.StrokeThickness = 1;
            canvasObjeto.Name = "nodo_" + idNodo;
            canvasObjeto.SetValue(Canvas.TopProperty, 50.0);
            canvasObjeto.SetValue(Canvas.LeftProperty, 50.0);

            return new Nodo() { Id = idNodo , CanvasObjeto = canvasObjeto};
        }
        public TextBox GenerarTextoNodo(int id)
        {
            TextBox tb = new TextBox();
            tb.Name = "txt_" + id;
            tb.Text = "";
            tb.Width = 30;
            tb.Height = 16;
            tb.Background = Brushes.Transparent;
            tb.BorderBrush = Brushes.Transparent;
            tb.Foreground = Brushes.Black;
            tb.SelectionBrush = Brushes.Green;
            tb.TextAlignment = System.Windows.TextAlignment.Center;
            tb.SetValue(Canvas.TopProperty, 67.0);
            tb.SetValue(Canvas.LeftProperty, 60.0);

            return tb;
        }

        public string ObtenerId(Ellipse nodo)
        {
            string nodo_id = nodo.Name;
            string s_rem = "nodo_";
            string id = nodo_id.Replace(s_rem, "");

            return id;
        }


        public int GenerarId(List<Nodo> ids)
        {
            int new_id = 0;

            // ids == null significa que la lista no ha sido inicializada.
            if (ids == null)
            {
                return new_id = 1;
            }
            else
            {
                new_id = 1;
                for (int i = 0; i < ids.Count; i++)
                {
                    if (ids[i].Id == 0)
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
    }
}
