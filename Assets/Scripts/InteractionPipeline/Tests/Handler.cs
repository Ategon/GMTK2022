using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataPipeline.Test
{
    public class Handler : IHandler<TestData, TestDataReader>
    {
        public void Handle(TestDataReader data)
        {
            Debug.Log("Test String: " + data.TestString);
            Debug.Log("Test Int: " + data.TestInt);
        }
    }
}
