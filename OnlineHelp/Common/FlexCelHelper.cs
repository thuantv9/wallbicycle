using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlexCel.Core;
using FlexCel.XlsAdapter;
using FlexCel.Report;
using System.Data;
using FlexCel;
using System.IO;
using System.Reflection;

namespace Fisbank.Cbs.CommonLib
{
    public enum DataType
    {
        Int = 1,
        Decimal = 2,
        Double = 3,
        String = 4,
        Date = 5,
        DateTime = 6
    }

    public class ExcelTitle
    {
        public string FieldName { get; set; }
        public object FieldValue { get; set; }
        public DataType DataType { get; set; }
        public ExcelTitle(string FieldName, object FieldValue, DataType DataType)
        {
            this.FieldName = FieldName;
            this.FieldValue = FieldValue;
            this.DataType = DataType;
        }
    }

    public class ImportExcel
    {
        public static DataTable ImportFile(string FileName)
        {
            try
            {
                // Open the Excel file.
                XlsFile xls = new XlsFile(false);
                xls.Open(FileName);
                // We will create a DataTable "SheetN" for each sheet on the Excel sheet.
                xls.ActiveSheet = 1;
                DataTable Data = new DataTable();
                Data.TableName = xls.SheetName;
                int ColCount = xls.ColCount;
                int count = 0;
                // Add one column on the dataset for each used column on Excel.
                for (int c = 1; c <= ColCount; c++)
                {
                    if (xls.GetStringFromCell(1, c).Value.ToString() != string.Empty)
                        Data.Columns.Add(xls.GetStringFromCell(1, c).Value, typeof(String)); // Here we will add all strings, since we do not know what we are waiting for.
                    else break;
                }
                string[] dr = new string[Data.Columns.Count];
                int RowCount = xls.RowCount;
                
                for (int r = 2; r <= RowCount; r++)
                {
                    Array.Clear(dr, 0, dr.Length);

                    //This loop will only loop on used cells. It is more efficient than looping on all the columns.
                    for (int cIndex = xls.ColCountInRow(r); cIndex > 0; cIndex--) // Reverse the loop to avoid calling ColCountInRow more than once.
                    {
                        int Col = xls.ColFromIndex(r, cIndex);
                       
                        dr[Col - 1] = xls.GetStringFromCell(r, Col).Value;
                        if(dr[Col - 1].ToString() == string.Empty){
                            count++;
                        }                       
                    }
                    if (count != dr.Count())
                        Data.Rows.Add(dr);
                    count = 0;
                }
                return Data;
            }
            catch
            {
                return null;
            }
        }

        
        public static void ExportExcel(DataTable ds, string FileName)
        {
            try
            {
                // Open the Excel file.
                string[] Heard = {"BranchNumber", "Module", "GLAccountNumber", "CostCentre", "Description" };
                XlsFile xls = new XlsFile(FileName, true);
             
                for (int row = 0; row < ds.Rows.Count; row++)
                {
                    for (int col = 0; col < ds.Columns.Count; col++)
                    {
                        object value = ds.Rows[row][col];
                        
                        if (row == 0)
                        {

                            xls.SetCellValue(row + 1, col + 1, Heard[col] );
                        }
                        else
                        {
                            xls.SetCellValue(row + 1, col + 1, value.ToString());
                        }                        
                    }

                }
                xls.Save(FileName);
            }
            catch (Exception ex)
            {
            }
        }

    }
}
