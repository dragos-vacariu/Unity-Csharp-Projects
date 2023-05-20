using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This script describes the behaviour of the platform*/

public class platformScript : MonoBehaviour
{
    // Start is called before the first frame update
	Vector3 platformPositionDefaults;
	
	public uint platformNumberOfElements;
	enum platformBalanceDirection {None, Left, Right};
	platformBalanceDirection platformBalancing;
	float balanceSpeed;
	float balanceOffset;
	float platformStartingXPos;
	uint blocksInAltitude;
	bool platformBoxColliderEnabled;
		
	altitudeTextScript altitudeLevelScriptComponent;
	gameScript gameScriptObject;
	Transform blockOnTopTransform;

    void Start()
    {
		this.platformPositionDefaults = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
		this.platformNumberOfElements = this.countPlatformElements();
		this.platformBalancing = platformBalanceDirection.None;
		this.addPlatformBalanceDirection();
		this.balanceSpeed = 0.00f;
		this.balanceOffset = 2;
		this.platformStartingXPos = this.gameObject.transform.position.x;
		this.blocksInAltitude = 0;
		this.altitudeLevelScriptComponent = GameObject.Find("AltitudeText").GetComponent<altitudeTextScript>();
		this.gameScriptObject = GameObject.Find("game").GetComponent<gameScript>();
		this.blockOnTopTransform = null;
		this.platformBoxColliderEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {
		
    }
	//MonoBehaviour.FixedUpdate is frame-rate independent - used for physics calculations.
	void FixedUpdate()
	{
		if(gameScriptObject.getGameState() == gameScript.gameStates.Running)
		{
			this.balancePlatform();
			this.autoDisableBoxCollider();
		}
	}
	public void resetPlatform()
	{
		//remove all elements on the platform;
		for(int i = 0; i < this.gameObject.transform.childCount ; i++) //this.gameObject.transform.childCount-1 is the last element
		{
			//remove all elements on the platform;
			if (this.gameObject.transform.GetChild(i).gameObject.name.Contains("block"))
			{
				Destroy(this.gameObject.transform.GetChild(i).gameObject);
			}
		}
		
		//resetting platform position;
		this.gameObject.transform.position = new Vector3 (this.platformPositionDefaults.x, this.platformPositionDefaults.y, this.platformPositionDefaults.z);
		this.enableBoxCollider();
		//reinitialize everything else;
		this.platformNumberOfElements = this.countPlatformElements();
		this.platformBalancing = platformBalanceDirection.None;
		this.addPlatformBalanceDirection();
		this.balanceSpeed = 0.00f;
		this.balanceOffset = 2;
		this.platformStartingXPos = this.gameObject.transform.position.x;
		this.blocksInAltitude = 0;
		this.altitudeLevelScriptComponent = GameObject.Find("AltitudeText").GetComponent<altitudeTextScript>();
		this.gameScriptObject = GameObject.Find("game").GetComponent<gameScript>();
		this.blockOnTopTransform = null;
	}
	public void autoDisableBoxCollider()
	{
		//will disable box collider when this gets out of the screen. Will not affect the blocks on screen.
		if(this.gameObject.transform.position.y < gameScript.blockObjectOutOfScreenPosY && this.platformBoxColliderEnabled)
		{
			GameObject.Find("platform_brick1").GetComponent<BoxCollider2D>().enabled = false;
			GameObject.Find("platform_brick2").GetComponent<BoxCollider2D>().enabled = false;
			this.platformBoxColliderEnabled = false;
		}
	}
	public void enableBoxCollider()
	{
		if (this.platformBoxColliderEnabled == false && this.gameObject.transform.position.y > gameScript.blockObjectOutOfScreenPosY )
		{
			//will enable box collider after resetting the game.
			GameObject.Find("platform_brick1").GetComponent<BoxCollider2D>().enabled = true;
			GameObject.Find("platform_brick2").GetComponent<BoxCollider2D>().enabled = true;
			this.platformBoxColliderEnabled = true;
		}
	}
	//Not used used -> replaced by autoDisableBoxCollider
	public void autoDestroyPlatform()
	{
		if(this.gameObject.transform.position.y < gameScript.blockObjectOutOfScreenPosY)
		{
			Destroy(GameObject.Find("platform_brick1"));
			Destroy(GameObject.Find("platform_brick2"));
		}
	}
	uint countPlatformElements() //function that will count the elements that construct the platform. Blocks dropped on the platform will not be counted.
	{
		uint counter = 0;
		if(this.gameObject.transform.childCount > 0)
		{
			for (int i =0; i < this.gameObject.transform.childCount; i++)
			{
				string gameObjectName = this.gameObject.transform.GetChild(i).gameObject.name;
				if (gameObjectName.Contains("platform"))
				{
					counter += 1;
				}

			}
		}
		return counter;
	}
	GameObject getPreviousBlockOnPlatform()
	{
		for(int i = this.gameObject.transform.childCount-2; i>=0 ; i--) //this.gameObject.transform.childCount-1 is the last element
		{
			if (this.gameObject.transform.GetChild(i).gameObject.name.Contains("block"))
			{
				return this.gameObject.transform.GetChild(i).gameObject;
			}
		}
		return null;
	}
	GameObject getLastBlockOnPlatform()
	{
		for(int i = this.gameObject.transform.childCount-1; i>=0 ; i--) //this.gameObject.transform.childCount-1 is the last element
		{
			if (this.gameObject.transform.GetChild(i).gameObject.name.Contains("block"))
			{
				return this.gameObject.transform.GetChild(i).gameObject;
			}
		}
		return null;
	}
	public uint getNumberOfBlocksInAltitude()
	{
		return this.blocksInAltitude;
	}
	public Transform getBlockOnTopTransform()
	{
		return this.blockOnTopTransform;
	}
	public void checkIfLastElementIncreasedTheAltitude()
	{
		GameObject lastElement = this.gameObject.transform.GetChild(this.gameObject.transform.childCount-1).gameObject;
		this.blockOnTopTransform = this.gameObject.transform.GetChild(this.gameObject.transform.childCount-1).gameObject.transform;
		bool lastElementContributedToAltitude = true;
		for (int i = this.gameObject.transform.childCount-2; i >= 0 ; i--) // last element will not be iterated
		{
			if (this.gameObject.transform.GetChild(i).gameObject.transform.name != "platform_text") // ignore platform_text since it's role is to display text always on top of last
			{
				if(lastElement.transform.position.y <= this.gameObject.transform.GetChild(i).gameObject.transform.position.y)
				{
					lastElementContributedToAltitude = false;
				}
				if(this.blockOnTopTransform.position.y < this.gameObject.transform.GetChild(i).gameObject.transform.position.y)
				{
					this.blockOnTopTransform = this.gameObject.transform.GetChild(i).gameObject.transform;
				}
			}
		}
		if (lastElementContributedToAltitude)
		{
			this.blocksInAltitude++;
			this.altitudeLevelScriptComponent.setAltitudeLevel(this.blocksInAltitude);
			if (this.blocksInAltitude % 10 == 0)
			{
				this.increaseBalanceSpeed(this.blocksInAltitude);
			}
			if(this.blocksInAltitude % 5 == 0)
			{
				blockScript.windToBeApplied = 0; // wind will be recalculated by blockScript
			}
		}
	}
	void addPlatformBalanceDirection()
	{
		int randomNum = Random.Range(0,2);  // generate a random number between 0 and 1
		//int randomNum = rndGen.Next(2);
		this.platformBalancing = randomNum == 1 ? platformBalanceDirection.Left : platformBalanceDirection.Right;
	}
	void balancePlatform()
	{
		if( platformBalancing == platformBalanceDirection.Right)
		{
			this.balancePlatformRight();
		}
		else if(platformBalancing == platformBalanceDirection.Left)
		{
			this.balancePlatformLeft();
		}
	}
	void balancePlatformRight()
	{
		if (this.gameObject.transform.position.x < (platformStartingXPos + this.balanceOffset) )
		{
			this.gameObject.transform.position = new Vector3 (this.gameObject.transform.position.x + this.balanceSpeed, this.gameObject.transform.position.y,
																					this.gameObject.transform.position.z);
		}
		else
		{
			platformBalancing = platformBalanceDirection.Left;
		}
	}
	void balancePlatformLeft()
	{
		if (this.gameObject.transform.position.x > (platformStartingXPos - this.balanceOffset) )
		{
			this.gameObject.transform.position = new Vector3 (this.gameObject.transform.position.x - this.balanceSpeed, this.gameObject.transform.position.y,
																		this.gameObject.transform.position.z);
		}
		else
		{
			platformBalancing = platformBalanceDirection.Right;
		}
	}
	public void increaseBalanceSpeed(uint altitudeLevel)
	{

		this.balanceSpeed = altitudeLevel * 0.001f;
	}
}
