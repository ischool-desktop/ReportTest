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
    /// 高中學期科目成績
    /// </summary>
    [MargeClass(Name = "高中學期科目成績")]
    public class SemesterSubjectScoreSH : MargeGroup
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
            get { return new List<string>(new string[] { "學期科目成績學年度", "學期科目成績學期", "學期科目成績年級", "學期科目名稱", "學期科目級別", "學期科目不計學分", "學期科目不需評分", "學期科目修課必選修", "學期科目修課校部訂", "學期科目原始成績", "學期科目學年調整成績", "學期科目擇優採計成績", "學期科目是否取得學分", "學期科目補考成績", "學期科目重修成績", "學期科目開課分項類別", "學期科目開課學分數", "學期科目成績班排名", "學期科目成績班排名母數", "學期科目成績科排名", "學期科目成績科排名母數", "學期科目成績校排名", "學期科目成績校排名母數" }); }
        }

        public List<string> GroupKeys
        {
            get { return new List<string>(new string[] { "學期科目成績學年度", "學期科目成績學期", "學期科目成績年級", "學期科目名稱", "學期科目級別", "學期科目不計學分", "學期科目不需評分", "學期科目修課必選修", "學期科目修課校部訂", "學期科目原始成績", "學期科目學年調整成績", "學期科目擇優採計成績", "學期科目是否取得學分", "學期科目補考成績", "學期科目重修成績", "學期科目開課分項類別", "學期科目開課學分數", "學期科目成績班排名", "學期科目成績班排名母數", "學期科目成績科排名", "學期科目成績科排名母數", "學期科目成績校排名", "學期科目成績校排名母數" }); }
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

            _OptionText = " order by sid,sems_subj_score.school_year,sems_subj_score.semester";

            if(SchoolYear.HasValue && Semester.HasValue)
            _OptionText = " and sems_subj_score.school_year=" + SchoolYear.Value + " and sems_subj_score.semester=" + Semester.Value + " order by sid,sems_subj_score.school_year,sems_subj_score.semester";

            dt.Columns.Add("ID");
            foreach (string str in Fields)
                dt.Columns.Add(str);

            string queryKey = string.Join(",", keyList.ToArray());

            string query1 = @"select sems_subj_score.id,sems_subj_score.ref_student_id as sid,
sems_subj_score.school_year as 學期科目成績學年度,
sems_subj_score.semester as 學期科目成績學期,
sems_subj_score.grade_year as 學期科目成績年級,
s0.d1 as 學期科目名稱,
s0.d2 as 學期科目級別,
s0.d3 as 學期科目不計學分,
s0.d4 as 學期科目不需評分,
s0.d5 as 學期科目修課必選修,
s0.d6 as 學期科目修課校部訂,
s0.d10 as 學期科目是否取得學分,
CAST(regexp_replace(s0.d7, '^$', '0') as decimal) as 學期科目原始成績,
CAST(regexp_replace(s0.d8, '^$', '0') as decimal) as 學期科目學年調整成績,
CAST(regexp_replace(s0.d9, '^$', '0') as decimal) as 學期科目擇優採計成績,
CAST(regexp_replace(s0.d11, '^$', '0') as decimal) as 學期科目補考成績,
CAST(regexp_replace(s0.d12, '^$', '0') as decimal) as 學期科目重修成績,
s0.d13 as 學期科目開課分項類別,
CAST(regexp_replace(s0.d14, '^$', '0') as decimal) as 學期科目開課學分數,
CAST(regexp_replace(s1.d2, '^$', '0') as decimal) as 學期科目成績班排名,
CAST(regexp_replace(s1.d3, '^$', '0') as decimal) as 學期科目成績班排名母數,
CAST(regexp_replace(s2.d2, '^$', '0') as decimal) as 學期科目成績科排名,
CAST(regexp_replace(s2.d3, '^$', '0') as decimal) as 學期科目成績科排名母數,
CAST(regexp_replace(s2.d2, '^$', '0') as decimal) as 學期科目成績校排名,
CAST(regexp_replace(s2.d3, '^$', '0') as decimal) as 學期科目成績校排名母數  from sems_subj_score inner join xpath_table('id','score_info','sems_subj_score','/SemesterSubjectScoreInfo/Subject/@科目
|/SemesterSubjectScoreInfo/Subject/@科目級別
|/SemesterSubjectScoreInfo/Subject/@不計學分
|/SemesterSubjectScoreInfo/Subject/@不需評分
|/SemesterSubjectScoreInfo/Subject/@修課必選修
|/SemesterSubjectScoreInfo/Subject/@修課校部訂
|/SemesterSubjectScoreInfo/Subject/@原始成績
|/SemesterSubjectScoreInfo/Subject/@學年調整成績
|/SemesterSubjectScoreInfo/Subject/@擇優採計成績
|/SemesterSubjectScoreInfo/Subject/@是否取得學分
|/SemesterSubjectScoreInfo/Subject/@補考成績
|/SemesterSubjectScoreInfo/Subject/@重修成績
|/SemesterSubjectScoreInfo/Subject/@開課分項類別
|/SemesterSubjectScoreInfo/Subject/@開課學分數'
,'ref_student_id in(" + queryKey+@")')
 as s0(id integer,d1 character varying(30),d2 character varying(30),d3 character varying(30),d4 character varying(30),d5 character varying(30),d6 character varying(30),d7 character varying(30),d8 character varying(30),d9 character varying(30),d10 character varying(30),d11 character varying(30),d12 character varying(30),d13 character varying(30),d14 character varying(30))
 on sems_subj_score.id=s0.id left join xpath_table('id','class_rating','sems_subj_score','/Rating/Item/@科目|/Rating/Item/@排名|/Rating/Item/@成績人數','ref_student_id in("+queryKey+@")')
 as s1(id integer,d1 character varying(30),d2 character varying(30),d3 character varying(30))
 on s0.id=s1.id and s0.d1=s1.d1 left join xpath_table('id','dept_rating','sems_subj_score','/Rating/Item/@科目|/Rating/Item/@排名|/Rating/Item/@成績人數','ref_student_id in("+queryKey+@")')
 as s2(id integer,d1 character varying(30),d2 character varying(30),d3 character varying(30))
 on s1.id=s2.id and s1.d1=s2.d1 left join xpath_table('id','year_rating','sems_subj_score','/Rating/Item/@科目|/Rating/Item/@排名|/Rating/Item/@成績人數','ref_student_id in("+queryKey+@")')
 as s3(id integer,d1 character varying(30),d2 character varying(30),d3 character varying(30))
 on s1.id=s3.id and s1.d1=s3.d1 where sems_subj_score.ref_student_id in("+queryKey+@") "+_OptionText;

             QueryHelper qh1 = new QueryHelper();            
            DataTable dt1 = qh1.Select(query1);

            foreach (DataRow dr in dt1.Rows)
            {
                dt.Rows.Add(
                    dr["sid"]
                    , dr["學期科目成績學年度"]
                    , dr["學期科目成績學期"]
                    , dr["學期科目成績年級"]
                    , dr["學期科目名稱"]
                    , dr["學期科目級別"]
                    , dr["學期科目不計學分"]
                    , dr["學期科目不需評分"]
                    , dr["學期科目修課必選修"]
                    , dr["學期科目修課校部訂"]
                    , dr["學期科目原始成績"]
                    , dr["學期科目學年調整成績"]
                    , dr["學期科目擇優採計成績"]
                    , dr["學期科目是否取得學分"]
                    , dr["學期科目補考成績"]
                    , dr["學期科目重修成績"]
                    , dr["學期科目開課分項類別"]
                    , dr["學期科目開課學分數"]
                    , dr["學期科目成績班排名"]
                    , dr["學期科目成績班排名母數"]
                    , dr["學期科目成績科排名"]
                    , dr["學期科目成績科排名母數"]
                    , dr["學期科目成績校排名"]
                    , dr["學期科目成績校排名母數"]
                    );
            }

            return dt;
        }
    }
}
