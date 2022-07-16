using System.Collections;
using System.Collections.Generic;

namespace DataPipeline
{
    public interface IHandler<T> where T : IData
    {
        void Handle(in T data);
    }
}