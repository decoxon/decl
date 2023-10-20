using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using declang.Parsing;
using declang.Expressions;

namespace declang
{
    public class Thing : ExpressionResult, IDictionary<string, IExpressionResult>
    {
        private IDictionary<string, IExpressionResult> contents;

        public override string Value => ToString();

        public Thing() : base("Empty Object", ExpressionType.Thing, "")
        {
            contents = new Dictionary<string, IExpressionResult>();
        }

        public Thing(IDictionary<string, IExpressionResult> contents) : this() { this.contents = contents; }

        public Thing(string operationType, ExpressionType type, string value, IEnumerable<ExpressionResult> componentResults = null, IDictionary<string, IExpressionResult> contents = null)
            : base(operationType, type, value, componentResults)
        {
            this.contents = contents ?? new Dictionary<string, IExpressionResult>();
        }

        public IExpressionResult GetValue(string propertyPath, IDictionary<string, IExpressionResult> context = null)
        {
            if (String.IsNullOrWhiteSpace(propertyPath))
            {
                throw new Exception(String.Format("Empty property path for Thing {0}", this));
            }

            var target = context ?? contents;

            var pathElements = new List<string>(propertyPath.Split(ExpressionDefinitions.GetTriggerCharacters(ExpressionType.Accessor)));

            if (pathElements.Count > 0 && target.ContainsKey(pathElements[0]))
            {
                if (pathElements.Count > 1)
                {
                    if (target[pathElements[0]] is Thing)
                    {
                        pathElements.RemoveAt(0);
                        return ((Thing)target[pathElements[0]]).GetValue(String.Join(".", pathElements.ToArray()), (Thing)target[pathElements[0]]);
                    }
                }
                else
                {
                    return target[pathElements[0]];
                }
            }

            throw new Exception(String.Format("Invalid property path: {0} for Thing: {1}", propertyPath, target));
        }

        public IExpressionResult SetValue(string propertyPath, IExpressionResult value, IDictionary<string, IExpressionResult> context = null)
        {
            if (String.IsNullOrWhiteSpace(propertyPath))
            {
                throw new Exception(String.Format("Empty property path for Thing {0}", this));
            }

            var triggerCharacters = ExpressionDefinitions.GetTriggerCharacters(ExpressionType.Accessor);

            var target = context ?? contents;
            var pathElements = new List<string>(propertyPath.Split(triggerCharacters));

            if (pathElements.Count > 0)
            {
                if (pathElements.Count > 1)
                {
                    if (!target.ContainsKey(pathElements[0]) || !(target[pathElements[0]] is Thing))
                    {
                        target[pathElements[0]] = new Thing();
                    }

                    var subObject = target[pathElements[0]] as Thing;
                    pathElements.RemoveAt(0);
                    return subObject.SetValue(String.Join(".", pathElements.ToArray()), value);

                }
                else
                {
                    target[pathElements[0]] = value;
                    return value;
                }
            }

            throw new Exception(String.Format("Invalid property path: {0} for Thing: {1}", propertyPath, target));
        }

        #region ToString
        public override string ToString()
        {
            return contentsToStringBuilder().ToString();
        }

        private StringBuilder contentsToStringBuilder()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("{");

            if (contents.Count > 0)
            {
                foreach (var kvp in contents)
                {
                    keyValuePairToStringBuilder(stringBuilder, kvp);
                    stringBuilder.Append(", ");
                }
                // Remove trailing comma
                stringBuilder.Length -= 2;
            }

            stringBuilder.Append("}");

            return stringBuilder;
        }

        private void keyValuePairToStringBuilder(StringBuilder stringBuilder, KeyValuePair<string, IExpressionResult> kvp)
        {
            stringBuilder.Append(kvp.Key);
            stringBuilder.Append(": ");
            stringBuilder.Append(kvp.Value.ToString());
        }
        #endregion

        #region IDictionary
        public IExpressionResult this[string key]
        {
            get { return contents[key]; }
            set { contents[key] = value; }
        }

        public ICollection<string> Keys => contents.Keys;

        public ICollection<IExpressionResult> Values => contents.Values;

        public int Count => contents.Count;

        public bool IsReadOnly => contents.IsReadOnly;

        public void Add(string key, IExpressionResult value)
        {
            contents.Add(key, value);
        }

        public void Add(KeyValuePair<string, IExpressionResult> item)
        {
            contents.Add(item);
        }

        public void Clear()
        {
            contents.Clear();
        }

        public bool Contains(KeyValuePair<string, IExpressionResult> item)
        {
            return contents.Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return contents.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, IExpressionResult>[] array, int arrayIndex)
        {
            contents.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, IExpressionResult>> GetEnumerator()
        {
            return contents.GetEnumerator();
        }

        public bool Remove(string key)
        {
            return contents.Remove(key);
        }

        public bool Remove(KeyValuePair<string, IExpressionResult> item)
        {
            return contents.Remove(item);
        }

        public bool TryGetValue(string key, out IExpressionResult value)
        {
            return contents.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return contents.GetEnumerator();
        }
        #endregion IDictionary
    }
}
