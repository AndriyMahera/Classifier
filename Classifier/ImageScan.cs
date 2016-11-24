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
using System.Numerics;

namespace Classifier
{
    class ImageScan
    {
        public static SupportVectorMachine<Gaussian> SVM = AuxiliaryFunctions.MakeDeserialization("SVM.xml");
        public static List<Rectangle> rectangleList = new List<Rectangle>();
        public static List<double> percentage = new List<double>();
        public static List<Tuple<int,double, bool,Rectangle>> tuple = new List<Tuple<int,double, bool,Rectangle>>();
        public static Rectangle cropRect;

        public static int Counter = 0;

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
            tuple = tuple.OrderByDescending(x => x.Item2).ToList();
            AuxiliaryFunctions.WritePercentage(tuple.ToArray(), @"Output\percentage.txt");
            rectangleList = tuple.Take(5).Select(x => x.Item4).ToList();
            
        }

        public static void OnePassOfWindow(System.Drawing.Image src, int width, int height, int step)
        {
            
            Bitmap bmpImage = new Bitmap(src);
            cropRect = new Rectangle();
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
                        newImage.Save(@"Output\image_" + Counter + ".png");
                    }
                    Counter += 1;
                }
            }
        }
        public static bool CompareHOG(double[,][] hogHist)
        {
            double[,][] hogHistogram = hogHist;
            double[] weight = AuxiliaryFunctions.ReadWeight("weight.txt");
            double[] line = AuxiliaryFunctions.ToOneLine(hogHistogram);  
                     
            bool isHuman = SVM.Decide(line);
            double percent = SVM.Probability(line);
            if (isHuman)
            {
                tuple.Add(Tuple.Create(Counter, percent, isHuman,cropRect));
            }
            

            return isHuman;
        }
    }
}
