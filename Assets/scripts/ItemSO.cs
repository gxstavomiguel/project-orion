using UnityEngine;

// ScriptableObject = arquivo de dados, não vai na cena
// Crie via: clique direito no Project > Create > RPG > Item
[CreateAssetMenu(fileName = "NovoItem", menuName = "RPG/Item")]
public class ItemSO : ScriptableObject
{
    [Header("Identificação")]
    public string nomeItem = "Machado";
    public Sprite icone;           // para o inventário futuro

    [Header("Combat")]
    public float bonusDano = 15f;  // dano extra que a arma adiciona
    public string animacaoAtaque = "LightAttack"; // trigger no Animator

    [Header("Visual")]
    // Prefab do modelo 3D que aparece na mão do player
    public GameObject prefabNaMao;
}