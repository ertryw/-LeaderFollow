using UnityEngine;

[System.Serializable]
public class Limits
{
    public float min;
    public float max;
}

[System.Serializable]
public class LimitsLow
{
    [Range(0.1f, 1f)]
    public float min;
    [Range(0.1f, 1f)]
    public float max;
}

[CreateAssetMenu(fileName = "GameLimitsData", menuName = "ScriptableObjects/GameLimitsScriptableObject", order = 1)]
[System.Serializable]
public class GameLimits : ScriptableObject
{
    public Limits speed;
    public Limits stamina;
    public LimitsLow mobility;
}
