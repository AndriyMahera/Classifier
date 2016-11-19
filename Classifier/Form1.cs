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
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Statistics.Kernels;
using Accord.MachineLearning.VectorMachines;

namespace Classifier
{
    public partial class Form1 : Form
    {
        private FolderBrowserDialog FBD;
        private Bitmap bitmap;
        private HistogramsOfOrientedGradients hog;
        private double[] line, resultLine;
        private byte[] byteArray;
        private HumanModel humanModel;
        private Human human;
        private double[][] trainArray;

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


        private void trainHumansToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            List<double> trainLine;
            humanModel = new HumanModel();
            trainArray = new double[humanModel.Length][];
            outputArray = new double[humanModel.Length];
            var allHumans = humanModel.GetAll();

            //саме навчання
            if (true)
            {
                
                for (int i = 0; i < humanModel.Length; i++)
                {
                    //double[] item = AuxiliaryFunctions.NormalizeHistogram(AuxiliaryFunctions.ByteArrayToDouble(allHumans[i].HOG));
                    trainArray[i] = AuxiliaryFunctions.ByteArrayToDouble(allHumans[i].HOG);
                    outputArray[i] = allHumans[i].IsHuman;
                }
                var teacher = new SequentialMinimalOptimization<Gaussian>()
                {
                    UseComplexityHeuristic = true,
                    UseKernelEstimation = true // Estimate the kernel from the data
                };
                SupportVectorMachine<Gaussian> svm = teacher.Learn(trainArray, outputArray);
                //LogisticGradient lg = new LogisticGradient(trainArray[0].Count()-1);
                //resultLine = lg.Train(trainArray, 1000, 0.1);

                resultLine = svm.Weights;

                AuxiliaryFunctions.WriteWeight(resultLine, "weight.txt");
                AuxiliaryFunctions.MakeSerialization(svm,"SVM.xml");
            }
            //розпізнаю з БД
            else
            {
                double[] checkArray = new double[humanModel.Length];
                double[] weight = AuxiliaryFunctions.ReadWeight("weight.txt");
                for (int i = 0; i < checkArray.Length; i++)
                {
                    double[] data = AuxiliaryFunctions.ByteArrayToDouble(allHumans[i].HOG);
                    LogisticGradient lg = new LogisticGradient(data.Length);
                    checkArray[i] = lg.ComputeOutput(data, weight);
                }
            }
        }
        private void testImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double[] weight = AuxiliaryFunctions.ReadWeight("weight.txt");
            hog = new HistogramsOfOrientedGradients();
            FBD = new FolderBrowserDialog();
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                foreach (string fileName in Directory.GetFiles(FBD.SelectedPath))
                {
                    hog.ProcessImage(new Bitmap(fileName));
                    line = AuxiliaryFunctions.ToOneLine(hog.Histograms);
                    LogisticGradient lg = new LogisticGradient(line.Length);
                    double result = lg.ComputeOutput(line, weight);
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string filename;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filename = openFileDialog1.FileName;
                ScanForm scanForm = new ScanForm(filename);
                scanForm.Show();
            }
        }

        private void clearHumanDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            humanModel = new HumanModel();
            humanModel.DeleteAll();
        }
    }
}
