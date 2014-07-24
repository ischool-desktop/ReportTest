using ReportTest.framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportTest.DAO
{

    /// <summary>
    /// 新竹國中課程成績
    /// </summary>
    public class CourseScoreHC : MargeGroup
    {
        public List<string> Fields
        {
            get { throw new NotImplementedException(); }
        }

        public List<string> GroupKeys
        {
            get { throw new NotImplementedException(); }
        }

        public System.Data.DataTable BuildMargeData(IEnumerable<string> keys)
        {
            throw new NotImplementedException();
        }
    }
}
