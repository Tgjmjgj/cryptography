using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD55
{
    static class SF
    {
        public class Triad : IComparable<Triad>
        {
            public char Symbol;
            public double Probability;
            public string Code;
            public Triad()
            {
                this.Probability = 0;
                this.Code = "";
            }
            public override string ToString()
            {
                return "\"" + this.Symbol + "\" : " + this.Probability + " = " + this.Code;
            }
            public int CompareTo(Triad two)
            {
                return two.Probability.CompareTo(this.Probability);
            }
        }

        public static List<Triad> Table;
 
        private static void ParseMessage(string str)
        {
            if (Table == null)
                Table = new List<Triad>();
            Table.Clear();
            for (int i = 0; i < str.Length; i++)
            {
                bool find = false;
                for (int j = 0; j < Table.Count; j++)
                    if (Table[j].Symbol == str[i])
                    {
                        find = true;
                        Table[j].Probability++;
                    }
                if (!find)
                {
                    Triad tr = new Triad();
                    tr.Symbol = str[i];
                    tr.Probability++;
                    Table.Add(tr);
                }
            }
            for (int i = 0; i < Table.Count; i++)
            {
                Table[i].Probability /= (double)str.Length;
                Math.Round(Table[i].Probability, 2);
            }
            Table.Sort();
        }
        
        private static double sum(int i1, int i2)
        {
            double ret = 0.0;
            for (int i = i1; i < i2; i++)
                ret += Table[i].Probability;
            return ret;
        }

        private static void Step(int i1, int i2)
        {
            if (i1 == i2 + 1) 
                return;
            double st = 0.0;
            double st1 = 0.0;
            double st_old = 1.0;
            for (int i = i1 + 1; i < i2; i++)
            {
                st = sum(i1, i);
                st1 = sum(i, i2);
                double d_st = Math.Abs(st - st1);
                if (d_st >= st_old || i == i2 - 1)
                {
                    if (d_st >= st_old)
                        i = i - 1;
                    for (int a = i1; a < i; a++)
                        Table[a].Code += "1";
                    for (int b = i; b < i2; b++)
                        Table[b].Code += "0";
                    Step(i1, i);
                    Step(i, i2);
                    break;
                }
                else
                    st_old = d_st;
            }
        }

        public static string Encode(string str)
        {
            ParseMessage(str);
            Step(0, Table.Count);
            string ret = "";
            for (int i = 0; i < str.Length; i++)
                ret += Table.First(tri => tri.Symbol == str[i]).Code;
            return ret;
        }

        public static string Decode(string str)
        {
            string ret = "";
            string buf = "";
            Triad symb = new Triad();
            for (int i = 0; i < str.Length; i++)
            {
                buf += str[i];
                symb = Table.FirstOrDefault(tri => tri.Code == buf);
                if (symb != null)
                {
                    ret += symb.Symbol;
                    buf = "";
                }
            }
            if (symb == null)
                return "Архив поврежден";
            else
                return ret;
        }
    }
}
