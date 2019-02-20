using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ln
{
    class BigNumber
    {
        public int positive;//判断数字的正负
        public int[] value = new int[60];
        /// <summary>
        /// 构造函数
        /// </summary>
        public BigNumber()
        {
            positive = 1;
            for (int i = 0; i < 60; i++)
            {
                value[i] = 0;
            }
        }
        /// <summary>
        /// int型数据转换为大数类
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static BigNumber Trans(int a)
        {
            BigNumber IntToBn = new BigNumber();
            if (a < 0)
            {
                IntToBn.positive = -1;
                a = -a;
            }
            else
                IntToBn.positive = 1;
            int i;
            IntToBn.value[19] = a % 10;
            for (i = 1; i <= 19; i++)
            {
                a = a / 10;
                IntToBn.value[19 - i] = a % 10;
            }
            return IntToBn;
        }
        /// <summary>
        /// 大数类转换为int型
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static int Trans(BigNumber a)
        {
            int BnToInt = 0;
            for (int i = 0; 19 - i >= 0; i++)
                BnToInt += (int)(Math.Pow(10.0, i)) * a.value[19 - i];
            if (a.positive == -1)
                BnToInt = -BnToInt;
            return BnToInt;
        }
        /// <summary>
        /// double型转换为大数类
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static BigNumber Trans(double a)
        {
            BigNumber DoubleToBn = new BigNumber();
            if (a < 0)
            {
                DoubleToBn.positive = -1;
                a = -a;
            }
            else
                DoubleToBn.positive = 1;
            int i, t;
            int s = (int)a;
            a = a - s;
            for (i = 1; i < 21; i++)
            {
                a *= 10;
                t = (int)a;
                DoubleToBn.value[19 + i] = t % 10;
                a -= t;
            }
            DoubleToBn = DoubleToBn + Trans(s);
            return DoubleToBn;
        }
        /// <summary>
        /// 复制函数
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static BigNumber copy(BigNumber a)
        {
            BigNumber copy = new BigNumber();
            copy.positive = a.positive;
            for (int i = 0; i < 60; i++)
                copy.value[i] = a.value[i];
            return copy;
        }
        /// <summary>
        /// >重载
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator >(BigNumber a, BigNumber b)
        {
            if((a.positive==-1)&&(b.positive==1))
                return false;
            else if((a.positive==1)&&(b.positive==-1))
                return true;
            else
            {
                for (int i = 0; i < 60; i++)
                {
                    if (a.value[i] > b.value[i])
                        return true;
                    else if (a.value[i] < b.value[i])
                        return false;
                    else
                        continue;
                }
                return false;
            }            
        }
        /// <summary>
        /// 重载<
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator <(BigNumber a, BigNumber b)
        {
            return (b > a);
        }
        /// <summary>
        /// +重载:两个高精度数相加
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static BigNumber operator +(BigNumber a, BigNumber b)
        {
            BigNumber c = new BigNumber();
            int flag = a.positive * b.positive;
            if (flag == 1)
            {
                c.positive = a.positive;
                int carry = 0;
                for (int i = 59; i >= 0; i--)
                {
                    c.value[i] = a.value[i] + b.value[i] + carry;
                    if (c.value[i] > 9)
                    {
                        c.value[i] = c.value[i] - 10;
                        carry = 1;
                    }
                    else
                        carry = 0;
                }
            }
            else
            {
                if (abs(a) > abs(b))
                {
                    c.positive = a.positive;
                    int borrow = 0;
                    for (int i = 59; i >= 0; i--)
                    {
                        c.value[i] = a.value[i] - b.value[i] - borrow;
                        if (c.value[i] >= 0)
                            borrow = 0;
                        else
                        {
                            c.value[i] += 10;
                            borrow = 1;
                        }
                    }
                }
                else
                {
                    c.positive = b.positive;
                    int borrow = 0;
                    for (int i = 59; i >= 0; i--)
                    {
                        c.value[i] = b.value[i] - a.value[i] - borrow;
                        if (c.value[i] >= 0)
                            borrow = 0;
                        else
                        {
                            c.value[i] += 10;
                            borrow = 1;
                        }
                    }
                }
            }
            if (c.zero(c))
                c.positive = 1;
            return c;
        }
        /// <summary>
        /// 判断是否为0
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public bool zero(BigNumber a)
        {
            for (int i = 0; i <= 59; i++)
                if (a.value[i] != 0)
                    return false;
            return true;
        }
        /// <summary>
        /// -重载:两个高精度数相减
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static BigNumber operator -(BigNumber a, BigNumber b)
        {
            BigNumber c = invert(b);
            return (a + c);
        }
        /// <summary>
        /// +重载：一个高精度数加一个int型
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static BigNumber operator +(BigNumber a, int b)
        {
            BigNumber c = Trans(b);
            return (a + c);
        }
        /// <summary>
        /// +重载：一个int型加一个高精度型
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static BigNumber operator +(int a, BigNumber b)
        {
            BigNumber c = Trans(a);
            return (c + b);
        }
        /// <summary>
        /// +重载：一个高精度数加一个double型
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static BigNumber operator +(BigNumber a, double b)
        {
            BigNumber c = Trans(b);
            return (a + c);
        }
        /// <summary>
        /// +重载：一个double型加一个高精度数
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static BigNumber operator +(double a, BigNumber b)
        {
            BigNumber c = Trans(a);
            return (b + c);
        }
        /// <summary>
        /// -重载：一个高精度数减一个int型
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static BigNumber operator -(BigNumber a, int b)
        {
            BigNumber c = Trans(b);
            BigNumber result = new BigNumber();
            result = a - c;
            return result; ;
        }
        /// <summary>
        /// -重载：一个高精度数减一个double型
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static BigNumber operator -(BigNumber a, double b)
        {
            BigNumber c = Trans(b);
            return (a - c);
        }
        /// <summary>
        /// -重载：一个double型减一个高精度数
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static BigNumber operator -(double a, BigNumber b)
        {
            BigNumber c = Trans(a);
            return (c - b);
        }
        /// <summary>
        /// -重载：一个int型减一个高精度数
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static BigNumber operator -(int a, BigNumber b)
        {
            BigNumber c = Trans(a);
            return (c - b);
        }
        /// <summary>
        /// *重载：一个高精度数乘一个int型
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static BigNumber operator *(BigNumber a, int b)
        {
            BigNumber c = new BigNumber();
            int b_positive = 1;
            int b1 = b;
            int carry = 0;
            int temp = 0;
            if (b1 == 0) return c;
            else if (b1 < 0)
            {
                b_positive = -1;
                b1 = -b;
            }
            c.positive = a.positive * b_positive;
            for (int i = 59; i >= 0; i--)
            {
                temp = a.value[i] * b1 + carry;
                c.value[i] = temp % 10;
                carry = temp / 10;
            }
            return c;
        }
        /// <summary>
        /// *重载：两个高精度数相乘
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static BigNumber operator *(BigNumber a, BigNumber b)
        {
            BigNumber c = new BigNumber();
            if (a.zero(a) || b.zero(b)) return c;
            int positive = a.positive * b.positive;
            a.positive = 1;
            b.positive = 1;
            BigNumber d = copy(b);
            int i, j;
            for (i = 59; i >= 0; i--)
            {
                if (i >= 20)                       //处理小数部分
                {
                    for (j = 0; j < i - 19; j++)
                        d.value[j] = 0;
                    for (j = i - 19; j < 60; j++)
                        d.value[j] = b.value[j - i + 19];
                    c = c + (d * a.value[i]);
                }
                else                                //处理整数部分
                {
                    for (j = 0; j < 60 - 19 + i; j++) d.value[j] = b.value[j + 19 - i];
                    c = c + (d * a.value[i]);
                }
            }
            c.positive = positive;
            if (c.zero(c))
                c.positive = 1;
            return c;
        }
        /// <summary>
        /// *重载：一个int型乘一个高精度数
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static BigNumber operator *(int a, BigNumber b)
        {
            BigNumber c = Trans(a);
            return (b * c);
        }
        /// <summary>
        /// *重载：一个高精度数乘一个double型
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static BigNumber operator *(BigNumber a, double b)
        {
            BigNumber c = Trans(b);
            return (a * c);
        }
        /// <summary>
        /// *重载：一个double型乘一个高精度数
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static BigNumber operator *(double a, BigNumber b)
        {
            BigNumber c = Trans(a);
            return (b * c);
        }
        /// <summary>
        /// /重载：一个高精度数除以一个int型的数
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static BigNumber operator /(BigNumber a, int b)
        {
            BigNumber c = new BigNumber();
            BigNumber a1 = copy(a);
            int b1 = b;
            int b_positive;
            if (b >= 0) b_positive = 1;
            else
            {
                b_positive = -1;
                b1 = -b;
            }
            c.positive = a.positive * b_positive;
            if (b1 == 1) c = a1;
            else if (b1 == -1) c = (-1) * a1;
            else
            {
                int remain = 0;
                int temp = 0;
                for (int i = 0; i <= 59; i++)
                {
                    temp = a1.value[i] + remain;
                    c.value[i] = temp / b1;
                    remain = (temp % b1) * 10;
                }
            }
            return c;
        }
        /// <summary>
        /// /重载：一个高精度数除以一个高精度数
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static BigNumber operator /(BigNumber a, BigNumber b)
        {
            BigNumber c = new BigNumber();
            BigNumber temp = new BigNumber();
            if (Comparer(a,Trans(0)))
                return c;
            c.positive = a.positive * b.positive;
            a.positive = 1;
            b.positive = 1;
            temp = a;
            int times = 0, add = 0;
            int i, t, s, p;
            for (s = 0; a.value[s] == 0; s++) ;
            for (t = 0; b.value[t] == 0; t++) ;
            if (t - s > 0)
            {
                times = t - s;
                for (i = 59 - times; i >= 0; i--)
                    temp.value[i + times] = temp.value[i];
                for (i = 0; i < times; i++)
                    temp.value[i] = 0;
            }
            p = 19 - t;
            for (i = t; p + i < 60; i++)
            {
                int k = 9; 
                while ((temp - b * k).positive == -1 && k >= 0) k--;
                c.value[p + i] = k;
                temp = (temp - b * k) * 10;
            }
            c = (c + Trans(add)) * Math.Pow(10.0, times);
            if (c.zero(c))
                c.positive = 1;
            return c;
        }
        /// <summary>
        /// /重载：一个int型除以一个高精度数
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static BigNumber operator /(int a, BigNumber b)
        {
            BigNumber c = Trans(a);
            return (c / b);
        }
        /// <summary>
        /// /重载：一个高精度数除以一个double型的数
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
         public static BigNumber operator /(BigNumber a, double b)
        {
            BigNumber c = Trans(b);
            return (a / c);
        }
        /// <summary>
        /// /重载：一个double型的数除以一个高精度数
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static BigNumber operator /(double a, BigNumber b)
        {
            BigNumber c = Trans(a);
            return (c / b);
        }
        /// <summary>
        /// 重载++
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static BigNumber operator ++(BigNumber a)
        {
            return (a + 1);
        }
        /// <summary>
        /// 把string转化为高精度数
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public BigNumber StringToBn(string a)
        {
            BigNumber b = new BigNumber();
            bool Decimal = false;
            int i, dot = 0;
            for (i = 0; i < a.Length; i++)
            {
                if (a[i] == '.')
                {
                    dot = i;
                    Decimal = true;
                    break;
                }
            }
            if (a[0] == '-')
            {
                b.positive = -1;
                if (Decimal)
                {
                    for (i = dot - 1; i > 0; i--)
                        b.value[i + 20 - dot] = a[i] - 48;
                    for (i = dot + 1; i < a.Length; i++)
                        b.value[i + 19 - dot] = a[i] - 48;
                }
                else
                {
                    for (i = 1; i < a.Length; i++)
                        b.value[20 + i - a.Length] = a[i] - 48;
                }
                return b;
            }
            else
            {
                b.positive = 1;
                if (Decimal)
                {
                    for (i = dot - 1; i >= 0; i--)
                        b.value[i + 20 - dot] = a[i] - 48;
                    for (i = dot + 1; i < a.Length; i++)
                        b.value[i + 19 - dot] = a[i] - 48;
                }
                else
                {
                    for (i = 0; i < a.Length; i++)
                        b.value[20 + i - a.Length] = a[i] - 48;
                }
                return b;
            }
        }
        /// <summary>
        /// 把高精度数转化为string类型
        /// </summary>
        /// <returns></returns>
        public string BnToString()
        {
            string result = string.Empty;
            if (positive == -1)
                result += '-';
            int i;
            for (i = 0; i < 20; i++)
                result += Convert.ToString(value[i]);
            result += '.';
            for (i = 20; i < 60; i++)
                result += Convert.ToString(value[i]);
            return result;
        }
        /// <summary>
        /// 得到最终输出的数字
        /// </summary>
        /// <returns></returns>
        public string output()
        {
            string a = BnToString();
            string result = string.Empty;
            int i;
            if (a[0] == '-')
            {
                if (a[1] != '0')
                {
                    for (i = 0; i < 42; i++)
                        result += a[i];
                    return result;
                }
                else
                {
                    int start = 21;
                    for (i = 1; i < 21; i++)
                    {
                        if (a[i] != '0')
                        {
                            start = i;
                            break;
                        }
                    }
                    if (start < 21)//说明整数部分不为0
                    {
                        result += '-';
                        for (i = start; i < 42; i++)
                            result += a[i];
                        return result;
                    }
                    else
                    {
                        result += '-';
                        for (i = 20; i < 42; i++)
                            result += a[i];
                        return result;
                    }
                }
            }
            else
            {
                if (a[0] != '0')
                {
                    for (i = 0; i < 41; i++)
                        result += a[i];
                    return result;
                }
                else
                {
                    int start = 20;
                    for (i = 0; i < 20; i++)
                    {
                        if (a[i] != '0')
                        {
                            start = i;
                            break;
                        }
                    }
                    if (start < 20)//说明整数部分不为0
                    {
                        for (i = start; i < 41; i++)
                            result += a[i];
                        return result;
                    }
                    else
                    {
                        for (i = 19; i < 41; i++)
                            result += a[i];
                        return result;
                    }
                }
            }
        }

        /// <summary>
        /// 取反函数
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static BigNumber invert(BigNumber a)
        {
            BigNumber b = copy(a);
            if (a.positive == 1)
                b.positive = -1;
            else b.positive = 1;
            return b;
        }

        /// 取绝对值函数
        /// 
        public static BigNumber abs(BigNumber a)
        {
            BigNumber b = copy(a);
            b.positive = 1;
            return b;
        }
        /// <summary>
        /// 判断两个高精度数是否相等
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Comparer(BigNumber a, BigNumber b)
        {
            if (a.positive * b.positive == -1)
                return false;
            else
            {
                for (int i = 0; i < 60; i++)
                    if (a.value[i] != b.value[i])
                        return false;
                return true;
            }
        }
        /// <summary>
        /// 设置符号的函数
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static void set(BigNumber a, bool b)
        {
            if (b) 
                a.positive = 1;
            else 
                a.positive = -1;
        }
        /// <summary>
        /// 求平方
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static BigNumber square(BigNumber a)
        {
            return copy(a * a);
        }
        /// <summary>
        /// 减半Taylor
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public BigNumber Taylor(BigNumber a)
        {
            int i = 0, count = 0;
            BigNumber two = new BigNumber();
            //Trans(2);
            two.value[19] = 2;
            BigNumber ln2 = new BigNumber();
            //0.69314 71805 59945 30941 72321 21458 17656 80718
            //  20 24 25 29 30 34 35 39 40 44 45 49 50 54 55 59
            ln2.value[23] = ln2.value[26] = ln2.value[39] = ln2.value[44] = ln2.value[46] = ln2.value[50] = ln2.value[58] = 1;
            ln2.value[41] = ln2.value[43] = ln2.value[45] = 2;
            ln2.value[22] = ln2.value[35] = ln2.value[42] = 3;
            ln2.value[24] = ln2.value[33] = ln2.value[38] = ln2.value[47] = 4;
            ln2.value[29] = ln2.value[30] = ln2.value[34] = ln2.value[48] = ln2.value[53] = 5;
            ln2.value[20] = ln2.value[52] = ln2.value[54] = 6;
            ln2.value[25] = ln2.value[40] = ln2.value[51] = ln2.value[57] = 7;
            ln2.value[27] = ln2.value[49] = ln2.value[55] = ln2.value[59] = 8;
            ln2.value[21] = ln2.value[31] = ln2.value[32] = ln2.value[37] = 9;
            if (!(a < two))
            {
                do
                {
                    a = a / 2;
                    i++;
                } while (!(a < two));
            }
            a = a - 1;
            BigNumber x = copy(a);
            BigNumber lnx = new BigNumber();
            BigNumber temp1 = new BigNumber();
            int n = 1;
            BigNumber temp = x / n;
            do
            {                
                lnx = lnx + temp;
                count++;
                temp1 = x * a;
                x = invert(temp1);
                n++;
                temp = x / n;
            } while (!(temp.zero(temp)));
            BigNumber result = new BigNumber();
            result = lnx + (ln2 * i);
            return result;
        }
        /// <summary>
        /// 换元Taylor
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public BigNumber Taylor_Former(BigNumber a)
        {
            int count = 0;
            BigNumber temp1, temp2;
            temp1 = a - 1;
            temp2 = a + 1;
            BigNumber u = new BigNumber();
            u = temp1 / temp2;
            BigNumber an = u;
            BigNumber lna = new BigNumber();
            int n = 1;
            do
            {
                lna = lna + (an / n) * 2;
                an = an * u * u;
                count++;
                n += 2;
            } while (!((an * 2) / (1 - an * an) / n).zero((an * 2) / (1 - an * an) / n));
            return lna;
        }
        /// <summary>
        /// 经典四阶R-K算法
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public BigNumber RK(BigNumber a)
        {
            int i = 0;
            BigNumber two = new BigNumber();
            two.value[19] = 2;
            if (!(a < two))
            {
                do
                {
                    a = a / 2;
                    i++;
                } while (!(a < two));
            }
            BigNumber an = a;
            BigNumber yn = new BigNumber();
            BigNumber xn = new BigNumber();
            //Trans(1);
            xn.value[19] = 1;
            BigNumber one = new BigNumber();
            one.value[19] = 1;
            BigNumber h = new BigNumber();
            //Trans(0.000005);
            h.value[25] = 5;
            BigNumber ln2 = new BigNumber();
            //0.69314 71805 59945 30941 72321 21458 17656 80718
            //  20 24 25 29 30 34 35 39 40 44 45 49 50 54 55 59
            ln2.value[23] = ln2.value[26] = ln2.value[39] = ln2.value[44] = ln2.value[46] = ln2.value[50] = ln2.value[58] = 1;
            ln2.value[41] = ln2.value[43] = ln2.value[45] = 2;
            ln2.value[22] = ln2.value[35] = ln2.value[42] = 3;
            ln2.value[24] = ln2.value[33] = ln2.value[38] = ln2.value[47] = 4;
            ln2.value[29] = ln2.value[30] = ln2.value[34] = ln2.value[48] = ln2.value[53] = 5;
            ln2.value[20] = ln2.value[52] = ln2.value[54] = 6;
            ln2.value[25] = ln2.value[40] = ln2.value[51] = ln2.value[57] = 7;
            ln2.value[27] = ln2.value[49] = ln2.value[55] = ln2.value[59] = 8;
            ln2.value[21] = ln2.value[31] = ln2.value[32] = ln2.value[37] = 9;
            if (a > one)
            {
                while (xn < an)
                {
                    yn += ((((1 / xn) + (4 / (xn + (h / 2))) + (1 / (xn + h))) * h) / 6);
                    xn += h;
                }
            }
            else
            {
                bool flag = false;
                set(h, flag);
                while (xn > an)
                {
                    yn = yn + ((((1 / xn) + (4 / (xn + (h / 2))) + (1 / (xn + h))) * h) / 6);
                    xn += h;
                }
            }
            BigNumber result = new BigNumber();
            result = yn + (ln2 * i);
            return result;
        }
        /// <summary>
        /// e^x高精度运算
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public BigNumber exp(BigNumber a)
        {
            BigNumber e = new BigNumber();
            int i;
            for (i = 0; i < 18; i++)
                e.value[i] = 0;
            e.value[32] = e.value[40] = 0;
            e.value[21] = e.value[25] = e.value[46] = 1;
            e.value[19] = e.value[23] = e.value[27] = e.value[35] = e.value[41] = e.value[49] = e.value[52] = e.value[59] = 2;
            e.value[36] = e.value[38] = e.value[47] = 3;
            e.value[29] = e.value[33] = e.value[44] = e.value[53] = 4;
            e.value[30] = e.value[34] = e.value[37] = e.value[48] = e.value[57] = 5;
            e.value[39] = e.value[50] = e.value[51] = 6;
            e.value[20] = e.value[43] = e.value[45] = e.value[55] = e.value[56] = e.value[58] = 7;
            e.value[22] = e.value[24] = e.value[26] = e.value[28] = e.value[42] = 8;
            e.value[31] = e.value[54] = 9;
            //Trans(2.7182818284590452353602874713526624977572);
            BigNumber Integer = new BigNumber();
            BigNumber Decimal = new BigNumber();
            for (i = 0; i < 20; i++)
            {
                Integer.value[i] = a.value[i];
            }
            for (i = 20; i < 60; i++)
            {
                Decimal.value[i] = a.value[i];
            }
            BigNumber temp = Trans(1);
            BigNumber temp1 = copy(Decimal);
            BigNumber result = Trans(1);
            i = 1;

            if (a.zero(a))
                return Trans(1);
            else if (a.positive == 1)
            {
                while (!Integer.zero(Integer))
                {
                    temp = temp * e;
                    Integer = Integer - 1;
                }
                while (!temp1.zero(temp1))
                {
                    result = result + temp1;
                    i++;
                    temp1 = temp1 * Decimal / i;
                }
                result = result * temp;
                return result;
            }
            else
            {
                while (!Integer.zero(Integer))
                {
                    temp = temp * e;
                    Integer = Integer - 1;
                }
                while (!temp1.zero(temp1))
                {
                    result = result + temp1;
                    i++;
                    temp1 = temp1 * Decimal / i;
                }
                result = 1 / (temp * result);
                return result;
            }
        }
        /// <summary>
        /// 牛顿迭代法
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public BigNumber Newton(BigNumber a)
        {
            BigNumber result = new BigNumber() ;
            BigNumber estimate = new BigNumber();
            BigNumber inv = new BigNumber();
            //BigNumber temp = new BigNumber();
            BigNumber minor = new BigNumber();
            minor.value[55] = 1;
            int i = 0;
            estimate = a * exp(inv) - 1;
            while (abs(estimate) > minor)
            {
                result = result + estimate;
                i++;
                inv = invert(result);
                estimate = a * exp(inv) - 1;
            }
            return result;
        }
        /// <summary>
        /// 迭代开根号
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public BigNumber sqrt(BigNumber a)
        {
            BigNumber one = new BigNumber();
            one.value[19] = 1;
            BigNumber x1 = new BigNumber();
            BigNumber x2 = new BigNumber();
            if (a.zero(a))
                return a;
            else
            {
                if (a > one)
                    x2 = a / 2;
                else
                    x2 = a * 2;
                do
                {
                    x1 = copy(x2);
                    x2 = (x1 + (a / x1)) / 2;
                } while (!(x2 - x1).zero(x2 - x1));
                for (int i = 0; i < 60; i++)
                {
                    if (x2.value[i] > 9)
                    {
                        x2.value[i] -= 10;
                        x2.value[i + 1] += 1;
                    }
                }
                return x2;
            }
        }
        /// <summary>
        /// 复化梯形公式
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public BigNumber Trapezoid(BigNumber a)
        {
            BigNumber tenth = new BigNumber();
            tenth.value[9] = 1;
            BigNumber one = new BigNumber();
            one.value[19] = 1;
            BigNumber zero = new BigNumber();
            BigNumber n = new BigNumber();
            n = tenth * (a - one) * sqrt(5 * (a - one) / 3);
            //n.value[19] += 2;
            int i;
            for (i = 20; i < 60; i++)
                n.value[i] = 0;
            BigNumber h = new BigNumber();
            h = abs(a - 1) / (n - 1);
            BigNumber k = new BigNumber();
            BigNumber result = new BigNumber();
            result = result + one + 1 / a;
            if (Comparer(a, one))
                return zero;
            else if (a > one)
            {
                for (k = one; k < n - one; k++)
                    result += 2 / (1 + k * h);
            }
            result = result * h / 2;
            return result;
        }

    }
}
