using UnityEngine;

public class UnitStats
{
    public float Speed { get; set; }
    public float Stamina { get; set; }
    public float Mobility { get; set; }

    public UnitStats(GameLimits gameLimits)
    {
        Speed = Random.Range(gameLimits.speed.min, gameLimits.speed.max);
        Stamina = Random.Range(gameLimits.stamina.min, gameLimits.stamina.max);
        Mobility = Random.Range(gameLimits.mobility.min, gameLimits.mobility.max);
    }
}
