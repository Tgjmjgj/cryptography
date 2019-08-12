using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD55
{
    static class HFF
    {
        public class TFF_Node : IComparable<TFF_Node>
        {
            public TFF_Node Left;
            public TFF_Node Right;
            public TFF_Node Parent;
            public char Code;
            public int Weight;

            public TFF_Node()
            {
                this.Left = this.Right = this.Parent = null;
                this.Code = Char.MinValue;
                this.Weight = 0;
            }

            public int CompareTo(TFF_Node two)
            {
                return this.Weight.CompareTo(two.Weight);
            }
        }

        public static List<KeyValuePair<char, TFF_Node>> Table = new List<KeyValuePair<char, TFF_Node>>();

        private static List<TFF_Node> FreeSpace = new List<TFF_Node>();

        private static void CreateTable(string str)
        {
            Table.Clear();
            FreeSpace.Clear();
            for (int i = 0; i < str.Length; i++)
            {
                bool find = false;
                for (int j = 0; j < Table.Count; j++)
                    if (Table[j].Key == str[i])
                    {
                        find = true;
                        Table[j].Value.Weight++;
                    }
                if (!find)
                {
                    var nw = new TFF_Node();
                    nw.Weight = 1;
                    FreeSpace.Add(nw);
                    Table.Add(new KeyValuePair<char, TFF_Node>(str[i], nw));
                }
            }
            FreeSpace.Sort();
        }

        private static void GrowTree()
        {
            while (FreeSpace.Count > 1)
            {
                var nw = new TFF_Node();
                nw.Left = FreeSpace[0];
                nw.Right = FreeSpace[1];
                nw.Left.Parent = nw.Right.Parent = nw;
                nw.Weight = nw.Left.Weight + nw.Right.Weight;
                nw.Left.Code = '0';
                nw.Right.Code = '1';
                FreeSpace.Remove(nw.Left);
                FreeSpace.Remove(nw.Right);
                FreeSpace.Add(nw);
                FreeSpace.Sort();
            }
            FreeSpace[0].Code = 'P';
        }
        
        public static string getCode(TFF_Node node)
        {
            return node.Code + (node.Parent.Code == 'P' ? null : getCode(node.Parent)); 
        }

        public static string Encode(string str)
        {
            CreateTable(str);
            GrowTree();
            string ret = "";
            for (int i = 0; i < str.Length; i++)
                ret += new string((getCode(Table.First(nd => nd.Key == str[i]).Value)).Reverse().ToArray());
            return ret;
        }

        public static string Decode(string str)
        {
            string ret = "";
            string buf = "";
            for (int i = 0; i < str.Length; i++)
            {
                buf += str[i];
                for (int j = 0; j < Table.Count; j++)
                    if (new string((getCode(Table[j].Value)).Reverse().ToArray()) == buf)
                    {
                        ret += Table[j].Key;
                        buf = "";
                    }
            }
            if (buf != "")
                return "Архив поврежден!";
            else
                return ret;
        }

    }
}