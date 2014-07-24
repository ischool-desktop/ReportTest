using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReportTest.framework;
using FISCA.Data;
using System.Data;

namespace ReportTest.DAO
{
    [MargeClass(Name="異動資料")]
   public class UpdateRecord:MargeGroup
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


        public List<string> Fields
        {
            get { return new List<string>(new string[] { "異動代碼","異動狀況", "異動原因", "異動學年度", "異動學期", "異動學校", "異動日期", "異動核准日期", "異動核准文號", "異動備註" }); }
        }

        public List<string> GroupKeys
        {
            get { return new List<string>(new string[] { "異動代碼", "異動狀況", "異動原因", "異動學年度", "異動學期", "異動學校", "異動日期", "異動核准日期", "異動核准文號", "異動備註" }); }
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

            _OptionText = "";

            // 處理參數學年度(只有學年度)
            if (SchoolYear.HasValue)
            {
                _OptionText = "and update_record.school_year=" + SchoolYear.Value;
            }

            // 處理參數學年度、學期
            if (SchoolYear.HasValue && Semester.HasValue)
            {
                _OptionText = "and update_record.school_year=" + SchoolYear.Value + " and attendance.semester=" + Semester.Value;
            }

            // 處理參數開始日期、結束日期
            if (beginDate.HasValue && endDate.HasValue)
            {
                _OptionText = "and update_record.update_date>='" + string.Format("{0:yyyy-MM-dd}", beginDate.Value) + "' and  update_record.update_date<'" + string.Format("{0:yyyy-MM-dd}", endDate.Value.AddDays(1)) + "'";
            }


            string query1 = @"select ref_student_id as sid,update_code as 異動代碼,update_desc as 異動原因,school_year as 異動學年度,
semester as 異動學期,xpath_string(context_info,'/ContextInfo/ImportExportSchool') as 異動學校,update_date as 異動日期,ad_date as 異動核准日期,
ad_number as 異動核准文號,comment as 異動備註 from update_record where ref_student_id in(" +string.Join(",",keyList.ToArray())+") "+_OptionText+" order by ref_student_id,update_date,id";

            QueryHelper qh1 = new QueryHelper();
            DataTable dt1 = qh1.Select(query1);

            foreach (DataRow dr in dt1.Rows)
            {
                dt.Rows.Add(
                    dr["sid"]
                    ,dr["異動代碼"]
                    ,dr["異動原因"]
                    ,dr["異動學年度"]
                    ,dr["異動學期"]
                    ,dr["異動學校"]
                    ,dr["異動日期"]
                    ,dr["異動核准日期"]
                    ,dr["異動核准文號"]
                    ,dr["異動備註"]
                    );
            }

            return dt;
        }
    }
}
