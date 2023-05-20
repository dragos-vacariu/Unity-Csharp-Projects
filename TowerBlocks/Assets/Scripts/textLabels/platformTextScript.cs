using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This script describes the behaviour of combo label text when dropping perfectly*/

public class platformTextScript : MonoBehaviour
{
	uint combo;
	TMPro.TMP_Text platformTextComponent;

	Animator anim;
	platformScript platformScriptObject;
	MaxComboScript maxComboScriptObject;
    // Start is called before the first frame update
    void Start()
    {
        this.platformTextComponent = this.gameObject.GetComponent<TMPro.TMP_Text>();
		this.resetCombo();
		this.platformScriptObject = GameObject.Find("platform").GetComponent<platformScript>();
		this.maxComboScriptObject = GameObject.Find("MaxComboText").GetComponent<MaxComboScript>();
		this.anim = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	public void updateCombo(Transform textPlacementTransform)
	{
		this.combo++;
		this.platformTextComponent.text = "Bonus X" + this.combo;
		//this.platformTextComponent.enabled = true;
		this.repositionTextObject(textPlacementTransform);
		this.anim.Play("platformTextAppearDissapear");
		this.maxComboScriptObject.checkMaxCombo(this.combo);
	}
	public void resetCombo()
	{
		//this.platformTextComponent.enabled = false;
		this.combo = 1;
	}
	public uint getCombo()
	{
		return this.combo;
	}
	void repositionTextObject(Transform textPlacementTransform)
	{
		float YAxisPlacementOffset = 2.0f;
		this.gameObject.transform.position = new Vector3 (textPlacementTransform.position.x, textPlacementTransform.position.y + YAxisPlacementOffset, 
																		textPlacementTransform.position.z);
	}
}
