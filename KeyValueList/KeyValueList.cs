// (c) 2017 Chris Hecker.

using System;
using System.Xml;
using System.IO;
using System.Web.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace KeyValueList
{
    public class KeyValueList<TKey, TVal>: IKeyValueList<TKey, TVal> where TKey : IComparable<TKey> where TVal : IComparable<TVal>
    {
        // We return an Enumerator that isn't tied to the internal implementation.
        public IEnumerator Enumerator
        {
            // Returns a new structure so that subsequent changes by other actors don't invalidate the existing Enumerator.
            // If necessary I would use mutual exclusion to isolate changes to the keyValueList.
            get {
                Dictionary<TKey, TVal> dict = new Dictionary<TKey, TVal>();

                keyValueList.ForEach(
                    (item) => { dict.Add(item.Key, item.Value); }
                );

                return dict.GetEnumerator();
            }
        }

        public List<string> DisplayList
        {
            get
            {
                var list = new List<String>();

                keyValueList.ForEach(
                    (item) => { list.Add(item.Key + "=" + item.Value); }
                );
                return list;
            }
        }

        public void AddPair(TKey key, TVal value)
        {
            keyValueList.Add(new KeyValuePair<TKey, TVal>(key, value));
        }
        public void RemovePair(TKey key, TVal value)
        {
            keyValueList.Remove(new KeyValuePair<TKey, TVal>(key, value));
        }
        public void ClearAll()
        {
            keyValueList.Clear();
        }
        public void SaveAsXml(string filePath)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            using (XmlWriter writer = XmlWriter.Create(filePath, settings))
            {
                writer.WriteStartElement("Document");

                writer.WriteStartElement("Header");
                writer.WriteAttributeString("FileType", "KeyValueList");
                writer.WriteAttributeString("KeyType", typeof(TKey).ToString());
                writer.WriteAttributeString("ValueType", typeof(TVal).ToString());
                writer.WriteEndElement();  // Header

                writer.WriteStartElement("Body");
                foreach (KeyValuePair<TKey, TVal> pair in keyValueList)
                {
                    writer.WriteStartElement("Pair");
                    writer.WriteAttributeString("key", pair.Key.ToString());
                    writer.WriteAttributeString("value", pair.Value.ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement(); // Body
                writer.WriteEndElement(); // Document
            }
        }
        public void SaveAsJson(string filePath)
        {
            string jsonText = Json.Encode(keyValueList);
            Byte[] jsonBytes = new UTF8Encoding(true).GetBytes(jsonText);

            using (FileStream writer = File.Create(filePath))
            {
                writer.Write(jsonBytes, 0, jsonBytes.Length);
                writer.Flush();
            }
        }
        public void SortByKey()
        {
            keyValueList.Sort(KeyValueComparers<TKey, TVal>.CompareByKey);
        }
        public void SortByValue()
        {
            keyValueList.Sort(KeyValueComparers<TKey, TVal>.CompareByValue);
        }

        private List<KeyValuePair<TKey, TVal>> keyValueList = new List<KeyValuePair<TKey, TVal>>();
    }
}
