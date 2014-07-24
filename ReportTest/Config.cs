using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportTest
{
    public class Config
    {
        List<ConfigItem> _ConfigItemList;

        public Config(string ConfigValue)
        {
            _ConfigItemList = new List<ConfigItem>();
            try
            {
                if (string.IsNullOrWhiteSpace(ConfigValue))
                {
                    return;   
                }

                string[] lines = System.Text.RegularExpressions.Regex.Split(ConfigValue, "\r\n");
                foreach (string line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        ConfigItem ci = new ConfigItem(line);
                        _ConfigItemList.Add(ci);
                    }
                }
               
            }
            catch (Exception ex)
            { 
                
            }
        }

        public List<ConfigItem> GetConfigItemList()
        {
            return _ConfigItemList;
        }
    }
}
