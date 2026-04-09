using UnityEngine;
using UnityEngine.UI;

public class CrosshairUI : MonoBehaviour
{
    [Header("Referência")]
    // Arraste o Main Camera aqui no Inspector
    public CameraFollow cameraFollow;

    [Header("Elementos Visuais")]
    public Image dot;   // Arraste o GameObject "Dot" aqui
    public Image ring;  // Arraste o GameObject "Ring" aqui

    [Header("Animação")]
    // Velocidade do fade ao trocar de modo (0 = instantâneo)
    public float velocidadeFade = 8f;

    // Alpha atual da mira (0 = invisível, 1 = visível)
    private float alphaAtual = 0f;

    // Cor base branca — vamos só mudar o alpha dela
    private Color corDot   = Color.white;
    private Color corRing  = new Color(1f, 1f, 1f, 0.7f); // anel levemente transparente

    void Update()
    {
        if (cameraFollow == null) return;

        // Define se a mira deve estar visível ou não
        float alphaAlvo = cameraFollow.modoFirstPerson ? 1f : 0f;

        // Suaviza o fade entre visível/invisível
        alphaAtual = Mathf.Lerp(alphaAtual, alphaAlvo, Time.deltaTime * velocidadeFade);

        // Aplica o alpha nos dois elementos
        AplicarAlpha(dot,  corDot,  alphaAtual);
        AplicarAlpha(ring, corRing, alphaAtual * 0.7f); // anel sempre mais sutil
    }

    void AplicarAlpha(Image imagem, Color corBase, float alpha)
    {
        if (imagem == null) return;
        corBase.a = alpha;
        imagem.color = corBase;
    }
}