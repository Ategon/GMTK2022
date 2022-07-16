using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataPipeline;
using DataPipeline.Test;

public class TestInteractionPipeline : MonoBehaviour
{
    private InteractionPipeline<TestData, TestDataReader> pipeline;
    private List<Generator> gens;
    private int count;

    void Start()
    {
        pipeline = new InteractionPipeline<TestData, TestDataReader>(new TestDataReader.TestDataInitializer());
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
        Handler handler = new Handler();

        gens.Add(gen0);
        gens.Add(gen1);

        foreach (Generator gen in gens)
        {
            pipeline.AddGenerator(gen);
        }

        pipeline.AddHandler(handler);
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
