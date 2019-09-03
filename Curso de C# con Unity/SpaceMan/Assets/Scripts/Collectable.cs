using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectableType
{
    healthPotion,
    manaPotion,
    money
}

public class Collectable : MonoBehaviour {

    public CollectableType type = CollectableType.money;

    //variable para buscar el jugador
    GameObject player;

    SpriteRenderer sprite; //para acceder a la imagen
    CircleCollider2D itemCollider; //para acceder al collider directamente

    bool hasBeenCollected = false; //se usa depende del item, si es mana sube un pto de mana y asi con los demas

    public int value = 1;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        itemCollider = GetComponent<CircleCollider2D>();

    }
    //para mostrar la moneda
    void Show()
    {
        sprite.enabled = true;
        itemCollider.enabled = true;
        hasBeenCollected = false;
    }
    //ocultar la moneda
    void Hide()
    {
        sprite.enabled = false;
        itemCollider.enabled = false;

    }
    //recoger la moneda
    void Collect()
    {
        Hide();
        hasBeenCollected = true;

        //type de tipo de collectable
        switch (type)
        {
            case CollectableType.money:
                GameManager.sharedInstance.CollectObject(this);
                GetComponent<AudioSource>().Play();
                break;
            case CollectableType.healthPotion:
                GetComponent<AudioSource>().Play();
                player.GetComponent<PlayerController>().CollectHealth(value);
                break;
            case CollectableType.manaPotion:
                GetComponent<AudioSource>().Play();
                player.GetComponent<PlayerController>().CollectMana(value);
                break;
        }
    }

    // Use this for initialization
    void Start () {
        player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Collect();
        }
    }
}
