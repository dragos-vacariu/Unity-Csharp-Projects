using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceTimeScript : MonoBehaviour
{
    Text raceTimeTextComponent;
    float raceTime;
    // Start is called before the first frame update
    void Start()
    {
        this.raceTimeTextComponent = GetComponent<UnityEngine.UI.Text>();
        this.raceTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        this.raceTime+=Time.deltaTime;
        raceTimeTextComponent.text = "Race Time: " + (int)(raceTime/60) + ":" + (raceTime%60).ToString("00.");
    }
    
    public float getTime()
    {
        return this.raceTime;
    }
}
