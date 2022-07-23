using System;
using UnityEngine;
using DataPipeline;
using Spellbound.Managers;

[Serializable]
public class GameState : IData
{
    public int roundLength;
    public int waveLength;
    public int numRounds;
    public double gameTimer;
    public double roundTimer;
    public double waveTimer;
    public int roundsElapsed;
    public int wavesElapsed;
    public RoundPhase roundPhase;
    public bool spawnBosses;

    public bool bossKilled;

    public void Clear()
    {

    }
}
