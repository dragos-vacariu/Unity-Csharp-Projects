using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/*This script describes the rules and the gameplay.*/

public class gameScript : MonoBehaviour
{
	platformScript platformScriptObject;
	craneHookScript craneHookScriptObject;
	scoreTextScript scoreScriptObject;
	StartGameButtonScript StartGameButtonScriptObject;
	HighScoreButtonScript HighScoreButtonScriptObject;
	OptionsButtonScript	OptionsButtonScriptObject;
	SpareBlocksTextScript SpareBlocksTextScriptObject;
	platformTextScript platformTextScriptObject;
	OptionsGravitySliderScript OptionsGravitySliderScriptObject;
	OptionsWindAppliedSliderScript OptionsWindAppliedSliderScriptObject;
	
	SpriteRenderer gameOverSpriteRenderer;
	Canvas CanvasGameOver;
	TMPro.TMP_Text gameOverMessageTextComponent;
	
	Vector3 gameBackgroundPositionDefaults;
	Vector3 craneBodyPositionDefaults;
	Vector3 craneArmPositionDefaults;
	Vector3 craneHookPositionDefaults;
	
	const int defaultGravityAppliedToFalingBlock = -2;
	const int defaultMaxWindAppliedToFalingBlock = 0;
	static float gravityAppliedToFallingBlock = defaultGravityAppliedToFalingBlock;
	public  static int maxWindAppliedToFallingBlock = defaultMaxWindAppliedToFalingBlock;
	
	public enum gameStates {NotStarted, Running, Paused, GameOver};
	public enum gameNavigation {MainMenu, Options, HighScore, Game};
	
	GameObject cameraGameObj;
	GameObject gameBackground;
	GameObject craneBody;
	GameObject craneArm;
	GameObject craneHook;
	GameObject crane;
	Vector2 craneBodyDefaults;
	Vector2 craneArmDefaults;
	
	public const int blockObjectOutOfScreenPosY = -6;
	public const int backgroundObjectOutOfScreenPosY = -8;
	public const int craneBodyObjectOutOfScreenPosY = -8;
	public const float screenMoveDownCameraOffset = -1.0f;
	const int defaultNumberOfBlocksToSpare = 5;
	int spareBlocks;
	
	Animator craneArmAnimator;
	Animator craneBodyAnimator;
	Animator craneHookAnimator;
	Animator gameBackgroundAnimator;
	Animator platformAnimator;
	
	gameStates statusOfGame;
	gameNavigation gameNav;
	
	const string SaveGameDirectoryName = "/SaveGame";
	public static string playerNameFile = SaveGameDirectoryName + "/playerName.tBlocks";
	public static string craneHookSpeedFile = SaveGameDirectoryName + "/craneHookSpeed.tBlocks";
	public static string DropAssistFile = SaveGameDirectoryName + "/dropAssist.tBlocks";
	public static string gravityFile = SaveGameDirectoryName + "/gravity.tBlocks";
	public static string windAppliedFile = SaveGameDirectoryName + "/windApplied.tBlocks";
	public static string scoreboardFilePath = SaveGameDirectoryName +"/highscores.tblocks";
	
	public static float offsetPerfectPositioning = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
			this.craneHookScriptObject = GameObject.Find("crane_hook").GetComponent<craneHookScript>();
			this.craneHookAnimator = this.craneHookScriptObject.GetComponent<Animator>();
			this.platformScriptObject = GameObject.Find("platform").GetComponent<platformScript>();
			this.scoreScriptObject = GameObject.Find("ScoreText").GetComponent<scoreTextScript>();
			this.StartGameButtonScriptObject = GameObject.Find("StartGameButton").GetComponent<StartGameButtonScript>();
			this.HighScoreButtonScriptObject = GameObject.Find("HighScoreButton").GetComponent<HighScoreButtonScript>();
			this.OptionsButtonScriptObject = GameObject.Find("OptionsButton").GetComponent<OptionsButtonScript>();
			this.SpareBlocksTextScriptObject = GameObject.Find("SpareBlocksText").GetComponent<SpareBlocksTextScript>();
			this.OptionsGravitySliderScriptObject = GameObject.Find("OptionsGravitySlider").GetComponent<OptionsGravitySliderScript>();
			this.OptionsWindAppliedSliderScriptObject = GameObject.Find("OptionsAppliedWindSlider").GetComponent<OptionsWindAppliedSliderScript>();
			
			this.gameOverSpriteRenderer = GameObject.Find("gameOverLogo").GetComponent<SpriteRenderer>();
			this.gameOverMessageTextComponent = GameObject.Find("GameOverMessageText").GetComponent<TMPro.TMP_Text>();
			this.CanvasGameOver = GameObject.Find("CanvasGameOver").GetComponent<Canvas>();
			this.platformTextScriptObject = GameObject.Find("platform_text").GetComponent<platformTextScript>();
			
			this.cameraGameObj = GameObject.Find("Main Camera");
			this.gameBackground = GameObject.Find("background");
			this.craneBody = GameObject.Find("crane_body");
			this.craneArm = GameObject.Find("crane_arm");
			this.crane = GameObject.Find("crane");
			this.craneBodyDefaults = new Vector2(this.craneBody.transform.position.x, this.craneBody.transform.position.y);
			this.craneArmDefaults = new Vector2(this.craneArm.transform.position.x, this.craneArm.transform.position.y);
			
			this.craneArmAnimator = this.craneArm.GetComponent<Animator>();
			this.craneBodyAnimator = this.craneBody.GetComponent<Animator>();
			this.gameBackgroundAnimator = this.gameBackground .GetComponent<Animator>();
			this.platformAnimator = this.platformScriptObject.GetComponent<Animator>();
			
			this.statusOfGame = gameStates.NotStarted;
			this.gameNav = gameNavigation.MainMenu;
			this.spareBlocks = defaultNumberOfBlocksToSpare;
			this.SpareBlocksTextScriptObject.updateSpareBlocks(this.spareBlocks);
			
			this.gameBackgroundPositionDefaults = new Vector3 (this.gameBackground.transform.position.x, this.gameBackground.transform.position.y, this.gameBackground.transform.position.z);
			this.craneBodyPositionDefaults = new Vector3 (this.craneBody.transform.position.x, this.craneBody.transform.position.y, this.craneBody.transform.position.z);
			this.craneArmPositionDefaults = new Vector3 (this.craneArm.transform.position.x, this.craneArm.transform.position.y, this.craneArm.transform.position.z);
			this.craneHookPositionDefaults = new Vector3 (this.craneHookScriptObject.gameObject.transform.position.x, this.craneHookScriptObject.gameObject.transform.position.y, this.craneHookScriptObject.gameObject.transform.position.z);
			
			playerNameFile = Application.persistentDataPath + playerNameFile;
			DropAssistFile = Application.persistentDataPath + DropAssistFile;
			craneHookSpeedFile = Application.persistentDataPath + craneHookSpeedFile;
			gravityFile =  Application.persistentDataPath + gravityFile;
			windAppliedFile = Application.persistentDataPath + windAppliedFile;
			scoreboardFilePath = Application.persistentDataPath + scoreboardFilePath;
			
			System.IO.Directory.CreateDirectory(string.Format("{0}\\{1}", Application.persistentDataPath, SaveGameDirectoryName));
			
			readGravitySavedFromSlider();

			this.OptionsWindAppliedSliderScriptObject.readWindAppliedFromFile();
    }

    // Update is called once per frame
    void Update()
    {
		if (this.statusOfGame == gameStates.Running)
		{
			this.craneHookScriptObject.blockScriptObject.DropBlock();
			Transform blockOnTopOfPlatformTransform = this.platformScriptObject.getBlockOnTopTransform();
			if (this.craneHookScriptObject.blockScriptObject.rigidBodyComponent != null)
			{
				if (this.craneHookScriptObject.blockScriptObject.isBlockDropped() && this.craneHookScriptObject.blockScriptObject.getOverlapping())
				{
					//if entered here - velocity.magnitude 0 means body has stopped from moving / falling, if y < 3 but in the sametime > -5 it means it landed on the platform.
					this.attachObjectToPlatform();
				}
				else if (this.craneHookScriptObject.blockScriptObject.isBlockDropped() && this.craneHookScriptObject.blockScriptObject.rigidBodyComponent.velocity.magnitude > 0 
						&& this.craneHookScriptObject.blockScriptObject.gameObject.transform.position.y <= blockObjectOutOfScreenPosY)
				{
					this.decreaseSpareBlocks();
				}
			}
			if (blockOnTopOfPlatformTransform != null )
			{
				//will enter this if when the last platform block will reach same Y as the camera
				if (blockOnTopOfPlatformTransform.position.y >= this.cameraGameObj.transform.position.y + screenMoveDownCameraOffset)
				{
					this.MoveScreenPlatformDown();
				}
			}
			this.StartGameButtonScriptObject.displayMenu();
		}
		else if(this.gameNav == gameNavigation.HighScore)
		{
			this.HighScoreButtonScriptObject.HideHighScore();
		}
		else if(this.gameNav == gameNavigation.MainMenu && this.statusOfGame == gameStates.Paused)
		{
			this.StartGameButtonScriptObject.checkResumeGame();
		}
		else if(this.gameNav == gameNavigation.Options)
		{
			this.OptionsButtonScriptObject.hideOptions();
		}
    }
	public void resetSpareBlocks()
	{
		this.spareBlocks = defaultNumberOfBlocksToSpare;
	}
	public void setGameNavigation(gameNavigation nav)
	{
		this.gameNav = nav;
	}
	public gameNavigation getGameNavigation()
	{
		return this.gameNav;
	}
	public void setGameState(gameStates state)
	{
		this.statusOfGame = state;
	}
	public gameStates getGameState()
	{
		return this.statusOfGame;
	}
	public int getSpareBlocks()
	{
		return this.spareBlocks;
	}
	void decreaseSpareBlocks()
	{
		this.spareBlocks--;
		this.SpareBlocksTextScriptObject.updateSpareBlocks(this.spareBlocks);
		this.platformTextScriptObject.resetCombo();
		if (this.spareBlocks < 0) // once here it's gameover
		{
			this.gameOverSpriteRenderer.enabled = true;
			this.statusOfGame = gameStates.GameOver;
			bool highscore_set = this.HighScoreButtonScriptObject.checkSetNewScore();
			this.CanvasGameOver.enabled = true;
			if (highscore_set)
			{
				this.gameOverMessageTextComponent.text = "You set a new highscore record: " + this.scoreScriptObject.getScore() + 
														"\nThe top score is: " + this.HighScoreButtonScriptObject.getHighestScoreOnScoreboard();
			}
			else
			{
				this.gameOverMessageTextComponent.text = "You didn't set a new highscore record: \nThe old records are between: "  
				+ this.HighScoreButtonScriptObject.getSmallestScoreOnScoreboard() + " - " + this.HighScoreButtonScriptObject.getHighestScoreOnScoreboard();
			}
		}
	}
	public static float getGravity()
	{
		return gravityAppliedToFallingBlock;
	}
	public static void setGravity(float gravity)
	{
		//Keeping the gravity value controlled
		if (defaultGravityAppliedToFalingBlock + gravity < 0 && defaultGravityAppliedToFalingBlock + gravity > -4)
		{
			gravityAppliedToFallingBlock = (defaultGravityAppliedToFalingBlock + (gravity));
		}
	}
	public static float giveAppliedGravityToSlider()
	{
		readGravitySavedFromSlider();
		return (gravityAppliedToFallingBlock - defaultGravityAppliedToFalingBlock);
	}
	static void readGravitySavedFromSlider()
	{
		System.IO.FileInfo fInfo = new System.IO.FileInfo(gravityFile);
		if (System.IO.File.Exists(gravityFile) && fInfo.Length > 0)
		{
			string gravityContent = System.IO.File.ReadAllText(gravityFile);
			float gravityParsed = float.Parse(gravityContent);
			gravityAppliedToFallingBlock = gravityParsed;
		}
	}
	public void resetGame()
	{
			this.gameBackground.transform.position = new Vector3(this.gameBackgroundPositionDefaults.x, this.gameBackgroundPositionDefaults.y, this.gameBackgroundPositionDefaults.z);
			this.craneBody.transform.position  = new Vector3(this.craneBodyPositionDefaults.x, this.craneBodyPositionDefaults.y, this.craneBodyPositionDefaults.z);
			this.craneArm.transform.position = new Vector3(this.craneArmPositionDefaults.x, this.craneArmPositionDefaults.y, this.craneArmPositionDefaults.z);
			this.craneHookScriptObject.gameObject.transform.position = new Vector3(this.craneHookPositionDefaults.x, this.craneHookPositionDefaults.y, this.craneHookPositionDefaults.z);
						
			this.platformScriptObject.resetPlatform();
			this.craneHookAnimator.speed = 0;
	}
	void attachObjectToPlatform()
	{
		Destroy(this.craneHookScriptObject.blockScriptObject.rigidBodyComponent);
		this.checkPlacePerfectDrop();
		this.craneHookScriptObject.blockScriptObject.gameObject.transform.parent = this.platformScriptObject.gameObject.transform;
		this.platformScriptObject.checkIfLastElementIncreasedTheAltitude();
	}
	void MoveScreenPlatformDown()
	{
		//int numberOfUnits = 2; // this variable was used to moving gameObject down the screen with 2 units. 
		
		/*
		this.platformScriptObject.gameObject.transform.position = new Vector3(this.platformScriptObject.gameObject.transform.position.x, 
											this.platformScriptObject.gameObject.transform.position.y - numberOfUnits, this.platformScriptObject.gameObject.transform.position.z);
		*/
		
		//this animation will move the platform down with 2 units - replacing the commented statement above.
		this.platformAnimator.Play("platformGoingDown"); //trigger animation using Play - works better than triggering animation.
		
		//this.platformAnimator.SetBool("platformDropping", true); //trigger animation using bool - doesn't work properly on the circumstances. Once bool is set to true, it will keep repeating the animation
		//this.platformAnimator.SetTrigger("platformDropping"); //trigger animation using trigger - works slower than Play() function from above.
			
		if (this.gameBackground.transform.position.y > backgroundObjectOutOfScreenPosY) // if this.gameBackground.transform.position.y gets smaller than  ObjectOutOfScreenPosY it means we reached blue sky which is the repetitive part of the background
		{
			//Using root motion animation to move elements
			this.gameBackgroundAnimator.Play("backgroundGoingDown"); //the animation will move the background down with 2 units replacing the commented statement below:
			
			//this.gameBackground.transform.position = new Vector3(this.gameBackground.transform.position.x, this.gameBackground.transform.position.y - numberOfUnits, this.gameBackground.transform.position.z);
		}
		if(this.crane.transform.Find(this.craneBody.gameObject.name).gameObject.transform.position.y >= craneBodyObjectOutOfScreenPosY) // getting the craneBody position relative to the world.
		{
			this.craneArmAnimator.Play("craneArmGoingUp"); //the animation will move the craneArm up with 2 units to compensate for the craneBody which is going down the screen in the next animation.
			this.craneBodyAnimator.Play("craneBodyGoingDown"); //the animation will move the craneBody down with 2 units replacing the commented statement below:
			
			//this.craneBody.transform.position = new Vector3(this.craneBody.transform.position.x, this.craneBody.transform.position.y - numberOfUnits, this.craneBody.transform.position.z);
		}
		else
		{
			//This will reset the position of the craneBody so that the animation can be rerun without reaching the end of the texture.
			this.craneBody.transform.position = new Vector2 (this.craneBodyDefaults.x, this.craneBodyDefaults.y - (this.craneBodyDefaults.y - this.craneArmDefaults.y));
			this.craneArm.transform.position = new Vector2 (this.craneArmDefaults.x, this.craneArmDefaults.y);
		}
	}
	void checkPlacePerfectDrop()
	{
		GameObject lastPlatformChild = null;
		if (this.platformScriptObject.gameObject.transform.childCount > this.platformScriptObject.platformNumberOfElements) 
		{
			int lastPlatformChildIndex = this.platformScriptObject.gameObject.transform.childCount -1;
			lastPlatformChild = this.platformScriptObject.gameObject.transform.GetChild(lastPlatformChildIndex).gameObject;
			
			if (this.craneHookScriptObject.blockScriptObject.gameObject.transform.position.x <= lastPlatformChild.transform.position.x + offsetPerfectPositioning &&
				this.craneHookScriptObject.blockScriptObject.gameObject.transform.position.x >= lastPlatformChild.transform.position.x - offsetPerfectPositioning)
			{		
				this.scoreScriptObject.updatePerfectScore(lastPlatformChild.transform);
				//Allign this block on the previous block;
				this.craneHookScriptObject.blockScriptObject.gameObject.transform.position = new Vector3(lastPlatformChild.transform.position.x, 
																				this.craneHookScriptObject.blockScriptObject.gameObject.transform.position.y,
																				this.craneHookScriptObject.blockScriptObject.gameObject.transform.position.z);
			}
			else
			{
				this.scoreScriptObject.updateRegularScore();
			}
		}
		else
		{
			this.scoreScriptObject.updateRegularScore();
		}
	}
	//function not used;
	float calculateDifferenceOnXAxis(Transform ObjectA, Transform objectB)
	{
		if(ObjectA.position.x > objectB.position.x)
		{
			return ObjectA.position.x - objectB.position.x;
		}
		else
		{
			return objectB.position.x - ObjectA.position.x;
		}
	}
}
