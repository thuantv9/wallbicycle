using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using System.Globalization;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Principal;
using System.Security.Cryptography;
using System.Security.AccessControl;
using System.Text.RegularExpressions;
using System.Runtime.Serialization.Formatters.Binary;
using ZetaCompressionLibrary;
using Lib.Utils.Extension;

namespace Fisbank.Cbs.Diagnostics
{
    public sealed class ProtectData
    {
        public static string EncryptData(string data)
        {
            if (!data.IsNull())
            {
                byte[] dataIn;
                byte[] arrKey;

                /* Convert data string to bytes */
                dataIn = UTF8Encoding.UTF8.GetBytes(data);

                /* Hash string */
                MD5CryptoServiceProvider MD5Hash;
                MD5Hash = new MD5CryptoServiceProvider();
                arrKey = MD5Hash.ComputeHash(UTF8Encoding.UTF8.GetBytes(DiagConstants.ProtectDataKey));
                MD5Hash.Clear();

                /* Encrypt */
                TripleDESCryptoServiceProvider DESCrypt;
                DESCrypt = new TripleDESCryptoServiceProvider();
                DESCrypt.Key = arrKey;
                DESCrypt.Mode = CipherMode.ECB;
                DESCrypt.Padding = PaddingMode.PKCS7;

                /* Encryption */
                ICryptoTransform cTransform;
                cTransform = DESCrypt.CreateEncryptor();

                /* Return string encrypted */
                byte[] dataOut;
                dataOut = cTransform.TransformFinalBlock(dataIn, 0, dataIn.Length);
                DESCrypt.Clear();

                if (!dataOut.IsNull())
                {
                    return Convert.ToBase64String(dataOut, 0, dataOut.Length);
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }
        public static string DecryptData(string data)
        {
            if (data.IsNotNull())
            {
                byte[] dataIn;
                byte[] arrKey;

                /* Convert data string to bytes */
                dataIn = Convert.FromBase64String(data);

                /* Hash string */
                MD5CryptoServiceProvider MD5Hash;
                MD5Hash = new MD5CryptoServiceProvider();
                arrKey = MD5Hash.ComputeHash(UTF8Encoding.UTF8.GetBytes(DiagConstants.ProtectDataKey));
                MD5Hash.Clear();

                /* Encrypt */
                TripleDESCryptoServiceProvider DESCrypt;
                DESCrypt = new TripleDESCryptoServiceProvider();
                DESCrypt.Key = arrKey;
                DESCrypt.Mode = CipherMode.ECB;
                DESCrypt.Padding = PaddingMode.PKCS7;

                /* Encryption */
                ICryptoTransform cTransform;
                cTransform = DESCrypt.CreateDecryptor();

                /* Return string encrypted */
                byte[] dataOut;
                dataOut = cTransform.TransformFinalBlock(dataIn, 0, dataIn.Length);
                DESCrypt.Clear();

                if (!dataOut.IsNull())
                {
                    return UTF8Encoding.UTF8.GetString(dataOut);
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }
        public static bool Compression(string data, ref byte[] byteData)
        {
            try
            {
                data = EncryptData(data);
                byteData = CompressionHelper.CompressString(data);
                return true;
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return false;
            }
        }
        public static bool CompressionDataTable(DataTable data, ref byte[] byteData)
        {
            try
            {
                //data = EncryptData(data);
                byteData = CompressionHelper.CompressDataSet(data.DataSet);
                return true;
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return false;
            }
        }

        public static bool CompressionDataset(DataSet ds, ref byte[] byteData)
        {
            try
            {
                //data = EncryptData(data);
                byteData = CompressionHelper.CompressDataSet(ds);
                return true;
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return false;
            }
        }
        public static bool DecompressionDataset(byte[] byteData, ref DataSet data)
        {
            try
            {
                data = CompressionHelper.DecompressDataSet(byteData);
                //data = DecryptData(data);
                return true;
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return false;
            }
        }

        public static bool DecompressionDataTable(byte[] byteData, ref DataSet data)
        {
            try
            {
                data = CompressionHelper.DecompressDataSet(byteData);
                //data = DecryptData(data);
                return true;
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return false;
            }
        }

        public static bool Decompression(byte[] byteData, ref string data)
        {
            try
            {
                data = CompressionHelper.DecompressString(byteData);
                data = DecryptData(data);
                return true;
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return false;
            }
        }
        public static XmlNode GetXmlNodeByName(XmlNode parentNode, string subNodeName)
        {
            try
            {
                foreach (XmlNode xmlNode in parentNode.ChildNodes)
                    if (xmlNode.Name.ToUpper().Equals(subNodeName.ToUpper()))
                        return xmlNode;
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
            }
            return null;
        }
        public static void SetFileAttribute(string path, FileAttributes attr)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(path);
                fileInfo.Attributes = attr;
            }
            catch
            { }
        }
        public static void SetFolderAttribute(string path, FileAttributes attr)
        {
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                dirInfo.Attributes = attr;

                FileInfo[] FileInfos = dirInfo.GetFiles();
                if (!FileInfos.IsNull())
                    foreach (FileInfo fileInfo in FileInfos)
                        fileInfo.Attributes = attr;

                DirectoryInfo[] SubDirInfos = dirInfo.GetDirectories();
                if (!SubDirInfos.IsNull())
                    foreach (DirectoryInfo subDir in SubDirInfos)
                        SetFolderAttribute(subDir.FullName, attr);
            }
            catch
            { }
        }
        public static void SetFolderAccessRule(string path)
        {
            try
            {
                string UserName = WindowsIdentity.GetCurrent().Name;
                DirectoryInfo diInfo = new DirectoryInfo(path);
                DirectorySecurity diSecurity = diInfo.GetAccessControl();
                diSecurity.AddAccessRule(new FileSystemAccessRule(UserName, FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                diInfo.SetAccessControl(diSecurity);
            }
            catch
            { }
        }
    }
    public static class DCC
    {
        public static bool IsNull<T>(T obj) where T : class
        {
            if (obj == null) return true;
            else
            {
                if (typeof(T) == typeof(string))
                    return string.IsNullOrWhiteSpace(obj as string);
                else if (typeof(T) == typeof(DataSet))
                    return ((obj as DataSet).Tables.Count == 0);
                else if (typeof(T) == typeof(DataTable))
                    return ((obj as DataTable).Rows.Count == 0);
                else if (typeof(T) == typeof(Hashtable))
                    return ((obj as Hashtable).Count == 0);
                else if (typeof(T) == typeof(ArrayList))
                    return ((obj as ArrayList).Count == 0);
                else if (typeof(T) == typeof(Array))
                    return ((obj as Array).Length == 0);
                else
                    return false;
            }
        }
        public static bool IsNull<T>(T[] obj)
        {
            return (obj == null || obj.Length == 0);
        }
        public static bool IsNull<T>(List<T> obj)
        {
            return (obj == null || obj.Count == 0);
        }
        // CR 34
        public static Control GetAnyControlAt(this TableLayoutPanel panel, int column, int row)
        {
            foreach (Control control in panel.Controls)
            {
                var cellPosition = panel.GetCellPosition(control);
                if (cellPosition.Column == column && cellPosition.Row == row)
                    return control;
            }
            return null;
        }
        // CR 34
        public static bool ToBoolean(bool value)
        {
            try
            {
                return Convert.ToBoolean(value);
            }
            catch
            {
                return false;
            }
        }
        public static bool ToBoolean(byte value)
        {
            try
            {
                return Convert.ToBoolean(value);
            }
            catch
            {
                return false;
            }
        }
        public static bool ToBoolean(char value)
        {
            try
            {
                return Convert.ToBoolean(value);
            }
            catch
            {
                return false;
            }
        }
        public static bool ToBoolean(DateTime value)
        {
            try
            {
                return Convert.ToBoolean(value);
            }
            catch
            {
                return false;
            }
        }
        public static bool ToBoolean(decimal value)
        {
            try
            {
                return Convert.ToBoolean(value);
            }
            catch
            {
                return false;
            }
        }
        public static bool ToBoolean(double value)
        {
            try
            {
                return Convert.ToBoolean(value);
            }
            catch
            {
                return false;
            }
        }
        public static bool ToBoolean(float value)
        {
            try
            {
                return Convert.ToBoolean(value);
            }
            catch
            {
                return false;
            }
        }
        public static bool ToBoolean(int value)
        {
            try
            {
                return Convert.ToBoolean(value);
            }
            catch
            {
                return false;
            }
        }
        public static bool ToBoolean(long value)
        {
            try
            {
                return Convert.ToBoolean(value);
            }
            catch
            {
                return false;
            }
        }
        public static bool ToBoolean(object value)
        {
            try
            {
                return Convert.ToBoolean(value);
            }
            catch
            {
                return false;
            }
        }
        public static bool ToBoolean(sbyte value)
        {
            try
            {
                return Convert.ToBoolean(value);
            }
            catch
            {
                return false;
            }
        }
        public static bool ToBoolean(short value)
        {
            try
            {
                return Convert.ToBoolean(value);
            }
            catch
            {
                return false;
            }
        }
        public static bool ToBoolean(string value)
        {
            try
            {
                if (IsNull(value)) return false;
                else
                {
                    string clone = value.ToLower().Trim();
                    if (clone == "0" || clone == "1")
                        return Convert.ToBoolean(Convert.ToInt32(clone));
                    else if (clone == "false" || clone == "true")
                        return Convert.ToBoolean(clone);
                    else if (clone == "y" || clone == "yes")
                        return true;
                    else
                        return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public static bool ToBoolean(uint value)
        {
            try
            {
                return Convert.ToBoolean(value);
            }
            catch
            {
                return false;
            }
        }
        public static bool ToBoolean(ulong value)
        {
            try
            {
                return Convert.ToBoolean(value);
            }
            catch
            {
                return false;
            }
        }
        public static bool ToBoolean(ushort value)
        {
            try
            {
                return Convert.ToBoolean(value);
            }
            catch
            {
                return false;
            }
        }
        public static bool ToBoolean(XAttribute value)
        {
            try
            {
                return ToBoolean(ToString(value));
            }
            catch
            {
                return false;
            }
        }
        public static string ToString(bool value)
        {
            try
            {
                return Convert.ToString(value);
            }
            catch
            {
                return string.Empty;
            }
        }
        public static string ToString(byte value)
        {
            try
            {
                return Convert.ToString(value);
            }
            catch
            {
                return string.Empty;
            }
        }
        public static string ToString(char value)
        {
            try
            {
                return Convert.ToString(value);
            }
            catch
            {
                return string.Empty;
            }
        }
        public static string ToString(DateTime value, string format = null)
        {
            try
            {
                return IsNull(format) ? value.ToString(DiagConstants.DefaultDateFormat) : value.ToString(format);
            }
            catch
            {
                return string.Empty;
            }
        }
        public static string ToString(decimal value)
        {
            try
            {
                return Convert.ToString(value);
            }
            catch
            {
                return string.Empty;
            }
        }
        public static string ToString(double value)
        {
            try
            {
                return Convert.ToString(value);
            }
            catch
            {
                return string.Empty;
            }
        }
        public static string ToString(float value)
        {
            try
            {
                return Convert.ToString(value);
            }
            catch
            {
                return string.Empty;
            }
        }
        public static string ToString(int value)
        {
            try
            {
                return Convert.ToString(value);
            }
            catch
            {
                return string.Empty;
            }
        }
        public static string ToString(long value)
        {
            try
            {
                return Convert.ToString(value);
            }
            catch
            {
                return string.Empty;
            }
        }
        public static string ToString(object value)
        {
            try
            {
                return IsNull(value) ? string.Empty : value.ToString().Trim();
            }
            catch
            {
                return string.Empty;
            }
        }
        public static string ToString(sbyte value)
        {
            try
            {
                return Convert.ToString(value);
            }
            catch
            {
                return string.Empty;
            }
        }
        public static string ToString(short value)
        {
            try
            {
                return Convert.ToString(value);
            }
            catch
            {
                return string.Empty;
            }
        }
        public static string ToString(string value)
        {
            try
            {
                return IsNull(value) ? string.Empty : value.Trim();
            }
            catch
            {
                return string.Empty;
            }
        }
        public static string ToString(uint value)
        {
            try
            {
                return Convert.ToString(value);
            }
            catch
            {
                return string.Empty;
            }
        }
        public static string ToString(ulong value)
        {
            try
            {
                return Convert.ToString(value);
            }
            catch
            {
                return string.Empty;
            }
        }
        public static string ToString(ushort value)
        {
            try
            {
                return Convert.ToString(value);
            }
            catch
            {
                return string.Empty;
            }
        }
        public static string ToString(XAttribute value)
        {
            try
            {
                var clone = value as XAttribute;
                return IsNull(clone) ? string.Empty : clone.Value.Trim();
            }
            catch
            {
                return string.Empty;
            }
        }
        public static byte ToByte(object obj)
        {
            try
            {
                return IsNull(obj) ? (byte)0 : byte.Parse(obj.ToString().Trim().Replace(",", ""));
            }
            catch
            {
                return (byte)0;
            }
        }
        public static decimal ToDecimal(object obj)
        {
            try
            {
                return IsNull(obj) ? (decimal)0 : decimal.Parse(obj.ToString().Trim().Replace(",", ""));
            }
            catch
            {
                return (decimal)0;
            }
        }
        public static double ToDouble(object obj)
        {
            try
            {
                return IsNull(obj) ? 0.0 : double.Parse(obj.ToString().Trim().Replace(",", ""));
            }
            catch
            {
                return 0.0;
            }
        }
        public static float ToFloat(object obj)
        {
            try
            {
                return IsNull(obj) ? (float)0 : float.Parse(obj.ToString().Trim().Replace(",", ""));
            }
            catch
            {
                return (float)0;
            }
        }
        public static int ToInt(object obj)
        {
            try
            {
                return IsNull(obj) ? 0 : int.Parse(obj.ToString().Trim().Replace(",", ""));
            }
            catch
            {
                return 0;
            }
        }
        public static long ToLong(object obj)
        {
            try
            {
                return IsNull(obj) ? (long)0 : long.Parse(obj.ToString().Trim().Replace(",", ""));
            }
            catch
            {
                return (long)0;
            }
        }
        public static short ToShort(object obj)
        {
            try
            {
                return IsNull(obj) ? (short)0 : short.Parse(obj.ToString().Trim().Replace(",", ""));
            }
            catch
            {
                return (short)0;
            }
        }
        public static DateTime ToDateTime(string value)
        {
            try
            {
                return IsNull(value) ? DateTime.MinValue : DateTime.Parse(value);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
        public static DateTime ToDateTime(object value)
        {
            try
            {
                return IsNull(value) ? DateTime.MinValue : DateTime.Parse(DCC.ToString(value));
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
        public static DateTime ToDateTime(string value, string format)
        {
            try
            {
                return IsNull(value) ? DateTime.MinValue : DateTime.ParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
        public static DateTime ToDateTime(string value, string[] format)
        {
            try
            {
                return IsNull(value) ? DateTime.MinValue : IsNull(format) ? DateTime.ParseExact(value, DiagConstants.AvailableDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None) : DateTime.ParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
        public static DateTime ToDateTime(double julianDate)
        {
            try
            {
                return DateTime.FromOADate(julianDate - 2415018.5);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
        public static double ToJulianDate(DateTime dateTime)
        {
            try
            {
                return dateTime.ToOADate() + 2415018.5;
            }
            catch
            {
                return 0.0;
            }
        }
        public static string MoneyToString(decimal number)
        {
            string s = number.ToString("#");
            string[] so = new string[] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] hang = new string[] { "", "nghìn", "triệu", "tỷ" };
            int i, j, donvi, chuc, tram;
            string str = " ";
            bool booAm = false;
            decimal decS = 0;
            try
            {
                decS = Convert.ToDecimal(s.ToString());
            }
            catch
            {
            }
            if (decS < 0)
            {
                decS = -decS;
                s = decS.ToString();
                booAm = true;
            }
            i = s.Length;
            if (i == 0)
                str = so[0] + str;
            else
            {
                j = 0;
                while (i > 0)
                {
                    donvi = Convert.ToInt32(s.Substring(i - 1, 1));
                    i--;
                    if (i > 0)
                        chuc = Convert.ToInt32(s.Substring(i - 1, 1));
                    else
                        chuc = -1;
                    i--;
                    if (i > 0)
                        tram = Convert.ToInt32(s.Substring(i - 1, 1));
                    else
                        tram = -1;
                    i--;
                    if ((donvi > 0) || (chuc > 0) || (tram > 0) || (j == 3))
                        str = hang[j] + str;
                    j++;
                    if (j > 3) j = 1;
                    if ((donvi == 1) && (chuc > 1))
                        str = "một " + str;
                    else
                    {
                        if ((donvi == 5) && (chuc > 0))
                            str = "lăm " + str;
                        else if (donvi > 0)
                            str = so[donvi] + " " + str;
                    }
                    if (chuc < 0)
                        break;
                    else
                    {
                        if ((chuc == 0) && (donvi > 0)) str = "lẻ " + str;
                        if (chuc == 1) str = "mười " + str;
                        if (chuc > 1) str = so[chuc] + " mươi " + str;
                    }
                    if (tram < 0) break;
                    else
                    {
                        if ((tram > 0) || (chuc > 0) || (donvi > 0)) str = so[tram] + " trăm " + str;
                    }
                    str = " " + str;
                }
            }
            if (booAm) str = "Âm " + str;
            return str + "đồng chẵn";
        }
        public static LogTrace ILogger = new LogTrace(DiagConstants.LogTraceSource);
    }
    public sealed class DiagConstants
    {
        #region PROJECT CONSTANTS

        public const string LogTraceSource = "Teller Plugin";
        public const string ProtectDataKey = "FISBANK TELLER PLUGIN";
        public const string DefaultDateFormat = "yyyy-MM-dd hh:mm:ss tt";
        public static readonly string[] AvailableDateFormat = { "dd/MM/yyyy", "dd/M/yyyy", "d/MM/yyyy", "d/M/yyyy", "dd-MM-yyyy", "dd-M-yyyy", "d-MM-yyyy", "d-M-yyyy", "dd/MMM/yyyy", "d/MMM/yyyy", "MMM/dd/yyyy", "MMM/d/yyyy", "dd-MMM-yyyy", "d-MMM-yyyy", "MMM-dd-yyyy", "MMM-d-yyyy", "ddMMMyyyy", "dMMMyyyy", "MMMddyyyy", "MMMdyyyy", "ddMMyyyy", "ddMyyyy", "dMMyyyy", "dMyyyy" };

        #endregion

        #region SOLUTION CONSTANTS

        /// <summary>
        /// The application connect to ServiceMaster.
        /// </summary>
        public const string Application = "Plugin Singletons";
        /// <summary>
        /// The TellerPlugin.exe file.
        /// </summary>
        public const string appPlugin = "TellerPlugin.exe";
        /// <summary>
        /// The TellerPlugin.exe.config file.
        /// </summary>
        public const string appPluginConfigFile = "TellerPlugin.exe.config";
        /// <summary>
        /// The PluginConnector.exe file.
        /// </summary>
        public const string appConnector = "PluginConnector.exe";
        /// <summary>
        /// The Fisbank.Cbs.AutoInstaller.exe file.
        /// </summary>
        public const string appAutoInstaller = "Fisbank.Cbs.AutoInstaller.exe";
        /// <summary>
        /// The Version.xml file.
        /// </summary>
        public const string appVersionFile = "Version.xml";
        /// <summary>
        /// The App_Layouts folder.
        /// </summary>
        public const string connectorFolder = ".\\App_Layouts";
        /// <summary>
        /// The Connector.config file.
        /// </summary>
        public const string connectorFileName = ".\\App_Layouts\\Connector.config";
        /// <summary>
        /// The App_Layouts folder.
        /// </summary>
        public const string dataFolder = ".\\App_Layouts";
        /// <summary>
        /// The BAK.DAT file.
        /// </summary>
        public const string dataFileName = ".\\App_Layouts\\BAK.DAT";
        /// <summary>
        /// Procedure to cleanup the logs backup of database.
        /// </summary>
        public const string CleanUpLogsBackup = "fsp_PurgeLogsBackup";
        /// <summary>
        /// Batch schedule.
        /// </summary>
        public const string ScheBatch = "BATCH";
        /// <summary>
        /// Cleanup schedule.
        /// </summary>
        public const string ScheCleanup = "CLEANUP";
        /// <summary>
        /// Download  schedule.
        /// </summary>
        public const string ScheDownload = "DOWNLOAD";
        /// </summary>
        /// The pattern of time 12 hours.
        /// </summary>
        public const string TIME12HOURS_PATTERN = "(1[012]|0[1-9]):[0-5][0-9](\\s)(?-i)(AM|PM)";
        /// <summary>
        /// [0] = Sunday.
        /// [1] = Monday.
        /// [2] = Tuesday.
        /// [3] = Wednesday.
        /// [4] = Thursday.
        /// [5] = Friday.
        /// [6] = Saturday.
        /// </summary>
        public static readonly string[] ARR_DAYOFWEEK = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
        /// <summary>
        /// [0] = Daily.
        /// [1] = Weekly.
        /// [2] = Monthly.
        /// </summary>
        public static readonly string[] ARR_FREQUENCY = { "Daily", "Weekly", "Monthly" };
        /// <summary>
        /// [0] = Teller Plugin. 
        /// [1] = Teller for Signature.
        /// [2] = Teller for Admin
        /// </summary>
        //public static readonly string[] allConnection = { "CBSPlugin", "CBSTeller" , "CBSTellerAdmin" };
        public static readonly string[] allConnection = { "CBSPlugin", "CBSTeller" };

        #endregion
    }
    public static class Extensions
    {
        #region Convert Money To Money Name
        public static string ToMoneyName(this decimal gNum, string CurrencySymbol)
        {
            if (gNum == 0)
                return "Không đồng";
            string Number = gNum.ToString("0.00");
            string rightside = string.Empty;
            string leftside = string.Empty;
            if (Number.Contains('.'))
            {
                var arr = Number.Split('.');
                rightside = arr[1];
                leftside = arr[0];
            }
            else
            {
                leftside = Number;
            }
            string leftResult = ToMoneyName(leftside);
            string rightResult = ToMoneyName(rightside.TrimEnd('0'));
            if (rightResult.IsNotNull())
            {
                return leftResult.Trim() + " và " + rightResult.Trim();
            }
            else
            {
                return leftResult;
            }

        }
        public static string ToMoneyName(this decimal gNum)
        {
            if (gNum == 0)
                return "Không đồng";
            string lso_chu = "";
            string tach_mod = "";
            string tach_conlai = "";
            decimal Num = Math.Round(gNum, 0);
            string gN = Convert.ToString(Num);
            int m = Convert.ToInt32(gN.Length / 3);
            int mod = gN.Length - m * 3;
            string dau = "[+]";

            // Dau [+ , - ]
            if (Num < 0)
                dau = "[-]";
            dau = "";

            // Tach hang lon nhat
            if (mod.Equals(1))
                tach_mod = "00" + Convert.ToString(Num.ToString().Trim().Substring(0, 1)).Trim();
            if (mod.Equals(2))
                tach_mod = "0" + Convert.ToString(Num.ToString().Trim().Substring(0, 2)).Trim();
            if (mod.Equals(0))
                tach_mod = "000";
            // Tach hang con lai sau mod :
            if (Num.ToString().Length > 2)
                tach_conlai = Convert.ToString(Num.ToString().Trim().Substring(mod, Num.ToString().Length - mod)).Trim();

            ///don vi hang mod
            int im = m + 1;
            if (mod > 0)
                lso_chu = Tach(tach_mod).ToString().Trim() + " " + Donvi(im.ToString().Trim());
            /// Tach 3 trong tach_conlai

            int i = m;
            int _m = m;
            int j = 1;
            string tach3 = "";
            string tach3_ = "";

            while (i > 0)
            {
                tach3 = tach_conlai.Trim().Substring(0, 3).Trim();
                tach3_ = tach3;
                lso_chu = lso_chu.Trim() + " " + Tach(tach3.Trim()).Trim();
                m = _m + 1 - j;
                if (!tach3_.Equals("000"))
                    lso_chu = lso_chu.Trim() + " " + Donvi(m.ToString().Trim()).Trim();
                tach_conlai = tach_conlai.Trim().Substring(3, tach_conlai.Trim().Length - 3);

                i = i - 1;
                j = j + 1;
            }
            if (lso_chu.Trim().Substring(0, 1).Equals("k"))
                lso_chu = lso_chu.Trim().Substring(10, lso_chu.Trim().Length - 10).Trim();
            if (lso_chu.Trim().Substring(0, 1).Equals("l"))
                lso_chu = lso_chu.Trim().Substring(2, lso_chu.Trim().Length - 2).Trim();
            if (lso_chu.Trim().Length > 0)
                lso_chu = dau.Trim() + " " + lso_chu.Trim().Substring(0, 1).Trim().ToUpper() + lso_chu.Trim().Substring(1, lso_chu.Trim().Length - 1).Trim() + " đồng";
            return lso_chu.ToString().Trim();

        }
        public static string ToMoneyName(string gNum)
        {
            if (gNum.IsNull())
                return "";
            string lso_chu = "";
            string tach_mod = "";
            string tach_conlai = "";
            decimal Num = gNum.ToDecimal();
            string gN = Convert.ToString(Num);
            int m = Convert.ToInt32(gN.Length / 3);
            int mod = gN.Length - m * 3;
            string dau = "[+]";

            // Dau [+ , - ]
            if (Num < 0)
                dau = "[-]";
            dau = "";

            // Tach hang lon nhat
            if (mod.Equals(1))
                tach_mod = "00" + Convert.ToString(Num.ToString().Trim().Substring(0, 1)).Trim();
            if (mod.Equals(2))
                tach_mod = "0" + Convert.ToString(Num.ToString().Trim().Substring(0, 2)).Trim();
            if (mod.Equals(0))
                tach_mod = "000";
            // Tach hang con lai sau mod :
            if (Num.ToString().Length > 2)
                tach_conlai = Convert.ToString(Num.ToString().Trim().Substring(mod, Num.ToString().Length - mod)).Trim();

            ///don vi hang mod
            int im = m + 1;
            if (mod > 0)
                lso_chu = Tach(tach_mod).ToString().Trim() + " " + Donvi(im.ToString().Trim());
            /// Tach 3 trong tach_conlai

            int i = m;
            int _m = m;
            int j = 1;
            string tach3 = "";
            string tach3_ = "";

            while (i > 0)
            {
                tach3 = tach_conlai.Trim().Substring(0, 3).Trim();
                tach3_ = tach3;
                lso_chu = lso_chu.Trim() + " " + Tach(tach3.Trim()).Trim();
                m = _m + 1 - j;
                if (!tach3_.Equals("000"))
                    lso_chu = lso_chu.Trim() + " " + Donvi(m.ToString().Trim()).Trim();
                tach_conlai = tach_conlai.Trim().Substring(3, tach_conlai.Trim().Length - 3);

                i = i - 1;
                j = j + 1;
            }
            if (lso_chu.Trim().Substring(0, 1).Equals("k"))
                lso_chu = lso_chu.Trim().Substring(10, lso_chu.Trim().Length - 10).Trim();
            if (lso_chu.Trim().Substring(0, 1).Equals("l"))
                lso_chu = lso_chu.Trim().Substring(2, lso_chu.Trim().Length - 2).Trim();
            if (lso_chu.Trim().Length > 0)
                lso_chu = dau.Trim() + " " + lso_chu.Trim().Substring(0, 1).Trim().ToUpper() + lso_chu.Trim().Substring(1, lso_chu.Trim().Length - 1).Trim() + " đồng";
            return lso_chu.ToString().Trim();
        }
        private static string Donvi(string so)
        {
            string Kdonvi = "";

            if (so.Equals("1"))
                Kdonvi = "";
            if (so.Equals("2"))
                Kdonvi = "nghìn";
            if (so.Equals("3"))
                Kdonvi = "triệu";
            if (so.Equals("4"))
                Kdonvi = "tỷ";
            if (so.Equals("5"))
                Kdonvi = "nghìn tỷ";
            if (so.Equals("6"))
                Kdonvi = "triệu tỷ";
            if (so.Equals("7"))
                Kdonvi = "tỷ tỷ";

            return Kdonvi;
        }
        private static string Tach(string tach3)
        {
            string Ktach = "";
            if (tach3.Equals("000"))
                return "";
            if (tach3.Length == 3)
            {
                string tr = tach3.Trim().Substring(0, 1).ToString().Trim();
                string ch = tach3.Trim().Substring(1, 1).ToString().Trim();
                string dv = tach3.Trim().Substring(2, 1).ToString().Trim();
                if (tr.Equals("0") && ch.Equals("0"))
                    Ktach = " không trăm lẻ " + Chu(dv.ToString().Trim()) + " ";
                if (!tr.Equals("0") && ch.Equals("0") && dv.Equals("0"))
                    Ktach = Chu(tr.ToString().Trim()).Trim() + " trăm ";
                if (!tr.Equals("0") && ch.Equals("0") && !dv.Equals("0"))
                    Ktach = Chu(tr.ToString().Trim()).Trim() + " trăm lẻ " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && dv.Equals("0"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && dv.Equals("5"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi lăm ";
                if (tr.Equals("0") && ch.Equals("1") && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = " không trăm mười " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && ch.Equals("1") && dv.Equals("0"))
                    Ktach = " không trăm mười ";
                if (tr.Equals("0") && ch.Equals("1") && dv.Equals("5"))
                    Ktach = " không trăm mười lăm ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi " + Chu(dv.Trim()).Trim() + " ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && dv.Equals("0"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi lăm ";
                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười " + Chu(dv.Trim()).Trim() + " ";

                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && dv.Equals("0"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười ";
                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười lăm ";

            }


            return Ktach;

        }
        private static string Chu(string gNumber)
        {
            string result = "";
            switch (gNumber)
            {
                case "0":
                    result = "không";
                    break;
                case "1":
                    result = "một";
                    break;
                case "2":
                    result = "hai";
                    break;
                case "3":
                    result = "ba";
                    break;
                case "4":
                    result = "bốn";
                    break;
                case "5":
                    result = "năm";
                    break;
                case "6":
                    result = "sáu";
                    break;
                case "7":
                    result = "bảy";
                    break;
                case "8":
                    result = "tám";
                    break;
                case "9":
                    result = "chín";
                    break;
            }
            return result;
        }
        #endregion
        public static bool IsNull<T>(this T obj) where T : class
        {
            if (obj == null) return true;
            else
            {
                if (typeof(T) == typeof(string))
                    return string.IsNullOrWhiteSpace(obj as string);
                else if (typeof(T) == typeof(DataSet))
                    return ((obj as DataSet).Tables.Count == 0);
                else if (typeof(T) == typeof(DataTable))
                    return ((obj as DataTable).Rows.Count == 0);
                else if (typeof(T) == typeof(Hashtable))
                    return ((obj as Hashtable).Count == 0);
                else if (typeof(T) == typeof(ArrayList))
                    return ((obj as ArrayList).Count == 0);
                else if (typeof(T) == typeof(Array))
                    return ((obj as Array).Length == 0);
                else if (typeof(T) == typeof(IList))
                    return ((obj as IList).Count == 0);
                else if (typeof(T) == typeof(IEnumerable<T>))
                    return ((obj as IEnumerable<T>).Count() == 0);
                else
                    return false;
            }
        }
        public static List<T> Clone<T>(this List<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
        public static bool IsNotNull<T>(this T obj) where T : class
        {
            if (obj == null) return false;
            else
            {
                if (typeof(T) == typeof(string))
                    return !string.IsNullOrWhiteSpace(obj as string);
                else if (typeof(T) == typeof(DataSet))
                    return ((obj as DataSet).Tables.Count > 0);
                else if (typeof(T) == typeof(DataTable))
                    return ((obj as DataTable).Rows.Count > 0);
                else if (typeof(T) == typeof(Hashtable))
                    return ((obj as Hashtable).Count > 0);
                else if (typeof(T) == typeof(ArrayList))
                    return ((obj as ArrayList).Count > 0);
                else if (typeof(T) == typeof(Array))
                    return ((obj as Array).Length > 0);
                else if (typeof(T) == typeof(IList))
                    return ((obj as IList).Count > 0);
                else if (typeof(T) == typeof(IEnumerable<T>))
                    return ((obj as IEnumerable<T>).Count() > 0);
                else
                    return true;
            }
        }
        public static bool IsNull<T>(this T[] obj)
        {
            return (obj == null || obj.Length == 0);
        }
        public static bool IsNull<T>(this List<T> obj)
        {
            return (obj == null || obj.Count == 0);
        }
        public static string ToUnSign(this string obj)
        {
            if (obj.IsNull()) return string.Empty;
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = obj.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, string.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
        public static int CastToInt(this object obj)
        {
            if (obj == null) return -1;
            else
            {
                return (int)obj;
            }
        }
        public static string ToDateTimeFormart(this string obj)
        {
            if (obj == null)
                return "00/00/0000";
            else
            {
                var date = DCC.ToDateTime(obj, "ddMMyyyy");
                if (date != DateTime.MinValue && date.Year > 1900)
                    return date.ToString("dd/MM/yyyy");
                else
                    return DCC.ToDateTime(obj, "yyyyMMdd").ToString("dd/MM/yyyy");

            }
        }
        public static byte ToByte(this object obj)
        {
            try
            {
                return obj.IsNull() ? (byte)0 : byte.Parse(obj.ToString().Trim().Replace(",", ""));
            }
            catch
            {
                return (byte)0;
            }
        }
        public static decimal ToDecimal(this object obj)
        {
            try
            {
                return obj.IsNull() ? (decimal)0 : decimal.Parse(obj.ToString().Trim().Replace(",", ""));
            }
            catch
            {
                return (decimal)-1;
            }
        }
        public static double ToDouble(this object obj)
        {
            try
            {
                return obj.IsNull() ? 0.00 : double.Parse(obj.ToString().Trim().Replace(",", ""));
            }
            catch
            {
                return -1.00;
            }
        }
        public static float ToFloat(this object obj)
        {
            try
            {
                return obj.IsNull() ? (float)0 : float.Parse(obj.ToString().Trim().Replace(",", ""));
            }
            catch
            {
                return (float)-1;
            }
        }
        public static int ToInt(this object obj)
        {
            try
            {
                return obj.IsNull() ? 0 : int.Parse(obj.ToString().Trim().Replace(",", ""));
            }
            catch
            {
                return -1;
            }
        }
        public static long ToLong(this object obj)
        {
            try
            {
                return obj.IsNull() ? (long)0 : long.Parse(obj.ToString().Trim().Replace(",", ""));
            }
            catch
            {
                return (long)-1;
            }
        }
        public static short ToShort(this object obj)
        {
            try
            {
                return obj.IsNull() ? (short)0 : short.Parse(obj.ToString().Trim().Replace(",", ""));
            }
            catch
            {
                return (short)-1;
            }
        }
        public static double ToJulianDate(this DateTime obj)
        {
            try
            {
                return obj.ToOADate() + 2415018.5;
            }
            catch
            {
                return 0.0;
            }
        }
        public static DateTime ToDateTime(this double obj)
        {
            try
            {
                return DateTime.FromOADate(obj - 2415018.5);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
        public static DateTime ToDateTime(this string obj)
        {
            try
            {
                return obj.IsNull() ? DateTime.MinValue : DateTime.Parse(obj.Trim());
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
        public static DateTime ToDateTime(this string obj, string format)
        {
            try
            {
                return obj.IsNull() ? DateTime.MinValue : DateTime.ParseExact(obj.Trim(), format, CultureInfo.InvariantCulture, DateTimeStyles.None);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
        public static DateTime ToDateTime(this string obj, string[] format)
        {
            try
            {
                return obj.IsNull() ? DateTime.MinValue : format.IsNull() ? DateTime.ParseExact(obj.Trim(), DiagConstants.AvailableDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None) : DateTime.ParseExact(obj.Trim(), format, CultureInfo.InvariantCulture, DateTimeStyles.None);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
        public static string[] Split(this string obj, string substr)
        {
            return (obj == null) ? new string[] { string.Empty } : obj.Split(new string[] { substr }, StringSplitOptions.None);
        }
    }
    public sealed class ComboBoxItem
    {
        public string Name;
        public string Value;
        public ComboBoxItem(string Name, string Value)
        {
            this.Name = Name;
            this.Value = Value;
        }
        public override string ToString()
        {
            return this.Name;
        }
        public static string GetValue(ComboBox cbo)
        {
            return ((ComboBoxItem)cbo.SelectedItem).Value;
        }
        public static string GetDisplay(ComboBox cbo)
        {
            return ((ComboBoxItem)cbo.SelectedItem).ToString();
        }
    }
}
