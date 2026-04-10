using UnityEngine;

// Coloque esse script no Prefab do machado que fica no chão
// Requer: SphereCollider com "Is Trigger" = true (raio ~2)
public class WorldItem : MonoBehaviour
{
    [Header("Dados do Item")]
    public ItemSO dadosItem; // arraste o ItemSO do machado aqui no Inspector

    [Header("Interação")]
    public KeyCode teclaPickup = KeyCode.E;
    public string tagPlayer = "Player";

    // guarda referência ao player quando ele entra no raio
    private PlayerEquipment playerProximo = null;
    private bool playerDentro = false;

    void Update()
    {
        // só processa se o player estiver no raio
        if (!playerDentro || playerProximo == null) return;

        if (Input.GetKeyDown(teclaPickup))
            FazerPickup();
    }

    // chamado pelo Unity quando um Collider entra no trigger
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(tagPlayer)) return;

        // pega o componente de equipamento do player
        playerProximo = other.GetComponent<PlayerEquipment>();
        playerDentro = true;

        // TODO: mostrar prompt "E para pegar" na UI
        Debug.Log($"[WorldItem] Player próximo de: {dadosItem.nomeItem}");
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(tagPlayer)) return;

        playerProximo = null;
        playerDentro = false;

        // TODO: esconder prompt na UI
    }

    void FazerPickup()
    {
        // verifica se o player consegue pegar (mão livre ou mesmo item)
        if (!playerProximo.PodeEquipar(dadosItem)) return;

        playerProximo.Equipar(dadosItem, transform);
        // desativa o objeto no mundo (não destrói, facilita pooling no futuro)
        gameObject.SetActive(false);
    }
}