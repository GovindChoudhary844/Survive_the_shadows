using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBarSlider;
    public TMP_Text healthBarText;

    private Damageable playerDamageable;

    private void Awake()
    {
        // Corrected to use FindGameObjectWithTag
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("No Player found in the Scene. Make sure it has a tag 'Player'.");
            return;
        }

        playerDamageable = player.GetComponent<Damageable>();

        if (playerDamageable == null)
        {
            Debug.LogError("Player GameObject does not have a Damageable component.");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (playerDamageable != null)
        {
            healthBarSlider.value = CalculateSliderPercentage(playerDamageable.Health, playerDamageable.MaxHealth);
            healthBarText.text = $"HP {playerDamageable.Health} / {playerDamageable.MaxHealth}";
        }
    }

    private void OnEnable()
    {
        if (playerDamageable != null)
        {
            playerDamageable.healthChanged.AddListener(OnPlayerHealthChanged);
        }
    }

    private void OnDisable()
    {
        if (playerDamageable != null)
        {
            playerDamageable.healthChanged.RemoveListener(OnPlayerHealthChanged);
        }
    }

    private float CalculateSliderPercentage(float currentHealth, float maxHealth)
    {
        // Cast to float to avoid integer division
        return (float)currentHealth / maxHealth;
    }

    private void OnPlayerHealthChanged(int newHealth, int maxHealth)
    {
        healthBarSlider.value = CalculateSliderPercentage(newHealth, maxHealth);
        healthBarText.text = $"HP {newHealth} / {maxHealth}";
    }
}
