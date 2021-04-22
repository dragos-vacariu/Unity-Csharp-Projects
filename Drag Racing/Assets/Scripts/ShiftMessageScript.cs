using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShiftMessageScript : MonoBehaviour
{
    Text shiftMessageTextComponent;
    PlayerScript playerComponent;
    float messageTime;
    float messageThreshold = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        this.shiftMessageTextComponent = GetComponent<UnityEngine.UI.Text>();
        this.playerComponent = GameObject.Find("Player").GetComponent<PlayerScript>();
        this.messageTime = 5.0f;          //value outside the treshold.
    }

    // Update is called once per frame
    void Update()
    {
        if (this.messageTime <= this.messageThreshold && this.messageTime > 0.0f)
        {
            this.messageTime-=Time.deltaTime;
        }
        else
        {
            this.messageTime = 5.0f; //value outside the treshold.
            this.shiftMessageTextComponent.text = ""; //clear the screen.
        }
    }
    public void showDraftMessage()
    {
        this.messageTime = 1.0f;
        this.shiftMessageTextComponent.color = Color.yellow;
        this.shiftMessageTextComponent.text = "Drafting"; 
    }
    public void showShiftMessage()
    {
        CarScript.ShiftStatus shiftSts = playerComponent.getPlayerCar().getCarShiftStatus();
        this.messageTime = 1.5f;
        switch (shiftSts)
        {
            case CarScript.ShiftStatus.EarlyShift:
            {
                this.shiftMessageTextComponent.color = Color.black;
                this.shiftMessageTextComponent.text = "Early Shift"; 
                break;
            }
            case CarScript.ShiftStatus.GoodShift:
            {
                this.shiftMessageTextComponent.color = Color.blue;
                this.shiftMessageTextComponent.text = "Good Shift"; 
                break;
            }
            case CarScript.ShiftStatus.PerfectShift:
            {
                this.shiftMessageTextComponent.color = Color.green;
                this.shiftMessageTextComponent.text = "Perfect Shift"; 
                break;
            }
            case CarScript.ShiftStatus.OverShift:
            {
                this.shiftMessageTextComponent.color = Color.red;
                this.shiftMessageTextComponent.text = "Over Shift"; 
                break;
            }
            default:
            {
                this.shiftMessageTextComponent.color = Color.black;
                break;
            }
        }
    }
}
