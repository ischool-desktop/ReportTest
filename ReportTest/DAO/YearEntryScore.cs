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
    /// 高中學年分項成績
    /// </summary>
    [MargeClass(Name = "高中學年分項成績")]
    public class YearEntryScore : MargeGroup
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
                _FieldDict.Add("學年分項學年度", "學年分項學年度");
                _FieldDict.Add("學年分項年級", "學年分項年級");
                _FieldDict.Add("學業", "學年學業成績");
                _FieldDict.Add("體育", "學年體育成績");
                _FieldDict.Add("健康與護理", "學年健康與護理成績");
                _FieldDict.Add("國防通識", "學年國防通識成績");
                _FieldDict.Add("實習科目", "學年實習科目成績");
                _FieldDict.Add("專業科目", "學年專業科目成績");
                _FieldDict.Add("學業班排名", "學年學業班排名");
                _FieldDict.Add("學業班排名母數", "學年學業班排名母數");
                _FieldDict.Add("學業科排名", "學年學業科排名");
                _FieldDict.Add("學業科排名母數", "學年學業科排名母數");
                _FieldDict.Add("學業校排名", "學年學業校排名");
                _FieldDict.Add("學業校排名母數", "學年學業校排名母數");               
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

            if(SchoolYear.HasValue)
                _OptionText = " and year_entry_score.school_year=" + SchoolYear.Value;

            string queryKey = string.Join(",", keyList.ToArray());

            // 取得分項成績
            string query1 = @"select year_entry_score.id||'_'||ye1.d1 as key,year_entry_score.id as seid,ref_student_id as sid,school_year 
as 學年分項學年度,grade_year as 學年分項年級,ye1.d1 as 分項, cast(regexp_replace(ye1.d2, '^$', '0') as decimal) as 成績 
from year_entry_score inner join xpath_table('id','score_info','year_entry_score','/SchoolYearEntryScore/Entry/@分項|/SchoolYearEntryScore/Entry/@成績',
'ref_student_id in(" + queryKey + @")') as ye1(id integer,d1 character varying(30),d2 character varying(10)) 
            on year_entry_score.id=ye1.id where year_entry_score.ref_student_id in("+queryKey+")  and year_entry_score.entry_group=1 " + _OptionText;

            // 取得學年學業排名
            string query2 = @"select id,(case xpath_string(class_rating,'/Rating/Item/@分項') when '學業' then xpath_string(class_rating,'/Rating/Item/@排名') end) as 學業班排名,
(case xpath_string(class_rating,'/Rating/Item/@分項') when '學業' then xpath_string(class_rating,'/Rating/Item/@成績人數') end) as 學業班排名母數,
(case xpath_string(dept_rating,'/Rating/Item/@分項') when '學業' then xpath_string(dept_rating,'/Rating/Item/@排名') end) as 學業科排名,
(case xpath_string(dept_rating,'/Rating/Item/@分項') when '學業' then xpath_string(dept_rating,'/Rating/Item/@成績人數') end) as 學業科排名母數,
(case xpath_string(year_rating,'/Rating/Item/@分項') when '學業' then xpath_string(year_rating,'/Rating/Item/@排名') end) as 學業校排名,
(case xpath_string(year_rating,'/Rating/Item/@分項') when '學業' then xpath_string(year_rating,'/Rating/Item/@成績人數') end) as 學業校排名母數 from year_entry_score where year_entry_score.ref_student_id in(" +queryKey + ") and year_entry_score.entry_group=1 " + _OptionText;

            QueryHelper qh1 = new QueryHelper();
            DataTable dt1 = qh1.Select(query1);
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
                    newRow["學年分項學年度"] = dr["學年分項學年度"].ToString();
                    newRow["學年分項年級"] = dr["學年分項年級"].ToString();
                    data1.Add(key, newRow);
                }

                string item = dr["分項"].ToString();
                if(_FieldDict.ContainsKey(item))
                    data1[key][_FieldDict[item]] = dr["成績"];
            }

            List<string> tmpItemList = new List<string>();
            tmpItemList.Add("學業班排名");
            tmpItemList.Add("學業班排名母數");
            tmpItemList.Add("學業科排名");
            tmpItemList.Add("學業科排名母數");
            tmpItemList.Add("學業校排名");
            tmpItemList.Add("學業校排名母數");

            // 處理排名
            foreach (DataRow dr in dt2.Rows)
            {
                string key = dr["id"].ToString();

                if (data1.ContainsKey(key))
                {
                    foreach (string item in tmpItemList)
                        if (_FieldDict.ContainsKey(item))
                            data1[key][_FieldDict[item]] = dr[item];   
                }
            }

            foreach (string key in data1.Keys)
                dt.Rows.Add(data1[key]);


            return dt;
        }
    }
}
