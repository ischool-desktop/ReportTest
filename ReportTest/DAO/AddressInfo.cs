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
    /// 地址資訊
    /// </summary>
    [MargeClass(Name = "地址資訊")]
    public class AddressInfo : MargeGroup
    {
        public List<string> Fields
        {
            get { return new List<string>(new string[] { "戶籍郵遞區號", "戶籍地址", "聯絡郵遞區號", "聯絡地址", "其它郵遞區號", "其它地址" }); }
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

            string query1 = @"select id,xpath_string(student.permanent_address,'/AddressList/Address/ZipCode') as 戶籍郵遞區號,
(xpath_string(student.permanent_address,'/AddressList/Address/County')||xpath_string(student.permanent_address,
'/AddressList/Address/Town')||xpath_string(student.permanent_address,'/AddressList/Address/District')||
xpath_string(student.permanent_address,'/AddressList/Address/Area')||xpath_string(student.permanent_address,
'/AddressList/Address/DetailAddress')) as 戶籍地址,xpath_string(student.mailing_address,'/AddressList/Address/ZipCode') as 聯絡郵遞區號,
(xpath_string(student.mailing_address,'/AddressList/Address/County')||xpath_string(student.mailing_address,'/AddressList/Address/Town')
||xpath_string(student.mailing_address,'/AddressList/Address/District')||xpath_string(student.mailing_address,'/AddressList/Address/Area')
||xpath_string(student.mailing_address,'/AddressList/Address/DetailAddress')) as 聯絡地址,xpath_string(student.other_addresses,
'/AddressList/Address/ZipCode') as 其它郵遞區號,(xpath_string(student.other_addresses,'/AddressList/Address/County')||
xpath_string(student.other_addresses,'/AddressList/Address/Town')||xpath_string(student.other_addresses,'/AddressList/Address/District')||
xpath_string(student.other_addresses,'/AddressList/Address/Area')||xpath_string(student.other_addresses,'/AddressList/Address/DetailAddress'))
as 其它地址 from student where id in("+string.Join(",",keyList.ToArray())+")";

            QueryHelper qh1 = new QueryHelper();
            DataTable dt1 = qh1.Select(query1);

            foreach (DataRow dr in dt1.Rows)
            {
                dt.Rows.Add(
                    dr["id"]
                    , dr["戶籍郵遞區號"]
                    , dr["戶籍地址"]
                    , dr["聯絡郵遞區號"]
                    , dr["聯絡地址"]
                    , dr["其它郵遞區號"]
                    , dr["其它地址"]
                    );
            }            
            return dt;
        }
    }
}
