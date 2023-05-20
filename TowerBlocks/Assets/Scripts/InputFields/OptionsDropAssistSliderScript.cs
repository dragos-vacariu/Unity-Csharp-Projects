using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsDropAssistSliderScript : MonoBehaviour
{
	UnityEngine.UI.Slider dropAssistSlider;
    // Start is called before the first frame update
    void Start()
    {
        this.dropAssistSlider = this.gameObject.GetComponent<UnityEngine.UI.Slider>();
		this.readDropAssistFromFile();
		gameScript.offsetPerfectPositioning = (this.dropAssistSlider.value * 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	public void changeDropAssist()
	{
		gameScript.offsetPerfectPositioning = (this.dropAssistSlider.value * 0.1f);
		System.IO.FileStream fstream = new System.IO.FileStream(gameScript.DropAssistFile, System.IO.FileMode.Create);  
		System.IO.StreamWriter writer = new System.IO.StreamWriter(fstream);
		writer.WriteLine(this.dropAssistSlider.value);
		writer.Close();  
        fstream.Close();  
	}
	
	void readDropAssistFromFile()
	{
		System.IO.FileInfo fInfo = new System.IO.FileInfo(gameScript.DropAssistFile);
		if (System.IO.File.Exists(gameScript.DropAssistFile) && fInfo.Length > 0)
		{
			string dropAssistFileContent = System.IO.File.ReadAllText(gameScript.DropAssistFile);
			this.dropAssistSlider.value = int.Parse(dropAssistFileContent);
		}
	}
}
