using System;
using System.Data;
using System.IO;
using System.Linq;
using OfficeOpenXml; 
using OfficeOpenXml.Style; 

namespace Mehr.Classes
{    
    public static class Excel
    {
        public static void Write(string path, DataTable data)
        {
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
            DataTable dt = new DataTable();

            FileInfo existingFile = new FileInfo(path);
            using (ExcelPackage package = new ExcelPackage(existingFile))
            {
                //get the first worksheet in the workbook
                ExcelWorksheet worksheet;
                if (sheetName == "")
                    worksheet = package.Workbook.Worksheets.First();
                else
                    worksheet = package.Workbook.Worksheets[sheetName];
                
                int colCount = worksheet.Dimension.End.Column;  //get Column Count

                dt.TableName = Path.GetFileNameWithoutExtension(path);

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

                if (header)
                {
                    dt = readXLS(ref worksheet, dt, 2);
                }
                else
                {
                    dt = readXLS(ref worksheet, dt);
                }
            }
            
            return dt;
        }

        private static DataTable readXLS(ref ExcelWorksheet worksheet, DataTable dt, int startRow = 1)
        {
            int rowCount = worksheet.Dimension.End.Row;     //get row count
            int colCount = dt.Columns.Count;

            for (int rowInx = startRow; rowInx <= rowCount; rowInx++)
            {
                DataRow row = dt.NewRow();
                for (int colInx = 1; colInx <= colCount; colInx++)
                {
                    row[colInx - 1] = worksheet.Cells[rowInx, colInx].Value.ToString().Trim();
                }
                dt.Rows.Add(row.ItemArray);
            }

            return dt;
        }
    }
}