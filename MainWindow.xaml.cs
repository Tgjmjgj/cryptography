using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Numerics;

namespace MD55
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int z = 1000;

        private RSA rsa;

        public MainWindow()
        {
            InitializeComponent();
            this.rsa = new RSA();
        }
        //1
        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            textBox1.Text = EMDE5.Create(textBox.Text).ToUpper();
        }
        public static string ConvertToStringHex(Byte[] b)
        {
            string ret = "";
            for (int i = 0; i < b.Length; i++)
            {
                ret += b[i].ToString("X");
            }
            return ret;
        }

        private void label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void button2_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBox1.Text != "" && textBox1.Text != "ХЕШ")
            {
                String hash = EMDE5.Create(textBox.Text).ToUpper();
                if (textBox1.Text != hash)
                    textBox1.Background = Brushes.Red;
                else
                    textBox1.Background = Brushes.White;
            }
            else
                textBox1.Background = Brushes.White;
            
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            textBox.Height = 267;
            Canvas.SetZIndex(textBox, z++);
            Canvas.SetZIndex(button, z++);
            Canvas.SetZIndex(textBox1, z++);
        }
        //2
        private void button1_Copy_Click(object sender, RoutedEventArgs e)
        {
            textBox.Height = 184;
            Canvas.SetZIndex(textBox, z++);
            Canvas.SetZIndex(button, z++);
            Canvas.SetZIndex(textBox1, z++);
            Canvas.SetZIndex(grid_rsa, z++);
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (this.rsa.Create(BigInteger.Parse(this.textBox3.Text), BigInteger.Parse(this.textBox4.Text))
                && this.textBox1.Background == Brushes.White)
            {
                this.textBox2.Text = this.rsa.Crypt(this.textBox1.Text);
                this.textBox5.Text = this.rsa.e.ToString();
            }
            else
                this.textBox2.Text = "";
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            string res = this.rsa.Encrypt(this.textBox2.Text);
            if (res == this.textBox1.Text)
                this.textBox2.Background = Brushes.LawnGreen;
            else
                this.textBox2.Background = Brushes.Red;
        }

        private void textBox5_TextChanged(object sender, TextChangedEventArgs e)
        {
            int e_tmp = (int)this.rsa.e;
            try
            {
                this.rsa.e = Convert.ToInt32(this.textBox5.Text);
            }
            catch
            {
                this.textBox5.Text = "";
                this.rsa.e = e_tmp;
            }
        }

        private void button4_LostFocus(object sender, RoutedEventArgs e)
        {
            this.textBox2.Background = Brushes.White;
        }
        //3
        private void button1_Copy2_Click(object sender, RoutedEventArgs e)
        {
            Canvas.SetZIndex(grid_andr, z++);
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int count = 0;
                if (this.textBox_N.Text == "")
                    count++;
                if (this.textBox_s.Text == "")
                    count++;
                if (this.textBox_R.Text == "")
                    count++;
                if (this.textBox_d.Text == "")
                    count++;
                if (this.textBox_M.Text == "")
                    count++;
                if (this.textBox_P.Text == "")
                    count++;
                if (count == 0)
                {
                    BigInteger lN = BigInteger.Parse(this.textBox_N.Text);
                    int s = Convert.ToInt32(this.textBox_s.Text);
                    int lR = 1000 * Convert.ToInt32(this.textBox_R.Text);
                    int E = 8 * (s + Convert.ToInt32(this.textBox_d.Text));
                    long lM = 43200 * Convert.ToInt32(this.textBox_M.Text);
                    double p = Convert.ToDouble(this.textBox_P.Text);
                    BigInteger left = BigInteger.Pow(lN, s);
                    BigInteger right = new BigInteger((43200 * lR * lM) / (E * p));
                    if (left.CompareTo(right) >= 1)
                        this.textBlock3.Text = "Неравенство выполняется";
                    else
                        this.textBlock3.Text = "Неравенство не выполняется";
                }
                else if (count == 1)
                {
                    if (this.textBox_N.Text == "")
                    {
                        double s = Convert.ToInt32(this.textBox_s.Text);
                        double r = 1024 * Convert.ToInt32(this.textBox_R.Text);
                        double E = 8 * (s + Convert.ToInt32(this.textBox_d.Text));
                        double M = 30 * 24 * 60 * 60 * Convert.ToInt32(this.textBox_M.Text);
                        double p = Convert.ToDouble(this.textBox_P.Text);
                        double N = Math.Pow((43200 * (r / E) * M / p), 1 / s);
                        this.textBlock3.Text = "N должно быть >= " + N.ToString();
                    }
                    else if (this.textBox_R.Text == "")
                    {
                        BigInteger N = BigInteger.Parse(this.textBox_N.Text);
                        int s = Convert.ToInt32(this.textBox_s.Text);
                        double E = 8 * (s + Convert.ToDouble(this.textBox_d.Text));
                        double M = 30 * 24 * 60 * 60 * Convert.ToDouble(this.textBox_M.Text);
                        double p = Convert.ToDouble(this.textBox_P.Text);
                        BigInteger r = 1024 * BigInteger.Pow(N, s) / (int)Math.Pow((E * p) / (43200 * M), -1);
                        this.textBlock3.Text = "R должно быть <= " + r.ToString();
                    }
                    else if (this.textBox_M.Text == "")
                    {
                        BigInteger N = BigInteger.Parse(this.textBox_N.Text);
                        int s = Convert.ToInt32(this.textBox_s.Text);
                        double r = 1024 * Convert.ToDouble(this.textBox_R.Text);
                        double E = 8 * (s + Convert.ToDouble(this.textBox_d.Text));
                        double p = Convert.ToDouble(this.textBox_P.Text);
                        BigInteger m = BigInteger.Pow(N, s) / (int)Math.Pow((E * p) / (43200 * r), -1);
                        this.textBlock3.Text = "M должно быть <= " + m.ToString();
                    }
                    else if (this.textBox_P.Text == "")
                    {
                        BigInteger N = BigInteger.Parse(this.textBox_N.Text);
                        int s = Convert.ToInt32(this.textBox_s.Text);
                        int M = 30 * 24 * 60 * 60 * Convert.ToInt32(this.textBox_M.Text);
                        double r = 1024 * Convert.ToDouble(this.textBox_R.Text);
                        int E = 8 * (s + Convert.ToInt32(this.textBox_d.Text));
                        BigInteger big = E * BigInteger.Pow(N, s);
                        big /= (int)(43200 * r * M);
                        double P = Math.Round(Math.Pow((int)big, -1), 6);
                        this.textBlock3.Text = "P должна быть >= " + P.ToString();
                    }
                    else
                        this.textBlock3.Text = "Заполните параметры";
                }
                else
                    this.textBlock3.Text = "Заполните параметры";
                if (this.textBox_N.Text != "" && this.textBox_s.Text != "" && this.textBox_d.Text != "" && this.textBox_R.Text != "")
                {
                    BigInteger lN = BigInteger.Parse(this.textBox_N.Text);
                    int s = Convert.ToInt32(this.textBox_s.Text);
                    int lR = 1024 * Convert.ToInt32(this.textBox_R.Text);
                    int E = 8 * (s + Convert.ToInt32(this.textBox_d.Text));
                    BigInteger left = BigInteger.Pow(lN, s);
                    BigInteger time = left * E / (2 * lR);
                    int ages = (int)(time / 31536000);
                    time %= 31536000;
                    int months = (int)(time / 2592000);
                    time %= 2592000;
                    int days = (int)(time / 86400);
                    time %= 86400;
                    int hours = (int)(time / 3600);
                    time %= 3600;
                    int minutes = (int)(time / 60);
                    time %= 60;
                    int seconds = (int)time;
                    string ret = "";
                    if (ages != 0)
                        ret += ages + " года ";
                    if (months != 0)
                        ret += months + " месяцев ";
                    if (days != 0)
                        ret += days + " дней ";
                    if (hours != 0)
                        ret += hours + " часов ";
                    if (minutes != 0)
                        ret += minutes + " минут ";
                    if (seconds != 0)
                        ret += seconds + " секунд.";
                    this.textBox_time.Text = ret;
                }
            }
            catch
            {
                this.textBlock3.Text = "Сломали!";
            }
        }
        //4
        private void button1_Copy3_Click(object sender, RoutedEventArgs e)
        {
            Canvas.SetZIndex(grid_bwt, z++);
            BWT.refWindow = this;
        }

        private void bwt_button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string str = BWT.Encode(this.bwt_textBox1.Text);
                this.bwt_textBox2.Text = "{ " + str + " }";
            }
            catch
            {
                this.bwt_listBox_a.Items.Clear();
                this.bwt_listBox_b.Items.Clear();
                this.bwt_textBox1.Clear();
                this.bwt_textBox2.Clear();
            }
        }

        private void bwt_button2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BWT.Decode(this.bwt_textBox2.Text.Substring(2, this.bwt_textBox2.Text.Length - 4));

            }
            catch
            {
                this.bwt_listBox_a.Items.Clear();
                this.bwt_listBox_b.Items.Clear();
                this.bwt_textBox1.Clear();
                this.bwt_textBox2.Clear();
            }
        }
        //5
        private void button1_Copy4_Click(object sender, RoutedEventArgs e)
        {
            Canvas.SetZIndex(grid_sf, z++);
            sf_textBlock.Text = "Shannon-Fano";
        }

        private void sf_button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string arch = SF.Encode(this.sf_textBox1.Text);
                this.sf_listBox.Items.Clear();
                for (int i = 0; i < SF.Table.Count; i++)
                    this.sf_listBox.Items.Add(SF.Table[i].ToString());
                sf_textBox_c.Text = arch;
            }
            catch
            {
                sf_listBox.Items.Clear();
                sf_textBox1.Clear();
                sf_textBox_c.Clear();
                sf_textBox_d.Clear();
                sf_textBlock.Text = "Shannon-Fano";
            }
        }

        private void sf_button2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string msg = SF.Decode(sf_textBox_c.Text);
                sf_textBox_d.Text = msg;
            }
            catch
            {
                sf_listBox.Items.Clear();
                sf_textBox1.Clear();
                sf_textBox_c.Clear();
                sf_textBox_d.Clear();
                sf_textBlock.Text = "Shannon-Fano";
            }
        }

        private void sf_textBox_c_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sf_textBox_c.Text.Length == 0)
                sf_textBlock.Text = "Shannon-Fano";
            else
                sf_textBlock.Text = Convert.ToString(sf_textBox_c.Text.Length) + " бит"; 
        }

        private void sf_textBox_d_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sf_textBox_d.Text.Length == 0)
                sf_textBlock.Text = "Shannon-Fano";
            else
                sf_textBlock.Text = Convert.ToString(sf_textBox_d.Text.Length * 8) + " бит";
        }

        private void sf_textBox_LostFocus(object sender, RoutedEventArgs e)
        {
            sf_textBlock.Text = "Shannon-Fano";
        }
        //6
        private void button1_Copy5_Click(object sender, RoutedEventArgs e)
        {
            Canvas.SetZIndex(grid_hff, z++);
            hff_textBlock.Text = "Huffman";
        }

        private void hff_button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string arch = HFF.Encode(this.hff_textBox1.Text);
                this.hff_listBox.Items.Clear();
                for (int i = 0; i < HFF.Table.Count; i++)
                    this.hff_listBox.Items.Add("\"" + HFF.Table[i].Key + "\" : " + HFF.Table[i].Value.Weight + " = " + new string((HFF.getCode(HFF.Table[i].Value)).Reverse().ToArray()));
                hff_textBox_c.Text = arch;
            }
            catch
            {
                hff_listBox.Items.Clear();
                hff_textBox1.Clear();
                hff_textBox_c.Clear();
                hff_textBox_d.Clear();
                hff_textBlock.Text = "Huffman";
            }
        }

        private void hff_button2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string msg = HFF.Decode(hff_textBox_c.Text);
                hff_textBox_d.Text = msg;
            }
            catch
            {
                hff_listBox.Items.Clear();
                hff_textBox1.Clear();
                hff_textBox_c.Clear();
                hff_textBox_d.Clear();
                hff_textBlock.Text = "Huffman";
            }
        }

        private void hff_textBox_c_GotFocus(object sender, RoutedEventArgs e)
        {
            if (hff_textBox_c.Text.Length == 0)
                hff_textBlock.Text = "Huffman";
            else
                hff_textBlock.Text = Convert.ToString(hff_textBox_c.Text.Length) + " бит";
        }

        private void hff_textBox_d_GotFocus(object sender, RoutedEventArgs e)
        {
            if (hff_textBox_d.Text.Length == 0)
                hff_textBlock.Text = "Huffman";
            else
                hff_textBlock.Text = Convert.ToString(hff_textBox_d.Text.Length * 8) + " бит";
        }

        private void hff_textBox_LostFocus(object sender, RoutedEventArgs e)
        {
            hff_textBlock.Text = "Huffman";
        }
        //7
        private void button1_Copy6_Click(object sender, RoutedEventArgs e)
        {
            Panel.SetZIndex(this.grid_lz, z++);
            this.lz_grid.Visibility = Visibility.Collapsed;
        }

        private bool mode = false;

        private void lz_button1_Click(object sender, RoutedEventArgs e)
        {
            n1 = Convert.ToInt32(lz_textBox_d.Text);
            n2 = Convert.ToInt32(lz_textBox_b.Text);
            indI = 0 - n1;
            indII = n2;
            str = this.lz_textBox1.Text;
            ret = "";
            I = Convert.ToInt32(Math.Ceiling(Math.Log10(Convert.ToInt32(lz_textBox_d.Text)))
                + Math.Ceiling(Math.Log10(Convert.ToInt32(lz_textBox_b.Text))) + 1.0);
            incr = 0;
            ptr = 0;
            this.mode = false;
            this.lz_grid.Visibility = Visibility.Visible;
        }

        private void lz_button2_Click(object sender, RoutedEventArgs e)
        {
            ret = "";
            I = Convert.ToInt32(Math.Ceiling(Math.Log10(n1)) + Math.Ceiling(Math.Log10(n2)) + 1.0);
            indI = 0 - n1;
            indII = n2;
            buf = "";
            str = this.lz_textBox2.Text;
            counter = 0;
            p = 0;
            q = 0;
            ns = Char.MinValue;
            this.mode = true;
            this.lz_grid.Visibility = Visibility.Visible;
        }

        string ret, str;
        int I;
        int indI, n1;
        int indII, n2;
        string buf;
        int incr;
        int ptr;

        int counter = 0;
        int p = 0, q = 0;
        char ns = Char.MinValue;

        public bool Transform_step()
        {
            try
            {
                if (n2 < n1)
                    throw new ArgumentException();
                bool search = false;
                while (indII < str.Length + n2 && !search)
                {
                    if (indII + incr - n2 < str.Length)
                        buf += str[indII + incr - n2];
                    else
                        buf += Char.MinValue;
                    bool local_search = false;
                    for (int i = (indI < 0 ? 0 : indI); i < indII - n2; i++)
                    {
                        for (int j = 0; j < buf.Length; j++)
                            if (str[i + j] != buf[j] || i + j >= indII - n2)
                                break;
                            else if (j == buf.Length - 1)
                            {
                                ptr = i - indI;
                                local_search = true;
                            }
                        if (local_search)
                            break;
                    }
                    incr++;
                    if (!local_search)
                    {
                        search = true;
                        ret += "{" + ptr + "," + (buf.Length - 1) + "," + buf[buf.Length - 1] + "} ";
                        buf = "";
                        indI += incr;
                        indII += incr;
                        incr = ptr = 0;
                    }
                }
                this.lz_textBox2.Text = ret;
                if (indII < str.Length + n2)
                    return true;
                else
                    return false;
            }
            catch
            {
                this.lz_textBox2.Text = "Данные повреждены.";
                this.lz_grid.Visibility = Visibility.Collapsed;
                return false;
            }
        }

        public bool Recovery_step()
        {
            try
            {
                if (n2 < n1)
                    throw new ArgumentException();
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
                this.lz_textBox2.Text = this.lz_textBox2.Text.Remove(0, 8);
                for (int i = indI + p; i < indI + p + q; i++)
                    ret += ret[i];
                if (ns != Char.MinValue)
                    ret += ns;
                indI += q + 1;
                indII += q + 1;
                this.lz_textBox3.Text = ret;
                if (counter < str.Length - 1)
                    return true;
                else
                    return false;
            }
            catch
            {
                this.lz_textBox3.Text = "Данные повреждены.";
                this.lz_grid.Visibility = Visibility.Collapsed;
                return false;
            }
        }

        private void lz_step_Click(object sender, RoutedEventArgs e)
        {
            if (!mode)
            {
                if (!Transform_step())
                    this.lz_grid.Visibility = Visibility.Collapsed;
                string tmp = str.Substring((indI < 0 ? 0 : indI), indII - n2 - (indI < 0 ? 0 : indI));
                while (tmp.Length != n1)
                    tmp = '0' + tmp;
                tmp += '|';
                string buffer = str.Substring(indII - n2, (indII < str.Length ? n2 : n2 - (indII - str.Length)));
                while (buffer.Length != n2)
                    buffer += '0';
                this.lz_window.Text = tmp + buffer;
            }
            else
            {
                if (!Recovery_step())
                    this.lz_grid.Visibility = Visibility.Collapsed;
                string tmp = ret.Substring((indI < 0 ? 0 : indI), indII - n2 - (indI < 0 ? 0 : indI));
                while (tmp.Length != n1)
                    tmp = '0' + tmp;
                tmp += '|';
                string buffer = ret.Substring(indII - n2, (indII < ret.Length ? n2 : n2 - (indII - ret.Length)));
                while (buffer.Length != n2)
                    buffer += '0';
                this.lz_window.Text = tmp + buffer;
            }
        }

        private void lz_all_Click(object sender, RoutedEventArgs e)
        {
            if (!mode)
                while (Transform_step()) ;
            else
                while (Recovery_step()) ;
            this.lz_grid.Visibility = Visibility.Collapsed;
        }
    }
}
