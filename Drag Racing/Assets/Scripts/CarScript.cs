using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float topSpeed = 0.0f;
    [SerializeField] float carAccelerationFactor = 0.0f;
    float currentCarSpeed;
    float turningSensivity;
    int carNoGears;
    int carCurrentGear; 
    public enum ShiftStatus
    {
        NA,
        EarlyShift,
        GoodShift,
        PerfectShift,
        OverShift,
    }
    ShiftStatus shiftStatus;
    Rigidbody carRigidBodyComponent;
    void Start()
    {
        this.currentCarSpeed = 0.0f;
        if (this.carAccelerationFactor == 0.0f) // in case no value was provided in the Unity field
            this.carAccelerationFactor = 0.05f;
        if (this.topSpeed==0.0f) // in case no value was provided in the Unity field
            this.topSpeed = 15f; 
        this.carNoGears = 6;
        this.carCurrentGear = 1;
        this.shiftStatus = ShiftStatus.NA;
        this.turningSensivity = 0.5f;
        this.carRigidBodyComponent = gameObject.GetComponentInParent<Rigidbody>(); // the player and the car will have the same rigidbody component
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {

    }
    void accelerationReducer()
    {
        this.carAccelerationFactor *= (1.0f / this.carCurrentGear);
    }
    public float getCurrentCarSpeed()
    {
        return this.currentCarSpeed;
    }
    public float getCarTopSpeed()
    {
        return this.topSpeed;
    }
    public int getCarNoGears()
    {
        return this.carNoGears;
    }
    public int getCarCurrentGear()
    {
        return this.carCurrentGear;
    }
    public void Accelerate()
    {
        if (this.carRigidBodyComponent.transform.position.z <= GameObject.Find("FinishLine").GetComponent<Transform>().position.z)
        {
            if (this.currentCarSpeed > (this.topSpeed / this.carNoGears)*this.carCurrentGear
                 && this.carCurrentGear < this.carNoGears)
            {
                //do no increase the speed during this threshold, wait until user changes the gear
            }
            else 
            {
                if (this.currentCarSpeed<=this.topSpeed)
                {   
                    if (GameObject.Find("RacePosition").GetComponent<RacePositionScript>().draftingBehindOpponent(this.carRigidBodyComponent))
                    {
                        this.currentCarSpeed += (this.carAccelerationFactor*1.4f); // give 40% acceleration boost if the car is drafting.
                        if(this.carRigidBodyComponent == GameObject.Find("Player").GetComponent<Rigidbody>())
                        {
                            //if the player is the one drafting, use the hud to display him a message.
                            GameObject.Find("ShiftMessage").GetComponent<ShiftMessageScript>().showDraftMessage();
                        }
                    }
                    else
                    {
                        this.currentCarSpeed += this.carAccelerationFactor;
                    }
                }
                else
                {
                    this.currentCarSpeed = this.topSpeed;
                }
            }
            this.carRigidBodyComponent.transform.position = new Vector3(this.carRigidBodyComponent.transform.position.x + this.calculateVelocityOnXHorizontalAxis(), 
                                                          this.carRigidBodyComponent.transform.position.y, 
                                                        this.carRigidBodyComponent.transform.position.z + this.calculateVelocityOnZAxis());
        }
    }
    float calculateVelocityOnZAxis()
    {
        //this function will calculate the velocity on Z axis, taking into account the rotation on Y axis of the player.
        //when the player rotation is 90 or 180 degrees, it means the Z speed should be maximum, and the X speed should be 0.
        
        float velocityZ = (this.getCurrentCarSpeed()/10)*(90-this.carRigidBodyComponent.transform.rotation.eulerAngles.y);
        if (this.carRigidBodyComponent.transform.rotation.eulerAngles.y > 180)
            velocityZ = (this.getCurrentCarSpeed()/10)*((270-this.carRigidBodyComponent.transform.rotation.eulerAngles.y)*-1);
        
        return velocityZ/90; // since each of above will multiply the value of velocityZ with a value between (0/90), it shall be divided
    }    
    float calculateVelocityOnXHorizontalAxis()
    {
        //this function will calculate the velocity on X axis, taking into account the rotation on Y axis of the player.
        //when the player rotation is 90 or 270 degrees, it means the X speed should be maximum, and the Z speed should be 0.
        
        float velocityX = (this.getCurrentCarSpeed()/10)*(this.carRigidBodyComponent.transform.rotation.eulerAngles.y);
        if (this.carRigidBodyComponent.transform.rotation.eulerAngles.y > 90 && this.carRigidBodyComponent.transform.rotation.eulerAngles.y <= 180)
            velocityX = (this.getCurrentCarSpeed()/10)*(90-(this.carRigidBodyComponent.transform.rotation.eulerAngles.y-90));
        else if (this.carRigidBodyComponent.transform.rotation.eulerAngles.y > 180 && this.carRigidBodyComponent.transform.rotation.eulerAngles.y <= 270)
            velocityX = (this.getCurrentCarSpeed()/10)*((this.carRigidBodyComponent.transform.rotation.eulerAngles.y-180)*-1);
        else if (this.carRigidBodyComponent.transform.rotation.eulerAngles.y > 270 )
            velocityX = (this.getCurrentCarSpeed()/10)*(-90+(this.carRigidBodyComponent.transform.rotation.eulerAngles.y-270));

        return velocityX/90; // since each of above will multiply the value of velocityX with a value between (0/90), it shall be divided
    }
    public void Decelerate(float decelerationFactor = 0.5f)
    {
        if (this.currentCarSpeed > 0.0f)
        {
            this.currentCarSpeed -= decelerationFactor;
            this.carRigidBodyComponent.transform.position = new Vector3(this.carRigidBodyComponent.transform.position.x, 
                                                        this.carRigidBodyComponent.transform.position.y, 
                                                        this.carRigidBodyComponent.transform.position.z + this.getCurrentCarSpeed()/10);
        }
        else
        {
            this.currentCarSpeed = 0.0f;
        }
    }
    public void TurnRight()
    {
        //let the car be rotated, so that the velocity will make it move on X axis.
        this.carRigidBodyComponent.transform.Rotate(0,turningSensivity,0);
    }    
    public void TurnLeft()
    {
        //let the car be rotated, so that the velocity will make it move on X axis.
        this.carRigidBodyComponent.transform.Rotate(0,turningSensivity*-1,0);  
    }
    public void switchGear()
    {
        if (this.carCurrentGear < this.carNoGears)
            this.carCurrentGear++;
            this.accelerationReducer();
            //influence the acceleration factor according to the shift took.
            if (this.shiftStatus == ShiftStatus.PerfectShift)
                this.carAccelerationFactor *=1.10f;
            else if (this.shiftStatus == ShiftStatus.OverShift)
                this.carAccelerationFactor *=0.90f;
            else if (this.shiftStatus == ShiftStatus.EarlyShift)
                this.carAccelerationFactor*=0.95f;
            this.shiftStatus = ShiftStatus.NA;
    }
    public void gearingThreshold()
    {
        if (this.currentCarSpeed > (this.topSpeed / this.carNoGears)*this.carCurrentGear - 3.0f
            && this.currentCarSpeed < (this.topSpeed / this.carNoGears)*this.carCurrentGear - 2.0f
             && this.carCurrentGear < this.carNoGears && this.shiftStatus != ShiftStatus.EarlyShift)
        {
            this.shiftStatus = ShiftStatus.EarlyShift;
        }
        else if (this.currentCarSpeed >= (this.topSpeed / this.carNoGears)*this.carCurrentGear - 2.0f
             && this.currentCarSpeed <= (this.topSpeed / this.carNoGears)*this.carCurrentGear - 1.3f
             && this.carCurrentGear < this.carNoGears && this.shiftStatus != ShiftStatus.GoodShift)
        {
            this.shiftStatus = ShiftStatus.GoodShift;
        }
        else if (this.currentCarSpeed > (this.topSpeed / this.carNoGears)*this.carCurrentGear - 1.3f
             && this.currentCarSpeed < (this.topSpeed / this.carNoGears)*this.carCurrentGear - 0.8f
             && this.carCurrentGear < this.carNoGears && this.shiftStatus != ShiftStatus.PerfectShift)
        {
            this.shiftStatus = ShiftStatus.PerfectShift;
        }
        else if (this.currentCarSpeed >= (this.topSpeed / this.carNoGears)*this.carCurrentGear - 0.8f
             && this.currentCarSpeed <= (this.topSpeed / this.carNoGears)*this.carCurrentGear
             && this.carCurrentGear < this.carNoGears && this.shiftStatus != ShiftStatus.OverShift)
        {
            this.shiftStatus = ShiftStatus.OverShift;
        }
    } 
    public ShiftStatus getCarShiftStatus()
    {
        return this.shiftStatus;
    }
    public void RaceFinishedCarStop()
    {
        if (this.carRigidBodyComponent.transform.position.z > GameObject.Find("FinishLine").GetComponent<Transform>().position.z)
        {
            this.Decelerate();
        }
    }
}
