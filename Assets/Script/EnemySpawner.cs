
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Controlador da jornada")]
    [SerializeField] private StageController controladorJornada;

    [Header("Espera inicial")]
    [SerializeField] private float esperaInicial = 4f;
    [SerializeField] private float esperaAposReiniciar = 3f;

    [Header("Intervalo entre inimigos")]
    [SerializeField] private float intervaloMinimo = 2f;
    [SerializeField] private float intervaloMaximo = 4f;

    [Header("Posicionamento")]
    [SerializeField] private float alturaSpawn = -3.7f;

    private float temporizador;
    private float proximoIntervalo;

    private Camera cameraPrincipal;

    private void Start()
    {
        cameraPrincipal = Camera.main;
        proximoIntervalo = esperaInicial;
    }

    private void Update()
    {
        if (controladorJornada == null ||
            controladorJornada.JornadaConcluida)
            return;

        temporizador += Time.deltaTime;

        if (temporizador >= proximoIntervalo)
        {
            CriarInimigo();

            temporizador = 0f;

            proximoIntervalo = Random.Range(
                intervaloMinimo,
                intervaloMaximo
            );
        }
    }

    private void CriarInimigo()
    {
        if (cameraPrincipal == null ||
            controladorJornada == null)
            return;

        CycleConfig ciclo =
            controladorJornada.ConfiguracaoCicloAtual;

        if (ciclo == null || ciclo.familias == null)
            return;

        EnemyFamilyConfig familia = SortearFamilia(ciclo);

        if (familia == null || familia.prefab == null)
            return;

        if (familia.velocidadesPorEtapa == null ||
            familia.velocidadesPorEtapa.Length == 0)
            return;

        int indiceVelocidade =
            controladorJornada.EtapaAtual - 1;

        if (indiceVelocidade >= familia.velocidadesPorEtapa.Length)
            return;

        float velocidadeAtual =
            familia.velocidadesPorEtapa[indiceVelocidade];

        Vector3 posicaoSpawn = cameraPrincipal.ViewportToWorldPoint(
            new Vector3(1.1f, 0f, 10f)
        );

        posicaoSpawn.y = alturaSpawn;
        posicaoSpawn.z = 0f;

        GameObject novoInimigo = Instantiate(
            familia.prefab,
            posicaoSpawn,
            Quaternion.identity
        );

        Enemy componenteInimigo =
            novoInimigo.GetComponent<Enemy>();

        if (componenteInimigo != null)
        {
            componenteInimigo.DefinirVelocidade(velocidadeAtual);
        }
    }

    private EnemyFamilyConfig SortearFamilia(CycleConfig ciclo)
    {
        float totalPorcentagens = 0f;

        foreach (EnemyFamilyChance item in ciclo.familias)
        {
            if (item.familia != null &&
                item.familia.prefab != null &&
                item.porcentagem > 0f)
            {
                totalPorcentagens += item.porcentagem;
            }
        }

        if (totalPorcentagens <= 0f)
            return null;

        float sorteio = Random.Range(0f, totalPorcentagens);
        float acumulado = 0f;

        foreach (EnemyFamilyChance item in ciclo.familias)
        {
            if (item.familia == null ||
                item.familia.prefab == null ||
                item.porcentagem <= 0f)
                continue;

            acumulado += item.porcentagem;

            if (sorteio < acumulado)
            {
                return item.familia;
            }
        }

        return null;
    }
}
