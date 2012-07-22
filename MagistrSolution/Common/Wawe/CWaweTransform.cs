using System;
using System.Collections.Generic;

namespace Common.Wawe
{
    public class CWaweTransform
    {
        private List<double> Gn;
        private List<double> Hn;

        /// <summary>
        /// количемтво эл-в в фильтре
        /// </summary>
        private int M;

        #region Коэфициенты Добеши

        private readonly List<double> Dob1 = new List<double>();
        private readonly List<double> Dob10 = new List<double>();
        private readonly List<double> Dob2 = new List<double>();
        private readonly List<double> Dob3 = new List<double>();
        private readonly List<double> Dob4 = new List<double>();
        private readonly List<double> Dob5 = new List<double>();
        private readonly List<double> Dob6 = new List<double>();
        private readonly List<double> Dob7 = new List<double>();
        private readonly List<double> Dob8 = new List<double>();
        private readonly List<double> Dob9 = new List<double>();

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        public CWaweTransform()
        {
            #region SetKoefs

            Dob1.Add(0.7071067811865475);
            Dob1.Add(0.7071067811865475);

            Dob2.Add(0.4829629131445341);
            Dob2.Add(0.8365163037378077);
            Dob2.Add(0.2241438680420134);
            Dob2.Add(-0.1294095225512603);

            Dob3.Add(0.3326705529500827);
            Dob3.Add(0.8068915093110928);
            Dob3.Add(0.4598775021184915);
            Dob3.Add(-0.1350110200102546);
            Dob3.Add(-0.0854412738820267);
            Dob3.Add(0.0352262918857096);

            Dob4.Add(0.2303778133074431);
            Dob4.Add(0.7148465705484058);
            Dob4.Add(0.6308807679358788);
            Dob4.Add(-0.0279837694166834);
            Dob4.Add(-0.1870348117179132);
            Dob4.Add(0.0308413818353661);
            Dob4.Add(0.0328830116666778);
            Dob4.Add(-0.0105974017850021);

            Dob5.Add(0.1601023979741930);
            Dob5.Add(0.6038292697971898);
            Dob5.Add(0.7243085284377729);
            Dob5.Add(0.1384281459013204);
            Dob5.Add(-0.2422948870663824);
            Dob5.Add(-0.0322448695846381);
            Dob5.Add(0.0775714938400459);
            Dob5.Add(-0.0062414902127983);
            Dob5.Add(-0.0125807519990820);
            Dob5.Add(0.0033357252854738);

            Dob6.Add(0.1115407433501094);
            Dob6.Add(0.4946238903984530);
            Dob6.Add(0.7511339080210954);
            Dob6.Add(0.3152503517091980);
            Dob6.Add(-0.2262646939654399);
            Dob6.Add(-0.1297668675672624);
            Dob6.Add(0.0975016055873224);
            Dob6.Add(0.0275228655303053);
            Dob6.Add(-0.0315820393174862);
            Dob6.Add(0.0005538422011614);
            Dob6.Add(0.0047772575109455);
            Dob6.Add(-0.0010773010853085);

            Dob7.Add(0.0778520540850081);
            Dob7.Add(0.3965393194819136);
            Dob7.Add(0.7291320908462368);
            Dob7.Add(0.4697822874052154);
            Dob7.Add(-0.1439060039285293);
            Dob7.Add(-0.2240361849938538);
            Dob7.Add(0.0713092192668312);
            Dob7.Add(0.0806126091510820);
            Dob7.Add(-0.0380299369350125);
            Dob7.Add(-0.0165745416306664);
            Dob7.Add(0.0125509985560993);
            Dob7.Add(0.0004295779729214);
            Dob7.Add(-0.0018016407040474);
            Dob7.Add(0.0003537137999745);

            Dob8.Add(0.0544158422431049);
            Dob8.Add(0.3128715909143031);
            Dob8.Add(0.6756307362972904);
            Dob8.Add(0.5853546836541907);
            Dob8.Add(-0.0158291052563816);
            Dob8.Add(-0.2840155429615702);
            Dob8.Add(0.0004724845739124);
            Dob8.Add(0.1287474266204837);
            Dob8.Add(-0.0173693010018083);
            Dob8.Add(-0.0440882539307952);
            Dob8.Add(0.0139810279173995);
            Dob8.Add(0.0087460940474061);
            Dob8.Add(-0.0048703529934518);
            Dob8.Add(-0.0003917403733770);
            Dob8.Add(0.0006754494064506);
            Dob8.Add(-0.0001174767841248);

            Dob9.Add(0.0380779473638791);
            Dob9.Add(0.2438346746125939);
            Dob9.Add(0.6048231236901156);
            Dob9.Add(0.6572880780512955);
            Dob9.Add(0.1331973858249927);
            Dob9.Add(-0.2932737832791761);
            Dob9.Add(-0.0968407832229524);
            Dob9.Add(0.1485407493381306);
            Dob9.Add(0.0307256814793395);
            Dob9.Add(-0.0676328290613302);
            Dob9.Add(0.0002509471148340);
            Dob9.Add(0.0223616621236805);
            Dob9.Add(-0.0047232047577520);
            Dob9.Add(-0.0042815036824636);
            Dob9.Add(0.0018476468830564);
            Dob9.Add(0.0002303857635232);
            Dob9.Add(-0.0002519631889427);
            Dob9.Add(0.0000393473203163);

            Dob10.Add(0.0266700579005546);
            Dob10.Add(0.1881768000776863);
            Dob10.Add(0.5272011889317202);
            Dob10.Add(0.6884590394536250);
            Dob10.Add(0.2811723436606485);
            Dob10.Add(-0.2498464243272283);
            Dob10.Add(-0.1959462743773399);
            Dob10.Add(0.1273693403357890);
            Dob10.Add(0.0930573646035802);
            Dob10.Add(-0.0713941471663697);
            Dob10.Add(-0.0294575368218480);
            Dob10.Add(0.0332126740593703);
            Dob10.Add(0.0036065535669880);
            Dob10.Add(-0.0107331754833036);
            Dob10.Add(0.0013953517470692);
            Dob10.Add(0.0019924052951930);
            Dob10.Add(-0.0006858566949566);
            Dob10.Add(-0.0001164668551285);
            Dob10.Add(0.0000935886703202);
            Dob10.Add(-0.0000132642028945);

            #endregion
        }

        /// <summary>
        /// Инициализирует фильтры
        /// </summary>
        /// <param name="N">Порядок вейвлета</param>
        public void InitFilter(int N)
        {
            switch (N)
            {
                case 1:
                    Hn = Dob1;
                    break;
                case 2:
                    Hn = Dob2;
                    break;
                case 3:
                    Hn = Dob3;
                    break;
                case 4:
                    Hn = Dob4;
                    break;
                case 5:
                    Hn = Dob5;
                    break;
                case 6:
                    Hn = Dob6;
                    break;
                case 7:
                    Hn = Dob7;
                    break;
                case 8:
                    Hn = Dob8;
                    break;
                case 9:
                    Hn = Dob9;
                    break;
                case 10:
                    Hn = Dob10;
                    break;
            }
            M = Hn.Count;
            Gn = new List<double>();
            for (int n = 0; n < M; n++)
                Gn.Add(Math.Pow(-1, n)*Hn[M - 1 - n]);
        }

        /// <summary>
        /// Прямое дискретное вейвлет преобразование
        /// </summary>
        /// <param name="Yn">Сигнал</param>
        /// <param name="J">Степень двойки длины сигнала</param>
        /// <param name="N">Порядок вейвлета</param>
        /// <returns>Разложение сигнала</returns>
        public CDecomposition Decompose(List<double> Yn, int J, int N)
        {
            InitFilter(N);
            var L = (int) Math.Pow(2, J);
            var Decomp = new CDecomposition(J);
            Decomp.SignalLength = Yn.Count;
            Decomp.WawletOrder = N;
            Decomp.J = J;

            int u;
            var B = new double[J + 1][];
            var A = new double[J][];

            B[J] = new double[L];
            for (int n = 0; n < L; n++)
                B[J][n] = Math.Pow(L, -0.5)*Yn[n];

            for (int j = J; j > 0; j--)
            {
                B[j - 1] = new double[(int) Math.Pow(2, j - 1)];
                A[j - 1] = new double[(int) Math.Pow(2, j - 1)];

                for (int t = 0; t < Math.Pow(2, j - 1); t++)
                {
                    u = 2*t + 1;
                    A[j - 1][t] = Gn[0]*B[j][u];
                    B[j - 1][t] = Hn[0]*B[j][u];
                    for (int n = 1; n < M; n++)
                    {
                        u = u - 1;
                        if (u < 0)
                            u = (int) Math.Pow(2, j) - 1;
                        A[j - 1][t] += Gn[n]*B[j][u];
                        B[j - 1][t] += Hn[n]*B[j][u];
                    }
                    Decomp.Details[j - 1].Add(A[j - 1][t]);
                }
            }
            Decomp.Approx.Add(B[0][0]);


            return Decomp;
        }

        /// <summary>
        /// Обратное дискретное вейвлет преобразование
        /// </summary>
        /// <param name="Decomp">Разложение сигнала</param>
        /// <returns>Исходный сигнал</returns>
        public List<double> Reconstruct(CDecomposition Decomp)
        {
            var Y = new List<double>();
            InitFilter(Decomp.WawletOrder);
            int l, m, u, i, k;
            int J = Decomp.J;
            var B = new double[J + 1][];
            var A = new double[J][];
            int L = Decomp.SignalLength;

            B[J] = new double[Decomp.SignalLength];

            for (int j = J; j > 0; j--)
            {
                B[j - 1] = new double[(int) Math.Pow(2, j - 1)];
                A[j - 1] = new double[(int) Math.Pow(2, j - 1)];
            }

            B[0][0] = Decomp.Approx[0];
            A[0][0] = Decomp.Details[0][0];

            for (int j = 1; j < J + 1; j++)
            {
                l = -2;
                m = -1;
                for (int t = 0; t < Math.Pow(2, j - 1); t++)
                {
                    l = l + 2;
                    m = m + 2;
                    u = t;
                    i = 1;
                    k = 0;
                    B[j][l] = Gn[i]*Decomp.Details[j - 1][u] + Hn[i]*B[j - 1][u];
                    B[j][m] = Gn[k]*Decomp.Details[j - 1][u] + Hn[k]*B[j - 1][u];
                    if (M > 2)
                    {
                        for (int n = 1; n < M/2; n++)
                        {
                            u = u + 1;
                            if (u >= Math.Pow(2, j - 1))
                                u = 0;
                            i = i + 2;
                            k = k + 2;
                            B[j][l] += Gn[i]*Decomp.Details[j - 1][u] + Hn[i]*B[j - 1][u];
                            B[j][m] += Gn[k]*Decomp.Details[j - 1][u] + Hn[k]*B[j - 1][u];
                        }
                    }
                }
            }
            for (int n = 0; n < B[J].Length; n++)
                Y.Add(Math.Pow(L, 0.5)*B[J][n]);
            return Y;
        }

        /// <summary>
        /// Разделение сигнала
        /// </summary>
        /// <param name="Decomp">Разложение сигнала</param>
        /// <param name="j1">Пик №1</param>
        /// <param name="j2">Пик №2</param>
        /// <returns>Список разложений сигналов</returns>
        public List<CDecomposition> Divide(CDecomposition Decomp, int j1, int j2)
        {
            var LDecomp = new List<CDecomposition>();
            if (j2 == j1 + 2)
            {
                CDecomposition d1, d2;

                d1 = new CDecomposition(Decomp.J);
                d2 = new CDecomposition(Decomp.J);

                d1.J = Decomp.J;
                d1.WawletOrder = Decomp.WawletOrder;
                d1.SignalLength = Decomp.SignalLength;

                d2.J = Decomp.J;
                d2.WawletOrder = Decomp.WawletOrder;
                d2.SignalLength = Decomp.SignalLength;

                int z = j1 + 1; // level between j1 and j2
                var a = new double[(int) Math.Pow(2, z)];
                var b = new double[(int) Math.Pow(2, z)];

                for (int n = 0; n < a.Length; n++)
                {
                    a[n] = Decomp.Details[z - 1][n/2]*Decomp.Details[z - 1][n/2];
                    b[n] = (Decomp.Details[z + 1][2*n] + Decomp.Details[z + 1][2*n + 1])/2;
                }

                d1.Approx.Add(Decomp.Approx[0]);
                d2.Approx.Add(0);

                for (int j = 0; j < Decomp.J; j++)
                    for (int i = 0; i < Decomp.Details[j].Count; i++)
                        if (j < j1 + 1)
                            d1.Details[j].Add(Decomp.Details[j][i]);
                        else
                        {
                            if (j == z)
                            {
                                d1.Details[j].Add((a[i]/(a[i] + b[i]))*Decomp.Details[j][i]);
                            }
                            else
                                d1.Details[j].Add(0);
                        }

                for (int j = 0; j < Decomp.J; j++)
                    for (int i = 0; i < Decomp.Details[j].Count; i++)
                        if (j < j2 - 1)
                            d2.Details[j].Add(0);
                        else
                        {
                            if (j == z)
                            {
                                d2.Details[j].Add((b[i]/(a[i] + b[i]))*Decomp.Details[j][i]);
                            }
                            else
                                d2.Details[j].Add(Decomp.Details[j][i]);
                        }

                LDecomp.Add(d1);
                LDecomp.Add(d2);
            }
            else
            {
                CDecomposition d1, d2;

                d1 = new CDecomposition(Decomp.J);
                d2 = new CDecomposition(Decomp.J);

                d1.J = Decomp.J;
                d1.WawletOrder = Decomp.WawletOrder;
                d1.SignalLength = Decomp.SignalLength;

                d2.J = Decomp.J;
                d2.WawletOrder = Decomp.WawletOrder;
                d2.SignalLength = Decomp.SignalLength;


                d1.Approx.Add(Decomp.Approx[0]);
                d2.Approx.Add(0);

                for (int j = 0; j < Decomp.J; j++)
                    for (int i = 0; i < Decomp.Details[j].Count; i++)
                        if (j <= j1 + 1)
                            d1.Details[j].Add(Decomp.Details[j][i]);
                        else
                            d1.Details[j].Add(0);

                for (int j = 0; j < Decomp.J; j++)
                    for (int i = 0; i < Decomp.Details[j].Count; i++)
                        if (j <= j2 - 1)
                            d2.Details[j].Add(0);
                        else
                            d2.Details[j].Add(Decomp.Details[j][i]);

                LDecomp.Add(d1);
                LDecomp.Add(d2);
            }
            return LDecomp;
        }

        public List<double> GetAproxKoef(List<double> Yn, int N, int K)
        {
            InitFilter(N);
            int J = 0;
            if (Yn.Count%2 != 0)
                throw new Exception("Длина сигнала не является степенью двойки");
            for (int i = 1; i < 15; i++)
            {
                J = i;
                if (Yn.Count == Math.Pow(2, J))
                    break;
            }

            var L = (int) Math.Pow(2, J);

            int u;
            var B = new double[J + 1][];
            var A = new double[J][];

            B[J] = new double[L];
            for (int n = 0; n < L; n++)
                B[J][n] = Math.Pow(L, -0.5)*Yn[n];

            int resind = -1;
            for (int j = J; j > 0; j--)
            {
                B[j - 1] = new double[(int) Math.Pow(2, j - 1)];
                A[j - 1] = new double[(int) Math.Pow(2, j - 1)];

                for (int t = 0; t < Math.Pow(2, j - 1); t++)
                {
                    u = 2*t + 1;
                    A[j - 1][t] = Gn[0]*B[j][u];
                    B[j - 1][t] = Hn[0]*B[j][u];
                    for (int n = 1; n < M; n++)
                    {
                        u = u - 1;
                        if (u < 0)
                            u = (int) Math.Pow(2, j) - 1;
                        A[j - 1][t] += Gn[n]*B[j][u];
                        B[j - 1][t] += Hn[n]*B[j][u];
                    }
                }
                K--;
                if (K == 0)
                {
                    resind = j - 1;
                    break;
                }
            }

            var res = new List<double>();
            for (int i = 0; i < B[resind].Length; i++)
                res.Add(B[resind][i]);
            return res;
        }
    }
}