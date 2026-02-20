using UnityEngine;

public class Grounded : MonoBehaviour
{
    Character Player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     Player = gameObject.transform.parent.gameObject.GetComponent<Character>();  
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            Player.estapulando = false;
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Player.estapulando = true;
    }
}
