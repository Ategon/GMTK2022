using System.Collections;
using System.Collections.Generic;

namespace DataPipeline
{
    public interface IHandler<T>
    {
        void Handle(in T data);
    }
}