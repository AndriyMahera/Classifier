using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classifier
{
    class AuxiliaryFunctions
    {
        public static double[] ToOneLine(double[,][] hog)
        {
            List<double> list = new List<double>();
            for (int i = 0; i < hog.GetLength(0); i++)
            {
                for (int j = 0; j < hog.GetLength(1); j++)
                {
                    list.AddRange(hog[i, j].Select(x => x));
                }
            }
            return list.ToArray();
        }
        public static byte[] DoubleArrayToByte(double[] arr)
        {
            return arr.SelectMany(BitConverter.GetBytes).ToArray();
        }
        public static double[] ByteArrayToDouble(byte[] bytes)
        {
            List<double> list = new List<double>();
            for (int i = 0; i < bytes.Length; i += 8)
            {
                list.Add(BitConverter.ToDouble(bytes.Skip(i).Take(8).ToArray(), 0)*0.01-10);
                //list.Add(BitConverter.ToDouble(bytes.Skip(i).Take(8).ToArray(), 0)/100.0);
            }
            return list.ToArray();
        }
        public static void WriteWeight(double[] array, string path)
        {
            using (StreamWriter sw = new StreamWriter(path, false))
            {
                sw.Write(String.Join(" ", array));
            }
        }
        public static double[] ReadWeight(string path)
        {
            string file;
            using (StreamReader sr = new StreamReader(path))
            {
                file = sr.ReadToEnd();
            }
            return file.Split(' ').Select(x => Convert.ToDouble(x)).ToArray();
        }

        public static double[] MakeTail(double[] array, double tail)
        {
            double[] temp = { tail };
            return Enumerable.Concat(array, temp).ToArray();
        }
    }
}
