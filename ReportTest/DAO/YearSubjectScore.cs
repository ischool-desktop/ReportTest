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
    /// 高中學年科目成績
    /// </summary>
    [MargeClass(Name = "高中學年科目成績")]
    public class YearSubjectScore : MargeGroup
    {
        /// <summary>
        /// 傳入條件文字
        /// </summary>
        private string _OptionText = "";

        [MargeField(FieldName = "學年度", FieldType = "Int")]
        public int? SchoolYear;


        public List<string> Fields
        {
            get { return new List<string>(new string[] { "學年科目成績學年度", "學年科目成績年級", "學年科目名稱", "學年科目成績", "學年科目成績班排名", "學年科目成績班排名母數", "學年科目成績科排名", "學年科目成績科排名母數", "學年科目成績校排名", "學年科目成績校排名母數" }); }
        }

        public List<string> GroupKeys
        {
            get { return new List<string>(new string[] { "學年科目成績學年度", "學年科目成績年級", "學年科目名稱", "學年科目成績", "學年科目成績班排名", "學年科目成績班排名母數", "學年科目成績科排名", "學年科目成績科排名母數", "學年科目成績校排名", "學年科目成績校排名母數" }); }
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

            _OptionText = " order by sid,year_subj_score.school_year";

            if(SchoolYear.HasValue)
                _OptionText = "and year_subj_score.school_year=" + SchoolYear + " order by sid,year_subj_score.school_year";

            dt.Columns.Add("ID");
            foreach (string str in Fields)
                dt.Columns.Add(str);

            string queryKey = string.Join(",", keyList.ToArray());
            string query1 = @"select year_subj_score.id,year_subj_score.ref_student_id as sid,year_subj_score.school_year as 學年科目成績學年度,
            year_subj_score.grade_year as 學年科目成績年級,s0.d1 as 學年科目名稱,CAST(regexp_replace(s0.d2, '^$', '0') as decimal) as 學年科目成績
,CAST(regexp_replace(s1.d2, '^$', '0') as decimal) as 學年科目成績班排名,CAST(regexp_replace(s1.d3, '^$', '0') as decimal) as 學年科目成績班排名母數
,CAST(regexp_replace(s2.d2, '^$', '0') as decimal) as 學年科目成績科排名,CAST(regexp_replace(s2.d3, '^$', '0') as decimal) as 學年科目成績科排名母數
,CAST(regexp_replace(s2.d2, '^$', '0') as decimal) as 學年科目成績校排名,CAST(regexp_replace(s2.d3, '^$', '0') as decimal) as 學年科目成績校排名母數
from year_subj_score inner join xpath_table('id','score_info','year_subj_score','/SchoolYearSubjectScore/Subject/@科目|/SchoolYearSubjectScore/Subject/@學年成績','ref_student_id in(" + queryKey + @")')
as s0(id integer,d1 character varying(30),d2 character varying(30)) on year_subj_score.id=s0.id left join xpath_table('id','class_rating',
'year_subj_score','/Rating/Item/@科目|/Rating/Item/@排名|/Rating/Item/@成績人數','ref_student_id in(" + queryKey +@")') as s1(id integer,
d1 character varying(30),d2 character varying(30),d3 character varying(30)) on s0.id=s1.id and s0.d1=s1.d1 left join xpath_table('id',
'dept_rating','year_subj_score','/Rating/Item/@科目|/Rating/Item/@排名|/Rating/Item/@成績人數','ref_student_id in(" + queryKey + @")') 
as s2(id integer,d1 character varying(30),d2 character varying(30),d3 character varying(30)) on s1.id=s2.id and s1.d1=s2.d1 left join 
xpath_table('id','year_rating','year_subj_score','/Rating/Item/@科目|/Rating/Item/@排名|/Rating/Item/@成績人數','ref_student_id in(" + queryKey + @")') 
as s3(id integer,d1 character varying(30),d2 character varying(30),d3 character varying(30)) on s1.id=s3.id and s1.d1=s3.d1 where 
year_subj_score.ref_student_id in(" + queryKey + @") " + _OptionText;
            
            QueryHelper qh1 = new QueryHelper();            
            DataTable dt1 = qh1.Select(query1);

            foreach (DataRow dr in dt1.Rows)
            {
                dt.Rows.Add(
                    dr["sid"]
                    , dr["學年科目成績學年度"]
                    , dr["學年科目成績年級"]
                    , dr["學年科目名稱"]
                    , dr["學年科目成績"]
                    , dr["學年科目成績班排名"]
                    , dr["學年科目成績班排名母數"]
                    , dr["學年科目成績科排名"]
                    , dr["學年科目成績科排名母數"]
                    , dr["學年科目成績校排名"]
                    , dr["學年科目成績校排名母數"]
                    );
            }

            return dt;
        }
    }
}
