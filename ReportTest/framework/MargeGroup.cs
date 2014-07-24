using System;
using System.Collections.Generic;
using System.Text;

namespace ReportTest.framework
{
    public interface MargeGroup
    {
        List<string> Fields { get; }
        List<string> GroupKeys { get; }
        System.Data.DataTable BuildMargeData(IEnumerable<string> keys);
    }
}
