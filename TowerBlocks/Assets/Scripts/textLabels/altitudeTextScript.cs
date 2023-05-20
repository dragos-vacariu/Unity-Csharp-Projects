using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class altitudeTextScript : MonoBehaviour
{
    // Start is called before the first frame update
	TMPro.TMP_Text altitudeLevelTextComponent;

	Animator altitudeAnimator;
	
	uint blockHeightInMeters;
    void Start()
    {
        this.altitudeLevelTextComponent = this.gameObject.GetComponent<TMPro.TMP_Text>();
		this.altitudeLevelTextComponent.text = "Altitude: 0m";
		this.altitudeAnimator = this.gameObject.GetComponent<Animator>();
		this.blockHeightInMeters = 8;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	public void setAltitudeLevel(uint altitude)
	{
		this.altitudeLevelTextComponent.text = "Altitude: " + altitude * this.blockHeightInMeters + "m";
		this.altitudeAnimator.Play("generalTextChanging");
	}
	public void resetAltitude()
	{
		this.altitudeLevelTextComponent.text = "Altitude: 0m";
	}
}
