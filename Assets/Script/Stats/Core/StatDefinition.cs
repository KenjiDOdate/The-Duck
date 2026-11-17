
using UnityEngine;

[CreateAssetMenu(
    fileName = "NovoAtributo",
    menuName = "Jogo/Atributos/Definição de Atributo"
)]
public class StatDefinition : ScriptableObject
{
    [Header("Identificação")]
    public string nomeAtributo;

    [TextArea]
    public string descricao;

    [Header("Valor padrão")]
    public float valorPadrao = 0f;

    [Header("Limites")]
    public bool limitarValor = false;
    public float valorMinimo = 0f;
    public float valorMaximo = 100f;

    public float AplicarLimites(float valor)
    {
        if (!limitarValor)
            return valor;

        return Mathf.Clamp(
            valor,
            valorMinimo,
            valorMaximo
        );
    }
}
