// HealthComponent.cs
// Adicionar em: Player E em cada inimigo (é o mesmo script para os dois — vida é vida).
// Pense nele como um "serviço" de domínio: guarda o estado (HP atual/máximo) e
// dispara eventos quando esse estado muda, sem saber nada sobre UI ou combate.
// Quem quiser reagir (barra de vida, som de morte, etc.) se inscreve nos eventos.

using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour, IDamageable
{
    [Header("Configuração")]
    [SerializeField] private float maxHealth = 100f;

    [Header("Eventos (arraste listeners no Inspector ou use via código)")]
    // UnityEvent<float,float> = dispara com (vidaAtual, vidaMaxima) toda vez que muda.
    // É o "onChange" que a barra de vida vai escutar.
    public UnityEvent<float, float> OnHealthChanged;

    // Dispara uma única vez quando a vida chega a zero.
    public UnityEvent OnDeath;

    public float CurrentHealth { get; private set; }
    public float MaxHealth => maxHealth;
    public bool IsDead => CurrentHealth <= 0f;

    private void Awake()
    {
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (IsDead) return; // já morreu, ignora dano extra

        CurrentHealth = Mathf.Max(0f, CurrentHealth - amount);
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);

        if (CurrentHealth <= 0f)
        {
            OnDeath?.Invoke();
        }
    }

    public void Heal(float amount)
    {
        if (IsDead) return;

        CurrentHealth = Mathf.Min(maxHealth, CurrentHealth + amount);
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }
}
