using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReportTest.framework;
using FISCA.Data;
using System.Data;

namespace ReportTest.DAO
{
    [MargeClass(Name="學生電話")]
    public class StudentPhone:MargeGroup
    {
        public List<string> Fields
        {
            get
            {
                return new List<string>(new string[]{"學生戶籍電話","學生聯絡電話","學生行動電話","學生其它電話1","學生其它電話2","學生其它電話3"});
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

            // 取得戶籍、聯絡、手機
            string query1 = @"select id,permanent_phone as 學生戶籍電話,contact_phone as 學生聯絡電話,sms_phone as 學生行動電話 from student where id in ("+queryKey+")";
            QueryHelper qh1 = new QueryHelper();
            DataTable dt1 = qh1.Select(query1);            

            // 其它電話
            Dictionary<string, List<string>> otherPhone = new Dictionary<string, List<string>>();
            string query2 = @"select id,PhoneNumber from xpath_table('id','other_phones','student','/PhoneList/PhoneNumber','id in ("+queryKey+")') as ss(id int,PhoneNumber character varying(50))";
            QueryHelper qh2 = new QueryHelper ();
            DataTable dt2 = qh2.Select(query2);
            foreach (DataRow dr in dt2.Rows)
            {
                string id = dr["id"].ToString();
                if (!otherPhone.ContainsKey(id))
                    otherPhone.Add(id, new List<string>());

                otherPhone[id].Add(dr["phonenumber"].ToString());
            }

            // 填入電話
            foreach (DataRow dr in dt1.Rows)
            {
                string ID = dr["id"].ToString();

                DataRow newRow = dt.NewRow();
                newRow["ID"] = ID;
                newRow["學生戶籍電話"]=dr["學生戶籍電話"];
                newRow["學生聯絡電話"]=dr["學生聯絡電話"];
                newRow["學生行動電話"]=dr["學生行動電話"];

                if (otherPhone.ContainsKey(ID))
                    for (int i = 1; i <= otherPhone[ID].Count; i++)
                        newRow["學生其它電話" + i] = otherPhone[ID][i - 1];
                
                dt.Rows.Add(newRow);
            }

            return dt;
        }
    }
}
