using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windDirectionTextScript : MonoBehaviour
{
    // Start is called before the first frame update
	TMPro.TMP_Text windTextComponent;
	Animator windTextAnimator;
	
    void Start()
    {
        this.windTextComponent = this.gameObject.GetComponent<TMPro.TMP_Text>();
		this.windTextAnimator = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void updateWindDirection(string windDirection, int power) 
	{
		this.windTextComponent.text = "Wind: " + windDirection + " " + power;
		if (windDirection == "left")
		{
			this.windTextAnimator.Play("windLeftAnimation");
		}
		else if(windDirection == "right")
		{
			this.windTextAnimator.Play("windRightAnimation");
		}
		else
		{
			this.windTextAnimator.Play("generalTextChanging");
		}
	}
	public void resetWindDirectionText()
	{
		this.windTextComponent.text = "Wind: ";
	}
}
