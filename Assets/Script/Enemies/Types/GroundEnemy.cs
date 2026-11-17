
using UnityEngine;

public class GroundEnemy : MonoBehaviour
{
    [Header("Atributo de movimentação")]
    [SerializeField] private StatDefinition atributoVelocidadeVertical;

    [Header("Sinalizador")]
    [SerializeField] private GameObject prefabSinalizador;
    [SerializeField] private float alturaSinalizador = -3.95f;
    [SerializeField, Range(0f, 1f)] private float opacidadeInicial = 0.1f;
    [SerializeField, Range(0f, 1f)] private float opacidadeFinal = 1f;

    [Header("Aviso")]
    [SerializeField] private float duracaoAviso = 0.8f;

    [Header("Subida")]
    [SerializeField] private float alturaInicial = -4.5f;
    [SerializeField] private float alturaFinalViewport = 1.1f;

    [Header("Posicionamento")]
    [SerializeField, Range(0f, 0.4f)] private float margemHorizontal = 0.15f;

    private Camera cameraPrincipal;
    private Transform sinalizador;
    private SpriteRenderer imagemSinalizador;
    private SpriteRenderer imagemInimigo;
    private Collider2D colisor;

    private StatContainer atributos;

    private float temporizadorAviso;
    private bool iniciado;
    private bool avisando;
    private bool subindo;

    public void IniciarSubida(Camera camera, float velocidade)
    {
        cameraPrincipal = camera;
        atributos = GetComponent<StatContainer>();

        if (cameraPrincipal == null ||
            prefabSinalizador == null ||
            atributos == null ||
            atributoVelocidadeVertical == null)
        {
            Debug.LogError(
                "O inimigo terrestre precisa de câmera, sinalizador, " +
                "StatContainer e definição de VelocidadeVertical!",
                this
            );

            Destroy(gameObject);
            return;
        }

        atributos.DefinirValorBase(
            atributoVelocidadeVertical,
            velocidade
        );

        colisor = GetComponent<Collider2D>();
        imagemInimigo = GetComponent<SpriteRenderer>();

        if (imagemInimigo == null)
        {
            Debug.LogError(
                "O inimigo terrestre precisa de Sprite Renderer!",
                this
            );

            Destroy(gameObject);
            return;
        }

        imagemInimigo.enabled = false;

        if (colisor != null)
            colisor.enabled = false;

        float distanciaCamera = Mathf.Abs(
            transform.position.z - cameraPrincipal.transform.position.z
        );

        float posicaoX = Random.Range(
            margemHorizontal,
            1f - margemHorizontal
        );

        Vector3 posicaoInicial = cameraPrincipal.ViewportToWorldPoint(
            new Vector3(posicaoX, 0f, distanciaCamera)
        );

        posicaoInicial.y = alturaInicial;
        posicaoInicial.z = 0f;
        transform.position = posicaoInicial;

        Vector3 posicaoSinalizador = new Vector3(
            posicaoInicial.x,
            alturaSinalizador,
            0f
        );

        GameObject novoSinalizador = Instantiate(
            prefabSinalizador,
            posicaoSinalizador,
            Quaternion.identity
        );

        sinalizador = novoSinalizador.transform;
        imagemSinalizador = novoSinalizador.GetComponent<SpriteRenderer>();

        if (imagemSinalizador != null)
        {
            imagemSinalizador.color = new Color(
                0f, 0f, 0f, opacidadeInicial
            );
        }

        iniciado = true;
        avisando = true;
    }

    private void Update()
    {
        if (!iniciado)
            return;

        if (avisando)
            AtualizarAviso();
        else if (subindo)
            AtualizarSubida();
    }

    private void AtualizarAviso()
    {
        temporizadorAviso += Time.deltaTime;

        float progresso = Mathf.Clamp01(
            temporizadorAviso / duracaoAviso
        );

        if (imagemSinalizador != null)
        {
            float opacidade = Mathf.Lerp(
                opacidadeInicial,
                opacidadeFinal,
                progresso
            );

            imagemSinalizador.color = new Color(
                0f, 0f, 0f, opacidade
            );
        }

        if (temporizadorAviso >= duracaoAviso)
            IniciarEmergencia();
    }

    private void IniciarEmergencia()
    {
        avisando = false;
        subindo = true;

        if (sinalizador != null)
            Destroy(sinalizador.gameObject);

        imagemInimigo.enabled = true;

        if (colisor != null)
            colisor.enabled = true;
    }

    private void AtualizarSubida()
    {
        float velocidade = atributos.ObterValor(
            atributoVelocidadeVertical
        );

        transform.position += Vector3.up *
            velocidade * Time.deltaTime;

        float distanciaCamera = Mathf.Abs(
            transform.position.z - cameraPrincipal.transform.position.z
        );

        float topoTela = cameraPrincipal.ViewportToWorldPoint(
            new Vector3(0.5f, alturaFinalViewport, distanciaCamera)
        ).y;

        if (transform.position.y >= topoTela)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!subindo)
            return;

        if (collision.CompareTag("Player"))
            Debug.Log("Jogador atingido pelo inimigo terrestre!");
    }
}
