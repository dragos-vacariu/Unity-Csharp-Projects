using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script describes the behavior of Exit button.

public class ExitButtonScript : MonoBehaviour
{
	HighScoreButtonScript HighScoreButtonScriptObject;
    // Start is called before the first frame update
    void Start()
    {
		this.HighScoreButtonScriptObject = GameObject.Find("HighScoreButton").GetComponent<HighScoreButtonScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	public void ExitButtonPressed()
	{
		this.HighScoreButtonScriptObject.checkSetNewScore(); // saving any new highscores before exiting
		Application.Quit();
	}
}
