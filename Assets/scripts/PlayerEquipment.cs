using UnityEngine;
using UnityEngine.Events;

public class PlayerEquipment : MonoBehaviour
{
    [Header("Ponto de ancoragem na mão")]
    public Transform pontoDaMao;

    [Header("Estado atual — somente leitura em runtime")]
    [SerializeField] private ItemSO itemEquipado = null;
    public ItemSO ItemEquipado => itemEquipado; 

    private GameObject modeloNaMao = null;

    private Transform ultimoPontoNoMundo = null;

    public UnityEvent<ItemSO> OnItemEquipado;
    public UnityEvent<ItemSO> OnItemDropado;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && itemEquipado != null)
            Dropar();
    }

    public void Equipar(ItemSO item, Transform pontoMundoOrigem)
    {
        if (itemEquipado != null)
            Dropar();

        itemEquipado = item;
        ultimoPontoNoMundo = pontoMundoOrigem;

        if (item.prefabNaMao != null && pontoDaMao != null)
        {
            modeloNaMao = Instantiate(item.prefabNaMao, pontoDaMao);
            modeloNaMao.transform.localPosition = Vector3.zero;
            modeloNaMao.transform.localRotation = Quaternion.identity;
        }

        Debug.Log($"[Equipment] Equipado: {item.nomeItem}");
        OnItemEquipado?.Invoke(item); 
    }

    public void Dropar()
    {
        if (itemEquipado == null) return;

        Debug.Log($"[Equipment] Dropado: {itemEquipado.nomeItem}");

        if (ultimoPontoNoMundo != null)
        {
            ultimoPontoNoMundo.position = transform.position + transform.forward * 1f;
            ultimoPontoNoMundo.gameObject.SetActive(true);
        }

        if (modeloNaMao != null)
            Destroy(modeloNaMao);

        OnItemDropado?.Invoke(itemEquipado);
        itemEquipado = null;
        modeloNaMao = null;
    }

    public bool PodeEquipar(ItemSO item)
    {
        return true;
    }
}