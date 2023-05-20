using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;  
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters;

/*This script describes the behaviour of HighScoresButton*/
[Serializable]
public class Scoreboard
{
	[Serializable]
	public class Record
	{
		public string name;
		public uint score;
		public string assist;
		public string date;
		public Record(string Name, uint Score, string Assist, string dt)
		{
			this.score = Score;
			this.name = Name;
			this.assist = Assist;
			this.date = dt;
		}
	}
	public static int scoreboard_size = 5;
	public Record [] records = new Record[scoreboard_size];
	public Scoreboard()
	{
		//fast initialization of empty scoreboard
		for(uint i=0; i<scoreboard_size; i++)
		{
			this.records[i] = new Record("No Record" + i.ToString(), i, "-", DateTime.Now.ToString("MM/dd/yyyy HH:mm"));
		}
	}
	public string getObjectFieldsAsString()
	{
		string line = "";
		for(int i=0; i<scoreboard_size; i++)
		{
			//Formatting the strings for proper tabelation
			int column_NameSizeInChars = 10;
			int column_ScoreSizeInChars = 6;
			int column_AssistSizeInChars = 2;
			string formatedName = this.records[i].name;
			string formatedScore = this.records[i].score.ToString();
			string formatedAssist = this.records[i].assist;
			if(formatedAssist.Length < column_AssistSizeInChars)
			{
				formatedAssist += " ";
			}
			if (formatedName.Length > column_NameSizeInChars)
			{
				formatedName = formatedName.Substring(0, column_NameSizeInChars);
			}
			else if (formatedName.Length < column_NameSizeInChars)
			{
				formatedName += String.Concat(Enumerable.Repeat(" ", (column_NameSizeInChars-formatedName.Length)));
			}
			if (formatedScore.Length > column_ScoreSizeInChars)
			{
				formatedScore = formatedScore.Substring(0, column_ScoreSizeInChars);
			}
			else if (formatedScore.Length < column_ScoreSizeInChars)
			{
				formatedScore += String.Concat(Enumerable.Repeat(" ", (column_ScoreSizeInChars-formatedScore.Length)));
			}
			//End of formatting the strings
			line += (i+1).ToString() + ". " + formatedName + "   " + formatedScore + "   " + formatedAssist + "   " + this.records[i].date + "\n";
		}
		return line;
	}
	int getIndexOfSmallestScoreRecord()
	{
		int smallestRecordIndex = 0;
		for(int i=1; i < scoreboard_size; i++)
		{
			if (records[smallestRecordIndex].score > this.records[i].score)
			{
				smallestRecordIndex = i;
			}
		}
		return smallestRecordIndex;
	}
	int getIndexOfHighestScoreRecord()
	{
		int highestRecordIndex = 0;
		for(int i=1; i < scoreboard_size; i++)
		{
			if (records[highestRecordIndex].score < this.records[i].score)
			{
				highestRecordIndex = i;
			}
		}
		return highestRecordIndex;
	}
	public bool checkRecord(string Name, uint Score)
	{
		int smallestRecordIndex = this.getIndexOfSmallestScoreRecord();
		if(records[smallestRecordIndex].score < Score)
		{
			this.records[smallestRecordIndex] = new Record(Name, Score, (gameScript.offsetPerfectPositioning*10).ToString(), DateTime.Now.ToString("MM/dd/yyyy HH:mm"));
			this.sortRecordsDescending(); // sort the scoreboard in the descending order.
			return true;
		}
		return false;
	}
	public void sortRecordsDescending()
	{
		for(int i=0; i < scoreboard_size; i++)
		{
			for (int j=0; j< scoreboard_size; j++)
			{
				if (records[i].score > this.records[j].score)
				{
					Record aux = this.records[i];
					this.records[i] = this.records[j];
					this.records[j] = aux;
				}
			}
		}
	}
	public uint getHighestScore()
	{
		return this.records[this.getIndexOfHighestScoreRecord()].score;
	}
	public uint getSmallestScore()
	{
		return this.records[this.getIndexOfSmallestScoreRecord()].score;
	}
}

public class HighScoreButtonScript : MonoBehaviour
{
    // Start is called before the first frame update
	TMPro.TMP_Text highScoreTextComponent;
	GameObject highScoreTextObject;

	gameScript gameScriptObject;
	scoreTextScript scoreTextScriptObject;
	PlayerNameTextScript PlayerNameTextScriptObject;
	string HighScoreFileContent;
	Canvas canvasMenuObjectCanvas;
	Scoreboard HighScoreboard = new Scoreboard();
	
    void Start()
    {
		this.highScoreTextObject = GameObject.Find("highScoreText");
        this.highScoreTextComponent = this.highScoreTextObject.GetComponent<TMPro.TMP_Text>();
		this.canvasMenuObjectCanvas = GameObject.Find("CanvasMenu").GetComponent<Canvas>();
		this.highScoreTextComponent.enabled = false;
		this.gameScriptObject = GameObject.Find("game").GetComponent<gameScript>();
		this.scoreTextScriptObject = GameObject.Find("ScoreText").GetComponent<scoreTextScript>();
		this.PlayerNameTextScriptObject = GameObject.Find("PlayerNameText").GetComponent<PlayerNameTextScript>();
		//writeScoreboardToFile();
		this.readScoreboardFromFile();
		this.displayScoreBoardToCanvas();
    }

    // Update is called once per frame
    void Update()
    {
    }
	
	public void DisplayHighScore()
	{
		this.canvasMenuObjectCanvas.enabled = false;
		this.highScoreTextComponent.enabled = true;
		this.gameScriptObject.setGameNavigation(gameScript.gameNavigation.HighScore);
		this.checkSetNewScore();
	}
	public void HideHighScore()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && this.gameScriptObject.getGameNavigation() == gameScript.gameNavigation.HighScore)
		{
			this.gameScriptObject.setGameNavigation(gameScript.gameNavigation.MainMenu);
			this.canvasMenuObjectCanvas.enabled = true;
			this.highScoreTextComponent.enabled = false;
		}
	}
	void writeScoreboardToFile()
	{
		BinaryFormatter bf = new BinaryFormatter();
		// Create a file with the specified filename
		FileStream fs = new FileStream(gameScript.scoreboardFilePath, System.IO.FileMode.Create);

		// Serialize the provided object and output to filestream
		bf.Serialize(fs, this.HighScoreboard);

		// Flush the filestream buffer and close
		fs.Flush();
		fs.Close();
	}
	void readScoreboardFromFile()
	{
		FileInfo fInfo = new FileInfo(gameScript.scoreboardFilePath);
		if (File.Exists(gameScript.scoreboardFilePath) && fInfo.Length > 0)
		{
			FileStream stream = new FileStream(gameScript.scoreboardFilePath, FileMode.OpenOrCreate);  
			BinaryFormatter formatter = new BinaryFormatter();  
	  
			this.HighScoreboard = (Scoreboard)formatter.Deserialize(stream);  
	  
			stream.Close();  
		}
	}
	void displayScoreBoardToCanvas()
	{
		string Header = "<mspace=15px>    Name      Score    Assist    Date      \n\n";
		this.highScoreTextComponent.text = Header + this.HighScoreboard.getObjectFieldsAsString();
	}
	public bool checkSetNewScore() // this function should be called on GameOver or Exit.
	{
		if ( this.HighScoreboard.checkRecord(this.PlayerNameTextScriptObject.getPlayerName().TrimEnd( '\r', '\n' ), this.scoreTextScriptObject.getScore()) )
		{
			this.writeScoreboardToFile();
			this.displayScoreBoardToCanvas();
			return true;
		}
		return false;
	}
	public uint getHighestScoreOnScoreboard()
	{
		return this.HighScoreboard.getHighestScore();
	}
	public uint getSmallestScoreOnScoreboard()
	{
		return this.HighScoreboard.getSmallestScore();
	}
}
