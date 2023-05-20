using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsPlayerNameInputScript : MonoBehaviour
{
	PlayerNameTextScript PlayerNameTextScriptObject;
	TMPro.TMP_InputField PlayerNameInputField;
	
    // Start is called before the first frame update
    void Start()
    {
        this.PlayerNameTextScriptObject = GameObject.Find("PlayerNameText").GetComponent<PlayerNameTextScript>();
		this.PlayerNameInputField = this.gameObject.GetComponent<TMPro.TMP_InputField>();
		string playerName = PlayerNameTextScript.readPlayerNameFromFile();
		this.PlayerNameInputField.text = playerName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	public void changeName()
	{
		this.PlayerNameTextScriptObject.changePlayerName(this.PlayerNameInputField.text);
		System.IO.FileStream fstream = new System.IO.FileStream(gameScript.playerNameFile, System.IO.FileMode.Create);  
		System.IO.StreamWriter writer = new System.IO.StreamWriter(fstream);
		writer.WriteLine(this.PlayerNameInputField.text);
		writer.Close();  
        fstream.Close();  
	}
}
