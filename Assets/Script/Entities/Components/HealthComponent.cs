
using UnityEngine;

[DisallowMultipleComponent]
public class HealthComponent : MonoBehaviour
{
    [Header("Atributo de vida máxima")]
    [SerializeField] private StatDefinition atributoVidaMaxima;

    private float vidaAtual;

    private Entity entidade;
    private StatContainer atributos;

    public float VidaMaxima =>
        atributos != null && atributoVidaMaxima != null
            ? atributos.ObterValor(atributoVidaMaxima)
            : 0f;

    public float VidaAtual => vidaAtual;

    public float PorcentagemVida =>
        VidaMaxima > 0f ? vidaAtual / VidaMaxima : 0f;

    private void Awake()
    {
        entidade = GetComponent<Entity>();
        atributos = GetComponent<StatContainer>();

        if (entidade == null || atributos == null ||
            atributoVidaMaxima == null)
        {
            Debug.LogError(
                "HealthComponent precisa de Entity, StatContainer " +
                "e de uma definição para VidaMaxima!",
                this
            );

            enabled = false;
            return;
        }

        vidaAtual = VidaMaxima;

        if (vidaAtual <= 0f)
        {
            Debug.LogError(
                "O atributo VidaMaxima precisa ter um valor maior que zero!",
                this
            );

            enabled = false;
        }
    }

    public bool ReceberDano(float quantidade)
    {
        if (!enabled || entidade == null ||
            !entidade.IsAlive || quantidade <= 0f)
        {
            return false;
        }

        vidaAtual = Mathf.Max(0f, vidaAtual - quantidade);

        if (vidaAtual <= 0f)
            entidade.Die();

        return true;
    }

    public bool RecuperarVida(float quantidade)
    {
        if (!enabled || entidade == null ||
            !entidade.IsAlive || quantidade <= 0f)
        {
            return false;
        }

        vidaAtual = Mathf.Min(
            VidaMaxima,
            vidaAtual + quantidade
        );

        return true;
    }
}
