using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataPipeline.Test
{
    public class Generator : IGenerator<TestData>
    {
        public bool NeverDone = false;
        public int NotDoneTill = 0;
        public bool WriteString = true;
        public bool WriteInt = true;
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
            Debug.Log(TestString + " Start!");
        }

        public void StartRound()
        {
            Debug.Log(TestString + " Start Round!");
        }

        public void Write(ref TestData data)
        {
            if (WriteString) data.TestString = TestString;
            if (WriteInt) data.TestInt = TestInt;

            count++;
        }

        public bool IsNotDoneWriting()
        {
            return count <= NotDoneTill || NeverDone;
        }
    }


}
