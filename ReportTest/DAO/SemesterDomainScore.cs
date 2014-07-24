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
    /// 國中學期領域成績
    /// </summary>
    [MargeClass(Name = "國中學期領域成績")]
    public class SemesterDomainScore : MargeGroup
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
            get { return new List<string>(new string[] { "學期領域學年度", "學期領域學期", "學期領域年級", "學期領域名稱", "學期領域節數", "學期領域權數", "學期領域成績", "學期領域努力程度", "學期領域文字描述" }); }
        }

        public List<string> GroupKeys
        {
            get { return new List<string>(new string[] { "學期領域學年度", "學期領域學期", "學期領域年級", "學期領域名稱", "學期領域節數", "學期領域權數", "學期領域成績", "學期領域努力程度", "學期領域文字描述" }); }
        }

        public System.Data.DataTable BuildMargeData(IEnumerable<string> keys)
        {
            DataTable dt = new DataTable();
            List<string> keyList = new List<string>();
            foreach (string key in keys)
                keyList.Add(key);

            _OptionText = " order by g1.SchoolYear,g1.Semester,g1.id,s2.學期領域名稱";

            if(SchoolYear.HasValue && Semester.HasValue)
                _OptionText = " and g1.SchoolYear=" + SchoolYear.Value + " and g1.Semester=" + Semester.Value + " order by g1.SchoolYear,g1.Semester,g1.id,s2.領域";

            // 當沒有資料
            if (keyList.Count == 0)
                return dt;

            dt.Columns.Add("ID");
            foreach (string str in Fields)
                dt.Columns.Add(str);

            string queryKey = string.Join(",", keys.ToArray());

            string query1 = @"select g1.id,g1.SchoolYear as 學期領域學年度,g1.Semester as 學期領域學期,g1.GradeYear as 學期領域年級,s2.學期領域努力程度,s2.學期領域成績,s2.學期領域文字描述,
s2.學期領域權數,s2.學期領域節數,s2.學期領域註記,s2.學期領域名稱 from (select id,smh.SchoolYear,smh.semester,smh.GradeYear from xpath_table('id'
,'''<root>''||sems_history||''</root>''','student','/root/History/@SchoolYear|/root/History/@Semester|/root/History/@GradeYear','id in(" + queryKey + @")') 
            AS smh(id int, SchoolYear integer,Semester int,GradeYear int)) as g1 inner join (select ref_student_id,school_year,semester,s1.* 
from sems_subj_score inner join (select id,cast(regexp_replace(d1, '^$', '0') as decimal)as 學期領域努力程度,cast(regexp_replace(d2, '^$', '0') 
as decimal)as 學期領域成績,d3 as 學期領域文字描述,cast(regexp_replace(d4, '^$', '0') as decimal)as 學期領域權數,cast(regexp_replace(d5, '^$', '0') as decimal) as 學期領域節數
,d6 as 學期領域註記,d7 as 學期領域名稱 from xpath_table('id','''<root>''||score_info||''</root>''','sems_subj_score','/root/Domains/Domain/@努力程度|
/root/Domains/Domain/@成績|/root/Domains/Domain/@文字描述|/root/Domains/Domain/@權數|/root/Domains/Domain/@節數|/root/Domains/Domain/@註記|
/root/Domains/Domain/@領域','ref_student_id in("+queryKey+@")') as sd1(id integer,d1 character varying(6),d2 character varying(10),d3 character varying(500),
d4 character varying(6),d5 character varying(6),d6 character varying(30),d7  character varying(30))) as s1 on sems_subj_score.id=s1.id)
as s2 on g1.id=s2.ref_student_id and g1.SchoolYear=s2.school_year and g1.Semester=s2.semester where sems_subj_score.ref_student_id in("+queryKey+")" + _OptionText;

            QueryHelper qh1 = new QueryHelper();
            DataTable dt1 = qh1.Select(query1);

            foreach (DataRow dr in dt1.Rows)
            {
                dt.Rows.Add(
                    dr["id"]
                    , dr["學期領域學年度"]
                    , dr["學期領域學期"]
                    , dr["學期領域年級"]
                    , dr["學期領域名稱"]
                    , dr["學期領域節數"]
                    , dr["學期領域權數"]
                    , dr["學期領域成績"]
                    , dr["學期領域努力程度"]
                    , dr["學期領域文字描述"]);    
            }

            return dt;
        }
    }
}
