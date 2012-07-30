using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Common.Mathematic
{
    [DataContract]
    public sealed class Vector
    {
        [DataMember] private double[] sourceMas;

        public Vector(int n)
        {
            sourceMas = new double[n];
            Count = n;
        }

        public Vector(int n, params object[] values)
        {
            if (values.Length != n)
                throw new ArgumentException("wrong parameters");
            sourceMas = new double[n];
            Count = n;
            for (int i = 0; i < Count; i++)
            {
                sourceMas[i] = Convert.ToDouble(values[i]);
            }
        }

        [DataMember]
        public int Count { get; private set; }

        public double this[int index]
        {
            get
            {
                if ((index < 0) || (index > Count))
                    throw new ArgumentOutOfRangeException();
                return sourceMas[index];
            }
            set
            {
                if ((index < 0) || (index > Count))
                    throw new ArgumentOutOfRangeException();
                sourceMas[index] = value;
            }
        }

        public List<double> GetValues()
        {
            var d = new List<double>();
            d.AddRange(sourceMas);
            return d;
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            var resv = new Vector(v1.Count);
            for (int i = 0; i < resv.Count; i++)
                resv[i] = v1[i] + v2[i];
            return resv;
        }

        public static Vector operator +(Vector v1, double v2)
        {
            var resv = new Vector(v1.Count);
            for (int i = 0; i < resv.Count; i++)
                resv[i] = v1[i] + v2;
            return resv;
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            var resv = new Vector(v1.Count);
            for (int i = 0; i < resv.Count; i++)
                resv[i] = v1[i] - v2[i];
            return resv;
        }

        public static Vector operator *(double k, Vector v2)
        {
            var resv = new Vector(v2.Count);
            for (int i = 0; i < resv.Count; i++)
                resv[i] = k*v2[i];
            return resv;
        }

        public static Vector operator /(Vector v1, double d)
        {
            var resv = new Vector(v1.Count);
            for (int i = 0; i < resv.Count; i++)
                resv[i] = v1[i]/d;
            return resv;
        }

        public double Norma()
        {
            double s = 0;
            foreach (double val in sourceMas)
            {
                s += val;
            }
            return Math.Sqrt(s);
        }

        public override string ToString()
        {
            var res = new StringBuilder();
            res.Append("[");
            foreach (double x in sourceMas)
            {
                res.AppendFormat("{0:f3},", x);
            }
            res.Remove(res.Length - 1, 1);
            res.Append("]");
            return res.ToString();
        }
    }
}