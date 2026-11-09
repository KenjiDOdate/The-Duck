
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Movimentação")]
    [SerializeField] private float velocidade = 4f;

    private Camera cameraPrincipal;

    private void Start()
    {
        cameraPrincipal = Camera.main;
    }

    public void DefinirVelocidade(float novaVelocidade)
    {
        velocidade = novaVelocidade;
    }

    private void Update()
    {
        transform.Translate(Vector2.left * velocidade * Time.deltaTime);

        if (cameraPrincipal != null)
        {
            Vector3 posicaoNaTela =
                cameraPrincipal.WorldToViewportPoint(transform.position);

            if (posicaoNaTela.x < -0.1f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Jogador atingido!");
        }
    }
}
