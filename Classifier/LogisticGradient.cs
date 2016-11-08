using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classifier
{
    class LogisticGradient
    {
        private int numFeatures; // number of x variables aka features
        private double[] weights; // b0 = constant
        private Random rnd;

        public LogisticGradient(int numFeatures)
        {
            this.numFeatures = numFeatures;
            this.weights = new double[numFeatures + 1]; // [0] = b0 constant
            this.rnd = new Random(0);
            //for (int i = 0; i < weights.Length; ++i) // not necessary
            //  weights[i] = 0.01 * rnd.NextDouble(); // [0.00, 0.01)
        }

        public double[] Train(double[][] trainData, int maxEpochs, double alpha)
        {
            // alpha is the learning rate
            int epoch = 0;
            int[] sequence = new int[trainData.Length]; // random order
            for (int i = 0; i < sequence.Length; ++i)
                sequence[i] = i;

            while (epoch < maxEpochs)
            {
                ++epoch;

                if (epoch % 100 == 0 && epoch != maxEpochs)
                {
                    double mse = Error(trainData, weights);
                    Console.Write("epoch = " + epoch);
                    Console.WriteLine("  error = " + mse.ToString("F4"));
                }

                Shuffle(sequence); // process data in random order

                // stochastic/online/incremental approach
                for (int ti = 0; ti < trainData.Length; ++ti)
                {
                    int i = sequence[ti];
                    double computed = ComputeOutput(trainData[i], weights);
                    int targetIndex = trainData[i].Length - 1;
                    double target = trainData[i][targetIndex];

                    weights[0] += alpha * (target - computed) * 1; // the b0 weight has a dummy 1 input
                                                                   //weights[0] += alpha * (target - computed) * computed * (1 -computed) * 1; // alt. form
                    for (int j = 1; j < weights.Length; ++j)
                        weights[j] += alpha * (target - computed) * trainData[i][j - 1];
                    //weights[j] += alpha * (target - computed) * computed * (1 - computed) * trainData[i][j - 1]; // alt. form
                }

                // batch/offline approach
                //double[] accumulatedGradients = new double[weights.Length]; // one acc for each weight

                //for (int i = 0; i < trainData.Length; ++i)  // accumulate
                //{
                //  double computed = ComputeOutput(trainData[i], weights); // no need to shuffle order
                //  int targetIndex = trainData[i].Length - 1;
                //  double target = trainData[i][targetIndex];
                //  accumulatedGradients[0] += (target - computed) * 1; // for b0
                //  for (int j = 1; j < weights.Length; ++j)
                //    accumulatedGradients[j] += (target - computed) * trainData[i][j - 1];
                //}

                //for (int j = 0; j < weights.Length; ++j) // update
                //  weights[j] += alpha * accumulatedGradients[j];

            } // while
            return this.weights; // by ref is somewhat risky
        } // Train

        private void Shuffle(int[] sequence)
        {
            for (int i = 0; i < sequence.Length; ++i)
            {
                int r = rnd.Next(i, sequence.Length);
                int tmp = sequence[r];
                sequence[r] = sequence[i];
                sequence[i] = tmp;
            }
        }

        private double Error(double[][] trainData, double[] weights)
        {
            // mean squared error using supplied weights
            int yIndex = trainData[0].Length - 1; // y-value (0/1) is last column
            double sumSquaredError = 0.0;
            for (int i = 0; i < trainData.Length; ++i) // each data
            {
                double computed = ComputeOutput(trainData[i], weights);
                double desired = trainData[i][yIndex]; // ex: 0.0 or 1.0
                sumSquaredError += (computed - desired) * (computed - desired);
            }
            return sumSquaredError / trainData.Length;
        }

        public  double ComputeOutput(double[] dataItem, double[] weights)
        {
            double z = 0.0;
            z += weights[0]; // the b0 constant
            for (int i = 0; i < weights.Length - 1; ++i) // data might include Y
                z += (weights[i + 1] * dataItem[i]); // skip first weight
            return 1.0 / (1.0 + Math.Exp(-z));
        }

        private int ComputeDependent(double[] dataItem, double[] weights)
        {
            double y = ComputeOutput(dataItem, weights); // 0.0 to 1.0
            if (y <= 0.5)
                return 0;
            else
                return 1;
        }

        public double Accuracy(double[][] trainData, double[] weights)
        {
            int numCorrect = 0;
            int numWrong = 0;
            int yIndex = trainData[0].Length - 1;
            for (int i = 0; i < trainData.Length; ++i)
            {
                int computed = ComputeDependent(trainData[i], weights);
                int target = (int)trainData[i][yIndex]; // risky?

                if (computed == target)
                    ++numCorrect;
                else
                    ++numWrong;
            }
            return (numCorrect * 1.0) / (numWrong + numCorrect);
        }
    } // LogisticClassifier
}

