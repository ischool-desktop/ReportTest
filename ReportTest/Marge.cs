using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReportTest.framework;
using ReportTest.DAO;

namespace ReportTest
{
    public class Marge
    {
        /// <summary>
        /// 設定檔
        /// </summary>
        Config _Config;

        /// <summary>
        /// 學生系統編號
        /// </summary>
        List<string> _StudentIDList;

        /// <summary>
        /// MargeCenter
        /// </summary>
        MargeCenter _mc;

        /// <summary>
        /// MargeGroup
        /// </summary>
        Dictionary<string, MargeGroup> _MargeGroupDict;
        Dictionary<string, Type> _MargeGroupTypeDict;

        /// <summary>
        /// 所有合併 Fields
        /// </summary>
        List<string> _FieldList;

        public Marge(Config config,List<string> StudentIDList)
        {
            _mc = new MargeCenter();
            _FieldList = new List<string>();
            _MargeGroupTypeDict = new Dictionary<string, Type>();
            _MargeGroupDict = new Dictionary<string, MargeGroup>();
            _Config = config;            
            _StudentIDList = StudentIDList;
            // 載入類別
            LoadType();
        }

        /// <summary>
        /// 整理與合併資料
        /// </summary>
        public void MargeData()
        {
            List<ConfigItem> ciList = _Config.GetConfigItemList();
            foreach (ConfigItem ci in ciList)
            {
                if(ci.Name !=null)
                    AddMargeGroup(ci.Name);

                // 處理參數設定
                if (_MargeGroupDict.ContainsKey(ci.Name))
                { 
                    // 取得所標欄位
                    foreach (System.Reflection.FieldInfo fi in _MargeGroupDict[ci.Name].GetType().GetFields())
                    {
                        foreach (Attribute attr in fi.GetCustomAttributes(false))
                        {
                            if (attr is MargeField)
                            {
                                MargeField mf = (MargeField)attr;

                                Dictionary<string, string> dataValue = ci.GetFilterValue();
                                foreach (string key in dataValue.Keys)
                                {
                                    if (mf.FieldName == key)
                                    {
                                        int ii;
                                        DateTime dt;

                                        if (string.IsNullOrWhiteSpace(mf.FieldType))
                                        {
                                            fi.SetValue(_MargeGroupDict[ci.Name], dataValue[key]);
                                        }
                                        else
                                        {

                                            // 處理整數
                                            if (mf.FieldType.ToLower() == "int")
                                            {
                                                if (int.TryParse(dataValue[key], out ii))
                                                {
                                                    fi.SetValue(_MargeGroupDict[ci.Name], ii);
                                                }
                                            }
                                            else if (mf.FieldType.ToLower() == "datetime")
                                            {
                                                // 日期
                                                if (DateTime.TryParse(dataValue[key], out dt))
                                                {
                                                    fi.SetValue(_MargeGroupDict[ci.Name], dt);
                                                }
                                            }
                                            else
                                                fi.SetValue(_MargeGroupDict[ci.Name], dataValue[key]);
                                        }
                                    }                                
                                }
                            }
                        }
                    }
                
                }
            }

            
            foreach (MargeGroup mg in _MargeGroupDict.Values)
            {
                _mc.JoinGroup(mg);
            }            
            
            _mc.BuildDataTable(_StudentIDList);

            // 取得 Fields
            _FieldList.Clear();
            foreach (MargeGroup mg in _MargeGroupDict.Values)
            {
                foreach (string key in mg.Fields)
                    if (!_FieldList.Contains(key))
                        _FieldList.Add(key);
            }
        }

        /// <summary>
        /// 取得所合併的 Field 名稱
        /// </summary>
        /// <returns></returns>
        public List<string> GetMargeFields()
        {
            return _FieldList;
        }

        /// <summary>
        /// 取得合併後Data Table
        /// </summary>
        /// <returns></returns>
        public System.Data.DataTable GetDataTable()
        {
            return _mc.DataTable;
        }

        /// <summary>
        /// 加入MargeGroup
        /// </summary>
        /// <param name="Name"></param>
        private void AddMargeGroup(string Name)
        { 
            // 不存在加入
            if (!_MargeGroupDict.ContainsKey(Name))
            {
                if (_MargeGroupTypeDict.ContainsKey(Name))
                {
                    MargeGroup mg = (MargeGroup)Activator.CreateInstance(_MargeGroupTypeDict[Name]);
                    _MargeGroupDict.Add(Name, mg);
                }
            }
        }

        private void LoadType()
        {
            AddType(typeof(StudentBasicInfo));
            AddType(typeof(AddressInfo));
            AddType(typeof(AttendanceDeatil));
            AddType(typeof(AttendanceSummary));
            AddType(typeof(DiplomaLeaveInfoJH));
            AddType(typeof(DiplomaLeaveInfoSH));
            AddType(typeof(DisciplineDetail));
            AddType(typeof(DisciplineMDSummary));
            AddType(typeof(DisciplineSummary));
            AddType(typeof(ParentInfo));
            AddType(typeof(SchoolInfo));
            AddType(typeof(SemesterCourseScoreJH));
            AddType(typeof(SemesterDomainScore));
            AddType(typeof(SemesterEntryScore));
            AddType(typeof(SemesterSubjectScoreJH));
            AddType(typeof(SemesterSubjectScoreSH));
            AddType(typeof(YearEntryScore));
            AddType(typeof(YearSubjectScore));
            AddType(typeof(TextScoreSB));
            AddType(typeof(ServiceLearning));
            AddType(typeof(SemesterHistory));
            AddType(typeof(StudentPhone));
            AddType(typeof(UpdateRecord));
        }

        /// <summary>
        /// 新增類別
        /// </summary>
        /// <param name="type"></param>
        public void AddType (Type type)
        {
            string name = "";
            foreach (Attribute attr in type.GetCustomAttributes(false))
            {
                if (attr is MargeClass)
                {
                    MargeClass mm = (MargeClass)attr;
                    name = mm.Name;
                    break;
                }            
            }

            if (!string.IsNullOrEmpty(name))
            {
                if (!_MargeGroupTypeDict.ContainsKey(name))
                    _MargeGroupTypeDict.Add(name, type);
            }
        }
    }

}
