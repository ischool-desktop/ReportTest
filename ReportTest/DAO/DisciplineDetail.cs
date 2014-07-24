using ReportTest.framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FISCA.Data;

namespace ReportTest.DAO
{
    /// <summary>
    /// 獎懲明細
    /// </summary>
    [MargeClass(Name = "獎懲明細")]
    public class DisciplineDetail : MargeGroup
    {

        /// <summary>
        /// 傳入條件文字
        /// </summary>
        private string _OptionText = "";

        [MargeField(FieldName = "學年度", FieldType = "Int")]
        public int? SchoolYear;

        [MargeField(FieldName = "學期", FieldType = "Int")]
        public int? Semester;

        [MargeField(FieldName = "獎懲發生開始日期",FieldType="DateTime")]
        public DateTime? beginDate;

        [MargeField(FieldName = "獎懲發生結束日期", FieldType = "DateTime")]
        public DateTime? endDate;

        public List<string> Fields
        {
            get { return new List<string>(new string[] { "獎懲學年度", "獎懲學期","獎懲年級","獎懲發生日期", "嘉獎", "小功", "大功", "警告", "小過", "大過", "銷過", "銷過日期", "事由","獎懲登錄日期" }); }
        }

        public List<string> GroupKeys
        {
            get { return new List<string>(new string[] { "獎懲學年度", "獎懲學期","獎懲年級", "獎懲發生日期", "嘉獎", "小功", "大功", "警告", "小過", "大過", "銷過", "銷過日期", "事由", "獎懲登錄日期" }); }
        }

        public DataTable BuildMargeData(IEnumerable<string> keys)
        {
            DataTable dt = new DataTable();
            List<string> keyList = new List<string>();
            foreach (string key in keys)
                keyList.Add(key);

            // 當沒有資料
            if (keyList.Count == 0)
                return dt;

            _OptionText = "";

            if (SchoolYear.HasValue)
                _OptionText = "and discipline.school_year=" + SchoolYear.Value;

            if(SchoolYear.HasValue && Semester.HasValue)
                _OptionText = "and discipline.school_year=" + SchoolYear.Value + " and discipline.semester=" + Semester.Value;

            if(beginDate.HasValue && endDate.HasValue)
                _OptionText = "and discipline.occur_date>='" + string.Format("{0:yyyy-MM-dd}", beginDate.Value) + "' and discipline.occur_date<'" + string.Format("{0:yyyy-MM-dd}", endDate.Value.AddDays(1)) + "'";

            string queryKey = string.Join(",", keyList.ToArray());
            string query1 = @"select discipline.ref_student_id as id,discipline.school_year as 獎懲學年度,discipline.semester as 獎懲學期,g1.GradeYear as 獎懲年級
,to_char(occur_date,'YYYY/MM/DD') as 獎懲發生日期,to_char(register_date,'YYYY/MM/DD') as 獎懲登錄日期,reason as 事由,CAST(regexp_replace( xpath_string(discipline.detail,'/Discipline/Merit/@A'), '^$', '0')as integer) as 大功,
CAST(regexp_replace( xpath_string(discipline.detail,'/Discipline/Merit/@B'), '^$', '0') as integer) as 小功,
CAST(regexp_replace( xpath_string(discipline.detail,'/Discipline/Merit/@C'), '^$', '0') as integer) as 嘉獎,
CAST(regexp_replace( xpath_string(discipline.detail,'/Discipline/Demerit/@A'), '^$', '0') as integer) as 大過,
CAST(regexp_replace( xpath_string(discipline.detail,'/Discipline/Demerit/@B'), '^$', '0') as integer) as 小過
,CAST(regexp_replace( xpath_string(discipline.detail,'/Discipline/Demerit/@C'), '^$', '0') as integer) as 警告,
regexp_replace( xpath_string(discipline.detail,'/Discipline/Demerit/@Cleared'), '^$', '') as 銷過
,regexp_replace( xpath_string(discipline.detail,'/Discipline/Demerit/@ClearDate'), '^$', '') as 銷過日期
from discipline left join (select id,smh.SchoolYear,smh.semester,smh.GradeYear from 
xpath_table('id','''<root>''||sems_history||''</root>''','student',
'/root/History/@SchoolYear|/root/History/@Semester|/root/History/@GradeYear','id in(" + queryKey + @")') AS 
smh(id int, SchoolYear integer,Semester int,GradeYear int)) as g1 on discipline.school_year=g1.SchoolYear and discipline.semester=g1.Semester 
and discipline.ref_student_id=g1.id where discipline.ref_student_id in("+queryKey+@") " + _OptionText + " order by discipline.ref_student_id,occur_date ";

            QueryHelper qhq1 = new QueryHelper();
            DataTable qhdt1 = qhq1.Select(query1);
            dt.Columns.Add("ID");
            foreach (string Field in Fields)
                dt.Columns.Add(Field);

            foreach (DataRow dr in qhdt1.Rows)
            { 
             // 填值
                dt.Rows.Add("" + dr["id"]
                      , dr["獎懲學年度"]
                    , dr["獎懲學期"]
                    , dr["獎懲年級"]
                    , dr["獎懲發生日期"]
                    , dr["嘉獎"]
                    , dr["小功"]
                    , dr["大功"]
                    , dr["警告"]
                    , dr["小過"]
                    , dr["大過"]
                    , dr["銷過"]
                    , dr["銷過日期"]
                    , dr["事由"]
                    ,dr["獎懲登錄日期"]);
            }
           
            return dt;
        }
    }
}
