using Accord.Imaging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Statistics.Kernels;
using Accord.IO;
using Accord.Math;
using Accord.MachineLearning.VectorMachines;
using Accord.MachineLearning.VectorMachines.Learning;

namespace Classifier
{
    class ImageScan
    {
        public static SupportVectorMachine<Gaussian> SVM = AuxiliaryFunctions.MakeDeserialization("SVM.xml");
        public static List<Rectangle> rectangleList = new List<Rectangle>();
        public static void ImageScanning(System.Drawing.Image image, int step)
        {
            rectangleList.Clear();
            Bitmap grayscaleImage = ImageFunctions.MakeGrayscale3((Bitmap)image);
            grayscaleImage = ImageFunctions.ContrastStretch(grayscaleImage);

            AllPassesOfWindow(grayscaleImage, step);
        }
        public static void AllPassesOfWindow(System.Drawing.Image src, int step)
        {
            int width = 64;
            int height = 128;
            
            while (width < src.Width && height < src.Height)
            {
                OnePassOfWindow(src, width, height, step);
                width=(int)Math.Round(width*1.5);
                height = (int)Math.Round(height * 1.5);
            }
        }
        public static void OnePassOfWindow(System.Drawing.Image src, int width, int height, int step)
        {
            Bitmap bmpImage = new Bitmap(src);
            Rectangle cropRect = new Rectangle();
            Bitmap newImage = new Bitmap(64, 128);

            for (int i = 0; i < src.Height - height; i+=step)
            {
                for (int j = 0; j < src.Width - width; j+=step)
                {
                    cropRect = new Rectangle(j, i, width, height);
                    newImage = bmpImage.Clone(cropRect, bmpImage.PixelFormat);
                    newImage = (Bitmap)ImageFunctions.ScaleImage(newImage, 64, 128);

                    HistogramsOfOrientedGradients hog = new HistogramsOfOrientedGradients();
                    hog.ProcessImage(newImage);
                    double[,][] hogHistogram = hog.Histograms;
                    bool t = CompareHOG(hogHistogram);
                    if (t)
                    {
                        rectangleList.Add(cropRect);
                    }
                    Console.WriteLine(t+"");

                }
            }
        }

        public static double CompareHOG(object hogHist)
        {
            double[,][] hogHistogram = (double[,][])hogHist;
            double[] weight = AuxiliaryFunctions.ReadWeight("weight.txt");

            double[] line = AuxiliaryFunctions.ToOneLine(hogHistogram);


            LogisticGradient lg = new LogisticGradient(line.Length);


            return lg.ComputeOutput(line, weight);
        }
        public static bool CompareHOG(double[,][] hogHist)
        {
            double[,][] hogHistogram = hogHist;
            double[] weight = AuxiliaryFunctions.ReadWeight("weight.txt");

            double[] line = AuxiliaryFunctions.ToOneLine(hogHistogram);

            
            bool isHuman = SVM.Decide(line);


            //дикий бидлокод і хз чи адекватно паше
            //var teacher = new SequentialMinimalOptimization<Gaussian>()
            //{
            //    UseComplexityHeuristic = true,
            //    UseKernelEstimation = true // Estimate the kernel from the data
            //};
            //double[][] inputs2 = new double[4][];
            //inputs2[0] = new[] { 1.0, 4 };
            //inputs2[1] = new[] { 6.0, 8 };
            //inputs2[2] = new[] { 60.0, 78 };
            //inputs2[3] = new[] { 60.0, 90 };

            //double[] outputs2 = { 1, 1, 0, 0 };

            //SupportVectorMachine<Gaussian> svm = teacher.Learn(inputs2, outputs2);
            //svm.Weights = weight;
            //bool isHuman = svm.Decide(line);

            return isHuman;
        }
    }
}
