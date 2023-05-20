using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script describes the behaviour of StartGameButton

public class StartGameButtonScript : MonoBehaviour
{
	gameScript gameScriptObject;
	
	craneHookScript craneHookScriptObject;
	Animator craneHookAnimator;

	Canvas canvasHUDObjectComponent;
	Canvas canvasMenuObjectComponent;
	TMPro.TMP_Text buttonTextComponent;
	
    // Start is called before the first frame update
    void Start()
    {
        this.gameScriptObject = GameObject.Find("game").GetComponent<gameScript>();
		this.craneHookScriptObject = GameObject.Find("crane_hook").GetComponent<craneHookScript>();
		this.craneHookAnimator = this.craneHookScriptObject.GetComponent<Animator>();
		this.canvasHUDObjectComponent = GameObject.Find("CanvasHUD").GetComponent<Canvas>();
		this.canvasMenuObjectComponent = GameObject.Find("CanvasMenu").GetComponent<Canvas>();
		this.buttonTextComponent = this.gameObject.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
    }
	
	public void StartNewGame()
	{
if ( (this.gameScriptObject.getGameState() == gameScript.gameStates.NotStarted || this.gameScriptObject.getGameState() == gameScript.gameStates.GameOver) 
		&& this.gameScriptObject.getGameNavigation() == gameScript.gameNavigation.MainMenu)
		{
			if (this.gameScriptObject.getGameState() == gameScript.gameStates.GameOver)
			{
				//put the craneHook in its original position
				//this.craneHookAnimator.Play("Default");
			}
			this.buttonTextComponent.text = "Resume Game";
			this.craneHookScriptObject.prepareCraneHook();
			this.gameScriptObject.setGameState(gameScript.gameStates.Running);
			this.gameScriptObject.setGameNavigation(gameScript.gameNavigation.Game);
			
			this.craneHookAnimator.Play(OptionsCraneHookSpeedSliderScript.animationUsedForCraneHook);
			this.craneHookAnimator.speed = 1;
			//SetActive () will disable the gameobject completely and could lead to reference not set to instance of an object. Rather not to be used.
			//this.canvasHUDObject.SetActive(true); //  this will show the HUD; 
			this.canvasHUDObjectComponent.enabled = true; //  this will show the HUD;
			
			//SetActive () will disable the gameobject completely and could lead to reference not set to instance of an object. Rather not to be used.
			//this.canvasMenuObject.SetActive(false);  //this will hide the menu
			this.canvasMenuObjectComponent.enabled = false;
		}
		this.ResumeGame();
	}
	
	public void displayMenu()
	{
		if(Input.GetKeyDown(KeyCode.Escape) && this.gameScriptObject.getGameState() == gameScript.gameStates.Running && 
								this.gameScriptObject.getGameNavigation() == gameScript.gameNavigation.Game)
		{
			this.gameScriptObject.setGameState(gameScript.gameStates.Paused);
			this.gameScriptObject.setGameNavigation(gameScript.gameNavigation.MainMenu);
			this.craneHookAnimator.speed = 0;
			this.canvasMenuObjectComponent.enabled = true;
			this.canvasHUDObjectComponent.enabled = false;
		}
	}
	void ResumeGame()
	{
		if (this.gameScriptObject.getGameState() == gameScript.gameStates.Paused && this.gameScriptObject.getGameNavigation() == gameScript.gameNavigation.MainMenu)
		{
			this.gameScriptObject.setGameState(gameScript.gameStates.Running);
			this.gameScriptObject.setGameNavigation(gameScript.gameNavigation.Game);
			if(this.craneHookAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name != OptionsCraneHookSpeedSliderScript.animationUsedForCraneHook) //if the cranehook animation speed was changed in the options
			{
				this.craneHookAnimator.Play(OptionsCraneHookSpeedSliderScript.animationUsedForCraneHook);
			}
			this.craneHookAnimator.speed = 1;
			this.canvasMenuObjectComponent.enabled = false;
			this.canvasHUDObjectComponent.enabled = true;
		}
	}
	public void checkResumeGame()
	{
		if( Input.GetKeyDown(KeyCode.Escape) )
		{
			this.ResumeGame();
		}
	}
	public void resetButtonName()
	{
		this.buttonTextComponent.text = "Start Game";
	}
}
