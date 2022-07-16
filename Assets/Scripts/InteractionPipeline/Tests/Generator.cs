using System.Collections;
using System.Collections.Generic;

namespace DataPipeline.Test
{
    public class Generator : IGenerator<TestData>
    {
        public bool NeverDone = false;
        public int NotDoneTill = 0;
        private int count;

        public string TestString = "";
        public int TestInt = 0;

        public Generator()
        {
            count = 0;
        }

        public void Start()
        {
            count = 0;
        }

        public void StartRound()
        {

        }

        public void Write(TestData data)
        {
            data.TestString = TestString;
            data.TestInt = TestInt;

            count++;
        }

        public bool IsNotDoneWriting()
        {
            return count <= NotDoneTill || NeverDone;
        }
    }


}
