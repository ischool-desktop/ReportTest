using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportTest
{
    /// <summary>
    /// 設定項目    /// 
    /// </summary>
    public class ConfigItem
    {
        public ConfigItem(string strValue)
        {
            string[] value = System.Text.RegularExpressions.Regex.Split(strValue, ";");
            if (value.Count() >0)
                Name = value[0];
            if (value.Count() > 1)
                Filter = value[1];
            if (value.Count() > 2)
                Field = value[2];
        }
        
        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 篩選條件
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// 欄位
        /// </summary>
        public string Field { get; set; }

     

        /// <summary>
        /// 取得特定欄位
        /// </summary>
        /// <returns></returns>
        public List<string> GetFieldList()
        {
            List<string> retValue = new List<string>();
            List<string> fList= Field.Split(',').ToList();
            if (fList != null)
            {
                foreach (string str in fList)
                    retValue.Add(str.Trim());
            }
            return retValue;
        }

        /// <summary>
        /// 取得條件內容,ex. 學年度=98
        /// </summary>
        /// <returns></returns>
        public Dictionary<string,string> GetFilterValue()
        {
            Dictionary<string, string> retVal = new Dictionary<string, string>();
            List<string> valList = null;

            if (Filter != null)
                valList= Filter.Split(',').ToList();

            if(valList !=null)
            foreach (string str in valList)
            {
                string key = "";
                string value = "";

                // 用 = 分隔
                string[] strArr = str.Split('=');

                // 只放有解析成功
                if (strArr.Count() == 2)
                {
                    key = strArr[0].Trim();
                    value = strArr[1].Trim();

                    if (!string.IsNullOrWhiteSpace(key))
                        if (!retVal.ContainsKey(key))
                            retVal.Add(key, value);
                }            
            }
            return retVal;
        }
    }
}
