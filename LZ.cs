using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD55
{
    static class LZ
    {
        public static string Transform(int n1, int n2, string str)
        {
            string ret = "";
            int I = Convert.ToInt32(Math.Ceiling(Math.Log10(n1)) + Math.Ceiling(Math.Log10(n2)) + 1.0);
            int indI = 0 - n1;
            int indII = n2;
            string buf = "";
            int incr = 0;
            int ptr = 0;
            while (indII < str.Length + n2)
            {
                if (indII + incr - n2 < str.Length)
                    buf += str[indII + incr - n2];
                else
                    buf += Char.MinValue;
                bool search = false;
                for (int i = (indI < 0 ? 0 : indI); i < indII - n2; i++)
                {
                    for (int j = 0; j < buf.Length; j++)
                        if (str[i + j] != buf[j] || i + j >= indII - n2)
                            break;
                        else if (j == buf.Length - 1)
                        {
                            ptr = i - indI;
                            search = true;
                        }
                    if (search)
                        break;
                }
                incr++;
                if (!search)
                {
                    ret += "{" + ptr + "," + (buf.Length - 1) + "," + buf[buf.Length - 1] + "} ";
                    buf = "";
                    indI += incr;
                    indII += incr;
                    incr = ptr = 0;
                }
            }
            return ret;
        }

        public static string Recovery(int n1, int n2, string str)
        {
            string ret = "";
            int I = Convert.ToInt32(Math.Ceiling(Math.Log10(n1)) + Math.Ceiling(Math.Log10(n2)) + 1.0);
            int indI = 0 - n1;
            int indII = n2;
            string buf = "";
            int counter = 0;
            int p = 0, q = 0;
            char ns = Char.MinValue;
            while (counter < str.Length - 1)
            {
                for (int i = counter; i < str.Length; i++)
                {
                    buf += str[i];
                    if (str[i] == ' ' && buf.Length > 7)
                    {
                        int point = buf.IndexOf(',');
                        p = Convert.ToInt32(buf.Substring(1, point - 1));
                        buf = buf.Remove(0, point + 1);
                        point = buf.IndexOf(',');
                        q = Convert.ToInt32(buf.Substring(0, point));
                        ns = buf[buf.Length - 3];
                        counter = i + 1;
                        buf = "";
                        break;
                    }
                }
                for (int i = indI + p; i < indI + p + q; i++)
                    ret += ret[i];
                if (ns != Char.MinValue)
                    ret += ns;
                indI += q + 1;
                indII += q + 1;
            }
            return ret;
        }
    }
}
