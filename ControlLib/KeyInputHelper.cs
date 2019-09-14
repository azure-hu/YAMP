using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Azure.LibCollection.CS
{
    public static class KeyPressHelper
    {
        public static void checkANumWithSep(KeyPressEventArgs e, char[] Separators)
        {
            bool SepFound = checkSeparators(e, Separators);
            
            if (!SepFound)
            {
                checkAlphaNum(e);
            }
        }

        public static void checkAlphaNum(KeyPressEventArgs e)
        {
            if (!Char.IsLetterOrDigit(e.KeyChar))
            {
                if (!Char.IsControl(e.KeyChar))
                    e.Handled = true;
            }
        }

        public static void checkNumOrSignChar(KeyPressEventArgs e)
        {
            char[] Signs = { '-', '+' };
            checkNumWithSep(e, Signs);

        }

        public static void checkNumWithSep(KeyPressEventArgs e, char[] Separators)
        {
            bool SepFound = checkSeparators(e, Separators);

            if (!SepFound)
            {
                checkNumeric(e);
            }   
        }

        public static void checkAlphaWithSep(KeyPressEventArgs e, char[] Separators)
        {
            bool SepFound = checkSeparators(e, Separators);

            if (!SepFound)
            {
                checkAlpha(e);       
            }
        }

        public static void checkNumeric(KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar))
            {
                if (!Char.IsControl(e.KeyChar))
                {
                        e.Handled = true;
                }
            }
        }

        public static void checkAlpha(KeyPressEventArgs e)
        {
            if (!Char.IsLetter(e.KeyChar))
            {
                if (!Char.IsControl(e.KeyChar))
                {
                        e.Handled = true;
                }
            }
        }

        private static bool checkSeparators(KeyPressEventArgs e, char[] seps)
        {
            bool isSeparator = false;
            foreach (char c in seps)
            {
                if (e.KeyChar == c)
                {
                    isSeparator = true;
                    break;
                }
            }
            return isSeparator;
        }


    }
}
