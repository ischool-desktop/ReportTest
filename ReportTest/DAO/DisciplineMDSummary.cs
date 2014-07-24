using ReportTest.framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using FISCA.Data;

namespace ReportTest.DAO
{
    /// <summary>
    /// 獎懲統計(依功過相抵)
    /// </summary>
    [MargeClass(Name = "獎懲統計_功過相抵")]
    public class DisciplineMDSummary: MargeGroup
    {
        /// <summary>
        /// 傳入條件文字
        /// </summary>
        private string _OptionText = "";

        [MargeField(FieldName = "學年度", FieldType = "Int")]
        public int? SchoolYear;

        [MargeField(FieldName = "學期", FieldType = "Int")]
        public int? Semester;

        public List<string> Fields
        {
            get { return new List<string>(new string[] { "功過相抵大功支數", "功過相抵小功支數", "功過相抵嘉獎支數", "功過相抵大過支數", "功過相抵小過支數", "功過相抵警告支數" }); }
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

            _OptionText = "";

            if(SchoolYear.HasValue)
                _OptionText = " and discipline.school_year=" + SchoolYear.Value;

            if (SchoolYear.HasValue && Semester.HasValue)
                _OptionText = " and discipline.school_year=" + SchoolYear.Value +" and discipline.semester=" + Semester.Value;

            // 加入回傳欄位            
            dt.Columns.Add("ID");
            foreach (string name in Fields)
                dt.Columns.Add(name);


            // 取得功過換算表
            string query1 = @"select 
xpath_string(list.content,'/Reduce/Merit/AB') as mab,
xpath_string(list.content,'/Reduce/Merit/BC') as mbc,
xpath_string(list.content,'/Reduce/Demerit/AB') as dab,
xpath_string(list.content,'/Reduce/Demerit/BC') as dbc
from list where name='獎懲單位換算表'";

            int mab = 0,mbc=0,dab=0,dbc=0;

            QueryHelper qh1 = new QueryHelper();
            DataTable dt1 = qh1.Select(query1);
            if (dt1.Rows.Count > 0)
            {
                int.TryParse(dt1.Rows[0]["mab"].ToString(), out mab);
                int.TryParse(dt1.Rows[0]["mbc"].ToString(), out mbc);
                int.TryParse(dt1.Rows[0]["dab"].ToString(), out dab);
                int.TryParse(dt1.Rows[0]["dbc"].ToString(), out dbc);
            }

            string query2 = @"select g1.ref_student_id as sid,CASE WHEN sum(大功) is null THEN 0 ELSE sum(大功) END as 大功支數,CASE WHEN sum(小功) is null THEN 0 ELSE 
sum(小功) END as 小功支數,CASE WHEN sum(嘉獎) is null THEN 0 ELSE sum(嘉獎) END as 嘉獎支數,CASE WHEN sum(大過) is null THEN 0 ELSE sum(大過) END 
as 大過支數,CASE WHEN sum(小過) is null THEN 0 ELSE sum(小過) END as 小過支數,CASE WHEN sum(警告) is null THEN 0 ELSE sum(警告) END as 警告支數 from (
select discipline.ref_student_id,CAST(regexp_replace( xpath_string(discipline.detail,'/Discipline/Merit/@A'), '^$', '0')
as integer) as 大功,CAST(regexp_replace( xpath_string(discipline.detail,'/Discipline/Merit/@B'), '^$', '0') as integer) as 小功,
CAST(regexp_replace( xpath_string(discipline.detail,'/Discipline/Merit/@C'), '^$', '0') as integer) as 嘉獎,
CAST(regexp_replace( xpath_string(discipline.detail,'/Discipline/Demerit/@A'), '^$', '0') as integer) as 大過,
CAST(regexp_replace( xpath_string(discipline.detail,'/Discipline/Demerit/@B'), '^$', '0') as integer) as 小過,
CAST(regexp_replace( xpath_string(discipline.detail,'/Discipline/Demerit/@C'), '^$', '0') as integer) as 警告 
from discipline where xpath_string(discipline.detail,'/Discipline/Demerit/@Cleared')<>'是' and discipline.ref_student_id in(" + string.Join(",", keyList.ToArray()) + ") " + _OptionText + ") as g1 group by g1.ref_student_id";

            QueryHelper qh2 = new QueryHelper();
            DataTable dt2 = qh2.Select(query2);
            
            // 處理功過相抵

            foreach (DataRow dr in dt2.Rows)
            {
                // 計算最小基數,MA(功過相抵大功支數),MB(功過相抵小功支數),MC(功過相抵嘉獎支數),DA(功過相抵大過支數),DB(功過相抵小過支數),DC(功過相抵警告支數)
                int MSum=0,DSum=0,MDSum=0,MA=0,MB=0,MC=0,DA=0,DB=0,DC=0;
                // 功
                MSum = int.Parse(dr["大功支數"].ToString()) * mab * mbc + int.Parse(dr["小功支數"].ToString()) * mbc + int.Parse(dr["嘉獎支數"].ToString());
                
                // 過
                DSum = int.Parse(dr["大過支數"].ToString()) * dab * dbc + int.Parse(dr["小過支數"].ToString()) * dbc + int.Parse(dr["警告支數"].ToString());

                MDSum = MSum - DSum;

                // 功大於過
                if (MDSum > 0)
                {
                    int cot = MDSum, mabc = mab * mbc;

                    // 大功
                    while ((cot - mabc) >= 0)
                    {
                        cot -= mabc;
                        MA++;
                    }

                    // 小功
                    while ((cot - mbc) >= 0)
                    {
                        cot -= mbc;
                        MB++;
                    }

                    MC = cot;
                }

                // 功小於過
                if (MDSum < 0)
                {
                    int cot = MDSum*-1, dabc = dab * dbc;

                    // 大功
                    while ((cot - dabc) >= 0)
                    {
                        cot -= dabc;
                        DA++;
                    }

                    // 小功
                    while ((cot - dbc) >= 0)
                    {
                        cot -= dbc;
                        DB++;
                    }

                    DC = cot;                
                }

                // 將值加入回傳
                dt.Rows.Add(
                    dr["sid"]
                    ,MA
                    ,MB
                    ,MC
                    ,DA
                    ,DB
                    ,DC
                    );           
            
            }

            return dt;
        }
    }
}
