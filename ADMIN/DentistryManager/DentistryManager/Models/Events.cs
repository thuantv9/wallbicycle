using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DentistryManager.Models
{
    /// <summary>
    /// Class nay dung de luu Lich hen roi truyen cho FullCalendar qua Json
    /// </summary>
    public class Events 
    {
        [Required]
        public  string id { get;  set; }
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string url { get; set; }
        public bool allDay { get; set; }
        public Events()
        {
            url = string.Empty;
            allDay = false;
        }
    }

}