// ***********************************************************************
// Assembly         : FiseiD
// Author           : Usuario
// Created          : 12-07-2017
//
// Last Modified By : Usuario
// Last Modified On : 01-30-2018
// ***********************************************************************
// <copyright file="MainWindow.xaml.cs" company="">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Kinect;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
//using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Timer = System.Threading.Timer;
using System.Runtime.InteropServices;
using System.Globalization;
using Npgsql;
using System.Data;

namespace EvaluacionErgonomica
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    /// <seealso cref="System.Windows.Window" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class MainWindow : Window
    {

        /// <summary>
        /// The ds
        /// </summary>
        private DataSet ds = new DataSet();
        /// <summary>
        /// The dt
        /// </summary>
        private DataTable dt = new DataTable();

        /// <summary>
        /// The sensor
        /// </summary>
        KinectSensor Sensor;
        /// <summary>
        /// The frame reader c
        /// </summary>
        ColorFrameReader FrameReaderC;
        /// <summary>
        /// The bitmap to display c
        /// </summary>
        WriteableBitmap BitmapToDisplayC;
        /// <summary>
        /// The frame reader p
        /// </summary>
        DepthFrameReader FrameReaderP;
        //    WriteableBitmap BitmapToDisplayP;
        /// <summary>
        /// The frame reader i
        /// </summary>
        InfraredFrameReader FrameReaderI;
        /// <summary>
        /// The bitmap to display i
        /// </summary>
        WriteableBitmap BitmapToDisplayI;

        /// <summary>
        /// The frame reader p2
        /// </summary>
        MultiSourceFrameReader FrameReaderP2;
        /// <summary>
        /// The mode
        /// </summary>
        Mode _mode = Mode.Color;
        //private Bitmap _bitmap = null;

        //  BodyFrameReader FrameReaderE;

        /// <summary>
        /// The body data output file suffix
        /// </summary>
        private const string BodyDataOutputFileSuffix = "SALIDA.csv";
        /// <summary>
        /// The color data output file suffix
        /// </summary>
        private const string ColorDataOutputFileSuffix = "_color.yuy2";

        /// <summary>
        /// The kinect source
        /// </summary>
        private Datos _kinectSource;

        /// <summary>
        /// Dump body to file.
        /// </summary>
        private MainWindow _bodyFrameDumper;

        /// <summary>
        /// Dump body to file.
        /// </summary>
        private Timer _timer;



        //  [Option('v', "video", HelpText = "Dump color video stream data as a yuy2 raw format.")]
        /// <summary>
        /// Gets or sets a value indicating whether [dump video].
        /// </summary>
        /// <value><c>true</c> if [dump video]; otherwise, <c>false</c>.</value>
        public bool DumpVideo { get; set; }

        //  [Option('s', "synchronize", HelpText = "Synchronize streams.")]
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="MainWindow" /> is synchronize.
        /// </summary>
        /// <value><c>true</c> if synchronize; otherwise, <c>false</c>.</value>
        public bool Synchronize { get; set; }

        //  [Option("prefix", DefaultValue = "output", MetaValue = "PREFIX", HelpText = "Output files prefix.")]
        /// <summary>
        /// Gets or sets the base output file.
        /// </summary>
        /// <value>The base output file.</value>
        public string BaseOutputFile { get; set; }

        /// <summary>
        /// The connstring
        /// </summary>
        string connstring = String.Format("Server={0};Port={1};" +
                  "User Id={2};Password={3};Database={4};",
                  "127.0.0.1", "5432", "postgres",
                  "fisei123", "postgres");


        /// <summary>
        /// The connection
        /// </summary>
        NpgsqlConnection conn;
        /// <summary>
        /// Veaaaas the specified tiempo.
        /// </summary>
        /// <param name="tiempo">The tiempo.</param>
        /// <param name="tipoparte">The tipoparte.</param>
        /// <param name="px">The px.</param>
        /// <param name="py">The py.</param>
        /// <param name="pz">The pz.</param>
        /// <param name="ox">The ox.</param>
        /// <param name="oy">The oy.</param>
        /// <param name="oz">The oz.</param>
        /// <param name="ow">The ow.</param>
        /// <param name="stado">The stado.</param>
        /// <param name="NombreParte">The nombre parte.</param>
        public void veaaaa(double tiempo, int tipoparte, double px, double py, double pz, double ox, double oy, double oz, double ow, int stado, string NombreParte)
        {

            try
            {
                // PostgeSQL-style connection string
              
                // Making connection with Npgsql provider
                conn = new NpgsqlConnection(connstring);
                conn.Open();
                // quite complex sql statement
                //string sql = "SELECT * FROM kinect";
              //  string ja = "javier";

                string sql = "INSERT INTO KinectPrueba(tiempo,tipoparte,px,py,pz,ox,oy,oz,ow,stado,NombreParte) VALUES (" + tiempo + "," + tipoparte + "," + px + "," + py + "," + pz + "," + ox + "," + oy + "," + oz + "," + ow + "," + stado + ",'" + NombreParte + "')";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                // data adapter making request from our connection
               // NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);
                // i always reset DataSet before i do
                // something with it.... i don't know why :-)
               // ds.Reset();
                // filling DataSet with result from NpgsqlDataAdapter
                //da.Fill(ds);
                // since it C# DataSet can handle multiple tables, we will select first
              //  dt = ds.Tables[0];
                // connect grid to DataTable
                
             //   grilla.ItemsSource = dt.DefaultView;
                // since we only showing the result we don't need connection anymore
                conn.Close();
              
            }
            catch (Exception msg)
            {
                // something went wrong, and you wanna know why
                MessageBox.Show(msg.ToString());
               // throw;
            }

        }


        /// <summary>
        /// The frame reader e
        /// </summary>
        private BodyFrameReader FrameReaderE;
        /// <summary>
        /// The body brushes
        /// </summary>
        private readonly Brush[] BodyBrushes = new Brush[] {Brushes.White,
                                                    Brushes.Crimson,
                                                    Brushes.Indigo,
                                                    Brushes.DodgerBlue,
                                                    Brushes.Yellow,
                                                    Brushes.Pink};
        /// <summary>
        /// The bodies
        /// </summary>
        private Body[] Bodies;
        /// <summary>
        /// The body drawing group
        /// </summary>
        private DrawingGroup BodyDrawingGroup = new DrawingGroup();
        /// <summary>
        /// The body image bitmap rect
        /// </summary>
        private Rect BodyImageBitmapRect;

        /// <summary>
        /// The next status update
        /// </summary>
        private DateTime nextStatusUpdate = DateTime.MinValue;
        /// <summary>
        /// The stopwatch
        /// </summary>
        private Stopwatch stopwatch = null;
        /// <summary>
        /// The frames since update
        /// </summary>
        private uint framesSinceUpdate = 0;
        /// <summary>
        /// The FPS
        /// </summary>
        private double fps = 0.0;

        /// <summary>
        /// The next status update2
        /// </summary>
        private DateTime nextStatusUpdate2 = DateTime.MinValue;
        /// <summary>
        /// The stopwatch2
        /// </summary>
        private Stopwatch stopwatch2 = null;
        /// <summary>
        /// The frames since update2
        /// </summary>
        private uint framesSinceUpdate2 = 0;
        /// <summary>
        /// The FPS2
        /// </summary>
        private double fps2 = 0.0;

        /// <summary>
        /// The next status update3
        /// </summary>
        private DateTime nextStatusUpdate3 = DateTime.MinValue;
        /// <summary>
        /// The stopwatch3
        /// </summary>
        private Stopwatch stopwatch3 = null;
        /// <summary>
        /// The frames since update3
        /// </summary>
        private uint framesSinceUpdate3 = 0;
        /// <summary>
        /// The FPS3
        /// </summary>
        private double fps3 = 0.0;


        /// <summary>
        /// The next status update4
        /// </summary>
        private DateTime nextStatusUpdate4 = DateTime.MinValue;
        /// <summary>
        /// The stopwatch4
        /// </summary>
        private Stopwatch stopwatch4 = null;
        /// <summary>
        /// The frames since update4
        /// </summary>
        private uint framesSinceUpdate4 = 0;
        /// <summary>
        /// The FPS4
        /// </summary>
        private double fps4 = 0.0;

        /// <summary>
        /// Gets the sensor.
        /// </summary>
        public void GetSensor()
        {
            this.Sensor = KinectSensor.GetDefault();
            this.Sensor.Open();
        }

        /// <summary>
        /// Calculates the FPSC.
        /// </summary>
        /// <param name="tipo">The tipo.</param>
        /// <param name="e">The e.</param>

        public void CalculateFPSC(String tipo, String e)
        {
          
            if (tipo == "Color")
            {

                framesSinceUpdate++;
                //  stopwatch = null;
                // update status unless last message is sticky for a while
                if (DateTime.Now >= nextStatusUpdate)
                {
                    // calcuate fps based on last frame received

                    if (stopwatch.IsRunning)
                    {
                        stopwatch.Stop();
                        fps = framesSinceUpdate / stopwatch.Elapsed.TotalSeconds;
                        // 
                        FCOLOR.Content = e;
                        txtcolor.Text = fps.ToString();
                       
                       
                      

                        stopwatch.Reset();
                    }

                    nextStatusUpdate = DateTime.Now + TimeSpan.FromSeconds(1);
                }

                if (!stopwatch.IsRunning)
                {
                    framesSinceUpdate = 0;
                    stopwatch.Start();
                }


            }//color

            

        }
        /// <summary>
        /// Scolors this instance.
        /// </summary>
        public void Scolor()
        {


            stopwatch = new Stopwatch();
            
            
            //COLOR

            FrameReaderC = Sensor.ColorFrameSource.OpenReader();

            BitmapToDisplayC = new WriteableBitmap(
                FrameReaderC.ColorFrameSource.FrameDescription.Width,
                FrameReaderC.ColorFrameSource.FrameDescription.Height,
                96.0,
                96.0,
                PixelFormats.Bgra32,
                null);


            
        }
        /// <summary>
        /// Calculates the fpsi.
        /// </summary>
        /// <param name="tipo">The tipo.</param>
        /// <param name="e">The e.</param>
        public void CalculateFPSI(String tipo, String e)
        {

            if (tipo == "Infra")
            {

                framesSinceUpdate2++;
                //  stopwatch = null;
                // update status unless last message is sticky for a while
                if (DateTime.Now >= nextStatusUpdate2)
                {
                    // calcuate fps based on last frame received

                    if (stopwatch2.IsRunning)
                    {
                        stopwatch2.Stop();
                        fps2 = framesSinceUpdate2 / stopwatch2.Elapsed.TotalSeconds;
                        // 
                        FINFRA.Content = e;
                        txtinfra.Text = fps2.ToString();

                        
                        


                        stopwatch2.Reset();
                    }

                    nextStatusUpdate2 = DateTime.Now + TimeSpan.FromSeconds(1);
                }

                if (!stopwatch2.IsRunning)
                {
                    framesSinceUpdate2 = 0;
                    stopwatch2.Start();
                }


            }//color



        }
        /// <summary>
        /// Sinfras this instance.
        /// </summary>
        public void Sinfra()
        {

            stopwatch2 = new Stopwatch();
            //INFRA
            FrameReaderI = Sensor.InfraredFrameSource.OpenReader();

            BitmapToDisplayI = new WriteableBitmap(
                FrameReaderI.InfraredFrameSource.FrameDescription.Width,
                FrameReaderI.InfraredFrameSource.FrameDescription.Height,
                96.0,
                96.0,
                PixelFormats.Gray16,
                null);
        
        }
        /// <summary>
        /// Calculates the FPSP.
        /// </summary>
        /// <param name="tipo">The tipo.</param>
        /// <param name="e">The e.</param>
        public void CalculateFPSP(String tipo, String e)
        {

            if (tipo == "Pro")
            {

                framesSinceUpdate3++;
                //  stopwatch = null;
                // update status unless last message is sticky for a while
                if (DateTime.Now >= nextStatusUpdate3)
                {
                    // calcuate fps based on last frame received

                    if (stopwatch3.IsRunning)
                    {
                        stopwatch3.Stop();
                        fps3 = framesSinceUpdate3 / stopwatch3.Elapsed.TotalSeconds;
                        // 
                        FPRO.Content = e;

                        txtprofundidad.Text = fps3.ToString();
                        


                        stopwatch3.Reset();
                    }

                    nextStatusUpdate3 = DateTime.Now + TimeSpan.FromSeconds(1);
                }

                if (!stopwatch3.IsRunning)
                {
                    framesSinceUpdate3 = 0;
                    stopwatch3.Start();
                }


            }//color



        }
        /// <summary>
        /// Sprofundidads this instance.
        /// </summary>
        public void Sprofundidad()
        {
            stopwatch3 = new Stopwatch();
            FrameReaderP2 = Sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Depth | FrameSourceTypes.Infrared | FrameSourceTypes.Body);
            FrameReaderP2.MultiSourceFrameArrived += Reader_FrameArrived2;
                   

        }
        /// <summary>
        /// Calculates the fpse.
        /// </summary>
        /// <param name="tipo">The tipo.</param>
        /// <param name="e">The e.</param>
        public void CalculateFPSE(String tipo, String e)
        {

            if (tipo == "Esque")
            {

                framesSinceUpdate4++;
                //  stopwatch = null;
                // update status unless last message is sticky for a while
                if (DateTime.Now >= nextStatusUpdate4)
                {
                    // calcuate fps based on last frame received

                    if (stopwatch4.IsRunning)
                    {
                        stopwatch4.Stop();
                        fps4 = framesSinceUpdate4 / stopwatch4.Elapsed.TotalSeconds;
                        // 
                        FESQUE.Content = e;
                        txtesqueleto.Text = fps4.ToString();
                      



                        stopwatch4.Reset();
                    }

                    nextStatusUpdate4 = DateTime.Now + TimeSpan.FromSeconds(1);
                }

                if (!stopwatch4.IsRunning)
                {
                    framesSinceUpdate4 = 0;
                    stopwatch4.Start();
                }


            }//



        }
        /// <summary>
        /// Sesqueletoes this instance.
        /// </summary>
        public void Sesqueleto()
        {

            stopwatch4 = new Stopwatch();
            var depthDesc = this.Sensor.DepthFrameSource.FrameDescription;
            FrameReaderE = this.Sensor.BodyFrameSource.OpenReader();
            FrameReaderE.FrameArrived += Reader_FrameArrived;
            Bodies = new Body[this.Sensor.BodyFrameSource.BodyCount];
            BodyImageBitmapRect = new Rect(0,
                                        0,
                                        depthDesc.Width,
                                        depthDesc.Height);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow" /> class.
        /// </summary>
        public MainWindow()
        {
            //btncolor.IsEnabled = false;
            //btnprofundidad.IsEnabled = false;
            //btninfra.IsEnabled = false;
            //btnesqueleto.IsEnabled = false;
            //
            InitializeComponent();

           this.WindowStyle = System.Windows.WindowStyle.None;
           this.WindowState = System.Windows.WindowState.Maximized;

       
               //lista.Items.Add("aaa");



           System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
           customCulture.NumberFormat.NumberDecimalSeparator = ".";
           System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;



           //Loaded += OpenKinect;

          // Closing += CloseKinect();
           
          
        }

        /// <summary>
        /// Handles the FrameArrived2 event of the Reader control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MultiSourceFrameArrivedEventArgs" /> instance containing the event data.</param>
        private unsafe void Reader_FrameArrived2(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            

            //CalculateFPSP("Pro", (_InfraredFrame.RelativeTime.TotalSeconds / 10000).ToString());
            var reference = e.FrameReference.AcquireFrame();
            // If you do not dispose of the frame, you never get another one...
            // Depth
            using (var frame = reference.DepthFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    if (_mode == Mode.Depth)
                    {
                      

                       CalculateFPSP("Pro", (frame.RelativeTime.TotalSeconds / 10000).ToString());
                       camera.Source = frame.ToBitmap();

                       /*using (Microsoft.Kinect.KinectBuffer depthBuffer = frame.LockImageBuffer())
                       {


                           if ((frame.FrameDescription.Width * frame.FrameDescription.Height) == (depthBuffer.Size / frame.FrameDescription.BytesPerPixel))
                           {
                               ushort* frameData = (ushort*)depthBuffer.UnderlyingBuffer;
                               byte[] rawDataConverted = new byte[(int)(depthBuffer.Size / 2)];


                               for (int i = 0; i < (int)(depthBuffer.Size / 2); ++i)
                               {
                                   ushort depth = frameData[i];
                                   rawDataConverted[i] = (byte)(depth >= frame.DepthMinReliableDistance && depth <= frame.DepthMaxReliableDistance ? (depth) : 0);
                               }

                             //  String date = string.Format("{0:hh-mm-ss}", DateTime.Now);
                             ////  String filePath = System.IO.Directory.GetCurrentDirectory() + "/VEAMOS/" + date + ".JPEG";
                             //  File.WriteAllBytes(filePath, rawDataConverted);
                             //  rawDataConverted = null;

                           }
                       }*/
                    }
                    
                    
                    }
                  
            }

        }

        /// <summary>
        /// Enum Mode
        /// </summary>
        public enum Mode
        {
            /// <summary>
            /// The depth
            /// </summary>
            Depth,
            /// <summary>
            /// The infrared
            /// </summary>
            Infrared,
            /// <summary>
            /// The color
            /// </summary>
            Color

        }
        /// <summary>
        /// The bodies
        /// </summary>
        Body[] bodies;
        /// <summary>
        /// Handles the FrameArrived event of the Reader control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="BodyFrameArrivedEventArgs" /> instance containing the event data.</param>
        private void Reader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {

           
            
            var frameReference = e.FrameReference;                       
              using (var frame = frameReference.AcquireFrame())            
              {

                  try
                  {

                      CalculateFPSE("Esque", (frame.RelativeTime.TotalSeconds / 10000).ToString());
                  }
                  catch (Exception)
                  {
                      
                     // throw;
                  }
                  
                  
                  
                  
                  if (frame == null)
                      return;

                  if (bodies == null)
                  {  //Create an array of the bodies in the scene and update it
                      bodies = new Body[frame.BodyCount];
                  }
                  frame.GetAndRefreshBodyData(bodies);



                if (frame != null)
                {
                    

                    foreach (Body body in bodies)

                    {
                        try
                        {

                            if (body.IsTracked)
                           {

                                var joints = body.Joints; // Get all of the joints in that body
                                if (joints[JointType.HandRight].TrackingState == TrackingState.Tracked &&
                                    joints[JointType.HandLeft].TrackingState == TrackingState.Tracked)
                                {
                                    //txtLeft.Text = joints[JointType.HandLeft].Position.Y.ToString();
                                    //txtRight.Text = joints[JointType.HandRight].Position.Y.ToString();

                                    // txtiz.Text = joints[JointType.HandLeft].Position.Z.ToString();
                                    // txtder.Text = joints[JointType.HandRight].Position.Z.ToString();
                                    TXTX.Text = joints[JointType.HandRight].Position.X.ToString();
                                    TXTXA.Text = joints[JointType.WristRight].Position.X.ToString();
                                    TXTXB.Text = joints[JointType.ElbowRight].Position.X.ToString();
                                    TXTXC.Text = joints[JointType.ShoulderRight].Position.X.ToString();




                                    TXTY.Text = joints[JointType.HandRight].Position.Y.ToString();
                                    TXTYA.Text = joints[JointType.WristRight].Position.Y.ToString();
                                    TXTYB.Text = joints[JointType.ElbowRight].Position.Y.ToString();
                                    TXTYC.Text = joints[JointType.ShoulderRight].Position.Y.ToString();



                                    TXTZ.Text = joints[JointType.HandRight].Position.Z.ToString();
                                    TXTZA.Text = joints[JointType.WristRight].Position.Z.ToString();
                                    TXTZB.Text = joints[JointType.ElbowRight].Position.Z.ToString();
                                    TXTZC.Text = joints[JointType.ShoulderRight].Position.Z.ToString();
                                   //CABEZA
                                    if (joints[JointType.Head].Position.X > -0.25 && joints[JointType.Head].Position.Y < 0.44)

                                    {


                                       
                                        TXTUNO.Text = "1";

                                        TXTUNO.Background = Brushes.Green;
                                        //avgNear.Content = "Evaluación favorable cerca de la media " + joints[JointType.HandRight].Position.Z;
                                    }
                                    else
                                    {
                                       // TXTUNO.Text = "1";

                                        // TXTUNO.Text = "2";
                                        TXTUNO.Background = Brushes.Green;
                                    }
                                    if (joints[JointType.Head].Position.X > -0.39 && joints[JointType.Head].Position.Y < 0.34)
                                    {
                                           TXTUNO.Text = "2";
                                        TXTUNO.Background = Brushes.LightYellow;
                                        //avgNear.Content = "Evaluación favorable cerca de la media " + joints[JointType.HandRight].Position.Z;
                                    }
                                    else
                                    {
                                       // TXTUNO.Text = "2";
                                        // TXTUNO.Text = "3";
                                        TXTUNO.Background = Brushes.LightYellow;

                                    }
                                    if (joints[JointType.Head].Position.X > -0.73 && joints[JointType.Head].Position.Y < 0.29)
                                    {
                                        TXTUNO.Text = "3";
                                        TXTUNO.Background = Brushes.Yellow;
                                        //avgNear.Content = "Evaluación favorable cerca de la media " + joints[JointType.HandRight].Position.Z;
                                    }
                                    else
                                    {
                                        //TXTUNO.Text = "3";
                                        //  TXTUNO.Text = "3";
                                        TXTUNO.Background = Brushes.Yellow;
                                    }
                                    if (joints[JointType.Head].Position.X > -0.15 && joints[JointType.Head].Position.Y < 0.45)
                                    {
                                        TXTUNO.Text = "4";
                                        TXTUNO.Background = Brushes.Red;
                                        //avgNear.Content = "Evaluación favorable cerca de la media " + joints[JointType.HandRight].Position.Z;
                                    }
                                    else
                                    {
                                        //TXTUNO.Text = "4";
                                        TXTUNO.Background = Brushes.Red;
                                        //  TXTUNO.Text = "3";
                                    }

                                    //BRAZO derecho
                                    if (joints[JointType.ElbowRight].Position.X > -0.09 && joints[JointType.ElbowRight].Position.Y < -0.11)
                                    {
                                        TXTDOS.Text = "1";

                                        TXTDOS.Background = Brushes.Green;
                                        //avgNear.Content = "Evaluación favorable cerca de la media " + joints[JointType.HandRight].Position.Z;
                                    }
                                    else
                                    {
                                        //TXTDOS.Text = "1";

                                        // TXTUNO.Text = "2";
                                        TXTDOS.Background = Brushes.Green;
                                    }
                                    if (joints[JointType.ElbowRight].Position.X > -0.40 && joints[JointType.ElbowRight].Position.Y < -0.01)
                                    {
                                        TXTDOS.Text = "2";
                                        TXTDOS.Background = Brushes.LightYellow;
                                        //avgNear.Content = "Evaluación favorable cerca de la media " + joints[JointType.HandRight].Position.Z;
                                    }
                                    else
                                    {
                                        //TXTDOS.Text = "2";
                                        // TXTUNO.Text = "3";
                                        TXTDOS.Background = Brushes.LightYellow;

                                    }
                                    //DOS
                                    if (joints[JointType.ElbowRight].Position.X > -0.54 && joints[JointType.ElbowRight].Position.Y < -0.07)
                                    {
                                        TXTDOS.Text = "2";
                                        TXTDOS.Background = Brushes.LightYellow;
                                        //avgNear.Content = "Evaluación favorable cerca de la media " + joints[JointType.HandRight].Position.Z;
                                    }
                                    else
                                    {
                                        //TXTDOS.Text = "2";
                                        // TXTUNO.Text = "3";
                                        TXTDOS.Background = Brushes.LightYellow;

                                    }
                                    if (joints[JointType.ElbowRight].Position.X > -0.54 && joints[JointType.ElbowRight].Position.Y < 0.15)
                                    {
                                        TXTDOS.Text = "3";
                                        TXTDOS.Background = Brushes.Yellow;
                                        //avgNear.Content = "Evaluación favorable cerca de la media " + joints[JointType.HandRight].Position.Z;
                                    }
                                    else
                                    {
                                        //TXTDOS.Text = "3";

                                        //  TXTUNO.Text = "3";
                                        TXTDOS.Background = Brushes.Yellow;
                                    }
                                    if (joints[JointType.ElbowRight].Position.X > -0.47 && joints[JointType.ElbowRight].Position.Y < 0.47)
                                    {
                                        TXTDOS.Text = "4";
                                        TXTDOS.Background = Brushes.Red;
                                        //avgNear.Content = "Evaluación favorable cerca de la media " + joints[JointType.HandRight].Position.Z;
                                    }
                                    else
                                    {
                                      //  TXTDOS.Text = "4";
                                       // TXTDOS.Background = Brushes.Red;
                                        //  TXTUNO.Text = "3";
                                    }



                                    //ANTE BRAZO DERECHO
                                    if (joints[JointType.ShoulderRight].Position.X > -0.09 && joints[JointType.ShoulderRight].Position.Y < 0.18)
                                    {
                                        TXTRES.Text = "2";

                                        TXTRES.Background = Brushes.LightYellow;
                                        //avgNear.Content = "Evaluación favorable cerca de la media " + joints[JointType.HandRight].Position.Z;
                                    }
                                    else
                                    {
                                       // TXTRES.Text = "2";
                                        // TXTUNO.Text = "2";
                                        TXTRES.Background = Brushes.LightYellow;
                                    }
                                    if (joints[JointType.ShoulderRight].Position.X > -0.16 && joints[JointType.ShoulderRight].Position.Y < 0.21)
                                    {
                                        TXTRES.Text = "1";
                                        TXTRES.Background = Brushes.Green;
                                        //avgNear.Content = "Evaluación favorable cerca de la media " + joints[JointType.HandRight].Position.Z;
                                    }
                                    else
                                    {
                                       // TXTRES.Text = "1";
                                        // TXTUNO.Text = "3";
                                        TXTRES.Background = Brushes.Green;

                                    }
                                    //DOS
                                    if (joints[JointType.ShoulderRight].Position.X > -0.40 && joints[JointType.ShoulderRight].Position.Y < 0.18)
                                    {
                                        TXTRES.Text = "2";
                                        TXTRES.Background = Brushes.LightYellow;
                                        //avgNear.Content = "Evaluación favorable cerca de la media " + joints[JointType.HandRight].Position.Z;
                                    }
                                    else
                                    {
                                       // TXTRES.Text = "2";
                                        // TXTUNO.Text = "3";
                                        TXTRES  .Background = Brushes.LightYellow;

                                    }


                                    //MUÑE DERE
                                    if (joints[JointType.WristRight].Position.X > -0.63 && joints[JointType.WristRight].Position.Y < -0.25)
                                    {
                                        TXTCUATRO.Text = "1";

                                        TXTCUATRO.Background = Brushes.Green;
                                        //avgNear.Content = "Evaluación favorable cerca de la media " + joints[JointType.HandRight].Position.Z;
                                    }
                                    else
                                    {
                                       // TXTCUATRO.Text = "1";
                                        // TXTUNO.Text = "2";
                                        TXTCUATRO.Background = Brushes.Green;
                                    }
                                    if (joints[JointType.WristRight].Position.X > -0.65 && joints[JointType.WristRight].Position.Y < -0.27)
                                    {
                                        TXTCUATRO.Text = "2";
                                        TXTCUATRO.Background = Brushes.LightYellow;
                                        //avgNear.Content = "Evaluación favorable cerca de la media " + joints[JointType.HandRight].Position.Z;
                                    }
                                    else
                                    {
                                        //TXTCUATRO.Text = "2";
                                        // TXTUNO.Text = "3";
                                        TXTCUATRO.Background = Brushes.LightYellow;

                                    }
                                    if (joints[JointType.WristRight].Position.X > -0.61 && joints[JointType.WristRight].Position.Y < -0.22)
                                    {
                                        TXTCUATRO.Text = "3";
                                        TXTCUATRO.Background = Brushes.Yellow;
                                        //avgNear.Content = "Evaluación favorable cerca de la media " + joints[JointType.HandRight].Position.Z;
                                    }
                                    else
                                    {
                                      //  TXTCUATRO.Text = "3";
                                        //  TXTUNO.Text = "3";
                                        TXTCUATRO.Background = Brushes.Yellow;
                                    }



                                    
                                    //BRAZO izquierdo
                                    if (joints[JointType.ElbowLeft].Position.X > -0.67 && joints[JointType.ElbowLeft].Position.Y < -0.09)
                                    {
                                        TXTSIETE.Text = "1";

                                        TXTSIETE.Background = Brushes.Green;
                                        //avgNear.Content = "Evaluación favorable cerca de la media " + joints[JointType.HandRight].Position.Z;
                                    }
                                    else
                                    {
                                        //TXTDOS.Text = "1";

                                        // TXTUNO.Text = "2";
                                        TXTSIETE.Background = Brushes.Green;
                                    }
                                    if (joints[JointType.ElbowLeft].Position.X > -0.70 && joints[JointType.ElbowLeft].Position.Y < 0.07)
                                    {
                                        TXTSIETE.Text = "2";
                                        TXTSIETE.Background = Brushes.LightYellow;
                                        //avgNear.Content = "Evaluación favorable cerca de la media " + joints[JointType.HandRight].Position.Z;
                                    }
                                    else
                                    {
                                        //TXTDOS.Text = "2";
                                        // TXTUNO.Text = "3";
                                        TXTSIETE.Background = Brushes.LightYellow;

                                    }
                                    //DOS
                                    if (joints[JointType.ElbowLeft].Position.X > -0.52 && joints[JointType.ElbowLeft].Position.Y < -0.05)
                                    {
                                        TXTSIETE.Text = "2";
                                        TXTSIETE.Background = Brushes.LightYellow;
                                        //avgNear.Content = "Evaluación favorable cerca de la media " + joints[JointType.HandRight].Position.Z;
                                    }
                                    else
                                    {
                                        //TXTDOS.Text = "2";
                                        // TXTUNO.Text = "3";
                                        TXTSIETE.Background = Brushes.LightYellow;

                                    }
                                    if (joints[JointType.ElbowLeft].Position.X > -0.31 && joints[JointType.ElbowLeft].Position.Y < 0.18)
                                    {
                                        TXTSIETE.Text = "3";
                                        TXTSIETE.Background = Brushes.Yellow;
                                        //avgNear.Content = "Evaluación favorable cerca de la media " + joints[JointType.HandRight].Position.Z;
                                    }
                                    else
                                    {
                                        //TXTDOS.Text = "3";

                                        //  TXTUNO.Text = "3";
                                        TXTSIETE.Background = Brushes.Yellow;
                                    }
                                    if (joints[JointType.ElbowLeft].Position.X > -0.63 && joints[JointType.ElbowLeft].Position.Y < 0.44)
                                    {
                                        TXTSIETE.Text = "4";
                                        TXTSIETE.Background = Brushes.Red;
                                        //avgNear.Content = "Evaluación favorable cerca de la media " + joints[JointType.HandRight].Position.Z;
                                    }
                                    else
                                    {
                                        //  TXTDOS.Text = "4";
                                       // TXTSIETE.Background = Brushes.Red;
                                        //  TXTUNO.Text = "3";
                                    }



                                    //ANTE BRAZO IZQUIERDO
                                    if (joints[JointType.ShoulderLeft].Position.X > -0.66 && joints[JointType.ShoulderLeft].Position.Y < 0.18)
                                    {
                                        TXTOCHO.Text = "2";

                                        TXTOCHO.Background = Brushes.LightYellow;
                                        //avgNear.Content = "Evaluación favorable cerca de la media " + joints[JointType.HandRight].Position.Z;
                                    }
                                    else
                                    {
                                        // TXTRES.Text = "2";
                                        // TXTUNO.Text = "2";
                                        TXTOCHO.Background = Brushes.LightYellow;
                                    }
                                    if (joints[JointType.ShoulderLeft].Position.X > -0.59 && joints[JointType.ShoulderLeft].Position.Y < 0.13)
                                    {
                                        TXTOCHO.Text = "1";
                                        TXTOCHO.Background = Brushes.Green;
                                        //avgNear.Content = "Evaluación favorable cerca de la media " + joints[JointType.HandRight].Position.Z;
                                    }
                                    else
                                    {
                                        // TXTRES.Text = "1";
                                        // TXTUNO.Text = "3";
                                        TXTOCHO.Background = Brushes.Green;

                                    }
                                    //DOS
                                    if (joints[JointType.ShoulderLeft].Position.X > -0.55 && joints[JointType.ShoulderLeft].Position.Y < 0.21)
                                    {
                                        TXTOCHO.Text = "2";
                                        TXTOCHO.Background = Brushes.LightYellow;
                                        //avgNear.Content = "Evaluación favorable cerca de la media " + joints[JointType.HandRight].Position.Z;
                                    }
                                    else
                                    {
                                        // TXTRES.Text = "2";
                                        // TXTUNO.Text = "3";
                                        TXTOCHO.Background = Brushes.LightYellow;

                                    }


                                    //MUÑE IZ
                                    if (joints[JointType.WristLeft].Position.X > -0.35 && joints[JointType.WristLeft].Position.Y < -0.32)
                                    {
                                        TXTNUEVE.Text = "1";

                                        TXTNUEVE.Background = Brushes.Green;
                                        //avgNear.Content = "Evaluación favorable cerca de la media " + joints[JointType.HandRight].Position.Z;
                                    }
                                    else
                                    {
                                        // TXTCUATRO.Text = "1";
                                        // TXTUNO.Text = "2";
                                        TXTNUEVE.Background = Brushes.Green;
                                    }
                                    if (joints[JointType.WristLeft].Position.X > -0.26 && joints[JointType.WristLeft].Position.Y < -0.30)
                                    {
                                        TXTNUEVE.Text = "2";
                                        TXTNUEVE.Background = Brushes.LightYellow;
                                        //avgNear.Content = "Evaluación favorable cerca de la media " + joints[JointType.HandRight].Position.Z;
                                    }
                                    else
                                    {
                                        //TXTCUATRO.Text = "2";
                                        // TXTUNO.Text = "3";
                                        TXTNUEVE.Background = Brushes.LightYellow;

                                    }
                                    if (joints[JointType.WristLeft].Position.X > -0.42 && joints[JointType.WristLeft].Position.Y < -0.22)
                                    {
                                        TXTNUEVE.Text = "3";
                                        TXTNUEVE.Background = Brushes.Yellow;
                                        //avgNear.Content = "Evaluación favorable cerca de la media " + joints[JointType.HandRight].Position.Z;
                                    }
                                    else
                                    {
                                        //  TXTCUATRO.Text = "3";
                                        //  TXTUNO.Text = "3";
                                        TXTNUEVE.Background = Brushes.Yellow;
                                    }

                                 
                                   if (joints[JointType.HandLeft].Position.Z < 1.83704366)
                                   {
                                       avgFar.Content = "Evaluación RAW cerca de la media " + joints[JointType.HandLeft].Position.Z;
                                   }
                                   else
                                   {

                                       avgFar.Content = "ABIERTA";
                                   }
                                   if (joints[JointType.Head].Position.Z < 1.83704366)
                                   {

                                       avgCA.Content = "MOVIMIENTO CABEZA " + joints[JointType.Head].Position.Z;
                                   }
                                   else
                                   {

                                       avgCA.Content = "FRONTAL " + joints[JointType.Head].Position.X;
                                   }
                                   if (joints[JointType.Head].Position.X > -0.24000000)
                                   {
                                       avgCA_Copy1.Content = "MOVIMIENTO CABEZA DERECHA " + joints[JointType.Head].Position.X;
                                   }
                                   else
                                   {
                                       //      
                                        avgCA_Copy1.Content = "IZQUIERDA - DERECHA";   


                                   }
                                   if (joints[JointType.Head].Position.X < -0.36000000)
                                   {
                                       avgCA_Copy.Content = "MOVIMIENTO CABEZA IZQUIERDA " + joints[JointType.Head].Position.X;

                                   }

                           // else



                                    avgCA_Copy.Content = "IZQUIERDA - DERECHA";   
                                    
                                }

                            }
                        }
                        catch (Exception)
                        {
                            
                           // throw;
                        }
                       
                        
                    }


                  using (var dc = this.BodyDrawingGroup.Open())
                  {
                    dc.DrawRectangle(Brushes.Black, null, this.BodyImageBitmapRect);
                    frame.GetAndRefreshBodyData(this.Bodies);              
                    for (int index = 0; index <= this.Bodies.Length - 1; index++)
                    {
                      if (this.Bodies[index].IsTracked)
                      {
                        IReadOnlyDictionary<JointType, Joint> joints = this.Bodies[index].Joints;
                        Dictionary<JointType, Point> points = new Dictionary<JointType, Point>();
                        foreach (var joint in joints.Keys)                  
                        {
                            var pos = this.Sensor.CoordinateMapper.MapCameraPointToDepthSpace(joints[joint].Position);
                          points[joint] = new Point(pos.X, pos.Y);
                        }
                        DrawBody(joints, points, BodyBrushes[index], dc);  
                      }
                    }
                    this.BodyDrawingGroup.ClipGeometry = new RectangleGeometry(this.BodyImageBitmapRect);
                    ScreenImageE.Source = new DrawingImage(this.BodyDrawingGroup);
                  }
                }
  }
        }
        /// <summary>
        /// Draws the body.
        /// </summary>
        /// <param name="joints">The joints.</param>
        /// <param name="jointPoints">The joint points.</param>
        /// <param name="bodyBrash">The body brash.</param>
        /// <param name="dc">The dc.</param>
        private void DrawBody(IReadOnlyDictionary<JointType, Joint> joints,
                                  Dictionary<JointType, Point> jointPoints,
                                  Brush bodyBrash,
                                  DrawingContext dc)
            {
              
              foreach (JointType jointType in joints.Keys)
              {
                TrackingState state = joints[jointType].TrackingState;

                if (state == TrackingState.Tracked)
                {
                  dc.DrawEllipse(Brushes.White,
                                 null,
                                 jointPoints[jointType],                   
                                 10,
                                 10);
                }
              }
            }



        /// <summary>
        /// Checks for exit.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs" /> instance containing the event data.</param>
        private void CheckForExit(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                App.Current.Shutdown();
            }
        }




        /// <summary>
        /// The automatic exposure
        /// </summary>
        private float autoExposure;
        /// <summary>
        /// Colors the frame arrived.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ColorFrameArrivedEventArgs" /> instance containing the event data.</param>
        private void ColorFrameArrived(object sender, ColorFrameArrivedEventArgs e)
        {

            
            if (null == e.FrameReference) return;

            // If you do not dispose of the frame, you never get another one...
          
            using (ColorFrame _ColorFrame = e.FrameReference.AcquireFrame())
            {
                if (null == _ColorFrame) return;

                
                float shutterGain = ((float)_ColorFrame.ColorCameraSettings.ExposureTime.TotalMilliseconds / 333333f) / 0.5f;
                float gain = _ColorFrame.ColorCameraSettings.Gain / 3;
                autoExposure = shutterGain * gain;

             //   CalculateFPSC("Color", (_ColorFrame.RelativeTime.TotalSeconds / 10000).ToString());
                CalculateFPSC("Color", (autoExposure).ToString());
                BitmapToDisplayC.Lock();
                _ColorFrame.CopyConvertedFrameDataToIntPtr(
                    BitmapToDisplayC.BackBuffer,
                    Convert.ToUInt32(BitmapToDisplayC.BackBufferStride * BitmapToDisplayC.PixelHeight),
                    ColorImageFormat.Bgra);
                BitmapToDisplayC.AddDirtyRect(
                    new Int32Rect(
                        0,
                        0,
                        _ColorFrame.FrameDescription.Width,
                        _ColorFrame.FrameDescription.Height));
                BitmapToDisplayC.Unlock();


                
            }
            
           // var reference = e.FrameReference.AcquireFrame();
            
        }
        /*
        private void DepthFrameArrived(object sender, DepthFrameArrivedEventArgs e)
        {
          
            
            if (null == e.FrameReference)
                return;

            // If you do not dispose of the frame, you never get another one...
            using (DepthFrame _DepthFrame = e.FrameReference.AcquireFrame())
            {
                if (null == _DepthFrame) return;

                BitmapToDisplayP.Lock();
                _DepthFrame.CopyFrameDataToIntPtr(
                    BitmapToDisplayP.BackBuffer,
                    Convert.ToUInt32(BitmapToDisplayP.BackBufferStride * BitmapToDisplayP.PixelHeight));
                BitmapToDisplayP.AddDirtyRect(
                    new Int32Rect(
                        0,
                        0,
                        _DepthFrame.FrameDescription.Width,
                        _DepthFrame.FrameDescription.Height));
                BitmapToDisplayP.Unlock();
            }
        }
         * */
        /// <summary>
        /// Infrareds the frame arrived.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="InfraredFrameArrivedEventArgs" /> instance containing the event data.</param>
        private void InfraredFrameArrived(object sender, InfraredFrameArrivedEventArgs e)
        {
          // CalculateFPSI("Infra", (_ColorFrame.RelativeTime.TotalSeconds/10000).ToString());
            
            if (null == e.FrameReference)
                return;

            // If you do not dispose of the frame, you never get another one...
            using (InfraredFrame _InfraredFrame = e.FrameReference.AcquireFrame())
            {
                if (null == _InfraredFrame) return;
                
                CalculateFPSI("Infra", (_InfraredFrame.RelativeTime.TotalSeconds / 10000).ToString());
                BitmapToDisplayI.Lock();
                _InfraredFrame.CopyFrameDataToIntPtr(
                    BitmapToDisplayI.BackBuffer,
                    Convert.ToUInt32(BitmapToDisplayI.BackBufferStride * BitmapToDisplayI.PixelHeight));
                BitmapToDisplayI.AddDirtyRect(
                    new Int32Rect(
                        0,
                        0,
                        _InfraredFrame.FrameDescription.Width,
                        _InfraredFrame.FrameDescription.Height));
                BitmapToDisplayI.Unlock();
            }
        }




        /*
         private void BodyFrameArrived(object sender, BodyFrameArrivedEventArgs e)
         {
             if (null == e.FrameReference)
                 return;

             // Remove all the old points and clipping lines
             while (0 < KinectCanvasE.Children.Count)
             {
                 KinectCanvasE.Children.RemoveAt(0);
             }

             // If you do not dispose of the frame, you never get another one...
             using (BodyFrame _BodyFrame = e.FrameReference.AcquireFrame())
             {
                 if (null == _BodyFrame)
                     return;

                 Body[] _Bodies = new Body[_BodyFrame.BodyFrameSource.BodyCount];
                 _BodyFrame.GetAndRefreshBodyData(_Bodies);

                 foreach (Body _Body in _Bodies)
                     if (_Body.IsTracked)
                     {
                         foreach (Joint _Joint in _Body.Joints.Values)
                             if (TrackingState.Tracked == _Joint.TrackingState)
                             {
                                 Ellipse _Ellipse = new Ellipse();
                                 _Ellipse.Stroke = Brushes.Green;
                                 _Ellipse.Fill = Brushes.Green;
                                 _Ellipse.Width = 30;
                                 _Ellipse.Height = 30;

                                 ColorSpacePoint _ColorSpacePoint = Sensor.CoordinateMapper.MapCameraPointToColorSpace(_Joint.Position);
                                 Canvas.SetLeft(_Ellipse, _ColorSpacePoint.X);
                                 Canvas.SetTop(_Ellipse, _ColorSpacePoint.Y);
                                 KinectCanvasE.Children.Add(_Ellipse);
                             }

                         if (FrameEdges.Top == (FrameEdges.Top & _Body.ClippedEdges))
                             CreateClippingLine(0, 0, KinectCanvasE.ActualWidth, 0);

                         if (FrameEdges.Left == (FrameEdges.Left & _Body.ClippedEdges))
                             CreateClippingLine(0, 0, 0, KinectCanvasE.ActualHeight);

                         if (FrameEdges.Bottom == (FrameEdges.Bottom & _Body.ClippedEdges))
                             CreateClippingLine(0, KinectCanvasE.ActualHeight, KinectCanvasE.ActualWidth, KinectCanvasE.ActualHeight);

                         if (FrameEdges.Right == (FrameEdges.Right & _Body.ClippedEdges))
                             CreateClippingLine(KinectCanvasE.ActualWidth, 0, KinectCanvasE.ActualWidth, KinectCanvasE.ActualHeight);
                     }
             }
         }
         private void CreateClippingLine(double X1, double Y1, double X2, double Y2)
         {
             Line _Line = new Line();
             _Line.Stroke = Brushes.Red;
             _Line.StrokeThickness = 5;
             _Line.X1 = X1;
             _Line.Y1 = Y1;
             _Line.X2 = X2;
             _Line.Y2 = Y2;

             KinectCanvasE.Children.Add(_Line);
         }

         */


        /// <summary>
        /// Closes the kinect.
        /// </summary>
        private void CloseKinect()
        {
            if (null != FrameReaderC)
            {
                FrameReaderC.FrameArrived -= ColorFrameArrived;
                FrameReaderC.Dispose();
                FrameReaderC = null;

            }
            if (null != FrameReaderP)
            {
              //  FrameReaderP.FrameArrived -= DepthFrameArrived;
                FrameReaderP.Dispose();
                FrameReaderP = null;
            }
            if (null != FrameReaderI)
            {
                FrameReaderI.FrameArrived -= InfraredFrameArrived;
                FrameReaderI.Dispose();
                FrameReaderI = null;
            }
            if (null != FrameReaderE)
            {
                //  FrameReaderE.FrameArrived -= BodyFrameArrived;
                FrameReaderE.Dispose();
                FrameReaderE = null;
            }
        

            if (null != Sensor)
            {
                this.Sensor.Close();
                Sensor = null;
            }
        }

        /// <summary>
        /// Handles the Click event of the Button3 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Button3_Click(object sender, RoutedEventArgs e)
        {
           
            
        
            
            
            
            
           // CalculateFPS("Color");
           // 
           // FCOLOR.Content=CalculateFPS()
            //_mode = Mode.Color;
            Scolor();

            ScreenImageC.Source = BitmapToDisplayC;
            FrameReaderC.FrameArrived += ColorFrameArrived;
           // CalculateFPS();
        }

        /// <summary>
        /// Handles the Click event of the Button1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            //CalculateFPS();
            
            _mode = Mode.Depth;
            Sprofundidad();
            //ScreenImageP.Source = BitmapToDisplayP;
           // FrameReaderP.FrameArrived += DepthFrameArrived;


        }

        /// <summary>
        /// Handles the Click event of the Button2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            Sinfra();
            ScreenImageI.Source = BitmapToDisplayI;
            FrameReaderI.FrameArrived += InfraredFrameArrived;
        }

        /// <summary>
        /// Handles the Click event of the Button2_C control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Button2_C_Click(object sender, RoutedEventArgs e)
        {

            Sesqueleto();
            //   FrameReaderE.FrameArrived += BodyFrameArrived;
            
        }

        /// <summary>
        /// Handles the Click event of the Button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"D:\PROYECTO V4\x64\Debug/Programa.exe");
            
            //grilla.ItemsSource = Partes.LoadCollectionData();
           
               // grilla.Items.Add("Testing1");
            
           // Form2 ver = new Form2();
          //  ver.Show();

            //CloseKinect();
           //rilla.Items.Add("Testing1");

           // grilla.ItemsSource = LoadCollectionData();
            //lista.ItemsSource = LoadCollectionData();
           
          
           
        }

        /// <summary>
        /// Terminates this instance.
        /// </summary>
        private void Terminate()
        {
            //SendKeys.SendWait("Q");
        }
        /// <summary>
        /// Cleanups this instance.
        /// </summary>
        private void Cleanup()
        {
            Console.WriteLine( Environment.NewLine + "Stopping capture" );
            Close();
            
        }
        /*
        private void Close()
        {
           // _timer.Change( Timeout.Infinite, Timeout.Infinite );

            if ( _kinectSource != null )
            {
                _kinectSource.Close();
                _kinectSource = null;
            }

            if ( _bodyFrameDumper != null )
            {
             //   _bodyFrameDumper.Close();
                _bodyFrameDumper = null;
            }

           
        }
         * */

        /// <summary>
        /// Runs this instance.
        /// </summary>
        public void Run()
        {


            try
            {
                _kinectSource = new Datos();
                _kinectSource.FrameSync = Synchronize;
                _kinectSource.FrameProcessExceptionEvent += e =>
                {
                    Console.Error.WriteLine("Error: " + e.Message);
                    Terminate();
                };
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error initializing Kinect: " + e.Message);
                Cleanup();
                return;
            }
            // initialize dumpers
            try
            {
                _bodyFrameDumper = new MainWindow(_kinectSource, BaseOutputFile + BodyDataOutputFileSuffix);
               
                if (DumpVideo)
                {
                    // _colorFrameDumper = new ColorFrameDumper( _kinectSource, BaseOutputFile + ColorDataOutputFileSuffix );
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error preparing dumpers: " + e.Message);
                Cleanup();
                return;
            }

            Console.WriteLine("Starting capture");
            Console.WriteLine("Ouput skeleton data in file {BaseOutputFile + BodyDataOutputFileSuffix}");
            
            if (DumpVideo)
            {
                Console.WriteLine("Video stream @{_kinectSource.ColorFrameDescription.Width}x{_kinectSource.ColorFrameDescription.Height} outputed in file {BaseOutputFile + ColorDataOutputFileSuffix}");
            }
            Console.WriteLine("Press X, Q or Control + C to stop capture");
            Console.WriteLine();

            Console.WriteLine("Capture rate(s):");
            // write status in console every seconds
            _timer = new Timer(o =>
            {
                //Console.Write("{_bodyFrameDumper.BodyCount} Skeleton(s) @ { _kinectSource.BodySourceFps:F1} Fps");
                if (DumpVideo)
                {
                    Console.Write(" - Color Frames @ { _kinectSource.ColorSourceFps:F1} Fps");
                }
                Console.Write("\r");
            }, null, 1000, 1000);

            // start capture
            _kinectSource.Start();

            // wait for X, Q or Ctrl+C events to exit
             Console.CancelKeyPress += (sender, args) => Cleanup();
           /*
            
             while (true)
            {
                // Start a console read operation. Do not display the input.
                var cki = Console.ReadKey(true);

                // Exit if the user pressed the 'X', 'Q' or ControlC key. 
                if (cki.Key == ConsoleKey.X || cki.Key == ConsoleKey.Q)
                {
                    break;
                }
            }
            * */
           // Cleanup();

        }






        /// <summary>
        /// Handles the 1 event of the Button_Click control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string appPath = AppDomain.CurrentDomain.BaseDirectory;
            string doss = "x64rg\\Debug\\AccesoParticipantes.exe";
            string nueva = appPath + doss.Trim();
            System.Diagnostics.Process.Start(nueva);

            //Process.Start("C:/Users/varga_000/Desktop/KINECT PROYECTO FISEI/PROYECTO V3/bin/Debug/Evaluacion2.exe");
            // var main = new MainWindow();
            // main.Run();
            TXTUNO.Background = Brushes.White;
            TXTDOS.Background = Brushes.White;
            TXTRES.Background = Brushes.White;
            TXTCUATRO.Background = Brushes.White;
            TXTCINCO.Background = Brushes.White;
            TXTSEIS.Background = Brushes.White;
            TXTSIETE.Background = Brushes.White;
            TXTOCHO.Background = Brushes.White;
            TXTNUEVE.Background = Brushes.White;
           
            //Run();
        }

        /// <summary>
        /// Handles the Checked event of the CheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
           // grilla.ItemsSource = Partes.LoadCollectionData();
        }

        /// <summary>
        /// Handles the Click event of the chcsensor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void chcsensor_Click(object sender, RoutedEventArgs e)
        {
            if (chcsensor.IsChecked == true)
            {

                
                
                  
                chcsensor.Content = "Sensor ON";
                GetSensor();
                btncolor.IsEnabled = true;
                btnprofundidad.IsEnabled = true;
                btninfra.IsEnabled = true;
                btnesqueleto.IsEnabled = true;
              // FCOLOR.Content = "";

            }
            if (chcsensor.IsChecked == false)
            {
                chcsensor.Content = "Sensor OFF";
                btncolor.IsEnabled = false;
                btnprofundidad.IsEnabled = false;
                btninfra.IsEnabled = false;
                btnesqueleto.IsEnabled = false;
                CloseKinect();
              //  Closing += CloseKinect;
                KeyDown += CheckForExit;
               // FCOLOR.Content = "";
            }
        }

        /// <summary>
        /// Handles the Closing event of the Window control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CancelEventArgs" /> instance containing the event data.</param>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            
        
            
            CloseKinect();
            try
            {
                StreamWriter sw = new StreamWriter(@"FpsCOLOR.txt");
                sw.WriteLine(
                  "FRAMES F RelativeTime.TotalSeconds Millisecond Kind Second Ticks");
                foreach (object lista in lblllenar.Items)
                {
          
                    sw.WriteLine(lista.ToString() + " " + (double)DateTime.Now.Millisecond + " " + (double)DateTime.Now.Kind + " " + (double)DateTime.Now.Second + " " + (double)DateTime.Now.Ticks);
                }
                sw.Close();
                
                StreamWriter sw2 = new StreamWriter(@"FpsPRO.txt");
                sw2.WriteLine(
                    "FRAMES F RelativeTime.TotalSeconds Millisecond Kind Second Ticks");
                foreach (object lista in lbllenar2.Items)
                {
                    sw2.WriteLine(lista.ToString() + " " + (double)DateTime.Now.Millisecond + " " + (double)DateTime.Now.Kind + " " + (double)DateTime.Now.Second + " " + (double)DateTime.Now.Ticks);
                }
                sw2.Close();
                StreamWriter sw3 = new StreamWriter(@"FpsINFRA.txt");
                sw3.WriteLine(
                     "FRAMES F RelativeTime.TotalSeconds Millisecond Kind Second Ticks");
                foreach (object lista in lbllenar3.Items)
                {
                    sw3.WriteLine(lista.ToString() + " " + (double)DateTime.Now.Millisecond + " " + (double)DateTime.Now.Kind + " " + (double)DateTime.Now.Second + " " + (double)DateTime.Now.Ticks);
                }
                sw3.Close();
                StreamWriter sw4 = new StreamWriter(@"FpsESQUE.txt");
                sw4.WriteLine(
                 "FRAMES F RelativeTime.TotalSeconds Millisecond Kind Second Ticks");
                foreach (object lista in lbllenar4.Items)
                {
                    sw4.WriteLine(lista.ToString() + " " + (double)DateTime.Now.Millisecond + " " + (double)DateTime.Now.Kind + " " + (double)DateTime.Now.Second + " " + (double)DateTime.Now.Ticks);
                }
                sw4.Close();
            }
            catch (Exception)
            {
                
               // throw;
            }
            
        }

        /// <summary>
        /// Handles the SelectionChanged event of the grilla control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs" /> instance containing the event data.</param>
        private void grilla_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        /// <summary>
        /// The partes
        /// </summary>
        public static string partes;
        /// <summary>
        /// The tipoparte
        /// </summary>
        public static int tipoparte;
        /// <summary>
        /// The tiempo
        /// </summary>
        public static double tiempo;


        /// <summary>
        /// The px
        /// </summary>
        public static double px;
        /// <summary>
        /// The py
        /// </summary>
        public static double py;
        /// <summary>
        /// The pz
        /// </summary>
        public static double pz;
        /// <summary>
        /// The ox
        /// </summary>
        public static double ox;
        /// <summary>
        /// The oy
        /// </summary>
        public static double oy;
        /// <summary>
        /// The oz
        /// </summary>
        public static double oz;
        /// <summary>
        /// The ow
        /// </summary>
        public static double ow;
        /// <summary>
        /// The stad
        /// </summary>
        public static int stad;
        /*

                public class DatosSalida
                {
                    public double Tiempo { get; set; }

                    public int TipoParte { get; set; }
                    public double px { get; set; }
                    public double py { get; set; }
                    public double pz { get; set; }
                    public double ox { get; set; }
                    public double oy { get; set; }
                    public double oz { get; set; }
                    public double ow { get; set; }
                    public int stado { get; set; }
                    public string NombreParte { get; set; }
                    //  public DateTime DOB { get; set; }
                    //  public string BookTitle { get; set; }
                    //public bool IsMVP { get; set; }
                }
                public static List<DatosSalida> LoadCollectionData()
                {
                    List<DatosSalida> llenar = new List<DatosSalida>();
                    llenar.Add(new DatosSalida()
                    {
                        Tiempo = tiempo,
                        TipoParte = tipoparte,
                        px = px,
                        py = py,
                        pz = pz,
                        ox = ox,
                        oy = oy,
                        oz = oz,
                        ow = ow,
                        stado = stad,
                        NombreParte = partes,

                    });
                    llenar.Add(new DatosSalida()
                    {
                        //  Tiempo = 102,
                        //  NombreParte = Partes.partes,

                    });

                    return llenar;
                }
        */




        /// <summary>
        /// Struct Vector3
        /// </summary>
        public struct Vector3
        {
            /// <summary>
            /// Gets or sets the x.
            /// </summary>
            /// <value>The x.</value>
            public double X { get; set; }
            /// <summary>
            /// Gets or sets the y.
            /// </summary>
            /// <value>The y.</value>
            public double Y { get; set; }
            /// <summary>
            /// Gets or sets the z.
            /// </summary>
            /// <value>The z.</value>
            public double Z { get; set; }
            /// <summary>
            /// Gets the zero.
            /// </summary>
            /// <value>The zero.</value>
            public static Vector3 Zero
            {
                get
                {
                    return new Vector3() { X = 0, Y = 0, Z = 0 };
                }
            }
        }
        /// <summary>
        /// Quaternions to euler.
        /// </summary>
        /// <param name="q">The q.</param>
        /// <returns>Vector3.</returns>
        public static Vector3 QuaternionToEuler(Vector4 q)
        {
            Vector3 v = Vector3.Zero;
            v.X = Math.Atan2(2 * q.Y * q.W - 2 * q.X * q.Z,
                                    1 - 2 * Math.Pow(q.Y, 2) - 2 * Math.Pow(q.Z, 2));

            v.Z = Math.Asin(2 * q.X * q.Y + 2 * q.Z * q.W);

            v.Y = Math.Atan2(2 * q.X * q.W - 2 * q.Y * q.Z,
                                      1 - 2 * Math.Pow(q.X, 2) - 2 * Math.Pow(q.Z, 2));

            if (q.X * q.Y + q.Z * q.W == 0.5)
            {
                v.X = (2 * Math.Atan2(q.X, q.W));
                v.Y = 0;
            }
            else if (q.X * q.Y + q.Z * q.W == -0.5)
            {
                v.X = (-2 * Math.Atan2(q.X, q.W));
                v.Y = 0;
            }

            v.X = RadianToDegree(v.X);
            v.Y = RadianToDegree(v.Y);
            v.Z = RadianToDegree(v.Z);
            return v;
        }
        /// <summary>
        /// Radians to degree.
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <returns>System.Double.</returns>
        private static double RadianToDegree(double angle)
        {//Return degrees (0->360) from radians
            return angle * (180.0 / Math.PI) + 180;
        }




        /// <summary>
        /// Angles the between points.
        /// </summary>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <returns>System.Double.</returns>
        public static double AngleBetweenPoints(Point p1, Point p2)
        {
            double retval;
            double xDiff = p1.X - p2.X;
            double yDiff = p1.Y - p2.Y;
            retval = (double)Math.Atan2(yDiff, xDiff) * (double)(180 / Math.PI);
            return retval;
        }

        /// <summary>
        /// Creates the end point.
        /// </summary>
        /// <param name="startP">The start p.</param>
        /// <param name="vec">The vec.</param>
        /// <returns>CameraSpacePoint.</returns>
        private CameraSpacePoint CreateEndPoint(CameraSpacePoint startP, float[] vec)
        {
            CameraSpacePoint point = new CameraSpacePoint();
            point.X = startP.X + vec[0];
            point.Y = startP.Y + vec[1];
            point.Z = startP.Z + vec[2];
            return point;
        }
        /// <summary>
        /// Gets the parent joint.
        /// </summary>
        /// <param name="joint">The joint.</param>
        /// <returns>JointType.</returns>
        public static JointType GetParentJoint(JointType joint)
        {
            switch (joint)
            {
                case JointType.SpineBase:
                    return JointType.SpineBase;

                case JointType.Neck:
                    return JointType.SpineShoulder;

                case JointType.SpineShoulder:
                    return JointType.SpineBase;

                case JointType.ShoulderLeft:
                case JointType.ShoulderRight:
                    return JointType.SpineShoulder;

                case JointType.HipLeft:
                case JointType.HipRight:
                    return JointType.SpineBase;

                case JointType.HandTipLeft:
                case JointType.ThumbLeft:
                    return JointType.HandLeft;

                case JointType.HandTipRight:
                case JointType.ThumbRight:
                    return JointType.HandRight;
            }

            return (JointType)((int)joint - 1);
        }
        /// <summary>
        /// Class Quaternion.
        /// </summary>
        public class Quaternion
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Quaternion" /> class.
            /// </summary>
            /// <param name="w_">The w.</param>
            /// <param name="xi">The xi.</param>
            /// <param name="yj">The yj.</param>
            /// <param name="zk">The zk.</param>
            public Quaternion(float w_, float xi, float yj, float zk)
            {
                w = w_;
                x = xi;
                y = yj;
                z = zk;
            }
            /// <summary>
            /// The w
            /// </summary>
            private float w;
            /// <summary>
            /// Gets or sets the w.
            /// </summary>
            /// <value>The w.</value>
            public float W
            {
                set
                { w = value; }
                get
                { return w; }
            }
            /// <summary>
            /// The x
            /// </summary>
            private float x;
            /// <summary>
            /// Gets or sets the x.
            /// </summary>
            /// <value>The x.</value>
            public float X
            {
                set { x = value; }
                get { return x; }
            }
            /// <summary>
            /// The y
            /// </summary>
            private float y;
            /// <summary>
            /// Gets or sets the y.
            /// </summary>
            /// <value>The y.</value>
            public float Y
            {
                set { y = value; }
                get { return y; }
            }
            /// <summary>
            /// The z
            /// </summary>
            private float z;
            /// <summary>
            /// Gets or sets the z.
            /// </summary>
            /// <value>The z.</value>
            public float Z
            {
                set { z = value; }
                get { return z; }
            }
            /// <summary>
            /// Euclidean norm
            /// </summary>
            /// <value>The norm.</value>
            public float Norm
            {
                get { return (float)Math.Sqrt(w * w + x * x + y * y + z * z); }
            }
            /// <summary>
            /// Conjugate
            /// </summary>
            /// <value>The conj.</value>
            public Quaternion Conj
            {
                get { return new Quaternion(w, -x, -y, -z); }
            }
            /// <summary>
            /// Implements the + operator.
            /// </summary>
            /// <param name="q1">The q1.</param>
            /// <param name="q2">The q2.</param>
            /// <returns>The result of the operator.</returns>
            public static Quaternion operator +(Quaternion q1, Quaternion q2)
            {
                return new Quaternion(q1.W + q2.W, q1.X + q2.X, q1.Y + q2.Y, q1.Z + q2.Z);
            }
            /// <summary>
            /// Implements the - operator.
            /// </summary>
            /// <param name="q1">The q1.</param>
            /// <param name="q2">The q2.</param>
            /// <returns>The result of the operator.</returns>
            public static Quaternion operator -(Quaternion q1, Quaternion q2)
            {
                return new Quaternion(q1.W - q2.W, q1.X - q2.X, q1.Y - q2.Y, q1.Z - q2.Z);
            }
            /// <summary>
            /// Implements the * operator.
            /// </summary>
            /// <param name="q1">The q1.</param>
            /// <param name="q2">The q2.</param>
            /// <returns>The result of the operator.</returns>
            public static Quaternion operator *(Quaternion q1, Quaternion q2)
            {
                return new Quaternion(q1.w * q2.w - q1.x * q2.x - q1.y * q2.y - q1.z * q2.z
                    , q1.w * q2.x + q1.x * q2.w + q1.y * q2.z - q1.z * q2.y
                    , q1.w * q2.y + q1.y * q2.w + q1.z * q2.x - q1.x * q2.z
                    , q1.w * q2.z + q1.z * q2.w + q1.x * q2.y - q1.y * q2.x);
            }
            /// <summary>
            /// Implements the * operator.
            /// </summary>
            /// <param name="f">The f.</param>
            /// <param name="q">The q.</param>
            /// <returns>The result of the operator.</returns>
            public static Quaternion operator *(float f, Quaternion q)
            {
                return new Quaternion(f * q.w, f * q.x, f * q.y, f * q.z);
            }
            /// <summary>
            /// Implements the * operator.
            /// </summary>
            /// <param name="q">The q.</param>
            /// <param name="f">The f.</param>
            /// <returns>The result of the operator.</returns>
            public static Quaternion operator *(Quaternion q, float f)
            {
                return new Quaternion(f * q.w, f * q.x, f * q.y, f * q.z);
            }
            /// <summary>
            /// Implements the / operator.
            /// </summary>
            /// <param name="q">The q.</param>
            /// <param name="f">The f.</param>
            /// <returns>The result of the operator.</returns>
            /// <exception cref="System.DivideByZeroException"></exception>
            public static Quaternion operator /(Quaternion q, float f)
            {
                if (f == 0.0f) { throw new DivideByZeroException(); }
                return new Quaternion(1 / f * q.w, 1 / f * q.x, 1 / f * q.y, 1 / f * q.z);
            }
            /// <summary>
            /// Implements the / operator.
            /// </summary>
            /// <param name="f">The f.</param>
            /// <param name="q">The q.</param>
            /// <returns>The result of the operator.</returns>
            /// <exception cref="System.DivideByZeroException"></exception>
            public static Quaternion operator /(float f, Quaternion q)
            {
                if (q.Norm == 0.0f) { throw new DivideByZeroException(); }
                return f / (q.Norm * q.Norm) * q.Conj;
            }
            /// <summary>
            /// Implements the / operator.
            /// </summary>
            /// <param name="q1">The q1.</param>
            /// <param name="q2">The q2.</param>
            /// <returns>The result of the operator.</returns>
            public static Quaternion operator /(Quaternion q1, Quaternion q2)
            {
                return q1 * q2.Conj / (q2.Norm * q2.Norm);
            }
            /// <summary>
            /// Implements the == operator.
            /// </summary>
            /// <param name="q1">The q1.</param>
            /// <param name="q2">The q2.</param>
            /// <returns>The result of the operator.</returns>
            public static bool operator ==(Quaternion q1, Quaternion q2)
            {
                if (Math.Abs(q1.w - q2.w) < 0.00001f
                    && Math.Abs(q1.x - q2.x) < 0.00001f
                    && Math.Abs(q1.y - q2.y) < 0.00001f
                    && Math.Abs(q1.z - q2.z) < 0.00001f)
                {
                    return true;
                }
                return false;
            }
            /// <summary>
            /// Implements the != operator.
            /// </summary>
            /// <param name="q1">The q1.</param>
            /// <param name="q2">The q2.</param>
            /// <returns>The result of the operator.</returns>
            public static bool operator !=(Quaternion q1, Quaternion q2)
            {
                if (q1.w == q2.w && q1.x == q2.x && q1.y == q2.y && q1.z == q2.z)
                {
                    return false;
                }
                return true;
            }
            /// <summary>
            /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
            /// </summary>
            /// <param name="obj">The object to compare with the current object.</param>
            /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
            public override bool Equals(object obj)
            {
                Quaternion q = obj as Quaternion;
                return q == this;
            }
            /// <summary>
            /// Returns a hash code for this instance.
            /// </summary>
            /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
            public override int GetHashCode()
            {
                return this.w.GetHashCode() ^ (this.x.GetHashCode() * this.y.GetHashCode() * this.z.GetHashCode());
            }
            /// <summary>
            /// Rotates the specified x1.
            /// </summary>
            /// <param name="x1">The x1.</param>
            /// <param name="y1">The y1.</param>
            /// <param name="z1">The z1.</param>
            /// <returns>System.Single[].</returns>
            public float[] Rotate(float x1, float y1, float z1)
            {
                Quaternion q = new Quaternion(0.0f, x1, y1, z1);
                Quaternion r = this * q * this.Conj;
                return new float[3] { r.X, r.Y, r.Z };
            }
        }

        //double angle;




        /// <summary>
        /// Outputs the body.
        /// </summary>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="body">The body.</param>
        private void OutputBody(TimeSpan timestamp, Body body)
        {

           

            var joints = body.Joints;
            var orientations = body.JointOrientations;
           
            try
            {

                foreach (var jointType in joints.Keys)
                {
                    var tipo = joints[jointType];
                    var position = joints[jointType].Position;
                    var orientation = orientations[jointType].Orientation;
                    tiempo= timestamp.TotalSeconds;
                    
                    tipoparte = (int)jointType;
                    px = position.X;
                    py = position.Y;
                    pz = position.Z;
                    ox = orientation.X;
                    oy = orientation.Y;
                    oz = orientation.Z;
                    ow = orientation.W;
                    stad = (int)joints[jointType].TrackingState;
                    partes = tipo.JointType.ToString();

                    int valore = BodyCount;
                    //angle.Update(body.Joints[_start], body.Joints[_center], body.Joints[_end], 100);
                   // string cer = ((int)angle.Angle).ToString();


                    if (stad == 2 && valore == 1)
                    {
                        //radians = Math.Atan2(py, px);
                        //angle = radians * (180 / Math.PI);
                        veaaaa(tiempo, valore, px, py, pz, ox, oy, oz, ow, stad, partes);

                    }


                    
                       // lista.Items.Add("ver");
                     
                 //  _bodyOutputStream
                    /*
                    _bodyOutputStream.WriteLine(string.Format(CultureInfo.InvariantCulture.NumberFormat,
                        "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}",
                        timestamp.TotalSeconds,
                        (int)jointType,
                        position.X, position.Y, position.Z,
                        orientation.X, orientation.Y, orientation.Z, orientation.W,
                        (int)joints[jointType].TrackingState, tipo.JointType.ToString()));
                    */

                }
            }
            catch (Exception)
            {

               // MessageBox.Show(e.ToString());
            }

            // see https://msdn.microsoft.com/en-us/library/microsoft.kinect.jointtype.aspx for jointType Description


        }
        /// <summary>
        /// The body output stream
        /// </summary>
        private StreamWriter _bodyOutputStream;
        /// <summary>
        /// The bodies
        /// </summary>
        private Body[] _bodies;

        /// <summary>
        /// Number of currently tracked bodies.
        /// </summary>
        /// <value>The body count.</value>
        public int BodyCount { get; private set; }

        /// <summary>
        /// The initial time
        /// </summary>
        public TimeSpan InitialTime;

        /// <summary>
        /// Handles the body frame.
        /// </summary>
        /// <param name="frame">The frame.</param>
        /// <exception cref="System.InvalidOperationException">BodyFrameDumper is closed.</exception>
        public void HandleBodyFrame(BodyFrame frame)
        {
    
            if (_bodyOutputStream == null)
            {
                throw new InvalidOperationException("BodyFrameDumper is closed.");
            }
            var time = frame.RelativeTime;

            // lazy body buffer initialization
            if (_bodies == null)
            {
                _bodies = new Body[frame.BodyCount];
            }

         
            frame.GetAndRefreshBodyData(_bodies);

            // 
            BodyCount = 0;
            Body firstBody = null;
            
            foreach (var body in _bodies.Where(body => body.IsTracked))
            {
                BodyCount++;
                if (firstBody == null)
                {
                    firstBody = body;
                }
            }

            // vaciar first tracked body
            if (BodyCount > 0)
            {
                try
                {
                   
                    OutputBody(time - InitialTime, firstBody);
                    
                   // Grid.Items.Add("Testing1");
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine("Error writing to output file(s): " + e.Message);
                    Close();
                    //throw;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow" /> class.
        /// </summary>
        /// <param name="kinectSource">The kinect source.</param>
        /// <param name="outputFileName">Name of the output file.</param>
        public MainWindow(Datos kinectSource, string outputFileName)
        {
            // open file for output

          
            try
            {
                
                _bodyOutputStream = new StreamWriter( new BufferedStream( new FileStream( outputFileName, FileMode.Create ) ) );
              //  Method();
                // write header


                _bodyOutputStream.WriteLine(
                    "# timestamp, jointType, position.X, position.Y, position.Z, orientation.X, orientation.Y, orientation.Z, orientation.W, state" );
               // lista.Items.Add("bbb");
            }
            catch ( Exception e )
            {
                Console.Error.WriteLine( "Error opening output file: " + e.Message );
                Close();
                //throw;
            }
           
            kinectSource.BodyFrameEvent += HandleBodyFrame;
            kinectSource.FirstFrameRelativeTimeEvent += ts => InitialTime = ts;
           
        }



        /// <summary>
        /// Handles the Loaded event of the Window control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            
        }
        /// <summary>
        /// The t
        /// </summary>
        int[] t = new int[1000];
        /// <summary>
        /// The index
        /// </summary>
        int index = 0;

        /// <summary>
        /// Handles the DataContextChanged event of the FCOLOR control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private void FCOLOR_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {



            

        }

        /// <summary>
        /// Handles the MouseDoubleClick event of the FCOLOR control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs" /> instance containing the event data.</param>
        private void FCOLOR_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
        

          
        }

        /// <summary>
        /// Handles the SourceUpdated event of the FCOLOR control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Data.DataTransferEventArgs" /> instance containing the event data.</param>
        private void FCOLOR_SourceUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
          
        }

        /// <summary>
        /// Handles the TextInput event of the FCOLOR control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextCompositionEventArgs" /> instance containing the event data.</param>
        private void FCOLOR_TextInput(object sender, TextCompositionEventArgs e)
        {
            /*
            arreglo[index] = Convert.ToDouble(FCOLOR.Content);

            //MessageBox.Show(arreglo[index].ToString());
            lblllenar.Items.Add(arreglo[index].ToString());

            index++;
             * */
        }
        /// <summary>
        /// The valor
        /// </summary>
        int valor = 0;
        /// <summary>
        /// The arreglo
        /// </summary>
        double[] arreglo = new double[1000];
        /// <summary>
        /// Handles the TextChanged event of the txtcolor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs" /> instance containing the event data.</param>
        private void txtcolor_TextChanged(object sender, TextChangedEventArgs e)
        {
            
          
           
            try
            {
                if (index!=300)
                {
                    arreglo[index] = Convert.ToDouble(txtcolor.Text);
                    valor = index + 1;
                    lblllenar.Items.Add(arreglo[index].ToString() + " " + valor + " " + FCOLOR.Content);

                    index++;
                }
                else
                {
                    Close();
                }
                
            }
            catch (Exception)
            {
                
               // throw;
            }
            

        }
        /// <summary>
        /// The valor2
        /// </summary>
        int valor2 = 0;
        /// <summary>
        /// The index2
        /// </summary>
        int index2 = 0;
        /// <summary>
        /// The arreglo2
        /// </summary>
        double[] arreglo2 = new double[1000];
        /// <summary>
        /// Handles the TextChanged event of the txtprofundidad control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs" /> instance containing the event data.</param>
        private void txtprofundidad_TextChanged(object sender, TextChangedEventArgs e)
        {

            try
            {

                arreglo2[index2] = Convert.ToDouble(txtprofundidad.Text);
                valor2 = index2 + 1;

                lbllenar2.Items.Add(arreglo2[index2].ToString() + " " + valor2 + " " + FPRO.Content);
                
                index2++;
            }
            catch (Exception)
            {

                // throw;
            }
        }
        /// <summary>
        /// The valor3
        /// </summary>
        int valor3 = 0;
        /// <summary>
        /// The index3
        /// </summary>
        int index3 = 0;
        /// <summary>
        /// The arreglo3
        /// </summary>
        double[] arreglo3 = new double[1000];
        /// <summary>
        /// Handles the TextChanged event of the txtinfra control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs" /> instance containing the event data.</param>
        private void txtinfra_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                arreglo3[index3] = Convert.ToDouble(txtinfra.Text);
                valor3 = index3 + 1;

                lbllenar3.Items.Add(arreglo3[index3].ToString() + " " + valor3 + " " + FINFRA.Content);
                
                index3++;
            }
            catch (Exception)
            {

                // throw;
            }
        }
        /// <summary>
        /// The valor4
        /// </summary>
        int valor4 =0;
        /// <summary>
        /// The index4
        /// </summary>
        int index4 = 0;
        /// <summary>
        /// The arreglo4
        /// </summary>
        double[] arreglo4 = new double[1000];
        /// <summary>
        /// Handles the TextChanged event of the txtesqueleto control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs" /> instance containing the event data.</param>
        private void txtesqueleto_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                arreglo4[index4] = Convert.ToDouble(txtesqueleto.Text);
                valor4 = index4 + 1;

                lbllenar4.Items.Add(arreglo4[index4].ToString() + " " + valor4 + " " + FESQUE.Content);
                
                index4++;
            }
            catch (Exception)
            {

                // throw;
            }
        }

        /// <summary>
        /// Handles the 2 event of the Button_Click control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            foreach (var process in Process.GetProcessesByName("OBTENCIONDATOS.exe"))
            {
                process.Kill();
            }
            foreach (var process in Process.GetProcessesByName("Evaluacion.exe"))
            {
                process.Kill();
            }
            foreach (var process in Process.GetProcessesByName("EvaluacionErgonomica.exe"))
            {
                process.Kill();
            }
            //Close();
            System.Windows.Application.Current.Shutdown();
           // this.Exit();
        }

        /// <summary>
        /// Handles the 3 event of the button_Click control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void button_Click_3(object sender, RoutedEventArgs e)
        {
            Console.Beep();

            FormularioReporteA OP = new FormularioReporteA();
            OP.Show();
        }

        /// <summary>
        /// Handles the 4 event of the Button_Click control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Console.Beep();

            FormularioReporteB OP = new FormularioReporteB();
            OP.Show();
        }

        private void button_Click_5(object sender, RoutedEventArgs e)
        {
            Console.Beep();

            FormularioReporteA OP = new FormularioReporteA();
            OP.Show();
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            Console.Beep();

            FormularioReporteB OP = new FormularioReporteB();
            OP.Show();
        }
    }
}
