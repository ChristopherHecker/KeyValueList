// (c) 2017 Chris Hecker.  All rights reserved.

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestKeyValueList
{
    using KeyValueList;

    [TestClass]
    public class UnitTest
    {
        private static readonly Dictionary<string, string> stringTestData = setStringData();
        private static readonly Dictionary<int, double> numericTestData = setNumericData();

        private static Dictionary<string, string> setStringData()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("transportation", "car");
            dict.Add("food", "pizza");
            dict.Add("animal", "elephant");
            dict.Add("building", "stadium");
            dict.Add("phone", "android");
            dict.Add("decoration", "balloon");
            dict.Add("robot", "android");

            return dict;
        }

        private static Dictionary<int, double> setNumericData()
        {
            Dictionary<int, double> dict = new Dictionary<int, double>();
            dict.Add(1, 3.0);
            dict.Add(12, 17.0);
            dict.Add(-45, 8873.34);
            dict.Add(13, -33.553993);
            dict.Add(9888484, 3997277.893837);
            dict.Add(0, 0.0);
            dict.Add(15, 8873.34);

            return dict;
        }

        private static TestSuite<int, double> CreateNumericSuite()
        {
            return new TestSuite<int, double>(numericTestData, new KeyValueList<int, double>());
        }

        private static TestSuite<string, string> CreateStringSuite()
        {
            return new TestSuite<string, string>(stringTestData, new KeyValueList<string, string>());
        }

        [TestMethod]
        public void TestAddString()
        {
            CreateStringSuite().TestAddPair();
        }

        [TestMethod]
        public void TestRemoveString()
        {
            CreateStringSuite().TestRemovePair();
        }

        [TestMethod]
        public void TestClearAllString()
        {
            CreateStringSuite().TestClearAll();
        }

        [TestMethod]
        public void TestSortByKeyString()
        {
            CreateStringSuite().TestSortByKey();
        }

        [TestMethod]
        public void TestSortByValueString()
        {
            CreateStringSuite().TestSortByValue();
        }

        [TestMethod]
        public void TestSaveAsXmlString()
        {
            CreateStringSuite().TestSaveAsXml("TestSaveAsXmlString.xml");
        }

        [TestMethod]
        public void TestSaveAsJsonString()
        {
            CreateStringSuite().TestSaveAsJson("TestSaveAsJsonString.xml");
        }

    
        [TestMethod]
        public void TestAddNumeric()
        {
            CreateNumericSuite().TestAddPair();
        }

        [TestMethod]
        public void TestRemoveNumeric()
        {
            CreateNumericSuite().TestRemovePair();
        }

        [TestMethod]
        public void TestClearAllNumeric()
        {
            CreateNumericSuite().TestClearAll();
        }

        [TestMethod]
        public void TestSortByKeyNumeric()
        {
            CreateNumericSuite().TestSortByKey();
        }

        [TestMethod]
        public void TestSortByValueNumeric()
        {
            CreateNumericSuite().TestSortByValue();
        }

        [TestMethod]
        public void TestSaveAsXmlNumeric()
        {
            CreateNumericSuite().TestSaveAsXml("TestSaveAsXmlNumeric.xml");
        }

        [TestMethod]
        public void TestSaveAsJsonNumeric()
        {
            CreateNumericSuite().TestSaveAsJson("TestSaveAsJsonNumeric.json");
        }

        private class TestSuite<TKey, TVal> where TKey : IComparable<TKey> where TVal : IComparable<TVal>
        {
            const int addAllItems = 0;

            public TestSuite(Dictionary<TKey, TVal> data, KeyValueList<TKey, TVal> list)
            {
                testData = data;
                testList = list;
            }

            private IEnumerator AddFirstDataItem()
            {
                IEnumerator dataEnum = testData.GetEnumerator();
                Assert.IsTrue(dataEnum.MoveNext());

                var pair = (KeyValuePair<TKey, TVal>)dataEnum.Current;
                testList.AddPair(pair.Key, pair.Value);

                return dataEnum;
            }

            private int AddDataItems(int count)
            {
                int counter = 0;

                foreach(var item in testData)
                {
                    testList.AddPair(item.Key, item.Value);
                    if (++counter == count)
                        break;
                }
                return counter;
            }

            public void TestAddPair()
            {
                IEnumerator dataEnum = AddFirstDataItem();

                IEnumerator listEnum = testList.Enumerator;
                Assert.IsTrue(listEnum.MoveNext());
                Assert.AreEqual(listEnum.Current, dataEnum.Current);
                Assert.IsFalse(listEnum.MoveNext());
            }

            public void TestRemovePair()
            {
                IEnumerator dataEnum = AddFirstDataItem();

                var pair = (KeyValuePair<TKey, TVal>)dataEnum.Current;
                testList.RemovePair(pair.Key, pair.Value);

                IEnumerator listEnum = testList.Enumerator;
                Assert.IsFalse(listEnum.MoveNext());
            }

            public void TestClearAll()
            {
                IEnumerator dataEnum = AddFirstDataItem();

                testList.ClearAll();

                IEnumerator listEnum = testList.Enumerator;
                Assert.IsFalse(listEnum.MoveNext());
            }

            public void TestSortByKey()
            {
                int count = AddDataItems(addAllItems);

                testList.SortByKey();

                IEnumerator listEnum = testList.Enumerator;

                int index = 0;
                TKey oldKey = default;

                while (index++ < count)
                {
                    Assert.IsTrue(listEnum.MoveNext());
                    TKey newKey = ((KeyValuePair<TKey, TVal>)listEnum.Current).Key;

                    if (index > 1)
                        Assert.IsTrue(newKey.CompareTo(oldKey) > -1);
                    oldKey = newKey;
                }
                Assert.IsFalse(listEnum.MoveNext());
            }

            public void TestSortByValue()
            {
                int count = AddDataItems(addAllItems);

                testList.SortByValue();

                IEnumerator listEnum = testList.Enumerator;

                int index = 0;
                TVal oldVal = default;

                while (index++ < count)
                {
                    Assert.IsTrue(listEnum.MoveNext());
                    TVal newVal = ((KeyValuePair<TKey, TVal>)listEnum.Current).Value;

                    if (index > 1)
                        Assert.IsTrue(newVal.CompareTo(oldVal) > -1);
                    oldVal = newVal;
                }
                Assert.IsFalse(listEnum.MoveNext());
            }

            public void TestSaveAsXml(string testFile)
            {
                int count = AddDataItems(addAllItems);

                try
                {
                    testList.SaveAsXml(testFile);

                    Assert.IsTrue(File.Exists(testFile));
                    // Normally I would add a lot of XML testing here.
                }
                finally
                {
                    if (File.Exists(testFile))
                    {
                        File.Delete(testFile);
                    }
                }
                
            }

            public void TestSaveAsJson(string testFile)
            {
                int count = AddDataItems(addAllItems);

                try
                {
                    testList.SaveAsJson(testFile);

                    Assert.IsTrue(File.Exists(testFile));
                    // Normally I would add a lot of JSON testing here.
                }
                finally
                {
                    if (File.Exists(testFile))
                    {
                        File.Delete(testFile);
                    }
                }

            }
            private Dictionary<TKey, TVal> testData;
            private KeyValueList<TKey, TVal> testList;
        }
    }
}
