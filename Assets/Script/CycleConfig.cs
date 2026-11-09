
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyFamilyChance
{
    [Header("Família de inimigos")]
    public EnemyFamilyConfig familia;

    [Header("Chance de aparecer (%)")]
    [Range(0f, 100f)]
    public float porcentagem;
}

[CreateAssetMenu(
    fileName = "NovoCiclo",
    menuName = "Jogo/Jornada/Ciclo"
)]
public class CycleConfig : ScriptableObject
{
    [Header("Identificação")]
    public string nomeCiclo;

    [Header("Distribuição das famílias")]
    public List<EnemyFamilyChance> familias =
        new List<EnemyFamilyChance>();

    [Header("Duração das etapas (segundos)")]
    [Min(1f)]
    public float[] duracoesEtapas =
    {
        60f,
        60f,
        60f,
        60f
    };
}
