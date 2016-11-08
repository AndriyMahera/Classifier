using System;
using System.Collections.Generic;
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
                list.Add(BitConverter.ToDouble(bytes.Skip(i).Take(8).ToArray(), 0));
            }
            return list.ToArray();
        }
    }
}
