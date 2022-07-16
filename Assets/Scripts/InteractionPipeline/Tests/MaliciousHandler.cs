using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataPipeline.Test
{
    public class MaliciousHandler : IHandler<TestData>
    {
        public void Handle(in TestData data)
        {
            data.Clear();
            Debug.Log("String: " + data.TestString);
            Debug.Log("Int: " + data.TestInt);
        }
    }
}
