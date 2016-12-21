using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Accord.Imaging;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Statistics.Kernels;
using Accord.MachineLearning.VectorMachines;
using System.Threading;

namespace Classifier
{
    public partial class Form1 : Form
    {
        private FolderBrowserDialog FBD;
        private Bitmap bitmap;
        private HistogramsOfOrientedGradients hog;
        private double[] line, resultLine, outputArray;
        private byte[] byteArray;
        private Human human;
        private double[][] trainArray;
        private List<Human> humanList;
        private int index, fileCount, progressValue;
        private static ProgressBar pb;

        public Form1()
        {
            InitializeComponent();
            pb = progressBar1;
            FBD = new FolderBrowserDialog();     
        }

        private void makeGrayscaleAndResizingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                ImageFunctions.ConvertImage(FBD.SelectedPath, @"E:\Output");
            }
        }

        private void InitializeProgressBar()
        {
            pb.Minimum = 0;
            pb.Maximum = 100;
            index = 0;
            progressValue = 0;
            pb.Value = progressValue;
        }

        private void Scanning_Click(object sender, EventArgs e)
        {
            string filename;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filename = openFileDialog1.FileName;
                ScanForm scanForm = new ScanForm(filename);
                scanForm.Show();
            }
        }

        private void addAllImagesToDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            humanList = new List<Human>();
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                DirectoryInfo myFolder = new DirectoryInfo(FBD.SelectedPath);
                fileCount = Directory.GetFiles(FBD.SelectedPath).Count();
                InitializeProgressBar();
                foreach (string filename in Directory.GetFiles(FBD.SelectedPath))
                {
                    bitmap = new Bitmap(filename);
                    hog = new HistogramsOfOrientedGradients();
                    hog.ProcessImage(bitmap);
                    human = new Human();
                    if (filename.Contains("image_human"))
                        human.IsHuman = 1;
                    else
                        human.IsHuman = -1;
                    line = AuxiliaryFunctions.ToOneLine(hog.Histograms);
                    byteArray = AuxiliaryFunctions.DoubleArrayToByte(line);
                    human.HOG = byteArray;

                    //XML file
                    humanList.Add(human);

                    
                    index++;

                    if (index % (fileCount / 100) == 0)
                    {
                        progressValue++;
                        label1.Text = (progressValue + " %").ToString();
                        pb.Value = progressValue<=100?progressValue:100;
                        Thread.Sleep(10);
                    }
                }
                if (!File.Exists("Database.xml"))
                {
                    AuxiliaryFunctions.Serialize(humanList, "Database.xml");
                }
                else
                {
                    humanList.AddRange(AuxiliaryFunctions.Deserialize<List<Human>>("Database.xml"));
                    AuxiliaryFunctions.Serialize(humanList, "Database.xml");
                }
            }
        }
        private void trainHumansToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            //XML file
            humanList = new List<Human>();
            humanList = AuxiliaryFunctions.Deserialize<List<Human>>("Database.xml");
            trainArray = new double[humanList.Count][];
            outputArray = new double[humanList.Count];
            fileCount = humanList.Count;
            InitializeProgressBar();
            for (int i = 0; i < humanList.Count; i++)
            {
                trainArray[i] = AuxiliaryFunctions.ByteArrayToDouble(humanList[i].HOG);
                outputArray[i] = humanList[i].IsHuman;
                index++;

                if (index % (fileCount / 100) == 0)
                {
                    progressValue++;
                    label1.Text = (progressValue + " %").ToString();
                    pb.Value = progressValue <= 100 ? progressValue : 100;
                    Thread.Sleep(10);
                }
            }
            //trainArray = humanList.Select(x => AuxiliaryFunctions.ByteArrayToDouble(x.HOG)).ToArray();
            //outputArray = humanList.Select(x => (double)x.IsHuman).ToArray();

            var teacher = new SequentialMinimalOptimization<Gaussian>()
            {
                UseComplexityHeuristic = true,
                UseKernelEstimation = true
            };
            SupportVectorMachine<Gaussian> svm = teacher.Learn(trainArray, outputArray);
            resultLine = svm.Weights;
            AuxiliaryFunctions.WriteWeight(resultLine, "weight.txt");
            AuxiliaryFunctions.MakeSerialization(svm, "SVM_G.xml");
        }
        private void testImageToolStripMenuItem_Click(object sender, EventArgs e)
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
            if (File.Exists("Database.xml"))
            {
                File.Delete("Database.xml");
            }
            else
            {
                MessageBox.Show("Database is already empty");
            }
        }
    }
}
