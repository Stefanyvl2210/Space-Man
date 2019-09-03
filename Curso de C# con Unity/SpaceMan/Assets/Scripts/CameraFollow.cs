using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    //identifica a quien debe seguir la camara
    public Transform target;

    //ubica la posicion de la camara (que tan centrado)
    public Vector3 offset = new Vector3(0f, 0.0f, -10f);

    //seguimiento de la camara, tardará un poco en acompañar el movimiento del target
    public float dampingTime = 0.3f;

    //velocidad a la que tiene que ir la camara
    public Vector3 velocity = Vector3.zero;

    void Awake()
    {
        //va a intentar seguir este frame
        Application.targetFrameRate = 60;

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        MoveCamera(true);
	}

    //para que la camara vuelva a la posicion del personaje cuando éste muera
    public void ResetCameraPosition()
    {
        //hara el movimiento instantaneo, sin el suavizado (smooth)
        MoveCamera(false);
    }

    //el smooth hara un movimiento suave con el movimiento de la camara
    void MoveCamera(bool smooth)
    {
        //destino al que tiene que seguir la camara, seguir al target
        Vector3 destination = new Vector3(
            target.position.x - offset.x,
            offset.y,
            offset.z
            );
        if (smooth) //se usa el barrido suavizado de la camara siguiendo al target
        {
            transform.position = Vector3.SmoothDamp(
                transform.position,
                destination,
                ref velocity, //le pasamos por referencia esto para que unity me haga los calculos y los devuelva al script
                dampingTime //le da una sensacion cinematica
                );
        }
        else// va a la posicion del target sin tomar en cuenta ningun barrido
        {
            transform.position = destination;
        }
    }
}
