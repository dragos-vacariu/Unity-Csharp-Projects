using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsButtonScript : MonoBehaviour
{
    // Start is called before the first frame update
	Canvas Options;
	gameScript gameScriptObject;
	Canvas canvasMenuObjectCanvas;
	OptionsGravitySliderScript OptionsGravitySliderScriptObject;
	
    void Start()
    {
        this.Options = GameObject.Find("Options").GetComponent<Canvas>();
		this.gameScriptObject = GameObject.Find("game").GetComponent<gameScript>();
		this.canvasMenuObjectCanvas = GameObject.Find("CanvasMenu").GetComponent<Canvas>();
		this.OptionsGravitySliderScriptObject = GameObject.Find("OptionsGravitySlider").GetComponent<OptionsGravitySliderScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	public void showOptions()
	{
		this.canvasMenuObjectCanvas.enabled = false;
		this.Options.enabled = true;
		this.gameScriptObject.setGameNavigation(gameScript.gameNavigation.Options);
		this.OptionsGravitySliderScriptObject.setGravitySlider(gameScript.getGravity());
	}
	public void hideOptions()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && this.gameScriptObject.getGameNavigation() == gameScript.gameNavigation.Options)
		{
			this.canvasMenuObjectCanvas.enabled = true;
			this.Options.enabled = false;
			this.gameScriptObject.setGameNavigation(gameScript.gameNavigation.MainMenu);
		}
	}
}
