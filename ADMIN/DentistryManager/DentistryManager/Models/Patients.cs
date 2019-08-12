using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DentistryManager.Models
{
    public class Patients
    {
        public string id { get; set; }
        public string name { get; set; }
        public int yearofbirth { get; set; }
        public string address { get; set; }
        public string image { get; set; }
        public bool gender { get; set; }
        public string telephone { get; set; }
        public int age { get; set; }
        public string email { get; set; }
        public string metadata { get; set; }
        public string medicalalert { get; set; }
        public string medicalhistory { get; set; }
        public string examjson { get; set; }
        public string treatmentjson { get; set; }
        public string paymentjson { get; set; }
        public string status { get; set; }
        public string statusinaday { get; set; }
        public bool active { get; set; }
    }

    public class MetaData
    {
        private Dictionary<string, string> extendDict { get; set; }
        public string GetValueFromExtendDict(string key)
        {
            if (extendDict.ContainsKey(key))
                return extendDict[key];
            else
                return string.Empty;
        }
        public void SetValueToExtendDict(string key, string value)
        {
            extendDict[key] = value;
        }
    }

    public class Exam
    {
        public DateTime datetime { get; set; }
        public string examresult { get; set; }       
    }
    public class Treatment
    {
        public DateTime datetime { get; set; }
       // danh sách các thủ thuật điều trị, định dạng trường này như sau: Procedure01|Procedure02
       // các mã thủ thuật này link đến bảng MedicalProcedure
        public string idtreatment { get; set; }
    }

    public class Payment
    {
        public DateTime datetime { get; set; }
        public string idpayment { get; set; }
    }
    // Ý tưởng thiết kế bảng Thanh toán cũng như bảng thủ thuât với thanh toán 1 lần và thanh toán nhiều lần
}