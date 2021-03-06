﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Foundation.Collections;

namespace DriverLite.Common
{
    public class ObservableDictionary<K,T> : IObservableMap<K, T>
    {
        private class ObservableDictionaryChangedEventArgs : IMapChangedEventArgs<K>
        {
            public ObservableDictionaryChangedEventArgs(CollectionChange change, K key)
            {
                CollectionChange = change;
                Key = key;
            }

            public CollectionChange CollectionChange { get; private set; }
            public K Key { get; private set; }
        }

        private readonly Dictionary<K, T> _dictionary = new Dictionary<K, T>();
        public event MapChangedEventHandler<K, T> MapChanged;

        private void InvokeMapChanged(CollectionChange change, K key)
        {
            var eventHandler = MapChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new ObservableDictionaryChangedEventArgs(change, key));
            }
        }

        public void Add(K key, T value)
        {
            _dictionary.Add(key, value);
            InvokeMapChanged(CollectionChange.ItemInserted, key);
        }

        public void Add(KeyValuePair<K, T> item)
        {
            Add(item.Key, item.Value);
        }

        public bool Remove(K key)
        {
            if (_dictionary.Remove(key))
            {
                InvokeMapChanged(CollectionChange.ItemRemoved, key);
                return true;
            }
            return false;
        }

        public bool Remove(KeyValuePair<K, T> item)
        {
            T currentValue;
            if (_dictionary.TryGetValue(item.Key, out currentValue) &&
                Equals(item.Value, currentValue) && _dictionary.Remove(item.Key))
            {
                InvokeMapChanged(CollectionChange.ItemRemoved, item.Key);
                return true;
            }
            return false;
        }

        public T this[K key]
        {
            get
            {
                return _dictionary[key];
            }
            set
            {
                _dictionary[key] = value;
                InvokeMapChanged(CollectionChange.ItemChanged, key);
            }
        }

        public void Clear()
        {
            var priorKeys = _dictionary.Keys.ToArray();
            _dictionary.Clear();
            foreach (var key in priorKeys)
            {
                InvokeMapChanged(CollectionChange.ItemRemoved, key);
            }
        }

        public ICollection<K> Keys
        {
            get { return _dictionary.Keys; }
        }

        public bool ContainsKey(K key)
        {
            return _dictionary.ContainsKey(key);
        }

        public bool TryGetValue(K key, out T value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        public ICollection<T> Values
        {
            get { return _dictionary.Values; }
        }

        public bool Contains(KeyValuePair<K, T> item)
        {
            return _dictionary.Contains(item);
        }

        public int Count
        {
            get { return _dictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public IEnumerator<KeyValuePair<K, T>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        public void CopyTo(KeyValuePair<K, T>[] array, int arrayIndex)
        {
            var arraySize = array.Length;
            foreach (var pair in _dictionary)
            {
                if (arrayIndex >= arraySize) break;
                array[arrayIndex++] = pair;
            }
        }
    }
}
