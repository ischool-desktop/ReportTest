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
    /// 父母監護人資訊
    /// </summary>
    [MargeClass(Name = "父母監護人資訊")]
    public class ParentInfo : MargeGroup
    {
        public List<string> Fields
        {
            get { return new List<string>(new string[] { "監護人姓名", "父親姓名", "母親姓名" }); }
        }

        public List<string> GroupKeys
        {
            get { return new List<string>(); }
        }

        public DataTable BuildMargeData(IEnumerable<string> keys)
        {
            DataTable dt = new DataTable();


            List<string> keyList = new List<string>();
            foreach (string key in keys)
                keyList.Add(key);

            // 當沒有資料
            if (keyList.Count == 0)
                return dt;

            dt.Columns.Add("ID");
            foreach (string key in Fields)
                dt.Columns.Add(key);
            string query1 = @"select id,custodian_name as 監護人姓名,father_name as 父親姓名,mother_name as 母親姓名 from student where id in("+string.Join(",",keyList.ToArray())+")";
            QueryHelper qh1 = new QueryHelper();
            DataTable dt1 = qh1.Select(query1);

            foreach (DataRow dr in dt1.Rows)
            {
                dt.Rows.Add(
                    dr["id"]
                    ,dr["監護人姓名"]
                    ,dr["父親姓名"]
                    ,dr["母親姓名"]
                    );
            }
            return dt;
        }
    }
}
