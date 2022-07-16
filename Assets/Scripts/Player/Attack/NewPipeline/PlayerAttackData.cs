using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DataPipeline;

public class PlayerAttackData : IData
{
    public string PlayerAttackString;
    public int PlayerAttackInt;
    public void Clear()
    {
        PlayerAttackString = "";
        PlayerAttackInt = 0;
    }
}

public class PlayerAttackDataReader : IDataReader<PlayerAttackData>
{
    private PlayerAttackData data;

    private PlayerAttackDataReader(PlayerAttackData data)
    {
        this.data = data;
    }

    public class PlayerAttackDataInitializer : IDataInitializer<PlayerAttackData, PlayerAttackDataReader>
    {
        private PlayerAttackData data;
        private PlayerAttackDataReader reader;

        public PlayerAttackDataInitializer()
        {

        }

        public void NewData()
        {
            data = new PlayerAttackData();
            reader = new PlayerAttackDataReader(data);
        }

        public PlayerAttackData GetData()
        {
            return data;
        }

        public PlayerAttackDataReader GetReader()
        {
            return reader;
        }
    }
}
