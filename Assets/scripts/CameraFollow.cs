using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Alvo")]
    public Transform alvo;

    [Header("Configuração — Terceira Pessoa")]
    public float distancia = 6f;
    public float altura = 1.5f;

    [Header("Configuração — Primeira Pessoa")] 
    public float alturaFirstPerson = 0.7f; 

    [Header("Mouse")]
    public float sensibilidade = 3f;
    public float limiteVerticalMin = -20f;
    public float limiteVerticalMax = 60f;

    [HideInInspector] public float rotacaoX;
    [HideInInspector] public bool modoFirstPerson = false;

    private float rotacaoY;
    private bool mouseCapturado = true;

    void Start()
    {
        CapturarMouse(true);
        rotacaoX = transform.eulerAngles.y;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (mouseCapturado)
                CapturarMouse(false);
            else
                Application.Quit();
        }

        if (!mouseCapturado && Input.GetMouseButtonDown(0))
            CapturarMouse(true);

        if (Input.GetKeyDown(KeyCode.V))
            modoFirstPerson = !modoFirstPerson;

        if (!mouseCapturado) return;

        rotacaoX += Input.GetAxis("Mouse X") * sensibilidade;
        rotacaoY -= Input.GetAxis("Mouse Y") * sensibilidade;
        rotacaoY = Mathf.Clamp(rotacaoY, limiteVerticalMin, limiteVerticalMax);
    }

    void LateUpdate()
    {
        if (alvo == null) return;

        if (modoFirstPerson)
            AtualizarFirstPerson();
        else
            AtualizarThirdPerson();
    }

    void AtualizarFirstPerson()
    {
        transform.position = alvo.position + Vector3.up * alturaFirstPerson;

        transform.rotation = Quaternion.Euler(rotacaoY, rotacaoX, 0f);
    }

    void AtualizarThirdPerson()
    {
        Quaternion rotacao = Quaternion.Euler(rotacaoY, rotacaoX, 0f);
        Vector3 posicaoAlvo = alvo.position + rotacao * new Vector3(0f, altura, -distancia);

        transform.position = posicaoAlvo;
        transform.LookAt(alvo.position + Vector3.up * altura);
    }

    void CapturarMouse(bool capturar)
    {
        mouseCapturado = capturar;
        Cursor.lockState = capturar ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !capturar;
    }
}