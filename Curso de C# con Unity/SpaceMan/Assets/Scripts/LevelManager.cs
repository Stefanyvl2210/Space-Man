using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public static LevelManager sharedInstance;
    public List<LevelBlock> allTheLevelBlocks = new List<LevelBlock>();
    public List<LevelBlock> currentLevelBlocks = new List<LevelBlock>();

    public Transform levelStartPosition;

    void Awake()
    {
        if(sharedInstance == null)
        {
            sharedInstance = this;
        }
    }

    // Use this for initialization
    void Start () {
        GenerateInitialBlocks();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddLevelBlock()
    {
        //generamos un nivel
        int randomIdx = Random.Range(0, allTheLevelBlocks.Count);

        //bloque que añadiremos 
        LevelBlock block;

        //ubicamos el nivel en posicion 0,0,0 mientras calculamos la posicion inicial
        Vector3 spawnPosition = Vector3.zero;

        //si no se ha generado ningun nivel, se genera el nivel 0
        if(currentLevelBlocks.Count == 0)
        {
            block = Instantiate(allTheLevelBlocks[0]);
            spawnPosition = levelStartPosition.position;
        }
        else//si no se genera un nivel aleatorio
        {
            block = Instantiate(allTheLevelBlocks[randomIdx]);
            spawnPosition = currentLevelBlocks[currentLevelBlocks.Count - 1].exitPoint.position;
        }

        block.transform.SetParent(transform, false);

        //esto calcula la posicion del generado en la posicion inicial
        Vector3 correction = new Vector3(
            spawnPosition.x - block.startPoint.position.x,
            spawnPosition.y - block.startPoint.position.y,
            0);
        block.transform.position = correction; //le da la ubicacion
        currentLevelBlocks.Add(block);
    }

    public void RemoveLevelBlock()
    {
        LevelBlock oldBlock = currentLevelBlocks[0];
        currentLevelBlocks.Remove(oldBlock);
        Destroy(oldBlock.gameObject);
    }

    public void RemoveAllLevelBlocks()
    {
        while(currentLevelBlocks.Count > 0)
        {
            RemoveLevelBlock();
        }
    }

    public void GenerateInitialBlocks()
    {
        for (int i = 0; i < 2; i++)
        {
            AddLevelBlock();
        }
    }
}
