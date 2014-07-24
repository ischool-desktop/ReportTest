using ReportTest.framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FISCA.Data;
using System.Data;

namespace ReportTest.DAO
{
    /// <summary>
    /// 高中畢業及離校資訊
    /// </summary>
    [MargeClass(Name = "高中畢業及離校資訊")]
    public class DiplomaLeaveInfoSH : MargeGroup
    {

        public List<string> Fields
        {
            get
            {
                return new List<string>(new string[]{"離校學年度","離校類別","離校科別","離校班級","畢業證書字號"});
            }
        }

        public List<string> GroupKeys
        {
            get { return new List<string>(); }
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

            string queryKey = string.Join(",", keyList.ToArray());
            string query1 = @"select id,xpath_string(student.leave_info,'/LeaveInfo/@SchoolYear') as 離校學年度,
xpath_string(student.leave_info,'/LeaveInfo/@Reason') as 離校類別,xpath_string(student.leave_info,'/LeaveInfo/@Department') as 離校科別,
xpath_string(student.leave_info,'/LeaveInfo/@ClassName') as 離校班級,xpath_string('<root>'||student.diploma_number||'</root>','/root/DiplomaNumber') 
as 畢業證書字號 from student where id in("+queryKey+")";

            QueryHelper qh1 = new QueryHelper();
            DataTable dt1 = qh1.Select(query1);

            foreach(DataRow dr in dt1.Rows)
            {
               dt.Rows.Add(
                      dr["id"]
                    ,dr["離校學年度"]
                    ,dr["離校類別"]
                    ,dr["離校科別"]
                    ,dr["離校班級"]
                    ,dr["畢業證書字號"]                   
                   );
            }
            return dt;
        }
    }
}
