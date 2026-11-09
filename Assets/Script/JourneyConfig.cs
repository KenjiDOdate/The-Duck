
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "NovaJornada",
    menuName = "Jogo/Jornada/Configuração da Jornada"
)]
public class JourneyConfig : ScriptableObject
{
    [Header("Ciclos da jornada")]
    public List<CycleConfig> ciclos = new List<CycleConfig>();
}
