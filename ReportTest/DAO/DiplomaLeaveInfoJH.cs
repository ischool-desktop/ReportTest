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
    /// 國中畢業資訊
    /// </summary>
    [MargeClass(Name = "國中畢業資訊")]
    public class DiplomaLeaveInfoJH : MargeGroup
    {
        public List<string> Fields
        {
            get { return new List<string>(new string[] { "畢業學年度", "畢業資格", "畢業證書字號", "畢業相關訊息" }); }
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

            string query1 = @"select id,xpath_string(student.leave_info,'/LeaveInfo/@SchoolYear') as 畢業學年度,
xpath_string(student.leave_info,'/LeaveInfo/@Reason') as 畢業資格,xpath_string('<root>'||student.diploma_number||'</root>','/root/DiplomaNumber') 
as 畢業證書字號,xpath_string(student.leave_info,'/LeaveInfo/@Memo') as 畢業相關訊息 from student where id in("+string.Join(",",keyList.ToArray())+")";

            QueryHelper qh1 = new QueryHelper();
            DataTable dt1 = qh1.Select(query1);

            foreach (DataRow dr in dt1.Rows)
            {
                dt.Rows.Add(
                    dr["id"]
                    , dr["畢業學年度"]
                    , dr["畢業資格"]
                    , dr["畢業證書字號"]
                    , dr["畢業相關訊息"]
                    );
            }

            return dt;
        }
    }
}
