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
    /// 學校資訊
    /// </summary>
    [MargeClass(Name = "學校資訊")]
    public class SchoolInfo : MargeGroup
    {
        public List<string> Fields
        {
            get { return new List<string>(new string[] { "學校代碼", "學校中文名稱", "學校英文名稱", "學校中文地址", "學校英文地址", "學校電話", "校長中文名稱", "校長英文名稱" }); }
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
    
            dt.Columns.Add("ID");
            foreach (string colName in Fields)
                dt.Columns.Add(colName);
            
            string query1 = @"select xpath_string(list.content,'/SchoolInformation/Code') as 學校代碼,xpath_string(list.content,'/SchoolInformation/ChineseName') as 學校中文名稱,xpath_string(list.content,'/SchoolInformation/EnglishName') as 學校英文名稱,xpath_string(list.content,'/SchoolInformation/Address') as 學校中文地址,xpath_string(list.content,'/SchoolInformation/EnglishAddress') as 學校英文地址,xpath_string(list.content,'/SchoolInformation/Telephone') as 學校電話,xpath_string(list.content,'/SchoolInformation/ChancellorChineseName') as 校長中文名稱,xpath_string(list.content,'/SchoolInformation/ChancellorEnglishName') as 校長英文名稱 from list where name ='學校資訊'";
            QueryHelper qh1 = new QueryHelper ();
            DataTable dt1 = qh1.Select(query1);
              
            foreach(string key in keys)
            {
                dt.Rows.Add(key
                    , dt1.Rows[0]["學校代碼"]
                    , dt1.Rows[0]["學校中文名稱"]
                    , dt1.Rows[0]["學校英文名稱"]
                    , dt1.Rows[0]["學校中文地址"]
                    , dt1.Rows[0]["學校英文地址"]
                    , dt1.Rows[0]["學校電話"]
                    , dt1.Rows[0]["校長中文名稱"]
                    , dt1.Rows[0]["校長英文名稱"]
                    );
            }

            return dt;
        }
    }
}
