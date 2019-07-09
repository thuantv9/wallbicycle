using System;
using System.IO;
using System.Xml;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32;
using Fisbank.Cbs.DataMemory;
using Fisbank.Cbs.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;
using Fisbank.Cbs.ObjectInfo.TransMaster;

namespace Fisbank.Cbs.CommonLib
{
    public class CommonFunc
    {
        #region Constant Values

        public const string APP_TITLE = "Teller Plugin";
        public const string SignatureResponse = "Signature Response";
        public const string VCBResponse = "VCB Response";
        public const string HOResponse = "HO Response";
        public const string APP_PRINT_EXCEL_TYPE = "EXCEL";
        public const string ExcelDesignFilePath = "App_Reports//";
        public const string ImportFileFilePath = "App_Layouts";
        public static Excel.Application APP = null;
        public static Excel.Workbook WB = null;
        public static Excel.Worksheet WS = null;
        public static Excel.Range Range = null;

        #endregion
        #region Constant H2H
        public static string H2HSource(int TxSource)
        {
            switch (TxSource)
            {
                case 0:
                    return "VCB";
                case 1:
                    return "UniTeller";
                case 2:
                    return "TNMONEX";
                case 3:
                    return "Xoom";
                case 4:
                    return "MoneyPolo";
                case 5:
                    return "Pay2Home";
                case 6:
                    return "Unidos";
                default:
                    return "";
            }
        }
        #endregion Constant H2H
        #region Data Format Types

        public const string FORMAT_NUMERIC_INTEGER = "#,##0";
        public const string FORMAT_NUMERIC_DECIMAL = "#,##0.##";
        public const string FORMAT_NUMERIC_DOUBLE = "#,##0.######";
        public const string FORMAT_DATE = "dd/MM/yyyy";
        public const string FORMAT_DATE_CHAR = "dd-MMM-yyyy";
        public const string FORMAT_DATE_TIME = "dd/MM/yyyy HH:mm:ss tt";
        public const string _FORMAT_DATE_TIME = "dd/MM/yyyy HH:mm:ss";
        public const string FORMAT_TIME = "HH:mm:ss tt";
        public const string FORMAT_ALIAS_ACOUNT_PRINT = "###-#-##-######-#";

        #endregion

        #region Validation Methos

        public static bool IsNumber(object obj, bool ignore = false)
        {
            try
            {
                string objTest = DCC.IsNull(obj) ? string.Empty : (ignore ? obj.ToString().Replace(",", "") : obj.ToString());
                Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
                return regex.IsMatch(objTest);
            }
            catch
            {
                return false;
            }
        }

        public static bool IsMoney(object obj, int numberOfDegit)
        {
            try
            {
                string objTest = DCC.IsNull(obj) ? string.Empty : obj.ToString();
                Regex regex;
                if (numberOfDegit == 15)
                {
                    regex = new Regex(@"\$\d{13}+\.\d{2}");
                }
                else
                {
                    regex = new Regex(@"\$\d{15}+\.\d{2}");
                }
                return regex.IsMatch(objTest);
            }
            catch
            {
                return false;
            }
        }

        public static bool IsEmail(object obj)
        {
            try
            {
                string objTest = DCC.IsNull(obj) ? string.Empty : obj.ToString();
                //Regex regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase);
                Regex regex = new Regex(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                return regex.IsMatch(objTest);
            }
            catch
            {
                return false;
            }
        }

        public static bool IsLeapYear(object obj)
        {
            try
            {
                if (!IsNumber(obj)) return false;
                int objTest = Convert.ToInt32(obj.ToString());
                return DateTime.IsLeapYear(objTest);
            }
            catch
            {
                return false;
            }
        }

        public static bool IsAlphanumeric(object obj)
        {
            Regex r = new Regex("^[a-zA-Z0-9]*$");
            if (DCC.IsNull(obj))
                return false;
            else
                return r.IsMatch((obj as string));
        }

        public static bool IsAlpha(object obj)
        {
            Regex r = new Regex("^[a-zA-Z]*$");
            if (DCC.IsNull(obj))
                return false;
            else
                return r.IsMatch((obj as string));
        }
        #endregion

        #region Process Methods

        public static bool IsWindowsAdminUser()
        {
            var currentUser = System.Security.Principal.WindowsIdentity.GetCurrent();
            var objPrincipal = new System.Security.Principal.WindowsPrincipal(currentUser);
            return objPrincipal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
        }

        public static void RevNumberFormat(int aDot, ref string obj)
        {
            try
            {
                if (aDot > 0)
                {
                    obj = FORMAT_NUMERIC_INTEGER + ".";
                    for (int i = 0; i < aDot; i++) obj += "0";
                }
                else
                {
                    obj = FORMAT_NUMERIC_INTEGER;
                }
            }
            catch
            {
                obj = FORMAT_NUMERIC_INTEGER;
            }
        }

        public static string FormatAliasAccountPrint(string AccountNumber)
        {
            string _AccountNumber = "";
            try
            {
                if (AccountNumber.Length != 13)
                {
                    _AccountNumber = String.Format(FORMAT_ALIAS_ACOUNT_PRINT, AccountNumber);
                }
            }
            catch
            {
                _AccountNumber = "Format Wrong!";
            }
            return _AccountNumber;
        }
        public static string FormatNumber(object obj, int aDot = 0)
        {
            try
            {
                string objType = string.Empty;
                RevNumberFormat(aDot, ref objType);
                return obj.IsNull() ? "0" : obj.ToDecimal().ToString(objType);
            }
            catch
            {
                return "0";
            }
        }

        public static string DeFormatNumber(object obj, char type = ',')
        {
            try
            {
                return obj.IsNull() ? string.Empty : obj.ToString().Trim().Replace(DCC.ToString(type), "");
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion

        #region Print Methods

        public static void PrinterCore(DataSet ds, List<ExcelTitle> lstAlias, string rptName, string rptType, List<int> IndexRows = null, string printerName = "", Dictionary<string, bool> dic_Checkbox = null)
        {
            try
            {
                string DataPath = Path.Combine(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Replace("file:\\", String.Empty).Replace("file://", "//"), ".."), "..") + Path.DirectorySeparatorChar;

                FlexCel.Report.FlexCelReport flcReport = new FlexCel.Report.FlexCelReport(true);
                string _Path = CommonFunc.ExcelDesignFilePath;
                string _fileExcelName = rptName;

                if (!DCC.IsNull(ds))
                {
                    PlgPrinter.SetValueExportByDataTable(ref flcReport, ds);
                }
                _Path += _fileExcelName;
                if (!DCC.IsNull(lstAlias) && lstAlias.Count > 0)
                {
                    foreach (ExcelTitle obj in lstAlias)
                    {
                        switch (obj.DataType)
                        {
                            case DataType.String:
                                PlgPrinter.SetValueExportByString(ref flcReport, obj.FieldName, DCC.ToString(obj.FieldValue));
                                break;
                            case DataType.Int:
                                PlgPrinter.SetValueExportByString(ref flcReport, obj.FieldName, DCC.ToInt(obj.FieldValue).ToString(CommonFunc.FORMAT_NUMERIC_INTEGER));
                                break;
                            case DataType.Double:
                                PlgPrinter.SetValueExportByString(ref flcReport, obj.FieldName, DCC.ToDouble(obj.FieldValue).ToString(CommonFunc.FORMAT_NUMERIC_DOUBLE));
                                break;
                            case DataType.Decimal:
                                PlgPrinter.SetValueExportByString(ref flcReport, obj.FieldName, DCC.ToDecimal(obj.FieldValue).ToString(CommonFunc.FORMAT_NUMERIC_DECIMAL));
                                break;
                            case DataType.Date:
                                PlgPrinter.SetValueExportByString(ref flcReport, obj.FieldName, DCC.ToDateTime(obj.FieldValue).ToString(CommonFunc.FORMAT_DATE));
                                break;
                            case DataType.DateTime:
                                PlgPrinter.SetValueExportByString(ref flcReport, obj.FieldName, DCC.ToDateTime(obj.FieldValue).ToString(CommonFunc.FORMAT_DATE_TIME));
                                break;
                        }
                    }
                }

                string fileExported = string.Empty;

                if (rptType.ToUpper().Trim().Equals(CommonFunc.APP_PRINT_EXCEL_TYPE))
                {
                    if (printerName.IsNotNull())
                    {
                        var pinterFile = string.Format("printerNameTmp{0}.xls", Guid.NewGuid());
                        ExportResult result = PlgPrinter.ExportEXCEL(flcReport, _Path, pinterFile, IndexRows, dic_Checkbox);
                        if (result == ExportResult.Successful)
                        {
                            PrinterProcessing(pinterFile, printerName);
                        }
                        else
                        {
                            PrintShowMessage(result, null, ApplicationCore.GetSubNodes(CommonSettings.IEAppMessages, "PrintMessage"));
                        }
                    }
                    else
                    {
                        SaveFileDialog saveReport = new SaveFileDialog();
                        saveReport.Filter = "Excel files (*.xls)|*.xls";
                        if (saveReport.ShowDialog() == DialogResult.OK)
                        {
                            ExportResult result = PlgPrinter.ExportEXCEL(flcReport, _Path, saveReport, ref fileExported, IndexRows, dic_Checkbox);
                            PrintShowMessage(result, fileExported, ApplicationCore.GetSubNodes(CommonSettings.IEAppMessages, "PrintMessage"));
                        }
                    }
                }
                else
                {
                    SaveFileDialog PdfSaveFileDialog = new SaveFileDialog();
                    PdfSaveFileDialog.DefaultExt = "pdf";
                    PdfSaveFileDialog.Filter = "Pdf Files|*.pdf";
                    PdfSaveFileDialog.Title = "Select the file to export to: ";
                    if (PdfSaveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        ExportResult result = PlgPrinter.ExportPDF(flcReport, _Path, PdfSaveFileDialog, ref fileExported);
                        PrintShowMessage(result, fileExported, ApplicationCore.GetSubNodes(CommonSettings.IEAppMessages, "PrintMessage"));
                    }
                }
            }
            catch (Exception ex)
            {
                string errorSource = "Fisbank.Cbs.CommonLib.CommonFunc.PrinterCore(DataSet ds, List<ExcelTitle> lstAlias, string rptName, string rptType)";
                ErrorLog.Log(Environment.NewLine + " --> Source: " + errorSource + Environment.NewLine + " --> Message: " + ex.Message);
            }
        }

        public static DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }
        public static DataTable ConvertListToDataTable(List<string> list)
        {
            // New table.
            DataTable table = new DataTable();

            // Get max columns.
            int columns = 1;
            //if (list.Count > 0)
            //{
            //    columns = list.Count;
            //}

            // Add columns.
            for (int i = 0; i < columns; i++)
            {
                table.Columns.Add("Datastring");
            }

            //Add rows.
            foreach (var array in list)
            {
                table.Rows.Add(array);
            }

            return table;
        }

        public static List<T> DataTableToList<T>(DataTable table) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();

                foreach (var row in table.AsEnumerable())
                {
                    T obj = new T();

                    ConvertDataRowToObject(row, ref obj);

                    list.Add(obj);
                }

                return list;
            }
            catch (Exception ex)
            {
                string errorSource = "Fisbank.Cbs.CommonLib.CommonFunction.DataTableToList<T>(DataTable table)";
                ErrorLog.Log(Environment.NewLine + " --> Source: " + errorSource + Environment.NewLine + " --> Message: " + ex.Message.ToUpper());
                return null;
            }
        }

        public static List<T> ListDataRowToList<T>(IEnumerable<DataRow> DataRows) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();

                foreach (var row in DataRows)
                {
                    T obj = new T();
                    ConvertDataRowToObject(row, ref obj);

                    list.Add(obj);
                }

                return list;
            }
            catch (Exception ex)
            {
                string errorSource = "Fisbank.Cbs.CommonLib.CommonFunction.DataTableToList<T>(DataTable table)";
                ErrorLog.Log(Environment.NewLine + " --> Source: " + errorSource + Environment.NewLine + " --> Message: " + ex.Message.ToUpper());
                return null;
            }
        }

        private static void ConvertDataRowToObject<T>(DataRow row, ref T obj) where T : class, new()
        {
            foreach (var prop in obj.GetType().GetProperties())
            {
                try
                {
                    PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                    propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                }
                catch
                {
                    continue;
                }
            }
        }

        public static T DataRowToObject<T>(DataRow dataRow) where T : class, new()
        {
            try
            {
                T item = new T();
                foreach (DataColumn column in dataRow.Table.Columns)
                {
                    PropertyInfo property = item.GetType().GetProperty(column.ColumnName);

                    if (property != null && dataRow[column] != DBNull.Value)
                    {
                        object result = Convert.ChangeType(dataRow[column], property.PropertyType);
                        property.SetValue(item, result, null);
                    }
                }

                return item;
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return null;
            }
        }

        public static void DictionaryTransError<T>(T OBJ) where T : class, new()
        {
            try
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();

                //Data START
                foreach (PropertyInfo obj in OBJ.GetType().GetProperties())
                {
                    dictionary.Add(obj.Name + ": ", obj.GetValue(OBJ, null).ToString());
                }
                //Data END

                Process open_dictionary = new Process();
                String Path_dictionary = Path.Combine(Application.StartupPath, "TransErrorDetail.txt");
                StreamWriter sw = new StreamWriter(Path_dictionary);
                foreach (var dr in dictionary)
                {
                    sw.WriteLine(dr.Key + dr.Value);
                }
                sw.Close();
                open_dictionary.StartInfo.FileName = Path_dictionary;
                open_dictionary.Start();
                dictionary.Clear();
            }
            catch (Exception ex)
            {
                string errorSource = "Fisbank.Cbs.CommonLib.CommonFunc.DictionaryTransError<T>(T OBJ)";
                ErrorLog.Log(Environment.NewLine + " --> Source: " + errorSource + Environment.NewLine + " --> Message: " + ex.Message);
            }
        }

        public static void DictionaryTransErrorCommand(string data)
        {
            try
            {
                string[] datatemp = data.Split(';');
                Dictionary<string, string> dictionary = new Dictionary<string, string>();

                //Data START
                for (int i = 0; i < datatemp.Count() - 1; i++)
                {
                    dictionary.Add("Data " + i.ToString() + ": ", datatemp[i].ToString());
                }
                //Data END

                Process open_dictionary = new Process();
                String Path_dictionary = Path.Combine(Application.StartupPath, "TransErrorDetail.txt");
                StreamWriter sw = new StreamWriter(Path_dictionary);
                foreach (var dr in dictionary)
                {
                    sw.WriteLine(dr.Key + dr.Value);
                }
                sw.Close();
                open_dictionary.StartInfo.FileName = Path_dictionary;
                open_dictionary.Start();
                dictionary.Clear();
            }
            catch (Exception ex)
            {
                string errorSource = "Fisbank.Cbs.CommonLib.CommonFunc.DictionaryTransErrorCommand(string data)";
                ErrorLog.Log(Environment.NewLine + " --> Source: " + errorSource + Environment.NewLine + " --> Message: " + ex.Message);
            }
        }

        private static void PrintShowMessage(ExportResult result, string fileExported, IEnumerable<XElement> xmlResource)
        {
            try
            {
                string eMessage = string.Empty;

                switch (result)
                {
                    case ExportResult.NoTemplate:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "NoTemplate");
                        MessageBox.Show(eMessage, CommonFunc.APP_TITLE);
                        break;

                    case ExportResult.TempIsUsed:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "TempIsUsed");
                        MessageBox.Show(eMessage, CommonFunc.APP_TITLE);
                        break;

                    case ExportResult.Failure:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "ExportFailure");
                        MessageBox.Show(eMessage, CommonFunc.APP_TITLE);
                        break;

                    case ExportResult.Successful:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "OpenFileQuestion");
                        DialogResult dlg = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dlg == System.Windows.Forms.DialogResult.Yes) System.Diagnostics.Process.Start(fileExported);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, CommonFunc.APP_TITLE);
            }
        }


        static void PrinterProcessing(string ExcelFile, string printerName)
        {
            try
            {
                APP = new Microsoft.Office.Interop.Excel.Application();
                var spreadsheetLocation = Path.Combine(Directory.GetCurrentDirectory(), ExcelFile);
                WB = APP.Workbooks.Open(spreadsheetLocation, 1);
                WS = (Excel.Worksheet)WB.Worksheets[1];
                WS.PageSetup.Orientation = Excel.XlPageOrientation.xlPortrait;
                WS.PageSetup.PaperSize = Excel.XlPaperSize.xlPaperA4;
                WS.PrintOut(
                    Type.Missing, WS.PageSetup.Pages.Count, Type.Missing, Type.Missing,
                    printerName, Type.Missing, Type.Missing, Type.Missing);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Marshal.FinalReleaseComObject(WS);
                WB.Close(false, Type.Missing, Type.Missing);
                Marshal.FinalReleaseComObject(WB);
                APP.Quit();
                Marshal.FinalReleaseComObject(APP);
                File.Delete(Path.Combine(Directory.GetCurrentDirectory(), ExcelFile));
            }
            catch (Exception ex)
            {
                string errorSource = "PrinterProcessing(string ExcelFile, string printerName)";
                ErrorLog.Log(Environment.NewLine + " --> Source: " + errorSource + Environment.NewLine + " --> Message: " + ex.Message);
                if (WS != null)
                {
                    Marshal.FinalReleaseComObject(WS);
                }
                if (WB != null)
                {
                    WB.Close(false, Type.Missing, Type.Missing);
                    Marshal.FinalReleaseComObject(WB);
                }
                if (APP != null)
                {
                    APP.Quit();
                    Marshal.FinalReleaseComObject(APP);
                }
                File.Delete(Path.Combine(Directory.GetCurrentDirectory(), ExcelFile));
            }

        }

        #endregion

        #region Aproval Methods

        public static bool AprovalShowMessage(ApprovalResult ProsessAproval, IEnumerable<XElement> xmlResource, string TranidTime)
        {
            try
            {
                string eMessage = string.Empty;

                switch (ProsessAproval)
                {
                    case ApprovalResult.Aproval:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsAproval");
                        DialogResult dlgAproval = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dlgAproval == System.Windows.Forms.DialogResult.No)
                            return false;
                        break;

                    case ApprovalResult.AprovalAll:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsAprovalAll");
                        DialogResult dlgAprovalAll = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dlgAprovalAll == System.Windows.Forms.DialogResult.No)
                            return false;
                        break;

                    case ApprovalResult.Reject:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsReject");
                        DialogResult dlgReject = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dlgReject == System.Windows.Forms.DialogResult.No)
                            return false;
                        break;

                    case ApprovalResult.Cancel:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsCancel");
                        DialogResult dlgCancel = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dlgCancel == System.Windows.Forms.DialogResult.No)
                            return false;
                        break;

                    case ApprovalResult.Delete:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsDelete");
                        DialogResult dlgDelete = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dlgDelete == System.Windows.Forms.DialogResult.No)
                            return false;
                        break;

                    case ApprovalResult.Aproval_Reject:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsAproval_Reject");
                        DialogResult dlgAproval_Reject = MessageBox.Show(eMessage, CommonFunc.APP_TITLE);
                        if (dlgAproval_Reject == DialogResult.OK || dlgAproval_Reject == DialogResult.Yes)
                            return false;
                        break;

                    case ApprovalResult.ByPass:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsByPass");
                        DialogResult dlgByPass = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dlgByPass == System.Windows.Forms.DialogResult.No)
                            return false;
                        break;

                    case ApprovalResult.ByPassEnd:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsdlgByPassEnd");
                        DialogResult dlgByPassEnd = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (dlgByPassEnd == DialogResult.OK)
                            return false;
                        break;

                    case ApprovalResult.OvrerTime:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsdlgOvrerTime");
                        DialogResult dlgOvrerTime = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        if (dlgOvrerTime == DialogResult.OK)
                            return false;
                        break;
                    case ApprovalResult.OvrerTimeAll:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsdlgOvrerTime") + Environment.NewLine + ApplicationCore.GetXElementText(xmlResource, "MsdlgOvrerTimeAll") + TranidTime;
                        DialogResult dlgOvrerTimeAll = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        if (dlgOvrerTimeAll == DialogResult.OK)
                            return false;
                        break;

                    case ApprovalResult.SendforOfficer:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsSendforOfficer") + Environment.NewLine + ApplicationCore.GetXElementText(xmlResource, "MsSendforOfficerAll") + TranidTime;
                        DialogResult dlgSendforOfficer = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dlgSendforOfficer == System.Windows.Forms.DialogResult.No)
                            return false;
                        break;

                    case ApprovalResult.SendforSupper:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsSendforSupper") + Environment.NewLine + ApplicationCore.GetXElementText(xmlResource, "MsSendforSupperAll") + TranidTime;
                        DialogResult dlgSendforSupper = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dlgSendforSupper == System.Windows.Forms.DialogResult.No)
                            return false;
                        break;

                    case ApprovalResult.ApprovalByOfficer:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsApprovalByOfficer");
                        DialogResult dlgApprovalByOfficer = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dlgApprovalByOfficer == System.Windows.Forms.DialogResult.No)
                            return false;
                        break;

                    case ApprovalResult.Adminset:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsAdminset1") + Environment.NewLine + ApplicationCore.GetXElementText(xmlResource, "MsAdminset2");
                        DialogResult dlgAdminset = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dlgAdminset == System.Windows.Forms.DialogResult.No)
                            return false;
                        break;

                    case ApprovalResult.ChargeTypeselected:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsChargeType");
                        DialogResult dlgChargeType = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dlgChargeType == System.Windows.Forms.DialogResult.No)
                            return false;
                        break;

                    case ApprovalResult.SetCurrency:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsSetCurrency");
                        DialogResult dlgSetCurrency = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dlgSetCurrency == System.Windows.Forms.DialogResult.No)
                            return false;
                        break;

                    case ApprovalResult.BlockTrans:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsBlockTrans") + TranidTime;
                        DialogResult dlgBlockTrans = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        if (dlgBlockTrans == DialogResult.OK)
                            return false;
                        break;
                    case ApprovalResult.processed:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "Msprocessed") + TranidTime;
                        DialogResult dlgprocessed = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        if (dlgprocessed == DialogResult.OK)
                            return false;
                        break;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, CommonFunc.APP_TITLE);
                return false;
            }
        }

        public static bool AprovalMsSingResponse(ApprovalResult ProsessAproval, IEnumerable<XElement> xmlResource, int RecordAccept, int RecordReject)
        {
            try
            {
                string eMessage = string.Empty;
                string eMessage1 = string.Empty;
                string eMessage2 = string.Empty;

                switch (ProsessAproval)
                {
                    case ApprovalResult.SingResponse:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsSingResponse1") + RecordAccept + " giao dịch." + Environment.NewLine + ApplicationCore.GetXElementText(xmlResource, "MsSingResponse2") + RecordReject + " giao dịch.";
                        DialogResult dlgSingResponse = MessageBox.Show(eMessage, CommonFunc.SignatureResponse, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (dlgSingResponse == DialogResult.OK)
                            return false;
                        break;

                    case ApprovalResult.ByPass2:
                        eMessage2 = ApplicationCore.GetXElementText(xmlResource, "MsByPass21") + Environment.NewLine;
                        foreach (var item in DataMem.lstByPassMs)
                        {
                            eMessage1 += "TransID " + item.TransID + ": " + item.ResponseMessage + Environment.NewLine;
                        }
                        eMessage = eMessage2 + eMessage1 + ApplicationCore.GetXElementText(xmlResource, "MsByPass2");
                        DialogResult dlgByPass2 = MessageBox.Show(eMessage, CommonFunc.SignatureResponse, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dlgByPass2 == System.Windows.Forms.DialogResult.No)
                            return false;
                        break;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, CommonFunc.APP_TITLE);
                return false;
            }
        }

        public static bool AprovalMsHostResponse(ApprovalResult ProsessAproval, IEnumerable<XElement> xmlResource, string ResponseMessage)
        {
            try
            {
                string eMessage = string.Empty;
                string eMessage1 = string.Empty;
                string eMessage2 = string.Empty;

                switch (ProsessAproval)
                {
                    case ApprovalResult.HostResponse:
                        eMessage2 = ApplicationCore.GetXElementText(xmlResource, "MsByPass21") + Environment.NewLine;
                        eMessage1 = ResponseMessage + Environment.NewLine;

                        eMessage = eMessage2 + eMessage1 + ApplicationCore.GetXElementText(xmlResource, "MsHostResponse");
                        DialogResult dlgHostResponse = MessageBox.Show(eMessage, CommonFunc.SignatureResponse, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dlgHostResponse == System.Windows.Forms.DialogResult.No)
                            return false;
                        break;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, CommonFunc.APP_TITLE);
                return false;
            }
        }

        #endregion

        #region Message SqlResult Methods

        public enum SqlResult
        {
            Insert_Normal = 0,
            Insert_Error = 1,
            Update_Normal = 2,
            Update_Error = 3,
            Delete_Normal = 4,
            Delete_Error = 5,
            Insert_Normal_collectionItem = 6,
            Insert_Error_collectionItem = 7,
            Insert_Normal_collectionLetter = 8,
            Insert_Error_collectionLetter = 9,
            MsValidate_InvalidMoney = 10,
            MsValidate_OutValueMoney = 11,
            CheckInput = 12,
            InputDescription = 13,
            InputSecurityProfile = 14,
            InputPriority = 15,
            CheckPriority = 16,
            RefNo_Exist_collectionLetter = 17,
            Validate_Item_Collectionletter = 18,
            Insert_Normal_CancelItem = 19,
            Insert_Error_CancelItem = 20,
            MsUpdate_Nomal_Collection = 21,
            MsUpdate_Error_Collection = 22,
            MsCollection_Item_InUser = 23,
            MsSerial_NotExist_CollectionItem = 24,
            Update_DBPLUGIN_Error = 25,
            Update_RefNoDBPLUGIN_Error = 26,
            Update_RefNoVCB_Error = 27,
            HOResponse = 28
        }

        public enum CheckInput
        {
            Insert_data = 1
        }

        public enum InputHostToHost
        {
            NumRefOk = 1,
            NumRefInput = 2,
            SuppervisorOverride = 3,
            SendApproval = 4,
            WaitingApproval = 5,
            InsertRefnoError = 6,
            InsertRefnoErrorPlgin = 21,
            UnBlockOk = 7,
            UnBlockError = 8,
            UnBlockErrorplugin = 22,
            ApprovalOk = 9,
            ApprovalError = 10,
            RefInforTab = 11,
            RefConfirm = 12,
            RefConfirmCancel = 13,
            txtCrAcNumberInput = 14,
            txtCrAcNumber1Input = 15,
            txtCheckAcc = 16,
            txtReceiverInput = 17,
            Checkuserlogin = 18,
            checkRptInfor = 19,
            ResponeVCB = 20,
            UnblockedTrans = 23,
            WaitingUnblockTrans = 24
        }
        // Add Messager Start
        public enum F5Aproval
        {
            SendToSup = 1,
            CancelData = 2,
            CancelDataDone = 3,
            CancelDataApproval = 4,
            SendForSig = 5,
            CancelDataWaiting = 6


        }
        public static bool ShowShowMessageF5(int sqlResult, string TextInput)
        {
            return AprovalShowMessageF5(sqlResult, TextInput, ApplicationCore.GetSubNodes(CommonSettings.IEAppMessages, "AprovalMessageF5"));
        }

        public static bool AprovalShowMessageF5(int ProsessAproval, string Input, IEnumerable<XElement> xmlResource)
        {
            try
            {
                string eMessage = string.Empty;

                switch (ProsessAproval)
                {
                    case (int)F5Aproval.SendToSup:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsSendToSup");
                        DialogResult dlgSendToSup = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dlgSendToSup == System.Windows.Forms.DialogResult.No)
                            return false;
                        break;
                    case (int)F5Aproval.CancelData:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsCancelData");
                        DialogResult dlgCancelData = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dlgCancelData == System.Windows.Forms.DialogResult.No)
                            return false;
                        break;

                    case (int)F5Aproval.CancelDataDone:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsCancelDataDone");
                        DialogResult dlgCancelDataDone = MessageBox.Show(eMessage + Input, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (dlgCancelDataDone == DialogResult.OK)
                            return false;
                        break;
                    case (int)F5Aproval.CancelDataApproval:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsCancelDataApproval");
                        DialogResult dlgCancelDataApproval = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (dlgCancelDataApproval == DialogResult.OK)
                            return false;
                        break;
                    case (int)F5Aproval.SendForSig:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsSendForSig");
                        DialogResult dlgSendForSig = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (dlgSendForSig == DialogResult.OK)
                            return false;
                        break;
                    case (int)F5Aproval.CancelDataWaiting:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsCancelDataWaiting");
                        DialogResult dlgCancelDataWaiting = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (dlgCancelDataWaiting == DialogResult.OK)
                            return false;
                        break;

                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, CommonFunc.APP_TITLE);
                return false;
            }
        }

        // Add Messager End


        public static void ShowMessageCheckInput(int sqlResult, string TextInput)
        {
            CheckInputShowMessage(sqlResult, TextInput, ApplicationCore.GetSubNodes(CommonSettings.IEAppMessages, "CheckInputMessage"));
        }

        public static void ShowMessageHostToHost(InputHostToHost sqlResult, string TextInput = "", string[] ResponeVCB = null)
        {
            MessageOfHostToHost(sqlResult, TextInput, ApplicationCore.GetSubNodes(CommonSettings.IEAppMessages, "HostToHostMessage"), ResponeVCB);
        }

        public static void ShowMessageSQL(int sqlResult, int sqlRecord)
        {
            SqlShowMessage(sqlResult, sqlRecord, ApplicationCore.GetSubNodes(CommonSettings.IEAppMessages, "SqlResultMessage"));
        }

        public static void ShowMessageSQL_ROU(string AdditionMsg, int sqlResult, int sqlRecord)
        {
            SqlShowMessage_ROU(AdditionMsg, sqlResult, sqlRecord, ApplicationCore.GetSubNodes(CommonSettings.IEAppMessages, "SqlResultMessage"));
        }

        public static void CheckInputShowMessage(int sqlResult, string TextInput, IEnumerable<XElement> xmlResource)
        {
            try
            {
                string eMessage = string.Empty;

                switch (sqlResult)
                {
                    case (int)CheckInput.Insert_data:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsInsertdata") + "'" + TextInput + "'.";
                        DialogResult dlgInsertdata = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, CommonFunc.APP_TITLE);
            }
        }

        public static void MessageOfHostToHost(InputHostToHost sqlResult, string TextInput, IEnumerable<XElement> xmlResource, string[] ResponeVCB = null)
        {
            try
            {
                string eMessage = string.Empty;

                switch (sqlResult)
                {
                    case InputHostToHost.NumRefOk:
                        string eMsNumRefOk1 = string.Empty;
                        string eMsNumRefOk2 = string.Empty;
                        if (ResponeVCB.IsNull())
                        {
                            eMsNumRefOk1 = ApplicationCore.GetXElementText(xmlResource, "MsResponeVCBCode") + string.Empty;
                            eMsNumRefOk2 = ApplicationCore.GetXElementText(xmlResource, "MsResponeVCBMs") + string.Empty;
                        }
                        else
                        {
                            eMsNumRefOk1 = ApplicationCore.GetXElementText(xmlResource, "MsResponeVCBCode") + ResponeVCB[0];
                            eMsNumRefOk2 = ApplicationCore.GetXElementText(xmlResource, "MsResponeVCBMs") + ResponeVCB[1];
                        }
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsNumRefOk");
                        DialogResult dlgNumRefOk = MessageBox.Show(eMsNumRefOk1 + Environment.NewLine + eMsNumRefOk2 + Environment.NewLine + eMessage, CommonFunc.VCBResponse, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case InputHostToHost.NumRefInput:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsNumRefInput");
                        DialogResult dlgNumRefInput = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case InputHostToHost.SuppervisorOverride:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsSuppervisorOverride");
                        DialogResult dlgSuppervisorOverride = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case InputHostToHost.SendApproval:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsSendApproval");
                        DialogResult dlgSendApproval = MessageBox.Show(eMessage + Environment.NewLine + TextInput, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case InputHostToHost.WaitingApproval:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsWaitingApproval");
                        DialogResult dlgWaitingApproval = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case InputHostToHost.InsertRefnoError:

                        string eMsInsert1 = string.Empty;
                        string eMsInsert2 = string.Empty;
                        if (ResponeVCB.IsNull())
                        {
                            eMsInsert1 = ApplicationCore.GetXElementText(xmlResource, "MsResponeVCBCode") + string.Empty;
                            eMsInsert2 = ApplicationCore.GetXElementText(xmlResource, "MsResponeVCBMs") + string.Empty;

                        }
                        else
                        {
                            eMsInsert1 = ApplicationCore.GetXElementText(xmlResource, "MsResponeVCBCode") + ResponeVCB[0];
                            eMsInsert2 = ApplicationCore.GetXElementText(xmlResource, "MsResponeVCBMs") + ResponeVCB[1];
                        }
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsInsertRefnoError");
                        DialogResult dlgInsertRefnoError = MessageBox.Show(eMsInsert1 + Environment.NewLine + eMsInsert2 + Environment.NewLine + eMessage, CommonFunc.VCBResponse, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case InputHostToHost.InsertRefnoErrorPlgin:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsInsertRefnoError");
                        DialogResult dlgInsertRefnoErrorPlgin = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case InputHostToHost.UnBlockOk:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsUnBlockOk");
                        DialogResult dlgUnBlockOk = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case InputHostToHost.UnBlockErrorplugin:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsUnBlockError");
                        DialogResult dlgUnBlockError = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case InputHostToHost.UnBlockError:
                        string eMsUnBlockError1 = string.Empty;
                        string eMsUnBlockError2 = string.Empty;
                        if (ResponeVCB.IsNull())
                        {
                            eMsUnBlockError1 = ApplicationCore.GetXElementText(xmlResource, "MsResponeVCBCode") + string.Empty;
                            eMsUnBlockError2 = ApplicationCore.GetXElementText(xmlResource, "MsResponeVCBMs") + string.Empty;
                        }
                        else
                        {
                            eMsUnBlockError1 = ApplicationCore.GetXElementText(xmlResource, "MsResponeVCBCode") + ResponeVCB[0];
                            eMsUnBlockError2 = ApplicationCore.GetXElementText(xmlResource, "MsResponeVCBMs") + ResponeVCB[1];
                        }
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsUnBlockError");
                        DialogResult dlgUnBlockErrorVCB = MessageBox.Show(eMsUnBlockError1 + Environment.NewLine + eMsUnBlockError2 + Environment.NewLine + eMessage, CommonFunc.VCBResponse, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case InputHostToHost.ApprovalOk:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsApprovalOk");
                        DialogResult dlgApprovalOk = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case InputHostToHost.ApprovalError:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsApprovalError");
                        DialogResult dlgApprovalError = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case InputHostToHost.RefInforTab:
                        string eMsRefInforTab1 = string.Empty;
                        string eMsRefInforTab2 = string.Empty;
                        if (ResponeVCB.IsNull())
                        {
                            eMsRefInforTab1 = ApplicationCore.GetXElementText(xmlResource, "MsResponeVCBCode") + string.Empty;
                            eMsRefInforTab2 = ApplicationCore.GetXElementText(xmlResource, "MsResponeVCBMs") + string.Empty;
                        }
                        else
                        {
                            eMsRefInforTab1 = ApplicationCore.GetXElementText(xmlResource, "MsResponeVCBCode") + ResponeVCB[0];
                            eMsRefInforTab2 = ApplicationCore.GetXElementText(xmlResource, "MsResponeVCBMs") + ResponeVCB[1];
                        }

                        if (ResponeVCB[33].ToString() == "A")
                        {
                            eMessage = ApplicationCore.GetXElementText(xmlResource, "MsRefInforTabStatusA");
                        }
                        else if (ResponeVCB[33].ToString() == "P")
                        {
                            eMessage = ApplicationCore.GetXElementText(xmlResource, "MsRefInforTabStatusP");
                        }
                        else
                        {
                            eMessage = ApplicationCore.GetXElementText(xmlResource, "MsRefInforTab");
                        }
                        DialogResult dlgRefInforTab = MessageBox.Show(eMsRefInforTab1 + Environment.NewLine + eMsRefInforTab2 + Environment.NewLine + eMessage, CommonFunc.VCBResponse, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case InputHostToHost.RefConfirm:
                        string eMsRefConfirm1 = string.Empty;
                        string eMsRefConfirm2 = string.Empty;
                        if (ResponeVCB.IsNull())
                        {
                            eMsRefConfirm1 = ApplicationCore.GetXElementText(xmlResource, "MsResponeVCBCode") + string.Empty;
                            eMsRefConfirm2 = ApplicationCore.GetXElementText(xmlResource, "MsResponeVCBMs") + string.Empty;
                        }
                        else
                        {
                            eMsRefConfirm1 = ApplicationCore.GetXElementText(xmlResource, "MsResponeVCBCode") + ResponeVCB[0];
                            eMsRefConfirm2 = ApplicationCore.GetXElementText(xmlResource, "MsResponeVCBMs") + ResponeVCB[1];
                        }
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsRefConfirm");
                        DialogResult dlgRefConfirm = MessageBox.Show(eMsRefConfirm1 + Environment.NewLine + eMsRefConfirm2 + Environment.NewLine + eMessage, CommonFunc.VCBResponse, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case InputHostToHost.RefConfirmCancel:
                        string eMsRefConfirmCancel1 = string.Empty;
                        string eMsRefConfirmCancel2 = string.Empty;
                        if (ResponeVCB.IsNull())
                        {
                            eMsRefConfirmCancel1 = ApplicationCore.GetXElementText(xmlResource, "MsResponeVCBCode") + string.Empty;
                            eMsRefConfirmCancel2 = ApplicationCore.GetXElementText(xmlResource, "MsResponeVCBMs") + string.Empty;
                        }
                        else
                        {
                            eMsRefConfirmCancel1 = ApplicationCore.GetXElementText(xmlResource, "MsResponeVCBCode") + ResponeVCB[0];
                            eMsRefConfirmCancel2 = ApplicationCore.GetXElementText(xmlResource, "MsResponeVCBMs") + ResponeVCB[1];
                        }

                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsRefConfirmCancel");
                        DialogResult dlgRefConfirmCancel = MessageBox.Show(eMsRefConfirmCancel1 + Environment.NewLine + eMsRefConfirmCancel2 + Environment.NewLine + eMessage, CommonFunc.VCBResponse, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;


                    case InputHostToHost.txtCrAcNumberInput:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MstxtCrAcNumberInput");
                        DialogResult dlgNumtxtCrAcNumberInput = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case InputHostToHost.txtCrAcNumber1Input:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MstxtCrAcNumber1Input");
                        DialogResult dlgNumtxtCrAcNumber1Input = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case InputHostToHost.txtCheckAcc:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MstxtCheckAcc");
                        DialogResult dlgtxtCheckAcc = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case InputHostToHost.txtReceiverInput:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MstxtReceiverInput");
                        DialogResult dlgtxtReceiverInput = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case InputHostToHost.Checkuserlogin:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsCheckuserlogin") + "'" + TextInput + "'.";
                        DialogResult dlgCheckuserlogin = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case InputHostToHost.UnblockedTrans:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsUnblockedTrans") + "'" + TextInput + "'.";
                        DialogResult dlgUnblockedTrans = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case InputHostToHost.WaitingUnblockTrans:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsWaitingUnblockTrans");
                        DialogResult dlgWaitingUnblockTrans = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case InputHostToHost.checkRptInfor:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MscheckRptInfor");
                        DialogResult dlgcheckRptInfor = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case InputHostToHost.ResponeVCB:
                        string eMsResponeVCB1 = string.Empty;
                        string eMsResponeVCB2 = string.Empty;
                        if (ResponeVCB.IsNull())
                        {
                            eMsResponeVCB1 = ApplicationCore.GetXElementText(xmlResource, "MsResponeVCBCode") + string.Empty;
                            eMsResponeVCB2 = ApplicationCore.GetXElementText(xmlResource, "MsResponeVCBMs") + string.Empty;
                        }
                        else
                        {
                            eMsResponeVCB1 = ApplicationCore.GetXElementText(xmlResource, "MsResponeVCBCode") + ResponeVCB[0];
                            eMsResponeVCB2 = ApplicationCore.GetXElementText(xmlResource, "MsResponeVCBMs") + ResponeVCB[1];
                        }
                        DialogResult dlgResponeVCB = MessageBox.Show(eMsResponeVCB1 + Environment.NewLine + eMsResponeVCB2, CommonFunc.VCBResponse, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, CommonFunc.APP_TITLE);
            }
        }

        public static void SqlShowMessage(int sqlResult, int sqlRecord, IEnumerable<XElement> xmlResource)
        {
            try
            {
                string eMessage = string.Empty;

                switch (sqlResult)
                {
                    case (int)SqlResult.Insert_Normal:
                        eMessage = sqlRecord + ApplicationCore.GetXElementText(xmlResource, "MsInsertNormal");
                        DialogResult dlgInsertNormal = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case (int)SqlResult.Insert_Error:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsInsertError");
                        DialogResult dlgInsertError = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case (int)SqlResult.Update_Normal:
                        eMessage = sqlRecord + ApplicationCore.GetXElementText(xmlResource, "MsUpdateNormal");
                        DialogResult dlgUpdateNormal = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case (int)SqlResult.Update_Error:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsUpdateError");
                        DialogResult dlgUpdateError = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case (int)SqlResult.Delete_Normal:
                        eMessage = sqlRecord + ApplicationCore.GetXElementText(xmlResource, "MsDeleteNormal");
                        DialogResult dlgDeleteNormal = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case (int)SqlResult.Delete_Error:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsDeleteError");
                        DialogResult dlgDeleteError = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case (int)SqlResult.CheckInput:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsCheckInput");
                        DialogResult dlgCheckInput = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case (int)SqlResult.InputDescription:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsInputDescription");
                        DialogResult dlgInputDescription = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case (int)SqlResult.InputSecurityProfile:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsInputSecurityProfile");
                        DialogResult dlgInputSecurityProfile = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case (int)SqlResult.InputPriority:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsInputPriority");
                        DialogResult dlgInputPriority = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case (int)SqlResult.CheckPriority:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsCheckPriority");
                        DialogResult dlgCheckPriority = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case (int)SqlResult.Update_DBPLUGIN_Error:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsUpdateDBPLUGIN");
                        DialogResult dlgUpdate_DBPLUGIN = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case (int)SqlResult.Update_RefNoDBPLUGIN_Error:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsUpdate_RefNoDBPLUGIN_Error");
                        DialogResult dlgUpdate_RefNoDBPLUGIN_Error = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case (int)SqlResult.Update_RefNoVCB_Error:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsUpdate_RefNoVCB_Error");
                        DialogResult dlgUpdate_RefNoVCB_Error = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case (int)SqlResult.HOResponse:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsHOResponse");
                        DialogResult dlgHOResponse = MessageBox.Show(eMessage, CommonFunc.HOResponse, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    #region Collection

                    case (int)SqlResult.Insert_Normal_collectionItem:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsInsertNormal_CreatecollectionItem");
                        DialogResult Insert_Normal_collectionItem = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case (int)SqlResult.Insert_Error_collectionItem:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsInsertError_CreatecollectionItem");
                        DialogResult Insert_Error_collectionItem = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case (int)SqlResult.Insert_Normal_collectionLetter:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsInsertNormal_CreatecollectionLetter");
                        DialogResult Insert_Normal_collectionLetter = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case (int)SqlResult.Insert_Error_collectionLetter:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsInsertError_CreatecollectionLetter");
                        DialogResult Insert_Error_collectionLetter = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case (int)SqlResult.MsValidate_InvalidMoney:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsValidate_InvalidMoney");
                        DialogResult MsValidate_InvalidMoney = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case (int)SqlResult.MsValidate_OutValueMoney:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsValidate_OutValueMoney");
                        DialogResult MsValidate_OutValueMoney = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case (int)SqlResult.RefNo_Exist_collectionLetter:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsRefNo_Exist_collectionLetter");
                        DialogResult RefNo_Exist_collectionLetter = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case (int)SqlResult.Validate_Item_Collectionletter:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsValidate_Item_Collectionletter");
                        DialogResult Validate_Item_Collectionletter = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case (int)SqlResult.Insert_Normal_CancelItem:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsInsert_Normal_CancelItem");
                        DialogResult Insert_Normal_CancelItem = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case (int)SqlResult.Insert_Error_CancelItem:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsInsert_Error_CancelItem");
                        DialogResult Insert_Error_CancelItem = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case (int)SqlResult.MsUpdate_Nomal_Collection:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsUpdate_Nomal_Collection");
                        DialogResult MsUpdate_Nomal_Collection = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case (int)SqlResult.MsUpdate_Error_Collection:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsUpdate_Error_Collection");
                        DialogResult MsUpdate_Error_Collection = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case (int)SqlResult.MsSerial_NotExist_CollectionItem:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsSerial_NotExist_CollectionItem");
                        DialogResult MsSerial_NotExist_CollectionItem = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                        #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, CommonFunc.APP_TITLE);
            }
        }

        public static void SqlShowMessage_ROU(string AdditionMsg, int sqlResult, int sqlRecord, IEnumerable<XElement> xmlResource)
        {
            try
            {
                string eMessage = string.Empty;

                switch (sqlResult)
                {
                    case (int)SqlResult.MsCollection_Item_InUser:
                        eMessage = ApplicationCore.GetXElementText(xmlResource, "MsCollection_Item_InUser") + AdditionMsg;
                        DialogResult MsCollection_Item_InUser = MessageBox.Show(eMessage, CommonFunc.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, CommonFunc.APP_TITLE);
            }
        }

        #endregion
    }
}
