using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DentistryManager.Models
{
    /// <summary>
    /// Lớp này mô tả về các thủ thuật điểu trị, lớp này đặc biệt quan trọng
    /// </summary>
    public class MedicalProcedure
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        // Tổng số tiền chi cho thủ thuật này
        public long amount { get; set; }
        // loại thủ thuật: 1 lần hay nhiều lần (cái này thiết lập trong bảng AllDataJson
        public string idcategory { get; set; }
        // Số lần điều trị: thủ thuật 1 lần sẽ là 1, thủ thuật nhiều lần sẽ là nhiều
        public int numberoftreatment { get; set; }
        // -- thủ thuật này sẽ dùng cho các kết quả khám nào, ví dụ Exam01|Exam02 với Exam01: sâu răng, Exam02: viêm lợi. (liên kết bảng MedicalExam)    
        public string idcause { get; set; }
        // -- mô tả chi tiết các bước điều trị: trường này sẽ lưu trữ json của các bước điều trị ví dụ 4 bước thì sẽ bao gồm
        public string steptreatment { get; set; }
    }

    /// <summary>
    /// -- idstep : mã bước
	/// -- namestep: tên bước
    /// -- distancestep: thời gian từ bước trước (vd: 15 ngày)
	/// -- descriptionstep: mô tả bước
	/// -- amountstep: số tiền phải trả khi thực hiện bước này.
    /// -- Tổng amountstep phải bằng amount.
    /// </summary>
    public class StepTreatment
    {
        public string idstep { get; set; }
        public string namestep { get; set; }
        public int distancestep { get; set; }
        public string descriptionstep { get; set; }
        public long amountstep { get; set; }
    }
}