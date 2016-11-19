using Accord.MachineLearning.VectorMachines;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Statistics.Kernels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Classifier
{
    [Serializable]
    public class SVMData
    {
        public SVMData() { }
        public int NumberOfInputs { get; set; }
        public int NumberOfOutputs { get; set; }
        public double[][] SupportVectors { get; set; }
        public double Threshold { get; set; }
        public double[] Weights { get; set; }
        public double Gamma { get; set; }
        public double Sigma { get; set; }
        public double SigmaSquared { get; set; }

    }
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
        public static double[] NormalizeHistogram(double[] array)
        {
            double max = array.Select(x => Math.Abs(x)).Max();
            return array.Select(x => x / max * 10.0).ToArray();
        }
        public static void Serialize<T>(T myObject,string path)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(T));
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                formatter.Serialize(fs, myObject);
            }
        }
        public static T Deserialize<T>(string path)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(T));
            T myObject;
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                myObject = (T)formatter.Deserialize(fs);
            }
            return myObject;
        }
        public static void MakeSerialization(SupportVectorMachine<Gaussian> svm, string path)
        {
            SVMData data = new SVMData();
            data.NumberOfInputs = svm.NumberOfInputs;
            data.NumberOfOutputs = svm.NumberOfOutputs;
            data.SupportVectors = svm.SupportVectors;
            data.Threshold = svm.Threshold;
            data.Weights = svm.Weights;
            data.Sigma = svm.Kernel.Sigma;
            data.SigmaSquared = svm.Kernel.SigmaSquared;
            data.Gamma = svm.Kernel.Gamma;
            Serialize(data, path);
        }
        public static SupportVectorMachine<Gaussian> MakeDeserialization(string path)
        {
            var teacher = new SequentialMinimalOptimization<Gaussian>()
            {
                UseComplexityHeuristic = true,
                UseKernelEstimation = true // Estimate the kernel from the data
            };
            double[][] inputs2 = new double[4][];
            inputs2[0] = new[] { 1.0, 4 };
            inputs2[1] = new[] { 6.0, 8 };
            inputs2[2] = new[] { 60.0, 78 };
            inputs2[3] = new[] { 60.0, 90 };

            double[] outputs2 = { 1, 1, 0, 0 };

            SupportVectorMachine<Gaussian> svmAfter = teacher.Learn(inputs2, outputs2);
            SVMData dataAfter = Deserialize<SVMData>(path);

            svmAfter.NumberOfInputs = dataAfter.NumberOfInputs;
            svmAfter.NumberOfOutputs = dataAfter.NumberOfOutputs;
            svmAfter.SupportVectors = dataAfter.SupportVectors;
            svmAfter.Threshold = dataAfter.Threshold;
            svmAfter.Weights = dataAfter.Weights;
            Gaussian g = new Gaussian();
            g.Gamma = dataAfter.Gamma;
            g.Sigma = dataAfter.Sigma;
            g.SigmaSquared = dataAfter.SigmaSquared;
            svmAfter.Kernel = g;

            return svmAfter;
        }
    }
}
