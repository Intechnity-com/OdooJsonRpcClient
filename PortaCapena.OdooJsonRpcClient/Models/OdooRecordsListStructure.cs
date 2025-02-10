using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortaCapena.OdooJsonRpcClient.Models
{

    public class OdooRecordsListStructure : Hashtable
    {
        private readonly ArrayList _keys = new ArrayList();
        private readonly ArrayList _values = new ArrayList();

        public override void Add(object key, object value)
        {
            if (!(key is string))
            {
                throw new ArgumentException("key must be a string.");
            }

            base.Add(key, value);
            _keys.Add(key);
            _values.Add(value);
        }

        public override object this[object key]
        {
            get => base[key];
            set
            {
                if (!(key is string))
                {
                    throw new ArgumentException("key must be a string.");
                }

                base[key] = value;
                _keys.Add(key);
                _values.Add(value);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != typeof(OdooRecordsListStructure))
                return false;
            var xmlRpcStruct = (OdooRecordsListStructure)obj;
            if (Keys.Count != xmlRpcStruct.Count)
                return false;

            foreach (string key in Keys)
            {
                if (!xmlRpcStruct.ContainsKey(key))
                    return false;
                if (!this[key].Equals(xmlRpcStruct[key]))
                    return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return Values.Cast<object>().Aggregate(0, (current, obj) => current ^ obj.GetHashCode());
        }

        public override void Clear()
        {
            base.Clear();
            _keys.Clear();
            _values.Clear();
        }

        public new IDictionaryEnumerator GetEnumerator()
        {
            return new Enumerator(_keys, _values);
        }

        public override ICollection Keys => _keys;

        public override void Remove(object key)
        {
            base.Remove(key);
            var idx = _keys.IndexOf(key);
            if (idx >= 0)
            {
                _keys.RemoveAt(idx);
                _values.RemoveAt(idx);
            }
        }

        public override ICollection Values => _values;

        private class Enumerator : IDictionaryEnumerator
        {
            private readonly ArrayList _keys;
            private readonly ArrayList _values;
            private int _index;

            public Enumerator(ArrayList keys, ArrayList values)
            {
                _keys = keys;
                _values = values;
                _index = -1;
            }

            public void Reset()
            {
                _index = -1;
            }

            public object Current
            {
                get
                {
                    CheckIndex();
                    return new DictionaryEntry(_keys[_index], _values[_index]);
                }
            }

            public bool MoveNext()
            {
                _index++;
                return _index < _keys.Count;
            }

            public DictionaryEntry Entry
            {
                get
                {
                    CheckIndex();
                    return new DictionaryEntry(_keys[_index], _values[_index]);
                }
            }

            public object Key
            {
                get
                {
                    CheckIndex();
                    return _keys[_index];
                }
            }

            public object Value
            {
                get
                {
                    CheckIndex();
                    return _values[_index];
                }
            }

            private void CheckIndex()
            {
                if (_index < 0 || _index >= _keys.Count)
                    throw new InvalidOperationException(
                      "Enumeration has either not started or has already finished.");
            }
        }
    }
}
