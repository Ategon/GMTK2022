using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataPipeline
{
    [System.Serializable]
    public class InteractionPipeline<T> where T : IData
    {
        private List<IGenerator<T>> generators;
        private List<IHandler<T>> handlers;

        public InteractionPipeline()
        {
            generators = new List<IGenerator<T>>();
            handlers = new List<IHandler<T>>();
        }

        public void Execute(ref T data)
        {
            if (generators.Count > 0)
            {
                ExecuteEvenWithNoGenerators(ref data);
            }
        }

        public void ExecuteEvenWithNoGenerators(ref T data)
        {
            if (handlers.Count > 0)
            {
                WriteData(ref data);
                HandleData(in data);
            }
        }

        public void WriteData(ref T data)
        {
            int writeCount = 0;

            List<IGenerator<T>> writting = new List<IGenerator<T>>();
            List<IGenerator<T>> writtingNext = new List<IGenerator<T>>();

            writting.AddRange(generators);
            writting.ForEach(x => x.Start());

            while (writting.Count > 0)
            {
                writting.ForEach(x => x.StartRound());

                foreach (IGenerator<T> generator in writting)
                {
                    generator.Write(ref data);
                    writeCount++;

                    if (generator.IsNotDoneWriting())
                    {
                        //add better code to handle infinite Write loops (Icecubegame)
                        if (writeCount < 9999)
                        {
                            writtingNext.Add(generator);
                        }
                    }
                }

                writting.Clear();
                List<IGenerator<T>> temp = writting;
                writting = writtingNext;
                writtingNext = temp;
            }
        }


        public void HandleData(in T data)
        {
            foreach (IHandler<T> handler in handlers)
            {
                handler.Handle(data);
            }
        }

        public void AddGenerator(IGenerator<T> generator)
        {
            generators.Add(generator);
        }

        public bool RemoveGenerator(IGenerator<T> generator)
        {
            return generators.Remove(generator);
        }

        public void AddHandler(IHandler<T> handler)
        {
            handlers.Add(handler);
        }

        public bool RemoveHandler(IHandler<T> handler)
        {
            return handlers.Remove(handler);
        }
    }
}