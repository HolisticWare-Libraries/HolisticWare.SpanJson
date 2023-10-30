#region License
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

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace SpanJson.Linq
{
    partial class JObject : IDictionary<string, JToken?>, IReadOnlyDictionary<string, JToken?>, IDictionary<string, object?>, IReadOnlyDictionary<string, object?>, ICustomTypeDescriptor
    {
        #region -- IReadOnlyDictionary<string, JToken> Members --

        IEnumerable<string> IReadOnlyDictionary<string, JToken?>.Keys => ((IDictionary<string, JToken?>)this).Keys;

        IEnumerable<JToken?> IReadOnlyDictionary<string, JToken?>.Values => this.PropertyValues();

        #endregion

        #region -- IReadOnlyDictionary<string, object> Members --

        IEnumerable<string> IReadOnlyDictionary<string, object?>.Keys => ((IDictionary<string, JToken?>)this).Keys;

        IEnumerable<object?> IReadOnlyDictionary<string, object?>.Values => this.PropertyValues();

        /// <summary>Gets or sets the <see cref="JToken"/> with the specified property name.</summary>
        /// <value></value>
        object? IReadOnlyDictionary<string, object?>.this[string propertyName]
        {
            get
            {
                if (propertyName is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.propertyName); }

                JProperty? property = Property(propertyName);

                return property?.Value;
            }
        }

        /// <summary>Tries to get the <see cref="JToken"/> with the specified property name.</summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if a value was successfully retrieved; otherwise, <c>false</c>.</returns>
        bool IReadOnlyDictionary<string, object?>.TryGetValue(string propertyName, [MaybeNullWhen(false)] out object value)
        {
            JProperty? property = Property(propertyName);
            if (property is not null)
            {
                value = property.Value;
                return true;
            }

            value = null;
            return false;
        }

        #endregion

        #region -- IDictionary<string,JToken> Members --

        /// <summary>Adds the specified property name.</summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        public void Add(string propertyName, JToken? value)
        {
            Add(new JProperty(propertyName, value));
        }

        /// <summary>Determines whether the JSON object has the specified property name.</summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns><c>true</c> if the JSON object has the specified property name; otherwise, <c>false</c>.</returns>
        public bool ContainsKey(string propertyName)
        {
            if (propertyName is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.propertyName); }

            return _properties.Contains(propertyName);
        }

        ICollection<string> IDictionary<string, JToken?>.Keys => _properties.Keys;

        /// <summary>Removes the property with the specified name.</summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns><c>true</c> if item was successfully removed; otherwise, <c>false</c>.</returns>
        public bool Remove(string propertyName)
        {
            JProperty? property = Property(propertyName);
            if (property is null)
            {
                return false;
            }

            property.Remove();
            return true;
        }

        /// <summary>Tries to get the <see cref="JToken"/> with the specified property name.</summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if a value was successfully retrieved; otherwise, <c>false</c>.</returns>
        public bool TryGetValue(string propertyName, [MaybeNullWhen(false)] out JToken value)
        {
            JProperty? property = Property(propertyName);
            if (property is not null)
            {
                value = property.Value;
                return true;
            }

            value = null;
            return false;
        }

        ICollection<JToken?> IDictionary<string, JToken?>.Values => this.Properties().Select(static x => (JToken?)x.Value).ToList();

        #endregion

        #region -- IDictionary<string, object> Members --

        /// <summary>Adds the specified property name.</summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        public void Add(string propertyName, object? value)
        {
            Add(new JProperty(propertyName, value));
        }

        ICollection<string> IDictionary<string, object?>.Keys => ((IDictionary<string, JToken?>)this).Keys;

        /// <summary>Tries to get the <see cref="JToken"/> with the specified property name.</summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if a value was successfully retrieved; otherwise, <c>false</c>.</returns>
        bool IDictionary<string, object?>.TryGetValue(string propertyName, [MaybeNullWhen(false)] out object value)
        {
            JProperty? property = Property(propertyName);
            if (property is not null)
            {
                value = property.Value;
                return true;
            }

            value = null;
            return false;
        }

        /// <summary>Gets or sets the <see cref="JToken"/> with the specified property name.</summary>
        /// <value></value>
        object? IDictionary<string, object?>.this[string propertyName]
        {
            get
            {
                if (propertyName is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.propertyName); }

                JProperty? property = Property(propertyName);

                return property?.Value;
            }
            set
            {
                JProperty? property = Property(propertyName);
                if (property is not null)
                {
                    property.Value = JToken.FromObject(value!);
                }
                else
                {
                    OnPropertyChanging(propertyName);
                    Add(propertyName, value);
                    OnPropertyChanged(propertyName);
                }
            }
        }

        ICollection<object?> IDictionary<string, object?>.Values => this.Properties().Select(static x => (object?)x.Value).ToList();

        #endregion

        #region -- ICollection<KeyValuePair<string,JToken>> Members --

        void ICollection<KeyValuePair<string, JToken?>>.Add(KeyValuePair<string, JToken?> item)
        {
            Add(new JProperty(item.Key, item.Value));
        }

        void ICollection<KeyValuePair<string, JToken?>>.Clear()
        {
            RemoveAll();
        }

        bool ICollection<KeyValuePair<string, JToken?>>.Contains(KeyValuePair<string, JToken?> item)
        {
            JProperty? property = Property(item.Key);
            if (property is null)
            {
                return false;
            }

            return (property.Value == item.Value);
        }

        void ICollection<KeyValuePair<string, JToken?>>.CopyTo(KeyValuePair<string, JToken?>[] array, int arrayIndex)
        {
            if (array is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array); }
            if ((uint)arrayIndex > JsonSharedConstant.TooBigOrNegative) { ThrowHelper2.ThrowArgumentOutOfRangeException_ArrayIndex(); }
            if ((uint)arrayIndex >= (uint)array.Length && arrayIndex != 0) { ThrowHelper2.ThrowArgumentException_ArrayIndex(); }
            if ((uint)Count > (uint)(array.Length - arrayIndex)) { ThrowHelper2.ThrowArgumentException_The_number_of_elements(); }

            int index = 0;
            foreach (JProperty property in _properties)
            {
                array[arrayIndex + index] = new KeyValuePair<string, JToken?>(property.Name, property.Value);
                index++;
            }
        }

        bool ICollection<KeyValuePair<string, JToken?>>.IsReadOnly => false;

        bool ICollection<KeyValuePair<string, JToken?>>.Remove(KeyValuePair<string, JToken?> item)
        {
            if (!((ICollection<KeyValuePair<string, JToken?>>)this).Contains(item))
            {
                return false;
            }

            Remove(item.Key);
            return true;
        }

        #endregion

        #region -- ICollection<KeyValuePair<string, object>> Members --

        void ICollection<KeyValuePair<string, object?>>.Add(KeyValuePair<string, object?> item)
        {
            Add(new JProperty(item.Key, item.Value));
        }

        void ICollection<KeyValuePair<string, object?>>.Clear()
        {
            RemoveAll();
        }

        bool ICollection<KeyValuePair<string, object?>>.Contains(KeyValuePair<string, object?> item)
        {
            JProperty? property = Property(item.Key);
            if (property is null)
            {
                return false;
            }

            return (property.Value == item.Value);
        }

        void ICollection<KeyValuePair<string, object?>>.CopyTo(KeyValuePair<string, object?>[] array, int arrayIndex)
        {
            if (array is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array); }
            if ((uint)arrayIndex > JsonSharedConstant.TooBigOrNegative) { ThrowHelper2.ThrowArgumentOutOfRangeException_ArrayIndex(); }
            if ((uint)arrayIndex >= (uint)array.Length && arrayIndex != 0) { ThrowHelper2.ThrowArgumentException_ArrayIndex(); }
            if ((uint)Count > (uint)(array.Length - arrayIndex)) { ThrowHelper2.ThrowArgumentException_The_number_of_elements(); }

            int index = 0;
            foreach (JProperty property in this.Properties())
            {
                array[arrayIndex + index] = new KeyValuePair<string, object?>(property.Name, property.Value);
                index++;
            }
        }

        bool ICollection<KeyValuePair<string, object?>>.IsReadOnly => false;

        bool ICollection<KeyValuePair<string, object?>>.Remove(KeyValuePair<string, object?> item)
        {
            if (!((ICollection<KeyValuePair<string, object?>>)this).Contains(item))
            {
                return false;
            }

            Remove(item.Key);
            return true;
        }

        #endregion

        #region -- IEnumerable Members --

        /// <summary>Returns an enumerator that can be used to iterate through the collection.</summary>
        /// <returns>A <see cref="IEnumerator{T}"/> that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<string, JToken?>> GetEnumerator()
        {
            foreach (JProperty property in _properties)
            {
                yield return new KeyValuePair<string, JToken?>(property.Name, property.Value);
            }
        }

        /// <summary>Returns an enumerator that can be used to iterate through the collection.</summary>
        /// <returns>A <see cref="IEnumerator{T}"/> that can be used to iterate through the collection.</returns>
        IEnumerator<KeyValuePair<string, object?>> IEnumerable<KeyValuePair<string, object?>>.GetEnumerator()
        {
            foreach (JProperty property in this.Properties())
            {
                yield return new KeyValuePair<string, object?>(property.Name, property.Value);
            }
        }

        #endregion

        // include custom type descriptor on JObject rather than use a provider because the properties are specific to a type

        #region -- ICustomTypeDescriptor --

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            return ((ICustomTypeDescriptor)this).GetProperties(null);
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[]? attributes)
        {
            PropertyDescriptor[] propertiesArray = new PropertyDescriptor[Count];
            int i = 0;
            foreach (KeyValuePair<string, JToken?> propertyValue in this)
            {
                propertiesArray[i] = new JPropertyDescriptor(propertyValue.Key);
                i++;
            }

            return new PropertyDescriptorCollection(propertiesArray);
        }

        AttributeCollection ICustomTypeDescriptor.GetAttributes()
        {
            return AttributeCollection.Empty;
        }

        string? ICustomTypeDescriptor.GetClassName()
        {
            return null;
        }

        string? ICustomTypeDescriptor.GetComponentName()
        {
            return null;
        }

        TypeConverter ICustomTypeDescriptor.GetConverter()
        {
            return new TypeConverter();
        }

        EventDescriptor? ICustomTypeDescriptor.GetDefaultEvent()
        {
            return null;
        }

        PropertyDescriptor? ICustomTypeDescriptor.GetDefaultProperty()
        {
            return null;
        }

        object? ICustomTypeDescriptor.GetEditor(Type editorBaseType)
        {
            return null;
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[]? attributes)
        {
            return EventDescriptorCollection.Empty;
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            return EventDescriptorCollection.Empty;
        }

        object? ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor? pd)
        {
            if (pd is JPropertyDescriptor)
            {
                return this;
            }

            return null;
        }

        #endregion
    }
}
