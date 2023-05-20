using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This script describes the behaviour of MaxCombo Label*/

public class MaxComboScript : MonoBehaviour
{
    // Start is called before the first frame update
	TMPro.TMP_Text maxComboTextComponent;
	uint maxCombo;
	Animator maxComboAnimator;
    void Start()
    {
        this.maxComboTextComponent = this.gameObject.GetComponent<TMPro.TMP_Text>();
		this.maxCombo = 1;
		this.maxComboTextComponent.text = "MaxCombo: " + this.maxCombo;
		this.maxComboAnimator = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	public void checkMaxCombo(uint value)
	{
		if (this.maxCombo < value)
		{
			this.maxCombo = value;
			this.maxComboTextComponent.text = "MaxCombo: " + this.maxCombo;
			this.maxComboAnimator.Play("generalTextChanging");
		}
	}
	
	public void resetMaxCombo()
	{
		this.maxCombo = 1;
		this.maxComboTextComponent.text = "MaxCombo: " + this.maxCombo;
	}
}
