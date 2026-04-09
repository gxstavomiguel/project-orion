using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Velocidades")]
    public float velocidadeNormal = 5f;
    public float velocidadeDevagar = 2f;
    public float velocidadeCorrer = 10f;

    [Header("Suavização (Efeito Moderno)")]
    public float tempoSuavidadeRotacao = 0.1f;
    private float velocidadeRotacaoAtual;

    [Header("Pulo")]
    public float forcaPulo = 6f;
    public float distanciaChao = 1.2f;
    public LayerMask layerChao;

    [Header("Referência")]
    public Transform cam;

    private Rigidbody rb;
    private Vector3 direcaoMovimento;
    private float velocidadeAtual;
    // private bool noChao;
    public bool noChao;

    private CameraFollow cameraFollow;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        if (cam == null && Camera.main != null)
            cam = Camera.main.transform;

        // Busca o script CameraFollow na câmera principal
        if (cam != null)
            cameraFollow = cam.GetComponent<CameraFollow>();
    }

    void Update()
    {
        VerificarChao();
        CapturarInput();
        CalcularDirecao();
        ProcessarPulo();
    }

    void FixedUpdate()
    {
        Mover();
        Rotacionar();
    }

    void VerificarChao()
    {
        Vector3 origem = transform.position + Vector3.up * 0.1f;
        noChao = Physics.Raycast(origem, Vector3.down, distanciaChao, layerChao);

         // Linha de debug — aparece na aba "Console" da Unity
    Debug.DrawRay(origem, Vector3.down * distanciaChao, noChao ? Color.green : Color.red);
    }

    void CapturarInput()
    {
        if (Input.GetKey(KeyCode.LeftControl))
            velocidadeAtual = velocidadeCorrer;
        else if (Input.GetKey(KeyCode.LeftShift))
            velocidadeAtual = velocidadeDevagar;
        else
            velocidadeAtual = velocidadeNormal;
    }

    void CalcularDirecao()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 direcaoEntrada = new Vector3(h, 0f, v).normalized;

        if (direcaoEntrada.magnitude >= 0.1f)
        {
            float anguloAlvo = Mathf.Atan2(direcaoEntrada.x, direcaoEntrada.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            direcaoMovimento = Quaternion.Euler(0f, anguloAlvo, 0f) * Vector3.forward;
        }
        else
        {
            direcaoMovimento = Vector3.zero;
        }
    }

    void ProcessarPulo()
    {
        if (Input.GetKeyDown(KeyCode.Space) && noChao)
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, forcaPulo, rb.linearVelocity.z);
    }

    void Mover()
    {
        Vector3 velocidadeAlvo = direcaoMovimento.normalized * (direcaoMovimento.magnitude > 0 ? velocidadeAtual : 0);
        rb.linearVelocity = new Vector3(velocidadeAlvo.x, rb.linearVelocity.y, velocidadeAlvo.z);
    }

    void Rotacionar()
    {
        bool emFirstPerson = cameraFollow != null && cameraFollow.modoFirstPerson;

        if (emFirstPerson)
        {
            // Em 1ª pessoa: corpo sempre alinhado com onde a câmera aponta (eixo Y)
            // Sem suavização — o corpo segue o mouse instantaneamente
            float anguloCamera = cameraFollow.rotacaoX;
            rb.MoveRotation(Quaternion.Euler(0f, anguloCamera, 0f));
        }
        else
        {
            // Em 3ª pessoa: rotação suave baseada na direção do movimento (comportamento original)
            if (direcaoMovimento.magnitude >= 0.1f)
            {
                float anguloAlvo = Mathf.Atan2(direcaoMovimento.x, direcaoMovimento.z) * Mathf.Rad2Deg;
                float anguloSuave = Mathf.SmoothDampAngle(transform.eulerAngles.y, anguloAlvo, ref velocidadeRotacaoAtual, tempoSuavidadeRotacao);
                rb.MoveRotation(Quaternion.Euler(0f, anguloSuave, 0f));
            }
        }
    }
}