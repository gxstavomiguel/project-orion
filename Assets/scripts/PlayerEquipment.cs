using UnityEngine;
using UnityEngine.Events;

// Coloque no mesmo GameObject do Player (junto com PlayerCombat, PlayerMovement)
public class PlayerEquipment : MonoBehaviour
{
    [Header("Ponto de ancoragem na mão")]
    // Crie um GameObject filho vazio na mão direita do modelo e arraste aqui
    public Transform pontoDaMao;

    [Header("Estado atual — somente leitura em runtime")]
    [SerializeField] private ItemSO itemEquipado = null;
    public ItemSO ItemEquipado => itemEquipado; // propriedade pública de leitura

    // Instância do modelo 3D que aparece na mão
    private GameObject modeloNaMao = null;

    // Referência para devolver ao mundo se dropar
    private Transform ultimoPontoNoMundo = null;

    // Evento para o inventário futuro ouvir
    public UnityEvent<ItemSO> OnItemEquipado;
    public UnityEvent<ItemSO> OnItemDropado;

    void Update()
    {
        // Drop com G (ou outro botão que quiser)
        if (Input.GetKeyDown(KeyCode.G) && itemEquipado != null)
            Dropar();
    }

    // Chamado pelo WorldItem quando o player pega o item
    public void Equipar(ItemSO item, Transform pontoMundoOrigem)
    {
        // se já tem algo equipado, dropa primeiro
        if (itemEquipado != null)
            Dropar();

        itemEquipado = item;
        ultimoPontoNoMundo = pontoMundoOrigem;

        // instancia o modelo 3D na mão, se existir
        if (item.prefabNaMao != null && pontoDaMao != null)
        {
            modeloNaMao = Instantiate(item.prefabNaMao, pontoDaMao);
            modeloNaMao.transform.localPosition = Vector3.zero;
            modeloNaMao.transform.localRotation = Quaternion.identity;
        }

        Debug.Log($"[Equipment] Equipado: {item.nomeItem}");
        OnItemEquipado?.Invoke(item); // avisa quem estiver ouvindo (inventário, UI)
    }

    public void Dropar()
    {
        if (itemEquipado == null) return;

        Debug.Log($"[Equipment] Dropado: {itemEquipado.nomeItem}");

        // reativa o objeto no mundo perto do player
        if (ultimoPontoNoMundo != null)
        {
            ultimoPontoNoMundo.position = transform.position + transform.forward * 1f;
            ultimoPontoNoMundo.gameObject.SetActive(true);
        }

        // destrói o modelo na mão
        if (modeloNaMao != null)
            Destroy(modeloNaMao);

        OnItemDropado?.Invoke(itemEquipado);
        itemEquipado = null;
        modeloNaMao = null;
    }

    // WorldItem chama isso antes de equipar
    public bool PodeEquipar(ItemSO item)
    {
        // MVP: sempre pode (substitui o que tem)
        return true;
    }
}