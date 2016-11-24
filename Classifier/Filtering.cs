using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classifier
{

    class Filtering
    {

        public static List<Rectangle> FilterData(List<Rectangle> rectangleList)
        {
            int i = 1;
            List<Rectangle> rectangleListOut = new List<Rectangle>();
            foreach (Rectangle rect in rectangleList)
            {
                lmp.Add(new MyPoint(i++, rect.X, rect.Y));
            }

            huinya();
            K = CenterList.Count();


            kmean();

            Pen myPen = new Pen(Color.Red, 8);
            foreach (var list in doublelist)
            {
                int xx = 0;
                int yy = 0;


                foreach (var point in list)
                {
                    xx+=point.x;
                    yy+=point.y;
                }
                
                rectangleListOut.Add(new Rectangle(xx/list.Count, yy/list.Count, 64, 128));
            }
            return rectangleListOut;
        }













        private static List<MyPoint> lmp = new List<MyPoint>();
        private static List<MyPoint> CenterList = new List<MyPoint>();

        private static List<double[]> ld = new List<double[]>();

        private static List<double> MinList = new List<double>();
        private static int p = -1;
        private static bool isOK = true;





        private static List<int[]> il = new List<int[]>();
        private static int count = 1;
        private static int K = 0;
        private static List<List<MyPoint>> doublelist = new List<List<MyPoint>>();
        private static List<MyPoint> lmp2;
        private static List<object> End = new List<object>();
        private static List<MyPoint> newCenter = new List<MyPoint>();
        private static bool isFirstIteration = true;
        private static int NUM = 0;
        private static Random rnd = new Random();
       






        public class MyPoint
        {
            public int number;
            public int x;
            public int y;
            public bool isCenter = false;
            public MyPoint(int n, int i, int j)
            {
                number = n;
                x = i;
                y = j;
            }
        }
        private static double FindDistance(MyPoint mp, MyPoint mp2)
        {
            double k = Math.Pow((mp.x - mp2.x), 2) + Math.Pow((mp.y - mp2.y), 2);
            double p = Math.Sqrt(k);
            return p;
        }
        private static double FindMaxDistance(List<double> dob, ref int pi)
        {
            double Max = 0;
            for (int i = 0; i < dob.Count; i++)
            {
                if (dob[i] > Max)
                {
                    Max = dob[i];
                    pi = i;
                }
            }
            return Max;
        }
        private static double FindMinimum(double[] dob)
        {
            double Min = 10000;
            for (int i = 0; i < dob.Length; i++)
            {
                if (dob[i] < Min)
                    Min = dob[i];
            }
            return Min;
        }
        private static double[] ListToDoubleArray(List<double[]> ltd, int index)
        {
            double[] array = new double[ltd.Count];
            for (int i = 0; i < ltd.Count; i++)
            {
                array[i] = ltd[i][index];
            }
            return array;

        }

        private static double MainAction(List<double> m, List<double[]> a, ref int k)
        {
            m.Clear();
            MakeMinimumList(m, a);
            double j = FindMaxDistance(m, ref k);
            return j;
        }




        public static void huinya()
        {
            
                CenterList.Clear();
                for (int i = 0; i < lmp.Count; i++)
                {
                    lmp[i].isCenter = false;
                }

                lmp[0].isCenter = true;
                CenterList.Add(lmp[0]);

                double[] k;
                List<double> LALALA = new List<double>();
                double Lround = 0.0;
                isOK = true;
                while (isOK)
                {
                    for (int j = 0; j < CenterList.Count; j++)
                    {
                        k = new double[lmp.Count];
                        for (int i = 0; i < lmp.Count; i++)
                        {
                            if (!lmp[i].isCenter)
                            {
                                k[i] = FindDistance(CenterList[j], lmp[i]);
                            }
                        }
                        ld.Add(k);
                    }
                    double L = MainAction(MinList, ld, ref p);

                    MinList.Clear(); ld.Clear();
                    LALALA.Add(L);
                    L = 0.0;
                    if (LALALA.Count == 1)
                    {
                        if (p == -1)
                        {
                            isOK = false;
                            break;

                        }
                        lmp[p].isCenter = true;
                        CenterList.Add(lmp[p]);

                        p = -1;
                    }
                    else if (LALALA.Count == 2)
                    {
                        if (LALALA[LALALA.Count - 1] > 0.5 * LALALA[LALALA.Count - 2])
                        {
                            Lround = (LALALA[LALALA.Count - 1] + LALALA[LALALA.Count - 2]) / 2;
                            lmp[p].isCenter = true;
                            CenterList.Add(lmp[p]);
                            p = -1;
                        }
                        else
                        {
                            isOK = false;
                        }
                    }
                    else if (LALALA.Count > 2)
                    {
                        if (LALALA[LALALA.Count - 1] > 0.5 * Lround)
                        {
                            Lround = 0;
                            for (int i = 0; i < LALALA.Count; i++)
                            {
                                Lround += LALALA[i];
                            }
                            Lround /= LALALA.Count;
                            lmp[p].isCenter = true;
                            CenterList.Add(lmp[p]);
                            p = -1;
                        }
                        else
                        {
                            isOK = false;
                        }
                    }
                }
                int numberCl = 1;
                for (int i = 0; i < lmp.Count; i++)
                {
                    if (lmp[i].isCenter)
                    {
                        numberCl += 1;
                    }

                }
            
           


        }












        public static void kmean()
        {

            isFirstIteration = true;
            isOK = true; doublelist.Clear();
            NUM = lmp.Count(); doublelist.Clear();
            ld.Clear(); MinList.Clear(); il.Clear();
            newCenter.Clear();
            if (K > 1)
            {
                if (NUM >= K)
                {
                    while (isOK)
                    {
                        if (isFirstIteration)
                        {
                            double[] k; int[] count;
                            for (int i = 0; i < K; i++)
                            {
                                lmp2 = new List<MyPoint>();
                                doublelist.Add(lmp2);
                            }
                            for (int i = 0; i < K; i++)
                            {
                                doublelist[i].Add(lmp[i]);
                            }
                            for (int j = 0; j < doublelist.Count; j++)
                            {
                                k = new double[lmp.Count - K];

                                for (int i = K; i < lmp.Count; i++)
                                {
                                    k[i - K] = FindDistance(doublelist[j][0], lmp[i]);
                                }
                                ld.Add(k);
                            }
                            //try
                            //{
                            MakeMinimumList2(MinList, ld);
                            MakeKluster(MinList, lmp, doublelist, isFirstIteration);
                            count = new int[doublelist.Count];
                            for (int i = 0; i < doublelist.Count; i++)
                            {
                                count[i] = doublelist[i].Count;
                            }
                            il.Add(count);
                            FindNewCenter(doublelist, newCenter);
                            //}
                            //catch (Exception ex)
                            //{
                            //    MessageBox.Show("Your K is equal to 0");
                            //    isOK = false;
                            //}
                        }
                        else if (!isFirstIteration)
                        {
                            double[] k; int[] count;
                            doublelist.Clear();
                            ld.Clear(); MinList.Clear();
                            for (int i = 0; i < K; i++)
                            {
                                lmp2 = new List<MyPoint>();
                                doublelist.Add(lmp2);
                            }
                            for (int i = 0; i < K; i++)
                            {
                                doublelist[i].Add(newCenter[i]);
                            }
                            for (int j = 0; j < doublelist.Count; j++)
                            {
                                k = new double[lmp.Count];
                                for (int i = 0; i < lmp.Count; i++)
                                {
                                    k[i] = FindDistance(doublelist[j][0], lmp[i]);
                                }
                                ld.Add(k);
                            }
                            MakeMinimumList2(MinList, ld);
                            MakeKluster(MinList, lmp, doublelist, isFirstIteration);
                            for (int i = 0; i < K; i++)
                            {
                                doublelist[i].RemoveAt(0);
                            }
                            count = new int[doublelist.Count];
                            for (int i = 0; i < doublelist.Count; i++)
                            {
                                count[i] = doublelist[i].Count;
                            }
                            il.Add(count);
                            newCenter.Clear();
                            FindNewCenter(doublelist, newCenter);
                            isOK = !Check(il);
                            il.RemoveAt(0);
                        }
                        isFirstIteration = false;
                    }

                }

            }

        }

        private static bool Check(List<int[]> li)
        {
            bool isEnd = true;
            for (int i = 0; i < li.Count; i++)
            {
                if (li[0][i] != li[1][i])
                {
                    isEnd = false;
                }
            }
            return isEnd;
        }
        private static void FindNewCenter(List<List<MyPoint>> dlist, List<MyPoint> list)
        {
            MyPoint mp;
            for (int i = 0; i < K; i++)
            {
                list.Add(mp = new MyPoint(0, 0, 0));
            }
            for (int i = 0; i < dlist.Count; i++)
            {
                for (int j = 0; j < dlist[i].Count; j++)
                {
                    list[i].x += dlist[i][j].x;
                    list[i].y += dlist[i][j].y;
                }

            }
            for (int i = 0; i < dlist.Count; i++)
            {
                list[i].x /= dlist[i].Count;
                list[i].y /= dlist[i].Count;
            }

        }

        private static double FindMinimum2(double[] dob)
        {
            int p = 0;
            double Min = 10000;
            for (int i = 0; i < dob.Length; i++)
            {
                if (dob[i] < Min)
                {
                    Min = dob[i];
                    p = i;

                }
            }
            return p;
        }

        private static void MakeMinimumList2(List<double> min, List<double[]> arr)
        {
            double k;
            double[] ARRAY = new double[arr.Count];
            for (int i = 0; i < arr[0].Length; i++)
            {
                ARRAY = ListToDoubleArray(arr, i);
                k = FindMinimum2(ARRAY);
                min.Add(k);
            }

        }
        private static void MakeMinimumList(List<double> min, List<double[]> arr)
        {
            double k;
            double[] ARRAY = new double[arr.Count];
            for (int i = 0; i < arr[0].Length; i++)
            {
                ARRAY = ListToDoubleArray(arr, i);
                k = FindMinimum(ARRAY);
                min.Add(k);
            }

        }
        private static void MakeKluster(List<double> min, List<MyPoint> list, List<List<MyPoint>> dlist, bool isFirst)
        {
            switch (isFirst)
            {
                case true:
                    for (int i = 0; i < min.Count; i++)
                    {
                        dlist[(int)min[i]].Add(list[i + K]);
                    }
                    break;
                case false:
                    for (int i = 0; i < min.Count; i++)
                    {
                        dlist[(int)min[i]].Add(list[i]);
                    }
                    break;
            }
        }

    }






}
