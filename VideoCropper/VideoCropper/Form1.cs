using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using Emgu.CV.Features2D;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;

namespace VideoCropper
{
    public partial class Form1 : Form
    {
        private Capture _capture;
        Point MouseDownStart = new Point(); //Точка начала выделения области
        Image<Gray, Byte> frame; //Текущий кадр
        int NumOfSavedImage = 150; //Номер текущего сохраняемого изображения
        object locker = new object(); //Контроль доступа к изображению
        string FoulderAdress; //Адрес папки куда сохранять
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                _capture = new Capture(); //Проверка доступа камеры
            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }
            Application.Idle += ProcessFrame; //Привязываем к событию камеры процедуру

            FolderBrowserDialog FBD = new FolderBrowserDialog();
            FBD.SelectedPath = Environment.CurrentDirectory;
            if (FBD.ShowDialog() == DialogResult.OK) //Запрашиваем рабочую дирректорию
            {
                FoulderAdress = FBD.SelectedPath;
                int temp = FoulderAdress.LastIndexOf("\\");
                string Adress = FoulderAdress.Substring(0, temp + 1);

                if (MessageBox.Show("Работать с положительной выборкой?", "Тип работы", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    checkBox1.Checked = true;
        //            if (File.Exists(Adress + "Good.dat"))
        //                File.Delete(Adress + "Good.dat");
                }
                else
                {
        //            if (File.Exists(Adress + "Bad.dat"))
        //                File.Delete(Adress + "Bad.dat");
                    checkBox1.Checked = false;
                }
            }

        }

        /// <summary>
        /// Обработка нового кадра
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        private void ProcessFrame(object sender, EventArgs arg)
        {
            lock (locker)
            {
                frame = _capture.QueryFrame().Convert<Gray, Byte>(); //Текущий кадр с камеры
                Image<Gray, Byte> frameforpic = frame.Clone();
                if (Rec != null)
                    frameforpic.Draw(Rec, new Gray(150), 3); //нарисуем область выделения
                VideoWindow.Image = frameforpic;
            }
        }
        Rectangle Rec; //Прямоугольник
        /// <summary>
        /// Окончание выделения области интереса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VideoWindow_MouseUp(object sender, MouseEventArgs e)
        {
            Point Select = new Point(Math.Min(e.X, MouseDownStart.X), Math.Min(e.Y, MouseDownStart.Y));
            Point Size = new Point(0, 0);
            Size.X = Math.Abs(e.X - MouseDownStart.X);
            Size.Y = Math.Abs(e.Y - MouseDownStart.Y);
            double sx = ((double)_capture.Width / (double)VideoWindow.Width);
            double sy = ((double)_capture.Height / (double)VideoWindow.Height);
            Rec = new Rectangle((int)(Select.X * sx), (int)(Select.Y * sy), (int)(Size.X * sx), (int)(Size.Y * sy));

        }
       /// <summary>
       /// Начало выделения обалсти интереса
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void VideoWindow_MouseDown(object sender, MouseEventArgs e)
        {
            MouseDownStart = new System.Drawing.Point(e.X, e.Y); 
        }
        /// <summary>
        /// Сохранение по пробелу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
            {
                 SaveIm();
            }
        }
        /// <summary>
        /// Сохраняем изображение
        /// </summary>
        private void SaveIm()
        {
            lock (locker)
            {
                frame.ROI = Rec;
                Image<Gray, Byte> Sel = frame.Clone();
                CvInvoke.cvResetImageROI(frame);
                Sel.Save(FoulderAdress + "\\" + NumOfSavedImage.ToString() + ".bmp");
                int temp = FoulderAdress.LastIndexOf("\\");
                string Adress = FoulderAdress.Substring(0, temp + 1);
                string FName = FoulderAdress.Substring(temp + 1, FoulderAdress.Length - temp - 1);
                if (checkBox1.Checked)
                    File.AppendAllText(Adress + "Good.dat", FName + "\\" + NumOfSavedImage.ToString() + ".bmp" + "  " + "1" + "  " + "0 0 " + Sel.Width.ToString() + " " + Sel.Height.ToString() + "\r\n");
                else
                    File.AppendAllText(Adress + "Bad.dat", FName + "\\" + NumOfSavedImage.ToString() + ".bmp" + "\r\n");
                NumOfSavedImage++;
            }
        }


    }
}
