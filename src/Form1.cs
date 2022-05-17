using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FindDifferent
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Mat mat;
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "图片|*.gif;*.jpg;*.jpeg;*.bmp;*.jfif;*.png;";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Load(openFileDialog.FileName);
                mat = new Mat(openFileDialog.FileName);
                Mat dst = new Mat();
                pictureBox1.Image = Image.FromStream(mat.ToMemoryStream());
            }
        }
        Mat mat2;
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "图片|*.gif;*.jpg;*.jpeg;*.bmp;*.jfif;*.png;";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox2.Load(openFileDialog.FileName);
                mat2 = new Mat(openFileDialog.FileName); 
                pictureBox2.Image = Image.FromStream(mat2.ToMemoryStream());
            }
        }
       Dictionary<int,List<Point2f>> points = new Dictionary<int, List<Point2f>>();
        private void button3_Click(object sender, EventArgs e)
        {
            Mat mat3 = new Mat(mat.Rows, mat.Cols,MatType.CV_8UC1);
            //重新对mat对象赋值，不然图像会出现莫名其妙的点
            for (int x = 0; x < mat3.Rows; x++)
            {
                for (int y = 0; y < mat3.Cols; y++)
                {
                    mat3.Set<byte>(x, y, 0);
                }
            }
            if (mat != null && mat2 != null)
            {
                if (mat.Rows == mat2.Rows && mat.Cols == mat2.Cols)
                {
                    for (int x = 0; x < mat.Rows; x++)
                    {
                        for (int y = 0; y < mat.Cols; y++)
                        {
                            var rgb = mat.At<Vec3b>(x,y);
                            var rgb2 = mat2.At<Vec3b>(x, y);
                            if (rgb.Item0 == rgb2.Item0 && rgb.Item1 == rgb2.Item1 && rgb.Item2 == rgb2.Item2)
                            {
                                //Console.WriteLine($"{x}-{y}=rgb-rgb2相同");
                            }
                            else
                            { 
                                Cv2.Line(mat3, new OpenCvSharp.Point(y, x), new OpenCvSharp.Point(y, x), new Scalar(255, 255, 255));
                                //Cv2.ImShow("image", mat3);
                                //Cv2.WaitKey(100);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("图片像素大小不一致");
                }
            } 
            OpenCvSharp.Point[][] list;
            HierarchyIndex[] h; 
            Cv2.FindContours(mat3, out list, out h, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
            Console.WriteLine(list.Length);
            foreach (var item in list)
            {
                var b = Cv2.BoundingRect(item);
                Cv2.Rectangle(mat, new Rect(b.X, b.Y, b.Width, b.Height), new Scalar(0, 0, 255),2);
                Cv2.Rectangle(mat2, new Rect(b.X, b.Y, b.Width, b.Height), new Scalar(0, 0, 255),2);
            }
            pictureBox1.Image = Image.FromStream(mat.ToMemoryStream());
            pictureBox2.Image = Image.FromStream(mat2.ToMemoryStream());
        }

        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            //Console.WriteLine($"x={e.X},y={e.Y}");
        }
    }
}
