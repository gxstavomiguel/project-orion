// using UnityEngine;

// public class PlayerCombat : MonoBehaviour
// {
//     [Header("Ataque Leve")]
//     public float cooldownAtaque = 0.5f;
//     private float ultimoAtaque = -Mathf.Infinity;

//     [SerializeField] private Animator animator;
    
//     private PlayerMovement playerMovement;

//     void Start()
//     {
//         if (animator == null)
//             animator = GetComponentInChildren<Animator>(); 

//         playerMovement = GetComponent<PlayerMovement>();

//         if (animator == null)
//             Debug.LogError("[Combat] Animator não encontrado nem nos filhos!");
//     }

//     void Update()
//     {
//         if (Input.GetMouseButtonDown(0))
//             TentarAtaqueLeve();
//     }

//     void TentarAtaqueLeve()
//     {
//         bool cooldownOk = Time.time >= ultimoAtaque + cooldownAtaque;
//         Debug.Log($"[Combat] Mouse0 pressionado | cooldownOk={cooldownOk}");

//         if (!cooldownOk) return;

//         ultimoAtaque = Time.time;

//         if (animator == null)
//         {
//             Debug.LogError("[Combat] Animator é null!");
//             return;
//         }

//         animator.SetTrigger("LightAttack");
//         Debug.Log("[Combat] Trigger 'LightAttack' enviado!");
//     }
// }

using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Ataque Leve")]
    public float cooldownAtaque = 0.5f;
    private float ultimoAtaque = -Mathf.Infinity;

    [SerializeField] private Animator animator;

    private PlayerMovement playerMovement;
    private PlayerEquipment playerEquipment; // <-- novo

    void Start()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        playerMovement  = GetComponent<PlayerMovement>();
        playerEquipment = GetComponent<PlayerEquipment>(); // busca no mesmo GO

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

    void TentarAtaqueLeve()
    {
        // sem arma equipada, não ataca (troque por soco se quiser)
        if (playerEquipment == null || playerEquipment.ItemEquipado == null)
        {
            Debug.Log("[Combat] Nenhuma arma equipada.");
            return;
        }

        bool cooldownOk = Time.time >= ultimoAtaque + cooldownAtaque;
        if (!cooldownOk) return;

        ultimoAtaque = Time.time;

        // usa o trigger definido no próprio ItemSO — flexível para armas diferentes
        string trigger = playerEquipment.ItemEquipado.animacaoAtaque;
        animator.SetTrigger(trigger);

        Debug.Log($"[Combat] Ataque com {playerEquipment.ItemEquipado.nomeItem} | trigger: {trigger}");
    }
}