using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This script provides functionalities for each block.*/

public class blockScript : MonoBehaviour
{
	//this.gameObject can be used to reffer to the gameObject attached to this script.
	public Rigidbody2D rigidBodyComponent;

	gameScript gameScriptObject;
	platformScript platformScriptObject;
	windDirectionTextScript windDirectionTextScriptObject;
    // Start is called before the first frame update
	bool blockDropped;
	bool overLapping;
	enum windDirection {Left = -1, Right = 1};
	
	static windDirection blockWindDirection;
	public static int windToBeApplied = 0;
    void Start()
    {
		//this function will be called each time a block is instaciated -> in blockRespawnerScript
		this.rigidBodyComponent = this.gameObject.GetComponent<Rigidbody2D>();
		this.rigidBodyComponent.isKinematic = true;
		this.gameScriptObject = GameObject.Find("game").GetComponent<gameScript>();
		this.platformScriptObject = GameObject.Find("platform").GetComponent<platformScript>();
		this.windDirectionTextScriptObject = GameObject.Find("WindDirectionText").GetComponent<windDirectionTextScript>();
		this.blockDropped = false;
		this.overLapping = false;
		this.calculateWindDirection(); // this will calculate the wind to be applied to the block
    }

    // Update is called once per frame
    void Update()
    {
		if(gameScriptObject.getGameState() == gameScript.gameStates.Running)
		{
			this.autoDestroy();
		}
    }

	public bool isBlockDropped()
	{
		return this.blockDropped;
	}
	//function not used
	public void getPositionRelativeToScene()
	{
		Debug.Log(transform.InverseTransformDirection(this.gameObject.transform.position - GameObject.Find("Main Camera").transform.position));
	}
	public void DropBlock()
	{
		if(Input.GetKeyDown(KeyCode.Space) && this.blockDropped == false)
		{
			//Once the animation gets stopped, the gravity of rigidBody will make the block fall.
			this.rigidBodyComponent.isKinematic = false;
			this.blockDropped = true;
			this.rigidBodyComponent.velocity = new Vector2(this.rigidBodyComponent.velocity.x + (windToBeApplied *(int)blockWindDirection), gameScript.getGravity()); //Applying velocity on Y Axis.
			this.gameObject.transform.parent = null;
		}
	}
	public void calculateWindDirection()
	{
		if (gameScript.maxWindAppliedToFallingBlock > 0 && windToBeApplied == 0)
		{
			int randomNum = Random.Range(0,2); // generate a random number between 0 and 1
			//Keeping it readable
			if (randomNum == 0)
			{
				blockWindDirection = windDirection.Left;
			}
			else
			{
				blockWindDirection = windDirection.Right;
			}
			windToBeApplied = Random.Range(0, gameScript.maxWindAppliedToFallingBlock+1);
			string direction = blockWindDirection == windDirection.Left ? "left" : "right";
			this.windDirectionTextScriptObject.updateWindDirection(direction, windToBeApplied);
		}
		else if (gameScript.maxWindAppliedToFallingBlock == 0)
		{
			blockWindDirection = windDirection.Right; // windDirection.Right is 1 so it will not affect the computation in case there is no wind applied.
			this.windDirectionTextScriptObject.updateWindDirection("", windToBeApplied);
		}
	}
	void autoDestroy()
	{

		if (this.gameObject.transform.position.y < gameScript.blockObjectOutOfScreenPosY)
		{
			this.destroyGameObject();
		}
	}
	public bool getOverlapping()
	{
		return this.overLapping;
	}
	//Function called automatically when there is a collision
	void OnCollisionEnter2D(Collision2D other) //function triggered on 2d collision
	{
		if(this.gameObject.transform.position.y > other.gameObject.transform.position.y + other.gameObject.transform.localScale.y)
		{
			this.overLapping = true; //this variable will be used as reference point for respawning a new block
		}
	}
	//Function not used
	void OnTriggerEnter2D(Collider2D other)
	{
		//Debug.Log(other);
	}
	public void destroyGameObject()
	{
		Destroy(this.gameObject);
	}
}
