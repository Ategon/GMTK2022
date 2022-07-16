using System.Collections;
using System.Collections.Generic;

namespace DataPipeline
{
    public interface IGenerator<T>
    {
        void Start();
        void StartRound();
        void Write(T data);
        bool IsNotDoneWriting();
    }
}
