using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNameTextScript : MonoBehaviour
{
	TMPro.TMP_Text playerNameTextComponent;
	Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        this.playerNameTextComponent = this.gameObject.GetComponent<TMPro.TMP_Text>();
		this.anim = this.gameObject.GetComponent<Animator>();
		this.playerNameTextComponent.text = readPlayerNameFromFile();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	public void changePlayerName(string Name)
	{
		this.playerNameTextComponent.text = Name;
		Debug.Log("Name: " + Name);
		this.anim.Play("generalTextChanging");
	}
	public string getPlayerName()
	{
		return playerNameTextComponent.text;
	}
	public static string readPlayerNameFromFile()
	{
		System.IO.FileInfo fInfo = new System.IO.FileInfo(gameScript.playerNameFile);
		if (System.IO.File.Exists(gameScript.playerNameFile) && fInfo.Length > 0)
		{
			string playerName = System.IO.File.ReadAllText(gameScript.playerNameFile);
			return playerName;
		}
		else
		{
			return "Name";
		}
	}
}
