
using UnityEngine;

public class StageController : MonoBehaviour
{
    [Header("Configuração da jornada")]
    [SerializeField] private JourneyConfig jornada;

    private int indiceCicloAtual = 0;
    private int indiceEtapaAtual = 0;

    private float tempoNaEtapa = 0f;

    private bool jornadaIniciada = false;
    private bool jornadaConcluida = false;

    public int CicloAtual => indiceCicloAtual + 1;
    public int EtapaAtual => indiceEtapaAtual + 1;

    public bool JornadaConcluida => jornadaConcluida;

    public CycleConfig ConfiguracaoCicloAtual
    {
        get
        {
            if (jornada == null ||
                indiceCicloAtual >= jornada.ciclos.Count)
                return null;

            return jornada.ciclos[indiceCicloAtual];
        }
    }

    public float TempoRestante
    {
        get
        {
            CycleConfig ciclo = ConfiguracaoCicloAtual;

            if (ciclo == null ||
                indiceEtapaAtual >= ciclo.duracoesEtapas.Length)
                return 0f;

            return Mathf.Max(
                0f,
                ciclo.duracoesEtapas[indiceEtapaAtual] - tempoNaEtapa
            );
        }
    }

    private void Start()
    {
        IniciarJornada();
    }

    private void Update()
    {
        if (!jornadaIniciada || jornadaConcluida)
            return;

        tempoNaEtapa += Time.deltaTime;

        CycleConfig ciclo = ConfiguracaoCicloAtual;

        if (tempoNaEtapa >= ciclo.duracoesEtapas[indiceEtapaAtual])
        {
            AvancarEtapa();
        }
    }

    private void IniciarJornada()
    {
        if (!ValidarJornada())
            return;

        indiceCicloAtual = 0;
        indiceEtapaAtual = 0;
        tempoNaEtapa = 0f;

        jornadaConcluida = false;
        jornadaIniciada = true;

        MostrarEtapaAtual();
    }

    private bool ValidarJornada()
    {
        if (jornada == null || jornada.ciclos == null ||
            jornada.ciclos.Count == 0)
        {
            Debug.LogError("Nenhuma jornada foi configurada!");
            return false;
        }

        for (int i = 0; i < jornada.ciclos.Count; i++)
        {
            CycleConfig ciclo = jornada.ciclos[i];

            if (ciclo == null ||
                ciclo.duracoesEtapas == null ||
                ciclo.duracoesEtapas.Length == 0)
            {
                Debug.LogError(
                    "O ciclo " + (i + 1) +
                    " não possui etapas configuradas!"
                );

                return false;
            }

            for (int j = 0; j < ciclo.duracoesEtapas.Length; j++)
            {
                if (ciclo.duracoesEtapas[j] <= 0f)
                {
                    Debug.LogError(
                        "A duração da etapa " + (j + 1) +
                        " do ciclo " + (i + 1) +
                        " precisa ser maior que zero!"
                    );

                    return false;
                }
            }
        }

        return true;
    }

    private void AvancarEtapa()
    {
        tempoNaEtapa = 0f;

        CycleConfig ciclo = ConfiguracaoCicloAtual;

        if (indiceEtapaAtual < ciclo.duracoesEtapas.Length - 1)
        {
            indiceEtapaAtual++;
        }
        else
        {
            indiceCicloAtual++;
            indiceEtapaAtual = 0;

            if (indiceCicloAtual >= jornada.ciclos.Count)
            {
                FinalizarJornada();
                return;
            }
        }

        MostrarEtapaAtual();
    }

    private void MostrarEtapaAtual()
    {
        Debug.Log(
            "Ciclo " + CicloAtual +
            " | Etapa " + EtapaAtual +
            " | Duração: " +
            ConfiguracaoCicloAtual.duracoesEtapas[indiceEtapaAtual] +
            " segundos"
        );
    }

    private void FinalizarJornada()
    {
        jornadaIniciada = false;
        jornadaConcluida = true;

        Debug.Log("Todos os ciclos da campanha foram concluídos!");
    }

    public void ReiniciarJornada()
    {
        IniciarJornada();
    }

    public int ObterIndiceVelocidade(EnemyFamilyConfig familia)
    {
        if (familia == null ||
            familia.velocidadesPorEtapa == null ||
            familia.velocidadesPorEtapa.Length == 0)
        {
            return -1;
        }

        int cicloIntroducao = -1;

        for (int i = 0; i < jornada.ciclos.Count; i++)
        {
            CycleConfig ciclo = jornada.ciclos[i];

            foreach (EnemyFamilyChance item in ciclo.familias)
            {
                if (item != null && item.familia == familia)
                {
                    cicloIntroducao = i;
                    break;
                }
            }

            if (cicloIntroducao != -1)
                break;
        }

        if (cicloIntroducao == -1)
            return -1;

        if (indiceCicloAtual > cicloIntroducao)
        {
            return familia.velocidadesPorEtapa.Length - 1;
        }

        if (indiceCicloAtual == cicloIntroducao)
        {
            return Mathf.Min(
                indiceEtapaAtual,
                familia.velocidadesPorEtapa.Length - 1
            );
        }

        return -1;
    }
}
