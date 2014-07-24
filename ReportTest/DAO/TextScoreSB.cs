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
    /// 文字評量與導師評語
    /// </summary>
    [MargeClass(Name="文字評量與導師評語")]
    public class TextScoreSB:MargeGroup
    {

        /// <summary>
        /// 傳入條件文字
        /// </summary>
        private string _OptionText = "";

        [MargeField(FieldName = "學年度", FieldType = "Int")]
        public int? SchoolYear;

        [MargeField(FieldName = "學期", FieldType = "Int")]
        public int? Semester;

        List<string> _FieldList = new List<string>();
       
        public List<string> Fields
        {
            get { return _FieldList; }
        }

        public TextScoreSB()
        {
            _FieldList = GetFieldNames();        
        }

        private List<string> GetFieldNames()
        {
            string bname = "導師評語";
            List<string> nameList = new List<string>();
            if (SchoolYear.HasValue && Semester.HasValue)
            {
                // 
            }
            else
            {
                nameList.Add("文字評量學年度");
                nameList.Add("文字評量學期");
                nameList.Add("文字評量年級");
            }
            string query1 = @"select tname from xpath_table('name','content','list','/Request/Content/Morality/@Face','name=''文字評量代碼表''') as tmp(name character varying(20),tname character varying(50))";
            QueryHelper qh1 = new QueryHelper();
            DataTable dt1 = qh1.Select(query1);

            foreach (DataRow row in dt1.Rows)
                nameList.Add(row["tname"].ToString());

            if (!nameList.Contains(bname))
                nameList.Add(bname);

            return nameList;
        }

        public List<string> GroupKeys
        {            
            get {
                if (SchoolYear.HasValue && Semester.HasValue)
                    return new List<string>();
                else
                    return _FieldList;
            }
        }

        public System.Data.DataTable BuildMargeData(IEnumerable<string> keys)
        {
            DataTable dt = new DataTable();
            // 當沒有資料
            if (keys.Count() == 0)
                return dt;

            List<string> keyList = new List<string>();
            foreach (string key in keys)
                keyList.Add(key);

            dt.Columns.Add("ID");
            foreach (string colName in Fields)
                dt.Columns.Add(colName);

            string queryKey = string.Join(",", keyList.ToArray());

            if (SchoolYear.HasValue)
            {
                _OptionText = " where s1.school_year="+SchoolYear.Value;
            }

            if (SchoolYear.HasValue && Semester.HasValue)
            {
                _OptionText = " where s1.school_year="+SchoolYear.Value+" and s1.semester="+Semester.Value;
            }

            // 取得導師評語
            string query1 = @"select s1.tid,s1.sid,g1.schoolyear as 文字評量學年度,g1.semester as 文字評量學期,g1.gradeyear as 
文字評量年級,s1.sb_comment as 導師評語 from (select id,smh.SchoolYear,smh.semester,smh.GradeYear from xpath_table('id','''<root>''||
sems_history||''</root>''','student','/root/History/@SchoolYear|/root/History/@Semester|/root/History/@GradeYear','id in("+queryKey+@")') 
AS smh(id int, SchoolYear integer,Semester int,GradeYear int)) as g1 inner join (select id as tid,ref_student_id as sid,school_year,
semester,sb_comment from sems_moral_score where ref_student_id="+queryKey+@") as s1 on g1.schoolyear=s1.school_year and g1.semester=s1.semester 
and g1.id = s1.sid "+_OptionText+" order by s1.sid,s1.school_year,s1.semester";

            QueryHelper qh1 = new QueryHelper();
            DataTable dt1 = qh1.Select(query1);
            
            // 取的文字評量
            string query2 = @"select t1.id as tid,face,facevalue from xpath_table('id','text_score','sems_moral_score','/Content/Morality
/@Face|/Content/Morality','ref_student_id in(" + queryKey + @")') as t1(id int,face character varying(50),facevalue character varying(100)) 
inner join sems_moral_score as s1 on t1.id=s1.id " + _OptionText;

            QueryHelper qh2 = new QueryHelper();
            DataTable dt2 = qh2.Select(query2);

            // 整理資料
            Dictionary<string, DataRow> tmpDataDict = new Dictionary<string, DataRow>();

            foreach (DataRow dr in dt1.Rows)
            {
                string key = dr["tid"].ToString();
                DataRow newRow = dt.NewRow();
                newRow["ID"] = dr["sid"].ToString();

                foreach (DataColumn dc in dt1.Columns)
                {
                    if (Fields.Contains(dc.ColumnName))
                        newRow[dc.ColumnName] = dr[dc.ColumnName];
                }

                if (!tmpDataDict.ContainsKey(key))
                    tmpDataDict.Add(key, newRow);
            }

            foreach (DataRow dr1 in dt2.Rows)
            {
                string key = dr1["tid"].ToString();
                if (tmpDataDict.ContainsKey(key))
                {
                    string face = dr1["face"].ToString();
                    if (Fields.Contains(face))
                        tmpDataDict[key][face] = dr1["facevalue"];                   
                }
            }

            foreach (DataRow dr in tmpDataDict.Values)
            {
                dt.Rows.Add(dr);
            }

            return dt;
        }
    }
}
