using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataPipeline.Test
{
    public class Handler : IHandler<TestData>
    {
        public void Handle(in TestData data)
        {
            Debug.Log("String: " + data.TestString);
            Debug.Log("Int: " + data.TestInt);
        }
    }
}
