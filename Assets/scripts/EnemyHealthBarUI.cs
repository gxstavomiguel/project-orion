// EnemyHealthBarUI.cs
// Adicionar em: um Canvas filho do inimigo, do tipo "World Space" (não Screen Space!),
// posicionado ~2 unidades acima da cabeça, com um Slider dentro dele.
// Começa desativado — o TargetLockSystem que liga/desliga ele quando você mira.

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class EnemyHealthBarUI : MonoBehaviour
{
    [Header("Referências (arraste no Inspector)")]
    [SerializeField] private HealthComponent enemyHealth; // arraste o inimigo (pai) aqui
    [SerializeField] private Slider healthSlider;          // arraste o Slider filho aqui

    private Canvas barCanvas;
    private Camera mainCam;

    private void Awake()
    {
        barCanvas = GetComponent<Canvas>();
        mainCam = Camera.main;
        SetVisible(false); // some por padrão
    }

    private void OnEnable()
    {
        if (enemyHealth != null)
            enemyHealth.OnHealthChanged.AddListener(UpdateBar);
    }

    private void OnDisable()
    {
        if (enemyHealth != null)
            enemyHealth.OnHealthChanged.RemoveListener(UpdateBar);
    }

    private void LateUpdate()
    {
        // "Billboard": faz a barra sempre encarar a câmera, senão ela vira de lado
        // conforme o inimigo se move. LateUpdate porque a câmera já se moveu nesse frame.
        if (barCanvas.enabled && mainCam != null)
        {
            transform.forward = mainCam.transform.forward;
        }
    }

    private void UpdateBar(float current, float max)
    {
        healthSlider.maxValue = max;
        healthSlider.value = current;
    }

    public void SetVisible(bool visible)
    {
        barCanvas.enabled = visible;
    }
}
