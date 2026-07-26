using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(HealthComponent))]
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [Header("Combate")]
    public float dano = 10f;
    public float distanciaAtaque = 2f;
    public float cooldownAtaque = 1.5f;

    [Header("Animação")]
    public Animator animator; // arraste o Animator do modelo do lobo aqui
    private static readonly int SpeedParam = Animator.StringToHash("Speed");
    private static readonly int AttackParam = Animator.StringToHash("Attack");
    private static readonly int DeathParam = Animator.StringToHash("Death");

    private float ultimoAtaque;
    private bool morto = false; // evita atacar/andar depois de morrer

    [Header("Referências")]
    private Transform player;
    private HealthComponent playerHealth;
    private NavMeshAgent agent;
    private HealthComponent health;

    void Start()
    {
        ultimoAtaque = -cooldownAtaque;

        GameObject obj = GameObject.FindGameObjectWithTag("Player");
        if (obj != null)
        {
            player = obj.transform;
            playerHealth = obj.GetComponent<HealthComponent>();
            if (playerHealth == null)
                Debug.LogError("Player não tem HealthComponent!");
        }
        else
        {
            Debug.LogError("Player não encontrado!");
        }

        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = distanciaAtaque * 0.9f; // evita ficar entrando/saindo do range

        health = GetComponent<HealthComponent>();
        health.OnDeath.AddListener(Morrer);
    }

    void Update()
    {
        if (morto || player == null) return;

        float distancia = Vector3.Distance(transform.position, player.position);

        if (distancia > distanciaAtaque)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
        {
            agent.isStopped = true;
            TentarAtacar();
        }

        // alimenta o Animator com a velocidade real do agent (0 = idle, >0 = correndo)
        if (animator != null)
            animator.SetFloat(SpeedParam, agent.velocity.magnitude);
    }

    void TentarAtacar()
    {
        if (Time.time >= ultimoAtaque + cooldownAtaque)
        {
            ultimoAtaque = Time.time;

            if (animator != null)
                animator.SetTrigger(AttackParam);

            playerHealth?.TakeDamage(dano);
        }
    }

    void Morrer()
    {
        if (morto) return; // impede o Destroy/animação de disparar duas vezes
        morto = true;

        Debug.Log("Inimigo morreu");

        // trava o inimigo no lugar e desliga colisão, mas deixa a animação de morte tocar
        agent.isStopped = true;
        agent.enabled = false;
        GetComponent<Collider>().enabled = false;

        if (animator != null)
        {
            animator.SetTrigger(DeathParam);
            Destroy(gameObject, 2.5f); // ajuste pro tempo real da animação de morte
        }
        else
        {
            Destroy(gameObject, 0.1f);
        }
    }
}