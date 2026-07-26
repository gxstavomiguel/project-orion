// IDamageable.cs
// Não vai em nenhum GameObject — é só um "contrato" que outros scripts implementam.
// Pense como uma interface TypeScript: qualquer classe que quiser "poder receber dano"
// implementa isso (HealthComponent implementa lá embaixo).

public interface IDamageable
{
    void TakeDamage(float amount);
    bool IsDead { get; }
}
