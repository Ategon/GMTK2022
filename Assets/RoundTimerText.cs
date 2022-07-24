using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataPipeline;
using TMPro;
using Spellbound.Managers;
using System.Linq;

public class RoundTimerText : MonoBehaviour, IHandler<PlayerInteractionState>
{
    private TextMeshProUGUI timerText;

    private void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        GameObject.Find("Player").GetComponent<PlayerInteractionPipline>().AddHandler(this);
    }

    public void Handle(in PlayerInteractionState data)
    {
        switch (data.GameState.roundPhase)
        {
            case RoundPhase.Wave:
                timerText.text = "Survive! " + TimerToString(SplitMinutes(data.GameState.roundLength * 60 - data.GameState.roundTimer));
                break;
            case RoundPhase.Boss:
                timerText.text = $"{RandomlyGenerateString(8)} {RandomlyGenerateString(2)}:{RandomlyGenerateString(2)}";
                break;
            default:
                break;
        }
    }

    System.Random random = new System.Random();

    private string RandomlyGenerateString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    private string TimerToString(double[] parts)
    {
        return $"{parts[0]:0}:{parts[1]:00.00}";
    }

    private double[] SplitMinutes(double time)
    {
        double minutes = 0;

        while (time >= 60)
        {
            time -= 60;
            minutes++;
        }

        return new double[] { minutes, time };
    }
}
