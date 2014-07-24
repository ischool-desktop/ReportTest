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
    /// 缺曠統計
    /// </summary>
    [MargeClass(Name = "缺曠統計")]
    public class AttendanceSummary : MargeGroup
    {

        /// <summary>
        /// 傳入條件文字
        /// </summary>
        private string _OptionText = "";

        [MargeField(FieldName = "學年度", FieldType = "Int")]
        public int? SchoolYear;

        [MargeField(FieldName = "學期", FieldType = "Int")]
        public int? Semester;

        [MargeField(FieldName = "開始日期", FieldType = "DateTime")]
        public DateTime? beginDate;

        [MargeField(FieldName = "結束日期", FieldType = "DateTime")]
        public DateTime? endDate;

        [MargeField(FieldName = "節次對照")]
        public string usePeriodType="";

        List<string> _FieldList;
        public AttendanceSummary() 
        {
            _FieldList = new List<string>();
         
        }

        private void LoadFieldList()
        {
                        
            // 取得系統內節次與假別對照
            string query1 = @"select PType from xpath_table('name','content','list','/Periods/Period/@Type','name=''節次對照表''') 
as tmp(name character varying(10),PType character varying(20)) Group by PType order by PType";
            QueryHelper qh1 = new QueryHelper();
            DataTable dt1 = qh1.Select(query1);

            string query2 = @"select AName from xpath_table('name','content','list','/AbsenceList/Absence/@Name','name=''假別對照表''')
as tmp(name character varying(10),AName character varying(20)) order by AName";
            QueryHelper qh2 = new QueryHelper();
            DataTable dt2 = qh2.Select(query2);


            // 使用節次對照
            if (!string.IsNullOrEmpty(usePeriodType) && usePeriodType.Trim() == "是")
            {
                foreach (DataRow dr1 in dt1.Rows)
                {
                    foreach (DataRow dr2 in dt2.Rows)
                    {
                        string key = dr1[0].ToString() + "_" + dr2[0].ToString();
                        _FieldList.Add(key);
                    }
                }
            }
            else
            {
                foreach (DataRow dr2 in dt2.Rows)
                {
                    string key =dr2[0].ToString();
                    _FieldList.Add(key);
                }
            }
            
        }

        public List<string> Fields
        {
            get { return _FieldList; }
        }

        public List<string> GroupKeys
        {
            get { return new List<string>(); }
        }

        public System.Data.DataTable BuildMargeData(IEnumerable<string> keys)
        {

            LoadFieldList();

            _OptionText = "";

            // 處理參數學年度(只有學年度)
            if (SchoolYear.HasValue)
            {
                _OptionText = "and attendance.school_year=" + SchoolYear.Value;
            }

            // 處理參數學年度、學期
            if (SchoolYear.HasValue && Semester.HasValue)
            {
                _OptionText = "and attendance.school_year=" + SchoolYear.Value + " and attendance.semester=" + Semester.Value;
            }

            // 處理參數開始日期、結束日期
            if (beginDate.HasValue && endDate.HasValue)
            {
                _OptionText = "and attendance.occur_date>='" + string.Format("{0:yyyy-MM-dd}", beginDate.Value) + "' and  attendance.occur_date<'" + string.Format("{0:yyyy-MM-dd}", endDate.Value.AddDays(1)) + "'";
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

            foreach (string colName in columnsList)
                dt.Columns.Add(colName);

            string queryKey=string.Join(",",keyList.ToArray());

            string query1="";
            if (!string.IsNullOrEmpty(usePeriodType) && usePeriodType.Trim() == "是")
            {
                // 取得缺曠資料
                query1 = @"select attendance.ref_student_id as sid,p3.PAType as Type,count(attendance.id) as Count from 
attendance inner join (select p2.id,p1.ptype||'_'||p2.AbsenceType1 as PAType from xpath_table('list_id','content','list',
'/Periods/Period/@Type|/Periods/Period/@Name','name=''節次對照表''') as p1(id int ,ptype character varying(10),pname character varying(20)) 
inner join (select id,AbsenceType1,AbsenceType2 from xpath_table('id','detail','attendance','/Attendance/Period/@AbsenceType|/Attendance/Period',
'ref_student_id in(" + queryKey + @")') as sa(id int,AbsenceType1 character varying(20),AbsenceType2 character varying(20))) as p2 on p1.pname=p2.AbsenceType2) as p3 on 
attendance.id=p3.id and attendance.ref_student_id in(" + queryKey + @") " + _OptionText + " group by attendance.ref_student_id,p3.PAType order by attendance.ref_student_id";
            }
            else
            {
                query1 = @"select attendance.ref_student_id as sid,p3.PAType as Type,count(attendance.id) as Count from 
attendance inner join (select p2.id,p2.AbsenceType1 as PAType from xpath_table('list_id','content','list',
'/Periods/Period/@Type|/Periods/Period/@Name','name=''節次對照表''') as p1(id int ,ptype character varying(10),pname character varying(20)) 
inner join (select id,AbsenceType1,AbsenceType2 from xpath_table('id','detail','attendance','/Attendance/Period/@AbsenceType|/Attendance/Period',
'ref_student_id in(" + queryKey + @")') as sa(id int,AbsenceType1 character varying(20),AbsenceType2 character varying(20))) as p2 on p1.pname=p2.AbsenceType2) as p3 on 
attendance.id=p3.id and attendance.ref_student_id in(" + queryKey + @") " + _OptionText + " group by attendance.ref_student_id,p3.PAType order by attendance.ref_student_id";
            
            }


            List<string> tmpkeys = new List<string>();
            QueryHelper qhq1 = new QueryHelper();
            DataTable dtq1 = qhq1.Select(query1);
            DataRow newRow = null;
            Dictionary<string, DataRow> tmpDict = new Dictionary<string, DataRow>();

            // 整理資料
            foreach (DataRow dr in dtq1.Rows)
            {
                string sid = dr["sid"].ToString();
                

                // 學生編號
                if (!tmpDict.ContainsKey(sid))
                {
                    DataRow dr1 = dt.NewRow();
                    dr1["ID"] = sid;
                    tmpDict.Add(sid,dr1);

                }

                string key = dr["type"].ToString();
                if (_FieldList.Contains(key))
                    tmpDict[sid][key] = dr["count"];
            }


            // 回傳 StudentID
            foreach (string sid in tmpDict.Keys)
            {
                DataRow dr = dt.NewRow();
                dt.Rows.Add(tmpDict[sid]);
            }

            return dt;
        }
    }
}
