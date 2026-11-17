
using System.Collections.Generic;

public static class StatCalculator
{
    public static float Calcular(
        float valorBase,
        List<StatModifier> modificadores,
        StatDefinition atributo)
    {
        float resultado = valorBase;
        float somaPercentual = 0f;
        float multiplicador = 1f;

        StatModifier substituicao = null;

        foreach (StatModifier modificador in modificadores)
        {
            if (modificador.Atributo != atributo)
                continue;

            switch (modificador.Tipo)
            {
                case StatModifierType.Flat:
                    resultado += modificador.Valor;
                    break;

                case StatModifierType.PercentAdd:
                    somaPercentual += modificador.Valor;
                    break;

                case StatModifierType.PercentMultiply:
                    multiplicador *= 1f + modificador.Valor;
                    break;

                case StatModifierType.Override:
                    if (substituicao == null ||
                        modificador.Prioridade >
                        substituicao.Prioridade)
                    {
                        substituicao = modificador;
                    }
                    break;
            }
        }

        resultado *= 1f + somaPercentual;
        resultado *= multiplicador;

        if (substituicao != null)
            resultado = substituicao.Valor;

        return atributo.AplicarLimites(resultado);
    }
}
