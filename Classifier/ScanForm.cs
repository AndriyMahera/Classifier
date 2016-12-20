using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Classifier
{
    public partial class ScanForm : Form
    {
        private Image image;
        private string path="";
        public ScanForm(string path)
        {
            InitializeComponent();
            this.path = path;
            image = Image.FromFile(@path);
            pictureBox1.Image = image;
            trackBar2.Minimum = 1;
            trackBar2.Value = 1;

            //image2 = ;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int step = trackBar1.Value;
            
            Stopwatch stopwatch = Stopwatch.StartNew();
            ImageScan.ImageScanning((Bitmap)image, step);

            stopwatch.Stop();
            label1.Text = "time: "+stopwatch.ElapsedMilliseconds/1000+" sec";

            Pen myPen = new Pen(Color.Blue, 2);
            foreach (Rectangle rect in ImageScan.rectangleList)
            {
                using (Graphics myGraphics = Graphics.FromImage(image))
                    myGraphics.DrawRectangle(myPen, rect);
            }

            trackBar2.Maximum = ImageScan.rectangleList.Count;
            pictureBox1.Refresh();

        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            image = Image.FromFile(@path);
            pictureBox1.Image = image;

            Pen myPen = new Pen(Color.Blue, 2);

            var ourFuckedList = ImageScan.tuple.Select(x=>x.Item4).Take(trackBar2.Value).ToList();

            foreach (Rectangle rect in ourFuckedList)
            {
                using (Graphics myGraphics = Graphics.FromImage(image))
                    myGraphics.DrawRectangle(myPen, rect);
            }

            pictureBox1.Refresh();

        }
    }
}
