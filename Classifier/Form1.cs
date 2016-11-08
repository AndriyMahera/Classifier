using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accord.Imaging;

namespace Classifier
{
    public partial class Form1 : Form
    {
        private FolderBrowserDialog FBD = new FolderBrowserDialog();
        private Bitmap bitmap;
        private HistogramsOfOrientedGradients hog;
        private double[] line, resultLine;
        private byte[] byteArray;
        private HumanModel humanModel;
        private Human human;

        public Form1()
        {
            
            InitializeComponent();
        }




        private void makeGrayscaleAndResizingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FBD = new FolderBrowserDialog();
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                DirectoryInfo myFolder = new DirectoryInfo(FBD.SelectedPath);
                ImageFunctions.ConvertImage(FBD.SelectedPath, @"E:\Output");

            }
        }

        private void addAllImagesToDBToolStripMenuItem_Click(object sender, EventArgs e)
        {

            FBD = new FolderBrowserDialog();
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                DirectoryInfo myFolder = new DirectoryInfo(FBD.SelectedPath);
                foreach (string filename in Directory.GetFiles(FBD.SelectedPath))
                {
                        bitmap = new Bitmap(filename);
                        hog = new HistogramsOfOrientedGradients();
                        hog.ProcessImage(bitmap);
                        human = new Human();
                        if (filename.Contains("image_human"))
                            human.IsHuman = 1;
                        else
                            human.IsHuman = 0;
                        line = AuxiliaryFunctions.ToOneLine(hog.Histograms);
                        byteArray = AuxiliaryFunctions.DoubleArrayToByte(line);
                        human.HOG = byteArray;
                        humanModel = new HumanModel();
                        humanModel.Insert(human);
                }

            }
        }

        private void clearHumanDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            humanModel = new HumanModel();
            humanModel.DeleteAll();
        }
    }
}
