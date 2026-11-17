
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Atributo de movimentação")]
    [SerializeField] private StatDefinition atributoVelocidadeMovimento;

    private StatContainer atributos;
    private Camera cameraPrincipal;

    private void Awake()
    {
        atributos = GetComponent<StatContainer>();

        if (atributos == null || atributoVelocidadeMovimento == null)
        {
            Debug.LogError(
                "Enemy precisa de StatContainer e da definição VelocidadeMovimento!",
                this
            );

            enabled = false;
        }
    }

    private void Start()
    {
        cameraPrincipal = Camera.main;
    }

    public void DefinirVelocidade(float novaVelocidade)
    {
        if (atributos == null || atributoVelocidadeMovimento == null)
            return;

        atributos.DefinirValorBase(
            atributoVelocidadeMovimento,
            novaVelocidade
        );
    }

    private void Update()
    {
        float velocidade = atributos.ObterValor(
            atributoVelocidadeMovimento
        );

        transform.Translate(
            Vector2.left * velocidade * Time.deltaTime
        );

        if (cameraPrincipal != null)
        {
            Vector3 posicaoNaTela =
                cameraPrincipal.WorldToViewportPoint(transform.position);

            if (posicaoNaTela.x < -0.1f)
                Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            Debug.Log("Jogador atingido!");
    }
}
