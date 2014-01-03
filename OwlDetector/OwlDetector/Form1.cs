using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using Emgu.CV.Features2D;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using Emgu.CV.UI;

namespace OwlDetector
{
    public partial class Form1 : Form
    {
        private static Capture _cameraCapture; //Камера
        public Form1()
        {
            InitializeComponent();
            Run(); //Запускаем камеру
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        void Run()
        {
            try
            {
                _cameraCapture = new Capture();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }
            Application.Idle += ProcessFrame;
        }
        String faceFileName = "cascade.xml"; //Каскад детектора

        /////////////////////////////////////////
        //Процедура обработки видео и поика Совы
        void ProcessFrame(object sender, EventArgs e)
        {
            Image<Bgr, Byte> frame = _cameraCapture.QueryFrame(); //Полученный кадр
            using (CascadeClassifier face = new CascadeClassifier(faceFileName)) //Каскад
            using (Image<Gray, Byte> gray = frame.Convert<Gray, Byte>()) //Хаар работает с ЧБ изображением
            {
                //Детектируем
                Rectangle[] facesDetected2 = face.DetectMultiScale(
                        gray, //Исходное изображение
                        1.1,  //Коэффициент увеличения изображения
                        6,   //Группировка предварительно обнаруженных событий. Чем их меньше, тем больше ложных тревог
                        new Size(5, 5), //Минимальный размер совы
                        Size.Empty); //Максимальный размер совы
               //Выводим всё найденное
                foreach (Rectangle f in facesDetected2)
                {
                    frame.Draw(f, new Bgr(Color.Blue), 2);
                }
            }
            VideoImage.Image = frame;

        }
    }
}
