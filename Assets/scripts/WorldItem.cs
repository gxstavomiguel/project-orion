using UnityEngine;

public class WorldItem : MonoBehaviour
{
    [Header("Dados do Item")]
    public ItemSO dadosItem; 

    [Header("Interação")]
    public KeyCode teclaPickup = KeyCode.E;
    public string tagPlayer = "Player";

    private PlayerEquipment playerProximo = null;
    private bool playerDentro = false;

    void Update()
    {
        if (!playerDentro || playerProximo == null) return;

        if (Input.GetKeyDown(teclaPickup))
            FazerPickup();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(tagPlayer)) return;

        playerProximo = other.GetComponent<PlayerEquipment>();
        playerDentro = true;

        Debug.Log($"[WorldItem] Player próximo de: {dadosItem.nomeItem}");
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(tagPlayer)) return;

        playerProximo = null;
        playerDentro = false;

    }

    void FazerPickup()
    {
        if (!playerProximo.PodeEquipar(dadosItem)) return;

        playerProximo.Equipar(dadosItem, transform);
        gameObject.SetActive(false);
    }
}