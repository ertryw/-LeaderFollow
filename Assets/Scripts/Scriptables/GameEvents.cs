using UnityEngine;

[CreateAssetMenu(fileName = "GameEvents", menuName = "ScriptableObjects/GameEvents", order = 2)]
[System.Serializable]
public class GameEvents : ScriptableObject
{
    public delegate void OnStaminaChange(float stamina);
    public static event OnStaminaChange onStaminaChange;

    public void ChangeStamina(float stamina)
    {
        onStaminaChange?.Invoke(stamina);
    }
}
