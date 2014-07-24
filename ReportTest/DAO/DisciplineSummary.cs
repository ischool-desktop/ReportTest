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
    /// 獎懲統計
    /// </summary>
    [MargeClass(Name = "獎懲統計")]
    public class DisciplineSummary : MargeGroup
    {
        /// <summary>
        /// 傳入條件文字
        /// </summary>
        private string _OptionText = "";

        [MargeField(FieldName = "學年度", FieldType = "Int")]
        public int? SchoolYear;

        [MargeField(FieldName = "學期", FieldType = "Int")]
        public int? Semester;

        public List<string> Fields
        {
            get { return new List<string>(new string[] { "大功支數", "小功支數", "嘉獎支數", "大過支數", "小過支數", "警告支數" }); }
        }

        public List<string> GroupKeys
        {
            get { return new List<string>(); }
        }

        public System.Data.DataTable BuildMargeData(IEnumerable<string> keys)
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
                _OptionText = " and discipline.school_year=" + SchoolYear.Value;

            if(SchoolYear.HasValue && Semester.HasValue)
                _OptionText = " and discipline.school_year=" + SchoolYear.Value +" and discipline.semester="+Semester.Value ;

            string queryKey = string.Join(",", keyList.ToArray());
            string query = @"select s1.ref_student_id as id,CASE WHEN sum(大功) is null THEN 0 ELSE sum(大功) END as 大功支數,CASE WHEN sum(小功) is null THEN 0 ELSE 
sum(小功) END as 小功支數,CASE WHEN sum(嘉獎) is null THEN 0 ELSE sum(嘉獎) END as 嘉獎支數,CASE WHEN sum(大過) is null THEN 0 ELSE sum(大過) END 
as 大過支數,CASE WHEN sum(小過) is null THEN 0 ELSE sum(小過) END as 小過支數,CASE WHEN sum(警告) is null THEN 0 ELSE sum(警告) END as 警告支數 from (select  discipline.ref_student_id,
CAST(regexp_replace( xpath_string(discipline.detail,'/Discipline/Merit/@A'), '^$', '0')
as integer) as 大功,CAST(regexp_replace( xpath_string(discipline.detail,'/Discipline/Merit/@B'), '^$', '0') as integer) as 小功,
CAST(regexp_replace( xpath_string(discipline.detail,'/Discipline/Merit/@C'), '^$', '0') as integer) as 嘉獎,
CAST(regexp_replace( xpath_string(discipline.detail,'/Discipline/Demerit/@A'), '^$', '0') as integer) as 大過,
CAST(regexp_replace( xpath_string(discipline.detail,'/Discipline/Demerit/@B'), '^$', '0') as integer) as 小過,
CAST(regexp_replace( xpath_string(discipline.detail,'/Discipline/Demerit/@C'), '^$', '0') as integer) as 警告 
from discipline where discipline.ref_student_id in("+queryKey+") "+_OptionText+") as s1 group by  s1.ref_student_id order by  s1.ref_student_id ";

            QueryHelper qh = new QueryHelper();
            DataTable qdt = qh.Select(query);
            dt.Columns.Add("ID");
            foreach(string Field in Fields)
                dt.Columns.Add(Field);            

            foreach (DataRow dr in qdt.Rows)
            {
                // 填值
                dt.Rows.Add("" + dr["id"]      
            , dr["大功支數"]
            , dr["小功支數"]
            , dr["嘉獎支數"]
            , dr["大過支數"]
            , dr["小過支數"]
            , dr["警告支數"]);
            }
            return dt;
        }
    }
}
