using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Configuração")]
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public float raioAtivacao = 15f;
    public float delayRespawn = 5f;

    private Transform player;
    private GameObject inimigoAtual;
    private bool aguardandoRespawn = false;
    private float tempoMorte;

    void Start()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Player");
        if (obj != null) player = obj.transform;

        // spawna o primeiro inimigo assim que a cena carrega
        SpawnInimigo();
    }

    void Update()
    {
        if (player == null) return;

        float distancia = Vector3.Distance(player.position, spawnPoint.position);
        bool playerPerto = distancia <= raioAtivacao;

        if (inimigoAtual == null && !aguardandoRespawn)
        {
            aguardandoRespawn = true;
            tempoMorte = Time.time;
        }

        if (aguardandoRespawn && playerPerto && Time.time >= tempoMorte + delayRespawn)
        {
            SpawnInimigo();
            aguardandoRespawn = false;
        }
    }

    void SpawnInimigo()
    {
        inimigoAtual = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        HealthComponent hc = inimigoAtual.GetComponent<HealthComponent>();
        if (hc != null)
            hc.OnDeath.AddListener(() => inimigoAtual = null);
    }
}