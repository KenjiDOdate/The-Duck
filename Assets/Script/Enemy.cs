using UnityEngine;
using UnityEngine.InputSystem;

public class Enemy : MonoBehaviour
{

    private float velocidade = 2f;

    public Rigidbody2D rig;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Mover();
    }

    void Mover()
    {
        float eixo = 0f;

            eixo = -1f;

        rig.linearVelocity = new Vector2(eixo * velocidade, rig.linearVelocity.y);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Character>().ReceberDano(1);
        }
    }
}
