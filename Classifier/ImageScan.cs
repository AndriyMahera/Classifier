using Accord.Imaging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classifier
{
    class ImageScan
    {
        public static void ImageScanning(System.Drawing.Image image, int step)
        {
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

                    CompareHOG(hogHistogram);
                }
            }
        }

        public static void CompareHOG(object hogHist)
        {
            double[,][] hogHistogram = (double[,][])hogHist;

            //compare code

        }
    }
}
