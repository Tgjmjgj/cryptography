using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace MD55
{
    class RSA
    {
        public BigInteger n;
        public BigInteger e;
        private BigInteger fi;
        private BigInteger d;

        private int[] Ferma = { 3, 5, 17, 257, 65537 };

        public bool Create(BigInteger p, BigInteger q)
        {
            this.n = this.e = this.fi = this.d = BigInteger.Zero;
            if (p.Equals(BigInteger.One) || q.Equals(BigInteger.One) || p.Equals(q))
                return false;
            this.n = p * q;
            this.fi = (p - BigInteger.One) * (q - BigInteger.One);
            int i = Ferma.Length;
            while (i > 0)
            {
                i--;
                if (Ferma[i] < this.fi)
                {
                    this.e = Ferma[i];
                    break;
                }
            }
            if (this.e == BigInteger.Zero)
                return false;
            BigInteger a = this.e;
            BigInteger b = this.fi, xx = BigInteger.Zero, dx = BigInteger.One;
            while (a.CompareTo(BigInteger.Zero) == 1)
            {
                BigInteger Q = b / a;
                BigInteger y = a;
                a = b % a;
                b = y;
                y = dx;
                dx = xx - Q * dx;
                xx = y;
            }
            xx = xx % n;
            if (xx.CompareTo(BigInteger.Zero) == -1)
                xx = (xx + n) % n;
            d = xx;
            return true;
        }

        public string Crypt(string msg)
        {
            string ret = "";
            foreach (char s in msg)
                ret += (char)((int)(BigInteger.Pow(new BigInteger((int)s), (int)this.d) % this.n));
            return ret;
        }

        public string Encrypt(string msg)
        {
            string ret = "";
            foreach (char s in msg)
                ret += (char)((int)(BigInteger.Pow(new BigInteger((int)s), (int)this.e) % this.n));
            return ret;
        }
    }
}
