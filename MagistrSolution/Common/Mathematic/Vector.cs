using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;

namespace Common.Mathematic
{
    [DataContract]
    public sealed class Vector
    {
        [DataMember]
        private double[] sourceMas;

        [DataMember]
        public int Count { get; private set; }

        public Vector(int n)
        {
            this.sourceMas = new double[n];
            this.Count = n;
        }

        public Vector(int n, params object[] values)
        {
            if (values.Length != n)
                throw new ArgumentException("wrong parameters");
            this.sourceMas = new double[n];
            this.Count = n;
            for (int i = 0; i < this.Count; i++)
            {
                this.sourceMas[i] = Convert.ToDouble(values[i]);
            }
        }

        public List<double> GetValues()
        {
            List<double> d = new List<double>();
            d.AddRange(this.sourceMas);
            return d;
        }

        public double this[int index]
        {
            get
            {
                if ((index < 0) || (index > this.Count))
                    throw new ArgumentOutOfRangeException();
                return sourceMas[index];
            }
            set
            {
                if ((index < 0) || (index > this.Count))
                    throw new ArgumentOutOfRangeException();
                sourceMas[index] = value;
            }
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            Vector resv = new Vector(v1.Count);
            for (int i = 0; i < resv.Count; i++)
                resv[i] = v1[i] + v2[i];
            return resv;
        }
        public static Vector operator +(Vector v1, double v2)
        {
            Vector resv = new Vector(v1.Count);
            for (int i = 0; i < resv.Count; i++)
                resv[i] = v1[i] + v2;
            return resv;
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            Vector resv = new Vector(v1.Count);
            for (int i = 0; i < resv.Count; i++)
                resv[i] = v1[i] - v2[i];
            return resv;
        }
        public static Vector operator *(double k, Vector v2)
        {
            Vector resv = new Vector(v2.Count);
            for (int i = 0; i < resv.Count; i++)
                resv[i] = k * v2[i];
            return resv;
        }
        public static Vector operator /(Vector v1, double d)
        {
            Vector resv = new Vector(v1.Count);
            for (int i = 0; i < resv.Count; i++)
                resv[i] = v1[i] / d;
            return resv;
        }

        public double Norma()
        {
            double s = 0;
            foreach (double val in this.sourceMas)
            {
                s += val;
            }
            return Math.Sqrt(s);
        }

        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            res.Append("[");
            foreach (var x in this.sourceMas)
            {
                res.AppendFormat("{0:f3},", x);
            }
            res.Remove(res.Length - 1, 1);
            res.Append("]");
            return res.ToString();
        }

    }
}
