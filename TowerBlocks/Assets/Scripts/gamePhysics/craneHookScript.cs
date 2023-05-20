using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This script describes the behaviour of craneHook*/

public class craneHookScript : MonoBehaviour
{
	blockSpawnerScript blockSpawnerObject;
	public blockScript blockScriptObject;
	platformScript platformScriptObject;
	gameScript gameScriptObject;
	const float hookPosYoffset = 1.6f;
    // Start is called before the first frame update
    void Start()
    {
        this.blockSpawnerObject = GameObject.Find("block_spawner").GetComponent<blockSpawnerScript>();
        this.gameScriptObject = GameObject.Find("game").GetComponent<gameScript>();

		this.platformScriptObject = GameObject.Find("platform").GetComponent<platformScript>();
		

    }

    // Update is called once per frame
    void Update()
    {
		if(gameScriptObject.getGameState() == gameScript.gameStates.Running)
		{
			if (this.blockScriptObject.gameObject.transform.position.y < gameScript.blockObjectOutOfScreenPosY && this.blockScriptObject.rigidBodyComponent.velocity.magnitude > 0
				|| this.blockScriptObject.gameObject.transform.parent == this.platformScriptObject.gameObject.transform)
			{
				//if entered here it means the block missed the platform and got out of the screen or it landed on the platform.
				
				//block gameobject will be auto-destroyed, just respawn a new one.
				this.blockScriptObject = this.blockSpawnerObject.SpawnBlock();
				this.blockScriptObject.gameObject.transform.parent = this.gameObject.transform;
				this.placeBlockIntoHook();
			}
		}
    }
	void placeBlockIntoHook()
	{
		this.blockScriptObject.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, 
																	this.gameObject.transform.position.y - hookPosYoffset, this.gameObject.transform.position.z);
	}
	public void prepareCraneHook()
	{
		if (this.gameObject.transform.childCount > 0)
		{
			this.blockScriptObject = this.gameObject.transform.GetChild(0).gameObject.GetComponent<blockScript>();
		}
		else
		{
			this.blockScriptObject = this.blockSpawnerObject.SpawnBlock();
			this.blockScriptObject.gameObject.transform.parent = this.gameObject.transform;
			this.placeBlockIntoHook();
		}
	}
}
