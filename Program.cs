using System;
using System.Windows.Forms;
using System.Text;

namespace ThesisSchedulerGUI
{

    static class Program
    {
        public static Form1 Page1;
        public static Form2 Page2;
        public static Form3 Page3;

        [STAThread]
        static void Main()
        {
            // Needed for ExcelDataReader encodings
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Page1 = new Form1();
            Page2 = new Form2();
            Page3 = new Form3(Page2);

            Application.Run(Page1);
        }
    }
}