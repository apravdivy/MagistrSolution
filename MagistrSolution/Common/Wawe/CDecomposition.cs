using System;
using System.Collections.Generic;

namespace Common.Wawe
{
    /// <summary>
    /// Структура разложения сигнала
    /// </summary>
    public class CDecomposition
    {
        private readonly int[][] L = new int[10][];

        /// <summary>
        /// Коэффициенты аппроксимации на последнем уровне разлжения
        /// </summary>
        public List<double> Approx;

        /// <summary>
        /// Коэффициенты детализации на каждом уровне разложения. Размер массива определяет глубину разложения
        /// </summary>
        public List<double>[] Details;

        /// <summary>
        /// Степень двойки длины сигнала
        /// </summary>
        public int J;

        /// <summary>
        /// Длина исходного сигнала
        /// </summary>
        public int SignalLength;

        /// <summary>
        /// Порядок вейвлета
        /// </summary>
        public int WawletOrder;

        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="Level">Уровень разложения</param>
        public CDecomposition(int Level)
        {
            Approx = new List<double>();
            Details = new List<double>[Level];
            for (int i = 0; i < Level; i++)
                Details[i] = new List<double>();
            for (int i = 0; i < L.Length; i++)
            {
                L[i] = new int[5];
            }

            #region Set Lj

            L[0][0] = 0;
            L[0][1] = 0;
            L[0][2] = 0;
            L[0][3] = 0;
            L[0][4] = 0;

            L[1][0] = 1;
            L[1][1] = 2;
            L[1][2] = 2;
            L[1][3] = 2;
            L[1][4] = 2;

            L[2][0] = 2;
            L[2][1] = 3;
            L[2][2] = 4;
            L[2][3] = 4;
            L[2][4] = 4;

            L[3][0] = 3;
            L[3][1] = 5;
            L[3][2] = 6;
            L[3][3] = 6;
            L[3][4] = 6;

            L[4][0] = 4;
            L[4][1] = 6;
            L[4][2] = 7;
            L[4][3] = 8;
            L[4][4] = 8;

            L[5][0] = 5;
            L[5][1] = 8;
            L[5][2] = 9;
            L[5][3] = 10;
            L[5][4] = 10;

            L[6][0] = 6;
            L[6][1] = 9;
            L[6][2] = 11;
            L[6][3] = 12;
            L[6][4] = 12;

            L[7][0] = 7;
            L[7][1] = 11;
            L[7][2] = 13;
            L[7][3] = 14;
            L[7][4] = 14;

            L[8][0] = 8;
            L[8][1] = 12;
            L[8][2] = 14;
            L[8][3] = 15;
            L[8][4] = 16;

            L[9][0] = 9;
            L[9][1] = 14;
            L[9][2] = 16;
            L[9][3] = 17;
            L[9][4] = 18;

            #endregion
        }

        /// <summary>
        /// Получение коефициентов для мер близости
        /// </summary>
        /// <returns></returns>
        public List<double> GetKoefsForMera()
        {
            var reslist = new List<double>();

            double Sum = 0;
            double Ls = 0;
            for (int j = Details.Length - 1; j >= 0; j--)
            {
                int nomer = 0;
                if (j == Details.Length - 2)
                    nomer = 1;
                if (j == Details.Length - 3)
                    nomer = 2;
                if (j == Details.Length - 4)
                    nomer = 3;
                if (j <= Details.Length - 5)
                    nomer = 4;

                var bbb = (int) Math.Pow(2, j);
                int P = Math.Min(L[WawletOrder - 1][nomer], bbb);

                for (int n = 0; n < Details[j].Count; n++)
                {
                    if (n >= P)
                    {
                        Ls++;
                        Sum += Math.Pow(Details[j][n]*Math.Pow(2, j), 2);
                    }
                }
            }
            Sum = Math.Sqrt(Sum);

            for (int j = Details.Length - 1; j >= 0; j--)
            {
                int nomer = 0;
                if (j == Details.Length - 2)
                    nomer = 1;
                if (j == Details.Length - 3)
                    nomer = 2;
                if (j == Details.Length - 4)
                    nomer = 3;
                if (j <= Details.Length - 5)
                    nomer = 4;

                var bbb = (int) Math.Pow(2, j);
                int P = Math.Min(L[WawletOrder - 1][nomer], bbb);

                for (int n = 0; n < Details[j].Count; n++)
                {
                    if (n >= P)
                    {
                        double val = Details[j][n]*Math.Pow(2, j)/Sum;
                        reslist.Add(val);
                    }
                }
            }

            return reslist;
        }

        public void GetKoefs()
        {
            for (int j = Details.Length - 1; j >= 0; j--)
            {
                int nomer = 0;
                if (j == Details.Length - 2)
                    nomer = 1;
                if (j == Details.Length - 3)
                    nomer = 2;
                if (j == Details.Length - 4)
                    nomer = 3;
                if (j <= Details.Length - 5)
                    nomer = 4;

                var bbb = (int) Math.Pow(2, j);
                int P = Math.Min(L[WawletOrder - 1][nomer], bbb);

                for (int n = 0; n < Details[j].Count; n++)
                    if (n >= P)
                        Details[j][n] = 0;
            }
        }

        public void DeNoise(double noiseval)
        {
            for (int j = Details.Length - 1; j >= 0; j--)
                for (int n = 0; n < Details[j].Count; n++)
                    if (Details[j][n] >= noiseval)
                        Details[j][n] = 0;
        }
    }
}