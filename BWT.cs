using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MD55
{
    class BWT
    {
        public static MainWindow refWindow = null;

        private static string lshift(string str, int n)
        {
            return str.Substring(n) + str.Substring(0, n);
        }

        public static string Encode(string str)
        {
            var matrix = new List<string>();
            for (int i = 0; i < str.Length; i++)
                matrix.Add(lshift(str, i));
            refWindow.bwt_listBox_a.Items.Add(FormationMatrix(matrix));
            matrix.Sort();
            refWindow.bwt_listBox_a.Items.Add(FormationMatrix(matrix));
            string ret = "";
            for (int i = 0; i < str.Length; i++)
                ret += matrix[i][str.Length - 1];
            ret += ", " + matrix.IndexOf(str);
            return ret;
        }

        private static TextBlock FormationMatrix(List<string> mtr, bool encode = true)
        {
            string ret = "";
            foreach (string s in mtr)
            {
                foreach (char c in s)
                    ret += c + " ";
                ret += Environment.NewLine;
            }
            TextBlock txt = new TextBlock();
            txt.TextWrapping = System.Windows.TextWrapping.Wrap;
            txt.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            if (encode)
                txt.TextAlignment = System.Windows.TextAlignment.Left;
            else
                txt.TextAlignment = System.Windows.TextAlignment.Right;
            txt.Text = ret;
            return txt;
        }

        public static void Decode(string str)
        {
            int ind = str.Length - 1;
            while (str[ind] != ' ')
                ind--;
            int index = Convert.ToInt32(str.Substring(ind));
            str = str.Substring(0, ind - 1);
            var matrix = new List<string>();
            for (int i = 0; i < str.Length; i++)
                matrix.Add(str[i].ToString());
            refWindow.bwt_listBox_b.Items.Add(FormationMatrix(new List<string>(matrix), false));
            matrix.Sort();
            refWindow.bwt_listBox_b.Items.Add(FormationMatrix(new List<string>(matrix), false));
            for (int i = 1; i < str.Length; i++)
            {
                for (int j = 0; j < str.Length; j++)
                    matrix[j] = str[j] + matrix[j];
                refWindow.bwt_listBox_b.Items.Add(FormationMatrix(new List<string>(matrix), false));
                matrix.Sort();
                refWindow.bwt_listBox_b.Items.Add(FormationMatrix(new List<string>(matrix), false));
            }
        }
    }
}
