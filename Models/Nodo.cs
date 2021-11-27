using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Grafos.Models
{
    public class Nodo
    {
        // Propiedad Id
        public int Id { get; set; }
        
        // Propiedad Nombre
        public string Nombre { get; set; }

        // Objeto que se renderizará en el canvas.
        public Ellipse CanvasObjeto { get; set; }
        public TextBox CanvasLabel { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public int[] IdAristas { get; set; }
    }
}
