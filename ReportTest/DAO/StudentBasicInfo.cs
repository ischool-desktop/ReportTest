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
    /// 學生基本資料
    /// </summary>
    [MargeClass(Name = "學生基本資料")]
    public class StudentBasicInfo : MargeGroup
    {
        
        public List<string> Fields
        {
            get { return new List<string>(new string[] { "學號", "年級", "班級", "座號", "姓名", "性別", "生日","學生代碼","家長代碼","出生地" }); }
        }

        public List<string> GroupKeys
        {
            get { return new List<string>(); }
        }

        public System.Data.DataTable BuildMargeData(IEnumerable<string> keys)
        {
            DataTable dt = new DataTable();
                       

            List<string> keyList = new List<string>();
            foreach (string key in keys)
                keyList.Add(key);

            // 當沒有資料
            if (keyList.Count == 0)
                return dt;
            // 依年級、班級、座號排序
            string query = "select student.id,student_number,class.grade_year,class_name,seat_no,student.name,(case when cast(student.gender as character varying(2))='1' then '男' when cast(student.gender as character varying(2))='0' then '女' else '' end) as 性別,student.birthdate,student_code,parent_code,birth_place from student inner join class on student.ref_class_id=class.id where student.id in(" + string.Join(",", keyList.ToArray()) + ") and class.grade_year is not null order by class.grade_year,class_name,seat_no;";
            QueryHelper qh = new QueryHelper();
            DataTable qdt = qh.Select(query);
            dt.Columns.Add("ID");
            foreach(string Field in Fields)
                dt.Columns.Add(Field);            

            foreach (DataRow dr in qdt.Rows)
            {               
                // 填值
                 dt.Rows.Add(""+dr["id"]                         
                         , dr["student_number"]
                         , dr["grade_year"]
                         , dr["class_name"]
                         , dr["seat_no"]
                         , dr["name"]
                         , dr["性別"]
                         , dr["birthdate"]
                         , dr["student_code"]
                         , dr["parent_code"]
                         , dr["birth_place"]
                         );
            }
            return dt;
        }
    }
}
