using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Ataque Leve")]
    public float cooldownAtaque = 0.5f;
    private float ultimoAtaque = -Mathf.Infinity;

    [SerializeField] private Animator animator;

    private PlayerMovement playerMovement;
    private PlayerEquipment playerEquipment;

    void Start()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        playerMovement  = GetComponent<PlayerMovement>();
        playerEquipment = GetComponent<PlayerEquipment>(); 

        if (animator == null)
            Debug.LogError("[Combat] Animator não encontrado!");
        if (playerEquipment == null)
            Debug.LogError("[Combat] PlayerEquipment não encontrado!");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            TentarAtaqueLeve();
    }

    System.Collections.IEnumerator DarDanoComDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Camera cam = Camera.main;
        if (cam == null) yield break;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * 2.5f, Color.red, 1f);

        if (Physics.Raycast(ray, out hit, 2.5f))
        {
            Enemy enemy = hit.collider.GetComponentInParent<Enemy>();

            if (enemy != null)
            {
                enemy.ReceberDano(25f);
            }
        }
    }

    void TentarAtaqueLeve()
    {
    if (playerEquipment == null || playerEquipment.ItemEquipado == null)
    {
        Debug.Log("[Combat] Nenhuma arma equipada.");
        return;
    }

    bool cooldownOk = Time.time >= ultimoAtaque + cooldownAtaque;
    if (!cooldownOk) return;

    ultimoAtaque = Time.time;

    string trigger = playerEquipment.ItemEquipado.animacaoAtaque;
    animator.SetTrigger(trigger);

    Debug.Log($"[Combat] Ataque com {playerEquipment.ItemEquipado.nomeItem} | trigger: {trigger}");

    StartCoroutine(DarDanoComDelay(0.2f));

    
    }
}