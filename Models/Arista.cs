using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace Grafos.Models
{
    public class Arista
    {
        public string NombreArista { get; set; }
        public int IdNodo1 { get; set; }
        public int IdNodo2 { get; set; }
        public Line CanvasObjeto { get; set; }
        public int Peso { get; set; }
    }
}
