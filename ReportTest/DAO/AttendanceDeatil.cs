using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FISCA.Data;
using ReportTest.framework;

namespace ReportTest.DAO
{
    /// <summary>
    /// 缺曠明細
    /// </summary>
    [MargeClass(Name="缺曠明細")]
    public class AttendanceDeatil : MargeGroup
    {
        List<string> _FieldList;

        /// <summary>
        /// 傳入條件文字
        /// </summary>
        private string _OptionText = "";

        [MargeField(FieldName="學年度",FieldType="Int")]
        public int? SchoolYear;

        [MargeField(FieldName = "學期", FieldType = "Int")]
        public int? Semester;

        [MargeField(FieldName = "開始日期",FieldType="DateTime")]
        public DateTime? beginDate;

        [MargeField(FieldName = "結束日期",FieldType="DateTime")]
        public DateTime? endDate;

        [MargeField(FieldName="縮寫")]
        public string AAbsenceValue = "";

        private void LoadFieldList()
        {
            _FieldList = new List<string>();
            _FieldList.Add("缺曠學年度");
            _FieldList.Add("缺曠學期");
            _FieldList.Add("缺曠年級");
            _FieldList.Add("缺曠日期");
            _FieldList.Add("缺曠星期");
            // 取得系統內節次
            string query1 = @"select PType from xpath_table('name','content','list','/Periods/Period/@Name','name=''節次對照表''') 
as tmp(name character varying(10),PType character varying(20)) Group by PType order by PType";
            QueryHelper qh1 = new QueryHelper();
            DataTable dt1 = qh1.Select(query1);

            foreach (DataRow dr in dt1.Rows)
            {              
                _FieldList.Add(dr[0].ToString());
            }
        }

        public List<string> Fields
        {
            get { return _FieldList; }
        }

        public List<string> GroupKeys
        {
            get { return _FieldList; }
        }

        public DataTable BuildMargeData(IEnumerable<string> keys)
        {
            LoadFieldList();

            _OptionText = "";

            // 處理參數學年度(只有學年度)
            if (SchoolYear.HasValue)
            {
                _OptionText = "where p2.school_year=" + SchoolYear.Value;
            }

            // 處理參數學年度、學期
            if (SchoolYear.HasValue && Semester.HasValue)
            {
                _OptionText = "where p2.school_year=" + SchoolYear.Value + " and p2.semester=" + Semester.Value;
            }

            // 處理參數開始日期、結束日期
            if (beginDate.HasValue && endDate.HasValue)
            {
                _OptionText = "where p2.occur_date>='" + string.Format("{0:yyyy-MM-dd}", beginDate.Value) + "' and  p2.occur_date<'" + string.Format("{0:yyyy-MM-dd}", endDate.Value.AddDays(1)) + "'";
            }

            DataTable dt = new DataTable();
            List<string> keyList = new List<string>();
            foreach (string key in keys)
                keyList.Add(key);

            // 當沒有資料
            if (keyList.Count == 0)
                return dt;            

            List<string> columnsList = new List<string>();
            columnsList.Add("ID");
            foreach (string name in _FieldList)
                columnsList.Add(name);

            foreach (string name in columnsList)
                dt.Columns.Add(name);

            string queryKey = string.Join(",", keyList.ToArray());
            // 取得缺曠資料
            string query1 = @"select g1.id as sid,g1.SchoolYear as 缺曠學年度,g1.Semester as 缺曠學期,g1.GradeYear as 缺曠年級,p2.id,to_char(p2.occur_date,'YYYY/MM/DD') as 缺曠日期,(case p2.occur_weekday when '0' then '日' when '1' then '一' when '2' then '二' when '3' then'三' when '4' then '四' when '5' then '五' when '6' then '六' else p2.occur_weekday end) as 缺曠星期,
p2.AbsenceType1 缺曠類別,p2.AbsenceType2 as 節次 from(select id,smh.SchoolYear,smh.semester,smh.GradeYear from 
xpath_table('id','''<root>''||sems_history||''</root>''','student',
'/root/History/@SchoolYear|/root/History/@Semester|/root/History/@GradeYear','id in (" + queryKey + @")') 
AS smh(id int, SchoolYear integer,Semester int,GradeYear int)) as g1 inner join
(select attendance.id,attendance.ref_student_id,attendance.school_year,attendance.semester,occur_date,cast(extract(dow from occur_date::timestamp)as character varying(1)) as occur_weekday,p1.AbsenceType1,
p1.AbsenceType2 from attendance inner join (select id,AbsenceType1,AbsenceType2 from xpath_table('id','detail','attendance',
'/Attendance/Period/@AbsenceType|/Attendance/Period','ref_student_id in(" + queryKey+@")')  as 
            sa(id int,AbsenceType1 character varying(20),AbsenceType2 character varying(20))) as p1 on attendance.id=p1.id) 
as p2 on g1.id=p2.ref_student_id and g1.SchoolYear=p2.school_year and g1.Semester=p2.semester  " + _OptionText + " order by g1.id,p2.occur_date";

            Dictionary<string, Dictionary<string, List<DataRow>>> tmpDict = new Dictionary<string, Dictionary<string, List<DataRow>>>();
            QueryHelper qhq1 = new QueryHelper();
            DataTable dtq1 = qhq1.Select(query1);

            // 使用縮寫
            bool useAAbsenceValue = false;

            if (!string.IsNullOrEmpty(AAbsenceValue) && AAbsenceValue.Trim() == "是")
                useAAbsenceValue = true;


           

            // 整理資料
            foreach (DataRow dr in dtq1.Rows)
            {
                string sid = dr["sid"].ToString();
                string id = dr["id"].ToString();

                // 學生編號
                if (!tmpDict.ContainsKey(sid))
                    tmpDict.Add(sid, new Dictionary<string, List<DataRow>>());

                // 缺曠編號
                if (!tmpDict[sid].ContainsKey(id))
                    tmpDict[sid].Add(id, new List<DataRow>());

                // 資料
                tmpDict[sid][id].Add(dr);                
            }


            if (useAAbsenceValue)
            {
                // 假別對照表,名稱與縮寫對應
                Dictionary<string, string> AbsenceNameDict = new Dictionary<string, string>();
                string query2 = @"select aname,bname from xpath_table('name','content','list','/AbsenceList/Absence/@Name|/AbsenceList/Absence/@Abbreviation','name=''假別對照表''') as tmp(name character varying(10),aname character varying(20),bname character varying(20)) order by aname";
                QueryHelper qh2 = new QueryHelper();
                DataTable dt2 = qh2.Select(query2);
                foreach (DataRow dr in dt2.Rows)
                {
                    string key = dr["aname"].ToString();
                    if (!AbsenceNameDict.ContainsKey(key))
                        AbsenceNameDict.Add(key, dr["bname"].ToString());
                }

                // 處理縮寫值
                foreach (string sid in tmpDict.Keys)
                {
                    foreach (string aid in tmpDict[sid].Keys)
                    {
                        foreach (DataRow ddr in tmpDict[sid][aid])
                        {
                            string key = ddr["缺曠類別"].ToString();
                            if (AbsenceNameDict.ContainsKey(key))
                                ddr["缺曠類別"] = AbsenceNameDict[key];
                        }

                    }
                }
            }
                     
            // 回傳 StudentID
            foreach (string sid in tmpDict.Keys)
            {                
                // 缺曠學年度、缺曠學期、日期
                foreach(string aid in tmpDict[sid].Keys)
                {
                    DataRow dr = dt.NewRow();
                    dr["ID"] = sid;
                    dr["缺曠學年度"] = tmpDict[sid][aid][0]["缺曠學年度"];
                    dr["缺曠學期"] = tmpDict[sid][aid][0]["缺曠學期"];
                    dr["缺曠年級"] = tmpDict[sid][aid][0]["缺曠年級"];
                    dr["缺曠日期"] = tmpDict[sid][aid][0]["缺曠日期"];
                    dr["缺曠星期"] = tmpDict[sid][aid][0]["缺曠星期"];
                    // 缺曠類別+節次
                    
                    foreach (DataRow ddr in tmpDict[sid][aid])
                    {
                        string key = ddr["節次"].ToString();
                        if (_FieldList.Contains(key))
                        {
                            dr[key] = ddr["缺曠類別"];                            
                        }
                    }
                    dt.Rows.Add(dr);
                }            
            } 

            return dt;
        }
    }
}
