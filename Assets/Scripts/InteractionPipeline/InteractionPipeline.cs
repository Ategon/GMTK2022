using System.Collections;
using System.Collections.Generic;

namespace DataPipeline
{
    public class InteractionPipeline<T, R>
        where T : IData
        where R : IDataReader<T>
    {
        private List<IGenerator<T>> generators;
        private List<IHandler<T, R>> handlers;
        private IDataInitializer<T, R> dataInitializer;
        private T data;
        private R dataReader;

        public InteractionPipeline(IDataInitializer<T, R> dataInitializer)
        {
            generators = new List<IGenerator<T>>();
            handlers = new List<IHandler<T, R>>();
            this.dataInitializer = dataInitializer;
        }

        public void Execute()
        {
            if (generators.Count > 0)
            {
                ExecuteEvenWithNoGenerators();
            }
        }

        public void ExecuteEvenWithNoGenerators()
        {
            if (handlers.Count > 0)
            {
                WriteData();
                HandleData();
            }
        }

        private void WriteData()
        {
            int writeCount = 0;

            NewData();
            List<IGenerator<T>> writting = new List<IGenerator<T>>();
            List<IGenerator<T>> writtingNext = new List<IGenerator<T>>();

            writting.AddRange(generators);
            writting.ForEach(x => x.Start());

            while (writting.Count > 0)
            {
                writting.ForEach(x => x.StartRound());

                foreach (IGenerator<T> generator in writting)
                {
                    generator.Write(data);
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


        private void HandleData()
        {
            foreach (IHandler<T, R> handler in handlers)
            {
                handler.Handle(dataReader);
            }
        }

        private void NewData()
        {
            dataInitializer.NewData();
            data = dataInitializer.GetData();
            dataReader = dataInitializer.GetReader();
        }

        public void AddGenerator(IGenerator<T> generator)
        {
            generators.Add(generator);
        }

        public bool RemoveGenerator(IGenerator<T> generator)
        {
            return generators.Remove(generator);
        }

        public void AddHandler(IHandler<T, R> handler)
        {
            handlers.Add(handler);
        }

        public bool RemoveHandler(IHandler<T, R> handler)
        {
            return handlers.Remove(handler);
        }
    }
}