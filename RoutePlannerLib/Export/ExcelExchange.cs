using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Export
{
    public class ExcelExchange
    {
        public void WriteToFile(string _fileName, IEnumerable<Link> _links)
        {
            var excel = new Application();
            excel.DisplayAlerts = false;
            var workbook = excel.Workbooks.Add();
            var worksheet = workbook.Worksheets[1];

            //set format for tiltle range
            Range titles;
            titles = worksheet.Range["A1", "D1"];
            titles.EntireColumn.ColumnWidth = 25;
            titles.Font.Size = 14;
            titles.Font.Bold = true;
            titles.BorderAround2(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium);

            //write titles
            worksheet.Cells[1, 1] = "From";
            worksheet.Cells[1, 2] = "To";
            worksheet.Cells[1, 3] = "Distance";
            worksheet.Cells[1, 4] = "Mode";

            int row = 2;
            foreach (var l in _links)
            {
                worksheet.Cells[row, 1] = l.FromCity.Name + " (" + l.FromCity.Country + ")";
                worksheet.Cells[row, 2] = l.ToCity.Name + " (" + l.ToCity.Country + ")";
                worksheet.Cells[row, 3] = l.Distance;
                worksheet.Cells[row, 4] = Enum.GetName(typeof(TransportMode), l.TransportMode);
                row++;
            }

            workbook.SaveAs(_fileName, XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing, true, false, XlSaveAsAccessMode.xlNoChange, XlSaveConflictResolution.xlLocalSessionChanges, Type.Missing, Type.Missing);
            workbook.Close();
        }
    }
}
