using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //atributos del player
    public float jumpForce = 6f;
    Rigidbody2D playerRig;
    Animator animator;

    int healthPoints, manaPoints;

    //valores que no van a cambiar
    public const int INITIAL_HEALTH = 100, INITIAL_MANA = 15,
        MAX_HEALTH = 200, MAX_MANA = 30,
        MIN_HEALTH = 10, MIN_MANA = 0;

    //estados de animacion
    const string STATE_ALIVE = "isAlive";
    const string STATE_IS_ON_THE_GROUND = "isOnTheGround";
    //velocidad del personaje
    public float runningSpeed = 4f;
    //LayerMask: capa que se encargara de tener un registro de quien forma parte del suelo
    public LayerMask groundMask;//para saber con que objetos podemos chocar en este caso todo lo que este con layer ground
    //posicion inicial del personaje
    Vector3 startPosition;
    //Para saber la direccion de la animacion del personaje
    bool facingRight = true;
    SpriteRenderer myRenderer;

    //variables usadas para el poder de super salto
    public const int SUPERJUMP_COST = 5;
    public const float SUPERJUMP_FORCE = 1.5f;

    void Awake()
    {
        playerRig = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        myRenderer = GetComponent<SpriteRenderer>();
    }

    // Use this for initialization
    void Start () {
        
        startPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		//salta al hacer click o presionar barra
        if(Input.GetButtonDown("Jump"))//0 es click izq, uno click der, 2 click rueda
        {
            Jump(false);
        }
        if (Input.GetButtonDown("SuperJump"))
        {
            Jump(true);
        }
   
        //cambia la animacion de acuerdo al estado de si esta tocando el suelo
        animator.SetBool(STATE_IS_ON_THE_GROUND, IsTouchingTheGround());

        //dibuja un trazo en el centro del personaje hacia el suelo
        Debug.DrawRay(transform.position, Vector2.down * 1.5f, Color.red);
	}

    //controla el movimiento del personaje (izq-der)
    void FixedUpdate()
    {
        if (GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            float move = Input.GetAxis("Horizontal");
            if(move > 0 && !facingRight)
            {
                Flip();
            }
            else if(move < 0 && facingRight)
            {
                Flip();
            }
            playerRig.velocity = new Vector2(move * runningSpeed, playerRig.velocity.y);

        }
    }

    //Gira la animacion del personaje
    void Flip()
    {
        facingRight = !facingRight;
        myRenderer.flipX = !myRenderer.flipX;
    }

    public void StartGame()
    {
        animator.SetBool(STATE_ALIVE, true);
        animator.SetBool(STATE_IS_ON_THE_GROUND, true);

        healthPoints = INITIAL_HEALTH;
        manaPoints = INITIAL_MANA;

        Invoke("ResetPosition", 0.4f);
        
    }

    void ResetPosition()
    {
        transform.position = startPosition;
        playerRig.velocity = Vector2.zero;
        GameObject mainCamera = GameObject.Find("Main Camera");
        //llama al metodo para que no haya barridos en la camara al morir
        mainCamera.GetComponent<CameraFollow>().ResetCameraPosition();
    }

    //da velocidad de movimiento al personaje en x
    /*void FixedUpdate()//actualiza los frameworks mas fluidos, sin lag, en el tiempo que es
    {
        if (GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            if (playerRig.velocity.x < runningSpeed)
            {
                playerRig.velocity = new Vector2(runningSpeed, playerRig.velocity.y);
            }
        }else{//si no estamos dentro de la partida
        
            playerRig.velocity = new Vector2(0, playerRig.velocity.y);
        }
    }*/

    void Jump(bool superJump)
    {
        float jumpForceFactor = jumpForce;
        if (superJump && manaPoints >= SUPERJUMP_COST)
        {
            manaPoints -= SUPERJUMP_COST;
            jumpForceFactor *= SUPERJUMP_FORCE;
        }
        if (GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            if (IsTouchingTheGround())
            {
                GetComponent<AudioSource>().Play();
                playerRig.AddForce(Vector2.up * jumpForceFactor, ForceMode2D.Impulse);// modo de fuerza impulse un golpe y se deja llevar por la gravedad

            }
        }
    }

    //indica si esta en el suelo o no
    bool IsTouchingTheGround()
    {
        //transform indica posicion, rotacion y escala, raycast traza rayo invisible con vector2.down traza hacia abajo, ultimo parametro: contra mascara del suelo
        if (Physics2D.Raycast(transform.position, Vector2.down, 1.5f, groundMask))
        {
            animator.speed = 1.7f;
            //GameManager.sharedInstance.currentGameState = GameState.inGame;//cambia el estado del controlador a modo juego

            return true;
        }
        else//si no toca el suelo
        {
            animator.speed = 2.6f;
    
            return false;
        }
    }

    //cambiamos la animacion del personaje, llamamos al GameOver de GameManager
    //guardamos el maxscore, para eso se usa el PlayerPrefs
    public void Die()
    {
        float travelledDistance = GetTravelDistance();
        //playerPrefes almacena preferencias del jugador entre partidas
        float previousMaxDistance = PlayerPrefs.GetFloat("maxscore", 0f);
        if(travelledDistance > previousMaxDistance)
        {
            PlayerPrefs.SetFloat("maxscore", travelledDistance);
        }
        //GetComponent<AudioSource>().Play();
        animator.SetBool(STATE_ALIVE, false);
        GameManager.sharedInstance.GameOver();
    }

    public void CollectHealth(int points)
    {
        healthPoints += points;
        if(healthPoints >= MAX_HEALTH)
        {
            healthPoints = MAX_HEALTH;
        }

        if(healthPoints <= 0)
        {
            Die();
        }
    }
    public void CollectMana(int points)
    {
        manaPoints += points;
        if(manaPoints >= MAX_MANA)
        {
            manaPoints = MAX_MANA;
        }
    }

    public int GetHealth()
    {
        return healthPoints;
    }
    public int GetMana()
    {
        return manaPoints;
    }

    //regresa el total de espacio recorrido entre el eje de las x
    //se usa para calcular el score del player
    public float GetTravelDistance()
    {
        return transform.position.x - startPosition.x;
    }
}
