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
        public ScanForm(string path)
        {
            InitializeComponent();
            image = Image.FromFile(@path);
            pictureBox1.Image = image;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int step = trackBar1.Value;
            
            Stopwatch stopwatch = Stopwatch.StartNew();
            ImageScan.ImageScanning(image, step);

            stopwatch.Stop();
            label1.Text = "time: "+stopwatch.ElapsedMilliseconds/1000+" sec";

            Pen myPen = new Pen(Color.Blue, 2);
            foreach (Rectangle rect in ImageScan.rectangleList)
            {
                using (Graphics myGraphics = Graphics.FromImage(image))
                    myGraphics.DrawRectangle(myPen, rect);
            }

            pictureBox1.Refresh();


            List<Rectangle> filteredRectList = Filtering.FilterData(ImageScan.rectangleList);
            Pen myPen2 = new Pen(Color.Red, 5);
            foreach (Rectangle rect in filteredRectList)
            {
                using (Graphics myGraphics = Graphics.FromImage(image))
                    myGraphics.DrawRectangle(myPen2, rect);
            }

            pictureBox1.Refresh();


        }
    }
}
