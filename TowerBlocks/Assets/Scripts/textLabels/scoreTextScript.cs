using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script describes the behavior of scoreText label

public class scoreTextScript : MonoBehaviour
{
    // Start is called before the first frame update
	uint score;
	TMPro.TMP_Text scoreTextComponent;
	Animator anim;
	platformTextScript platformTextScriptObject;
	
    void Start()
    {
        this.score = 0;
		this.scoreTextComponent = this.gameObject.GetComponent<TMPro.TMP_Text>();
		this.platformTextScriptObject = GameObject.Find("platform_text").GetComponent<platformTextScript>();
		this.scoreTextComponent.text = "Score: " + this.score;
		this.anim = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void updateRegularScore()
	{
		this.score++;
		this.scoreTextComponent.text = "Score: " + this.score;
		this.anim.Play("scoreRegularAnimation");
		this.platformTextScriptObject.resetCombo();
	}
	public void updatePerfectScore(Transform placementTransformForText)
	{
		this.platformTextScriptObject.updateCombo(placementTransformForText);
		this.score += this.platformTextScriptObject.getCombo();
		this.scoreTextComponent.text = "Score: " + this.score;
		this.anim.Play("scorePerfectAnimation");
	}
	public uint getScore()
	{
		return this.score;
	}
	public void resetScore()
	{
		this.score = 0;
		this.scoreTextComponent.text = "Score: " + this.score;
	}
}
