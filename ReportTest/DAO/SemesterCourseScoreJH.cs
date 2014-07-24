using ReportTest.framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using FISCA.Data;

namespace ReportTest.DAO
{
    /// <summary>
    /// 國中學期課程學習成績
    /// </summary>
    [MargeClass(Name = "國中學期課程學習成績")]
    public class SemesterCourseScoreJH : MargeGroup
    {
        /// <summary>
        /// 傳入條件文字
        /// </summary>
        private string _OptionText = "";

        [MargeField(FieldName = "學年度",FieldType="Int")]
        public int? SchoolYear;

        [MargeField(FieldName = "學期", FieldType = "Int")]
        public int? Semester;

        public List<string> Fields
        {
            get { return new List<string>(new string[] { "學期課程學年度", "學期課程學期", "學期課程年級", "學習領域成績", "課程學習成績" }); }
        }

        public List<string> GroupKeys
        {
            get { return new List<string>(new string[] { "學期課程學年度", "學期課程學期", "學期課程年級", "學習領域成績", "課程學習成績" }); }
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

            _OptionText = " order by g1.id,g1.SchoolYear,g1.Semester";

            if (SchoolYear.HasValue && SchoolYear.HasValue)
            {
                _OptionText = " and g1.SchoolYear=" + SchoolYear.Value + " and g1.Semester=" + Semester.Value + " order by g1.id";
            }

            dt.Columns.Add("ID");
            foreach (string str in Fields)
                dt.Columns.Add(str);

            string queryKey = string.Join(",", keyList.ToArray());
            string query1 = @"select id,g1.SchoolYear as 學期課程學年度,g1.Semester as 學期課程學期,g1.GradeYear as 學期課程年級,g2.學習領域成績,g2.課程學習成績 
from (select id,smh.SchoolYear,smh.semester,smh.GradeYear from xpath_table('id','''<root>''||sems_history||''</root>''','student',
'/root/History/@SchoolYear|/root/History/@Semester|/root/History/@GradeYear','id in(" +queryKey+@")') AS 
            smh(id int, SchoolYear integer,Semester int,GradeYear int)) as g1 inner join (select ref_student_id,school_year,semester,
CAST(regexp_replace( xpath_string('<root>'||score_info||'</root>','/root/LearnDomainScore'), '^$', '0') as decimal) as 學習領域成績,
CAST(regexp_replace( xpath_string('<root>'||score_info||'</root>','/root/CourseLearnScore'), '^$', '0')as decimal) as 課程學習成績 
from sems_subj_score) as g2 on g1.SchoolYear=g2.school_year and g1.Semester=g2.semester and g1.id=g2.ref_student_id where sems_subj_score.ref_student_id in("+queryKey+") " + _OptionText;

            QueryHelper qh = new QueryHelper();
            DataTable dt1 = qh.Select(query1);

            foreach (DataRow dr in dt1.Rows)
            {
                // 填值
                dt.Rows.Add("" + dr["id"]
             , dr["學期課程學年度"]
            , dr["學期課程學期"]
            , dr["學期課程年級"]
            , dr["學習領域成績"]
            , dr["課程學習成績"]);
            }
            return dt;
        }
    }
}
