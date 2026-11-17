
using UnityEngine;
using UnityEngine.Serialization;

public class AerialEnemy : MonoBehaviour
{
    [Header("Atributos de movimentação")]
    [SerializeField] private StatDefinition atributoVelocidadeVertical;
    [SerializeField] private StatDefinition atributoVelocidadeMovimento;

    [Header("Sinalizador")]
    [FormerlySerializedAs("prefabSombra")]
    [SerializeField] private GameObject prefabSinalizador;
    [SerializeField] private Vector2 tamanhoSombraInicial =
        new Vector2(0.25f, 0.08f);
    [SerializeField] private Vector2 tamanhoSombraFinal =
        new Vector2(1.2f, 0.25f);
    [SerializeField, Range(0f, 1f)] private float opacidadeInicial = 0.12f;

    [Header("Queda")]
    [SerializeField] private float alturaImpacto = -3.7f;
    [SerializeField] private float alturaInicialViewport = 1.25f;

    [Header("Posicionamento")]
    [SerializeField, Range(0f, 0.4f)] private float margemHorizontal = 0.15f;

    private Camera cameraPrincipal;
    private Transform sombra;
    private SpriteRenderer imagemSombra;
    private Collider2D colisor;

    private StatContainer atributos;
    private float alturaInicial;

    private bool iniciado;
    private bool caindo;
    private bool noChao;

    public void IniciarQueda(
        Camera camera,
        float velocidade,
        float velocidadeHorizontal)
    {
        cameraPrincipal = camera;
        atributos = GetComponent<StatContainer>();

        if (cameraPrincipal == null ||
            prefabSinalizador == null ||
            atributos == null ||
            atributoVelocidadeVertical == null ||
            atributoVelocidadeMovimento == null)
        {
            Debug.LogError(
                "A entidade aérea precisa de câmera, sinalizador, " +
                "StatContainer e das definições de velocidade!",
                this
            );

            Destroy(gameObject);
            return;
        }

        atributos.DefinirValorBase(
            atributoVelocidadeVertical,
            velocidade
        );

        atributos.DefinirValorBase(
            atributoVelocidadeMovimento,
            velocidadeHorizontal
        );

        colisor = GetComponent<Collider2D>();

        float distanciaCamera = Mathf.Abs(
            transform.position.z - cameraPrincipal.transform.position.z
        );

        float posicaoX = Random.Range(
            margemHorizontal,
            1f - margemHorizontal
        );

        Vector3 posicaoInicial = cameraPrincipal.ViewportToWorldPoint(
            new Vector3(posicaoX, alturaInicialViewport, distanciaCamera)
        );

        alturaInicial = posicaoInicial.y;
        posicaoInicial.z = 0f;
        transform.position = posicaoInicial;

        Vector3 posicaoSombra = new Vector3(
            posicaoInicial.x,
            alturaImpacto,
            0f
        );

        GameObject novoSinalizador = Instantiate(
            prefabSinalizador,
            posicaoSombra,
            Quaternion.identity
        );

        sombra = novoSinalizador.transform;
        imagemSombra = novoSinalizador.GetComponent<SpriteRenderer>();

        if (imagemSombra != null)
        {
            imagemSombra.color = new Color(
                0f, 0f, 0f, opacidadeInicial
            );
        }

        sombra.localScale = new Vector3(
            tamanhoSombraInicial.x,
            tamanhoSombraInicial.y,
            1f
        );

        iniciado = true;
        caindo = true;
    }

    private void Update()
    {
        if (!iniciado)
            return;

        if (caindo)
            AtualizarQueda();
        else if (noChao)
            AtualizarMovimentoNoChao();
    }

    private void AtualizarQueda()
    {
        float velocidade = atributos.ObterValor(
            atributoVelocidadeVertical
        );

        transform.position += Vector3.down *
            velocidade * Time.deltaTime;

        AtualizarSinalizador();

        if (transform.position.y <= alturaImpacto)
            AtingirChao();
    }

    private void AtualizarSinalizador()
    {
        if (sombra == null)
            return;

        float alturaTopoTela = cameraPrincipal.ViewportToWorldPoint(
            new Vector3(0.5f, 1f, Mathf.Abs(
                transform.position.z - cameraPrincipal.transform.position.z
            ))
        ).y;

        float progresso = Mathf.InverseLerp(
            alturaInicial,
            alturaTopoTela,
            transform.position.y
        );

        Vector2 tamanho = Vector2.Lerp(
            tamanhoSombraInicial,
            tamanhoSombraFinal,
            progresso
        );

        sombra.localScale = new Vector3(
            tamanho.x,
            tamanho.y,
            1f
        );

        if (imagemSombra != null)
        {
            float opacidade = Mathf.Lerp(
                opacidadeInicial,
                1f,
                progresso
            );

            imagemSombra.color = new Color(
                0f, 0f, 0f, opacidade
            );
        }
    }

    private void AtingirChao()
    {
        Vector3 posicao = transform.position;
        posicao.y = alturaImpacto;
        transform.position = posicao;

        caindo = false;
        noChao = true;

        if (colisor != null)
            colisor.enabled = false;

        if (sombra != null)
            Destroy(sombra.gameObject);
    }

    private void AtualizarMovimentoNoChao()
    {
        float velocidade = atributos.ObterValor(
            atributoVelocidadeMovimento
        );

        transform.position += Vector3.left *
            velocidade * Time.deltaTime;

        Vector3 posicaoNaTela =
            cameraPrincipal.WorldToViewportPoint(transform.position);

        if (posicaoNaTela.x < -0.1f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!caindo)
            return;

        if (collision.CompareTag("Player"))
            Debug.Log("Jogador atingido pela flecha!");
    }
}
