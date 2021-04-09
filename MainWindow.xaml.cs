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

using Nodo = Grafos.Resources.Nodo;
using EncontrarElemento = Grafos.Resources.EncontrarElemento;
using Arista = Grafos.Resources.Aristas;
using Calcular = Grafos.Resources.Calcular;
using Dijikstra = Grafos.Resources.Dijkstra;
using _Dijkstra = Grafos.Resources._Dijkstra;
using System.Text.RegularExpressions;

namespace Grafos
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        private readonly IInputElement MovementCanvas;
        private double max_x = 0, min_x = 10, max_y = 0, min_y = 70;

        private int[] nodo_ids = new int[128];
        private int[,] matriz;
        private string[] aristas = new string[128];
        private Boolean isConnecting = false;
        private double x1, y1, x2, y2;
        private string id_1, id_2;
        private char tipo_1;
        private char tipo_2;
        private bool select_start_nodo = false;
        private bool select_final_nodo = false;
        
        private int nodo_inicio = 1, nodo_final = 0;
        private bool alreadyMod = false, stillSame = false;


        private int peso_counter = 0;
        private int max;

        Calcular CalculoHelper = new Calcular();
        Nodo nodo = new Nodo();
        EncontrarElemento IHelper = new EncontrarElemento();
        Arista Arista = new Arista();

        private static readonly Regex _regex = new Regex("[^0-9.-]+");

        public MainWindow()
        {
            InitializeComponent();

            max_x = canvas.Width;
            max_y = canvas.Height;
        }

        private void nuevo_nodo_btn_Click(object sender, RoutedEventArgs e)
        {
            int id = nodo.generarID(nodo_ids);
            nodo_ids = nodo.FijarEnArreglo(nodo_ids, id);
            label_1.Content = id + " nodos.";

            Ellipse dibujar_nodo = nodo.CrearNodo(id);
            dibujar_nodo.MouseLeftButtonDown += Nodo_MouseLeftButtonDown;
            dibujar_nodo.MouseLeftButtonUp += Nodo_MouseLeftButtonUp;
            dibujar_nodo.MouseMove += Nodo_MouseMove;
            dibujar_nodo.MouseRightButtonUp += Nodo_Eliminar;
            dibujar_nodo.MouseEnter += Nodo_MouseEnter;
            dibujar_nodo.MouseLeave += Nodo_MouseLeave;
            dibujar_nodo.Visibility = Visibility.Visible;

            TextBox nodo_texto = nodo.GenerarTextoNodo(id);
            nodo_texto.Visibility = Visibility.Visible;
            nodo_texto.IsEnabled = false;
            nodo_texto.Text = id + "";

            //#FF7DD5F5

            Rectangle[] cons = nodo.GenerarConexiones(id);
            cons[0].MouseLeftButtonUp += CrearArista;
            cons[1].MouseLeftButtonUp += CrearArista;
            cons[2].MouseLeftButtonUp += CrearArista;
            cons[3].MouseLeftButtonUp += CrearArista;

            canvas.Children.Add(cons[0]);
            canvas.Children.Add(cons[1]);
            canvas.Children.Add(cons[2]);
            canvas.Children.Add(cons[3]);
            canvas.Children.Add(dibujar_nodo);
            canvas.Children.Add(nodo_texto);

            matriz = CalculoHelper.CalcularMatriz(nodo_ids, aristas, matriz);

        }
        private void Nodo_MouseEnter(object sender, MouseEventArgs e)
        {
            Ellipse nodoo = sender as Ellipse;
            nombre_nodo.Content = "Nodo " + nodo.ObtenerId(nodoo);
            msg.Content = "Para eliminar el nodo " + nodo.ObtenerId(nodoo) + " presione clic derecho.";
        }
        private void Nodo_MouseLeave(object sender, MouseEventArgs e)
        {
            nombre_nodo.Content = "";
            msg.Content = "";
        }
        private void Arista_MouseEnter(object sender, MouseEventArgs e)
        {
            Line ar = sender as Line;
            nombre_arista.Content = "Arista: " + Arista.ObtenerNombre(ar);
            msg.Content = "Para eliminar la arista " + Arista.ObtenerNombre(ar) + " presione clic derecho.";
        }
        private void Arista_MouseLeave(object sender, MouseEventArgs e)
        {
            nombre_arista.Content = "";
            msg.Content = "";
        }
        private void Nodo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse ellipse = sender as Ellipse;

            if (select_start_nodo)
            {

            }
            else
            {
                if (ellipse != null)
                {
                    ellipse.CaptureMouse();
                }
            }
            
        }

        private void Nodo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Ellipse ellipse = sender as Ellipse;

            if (select_start_nodo)
            {
                nodo_inicio = int.Parse(nodo.ObtenerId(IHelper.FindChild<Ellipse>(Application.Current.MainWindow, ellipse.Name)));

                if (nodo_final == nodo_inicio)
                {
                    MessageBox.Show("No es posible asignar el nodo inicio al mismo nodo final");
                    nodo_inicio = 0;
                }
                else
                {
                    nodo_inicial_lb.Content = "Nodo inicial: " + nodo_inicio;
                    select_start_nodo = false;
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

        private void Nodo_MouseMove(object sender, MouseEventArgs e)
        {
            Ellipse ellipse = sender as Ellipse;
            string txt_id = nodo.ObtenerId(ellipse);

            if (ellipse != null && ellipse.IsMouseCaptured)
            {
                if (e.GetPosition(MovementCanvas).X < max_x && e.GetPosition(MovementCanvas).Y < max_y &&
                    e.GetPosition(MovementCanvas).X > min_x && e.GetPosition(MovementCanvas).Y > min_y)
                {
                    Canvas.SetLeft(ellipse, e.GetPosition(MovementCanvas).X - 40);
                    Canvas.SetTop(ellipse, e.GetPosition(MovementCanvas).Y - 80);
                    MoverTexto(e.GetPosition(MovementCanvas).X - 40, e.GetPosition(MovementCanvas).Y - 80, txt_id);
                    MoverConexiones(e.GetPosition(MovementCanvas).X - 40, e.GetPosition(MovementCanvas).Y - 80, txt_id);
                    MoverAristas(txt_id);
                }
            }
        }

        private void Nodo_Eliminar(object sender, MouseButtonEventArgs e)
        {
            Ellipse ellipse = sender as Ellipse;
            string id = nodo.ObtenerId(ellipse);

            canvas.Children.Remove(IHelper.FindChild<TextBox>(Application.Current.MainWindow, "txt_" + id));
            canvas.Children.Remove(IHelper.FindChild<Rectangle>(Application.Current.MainWindow, "cone_t_" + id));
            canvas.Children.Remove(IHelper.FindChild<Rectangle>(Application.Current.MainWindow, "cone_r_" + id));
            canvas.Children.Remove(IHelper.FindChild<Rectangle>(Application.Current.MainWindow, "cone_b_" + id));
            canvas.Children.Remove(IHelper.FindChild<Rectangle>(Application.Current.MainWindow, "cone_l_" + id));
            canvas.Children.Remove(ellipse);

            if (int.Parse(id) == nodo_inicio)
            {
                nodo_inicio = 0;
                nodo_inicial_lb.Content = "Nodo inicial: 0";
            }
            

            if (Arista.TotalAristas(aristas) == 0)
            {
                sig_peso.IsEnabled = false;
                ant_peso.IsEnabled = false;
            }

            int id_r = int.Parse(id);
            nodo_ids = nodo.EliminarEnArreglo(nodo_ids, id_r);
            int total = nodo.TotalNodos(nodo_ids);
            QuitarAristaCanvas(id);
            RenombrarElementosNodos(nodo.getPosicion(), total);
            max = Arista.TotalAristas(aristas);

            label_1.Content = total + " nodos.";

            matriz = CalculoHelper.CalcularMatriz(nodo_ids, aristas, matriz);

        }

        private void MoverTexto(double x, double y, string id)
        {
            TextBox tb = IHelper.FindChild<TextBox>(Application.Current.MainWindow, "txt_" + id);

            Canvas.SetLeft(tb, x + 10);
            Canvas.SetTop(tb, y + 17);
        }

        private void MoverConexiones(double x, double y, string id)
        {
            Rectangle rect_t = IHelper.FindChild<Rectangle>(Application.Current.MainWindow, "cone_t_" + id);
            Rectangle rect_b = IHelper.FindChild<Rectangle>(Application.Current.MainWindow, "cone_b_" + id);
            Rectangle rect_r = IHelper.FindChild<Rectangle>(Application.Current.MainWindow, "cone_r_" + id);
            Rectangle rect_l = IHelper.FindChild<Rectangle>(Application.Current.MainWindow, "cone_l_" + id);

            Canvas.SetLeft(rect_t, x + 20);
            Canvas.SetTop(rect_t, y - 5);

            Canvas.SetLeft(rect_r, x + 45);
            Canvas.SetTop(rect_r, y + 20);

            Canvas.SetLeft(rect_b, x + 20);
            Canvas.SetTop(rect_b, y + 45);

            Canvas.SetLeft(rect_l, x - 5);
            Canvas.SetTop(rect_l, y + 20);

        }
        private void RenombrarElementosNodos(int id, int total)
        {
            for (int i = id + 1; i <= total; i++)
            {
                Ellipse Nodo = IHelper.FindChild<Ellipse>(Application.Current.MainWindow, "nodo_" + (i + 1));
                Nodo.Name = "nodo_" + i;
                TextBox tb = IHelper.FindChild<TextBox>(Application.Current.MainWindow, "txt_" + (i + 1));
                tb.Name = "txt_" + i;
                Rectangle rect_t = IHelper.FindChild<Rectangle>(Application.Current.MainWindow, "cone_t_" + (i + 1));
                rect_t.Name = "cone_t_" + i;
                Rectangle rect_r = IHelper.FindChild<Rectangle>(Application.Current.MainWindow, "cone_r_" + (i + 1));
                rect_r.Name = "cone_r_" + i;
                Rectangle rect_b = IHelper.FindChild<Rectangle>(Application.Current.MainWindow, "cone_b_" + (i + 1));
                rect_b.Name = "cone_b_" + i;
                Rectangle rect_l = IHelper.FindChild<Rectangle>(Application.Current.MainWindow, "cone_l_" + (i + 1));
                rect_l.Name = "cone_l_" + i;
            }
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            coordenadas.Content = "x: " + e.GetPosition(MovementCanvas).X + " y: " + e.GetPosition(MovementCanvas).Y;
        }

        private void CrearArista(object sender, MouseButtonEventArgs e)
        {
            Rectangle conector = sender as Rectangle;
            Line ar;

            if (isConnecting)
            {
                
                id_2 = nodo.getConID(conector.Name);
                if (id_1 == id_2)
                {
                    MessageBox.Show("No es posible realizar esta acción.");
                }
                else
                {
                    x2 = double.Parse(conector.GetValue(Canvas.LeftProperty).ToString());
                    y2 = double.Parse(conector.GetValue(Canvas.TopProperty).ToString());
                    tipo_2 = nodo.ObtenerTipoConector(conector.Name);
                    ar = Arista.CrearAristas(x1 + 5, y1 + 5, x2 + 5, y2 + 5, id_1, id_2, tipo_1, tipo_2);
                    ar.MouseRightButtonUp += Eliminar1Arista;
                    ar.MouseEnter += Arista_MouseEnter;
                    ar.MouseLeave += Arista_MouseLeave;
                    int pos_ar = Arista.TotalAristas(aristas);
                    aristas[pos_ar] = ar.Name;
                    canvas.Children.Add(ar);

                    isConnecting = false;
                    Cursor = Cursors.Arrow;
                    matriz = CalculoHelper.CalcularMatriz(nodo_ids, aristas, matriz);
                    max = Arista.TotalAristas(aristas);
                    sig_peso.IsEnabled = true;
                }

                
            }
            else
            {
                isConnecting = true;
                Cursor = Cursors.Pen;
                id_1 = nodo.getConID(conector.Name);
                x1 = double.Parse(conector.GetValue(Canvas.LeftProperty).ToString());
                y1 = double.Parse(conector.GetValue(Canvas.TopProperty).ToString());
                tipo_1 = nodo.ObtenerTipoConector(conector.Name);
            }
        }

        private void Eliminar1Arista(object sender, MouseEventArgs e)
        {
            Line ar = sender as Line;
            string[] ar_name = { ar.Name };
            for (int i = 0; i < aristas.Length; i++)
            {
                if (aristas[i] == ar_name[0])
                {
                    aristas = Arista.EliminarRegistro(aristas, ar_name);
                    canvas.Children.Remove(IHelper.FindChild<Line>(Application.Current.MainWindow, ar_name[0]));
                    max = Arista.TotalAristas(aristas);
                }
            }

            if (Arista.TotalAristas(aristas) == 0)
            {
                sig_peso.IsEnabled = false;
                ant_peso.IsEnabled = false;
            }
        }

        private void nodo_partida_btn_Click(object sender, RoutedEventArgs e)
        {
            if (select_final_nodo)
            {
                MessageBox.Show("Por favor, seleccione el nodo final e intetelo de nuevo.");
            }
            else
            {
                Cursor = Cursors.Pen;
                select_start_nodo = true;
            }
            
        }

        private void peso_box_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        private void peso_box_TextChanged(object sender, TextChangedEventArgs e)
        {

            
        }

        private void nodo_final_btn_Click(object sender, RoutedEventArgs e)
        {
            if (select_start_nodo)
            {
                MessageBox.Show("Por favor, seleccione el nodo inicial e intetelo de nuevo.");
            }
            else
            {
                Cursor = Cursors.Pen;
                select_final_nodo = true;
            }
        }

        private void peso_box_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void sig_peso_Loaded(object sender, RoutedEventArgs e)
        {
            max = Arista.TotalAristas(aristas);
            if (max == peso_counter)
            {
                sig_peso.IsEnabled = false;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string nombre = ar_label.Content.ToString();
            int id_n_1 = 0;
            int id_n_2 = 0;
            if (nombre.Length == 5)
            {
                id_n_1 = int.Parse(nombre[0].ToString());
                id_n_2 = int.Parse(nombre[4].ToString());
            }

            if (!string.IsNullOrEmpty(peso_box.Text))
            {
                if (int.Parse(peso_box.Text.ToString()) == 0)
                {
                    MessageBox.Show("No es posible guardar este peso.");
                }
                else
                {
                    if (id_n_1 != 0 && id_n_2 != 0)
                    {
                        matriz = CalculoHelper.AgregarPesoMatriz(matriz, id_n_1, id_n_2, int.Parse(peso_box.Text.ToString()), nodo_ids);
                        matriz = CalculoHelper.AgregarPesoMatriz(matriz, id_n_2, id_n_1, int.Parse(peso_box.Text.ToString()), nodo_ids);
                    }
                }
            }
        }

        private void ant_peso_Click(object sender, RoutedEventArgs e)
        {
            if (peso_counter > 0)
            {
                
                peso_counter--;
                if (aristas[peso_counter].Length == 5)
                {
                    Console.WriteLine(peso_counter);
                    string nombre = aristas[peso_counter];
                    string nombre_ids = nombre[1].ToString() + " - " + nombre[4].ToString();
                    ar_label.Content = nombre_ids;
                    alreadyMod = false;
                }
            }

            if (peso_counter == 0)
            {
                ant_peso.IsEnabled = false;
                sig_peso.IsEnabled = true;
            }
            else if(max == peso_counter)
            {
                sig_peso.IsEnabled = true;
            }
        }

        private void sig_peso_Click(object sender, RoutedEventArgs e)
        {
            max = Arista.TotalAristas(aristas);
            if (peso_counter > 0)
            {
                ant_peso.IsEnabled = true;
            }
            if (max == peso_counter)
            {
                sig_peso.IsEnabled = false;
            }
            else
            {

                

                if (max > 0)
                {
                    if (peso_counter < max)
                    {

                        if (aristas[peso_counter].Length == 5)
                        {
                            string nombre = aristas[peso_counter];
                            string nombre_ids = nombre[1].ToString() + " - " + nombre[4].ToString();
                            int id_n1 = int.Parse(nombre[1].ToString());
                            int id_n2 = int.Parse(nombre[4].ToString());

                            peso_box.Text = matriz[(id_n1 - 1), (id_n2 - 1)].ToString();
                            ar_label.Content = nombre_ids;
                            alreadyMod = false;
                            peso_counter++;
                        }

                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mostrarAristas();
        }

        private void MoverAristas(string id)
        {

            string[] idss = Arista.AristasAMover(aristas, id);
            char tipo;

            if (idss.Length != 0)
            {
                for (int i = 0; i < idss.Length; i++)
                {
                    string nombre_ar = idss[i];

                    if (id.Length == 1)
                    {
                        char[] temp = id.ToCharArray();

                        if (nombre_ar.IndexOf(temp[0]) == 1)
                        {
                            if (nombre_ar.ElementAt(0) == 't')
                            {
                                Line arista = IHelper.FindChild<Line>(Application.Current.MainWindow, idss[i]);
                                arista.X1 = double.Parse(IHelper.FindChild<Ellipse>(Application.Current.MainWindow, "nodo_" + id).GetValue(Canvas.LeftProperty).ToString()) + 25;
                                arista.Y1 = double.Parse(IHelper.FindChild<Ellipse>(Application.Current.MainWindow, "nodo_" + id).GetValue(Canvas.TopProperty).ToString());
                            }
                            else if (nombre_ar.ElementAt(0) == 'r')
                            {
                                Line arista = IHelper.FindChild<Line>(Application.Current.MainWindow, idss[i]);
                                arista.X1 = double.Parse(IHelper.FindChild<Ellipse>(Application.Current.MainWindow, "nodo_" + id).GetValue(Canvas.LeftProperty).ToString()) + 50;
                                arista.Y1 = double.Parse(IHelper.FindChild<Ellipse>(Application.Current.MainWindow, "nodo_" + id).GetValue(Canvas.TopProperty).ToString()) + 25;
                            }
                            else if (nombre_ar.ElementAt(0) == 'b')
                            {
                                Line arista = IHelper.FindChild<Line>(Application.Current.MainWindow, idss[i]);
                                arista.X1 = double.Parse(IHelper.FindChild<Ellipse>(Application.Current.MainWindow, "nodo_" + id).GetValue(Canvas.LeftProperty).ToString()) + 25;
                                arista.Y1 = double.Parse(IHelper.FindChild<Ellipse>(Application.Current.MainWindow, "nodo_" + id).GetValue(Canvas.TopProperty).ToString()) + 50;
                            }
                            else if (nombre_ar.ElementAt(0) == 'l')
                            {
                                Line arista = IHelper.FindChild<Line>(Application.Current.MainWindow, idss[i]);
                                arista.X1 = double.Parse(IHelper.FindChild<Ellipse>(Application.Current.MainWindow, "nodo_" + id).GetValue(Canvas.LeftProperty).ToString());
                                arista.Y1 = double.Parse(IHelper.FindChild<Ellipse>(Application.Current.MainWindow, "nodo_" + id).GetValue(Canvas.TopProperty).ToString()) + 25;
                            }

                        }
                        else if (nombre_ar.IndexOf(temp[0]) == 4)
                        {
                            if (nombre_ar.ElementAt(3) == 't')
                            {
                                Line arista = IHelper.FindChild<Line>(Application.Current.MainWindow, idss[i]);
                                arista.X2 = double.Parse(IHelper.FindChild<Ellipse>(Application.Current.MainWindow, "nodo_" + id).GetValue(Canvas.LeftProperty).ToString()) + 25;
                                arista.Y2 = double.Parse(IHelper.FindChild<Ellipse>(Application.Current.MainWindow, "nodo_" + id).GetValue(Canvas.TopProperty).ToString());
                            }
                            else if (nombre_ar.ElementAt(3) == 'r')
                            {
                                Line arista = IHelper.FindChild<Line>(Application.Current.MainWindow, idss[i]);
                                arista.X2 = double.Parse(IHelper.FindChild<Ellipse>(Application.Current.MainWindow, "nodo_" + id).GetValue(Canvas.LeftProperty).ToString()) + 50;
                                arista.Y2 = double.Parse(IHelper.FindChild<Ellipse>(Application.Current.MainWindow, "nodo_" + id).GetValue(Canvas.TopProperty).ToString()) + 25;
                            }
                            else if (nombre_ar.ElementAt(3) == 'b')
                            {
                                Line arista = IHelper.FindChild<Line>(Application.Current.MainWindow, idss[i]);
                                arista.X2 = double.Parse(IHelper.FindChild<Ellipse>(Application.Current.MainWindow, "nodo_" + id).GetValue(Canvas.LeftProperty).ToString()) + 25;
                                arista.Y2 = double.Parse(IHelper.FindChild<Ellipse>(Application.Current.MainWindow, "nodo_" + id).GetValue(Canvas.TopProperty).ToString()) + 50;
                            }
                            else if (nombre_ar.ElementAt(3) == 'l')
                            {
                                Line arista = IHelper.FindChild<Line>(Application.Current.MainWindow, idss[i]);
                                arista.X2 = double.Parse(IHelper.FindChild<Ellipse>(Application.Current.MainWindow, "nodo_" + id).GetValue(Canvas.LeftProperty).ToString());
                                arista.Y2 = double.Parse(IHelper.FindChild<Ellipse>(Application.Current.MainWindow, "nodo_" + id).GetValue(Canvas.TopProperty).ToString()) + 25;
                            }
                        }
                    }
                    else if (id.Length == 2)
                    {
                        char[] temp = id.ToCharArray();

                        if (nombre_ar.IndexOf(temp[0]) == 1 && nombre_ar.IndexOf(temp[0]) == 2)
                        {
                            if (nombre_ar.ElementAt(0) == 't')
                            {
                                Line arista = IHelper.FindChild<Line>(Application.Current.MainWindow, idss[i]);
                                arista.X1 = double.Parse(IHelper.FindChild<Ellipse>(Application.Current.MainWindow, "nodo_" + id).GetValue(Canvas.LeftProperty).ToString()) + 25;
                                arista.Y1 = double.Parse(IHelper.FindChild<Ellipse>(Application.Current.MainWindow, "nodo_" + id).GetValue(Canvas.TopProperty).ToString());
                            }
                            else if (nombre_ar.ElementAt(0) == 'r')
                            {
                                Line arista = IHelper.FindChild<Line>(Application.Current.MainWindow, idss[i]);
                                arista.X1 = double.Parse(IHelper.FindChild<Ellipse>(Application.Current.MainWindow, "nodo_" + id).GetValue(Canvas.LeftProperty).ToString()) + 50;
                                arista.Y1 = double.Parse(IHelper.FindChild<Ellipse>(Application.Current.MainWindow, "nodo_" + id).GetValue(Canvas.TopProperty).ToString()) + 25;
                            }
                            else if (nombre_ar.ElementAt(0) == 'b')
                            {
                                Line arista = IHelper.FindChild<Line>(Application.Current.MainWindow, idss[i]);
                                arista.X1 = double.Parse(IHelper.FindChild<Ellipse>(Application.Current.MainWindow, "nodo_" + id).GetValue(Canvas.LeftProperty).ToString()) + 25;
                                arista.Y1 = double.Parse(IHelper.FindChild<Ellipse>(Application.Current.MainWindow, "nodo_" + id).GetValue(Canvas.TopProperty).ToString()) + 50;
                            }
                            else if (nombre_ar.ElementAt(0) == 'l')
                            {
                                Line arista = IHelper.FindChild<Line>(Application.Current.MainWindow, idss[i]);
                                arista.X1 = double.Parse(IHelper.FindChild<Ellipse>(Application.Current.MainWindow, "nodo_" + id).GetValue(Canvas.LeftProperty).ToString());
                                arista.Y1 = double.Parse(IHelper.FindChild<Ellipse>(Application.Current.MainWindow, "nodo_" + id).GetValue(Canvas.TopProperty).ToString()) + 25;
                            }
                        }
                        else if ((nombre_ar.LastIndexOf(temp[0]) == 5 && nombre_ar.LastIndexOf(temp[1]) == 6) || (nombre_ar.LastIndexOf(temp[0]) == 4 && nombre_ar.LastIndexOf(temp[1]) == 5))
                        {
                            if (nombre_ar.ElementAt(4) == 't')
                            {
                                Line arista = IHelper.FindChild<Line>(Application.Current.MainWindow, idss[i]);
                                arista.X2 = double.Parse(IHelper.FindChild<Ellipse>(Application.Current.MainWindow, "nodo_" + id).GetValue(Canvas.LeftProperty).ToString()) + 25;
                                arista.Y2 = double.Parse(IHelper.FindChild<Ellipse>(Application.Current.MainWindow, "nodo_" + id).GetValue(Canvas.TopProperty).ToString());
                            }
                            else if (nombre_ar.ElementAt(4) == 'r')
                            {
                                Line arista = IHelper.FindChild<Line>(Application.Current.MainWindow, idss[i]);
                                arista.X2 = double.Parse(IHelper.FindChild<Ellipse>(Application.Current.MainWindow, "nodo_" + id).GetValue(Canvas.LeftProperty).ToString()) + 50;
                                arista.Y2 = double.Parse(IHelper.FindChild<Ellipse>(Application.Current.MainWindow, "nodo_" + id).GetValue(Canvas.TopProperty).ToString()) + 25;
                            }
                            else if (nombre_ar.ElementAt(4) == 'b')
                            {
                                Line arista = IHelper.FindChild<Line>(Application.Current.MainWindow, idss[i]);
                                arista.X2 = double.Parse(IHelper.FindChild<Ellipse>(Application.Current.MainWindow, "nodo_" + id).GetValue(Canvas.LeftProperty).ToString()) + 25;
                                arista.Y2 = double.Parse(IHelper.FindChild<Ellipse>(Application.Current.MainWindow, "nodo_" + id).GetValue(Canvas.TopProperty).ToString()) + 50;
                            }
                            else if (nombre_ar.ElementAt(4) == 'l')
                            {
                                Line arista = IHelper.FindChild<Line>(Application.Current.MainWindow, idss[i]);
                                arista.X2 = double.Parse(IHelper.FindChild<Ellipse>(Application.Current.MainWindow, "nodo_" + id).GetValue(Canvas.LeftProperty).ToString());
                                arista.Y2 = double.Parse(IHelper.FindChild<Ellipse>(Application.Current.MainWindow, "nodo_" + id).GetValue(Canvas.TopProperty).ToString()) + 25;
                            }
                        }
                    }
                }
            }
        }

        private void QuitarAristaCanvas(string id)
        {
            string[] a_eliminar = Arista.AristasAEliminar(aristas, id);

            for (int i = 0; i < a_eliminar.Length; i++)
            {
                canvas.Children.Remove(IHelper.FindChild<Line>(Application.Current.MainWindow, a_eliminar[i]));
            }

            aristas = Arista.EliminarRegistro(aristas, a_eliminar);
            RenombrarAristas(id);
            matriz = CalculoHelper.CalcularMatriz(nodo_ids, aristas, matriz);
            max = Arista.TotalAristas(aristas);
        }

        private void RenombrarAristas(string id)
        {
            int idd = int.Parse(id) + 1;
            int total_aristas = Arista.TotalAristas(aristas);
            int total_nodos = nodo.TotalNodos(nodo_ids);

            Console.WriteLine(idd);

            for (int i = idd; i <= total_nodos + 1; i++)
            {
                for (int j = 0; j < total_aristas; j++)
                {

                    Console.WriteLine(aristas[j]);
                    if (aristas[j].Contains(i.ToString()))
                    {
                        Line ar = IHelper.FindChild<Line>(Application.Current.MainWindow, aristas[j]);
                        string nuevo = Arista.RenombrarID(ar, i - 1);
                        aristas[j] = nuevo;
                        ar.Name = nuevo;
                        Console.WriteLine(id + " " + nuevo);
                    }
                }
            }
        }

        private void mostrarAristas()
        {
            label_2.Content = "";
            for (int i = 0; i < aristas.Length; i++)
            {
                if (aristas[i] == null)
                {

                }
                else
                {
                    label_2.Content += i + ": " + aristas[i] + " ";
                }
            }
        }

        private void set_inicio_Click(object sender, RoutedEventArgs e)
        {
            if (nodo.TotalNodos(nodo_ids) == 0 || Arista.TotalAristas(aristas) == 0)
            {
                MessageBox.Show("No hay nodos u aristas existentes.");
            }
            else
            {
                CalculoHelper.ResolverGrafo(matriz, nodo_inicio, nodo.TotalNodos(nodo_ids));
            }   
        }
        
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }
    }
}
