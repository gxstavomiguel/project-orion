using UnityEngine;

public class AnimatorManagerController : MonoBehaviour
{
    public Animator animator;

    private Rigidbody rb;
    private PlayerMovement playerMovement;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        bool atacando = stateInfo.IsName("LightAttack");

        if (!atacando)
        {
        bool estaMovendo = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;
        bool estaCorrendo = Input.GetKey(KeyCode.LeftControl) && estaMovendo;
        bool estaDevagar = Input.GetKey(KeyCode.LeftShift) && estaMovendo;

        float speedAlvo;
        if (!estaMovendo)       speedAlvo = 0f;
        else if (estaCorrendo)  speedAlvo = 1f;
        else if (estaDevagar)   speedAlvo = 0.25f;
        else                    speedAlvo = 0.5f;

        float speedSuave = Mathf.Lerp(animator.GetFloat("Speed"), speedAlvo, Time.deltaTime * 10f);
        animator.SetFloat("Speed", speedSuave);

        animator.SetBool("IsJumping", !playerMovement.noChao);
        }
        
    }
}