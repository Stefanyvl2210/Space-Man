using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float runningSpeed = 1.5f;
    Rigidbody2D rigBody;
    public bool facingRight = false;
    private Vector3 startPosition;
    public int enemyDamage = 10;

    private void Awake()
    {
        rigBody = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    // Use this for initialization
    void Start () {
        transform.position = startPosition;
	}
	
	
	void FixedUpdate () {
        float currentRunningSpeed = runningSpeed;

        //mirando a la derecha
        if (facingRight)
        {
            currentRunningSpeed = runningSpeed;
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else //mirando a la izquierda
        {
            currentRunningSpeed = -runningSpeed;
            transform.eulerAngles = Vector3.zero;//vector 0,0,0
        }

        if(GameManager.sharedInstance.currentGameState == GameState.inGame)
        {//le damos el movimiento al enemigo
            rigBody.velocity = new Vector2(currentRunningSpeed, rigBody.velocity.y);
        }

	}

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.tag == "Coin")
        {
            return;
        }

        if(collision.tag == "Player")
        {
            GetComponent<AudioSource>().Play();
            collision.gameObject.GetComponent<PlayerController>().
                CollectHealth(-enemyDamage);
            return;
        }

        facingRight = !facingRight;
    }
}
