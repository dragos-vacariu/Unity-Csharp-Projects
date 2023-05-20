using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This script will describe the respawning of the Blocks.*/

public class blockSpawnerScript : MonoBehaviour
{
	public GameObject spawnPrefabObject;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public blockScript SpawnBlock()
	{
		GameObject blockObject = Instantiate(spawnPrefabObject);
		blockObject.GetComponent<Rigidbody2D>().isKinematic = true;
		blockObject.transform.position = transform.position;
		return blockObject.GetComponent<blockScript>();
	}
}
