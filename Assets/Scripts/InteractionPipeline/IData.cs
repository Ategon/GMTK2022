using System.Collections;
using System.Collections.Generic;

namespace DataPipeline
{
    public interface IData
    {
        void Clear();
    }

    public interface IDataReader<T> where T : IData
    {

    }

    public interface IDataInitializer<T, R>
        where T : IData
        where R : IDataReader<T>
    {
        void NewData();
        T GetData();
        R GetReader();
    }
}