
using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    [Header("Movimentação")]
    [SerializeField] private float velocidade = 6f;

    [Header("Pulos")]
    [SerializeField] private float forcaPulo = 8f;
    [SerializeField] private float forcaPuloDuplo = 7f;

    [Header("Planar")]
    [SerializeField] private float gravidadeNormal = 1f;
    [SerializeField] private float gravidadePlanando = 0.2f;

    private Rigidbody2D rig;
    private Collider2D colisor;
    private Camera cameraPrincipal;

    private int pulosRestantes = 2;

    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        colisor = GetComponent<Collider2D>();
        cameraPrincipal = Camera.main;

        rig.gravityScale = gravidadeNormal;
    }

    private void Update()
    {
        VerificarPulo();
        VerificarPlanar();
    }

    private void FixedUpdate()
    {
        if (Keyboard.current == null)
            return;

        float eixo = 0f;

        if (Keyboard.current.aKey.isPressed ||
            Keyboard.current.leftArrowKey.isPressed)
        {
            eixo = -1f;
        }

        if (Keyboard.current.dKey.isPressed ||
            Keyboard.current.rightArrowKey.isPressed)
        {
            eixo = 1f;
        }

        float velocidadeHorizontal = eixo * velocidade;

        LimitarVelocidadeHorizontal(ref velocidadeHorizontal);

        rig.linearVelocity = new Vector2(
            velocidadeHorizontal,
            rig.linearVelocity.y
        );
    }

    private void LimitarVelocidadeHorizontal(ref float velocidadeHorizontal)
    {
        if (cameraPrincipal == null || colisor == null)
            return;

        float distanciaCamera = Mathf.Abs(
            transform.position.z - cameraPrincipal.transform.position.z
        );

        float limiteEsquerdo = cameraPrincipal.ViewportToWorldPoint(
            new Vector3(0f, 0.5f, distanciaCamera)
        ).x;

        float limiteDireito = cameraPrincipal.ViewportToWorldPoint(
            new Vector3(1f, 0.5f, distanciaCamera)
        ).x;

        float larguraJogador = colisor.bounds.extents.x;

        limiteEsquerdo += larguraJogador;
        limiteDireito -= larguraJogador;

        if (rig.position.x <= limiteEsquerdo && velocidadeHorizontal < 0f)
        {
            velocidadeHorizontal = 0f;
        }

        if (rig.position.x >= limiteDireito && velocidadeHorizontal > 0f)
        {
            velocidadeHorizontal = 0f;
        }
    }

    private void VerificarPulo()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.spaceKey.wasPressedThisFrame &&
            pulosRestantes > 0)
        {
            float forcaAtual = pulosRestantes == 2
                ? forcaPulo
                : forcaPuloDuplo;

            rig.linearVelocity = new Vector2(
                rig.linearVelocity.x,
                forcaAtual
            );

            pulosRestantes--;
        }
    }

    private void VerificarPlanar()
    {
        if (Keyboard.current == null)
            return;

        bool segurandoEspaco = Keyboard.current.spaceKey.isPressed;
        bool estaCaindo = rig.linearVelocity.y < 0;

        if (segurandoEspaco && estaCaindo)
        {
            rig.gravityScale = gravidadePlanando;
        }
        else
        {
            rig.gravityScale = gravidadeNormal;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            pulosRestantes = 2;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            pulosRestantes = 2;
        }
    }
}
