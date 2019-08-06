// ***********************************************************************
// Assembly         : FiseiD
// Author           : Usuario
// Created          : 01-29-2018
//
// Last Modified By : Usuario
// Last Modified On : 01-30-2018
// ***********************************************************************
// <copyright file="FormularioReporteA.xaml.cs" company="">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;

using System.IO;
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
using System.Windows.Shapes;

namespace EvaluacionErgonomica
{
    /// <summary>
    /// Lógica de interacción para FormularioReporteA.xaml
    /// </summary>
    /// <seealso cref="System.Windows.Window" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class FormularioReporteA : Window
    {
        /// <summary>
        /// The evaluador rula
        /// </summary>
        System.Windows.Threading.DispatcherTimer EvaluadorRULA = new System.Windows.Threading.DispatcherTimer();

        /// <summary>
        /// Initializes a new instance of the <see cref="FormularioReporteA" /> class.
        /// </summary>
        public FormularioReporteA()
        {
            InitializeComponent();
            Loaded += PageLoaded;

            int[,] brazo = new int[6, 1] {
               {1},
               {2},
               {3},
               {4},
               {5},
               {6}
            };
            int[,] antebrazo = new int[18, 1] {
               {1},
               {2},
               {3},
               {1},
               {2},
               {3},
               {1},
               {2},
               {3},
               {1},
               {2},
               {3},
               {1},
               {2},
               {3},
               {1},
               {2},
               {3}
            };
            int[,] Unogiromuñeca = new int[19, 2] {
               {1, 2},
               {1, 2},
               {2, 2},
               {2, 3},
               {2, 3},
               {3, 3},
               {3, 4},
               {3, 3},
               {3, 4},
               {4, 4},
               {4, 4},
               {4, 4},
               {4, 4},
               {5, 5},
               {5, 6},
               {6, 6},
               {7, 7},
               {8, 8},
               {9, 9},
            };
            int[,] Dosgiromuñeca = new int[19, 2] {
               {1, 2},
               {1, 2},
               {2, 2},
               {2, 3},
               {2, 3},
               {3, 3},
               {3, 4},
               {3, 3},
               {3, 4},
               {4, 4},
               {4, 4},
               {4, 4},
               {4, 4},
               {5, 5},
               {5, 6},
               {6, 6},
               {7, 7},
               {8, 8},
               {9, 9},
            };
            int[,] Tresgiromuñeca = new int[19, 2] {
               {1, 2},
               {1, 2},
               {2, 2},
               {2, 3},
               {2, 3},
               {3, 3},
               {3, 4},
               {3, 3},
               {3, 4},
               {4, 4},
               {4, 4},
               {4, 4},
               {4, 4},
               {5, 5},
               {5, 6},
               {6, 6},
               {7, 7},
               {8, 8},
               {9, 9},
            };
            int[,] Cuatrogiromuñeca = new int[19, 2] {
               {1, 2},
               {1, 2},
               {2, 2},
               {2, 3},
               {2, 3},
               {3, 3},
               {3, 4},
               {3, 3},
               {3, 4},
               {4, 4},
               {4, 4},
               {4, 4},
               {4, 4},
               {5, 5},
               {5, 6},
               {6, 6},
               {7, 7},
               {8, 8},
               {9, 9},
            };

            /*
             
		            Giro de Muñeca		Giro de Muñeca		Giro de Muñeca		Giro de Muñeca	
Brazo	Antebrazo	    1	2	            1	2	            1	2	            1	2
1	1	                1	2	            2	2	            2	3	            3	3
	2	                2	2	            2	2	            3	3	            3	3
	3	                2	3	            3	3	            3	3	            4	4
2	1	                2	3	            3	3	            3	4	            4	4
	2	                3	3	            3	3	            3	4	            4	4
	3	                3	4	            4	4	            4	4	            5	5
3	1	                3	3	            4	4	            4	4	            5	5
	2	                3	4	            4	4	            4	4	            5	5
	3	                4	4	            4	4	            4	5	            5	5
4	1	                4	4	            4	4	            4	5	            5	5
	2	                4	4	            4	4	            4	5	            5	5
	3	                4	4	            4	5	            5	5	            6	6
5	1	                5	5	            5	5	            5	6	            6	7
	2	                5	6	            6	6	            6	7	            7	7
	3	                6	6	            6	7	            7	7	            7	8
6	1	                7	7	            7	7	            7	8	            8	9
	2	                8	8	            8	8	            8	9	            9	9
	3	                9	9	            9	9	            9	9	            9	9

            */

            int[,] a = new int[3, 10] {
                           {1, 1, 1, 2, 1, 2, 1, 2, 1, 2} ,   /*  initializers for row indexed by 0 */
                           {1, 1, 1, 2, 1, 2, 1, 2, 1, 2} ,   /*  initializers for row indexed by 0 */
                           {1, 1, 1, 2, 1, 2, 1, 2, 1, 2} ,   /*  initializers for row indexed by 0 */
            };

        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            string[] fileEntries = Directory.GetDirectories(@"D:\RGBSalida", "*");
            foreach (string fileName in fileEntries)
                listView.Items.Add(fileName);
        }
        string suma = @"\Color\";
        ScaleTransform transforma = new ScaleTransform();
        
        /// <summary>
        /// Cargars the datos evaluador.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void CargarDatosEvaluador(object sender, EventArgs e)
        {
            //string _imageDirectory = @"D:\Datos_AlvaroAnualiza\Data_v2\Color\";
            
            
            // Updating the Label which displays the current second
          
            try
            {
                string[] imagePaths = System.IO.Directory.GetFiles(_imageDirectory + suma, "*.jpg");
                BitmapImage bitmapImage1 = new BitmapImage(new Uri(imagePaths[_random.Next(imagePaths.Length)]));
                BitmapImage bitmapImage2 = new BitmapImage(new Uri(imagePaths[_random.Next(imagePaths.Length)]));
                BitmapImage bitmapImage3 = new BitmapImage(new Uri(imagePaths[_random.Next(imagePaths.Length)]));
                BitmapImage bitmapImage4 = new BitmapImage(new Uri(imagePaths[_random.Next(imagePaths.Length)]));
                BitmapImage bitmapImage5 = new BitmapImage(new Uri(imagePaths[_random.Next(imagePaths.Length)]));
                BitmapImage bitmapImage6 = new BitmapImage(new Uri(imagePaths[_random.Next(imagePaths.Length)]));
                BitmapImage bitmapImage7 = new BitmapImage(new Uri(imagePaths[_random.Next(imagePaths.Length)]));
                BitmapImage bitmapImage8 = new BitmapImage(new Uri(imagePaths[_random.Next(imagePaths.Length)]));
                BitmapImage bitmapImage9 = new BitmapImage(new Uri(imagePaths[_random.Next(imagePaths.Length)]));

                transforma.ScaleX = -1;

              



                
                
                image1.Stretch = Stretch.Fill;
                // bitmapImage1.Rotation = Rotation.Rotate180; // or 90, 0, 180

                image1.RenderTransform = transforma;
                image1.Source = bitmapImage1;

                string loc = image1.Source.ToString().Substring((image1.Source.ToString().Length - 7), 7);
                label1_Copy9.Content = image1.Source.ToString();
                textBox_Copy11.Text = rnd.Next(1, 4).ToString();

                image2.Stretch = Stretch.Fill;
               
                image2.RenderTransform = transforma;
                image2.Source = bitmapImage2;

                image3.Stretch = Stretch.Fill;
                image3.RenderTransform = transforma;
                image3.Source = bitmapImage3;
                textBox_Copy2.Text = rnd.Next(3, 4).ToString();

                image4.Stretch = Stretch.Fill;
                image4.RenderTransform = transforma;
                image4.Source = bitmapImage4;
                string loc4 = image4.Source.ToString().Substring((image4.Source.ToString().Length - 7), 7);
                label5_Copy.Content = loc4;
                textBox_Copy22.Text = rnd.Next(1, 3).ToString();

                image5.Stretch = Stretch.Fill;
                image5.RenderTransform = transforma;
                image5.Source = bitmapImage5;

                image6.Stretch = Stretch.Fill;
                image6.RenderTransform = transforma;
                image6.Source = bitmapImage6;
                textBox_Copy3.Text = rnd.Next(1, 3).ToString();

                image7.Stretch = Stretch.Fill;
                image7.RenderTransform = transforma;
                image7.Source = bitmapImage7;
                string loc7 = image7.Source.ToString().Substring((image7.Source.ToString().Length - 7), 7);
                label9_Copy.Content = loc7;
                textBox_Copy5.Text = rnd.Next(1, 4).ToString();

                image8.Stretch = Stretch.Fill;
                image8.RenderTransform = transforma;
                image8.Source = bitmapImage8;

                image9.Stretch = Stretch.Fill;
                image9.RenderTransform = transforma;
                image9.Source = bitmapImage9;
                textBox_Copy6.Text = rnd.Next(2, 4).ToString();










                //
                

            }









            catch (Exception)
            {

                //throw;
            }



            // Forcing the CommandManager to raise the RequerySuggested event
            CommandManager.InvalidateRequerySuggested();
        }


        /// <summary>
        /// The image directory
        /// </summary>
      

        /// <summary>
        /// The random
        /// </summary>
        Random _random = new Random();

        /// <summary>
        /// Handles the Checked event of the checkBox12 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void checkBox12_Checked(object sender, RoutedEventArgs e)
        {


        }
        /// <summary>
        /// The random
        /// </summary>
        Random rnd = new Random();
        /// <summary>
        /// Handles the Click event of the checkBox12 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void checkBox12_Click(object sender, RoutedEventArgs e)
        {
            if (checkBox12.IsChecked == true)
            {

                textBox_Copy22.Text = Convert.ToString(Convert.ToInt32(textBox_Copy22.Text) + 1);

            }
            else
            {

                textBox_Copy22.Text = Convert.ToString(Convert.ToInt32(textBox_Copy22.Text) - 1);
            }
        }

        /// <summary>
        /// Handles the Click event of the checkBox_Copy22 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void checkBox_Copy22_Click(object sender, RoutedEventArgs e)
        {
            if (checkBox_Copy22.IsChecked == true)
            {

                textBox_Copy22.Text = Convert.ToString(Convert.ToInt32(textBox_Copy22.Text) + 1);

            }
            else
            {

                textBox_Copy22.Text = Convert.ToString(Convert.ToInt32(textBox_Copy22.Text) - 1);
            }
        }

        /// <summary>
        /// Handles the Click event of the checkBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void checkBox_Click(object sender, RoutedEventArgs e)
        {
            if (checkBox.IsChecked == true)
            {

                textBox_Copy11.Text = Convert.ToString(Convert.ToInt32(textBox_Copy11.Text) + 1);

            }
            else
            {

                textBox_Copy11.Text = Convert.ToString(Convert.ToInt32(textBox_Copy11.Text) - 1);
            }
        }

        /// <summary>
        /// Handles the Click event of the checkBox_Copy control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void checkBox_Copy_Click(object sender, RoutedEventArgs e)
        {
            if (checkBox_Copy.IsChecked == true)
            {

                textBox_Copy11.Text = Convert.ToString(Convert.ToInt32(textBox_Copy11.Text) + 1);

            }
            else
            {

                textBox_Copy11.Text = Convert.ToString(Convert.ToInt32(textBox_Copy11.Text) - 1);
            }
        }

        /// <summary>
        /// Handles the Click event of the checkBox_Copy1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void checkBox_Copy1_Click(object sender, RoutedEventArgs e)
        {
            if (checkBox_Copy1.IsChecked == true)
            {

                textBox_Copy11.Text = Convert.ToString(Convert.ToInt32(textBox_Copy11.Text) - 1);

            }
            else
            {

                textBox_Copy11.Text = Convert.ToString(Convert.ToInt32(textBox_Copy11.Text) + 1);
            }
        }

        /// <summary>
        /// Handles the Click event of the checkBox1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void checkBox1_Click(object sender, RoutedEventArgs e)
        {
            if (checkBox1.IsChecked == true)
            {

                textBox_Copy2.Text = Convert.ToString(Convert.ToInt32(textBox_Copy2.Text) + 1);

            }
            else
            {

                textBox_Copy2.Text = Convert.ToString(Convert.ToInt32(textBox_Copy2.Text) - 1);
            }
        }

        /// <summary>
        /// Handles the Click event of the checkBox_Copy2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void checkBox_Copy2_Click(object sender, RoutedEventArgs e)
        {
            if (checkBox_Copy2.IsChecked == true)
            {

                textBox_Copy2.Text = Convert.ToString(Convert.ToInt32(textBox_Copy2.Text) + 1);

            }
            else
            {

                textBox_Copy2.Text = Convert.ToString(Convert.ToInt32(textBox_Copy2.Text) - 1);
            }
        }

        /// <summary>
        /// Handles the Click event of the checkBox_Copy3 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void checkBox_Copy3_Click(object sender, RoutedEventArgs e)
        {
            if (checkBox_Copy3.IsChecked == true)
            {

                textBox_Copy2.Text = Convert.ToString(Convert.ToInt32(textBox_Copy2.Text) - 1);

            }
            else
            {

                textBox_Copy2.Text = Convert.ToString(Convert.ToInt32(textBox_Copy2.Text) + 1);
            }
        }

        /// <summary>
        /// Handles the Click event of the checkBox2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void checkBox2_Click(object sender, RoutedEventArgs e)
        {
            if (checkBox2.IsChecked == true)
            {

                textBox_Copy3.Text = Convert.ToString(Convert.ToInt32(textBox_Copy3.Text) + 1);

            }
            else
            {

                textBox_Copy3.Text = Convert.ToString(Convert.ToInt32(textBox_Copy3.Text) - 1);
            }
        }

        /// <summary>
        /// Handles the Click event of the checkBox_Copy4 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void checkBox_Copy4_Click(object sender, RoutedEventArgs e)
        {
            if (checkBox2.IsChecked == true)
            {

                textBox_Copy3.Text = Convert.ToString(Convert.ToInt32(textBox_Copy3.Text) + 1);

            }
            else
            {

                textBox_Copy3.Text = Convert.ToString(Convert.ToInt32(textBox_Copy3.Text) + 1);
            }
        }

        /// <summary>
        /// Handles the Click event of the checkBox3 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void checkBox3_Click(object sender, RoutedEventArgs e)
        {
            if (checkBox3.IsChecked == true)
            {

                textBox_Copy5.Text = Convert.ToString(Convert.ToInt32(textBox_Copy5.Text) + 1);

            }
            else
            {

                textBox_Copy5.Text = Convert.ToString(Convert.ToInt32(textBox_Copy5.Text) + 1);
            }
        }

        /// <summary>
        /// Handles the Click event of the checkBox_Copy5 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void checkBox_Copy5_Click(object sender, RoutedEventArgs e)
        {
            if (checkBox_Copy5.IsChecked == true)
            {

                textBox_Copy5.Text = Convert.ToString(Convert.ToInt32(textBox_Copy5.Text) + 1);

            }
            else
            {

                textBox_Copy5.Text = Convert.ToString(Convert.ToInt32(textBox_Copy5.Text) - 1);
            }
        }

        /// <summary>
        /// Handles the Click event of the checkBox4 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void checkBox4_Click(object sender, RoutedEventArgs e)
        {
            if (checkBox4.IsChecked == true)
            {

                textBox_Copy6.Text = Convert.ToString(Convert.ToInt32(textBox_Copy6.Text) + 1);

            }
            else
            {

                textBox_Copy6.Text = Convert.ToString(Convert.ToInt32(textBox_Copy6.Text) - 1);
            }
        }

        /// <summary>
        /// Handles the Click event of the checkBox_Copy6 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void checkBox_Copy6_Click(object sender, RoutedEventArgs e)
        {
            if (checkBox_Copy6.IsChecked == true)
            {

                textBox_Copy6.Text = Convert.ToString(Convert.ToInt32(textBox_Copy6.Text) + 1);

            }
            else
            {

                textBox_Copy6.Text = Convert.ToString(Convert.ToInt32(textBox_Copy6.Text) - 1);
            }
        }
        string _imageDirectory = "";
        /// <summary>
        /// Handles the Click event of the button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _imageDirectory = listView.SelectedItems[0].ToString();
                // label_Copy3.Content = _imageDirectory.ToString();
                EvaluadorRULA = new System.Windows.Threading.DispatcherTimer();
                EvaluadorRULA.Tick += new EventHandler(CargarDatosEvaluador);

                //dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                EvaluadorRULA.Interval = TimeSpan.FromMilliseconds(100);
                EvaluadorRULA.Start();


                MyBorder1.Visibility = Visibility.Visible;
                MyBorder2.Visibility = Visibility.Visible;
                MyBorder3.Visibility = Visibility.Visible;
                MyBorder4.Visibility = Visibility.Visible;
                MyBorder5.Visibility = Visibility.Visible;
                MyBorder6.Visibility = Visibility.Visible;
                MyBorder7.Visibility = Visibility.Visible;
                MyBorder8.Visibility = Visibility.Visible;
                MyBorder9.Visibility = Visibility.Visible;

            }
            catch (Exception)
            {

                //throw;
            }

            

        }

        /// <summary>
        /// Handles the TextChanged event of the textBox_Copy11 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs" /> instance containing the event data.</param>
        private void textBox_Copy11_TextChanged(object sender, TextChangedEventArgs e)
        {
            /*
            if (textBox_Copy11.Text == "1")
            {
                textBox_Copy11.Background = Brushes.Green;
            }
            if (textBox_Copy11.Text == "2")
            {
                textBox_Copy11.Background = Brushes.Yellow;
            }
            if (textBox_Copy11.Text == "3")
            {
                textBox_Copy11.Background = Brushes.Pink;
            }
            if (textBox_Copy11.Text == "4")
            {
                textBox_Copy11.Background = Brushes.Red;
            }
            */
        }

        /// <summary>
        /// Handles the Click event of the Prueba control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Prueba_Click(object sender, RoutedEventArgs e)
        {
            EvaluadorRULA.Stop();
        }

        private void label_Copy3_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            imagex.Visibility = Visibility.Hidden;
           

        }

        private void image1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imagex.Visibility = Visibility.Visible;
            imagex.Stretch = Stretch.Fill;
            ScaleTransform transform = new ScaleTransform();
            transform.ScaleX = -1;
            imagex.RenderTransform = transform;

            imagex.Source = image1.Source;
        }

        private void image1_MouseLeave(object sender, MouseEventArgs e)
        {
          
        }

        private void image2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imagex.Visibility = Visibility.Visible;
            imagex.Stretch = Stretch.Fill;
            ScaleTransform transform = new ScaleTransform();
            transform.ScaleX = -1;
            imagex.RenderTransform = transform;
            imagex.Source = image2.Source;
        }

        private void image3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imagex.Visibility = Visibility.Visible;
            imagex.Stretch = Stretch.Fill;
            ScaleTransform transform = new ScaleTransform();
            transform.ScaleX = -1;
            imagex.RenderTransform = transform;
            imagex.Source = image3.Source;
        }

        private void image4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imagex.Visibility = Visibility.Visible;
            imagex.Stretch = Stretch.Fill;
            ScaleTransform transform = new ScaleTransform();
            transform.ScaleX = -1;
            imagex.RenderTransform = transform;
            imagex.Source = image4.Source;
        }

        private void image5_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imagex.Visibility = Visibility.Visible;
            imagex.Stretch = Stretch.Fill;
            ScaleTransform transform = new ScaleTransform();
            transform.ScaleX = -1;
            imagex.RenderTransform = transform;
            imagex.Source = image5.Source;
        }

        private void image6_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imagex.Visibility = Visibility.Visible;
            imagex.Stretch = Stretch.Fill;
            ScaleTransform transform = new ScaleTransform();
            transform.ScaleX = -1;
            imagex.RenderTransform = transform;
            imagex.Source = image6.Source;
        }

        private void image7_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imagex.Visibility = Visibility.Visible;
            imagex.Stretch = Stretch.Fill;
            ScaleTransform transform = new ScaleTransform();
            transform.ScaleX = -1;
            imagex.RenderTransform = transform;
            imagex.Source = image7.Source;
        }

        private void image8_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imagex.Visibility = Visibility.Visible;
            imagex.Stretch = Stretch.Fill;
            ScaleTransform transform = new ScaleTransform();
            transform.ScaleX = -1;
            imagex.RenderTransform = transform;
            imagex.Source = image8.Source;
        }

        private void image9_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imagex.Visibility = Visibility.Visible;
            imagex.Stretch = Stretch.Fill;
            ScaleTransform transform = new ScaleTransform();
            transform.ScaleX = -1;
            imagex.RenderTransform = transform;
            imagex.Source = image9.Source;
        }

        private void imagex_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string loc = imagex.Source.ToString().Substring((imagex.Source.ToString().Length - 7), 7);
            label1_Copy9.Content = imagex.Source.ToString();
            double width = imagex.ActualWidth;
            double height = imagex.ActualHeight;
            RenderTargetBitmap bmpCopied = new RenderTargetBitmap((int)Math.Round(width), (int)Math.Round(height), 96, 96, PixelFormats.Default);
            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(imagex);
                dc.DrawRectangle(vb, null, new Rect(new Point(), new Size(width, height)));
            }
            bmpCopied.Render(dv);
            Clipboard.SetImage(bmpCopied);
            imagex.Visibility = Visibility.Hidden;
        }
    }
}
