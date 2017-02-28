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

    public partial class JpgRenamer : Form
    {
        public JpgRenamer()
        {
            InitializeComponent();
            listBox1.Visible = false;
            listBox2.Visible = false;

        }

        string folderPath = "";
        string[] FilePaths;
        string excPath = "";
        static bool t;     
        //This is a Browse Button 
        private void browse_button_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog ofd = new FolderBrowserDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                listBox1.Items.Clear();
                folderPath = ofd.SelectedPath;
                SomeFucntions.OpenFiles(folderPath, out FilePaths);
                foreach (string i in FilePaths)
                {
                    listBox1.Items.Add(SomeFucntions.GetShortName(i));
                }
                listBox1.Visible = true;
            }            
        }
        private void choose_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog exc = new OpenFileDialog();
            exc.Filter = "All Excel books Types|*.xlsx;*.xls";
            if (FilePaths != null)
            {
                if (exc.ShowDialog() == DialogResult.OK)
                {
                    listBox2.Items.Clear();
                    excPath = exc.FileName;
                    listBox2.Visible = true;
                    Excel.Application xlApp = new Excel.Application();
                    Excel.Workbook xlBook = xlApp.Workbooks.Open(excPath, 0, true, 5, "", "", true, Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                    Excel.Worksheet xlWorkSheet = (Excel.Worksheet)xlBook.Worksheets.get_Item(1);
                    Excel.Range range = xlWorkSheet.UsedRange;
                    int rw = range.Rows.Count;
                    if (rw < FilePaths.Length)
                    { MessageBox.Show("Names is less than .jpg files.Select another excel file"); }
                    else
                    {
                        t = true;
                        for (int rCnt = 1; rCnt <= FilePaths.Length; rCnt++)
                        {
                            string str = Convert.ToString((range.Cells[rCnt, 1] as Excel.Range).Value2);
                            listBox2.Items.Add(str + ".jpg");
                            
                        }
                    }
                }
                
            }
            else { MessageBox.Show("Please first select image folder"); }

        }      
        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Insert && t)
            {
                int index = listBox1.SelectedIndex;
                string str = listBox2.Items[index].ToString();
                FileInfo file = new FileInfo(FilePaths[index]);
                if (!(listBox1.Items.Contains(listBox2.Items[index])))
                {
                    file.MoveTo(folderPath + "\\" + str);
                    listBox1.Items[index] = str;
                    FilePaths[index] = folderPath + "\\" + str;
                    if (listBox1.SelectedIndex != FilePaths.Length - 1)
                        listBox1.SelectedIndex++;
                }
                else { MessageBox.Show("This name is already exist or Renamed"); }
            }
        }

        private void to_pdf_button_Click(object sender, EventArgs e)
        {
            string pdfFileName = "";
            SaveFileDialog pdf = new SaveFileDialog();
            pdf.Filter = "Adobe PDF|*.pdf";
            if (FilePaths != null)
            {
                if (pdf.ShowDialog() == DialogResult.OK)
                {
                    pdfFileName = pdf.FileName;
                    using (var document = new PdfDocument())
                    {
                        for (int i = 0; i < FilePaths.Length; i++)
                        {
                            Bitmap pic = new Bitmap(FilePaths[i]);
                            PdfPage page = document.AddPage();
                            using (XImage img = XImage.FromFile(FilePaths[i]))
                            {
                                page.Width = pic.Width;
                                page.Height = pic.Height;
                                XGraphics gfx = XGraphics.FromPdfPage(page);
                                gfx.DrawImage(img, 0, 0, pic.Width, pic.Height);
                            }
                        }
                        document.Save(pdfFileName);
                        Process.Start(pdfFileName);
                    }
                }
               
                
            }
            else { MessageBox.Show("Please select image folder."); }

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.Show();
        }
    }
}

