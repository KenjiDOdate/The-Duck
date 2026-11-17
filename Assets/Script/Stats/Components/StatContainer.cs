
using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class StatContainer : MonoBehaviour
{
    [Serializable]
    private class StatEntry
    {
        public StatDefinition atributo;
        public float valorBase;
    }

    [Header("Atributos base")]
    [SerializeField] private List<StatEntry> atributosBase =
        new List<StatEntry>();

    private readonly List<StatModifier> modificadores =
        new List<StatModifier>();

    public float ObterValorBase(StatDefinition atributo)
    {
        if (atributo == null)
            return 0f;

        foreach (StatEntry entrada in atributosBase)
        {
            if (entrada.atributo == atributo)
                return entrada.valorBase;
        }

        return atributo.valorPadrao;
    }

    public float ObterValor(StatDefinition atributo)
    {
        if (atributo == null)
            return 0f;

        return StatCalculator.Calcular(
            ObterValorBase(atributo),
            modificadores,
            atributo
        );
    }

    public void DefinirValorBase(
        StatDefinition atributo,
        float novoValor)
    {
        if (atributo == null)
            return;

        foreach (StatEntry entrada in atributosBase)
        {
            if (entrada.atributo == atributo)
            {
                entrada.valorBase = novoValor;
                return;
            }
        }

        atributosBase.Add(new StatEntry
        {
            atributo = atributo,
            valorBase = novoValor
        });
    }

    public void AdicionarModificador(StatModifier modificador)
    {
        if (modificador == null ||
            modificador.Atributo == null)
        {
            return;
        }

        modificadores.Add(modificador);
    }

    public void RemoverModificador(StatModifier modificador)
    {
        modificadores.Remove(modificador);
    }

    public void RemoverModificadoresDaOrigem(
        UnityEngine.Object origem)
    {
        if (origem == null)
            return;

        modificadores.RemoveAll(
            modificador => modificador.Origem == origem
        );
    }

    public void LimparModificadores()
    {
        modificadores.Clear();
    }
}
