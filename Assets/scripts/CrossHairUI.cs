using UnityEngine;
using UnityEngine.UI;

public class CrosshairUI : MonoBehaviour
{
    [Header("Referência")]
    public CameraFollow cameraFollow;

    [Header("Elementos Visuais")]
    public Image dot;   
    public Image ring;  

    [Header("Animação")]
    public float velocidadeFade = 8f;
    private float alphaAtual = 0f;

    private Color corDot   = Color.white;
    private Color corRing  = new Color(1f, 1f, 1f, 0.7f); 

    void Update()
    {
        if (cameraFollow == null) return;

        float alphaAlvo = cameraFollow.modoFirstPerson ? 1f : 0f;

        alphaAtual = Mathf.Lerp(alphaAtual, alphaAlvo, Time.deltaTime * velocidadeFade);

        AplicarAlpha(dot,  corDot,  alphaAtual);
        AplicarAlpha(ring, corRing, alphaAtual * 0.7f); 
    }

    void AplicarAlpha(Image imagem, Color corBase, float alpha)
    {
        if (imagem == null) return;
        corBase.a = alpha;
        imagem.color = corBase;
    }
}