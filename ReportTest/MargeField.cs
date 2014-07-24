using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportTest
{
    [System.AttributeUsage(AttributeTargets.Field)]
    public class MargeField:System.Attribute
    {
        /// <summary>
        /// 名稱
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 型態
        /// </summary>
        public string FieldType { get; set; }

    }
}
