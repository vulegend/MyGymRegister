using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGymRegister
{
    public static class Extensions
    {
        public static bool IsDigitsOnly(this string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                {
                    if(c != '+')
                        return false;
                }
            }

            return true;
        }

        public static Bitmap CropAtRect(this Bitmap b, Rectangle r)
        {
            int x = (b.Width / 2) - (350 / 2);
            int y = (b.Height / 2) - (350 / 2);
            Bitmap nb = new Bitmap(350, 350);
            Graphics g = Graphics.FromImage(nb);
            g.DrawImage(b, -x, -y, 350, 350);
            return nb;
        }

        public static string Between(this string Text, string FirstString, string LastString)

        {

            string STR = Text;

            string STRFirst = FirstString;

            string STRLast = LastString;

            string FinalString;

            string TempString;



            int Pos1 = STR.IndexOf(FirstString) + FirstString.Length;

            int Pos2 = STR.IndexOf(LastString);

            FinalString = STR.Substring(Pos1, Pos2 - Pos1);

            return FinalString;
        }
    }
}
