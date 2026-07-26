// WeaponHitbox.cs
// Adicionar em: o objeto "Axe" que já existe na sua hierarquia.
// Requisitos no Axe: um Collider marcado como "Is Trigger" (ex: Box Collider),
// e um Rigidbody (pode ser Kinematic) em algum objeto acima na hierarquia — sem
// Rigidbody em nenhum dos dois lados, o Unity não dispara OnTriggerEnter.
//
// Como ligar ao seu ataque: esse script fica "desarmado" a maior parte do tempo.
// Ele só causa dano durante a janela entre BeginAttack() e EndAttack(), chamados
// via Animation Event na animação do "hit fraco" (ou por código, se seu combate
// ainda não usa Animator — veja o exemplo de chamada manual no final do arquivo).

using UnityEngine;
using System.Collections.Generic;

public class WeaponHitbox : MonoBehaviour
{
    private float damage;
    private bool isAttacking = false;

    // evita bater 2x no mesmo inimigo dentro do mesmo golpe (o trigger pode
    // disparar várias vezes enquanto os colliders se sobrepõem)
    private readonly HashSet<IDamageable> hitThisSwing = new HashSet<IDamageable>();

    public void BeginAttack(float damageAmount)
    {
        damage = damageAmount;
        isAttacking = true;
        hitThisSwing.Clear();
    }

    public void EndAttack()
    {
        isAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isAttacking) return;

        if (other.TryGetComponent<IDamageable>(out var damageable) && !hitThisSwing.Contains(damageable))
        {
            damageable.TakeDamage(damage);
            hitThisSwing.Add(damageable);
        }
    }
}

// EXEMPLO de chamada manual (sem Animation Event), caso seu "hit fraco" atual
// seja feito por Coroutine/timer no script de combate do player:
//
//     [SerializeField] private WeaponHitbox axeHitbox;
//     [SerializeField] private float weakHitDamage = 10f;
//
//     void PerformWeakHit()
//     {
//         axeHitbox.BeginAttack(weakHitDamage);
//         // aguarda a duração do golpe (ex: 0.3s) e desliga
//         Invoke(nameof(EndWeakHit), 0.3f);
//     }
//     void EndWeakHit() => axeHitbox.EndAttack();
