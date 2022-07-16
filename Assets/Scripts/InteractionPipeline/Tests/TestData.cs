namespace DataPipeline.Test
{
    public struct TestData : IData
    {
        public string TestString;
        public int TestInt;

        public void Clear()
        {
            TestString = "";
            TestInt = 0;
        }
    }
}