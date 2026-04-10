using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    public TextMeshProUGUI textoFPS;

    private float timer;
    private float fps;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 0.5f)
        {
            fps = 1f / Time.unscaledDeltaTime;
            timer = 0f;

            if (fps >= 60)
                textoFPS.color = Color.green;
            else if (fps >= 30)
                textoFPS.color = Color.yellow;
            else
                textoFPS.color = Color.red;

            textoFPS.text = $"FPS: {Mathf.RoundToInt(fps)}";
        }
    }
}