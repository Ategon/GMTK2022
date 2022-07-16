using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataPipeline;
using DataPipeline.Test;

public class TestInteractionPipeline : MonoBehaviour
{
    private InteractionPipeline<TestData> pipeline;
    private List<Generator> gens;
    private int count;

    void Start()
    {
        pipeline = new InteractionPipeline<TestData>(new TestData());
        gens = new List<Generator>();

        Generator gen0 = new Generator
        {
            NeverDone = false,
            NotDoneTill = 1,
            TestString = "Test 0",
            TestInt = 0
        };
        Generator gen1 = new Generator
        {
            NeverDone = false,
            NotDoneTill = 0,
            TestString = "Test 1",
            TestInt = 1
        };
        Generator gen2 = new Generator
        {
            NeverDone = false,
            NotDoneTill = 0,
            WriteString = false,
            TestString = "You should not see this!",
            TestInt = 420
        };
        Handler handler = new Handler();
        Handler handerTwo = new Handler();

        gens.Add(gen0);
        gens.Add(gen1);
        //gens.Add(gen2);

        foreach (Generator gen in gens)
        {
            pipeline.AddGenerator(gen);
        }

        pipeline.AddHandler(handler);
        pipeline.AddHandler(handerTwo);
    }

    private int lengthOfTest = 0;

    void Update()
    {
        LogFrames();

        pipeline.Execute();

        if (count == lengthOfTest)
        {
            bool hasRemovedAllGenerators = true;
            foreach (Generator gen in gens)
            {
                hasRemovedAllGenerators &= pipeline.RemoveGenerator(gen);
            }

            if (hasRemovedAllGenerators)
            {
                Debug.Log("Removed All Generators");
            }

            gens.Clear();
            Debug.Log("End Test");
        }

        count++;
    }

    private void LogFrames()
    {
        if (count <= lengthOfTest)
        {
            Debug.Log("Frame: " + count);
        }
    }
}
