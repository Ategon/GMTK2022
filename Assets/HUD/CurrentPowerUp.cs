using System.Collections.Generic;
using UnityEngine;

public class CurrentPowerUp : MonoBehaviour
{
    public Queue<Dice> dices = new Queue<Dice>();
}

/// <summary>
/// I dunno yet what the dice class will look like, so here's that struct for now.
/// </summary>
public struct Dice
{
    int faces;
    string power;
}