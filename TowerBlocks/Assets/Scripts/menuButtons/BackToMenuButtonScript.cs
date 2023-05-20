using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToMenuButtonScript : MonoBehaviour
{
	Canvas CanvasGameOver;
	Canvas MenuCanvas;
	SpriteRenderer gameOverSpriteRenderer;
	gameScript gameScriptObject;
	TMPro.TMP_Text startButtonTextComponent;
	SpareBlocksTextScript SpareBlocksTextScriptObject;
	StartGameButtonScript StartGameButtonScriptObject;
	scoreTextScript scoreTextScriptObject;
	altitudeTextScript altitudeTextScriptObject;
	windDirectionTextScript windDirectionTextScriptObject;
	MaxComboScript MaxComboScriptObject;
	
    // Start is called before the first frame update
    void Start()
    {
		this.StartGameButtonScriptObject = GameObject.Find("StartGameButton").GetComponent<StartGameButtonScript>();
		this.scoreTextScriptObject = GameObject.Find("ScoreText").GetComponent<scoreTextScript>();
		this.altitudeTextScriptObject = GameObject.Find("AltitudeText").GetComponent<altitudeTextScript>();
		this.windDirectionTextScriptObject = GameObject.Find("WindDirectionText").GetComponent<windDirectionTextScript>();
		this.MaxComboScriptObject = GameObject.Find("MaxComboText").GetComponent<MaxComboScript>();
        this.CanvasGameOver = GameObject.Find("CanvasGameOver").GetComponent<Canvas>();
		this.MenuCanvas = GameObject.Find("CanvasMenu").GetComponent<Canvas>();
		this.gameScriptObject = GameObject.Find("game").GetComponent<gameScript>();
		this.gameOverSpriteRenderer = GameObject.Find("gameOverLogo").GetComponent<SpriteRenderer>();
		this.SpareBlocksTextScriptObject = GameObject.Find("SpareBlocksText").GetComponent<SpareBlocksTextScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	public void backToMenu()
	{
		this.gameOverSpriteRenderer.enabled = false;
		this.CanvasGameOver.enabled = false;
		this.MenuCanvas.enabled = true;
		//resetting the game status and gameobjects
		this.gameScriptObject.setGameNavigation(gameScript.gameNavigation.MainMenu);
		this.StartGameButtonScriptObject.resetButtonName();
		this.gameScriptObject.resetSpareBlocks();
		this.gameScriptObject.resetGame();
		//resetting the HUD
		this.SpareBlocksTextScriptObject.updateSpareBlocks(this.gameScriptObject.getSpareBlocks());
		this.altitudeTextScriptObject.resetAltitude();
		this.windDirectionTextScriptObject.resetWindDirectionText();
		this.MaxComboScriptObject.resetMaxCombo();
		this.scoreTextScriptObject.resetScore();
	}
}
