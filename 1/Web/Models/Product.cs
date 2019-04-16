using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Web.Models
{
    public class Product
    {
        public string GenerateItemNameAsParam()
        {
            string phrase = string.Format("{0}-{1}", Id, Seo);// Creates in the specific pattern  
            string str = GetByteArray(phrase).ToLower();
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");// Remove invalid characters for param  
            str = Regex.Replace(str, @"\s+", "-").Trim(); // convert multiple spaces into one hyphens   
            str = str.Substring(0, str.Length <= 30 ? str.Length : 30).Trim(); //Trim to max 30 char  
            str = Regex.Replace(str, @"\s", "-"); // Replaces spaces with hyphens     
            return str;
        }

        private string GetByteArray(string text)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(text);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string MadeFrom { get; set; }
        public int CategoryId { get; set; }
        public int Quantity { get; set; }
        public decimal Value { get; set; }
        public string Tag { get; set; }
        public string Image { get; set; }
        public string Remark { get; set; }
        public Boolean Status { get; set; }
        public string Seo { get; set; }
    }
}