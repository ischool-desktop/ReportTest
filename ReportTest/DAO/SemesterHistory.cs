using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReportTest.framework;
using System.Data;
using FISCA.Data;

namespace ReportTest.DAO
{
    [MargeClass(Name="學期歷程")]
    public class SemesterHistory:MargeGroup
    {
        public List<string> Fields
        {
            get { return new  List<string>(new string[] { "學期歷程學年度", "學期歷程學期", "學期歷程年級", "學期歷程科別名稱", "學期歷程班級名稱", "學期歷程座號", "學期歷程班導師", "學期歷程上課天數" }); }
        }

        public List<string> GroupKeys
        {
            get { return new List<string>(new string[] { "學期歷程學年度", "學期歷程學期", "學期歷程年級", "學期歷程科別名稱", "學期歷程班級名稱", "學期歷程座號", "學期歷程班導師", "學期歷程上課天數" }); }
        }

        public DataTable BuildMargeData(IEnumerable<string> keys)
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

            string query1 = @"select id,school_year as 學期歷程學年度,semester as 學期歷程學期,grade_year as 學期歷程年級,
dept_name as 學期歷程科別名稱,class_name as 學期歷程班級名稱,seat_no as 學期歷程座號,teacher as 學期歷程班導師,
school_day_count as 學期歷程上課天數 from xpath_table('id','''<root>''||sems_history||''</root>''','student',
'/root/History/@SchoolYear|/root/History/@Semester|/root/History/@GradeYear|/root/History/@DeptName|/root/History/@ClassName|
/root/History/@SeatNo|/root/History/@Teacher|/root/History/@SchoolDayCount','id in ("+string.Join(",",keyList.ToArray())+@")') as smh(id int, school_year int,
semester int,grade_year int, dept_name character varying(30),class_name character varying(30),seat_no character varying(10),
teacher character varying(50),school_day_count character varying(10)) order by id,school_year,semester";

            QueryHelper qh1 = new QueryHelper();
            DataTable dt1 = qh1.Select(query1);

            foreach (DataRow dr in dt1.Rows)
            {
                dt.Rows.Add(
                    dr["id"]
                    , dr["學期歷程學年度"]
                    , dr["學期歷程學期"]
                    , dr["學期歷程年級"]
                    , dr["學期歷程科別名稱"]
                    , dr["學期歷程班級名稱"]
                    , dr["學期歷程座號"]
                    , dr["學期歷程班導師"]
                    , dr["學期歷程上課天數"]
                    );
            }
            return dt;
        }
    }
}
