using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    private float forcaPulo = 8f;
    private float velocidade = 6f;

    public Rigidbody2D rig;

    private bool podeAndar = true;
    public bool estapulando;


    [SerializeField] private float forcaPuloDuplo = 6f;
    private int maxPulos = 1;
    private int pulosRestantes;

    [SerializeField] private float gravidadeNormal = 1f;
    [SerializeField] private float gravidadePlanando = 0.1f;


    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        rig.gravityScale = gravidadeNormal;

    }

    void Update()
    {
        if (!estapulando)
        {
            pulosRestantes = maxPulos;
        }

        Pulo();
        Planar();

    }

    void FixedUpdate()
    {
        if (podeAndar)
            Mover();
    }

    void Mover()
    {
        float eixo = 0f;

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            eixo = -1f;

        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            eixo = 1f;

        rig.linearVelocity = new Vector2(eixo * velocidade, rig.linearVelocity.y);
    }

    void Pulo()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && pulosRestantes > 0)
        {
            float forcaAtual = pulosRestantes == 1 ? forcaPulo : forcaPuloDuplo;

            rig.linearVelocity = new Vector2(rig.linearVelocity.x, forcaAtual);

            pulosRestantes--;
        }
    }

    void Planar()
    {
        if (estapulando &&
            rig.linearVelocity.y < 0 &&
            Keyboard.current.spaceKey.isPressed)
        {
            rig.gravityScale = gravidadePlanando;
        }
        else
        {
            rig.gravityScale = gravidadeNormal;
        }
    }



}
