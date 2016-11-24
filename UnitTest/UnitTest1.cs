using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;


namespace UnitTest
{

    [TestClass]
    public class UnitTest1
    {
        public static List<double> percentage = new List<double>();
        public static List<Tuple<int, double, bool>> tuple = new List<Tuple<int, double, bool>>();
        public static int Counter = 0;

        [TestMethod]
        public void TestSVM()
        {
            AuxiliaryFunctions.WritePercentage(tuple.ToArray(), @"Output\percentage.txt");
        }
    }
}
