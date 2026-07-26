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

    private TargetLockSystem targetLock; // NOVO
    void Start()
    {
    CapturarMouse(true);
    rotacaoX = transform.eulerAngles.y;
    if (alvo != null) targetLock = alvo.GetComponent<TargetLockSystem>(); // NOVO
    }

    void Update()
    {
    if (Input.GetKeyDown(KeyCode.Escape))
    {
        if (mouseCapturado) CapturarMouse(false);
        else Application.Quit();
    }

    if (!mouseCapturado && Input.GetMouseButtonDown(0))
        CapturarMouse(true);

    if (Input.GetKeyDown(KeyCode.V))
        modoFirstPerson = !modoFirstPerson;

    if (!mouseCapturado) return;

    bool travado = targetLock != null && targetLock.IsLocked; // NOVO

    if (travado)
    {
        // câmera é forçada a encarar o alvo — mouse X ignorado (aimlock forte)
        Vector3 dir = targetLock.CurrentTarget.position - alvo.position;
        dir.y = 0f;
        if (dir.sqrMagnitude > 0.01f)
        {
            float anguloAlvoCam = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            rotacaoX = Mathf.LerpAngle(rotacaoX, anguloAlvoCam, Time.deltaTime * 8f);
        }
        // mouse Y continua livre, só pra ajustar altura de visão
        rotacaoY -= Input.GetAxis("Mouse Y") * sensibilidade;
        rotacaoY = Mathf.Clamp(rotacaoY, limiteVerticalMin, limiteVerticalMax);
    }
    else
    {
        rotacaoX += Input.GetAxis("Mouse X") * sensibilidade;
        rotacaoY -= Input.GetAxis("Mouse Y") * sensibilidade;
        rotacaoY = Mathf.Clamp(rotacaoY, limiteVerticalMin, limiteVerticalMax);
    }
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









