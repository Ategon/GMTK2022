using System.Collections;
using System.Collections.Generic;

namespace DataPipeline
{
    public interface IHandler<T, R>
        where T : IData
        where R : IDataReader<T>
    {
        void Handle(R data);
    }
}