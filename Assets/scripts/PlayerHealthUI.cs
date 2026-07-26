// PlayerHealthUI.cs
// Adicionar em: um GameObject dentro do seu Canvas existente (ex: "PlayerHealthBar"),
// que já tem um Slider (UI > Slider) como filho.
// Esse script só "traduz" o evento do HealthComponent para a barra visual —
// ele não sabe nada sobre combate, só sobre desenhar.

using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("Referências (arraste no Inspector)")]
    [SerializeField] private HealthComponent playerHealth; // arraste o objeto Player aqui
    [SerializeField] private Slider healthSlider;           // arraste o Slider da barra aqui

    private void OnEnable()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChanged.AddListener(UpdateBar);
    }

    private void OnDisable()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChanged.RemoveListener(UpdateBar);
    }

    private void Start()
    {
        // garante que a barra já nasce cheia, sem esperar o primeiro dano
        if (playerHealth != null)
            UpdateBar(playerHealth.CurrentHealth, playerHealth.MaxHealth);
    }

    private void UpdateBar(float current, float max)
    {
        healthSlider.maxValue = max;
        healthSlider.value = current;
    }
}
