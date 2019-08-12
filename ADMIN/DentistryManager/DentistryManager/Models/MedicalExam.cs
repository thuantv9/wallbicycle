using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DentistryManager.Models
{
    /// <summary>
    /// Lớp này mô tả danh sách các kết quả khám bệnh gợi ý cho bác sĩ chọn.
    /// </summary>
    public class MedicalExam
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
}