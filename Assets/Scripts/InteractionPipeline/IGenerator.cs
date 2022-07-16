using System.Collections;
using System.Collections.Generic;

namespace DataPipeline
{
    public interface IGenerator<T>
    {
        void Start();
        void StartRound();
        void Write(ref T data);
        bool IsNotDoneWriting();
    }
}
