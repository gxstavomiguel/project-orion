using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float vida = 100f;
    public float dano = 10f;

    [Header("Combate")]
    public float distanciaAtaque = 2f;
    public float cooldownAtaque = 1.5f;

    private float ultimoAtaque;

    [Header("Referências")]
    private Transform player;
    private NavMeshAgent agent;

    void Start()
    {

        ultimoAtaque = -cooldownAtaque; 

        GameObject obj = GameObject.FindGameObjectWithTag("Player");

        if (obj != null)
            player = obj.transform;
        else
            Debug.LogError("Player não encontrado!");

        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (player == null) return;

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
    }

    void TentarAtacar()
    {
        if (Time.time >= ultimoAtaque + cooldownAtaque)
        {
            ultimoAtaque = Time.time;

            Debug.Log("Inimigo atacou o jogador");

            // FUTURO:
            // player.GetComponent<PlayerHealth>()?.ReceberDano(dano);
        }
    }

    public void ReceberDano(float danoRecebido)
    {
        vida -= danoRecebido;

        Debug.Log($"Inimigo recebeu {danoRecebido} de dano");

        if (vida <= 0)
        {
            Morrer();
        }
    }

    void Morrer()
    {
        Debug.Log("Inimigo morreu");
        Destroy(gameObject);
    }
}