
using UnityEngine;

[CreateAssetMenu(
    fileName = "NovaFamiliaInimigo",
    menuName = "Jogo/Inimigos/Familia de Inimigos"
)]
public class EnemyFamilyConfig : ScriptableObject
{
    [Header("Identificação")]
    public string nomeFamilia;

    [Header("Prefab")]
    public GameObject prefab;

    [Header("Velocidade por etapa")]
    public float[] velocidadesPorEtapa =
    {
        4f,
        4.5f,
        5f,
        5.5f
    };
}
