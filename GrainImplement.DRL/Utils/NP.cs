using System;
using System.Collections.Generic;
using System.Linq;

namespace GrainImplement.DRL.Utils
{
    public partial class NP
    {
        public static int Random(int maxValue)
        {
            return new Random(Guid.NewGuid().GetHashCode()).Next(0, maxValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static float[] ToOneDimensional(float[][] input)
        {
            var list = input.ToList();
            int rows = list.Count;
            int cols = list[0].Length;
            int totalCount = rows * cols;
            float[] output = new float[totalCount];
            for (int i = 0; i < totalCount; i++)
                output[i] = input[i / cols][i % cols];
            return output;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter_shape"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T[] PadToShape<T>(int[] filter_shape, T value) where T : struct
        {
            var result = new T[filter_shape.Length];
            for (int i = 0; i < result.Length; i++) { result[i] = value; }
            return result;
        }

        /// concat arrrays to one array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="argumetns"></param>
        /// <returns></returns>
        public static T[] Concat<T>(params T[][] argumetns) where T : struct
        {
            var list = new List<T>();
            for (int i = 0; i < argumetns.Length; i++)
                list.AddRange(argumetns[i]);
            return list.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static double Random()
        {
            return new Random(Guid.NewGuid().GetHashCode()).NextDouble();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public static int Argmax(float[] inputs)
        {
            float max = inputs.Max();
            return Array.IndexOf(inputs, max);
        }

        public static double CalcuteAccuracy(double[] predict, double[] target)
        {
            int predCount = predict.GetLength(0);
            int labelCount = predict.GetLength(0);
            if (predCount != labelCount)
                return 0.0;
            double right = 0.0;
            for (int i = 0; i < predCount; i++)
                right += (predict[i] == target[i]) ? 1 : 0;
            return right / predCount;
        }

    }
}
