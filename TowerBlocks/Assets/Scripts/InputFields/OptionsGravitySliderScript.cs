using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsGravitySliderScript : MonoBehaviour
{
    // Start is called before the first frame update
	UnityEngine.UI.Slider gravitySlider;
	
    void Start()
    {
        this.gravitySlider = this.gameObject.GetComponent<UnityEngine.UI.Slider>();
	}

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void changeGravity()
	{
		gameScript.setGravity(this.gravitySlider.value * -1); // *-1 to change sign because more gravity means smaller value applied to velocity on Y axis
		System.IO.FileStream fstream = new System.IO.FileStream(gameScript.gravityFile, System.IO.FileMode.Create);  
		System.IO.StreamWriter writer = new System.IO.StreamWriter(fstream);
		writer.WriteLine(this.gravitySlider.value * -1); // *-1 to change sign because more gravity means smaller value applied to velocity on Y axis
		writer.Close();  
        fstream.Close();  
	}
	public void setGravitySlider(float gravity)
	{
		this.gravitySlider = this.gameObject.GetComponent<UnityEngine.UI.Slider>();   
		this.gravitySlider.value = gravity * (-1); // *-1 to change sign because more gravity means smaller value applied to velocity on Y axis
	}
}
