using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsCraneHookSpeedSliderScript : MonoBehaviour
{
    // Start is called before the first frame update
	UnityEngine.UI.Slider craneHookSpeedSlider;
	public static string animationUsedForCraneHook = "craneHookBalancingSpeed2";
    void Start()
    {
        this.craneHookSpeedSlider = this.gameObject.GetComponent<UnityEngine.UI.Slider>();
		this.readCraneHookSpeedFromFile();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void changeCraneHookSpeed()
	{
		this.pickAnimationBasedOnSliderValue((int)this.craneHookSpeedSlider.value);
		System.IO.FileStream fstream = new System.IO.FileStream(gameScript.craneHookSpeedFile, System.IO.FileMode.Create);  
		System.IO.StreamWriter writer = new System.IO.StreamWriter(fstream);
		writer.WriteLine(this.craneHookSpeedSlider.value);
		writer.Close();  
        fstream.Close();  
	}
	void readCraneHookSpeedFromFile()
	{
		System.IO.FileInfo fInfo = new System.IO.FileInfo(gameScript.craneHookSpeedFile);
		if (System.IO.File.Exists(gameScript.craneHookSpeedFile) && fInfo.Length > 0)
		{
			string craneHookSpeedFileContent = System.IO.File.ReadAllText(gameScript.craneHookSpeedFile);
			int craneHookSpeedParsed = int.Parse(craneHookSpeedFileContent);  
			this.pickAnimationBasedOnSliderValue(craneHookSpeedParsed);
			this.craneHookSpeedSlider.value = craneHookSpeedParsed;
		}
	}
	void pickAnimationBasedOnSliderValue(int value)
	{
		switch(value)
		{
			case 1: 
			{
				animationUsedForCraneHook = "craneHookBalancingSpeed1";
				break;
			}
			case 2:
			{
				animationUsedForCraneHook = "craneHookBalancingSpeed2";
				break;
			}
			case 3:
			{
				animationUsedForCraneHook = "craneHookBalancingSpeed3";
				break;
			}
			default:
			{
				animationUsedForCraneHook = "craneHookBalancingSpeed2";
				break;
			}
		}
	}
}
