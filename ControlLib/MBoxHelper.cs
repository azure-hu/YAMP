using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace libIsh
{
    public static class MBoxHelper
    {
        public static void ShowErrorMsg(string Message, string ErrorStr)
        {
            MessageBox.Show(Message, ErrorStr,
                MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
        }

        public static void ShowErrorMsg(Exception ex, string ErrorStr)
        {
            ShowErrorMsg(ex.Message, ErrorStr);
        }


        public static void Whoops(string Message)
        {
            ShowInfoMsg(Message, "Whoops!");
        }

        public static void ShowWarnMsg(string Message, string WarnStr)
        {
            MessageBox.Show(Message, WarnStr,
                MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
        }


        public static void ShowWarnMsg(Exception ex, string WarnStr)
        {
            ShowWarnMsg(ex.Message, WarnStr);
        }

        public static void ShowInfoMsg(string Message, string InfoStr)
        {
            MessageBox.Show(Message, InfoStr,
                MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        public static DialogResult ShowAskMsg(string Ask, string Caption)
        {
            return MessageBox.Show(Ask,Caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

    }
}
