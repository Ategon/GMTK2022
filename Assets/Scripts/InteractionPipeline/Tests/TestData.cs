using System.Collections;
using System.Collections.Generic;

namespace DataPipeline.Test
{
    public class TestData : IData
    {
        public string TestString;
        public int TestInt;
        public void Clear()
        {
            TestString = "";
            TestInt = 0;
        }
    }

    public class TestDataReader : IDataReader<TestData>
    {
        private TestData data;
        public string TestString { get { return data.TestString; } }
        public int TestInt { get { return data.TestInt; } }

        private TestDataReader(TestData data)
        {
            this.data = data;
        }

        public class TestDataInitializer : IDataInitializer<TestData, TestDataReader>
        {
            private TestData data;
            private TestDataReader reader;

            public TestDataInitializer()
            {

            }

            public void NewData()
            {
                data = new TestData();
                reader = new TestDataReader(data);
            }

            public TestData GetData()
            {
                return data;
            }

            public TestDataReader GetReader()
            {
                return reader;
            }
        }
    }
}