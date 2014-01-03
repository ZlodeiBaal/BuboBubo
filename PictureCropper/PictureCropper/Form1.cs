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
using System.IO;

namespace PictureCropper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<string> St = new List<string>(); //Адреса всех файлов
        Image<Bgr, Byte> CurentIm; //Текущее изображение
        int currentimg = 0; //Номер текущего изображения в папке
        string fileadress; //Адрес файла с описанием
        Point MouseDownStart = new Point();//Начало выделения области
        Image<Bgr, Byte> Crop; //Вырезанное изображение
        private void Form1_Load(object sender, EventArgs e)
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            FBD.SelectedPath = Environment.CurrentDirectory;
            if (FBD.ShowDialog() == DialogResult.OK) //Откроем папку с изображениями
            {
                string folder = FBD.SelectedPath; 
                DirectoryInfo thisdir = new DirectoryInfo(folder);
                FileInfo[] FI = thisdir.GetFiles();
                for (int l = 0; l < FI.Length; l++) //Запишем все изображения из папки в лист
                    if ((FI[l].Extension == ".jpg") || (FI[l].Extension == ".jpeg") || (FI[l].Extension == ".bmp") || (FI[l].Extension == ".png"))
                    {
                        St.Add(FI[l].FullName);
                    }
                CurentIm = new Image<Emgu.CV.Structure.Bgr, byte>(St[currentimg]);
                PictureWindow.Image = CurentIm; //Вывод текущего изображения
                string s = folder + ".dat";
                if (File.Exists(s))
                    File.Delete(s);
                fileadress = s;
            }
        }
        /// <summary>
        /// Начало выделения области
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictureWindow_MouseDown(object sender, MouseEventArgs e)
        {
            MouseDownStart = new System.Drawing.Point(e.X, e.Y);
        }
        /// <summary>
        /// Окончание выделения области
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictureWindow_MouseUp(object sender, MouseEventArgs e)
        {
            Point Select = new Point(Math.Min(e.X, MouseDownStart.X), Math.Min(e.Y, MouseDownStart.Y));
            Point Size = new Point(0, 0);
            Size.X = Math.Abs(e.X - MouseDownStart.X);
            Size.Y = Math.Abs(e.Y - MouseDownStart.Y);
            double sx = ((double)CurentIm.Width / (double)PictureWindow.Width);
            double sy = ((double)CurentIm.Height / (double)PictureWindow.Height);
            Rectangle Rec = new Rectangle((int)(Select.X * sx), (int)(Select.Y * sy), (int)(Size.X * sx), (int)(Size.Y * sy));
            Image<Bgr, Byte> Sel = CurentIm.Clone();
            Sel.Draw(Rec, new Bgr(100, 100, 100), 3);
            PictureWindow.Image = Sel;
            CurentIm.ROI = Rec;
            Crop = CurentIm.Clone();
            CvInvoke.cvResetImageROI(CurentIm);
        }
        /// <summary>
        /// Реакция на кнопки управления
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ') 
            {
                NewNumber(); //Следующее изображение
            }
            if ((e.KeyChar == 's') || (e.KeyChar == 'ы'))
            {
                SaveIm(); //Сохранить область кропа
            }
            if ((e.KeyChar == 'r') || (e.KeyChar == 'к'))
            {
                SaveIm(); //Сохранить область кропа
                NewNumber();//Следующее изображение
            }
        }
        /// <summary>
        /// Взятие следующего изображения из папки
        /// </summary>
        private void NewNumber()
        {
            if (currentimg + 1 < St.Count)
            {
                currentimg += 1;
                if (File.Exists(St[currentimg]))
                {
                    CurentIm = new Image<Emgu.CV.Structure.Bgr, byte>(St[currentimg]);
                    PictureWindow.Image = CurentIm;
                }
            }
        }
        /// <summary>
        /// Сохранить изображение
        /// </summary>
        private void SaveIm()
        {
            string s = St[currentimg].Substring(0, St[currentimg].LastIndexOf("\\") + 1) + "Cropper";
            if (!Directory.Exists(s))
                System.IO.Directory.CreateDirectory(s);
            s = s + St[currentimg].Substring(St[currentimg].LastIndexOf("\\"), St[currentimg].Length - St[currentimg].LastIndexOf("\\") - 4)
                + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".bmp";
            Crop.Save(s);
            File.AppendAllText(fileadress, s + "  1  " + "0 0 " + Crop.Width + " " + Crop.Height + "\r\n");
            // File.AppendAllText(fileadress, s  + "\r\n");
        }
    }
}
