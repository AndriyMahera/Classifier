using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classifier
{
    class ImageFunctions
    {
        public static void ConvertImage(string pathIn, string pathOut)
        {
            int i = 0;
            foreach (string fileName in Directory.GetFiles(@pathIn))
            {
                System.Drawing.Image image = Image.FromFile(fileName);
                image = ScaleImage(image, 64, 128);
                Bitmap grayscaleImage = MakeGrayscale3((Bitmap)image);
                grayscaleImage = ContrastStretch(grayscaleImage);

                grayscaleImage.Save(@pathOut + "\\_" + i + ".png", ImageFormat.Png);
                i++;
            }
        }
        public static System.Drawing.Image ScaleImage(System.Drawing.Image image, int newWidth, int newHeight)
        {
            var newImage = new Bitmap(newWidth, newHeight);
            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }
        public static Bitmap MakeGrayscale3(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][]
               {
                     new float[] {.3f, .3f, .3f, 0, 0},
                     new float[] {.59f, .59f, .59f, 0, 0},
                     new float[] {.11f, .11f, .11f, 0, 0},
                     new float[] {0, 0, 0, 1, 0},
                     new float[] {0, 0, 0, 0, 1}
               });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }
        public static double Clamp(double val, double min, double max)
        {
            return Math.Min(Math.Max(val, min), max);
        }
        public static Bitmap ContrastStretch(Bitmap srcImage, double blackPointPercent = 0.02, double whitePointPercent = 0.01)
        {
            BitmapData srcData = srcImage.LockBits(new Rectangle(0, 0, srcImage.Width, srcImage.Height), ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);
            Bitmap destImage = new Bitmap(srcImage.Width, srcImage.Height);
            BitmapData destData = destImage.LockBits(new Rectangle(0, 0, destImage.Width, destImage.Height),
                ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            int stride = srcData.Stride;
            IntPtr srcScan0 = srcData.Scan0;
            IntPtr destScan0 = destData.Scan0;
            var freq = new int[256];

            unsafe
            {
                byte* src = (byte*)srcScan0;
                for (int y = 0; y < srcImage.Height; ++y)
                {
                    for (int x = 0; x < srcImage.Width; ++x)
                    {
                        ++freq[src[y * stride + x * 4]];
                    }
                }

                int numPixels = srcImage.Width * srcImage.Height;
                int minI = 0;
                var blackPixels = numPixels * blackPointPercent;
                int accum = 0;

                while (minI < 255)
                {
                    accum += freq[minI];
                    if (accum > blackPixels) break;
                    ++minI;
                }

                int maxI = 255;
                var whitePixels = numPixels * whitePointPercent;
                accum = 0;

                while (maxI > 0)
                {
                    accum += freq[maxI];
                    if (accum > whitePixels) break;
                    --maxI;
                }
                double spread = 255d / (maxI - minI);
                byte* dst = (byte*)destScan0;
                for (int y = 0; y < srcImage.Height; ++y)
                {
                    for (int x = 0; x < srcImage.Width; ++x)
                    {
                        int i = y * stride + x * 4;

                        byte val = (byte)Clamp(Math.Round((src[i] - minI) * spread), 0, 255);
                        dst[i] = val;
                        dst[i + 1] = val;
                        dst[i + 2] = val;
                        dst[i + 3] = 255;
                    }
                }
            }

            srcImage.UnlockBits(srcData);
            destImage.UnlockBits(destData);

            return destImage;
        }
    }
}
