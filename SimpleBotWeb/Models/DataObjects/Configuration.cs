using System.Collections.Generic;
using System.Linq;

namespace SimpleBotWeb.Models.DataObjects
{
    public class Configuration
    {
        private Dictionary<string, string> _values;

        public Configuration()
        {
            _values = new Dictionary<string, string>();
        }

        public void Add(string key, string value)
        {
            if (value == null)
            {
                // adding null value is pointless...
                return;
            }
            _values.Add(key, value);
        }

        public void Remove(string key)
        {
            if (!_values.ContainsKey(key))
            {
                // nothing to do
                return;
            }
            _values.Remove(key);
        }

        public string[] AllKeys()
        {
            return _values.Keys.ToArray();
        }

        public string this[string key]
        {
            get
            {
                string value;
                return _values.TryGetValue(key, out value) ? value : null;
            }
            set
            {
                if (value == null)
                {
                    Remove(key);
                }
                else
                {
                    // Add or update
                    if (!_values.ContainsKey(key))
                    {
                        Add(key, value);
                    }
                    else
                    {
                        _values[key] = value;
                    }
                }
            }
        }
    }
}
