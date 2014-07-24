using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportTest
{
    [System.AttributeUsage(AttributeTargets.Class)]
    public class MargeClass:System.Attribute
    {
        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }
    }
}
