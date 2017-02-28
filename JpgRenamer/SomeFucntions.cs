using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Diagnostics;
namespace JpgRenamer
{
    class SomeFucntions
    {
        public static void OpenFiles(string path, out string[] files)
        {
            files = Directory.GetFiles(path, "*.jpg");
        }
        public static string GetShortName(string str)
        {
            string str2 = str.Substring(str.LastIndexOf(@"\") + 1);
            return str2;
        }
        
    }
}
