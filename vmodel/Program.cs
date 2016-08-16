using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using RedeAppTrial;
namespace vmodel
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            TrialVer t = new TrialVer("VModel", Application.StartupPath + "\\RegFile.reg",
                Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\TMSetp.dbf",
                " ",
                2, 2, "745");

            byte[] MyOwnKey = { 97, 250, 1, 5, 84, 21, 7, 63,
            4, 54, 87, 56, 123, 10, 3, 62,
            7, 9, 20, 36, 37, 21, 101, 57};
            t.TripleDESKey = MyOwnKey;

            TrialVer.RunTypes RT = t.ShowDialog();

            if (RT == TrialVer.RunTypes.Trial)
            {

                Application.Run(new Form1());

            }
            //if (TrialVer.RunTypes == TrialVer.RunTypes.Expired)
            else
            {

                MessageBox.Show("Trail Expired. \nUninstall GsmAT");

            }
        }
    }
}
