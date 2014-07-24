using FISCA.Data;
using ReportTest.framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ReportTest.DAO
{
    /// <summary>
    /// 高中學期分項成績
    /// </summary>
    [MargeClass(Name = "高中學期分項成績")]
    public class SemesterEntryScore : MargeGroup
    {
        /// <summary>
        /// 傳入條件文字
        /// </summary>
        private string _OptionText = "";

        /// <summary>
        /// 內容項目與欄位對照
        /// </summary>
        private Dictionary<string, string> _FieldDict;

        [MargeField(FieldName = "學年度", FieldType = "Int")]
        public int? SchoolYear;

        [MargeField(FieldName = "學期", FieldType = "Int")]
        public int? Semester;

        public List<string> Fields
        {
            get { return _FieldDict.Values.ToList(); }
        }


        public List<string> GroupKeys
        {
            get { return _FieldDict.Values.ToList(); }
        }

        /// <summary>
        /// 欄位對照表
        /// </summary>
        private void LoadFields()
        {
            if (_FieldDict == null)
            {
                _FieldDict = new Dictionary<string, string>();
                _FieldDict.Add("學年度", "學期分項成績學年度");
                _FieldDict.Add("學期", "學期分項成績學期");
                _FieldDict.Add("年級", "學期分項成績年級");
                _FieldDict.Add("學業", "學期學業成績");
                _FieldDict.Add("體育", "學期體育成績");
                _FieldDict.Add("健康與護理", "學期健康與護理成績");
                _FieldDict.Add("國防通識", "學期國防通識成績");
                _FieldDict.Add("實習科目", "學期實習科目成績");
                _FieldDict.Add("專業科目", "學期專業科目成績");
                _FieldDict.Add("學業(原始)", "學期學業原始成績");
                _FieldDict.Add("體育(原始)", "學期體育原始成績");
                _FieldDict.Add("健康與護理(原始)", "學期健康與護理原始成績");
                _FieldDict.Add("國防通識(原始)", "學期國防通識原始成績");
                _FieldDict.Add("實習科目(原始)", "學期實習科目原始成績");
                _FieldDict.Add("專業科目(原始)", "學期專業科目原始成績");
                _FieldDict.Add("學業班排名", "學期學業班排名");
                _FieldDict.Add("學業班排名母數", "學期學業班排名母數");
                _FieldDict.Add("學業科排名", "學期學業科排名");
                _FieldDict.Add("學業科排名母數", "學期學業科排名母數");
                _FieldDict.Add("學業校排名", "學期學業校排名");
                _FieldDict.Add("學業校排名母數", "學期學業校排名母數");
            }
        }


        public System.Data.DataTable BuildMargeData(IEnumerable<string> keys)
        {
            LoadFields();

            DataTable dt = new DataTable();
            List<string> keyList = new List<string>();
            foreach (string key in keys)
                keyList.Add(key);

            dt.Columns.Add("ID");

            foreach (string field in Fields)
                dt.Columns.Add(field);

            // 當沒有資料
            if (keyList.Count == 0)
                return dt;           

            _OptionText = "";

            if (SchoolYear.HasValue && Semester.HasValue)
                _OptionText = "and sems_entry_score.school_year=" + SchoolYear.Value + " and  sems_entry_score.semester=" + Semester.Value;


            string queryKey=string.Join(",",keyList.ToArray());
            // 取得分項成績
            string query1 = @"select sems_entry_score.id||'_'||se1.d1 as key,sems_entry_score.id as seid,ref_student_id as sid,school_year 
as 學年度,semester as 學期,grade_year as 年級,se1.d1 as 分項, cast(regexp_replace(se1.d2, '^$', '0') as decimal) as 成績 
from sems_entry_score inner join xpath_table('id','score_info','sems_entry_score','/SemesterEntryScore/Entry/@分項|/SemesterEntryScore/Entry/@成績',
'ref_student_id in("+queryKey+@")') as se1(id integer,d1 character varying(30),d2 character varying(10)) 
            on sems_entry_score.id=se1.id where sems_entry_score.ref_student_id in("+queryKey+") and sems_entry_score.entry_group=1 " + _OptionText;

            // 取得學業分項排名
            string query2 = @"select id,(case xpath_string(class_rating,'/Rating/Item/@分項') when '學業' then xpath_string(class_rating,'/Rating/Item/@排名') end) as 學業班排名,
(case xpath_string(class_rating,'/Rating/Item/@分項') when '學業' then xpath_string(class_rating,'/Rating/Item/@成績人數') end) as 學業班排名母數,
(case xpath_string(dept_rating,'/Rating/Item/@分項') when '學業' then xpath_string(dept_rating,'/Rating/Item/@排名') end) as 學業科排名,
(case xpath_string(dept_rating,'/Rating/Item/@分項') when '學業' then xpath_string(dept_rating,'/Rating/Item/@成績人數') end) as 學業科排名母數,
(case xpath_string(year_rating,'/Rating/Item/@分項') when '學業' then xpath_string(year_rating,'/Rating/Item/@排名') end) as 學業校排名,
(case xpath_string(year_rating,'/Rating/Item/@分項') when '學業' then xpath_string(year_rating,'/Rating/Item/@成績人數') end) as 學業校排名母數 from sems_entry_score where sems_entry_score.ref_student_id in(" + queryKey + ") and sems_entry_score.entry_group=1 " + _OptionText;

            QueryHelper qh1 = new QueryHelper();
            DataTable dt1= qh1.Select(query1);
            QueryHelper qh2 = new QueryHelper();
            DataTable dt2 = qh2.Select(query2);


            Dictionary<string, DataRow> data1 = new Dictionary<string, DataRow>();

            // 處理成績
            foreach (DataRow dr in dt1.Rows)
            {
                string key = dr["seid"].ToString();
                if (!data1.ContainsKey(key))
                {
                    DataRow newRow = dt.NewRow();
                    newRow["ID"] = dr["sid"].ToString();
                    newRow["學期分項成績學年度"] = dr["學年度"].ToString();
                    newRow["學期分項成績學期"] = dr["學期"].ToString();
                    newRow["學期分項成績年級"] = dr["年級"].ToString();
                    data1.Add(key, newRow);
                }

                string item = dr["分項"].ToString();

                if (_FieldDict.ContainsKey(item))
                    data1[key][_FieldDict[item]] = dr["成績"];
            }

            List<string> tmpItemList = new List<string>();
            tmpItemList.Add("學業班排名");
            tmpItemList.Add("學業班排名母數");
            tmpItemList.Add("學業科排名");
            tmpItemList.Add("學業科排名母數");
            tmpItemList.Add("學業校排名");
            tmpItemList.Add("學業校排名母數");


            // 處理排名填值
            foreach (DataRow dr in dt2.Rows)
            {
                string key = dr["id"].ToString();

                if (data1.ContainsKey(key))
                {
                    foreach (string item in tmpItemList)                    
                        if(_FieldDict.ContainsKey(item))
                            data1[key][_FieldDict[item]]=dr[item];   
                }
            }

            foreach (string key in data1.Keys)
                dt.Rows.Add(data1[key]);

            return dt;
        }
    }
}
