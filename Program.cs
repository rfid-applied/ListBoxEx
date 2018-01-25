using System;

using System.Collections.Generic;
using System.Windows.Forms;

namespace ListBoxExSample
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
#if PocketPC
        [MTAThread]
#else
        [STAThread]
#endif
        static void Main()
        {
            Form form;
#if PocketPC
            form = new Form3();
#else
            form = new Form2();
#endif
            Application.Run(form);
        }
    }
}