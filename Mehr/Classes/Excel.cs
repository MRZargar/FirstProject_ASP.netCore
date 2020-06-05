using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using OfficeOpenXml; 
using OfficeOpenXml.Style; 

namespace Mehr.Classes
{    
    public static class Excel
    {
        public static void Write(string path, DataTable data)
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            // Creating an instance 
            // of ExcelPackage
            using (ExcelPackage excel = new ExcelPackage())
            {
                // name of the sheet 
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1"); 
                
                // setting the properties 
                // of the work sheet  
                workSheet.TabColor = System.Drawing.Color.Black; 
                workSheet.DefaultRowHeight = 12; 
        
                // Setting the properties 
                // of the first row 
                workSheet.Row(1).Height = 20; 
                workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; 
                workSheet.Row(1).Style.Font.Bold = true; 
                
                // Header of the Excel sheet 
                for (int i = 0; i < data.Columns.Count; i++)
                {
                    workSheet.Cells[1, i+1].Value = data.Columns[i].ColumnName; 
                }

                // Inserting the article data into excel 
                // sheet by using the for each loop 
                // As we have values to the first row  
                // we will start with second row 
                int recordIndex = 2; 
                foreach (DataRow r in data.Rows)
                {
                    for (int i = 0; i < data.Columns.Count; i++)
                    {
                        workSheet.Cells[recordIndex, i+1].Value = r[i]; 
                    }
                    recordIndex++;
                }
                
                // By default, the column width is not  
                // set to auto fit for the content 
                // of the range, so we are using 
                // AutoFit() method here.  
                foreach (int i in Enumerable.Range(1, data.Columns.Count))
                {
                    workSheet.Column(i).AutoFit(); 
                }

                try
                {
                    if (File.Exists(path)) 
                        File.Delete(path); 
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
                
                // Create excel file on physical disk  
                FileStream objFileStrm = File.Create(path); 
                objFileStrm.Close(); 
        
                // Write content to excel file  
                File.WriteAllBytes(path, excel.GetAsByteArray()); 
                //Close Excel package 
                excel.Dispose(); 
            }
        }

        public static DataTable Read(string path, bool header = true, string sheetName = "")
        {
            if (Path.GetExtension(path) == ".csv" || Path.GetExtension(path) == ".txt")
            {
                return readCSV(path);
            }

            using (FileStream stream = File.OpenRead(path))
            {
                return Read(stream, header, sheetName);
            }
        }

        public static DataTable Read(Stream stream, bool header = true, string sheetName = "")
        {
            DataTable dt = new DataTable();

            ExcelPackage.LicenseContext = LicenseContext.Commercial;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage(stream))
            {
                //get the first worksheet in the workbook
                ExcelWorksheet worksheet;
                if (sheetName == "")
                    worksheet = package.Workbook.Worksheets.First();
                else
                    worksheet = package.Workbook.Worksheets[sheetName];
                
                readXLS(ref worksheet, ref dt, header);
            }
            
            return dt;
        }

        private static void FilldataTable(ref ExcelWorksheet worksheet, ref DataTable dt, int startRow = 1)
        {
            int rowCount = worksheet.Dimension.End.Row;     //get row count
            int colCount = dt.Columns.Count;

            for (int rowInx = startRow; rowInx <= rowCount; rowInx++)
            {
                DataRow row = dt.NewRow();
                for (int colInx = 1; colInx <= colCount; colInx++)
                {
                    row[colInx - 1] = worksheet.Cells[rowInx, colInx].Value?.ToString().Trim();
                }
                dt.Rows.Add(row.ItemArray);
            }
        }

        private static DataTable readCSV(string path, bool header = true, char delimiter = ',')
        {
            DataTable dt = new DataTable();

            ExcelPackage.LicenseContext = LicenseContext.Commercial;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            //set the formatting options
            ExcelTextFormat format = new ExcelTextFormat();
            format.Delimiter = delimiter;
            format.Culture = new CultureInfo(Thread.CurrentThread.CurrentCulture.ToString());
            format.Culture.DateTimeFormat.ShortDatePattern = "dd-mm-yyyy";
            format.Encoding = new UTF8Encoding();

            //read the CSV file from disk
            FileInfo file = new FileInfo(path);
            
            //create a new Excel package
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                //create a WorkSheet
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet 1");

                //load the CSV data into cell A1
                worksheet.Cells["A1"].LoadFromText(file, format);

                readXLS(ref worksheet, ref dt, header);
            }
            return dt;
        }

        private static void readXLS(ref ExcelWorksheet worksheet, ref DataTable dt, bool header)
        {
            int colCount = worksheet.Dimension.End.Column;  //get Column Count

            for (int colInx = 0; colInx < colCount; colInx++)
            {
                if (header)
                {
                    dt.Columns.Add(worksheet.Cells[1, colInx + 1].Value.ToString().Trim());
                }
                else
                {
                    dt.Columns.Add();
                }
            }

            FilldataTable(ref worksheet, ref dt, header ? 2 : 1);
        }
    }
}