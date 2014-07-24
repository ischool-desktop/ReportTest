using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ReportTest.framework
{
    public class MargeCenter
    {
        #region 註冊靜態MargeGroup
        private interface IMargeGroupBulider
        {
            /// <summary>
            /// 建立新的DetailPane顯示項目實體
            /// </summary>
            /// <returns>DetailPane顯示項目</returns>
            MargeGroup GetMargeGroup();
        }
        /// <summary>
        /// DetailPane顯示項目的產生器
        /// </summary>
        /// <typeparam name="T">DetailPane顯示項目的型別，此型別必需實做預設建構子</typeparam>
        private class MargeGroupBulider<T> : IMargeGroupBulider where T : MargeGroup, new()
        {
            /// <summary>
            /// 建立新的DetailPane顯示項目實體
            /// </summary>
            /// <returns>DetailPane顯示項目</returns>
            public MargeGroup GetMargeGroup()
            {
                var newContent = new T();
                return newContent;
            }
        }

        private static Dictionary<string, IMargeGroupBulider> _MargeGroups = new Dictionary<string, IMargeGroupBulider>();
        public static void RigisterGroup<T>(string groupName) where T : MargeGroup, new()
        {
            if (!_MargeGroups.ContainsKey(groupName))
            {
                _MargeGroups.Add(groupName, new MargeGroupBulider<T>());
            }
            else
            {
                throw new Exception("GroupName 已經註冊。");
            }
        }
        #endregion

        private System.Data.DataTable _DataTable = new System.Data.DataTable();
        private Dictionary<string, MargeGroup> _FieldCreater = new Dictionary<string, MargeGroup>();
        private List<MargeGroup> _Groups = new List<MargeGroup>();
        public System.Data.DataTable DataTable { get { return _DataTable; } }
        public void JoinGroup(string name)
        {
            if (_MargeGroups.ContainsKey(name))
            {
                _Groups.Add(_MargeGroups[name].GetMargeGroup());
            }
            else
            {
                throw new Exception("MargeGroup不存在：" + name);
            }
        }
        public void JoinGroup(MargeGroup group)
        {
            _Groups.Add(group);
        }
        public void BuildDataTable(IEnumerable<string> keys)
        {
            Dictionary<string, DataRow> idRow = new Dictionary<string, DataRow>();
            _DataTable.Clear();
            _DataTable.Columns.Add("ID");
            foreach (var id in keys)
            {
                DataRow row = _DataTable.Rows.Add(new object[] { id });
                idRow.Add(id, row);
            }
            Dictionary<MargeGroup, DataTable> margeGroup = new Dictionary<MargeGroup, DataTable>();
            Dictionary<MargeGroup, Dictionary<string, List<DataRow>>> margeGroupRows = new Dictionary<MargeGroup, Dictionary<string, List<DataRow>>>();
            Dictionary<MargeGroup, Dictionary<string, MargeGroup>> margeGroupFieldCreater = new Dictionary<MargeGroup, Dictionary<string, MargeGroup>>();
            foreach (var group in _Groups)
            {
                var table = group.BuildMargeData(new List<string>(keys));
                if (group.GroupKeys.Count == 0)
                {
                    List<string> joinFields = new List<string>();
                    foreach (var field in group.Fields)
                    {
                        if (field != "ID" && !_FieldCreater.ContainsKey(field))
                        {
                            _DataTable.Columns.Add(field);
                            _FieldCreater.Add(field, group);
                            joinFields.Add(field);
                        }
                    }
                    foreach (DataRow row in table.Rows)
                    {
                        string id = "" + row["ID"];
                        if (idRow.ContainsKey(id))
                        {
                            foreach (var field in joinFields)
                            {
                                if (idRow[id].IsNull(field))
                                {
                                    idRow[id][field] = row[field];
                                }
                            }
                        }
                    }
                }
                else
                {
                    MargeGroup margeTarget = null;
                    foreach (var mg in margeGroup.Keys)
                    {
                        bool match = true;
                        foreach (var key in group.GroupKeys)
                        {
                            if (!mg.GroupKeys.Contains(key))
                            {
                                match = false;
                                break;
                            }
                        }
                        if (match)
                        {
                            margeTarget = mg;
                            break;
                        }
                    }
                    if (margeTarget == null)
                    {
                        margeGroup.Add(group, table);
                        margeGroupFieldCreater.Add(group, new Dictionary<string, MargeGroup>());
                        foreach (var field in group.Fields)
                        {
                            if (field != "ID" && table.Columns.Contains(field))
                            {
                                margeGroupFieldCreater[group].Add(field, group);
                            }
                        }
                        Dictionary<string, List<DataRow>> idRows = new Dictionary<string, List<DataRow>>();
                        foreach (DataRow row in table.Rows)
                        {
                            string id = "" + row["ID"];
                            if (!idRows.ContainsKey(id))
                            {
                                idRows.Add(id, new List<DataRow>());
                            }
                            idRows[id].Add(row);
                        }
                        margeGroupRows.Add(group, idRows);
                    }
                    else
                    {
                        List<string> joinFields = new List<string>();
                        foreach (var field in group.Fields)
                        {
                            if (field != "ID" && !margeGroupFieldCreater[margeTarget].ContainsKey(field))
                            {
                                margeGroup[margeTarget].Columns.Add(field);
                                margeGroupFieldCreater[margeTarget].Add(field, group);
                                joinFields.Add(field);
                            }
                        }
                        foreach (DataRow row in table.Rows)
                        {
                            string id = "" + row["ID"];
                            if (margeGroupRows[margeTarget].ContainsKey(id))
                            {
                                List<DataRow> margeRows = new List<DataRow>();
                                foreach (DataRow mRow in margeGroupRows[margeTarget][id])
                                {
                                    bool match = true;
                                    foreach (var key in group.GroupKeys)
                                    {
                                        if (!mRow[key].Equals(row[key]))
                                        {
                                            match = false;
                                            break;
                                        }
                                    }
                                    if (match)
                                    {
                                        foreach (var field in joinFields)
                                        {
                                            if (mRow.IsNull(field))
                                            {
                                                mRow[field] = row[field];
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            foreach (var mainGroup in margeGroup.Keys)
            {
                var maxRepeat = 0;
                foreach (var list in margeGroupRows[mainGroup].Values)
                {
                    if (list.Count > maxRepeat)
                        maxRepeat = list.Count;
                }

                //List<string> joinFields = new List<string>();
                foreach (var field in margeGroupFieldCreater[mainGroup].Keys)
                {
                    for (int i = 1; i <= maxRepeat; i++)
                    {
                        var rfield = field + "_" + i;
                        if (!_FieldCreater.ContainsKey(rfield))
                        {
                            _DataTable.Columns.Add(rfield);
                            _FieldCreater.Add(rfield, margeGroupFieldCreater[mainGroup][field]);
                            //joinFields.Add(rfield);
                        }
                    }
                }

                foreach (var id in margeGroupRows[mainGroup].Keys)
                {
                    if (idRow.ContainsKey(id))
                    {
                        var targetRow = idRow[id];
                        foreach (var field in margeGroupFieldCreater[mainGroup].Keys)
                        {
                            int i = 1;
                            foreach (var row in margeGroupRows[mainGroup][id])
                            {
                                var rfield = field + "_" + i;
                                if (idRow[id].IsNull(rfield))
                                {
                                    idRow[id][rfield] = row[field];
                                }
                                i++;
                            }
                        }
                    }
                }
            }
        }
    }
}
