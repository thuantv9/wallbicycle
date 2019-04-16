using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class News
    {
        public int NewsId { get; set; }
        public string NewsName { get; set; }
        public string NewsImage { get; set; }
        public string NewsDescription { get; set; }
        public string NewsRemark { get; set; }
        public string NewsMadeby { get; set; }
      
    }
}