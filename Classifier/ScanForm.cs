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
        }
    }
}
