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
        public DateTime birthday { get; set; }
        public string address { get; set; }
        public string image { get; set; }
        public bool gender { get; set; }
        public string telephone { get; set; }
        public int age { get; set; }
        public string email { get; set; }
        public string metadata { get; set; }
        public bool status { get; set; }
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
}