using FISCA.Data;
using ReportTest.framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Aspose.Words;
using Aspose.Words.Reporting;

namespace ReportTest
{
    class Program
    {

        static void Main(string[] args)
        {


            #region 範例
            //var mc = new MargeCenter();
            //mc.JoinGroup(new c1());
            //mc.JoinGroup(new c2());
            //mc.BuildDataTable(new string[] { "1", "2", "3" });

            //foreach (System.Data.DataColumn item in mc.DataTable.Columns)
            //{
            //    //Console.Write("{0,-4}", item.ColumnName);
            //}
            //foreach (System.Data.DataRow row in mc.DataTable.Rows)
            //{
            //    Console.Write("\n");
            //    foreach (System.Data.DataColumn item in mc.DataTable.Columns)
            //    {
            //        Console.Write("{0,-4}", "" + row[item]);
            //    }
            //    //
            //}

            //Console.Read();

            #endregion
        }
        //}
        //public class c1 : MargeGroup
        //{
        //    public List<string> Fields
        //    {
        //        get { return new List<string>(new string[] { "學年", "學期", "科目", "成績" }); }
        //    }

        //    public List<string> GroupKeys
        //    {
        //        get { return new List<string>(new string[] { "學年", "學期", "科目" }); }
        //    }

        //    public System.Data.DataTable BuildMargeData(IEnumerable<string> keys)
        //    {
        //        System.Data.DataTable dt = new System.Data.DataTable();
        //        dt.Columns.Add("ID");
        //        dt.Columns.Add("學年");
        //        dt.Columns.Add("學期");
        //        dt.Columns.Add("科目");
        //        dt.Columns.Add("成績");
        //        var rnd = new Random(DateTime.Now.Millisecond);
        //        foreach (var item in keys)
        //        {
        //            dt.Rows.Add("" + item
        //                , "100"
        //                , "1"
        //                , "國文"
        //                , rnd.Next(60, 100)
        //                );
        //            dt.Rows.Add("" + item
        //                , "100"
        //                , "1"
        //                , "英文"
        //                , rnd.Next(60, 100)
        //                );
        //            dt.Rows.Add("" + item
        //                , "100"
        //                , "1"
        //                , "數學"
        //                , rnd.Next(60, 100)
        //                );
        //        }
        //        return dt;
        //    }
        //}
        //public class c2 : MargeGroup
        //{
        //    public List<string> Fields
        //    {
        //        get { return new List<string>(new string[] { "學年", "學期", "科目", "努力" }); }
        //    }

        //    public List<string> GroupKeys
        //    {
        //        get { return new List<string>(new string[] { "學年", "學期", "科目" }); }
        //    }

        //    public System.Data.DataTable BuildMargeData(IEnumerable<string> keys)
        //    {
        //        System.Data.DataTable dt = new System.Data.DataTable();
        //        dt.Columns.Add("ID");
        //        dt.Columns.Add("學年");
        //        dt.Columns.Add("學期");
        //        dt.Columns.Add("科目");
        //        dt.Columns.Add("努力");
        //        var rnd = new Random(DateTime.Now.Millisecond+100);
        //        foreach (var item in keys)
        //        {
        //            dt.Rows.Add("" + item
        //                , "100"
        //                , "1"
        //                , "國文"
        //                , rnd.Next(1, 5)
        //                );
        //            dt.Rows.Add("" + item
        //                , "100"
        //                , "1"
        //                , "英文"
        //                , rnd.Next(1, 5)
        //                );
        //            dt.Rows.Add("" + item
        //                , "100"
        //                , "1"
        //                , "數學"
        //                , rnd.Next(1, 5)
        //                );
        //        }
        //        return dt;
        //    }


        //}
    }
}
