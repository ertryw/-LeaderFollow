using TMPro;
using UnityEngine;

public class LeaderStaminaUI : MonoBehaviour
{
    private TMP_Text textValue;

    // Start is called before the first frame update
    void Awake()
    {
        textValue = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        LeaderController.leaderStaminaChange += OnStaminaChange;
    }

    private void OnDisable()
    {
        LeaderController.leaderStaminaChange -= OnStaminaChange;
    }

    private void OnStaminaChange(float stamina)
    {
        textValue.text = ((int)stamina).ToString();
    }
}
