using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Core
{
    public class ProblemVisualizationSettings
    {
        public ObservableCollection<ObservableKeyValuePair> Dict { get; private set; }

        public void Add(string key, string value)
        {
            Dict.Add(new ObservableKeyValuePair(key, value));
        }

        public void Remove(string key)
        {
            foreach (var item in Dict)
            {
                if (item.Key == key) Dict.Remove(item);
            }
        }

        public string GetStringValue(string key)
        {
            foreach (var item in Dict)
            {
                if (item.Key == key) return item.Val;
            }
            return null;            
        }

        public int GetIntValue(string key)
        {
            foreach (var item in Dict)
            {
                if (item.Key == key) return int.Parse(item.Val);
            }
            return -1;            
        }

        public double GetDoubleValue(string key)
        {
            foreach (var item in Dict)
            {
                if (item.Key == key) return double.Parse(item.Val);
            }
            return -1;
        }

        public ProblemVisualizationSettings()
        {
            Dict = new ObservableCollection<ObservableKeyValuePair>();
        }
    }

    public class ObservableKeyValuePair : INotifyPropertyChanged
    {
        private string val;

        public ObservableKeyValuePair(string key, string val)
        {
            Key = key;
            Val = val;
        }

        public string Key { get; set; }
        public string Val
        {
            get
            {
                return val;
            }
            set
            {
                val = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Val"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
