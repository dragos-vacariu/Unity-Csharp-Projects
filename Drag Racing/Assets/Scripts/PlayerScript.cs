using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    Rigidbody rigidBodyComponent;
    CarScript playerCar;
    // Start is called before the first frame update
    void Start()
    {
        this.playerCar = GameObject.Find("Player/Car").GetComponent<CarScript>();
        this.rigidBodyComponent = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            GameObject.Find("ShiftMessage").GetComponent<ShiftMessageScript>().showShiftMessage();
            this.playerCar.switchGear();
        }

    }
    
    void FixedUpdate()
    {
        this.playerCar.Accelerate();
        this.playerCar.RaceFinishedCarStop();
        this.playerCar.gearingThreshold();
        this.CheckInputForDirection();
    }
    
    void CheckInputForDirection()
    {
        if(Input.GetKey(KeyCode.D))
        {
            this.playerCar.TurnRight();
        }
        else if (Input.GetKey(KeyCode.A))
        {
            this.playerCar.TurnLeft();
        }
    }
    public CarScript getPlayerCar()
    {
        return this.playerCar;
    }

}
