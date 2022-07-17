using System.Collections.Generic;
using UnityEngine;

public class CurrentPowerUp : MonoBehaviour
{
    public Queue<Dice2> dices = new Queue<Dice2>();
}

/// <summary>
/// I dunno yet what the dice class will look like, so here's that struct for now.
/// </summary>
public struct Dice2
{
    int faces;
    string power;
}