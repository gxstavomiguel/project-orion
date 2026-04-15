using UnityEngine;

[CreateAssetMenu(fileName = "NovoItem", menuName = "RPG/Item")]
public class ItemSO : ScriptableObject
{
    [Header("Identificação")]
    public string nomeItem = "Machado";
    public Sprite icone;           

    [Header("Combat")]
    public float bonusDano = 15f;  
    public string animacaoAtaque = "LightAttack"; 

    [Header("Visual")]
    public GameObject prefabNaMao;
}