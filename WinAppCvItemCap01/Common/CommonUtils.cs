using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAppCvItemCap01.Common
{
    class CommonUtils
    {
        public static void BubbleSort<T, C>(T[] array, C comparer) where C : IComparer<T>
        {
            int length = array.Length;

            for (int i = 0; i <= length - 2; i++)
            {
                //Console.Write("{0}: ", i + 1);
                for (int j = length - 1; j >= 1; j--)
                {
                    if (comparer.Compare(array[j], array[j - 1]) < 0)
                    {
                        Swap(ref array[j], ref array[j - 1]);
                    }
                }
                //Console.WriteLine();
                //AlgorithmHelper.PrintArray(array);
            }
        }
        private static void Swap<T>(ref T x, ref T y)
        {
            // Console.Write("{0}<-->{1} ", x, y);
            T temp = x;
            x = y;
            y = temp;
        }
    }
}
