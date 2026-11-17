
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

        int indiceVelocidade =
            controladorJornada.ObterIndiceVelocidade(familia);

        if (indiceVelocidade < 0)
            return;

        float velocidadeAtual =
            familia.velocidadesPorEtapa[indiceVelocidade];

        float velocidadeHorizontal =
            ObterVelocidadeHorizontal(ciclo);

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

        GroundEnemy componenteTerrestre =
            novoInimigo.GetComponent<GroundEnemy>();

        if (componenteTerrestre != null)
        {
            componenteTerrestre.IniciarSubida(
                cameraPrincipal,
                velocidadeAtual
            );

            return;
        }

        AerialEnemy componenteAereo =
            novoInimigo.GetComponent<AerialEnemy>();

        if (componenteAereo != null)
        {
            componenteAereo.IniciarQueda(
                cameraPrincipal,
                velocidadeAtual,
                velocidadeHorizontal
            );

            return;
        }

        Enemy componenteInimigo =
            novoInimigo.GetComponent<Enemy>();

        if (componenteInimigo != null)
        {
            componenteInimigo.DefinirVelocidade(velocidadeAtual);
        }
    }

    private float ObterVelocidadeHorizontal(CycleConfig ciclo)
    {
        foreach (EnemyFamilyChance item in ciclo.familias)
        {
            if (item.familia == null ||
                item.familia.prefab == null)
                continue;

            Enemy componenteHorizontal =
                item.familia.prefab.GetComponent<Enemy>();

            if (componenteHorizontal == null)
                continue;

            int indice = controladorJornada.ObterIndiceVelocidade(
                item.familia
            );

            if (indice >= 0)
            {
                return item.familia.velocidadesPorEtapa[indice];
            }
        }

        Debug.LogError(
            "Não foi encontrada uma família horizontal no ciclo atual!"
        );

        return 0f;
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
