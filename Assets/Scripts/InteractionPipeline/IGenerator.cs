using System.Collections;
using System.Collections.Generic;

namespace DataPipeline
{
    public interface IGenerator<T> where T : IData
    {
        void Start();
        void StartRound();
        void Write(ref T data);
        bool IsNotDoneWriting();
    }
}
