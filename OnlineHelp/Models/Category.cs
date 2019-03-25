using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineHelp.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public int Level { get; set; }
        public string CategoryName { get; set; }
        public int ParentCategoryID { get; set; }
        public string MappingScreen { get; set; }
        public DateTime EditDate { get; set; }
        public string Editor { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
    }
    public class MappingScreen
    {
        public string ScreenID { get; set; }
        public string ScreenName { get; set; }
    }
}