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
using System.Windows.Navigation;
using System.Windows.Shapes;


using EncontrarElemento = Grafos.Resources.EncontrarElemento;
using Calcular = Grafos.Resources.Calcular;
using APrim = Grafos.Resources.Prim;
using System.Text.RegularExpressions;
using Grafos.Controllers;
using Grafos.Models;

namespace Grafos
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        // Declaración de validación regex.
        private static readonly Regex _regex = new Regex("[^0-9.-]+");

        // Declaración del controlador de los nodos.
        private readonly NodoController _nodoController = new NodoController();

        // Declaración del controlador de las aristas.
        private readonly AristaController _aristaController = new AristaController();

        // Declaración de variables para obtener altura y ancho del canvas.
        private readonly IInputElement MovementCanvas;

        // Variables para guardar los limites de movimiento en el canvas.
        // min_y debe tener el margen de arriba hacia el primer borde del canvas.
        // min_x debe tener el margen de la izquierda hacia el primer borde del canvas.
        private double max_x = 0, min_x = 10, max_y = 0, min_y = 70;

        // Crear la lista de los objetos.
        private List<Nodo> nodos = new List<Nodo>();
        private List<Arista> aristas = new List<Arista>();

        // Boolean para saber si un nodo esta siendo conectado con otro.
        private Boolean conectandoArista = false;

        // Variables para saber si se esta seleccionando el nodo Inicio y Final.
        private Boolean selectInicio = false, selectFinal = false;


        // Matriz para guardar la matriz de adyacencia.
        private int[,] matriz;

        // Variables para guardar temporalmente los id de nodos.
        private int nodoId1 = 0, nodoId2 = 0;

        // Variables para guardar los id del visor de pesos.
        private int pNodoId1 = 0, pNodoId2 = 0;

        

        private int nodo_inicio = 1, nodo_final = 0;
        private int[,] arbol;

        private int peso_counter = 0;
        private int max = 0;

        Calcular CalculoHelper = new Calcular();
        EncontrarElemento IHelper = new EncontrarElemento();
        APrim Prim = new APrim();

        // Constructor
        public MainWindow()
        {
            InitializeComponent();

            // Obtener el valor máximo para mover los objetos por el canvas.
            max_x = canvas.Width;
            max_y = canvas.Height;
        }

        // Region Nodos

        #region Nodos

        private void btnNuevoNodo_Click(object sender, RoutedEventArgs e)
        {
            // Obtenemos un nuevo Id para el nuevo nodo.
            var id = _nodoController.GenerarId(nodos);

            // Mostrar el total de los nodos.
            label_1.Content = id + " nodos.";

            // Creamos el nodo con el id generado.
            var nodo = _nodoController.CrearNodo(id);

            // Asignamos todos los listeners necesarios.
            nodo.CanvasObjeto.MouseLeftButtonDown += Nodo_MouseLeftButtonDown;
            nodo.CanvasObjeto.MouseLeftButtonUp += Nodo_MouseLeftButtonUp;
            nodo.CanvasObjeto.MouseMove += Nodo_MouseMove;
            nodo.CanvasObjeto.MouseRightButtonUp += Nodo_Eliminar;
            nodo.CanvasObjeto.MouseEnter += Nodo_MouseEnter;
            nodo.CanvasObjeto.MouseLeave += Nodo_MouseLeave;
            nodo.CanvasObjeto.Visibility = Visibility.Visible;

            // Asignamos un textbox que indicará el id del nodo.
            var label = _nodoController.GenerarTextoNodo(id);
            label.Visibility = Visibility.Visible;
            label.IsEnabled = false;
            label.Text = id + "";
            nodo.CanvasLabel = label;
            // Guardamos el nuevo nodo en la lista.
            nodos.Add(nodo);

            // Añadimos los elementos al canvas.
            canvas.Children.Add(nodo.CanvasObjeto);
            canvas.Children.Add(nodo.CanvasLabel);

            // Seteamos el Z-Index para que haga overlap a las aristas y en su caso, el textbox al nodo.
            TraerEnfrente(canvas, nodo.CanvasObjeto, 1);
            TraerEnfrente(canvas, nodo.CanvasLabel, 2);

            // Calculamos la matriz.
            matriz = CalculoHelper.CalcularMatriz(nodos, aristas);
        }

        // Metodo para mostrar un mensaje de ayuda al pasar el mouse sobre el nodo.
        private void Nodo_MouseEnter(object sender, MouseEventArgs e)
        {
            Ellipse nodoo = sender as Ellipse;
            nombre_nodo.Content = "Nodo " + _nodoController.ObtenerId(nodoo);
            msg.Content = "Para eliminar el nodo " + _nodoController.ObtenerId(nodoo) + " presione clic derecho.";
        }

        // Metodo para quitar el mensaje de ayuda al pasar el mouse sobre el nodo.
        private void Nodo_MouseLeave(object sender, MouseEventArgs e)
        {
            nombre_nodo.Content = "";
            msg.Content = "";
        }

        // Metodo para capturar el focus en el nodo.
        private void Nodo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse ellipse = sender as Ellipse;


            if (!selectInicio)
            {
                if (ellipse != null)
                {
                    ellipse.CaptureMouse();
                }
            }
        }

        // Metodo para cuando se suelte el boton izq. del mouse sobre el nodo.
        private void Nodo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Ellipse ellipse = sender as Ellipse;

            if (conectandoArista)
            {
                var id = int.Parse(_nodoController.ObtenerId(ellipse));

                if (nodoId1 == 0)
                {
                    if (nodoId1 == id) MessageBox.Show("No puede asignar una arista al mismo nodo. \n Por favor seleccione otro.");
                    else nodoId1 = id;
                }
                else if (nodoId2 == 0) nodoId2 = id;

                if (nodoId1 != 0 && nodoId2 != 0)
                {
                    CrearArista(nodoId1, nodoId2);
                    Cursor = Cursors.Arrow;
                }
            }

            if (selectInicio)
            {
                nodo_inicio = int.Parse(_nodoController.ObtenerId(IHelper.FindChild<Ellipse>(Application.Current.MainWindow, ellipse.Name)));

                if (nodo_final == nodo_inicio)
                {
                    MessageBox.Show("No es posible asignar el nodo inicio al mismo nodo final");
                    nodo_inicio = 0;
                }
                else
                {
                    nodo_inicial_lb.Content = "Nodo inicial: " + nodo_inicio;
                    selectInicio = false;
                    Cursor = Cursors.Arrow;
                }


            }
            else
            {
                if (ellipse != null)
                {
                    ellipse.ReleaseMouseCapture();
                }
            }
        }

        // Metodo para arrastrar el nodo.
        private void Nodo_MouseMove(object sender, MouseEventArgs e)
        {
            Ellipse ellipse = sender as Ellipse;
            string txt_id = _nodoController.ObtenerId(ellipse);
            var id = int.Parse(_nodoController.ObtenerId(ellipse));
            var nodo = nodos.Find(n => n.Id == id);

            if (ellipse != null && ellipse.IsMouseCaptured && !conectandoArista)
            {
                if (e.GetPosition(MovementCanvas).X < max_x && e.GetPosition(MovementCanvas).Y < max_y &&
                    e.GetPosition(MovementCanvas).X > min_x && e.GetPosition(MovementCanvas).Y > min_y)
                {
                    Canvas.SetLeft(ellipse, e.GetPosition(MovementCanvas).X - 40);
                    Canvas.SetTop(ellipse, e.GetPosition(MovementCanvas).Y - 80);
                    MoverTexto(e.GetPosition(MovementCanvas).X - 40, e.GetPosition(MovementCanvas).Y - 80, txt_id);
                    nodo.X = Canvas.GetLeft(ellipse) + 25;
                    nodo.Y = Canvas.GetTop(ellipse) + 25;
                    MoverAristas(id);
                }
            }
        }

        // Metodo para eliminar el nodo.
        private void Nodo_Eliminar(object sender, MouseButtonEventArgs e)
        {
            Ellipse ellipse = sender as Ellipse;
            int id = int.Parse(_nodoController.ObtenerId(ellipse));
            var nodo = nodos.Find(n => n.Id == id);

            canvas.Children.Remove(nodo.CanvasObjeto);
            canvas.Children.Remove(nodo.CanvasLabel);
            nodos.RemoveAll(n => n.Id == id);

            if (id == nodo_inicio)
            {
                nodo_inicio = 0;
                nodo_inicial_lb.Content = "Nodo inicial: 0";
            }

            if (aristas.Count == 0)
            {
                btnSiguientePeso.IsEnabled = false;
                btnAnteriorPeso.IsEnabled = false;
            }

            int counter = 1;

            foreach (var nodoTemp in nodos)
            {
                if (nodoTemp.Id > id)
                {
                    nodoTemp.Id = counter;
                    nodoTemp.CanvasObjeto.Name = "nodo_" + counter;
                    nodoTemp.CanvasLabel.Name = "txt_" + counter;
                    nodoTemp.CanvasLabel.Text = counter + "";
                }
                counter++;
            }

            int id_r = id;

            QuitarAristaCanvas(id);

            max = aristas.Count;
            label_1.Content = nodos.Count + " nodos.";
            matriz = CalculoHelper.CalcularMatriz(nodos, aristas);
            canvas.UpdateLayout();
        }

        #endregion

        // Region Aristas

        #region Aristas

        // Metodo para crear la arista
        private void CrearArista(int IdNodo1, int IdNodo2)
        {
            if (conectandoArista && IdNodo1 != 0 && IdNodo2 != 0)
            {
                var nodo1 = nodos.Find(n => n.Id == IdNodo1);
                var nodo2 = nodos.Find(n => n.Id == IdNodo2);
                var arista = _aristaController.CrearAristas(nodo1.X, nodo1.Y, nodo2.X, nodo2.Y, IdNodo1, IdNodo2);
                arista.CanvasObjeto.MouseRightButtonUp += EliminarArista;
                arista.CanvasObjeto.MouseEnter += Arista_MouseEnter;
                arista.CanvasObjeto.MouseLeave += Arista_MouseLeave;

                canvas.Children.Add(arista.CanvasObjeto);
                aristas.Add(arista);

                nodoId1 = 0;
                nodoId2 = 0;
                conectandoArista = false;
                Cursor = Cursors.Arrow;
                btnGuardarPeso.IsEnabled = true;
                btnArbol.IsEnabled = true;
                btnNuevoNodo.IsEnabled = true;
                btnPuntoPartida.IsEnabled = true;
                btnCrearArista.IsEnabled = true;

                matriz = CalculoHelper.CalcularMatriz(nodos, aristas);

                if (aristas.Count == 1)
                {
                    int id1 = aristas[peso_counter].IdNodo1;
                    int id2 = aristas[peso_counter].IdNodo2;

                    pNodoId1 = id1;
                    pNodoId2 = id2;

                    var arista1 = aristas.Find(n => n.IdNodo1 == pNodoId1 && n.IdNodo2 == pNodoId2);
                    arista.CanvasObjeto.Stroke = Brushes.Green;

                    tbPeso.Text = arista.Peso.ToString();
                    ar_label.Content = id1 + "-" + id2;
                    canvas.UpdateLayout();
                }
                else if (aristas.Count > 1)
                {
                    btnSiguientePeso.IsEnabled = true;
                }
            }
        }

        // Metodo para eliminar 1 arista especifica
        private void EliminarArista(object sender, MouseEventArgs e)
        {
            Line ar = sender as Line;
            var arista = aristas.Find(n => n.CanvasObjeto == ar);
            aristas.RemoveAll(n => n.CanvasObjeto == ar);
            canvas.Children.Remove(arista.CanvasObjeto);
            max = aristas.Count();

            if (aristas.Count == 0)
            {
                btnSiguientePeso.IsEnabled = false;
                btnAnteriorPeso.IsEnabled = false;
            }

            RenombrarAristas(arista.IdNodo1);

            matriz = CalculoHelper.CalcularMatriz(nodos, aristas);
        }

        // Metodo para mostrar un mensaje de ayuda al pasar el mouse sobre la arista.
        private void Arista_MouseEnter(object sender, MouseEventArgs e)
        {
            Line ar = sender as Line;
            nombre_arista.Content = "Arista: " + _aristaController.ObtenerNombre(ar);
            msg.Content = "Para eliminar la arista " + _aristaController.ObtenerNombre(ar) + " presione clic derecho.";
        }

        // Metodo para quitar el mensaje de ayuda al pasar el mouse sobre la arista.
        private void Arista_MouseLeave(object sender, MouseEventArgs e)
        {
            nombre_arista.Content = "";
            msg.Content = "";
        }

        // Metodo para mover las aristas
        private void MoverAristas(int id)
        {
            var nodo = nodos.Find(n => n.Id == id);
            foreach (var arista in aristas)
            {
                if (arista.IdNodo1 == id)
                {
                    arista.CanvasObjeto.X1 = nodo.X;
                    arista.CanvasObjeto.Y1 = nodo.Y;
                }
                else if (arista.IdNodo2 == id)
                {
                    arista.CanvasObjeto.X2 = nodo.X;
                    arista.CanvasObjeto.Y2 = nodo.Y;
                }
            }
            canvas.UpdateLayout();
        }

        // Metodo para quitar todas las aristas al borrar un nodo
        private void QuitarAristaCanvas(int id)
        {
            var aristasQuitar = aristas.FindAll(n => n.IdNodo1 == id || n.IdNodo2 == id);
            foreach (var arista in aristasQuitar)
            {
                canvas.Children.Remove(arista.CanvasObjeto);
            }

            aristas.RemoveAll(n => n.IdNodo1 == id || n.IdNodo2 == id);

            RenombrarAristas(id);
            max = aristas.Count;
            matriz = CalculoHelper.CalcularMatriz(nodos, aristas);
        }

        // Metodo para renombrar las aristas (desopues de borrar un nodo)
        private void RenombrarAristas(int id)
        {
            int idd = id + 1;
            int total_aristas = aristas.Count;
            int total_nodos = nodos.Count;

            Console.WriteLine(idd);


            for (int i = idd; i <= total_nodos + 1; i++)
            {
                for (int j = 0; j < total_aristas; j++)
                {

                    Console.WriteLine(aristas[j]);
                    if (aristas.ElementAt(j).IdNodo1 == id)
                    {
                        Line ar = aristas.ElementAt(j).CanvasObjeto;
                        string nuevo = _aristaController.RenombrarID(ar, i - 1);
                        aristas[j].CanvasObjeto.Name = nuevo;
                        aristas[j].NombreArista = nuevo;
                        Console.WriteLine(id + " " + nuevo);
                    }
                    else if (aristas.ElementAt(j).IdNodo2 == id)
                    {

                    }
                }
            }
            canvas.UpdateLayout();
        }

        #endregion

        // Metodo para agregar un nuevo nodo

        #region Botones

        private void btnPuntoPartida_Click(object sender, RoutedEventArgs e)
        {
            if (selectFinal)
            {
                MessageBox.Show("Por favor, seleccione el nodo final e intetelo de nuevo.");
            }
            else
            {
                Cursor = Cursors.Pen;
                selectInicio = true;
            }

        }

        private void btnPuntoFinal_Click(object sender, RoutedEventArgs e)
        {
            if (selectInicio)
            {
                MessageBox.Show("Por favor, seleccione el nodo inicial e intetelo de nuevo.");
            }
            else
            {
                Cursor = Cursors.Pen;
                selectFinal = true;
            }
        }

        private void btnSiguientePeso_Loaded(object sender, RoutedEventArgs e)
        {
            max = aristas.Count;
            if (max == peso_counter)
            {
                btnSiguientePeso.IsEnabled = false;
            }
        }

        private void btnGuardarPeso_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbPeso.Text.ToString())) 
            {
                var arista = aristas.Find(n => n.IdNodo1 == pNodoId1 && n.IdNodo2 == pNodoId2);
                arista.Peso = int.Parse(tbPeso.Text.ToString());
                matriz = CalculoHelper.CalcularMatriz(nodos, aristas);
            }
        }

        private void btnAnteriorPeso_Click(object sender, RoutedEventArgs e)
        {
            var aristaAnterior = aristas.Find(n => n.IdNodo1 == pNodoId1 && n.IdNodo2 == pNodoId2);
            aristaAnterior.CanvasObjeto.Stroke = Brushes.Black;

            if (peso_counter > 0)
            {

                peso_counter--;

                int id1 = aristas[peso_counter].IdNodo1;
                int id2 = aristas[peso_counter].IdNodo2;

                pNodoId1 = id1;
                pNodoId2 = id2;

                var arista = aristas.Find(n => n.IdNodo1 == pNodoId1 && n.IdNodo2 == pNodoId2);
                arista.CanvasObjeto.Stroke = Brushes.Green;

                tbPeso.Text = arista.Peso.ToString();
                ar_label.Content = id1 + "-" + id2;
                canvas.UpdateLayout();
            }

            if (peso_counter == 0)
            {
                btnSiguientePeso.IsEnabled = true;
                btnAnteriorPeso.IsEnabled = false;
            }
            else if (max == peso_counter)
            {
                btnSiguientePeso.IsEnabled = true;
            }
        }

        private void btnSiguientePeso_Click(object sender, RoutedEventArgs e)
        {
            var aristaAnterior = aristas.Find(n => n.IdNodo1 == pNodoId1 && n.IdNodo2 == pNodoId2);
            aristaAnterior.CanvasObjeto.Stroke = Brushes.Black;

            max = aristas.Count;
            peso_counter++;

            if (peso_counter > 0)
            {
                btnAnteriorPeso.IsEnabled = true;
            }

            if (max-1 == peso_counter)
            {
                btnSiguientePeso.IsEnabled = false;
            }

            if (max > 0)
            {
                if (peso_counter < max)
                {
                    int id1 = aristas[peso_counter].IdNodo1;
                    int id2 = aristas[peso_counter].IdNodo2;

                    pNodoId1 = id1;
                    pNodoId2 = id2;

                    var arista = aristas.Find(n => n.IdNodo1 == pNodoId1 && n.IdNodo2 == pNodoId2);
                    arista.CanvasObjeto.Stroke = Brushes.Green;

                    tbPeso.Text = arista.Peso.ToString();
                    ar_label.Content = id1 + "-" + id2;

                    canvas.UpdateLayout();
                }
                else
                {
                    btnSiguientePeso.IsEnabled = false;
                }
            }

        }

        private void btnExpandir_Click(object sender, RoutedEventArgs e)
        {
            ExpandirMatrizWindow expandirMatriz = new ExpandirMatrizWindow(matriz, nodos);
            expandirMatriz.ShowDialog();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                nodoId1 = 0;
                nodoId2 = 0;
                conectandoArista = false;
                Cursor = Cursors.Arrow;
                btnGuardarPeso.IsEnabled = true;
                btnArbol.IsEnabled = true;
                btnNuevoNodo.IsEnabled = true;
                btnPuntoPartida.IsEnabled = true;
                btnCrearArista.IsEnabled = true;
            }
        }

        private void btnInicio_Click(object sender, RoutedEventArgs e)
        {
            if (nodos.Count == 0 || aristas.Count == 0)
            {
                MessageBox.Show("No hay nodos u aristas existentes.");
            }
            else
            {
                matriz = CalculoHelper.CalcularMatriz(nodos,aristas);
                CalculoHelper.ResolverGrafo(matriz, nodo_inicio, nodos.Count);
                List<string> ListaNodos = new List<string>();

                for (int i = 0; i < nodos.Count; i++)
                {
                    ListaNodos.Add(nodos[i].Id.ToString());
                }

                arbol = Prim.InitAlgPrim(matriz, ListaNodos, nodo_inicio);
                //ImprimirArbol(arbol);
                btnArbol.IsEnabled = true;
            }   
        }

        private void btnArbol_Click(object sender, RoutedEventArgs e)
        {
            /*MatrizArbol ArbolGenerador = new MatrizArbol(arbol, nodos);
            ArbolGenerador.ShowDialog();*/

        }

        private void btnCrearArista_Click(object sender, RoutedEventArgs e)
        {
            conectandoArista = true;
            Cursor = Cursors.Pen;
            btnGuardarPeso.IsEnabled = false;
            btnArbol.IsEnabled = false;
            btnAnteriorPeso.IsEnabled = false;
            btnSiguientePeso.IsEnabled = false;
            btnNuevoNodo.IsEnabled = false;
            btnPuntoPartida.IsEnabled = false;
            btnCrearArista.IsEnabled = false;
        }

        #endregion


        #region Listener

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            coordenadas.Content = "x: " + e.GetPosition(MovementCanvas).X + " y: " + e.GetPosition(MovementCanvas).Y;
        }

        private void tbPeso_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        #endregion


        private void MoverTexto(double x, double y, string id)
        {
            TextBox tb = IHelper.FindChild<TextBox>(Application.Current.MainWindow, "txt_" + id);

            Canvas.SetLeft(tb, x + 10);
            Canvas.SetTop(tb, y + 17);
        }

        static public void TraerEnfrente(Canvas pParent, object pToMove, int max)
        {
            try
            {
                int currentIndex = Canvas.GetZIndex((UIElement)pToMove);
                int zIndex = 0;
                int maxZ = max;
                UserControl child;
                for (int i = 0; i < pParent.Children.Count; i++)
                {
                    if (pParent.Children[i] is UserControl &&
                        pParent.Children[i] != pToMove)
                    {
                        child = pParent.Children[i] as UserControl;
                        zIndex = Canvas.GetZIndex(child);
                        maxZ = Math.Max(maxZ, zIndex);
                        if (zIndex > currentIndex)
                        {
                            Canvas.SetZIndex(child, zIndex - 1);
                        }
                    }
                }
                Canvas.SetZIndex((UIElement)pToMove, maxZ);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
