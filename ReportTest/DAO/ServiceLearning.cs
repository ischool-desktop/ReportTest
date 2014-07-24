using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReportTest.framework;
using FISCA.Data;
using System.Data;

namespace ReportTest.DAO
{
    /// <summary>
    /// 服務學習時數
    /// </summary>
    [MargeClass(Name="服務學習")]
    public class ServiceLearning:MargeGroup
    {
        /// <summary>
        /// 傳入條件文字
        /// </summary>
        private string _OptionText = "";

        [MargeField(FieldName = "學年度", FieldType = "Int")]
        public int? SchoolYear;

        [MargeField(FieldName = "學期", FieldType = "Int")]
        public int? Semester;

        [MargeField(FieldName = "開始日期", FieldType = "DateTime")]
        public DateTime? beginDate;

        [MargeField(FieldName = "結束日期", FieldType = "DateTime")]
        public DateTime? endDate;


        public List<string> Fields
        {
            get { return new List<string>(new string[] { "服務學習學年度", "服務學習學期", "服務學習年級", "服務學習發生日期", "服務學習事由", "服務學習時數", "服務學習主辦單位", "服務學習登錄日期", "服務學習備註" }); }
        }

        public List<string> GroupKeys
        {
            get { return new List<string>(new string[] { "服務學習學年度", "服務學習學期", "服務學習年級", "服務學習發生日期", "服務學習事由", "服務學習時數", "服務學習主辦單位", "服務學習登錄日期", "服務學習備註" }); }
        }

        DataTable MargeGroup.BuildMargeData(IEnumerable<string> keys)
        {
            _OptionText = "";

            // 處理參數學年度(只有學年度)
            if (SchoolYear.HasValue)
            {
                _OptionText = "where g1.school_year=" + SchoolYear.Value;
            }

            // 處理參數學年度、學期
            if (SchoolYear.HasValue && Semester.HasValue)
            {
                _OptionText = "where g1.school_year=" + SchoolYear.Value + " and g1.semester=" + Semester.Value;
            }

            // 處理參數開始日期、結束日期
            if (beginDate.HasValue && endDate.HasValue)
            {
                _OptionText = "and s1.occur_date>='" + string.Format("{0:yyyy-MM-dd}", beginDate.Value) + "' and  s1.occur_date<'" + string.Format("{0:yyyy-MM-dd}", endDate.Value.AddDays(1)) + "'";
            }

            DataTable dt = new DataTable();
            List<string> keyList = new List<string>();
            foreach (string key in keys)
                keyList.Add(key);

            // 當沒有資料
            if (keyList.Count == 0)
                return dt;

            List<string> columnsList = new List<string>();
            dt.Columns.Add("ID");
            foreach (string name in Fields)
                dt.Columns.Add(name);            

            string query1 = @"select g1.id,g1.school_year as 服務學習學年度,g1.semester as 服務學習學期,g1.grade_year as 服務學習年級,occur_date as 服務學習發生日期
,reason as 服務學習事由,hours as 服務學習時數,organizers as 服務學習主辦單位,register_date as 服務學習登錄日期,remark as 服務學習備註 
from (select id,smh.school_year,smh.semester,smh.grade_year from xpath_table('id','''<root>''||sems_history||''</root>''','student',
'/root/History/@SchoolYear|/root/History/@Semester|/root/History/@GradeYear','id in("+string.Join(",",keyList.ToArray())+@")') AS smh(id int, school_year 
integer,semester int,grade_year int)) as g1 inner join (select cast(ref_student_id as int) as id,school_year,semester,occur_date,
reason,hours,organizers,register_date,remark from $k12.service.learning.record where ref_student_id in('"+string.Join("','",keyList.ToArray())+@"')) 
as s1 on g1.id=s1.id and g1.school_year=s1.school_year and g1.semester=s1.semester " + _OptionText + " order by g1.id,g1.school_year,g1.semester";

            QueryHelper qh = new QueryHelper();
            DataTable dt1 = qh.Select(query1);

            foreach (DataRow dr in dt1.Rows)
            {
                dt.Rows.Add(
                    dr["id"]
                    , dr["服務學習學年度"]
                    , dr["服務學習學期"]
                    , dr["服務學習年級"]
                    , dr["服務學習發生日期"]
                    , dr["服務學習事由"]
                    , dr["服務學習時數"]
                    , dr["服務學習主辦單位"]
                    , dr["服務學習登錄日期"]
                    , dr["服務學習備註"]
                    );            
            }
            return dt;
        }
    }
}
