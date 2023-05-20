using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsWindAppliedSliderScript : MonoBehaviour
{
	UnityEngine.UI.Slider windAppliedSlider;
	
    // Start is called before the first frame update
    void Start()
    {
        this.windAppliedSlider = this.gameObject.GetComponent<UnityEngine.UI.Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void changeAppliedWind()
	{
		gameScript.maxWindAppliedToFallingBlock = (int)this.windAppliedSlider.value;
		blockScript.windToBeApplied = 0; // reset the actual wind;
		System.IO.FileStream fstream = new System.IO.FileStream(gameScript.windAppliedFile, System.IO.FileMode.Create);  
		System.IO.StreamWriter writer = new System.IO.StreamWriter(fstream);
		writer.WriteLine(this.windAppliedSlider.value);
		Debug.Log(this.windAppliedSlider.value);
		writer.Close();  
        fstream.Close();
	}
	public void readWindAppliedFromFile()
	{
		System.IO.FileInfo fInfo = new System.IO.FileInfo(gameScript.windAppliedFile);
		if (System.IO.File.Exists(gameScript.windAppliedFile) && fInfo.Length > 0)
		{
			string windAppliedFileContent = System.IO.File.ReadAllText(gameScript.windAppliedFile);
			int windAppliedParsed = int.Parse(windAppliedFileContent);  
			this.windAppliedSlider.value = windAppliedParsed;
			gameScript.maxWindAppliedToFallingBlock = windAppliedParsed;
		}
	}
}
