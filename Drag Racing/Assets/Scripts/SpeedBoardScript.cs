using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedBoardScript : MonoBehaviour
{
    Rigidbody rigidBodyComponent;
    PlayerScript playerComponent;
    Text speedTextComponent;
    // Start is called before the first frame update
    void Start()
    {
        this.playerComponent = GameObject.Find("Player").GetComponent<PlayerScript>();
        this.speedTextComponent = GetComponent<UnityEngine.UI.Text>();
    }

    // Update is called once per frame
    void Update()
    {
        float speed = this.mapSpeedValue(playerComponent.getPlayerCar().getCurrentCarSpeed());
        this.speedTextComponent.text = "Speed: " + speed.ToString("0.00") + " KMH";
    }
    
    float mapSpeedValue(float speedValue)
    {
        //map the speed value with a factor of 5 to look more normal;
        return speedValue*15;
    }
}
