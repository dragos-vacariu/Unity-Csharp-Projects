using UnityEngine;
using UnityEngine.UI;

public class GearScript : MonoBehaviour
{
    Rigidbody rigidBodyComponent;
    PlayerScript playerComponent;
    Text gearTextComponent;
    // Start is called before the first frame update
    void Start()
    {
        this.playerComponent = GameObject.Find("Player").GetComponent<PlayerScript>();
        this.gearTextComponent = GetComponent<UnityEngine.UI.Text>();
    }

    // Update is called once per frame
    void Update()
    {
        this.gearTextComponent.text = "Gear: " + playerComponent.getPlayerCar().getCarCurrentGear();
        this.changeGearColor();
        
    }
    void changeGearColor()
    {
        CarScript.ShiftStatus shiftSts = playerComponent.getPlayerCar().getCarShiftStatus();
        switch (shiftSts)
        {
            case CarScript.ShiftStatus.EarlyShift:
            {
                this.gearTextComponent.color = Color.black;
                break;
            }
            case CarScript.ShiftStatus.GoodShift:
            {
                this.gearTextComponent.color = Color.blue;
                break;
            }
            case CarScript.ShiftStatus.PerfectShift:
            {
                this.gearTextComponent.color = Color.green;
                break;
            }
            case CarScript.ShiftStatus.OverShift:
            {
                this.gearTextComponent.color = Color.red;
                break;
            }
            default:
            {
                this.gearTextComponent.color = Color.black;
                break;
            }
        }
    }
    
}
