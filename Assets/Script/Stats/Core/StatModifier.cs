
using UnityEngine;

public class StatModifier
{
    public StatDefinition Atributo { get; }
    public StatModifierType Tipo { get; }
    public float Valor { get; }
    public UnityEngine.Object Origem { get; }
    public int Prioridade { get; }

    public StatModifier(
        StatDefinition atributo,
        StatModifierType tipo,
        float valor,
        UnityEngine.Object origem = null,
        int prioridade = 0)
    {
        Atributo = atributo;
        Tipo = tipo;
        Valor = valor;
        Origem = origem;
        Prioridade = prioridade;
    }
}
