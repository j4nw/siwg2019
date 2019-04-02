using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class ProblemVisualizationSettings
    {
        private Dictionary<string, string> dict = new Dictionary<string, string>();

        public void Add(string key, string value)
        {
            dict.Add(key, value);
        }

        public void Remove(string key)
        {
            if (dict.ContainsKey(key)) dict.Remove(key);
        }

        public string GetStringValue(string key)
        {
            if (dict.ContainsKey(key)) return dict[key];
            else return null;
        }

        public int GetIntValue(string key)
        {
            if (dict.ContainsKey(key)) return int.Parse(dict[key]);
            else return -1;
        }

        public double GetDoubleValue(string key)
        {
            if (dict.ContainsKey(key)) return double.Parse(dict[key]);
            else return -1;
        }

        public void MakeSettingsFromString(string s)
        {
            dict.Clear();
            string[] sSplit = s.Split('\n');

            foreach (var item in sSplit)
            {
                string[] line = item.Split(' ');
                dict.Add(line[0], line[2]);
            }
        }

        public override string ToString()
        {
            string ret = "";

            foreach (var item in dict)
            {
                ret += item.Key + " : " + item.Value + "\n";
            }

            return ret;
        }
    }
}
