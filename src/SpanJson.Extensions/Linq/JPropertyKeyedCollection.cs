﻿#region License
// Copyright (c) 2007 James Newton-King
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using SpanJson.Utilities;

namespace SpanJson.Linq
{
    internal class JPropertyKeyedCollection : Collection<JToken>
    {
        private static readonly IEqualityComparer<string> Comparer = StringComparer.Ordinal;

        private Dictionary<string, JToken>? _dictionary;

        public JPropertyKeyedCollection() : base(new List<JToken>())
        {
        }

        private void AddKey(string key, JToken item)
        {
            EnsureDictionary();
            _dictionary![key] = item;
        }

        protected void ChangeItemKey(JToken item, string newKey)
        {
            if (!ContainsItem(item))
            {
                ThrowHelper2.ThrowArgumentException_The_specified_item_does_not_exist_in_this_KeyedCollection();
            }

            string keyForItem = GetKeyForItem(item);
            if (!Comparer.Equals(keyForItem, newKey))
            {
                if (newKey is not null)
                {
                    AddKey(newKey, item);
                }

                if (keyForItem is not null)
                {
                    RemoveKey(keyForItem);
                }
            }
        }

        protected override void ClearItems()
        {
            base.ClearItems();

            _dictionary?.Clear();
        }

        public bool Contains(string key)
        {
            if (key is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key); }

            if (_dictionary is not null)
            {
                return _dictionary.ContainsKey(key);
            }

            return false;
        }

        private bool ContainsItem(JToken item)
        {
            if (_dictionary is null)
            {
                return false;
            }

            string key = GetKeyForItem(item);
            return _dictionary.TryGetValue(key, out _);
        }

        private void EnsureDictionary()
        {
            if (_dictionary is null)
            {
                _dictionary = new Dictionary<string, JToken>(Comparer);
            }
        }

        private string GetKeyForItem(JToken item)
        {
            return ((JProperty)item).Name;
        }

        protected override void InsertItem(int index, JToken item)
        {
            AddKey(GetKeyForItem(item), item);
            base.InsertItem(index, item);
        }

        public bool Remove(string key)
        {
            if (key is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key); }

            if (_dictionary is not null)
            {
                return _dictionary.TryGetValue(key, out JToken? value) && Remove(value);
            }

            return false;
        }

        protected override void RemoveItem(int index)
        {
            string keyForItem = GetKeyForItem(Items[index]);
            RemoveKey(keyForItem);
            base.RemoveItem(index);
        }

        private void RemoveKey(string key)
        {
            _dictionary?.Remove(key);
        }

        protected override void SetItem(int index, JToken item)
        {
            string keyForItem = GetKeyForItem(item);
            string keyAtIndex = GetKeyForItem(Items[index]);

            if (Comparer.Equals(keyAtIndex, keyForItem))
            {
                if (_dictionary is not null)
                {
                    _dictionary[keyForItem] = item;
                }
            }
            else
            {
                AddKey(keyForItem, item);

                if (keyAtIndex is not null)
                {
                    RemoveKey(keyAtIndex);
                }
            }
            base.SetItem(index, item);
        }

        public JToken this[string key]
        {
            get
            {
                if (key is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key); }

                if (_dictionary is not null)
                {
                    return _dictionary[key];
                }

                throw ThrowHelper2.GetKeyNotFoundException();
            }
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out JToken value)
        {
            if (_dictionary is null)
            {
                value = null;
                return false;
            }

            return _dictionary.TryGetValue(key, out value);
        }

        public ICollection<string> Keys
        {
            get
            {
                EnsureDictionary();
                return _dictionary!.Keys;
            }
        }

        public ICollection<JToken> Values
        {
            get
            {
                EnsureDictionary();
                return _dictionary!.Values;
            }
        }

        public int IndexOfReference(JToken t)
        {
            return ((List<JToken>)Items).IndexOfReference(t);
        }

        public bool Compare(JPropertyKeyedCollection other)
        {
            if (this == other) { return true; }

            // dictionaries in JavaScript aren't ordered
            // ignore order when comparing properties
            Dictionary<string, JToken>? d1 = _dictionary;
            Dictionary<string, JToken>? d2 = other._dictionary;

            if (d1 is null && d2 is null) { return true; }

            if (d1 is null) { return (0u >= (uint)d2!.Count); }

            if (d2 is null) { return (0u >= (uint)d1.Count); }

            if (d1.Count != d2.Count) { return false; }

            foreach (KeyValuePair<string, JToken> keyAndProperty in d1)
            {
                if (!d2.TryGetValue(keyAndProperty.Key, out JToken? secondValue))
                {
                    return false;
                }

                JProperty p1 = (JProperty)keyAndProperty.Value;
                JProperty p2 = (JProperty)secondValue;

                if (p1.Value is null)
                {
                    return (p2.Value is null);
                }

                if (!p1.Value.DeepEquals(p2.Value))
                {
                    return false;
                }
            }

            return true;
        }
    }
}