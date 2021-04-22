using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIopponent1Script : MonoBehaviour
{
    Rigidbody rigidBodyComponent;
    CarScript aiCar;
    enum AI_Difficulty
    {
        Easy,
        Medium,
        Hard,
    }
    [SerializeField] AI_Difficulty ai_difficulty;

    uint AI_Shift_Decision; 
    uint AI_Draft_Decision;
    // Start is called before the first frame update
    enum AI_Driving_Action
    {
        NA,
        Return_To_Road,
        Overpass_Left,
        Overpass_Right,
    }
    AI_Driving_Action ai_action; //this will be initialized from Unity
    static float outOfRoadFactor; // this variable will be used to figure out whether the AI is going outside the road
    void Start()
    {
        this.aiCar = this.gameObject.transform.GetChild(0).GetComponent<CarScript>();
        this.rigidBodyComponent = GetComponent<Rigidbody>();
        this.AI_Shift_Decision = getRandDecision();
        this.AI_Draft_Decision = getRandDecision();
        this.ai_action = AI_Driving_Action.NA;
        outOfRoadFactor = (GameObject.Find("Street").GetComponent<Transform>().localScale.x/2) - this.rigidBodyComponent.transform.localScale.x/2 - 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        this.AI_Thinking();
    }
    void FixedUpdate()
    {
        this.aiCar.Accelerate();
        this.aiCar.RaceFinishedCarStop();
        this.aiCar.gearingThreshold();
        this.AI_Shift_Processing();
        this.AI_Looking_To_Draft();
        this.getBackOnTheRoad();
        this.checkAvoidCollision();
    }
    void AI_Shift_Processing()
    {
        if (this.ai_difficulty == AI_Difficulty.Easy)
        {
            this.Easy_Difficulty_Shift_Processing();
        }
        else if (this.ai_difficulty == AI_Difficulty.Medium)
        {
            this.Medium_Difficulty_Shift_Processing();
        }
        else if (this.ai_difficulty == AI_Difficulty.Hard)
        {
            this.Hard_Difficulty_Shift_Processing();
        }
    }
    void checkAvoidCollision()
    {
        List <Rigidbody> opponentsList = GameObject.Find("RacePosition").GetComponent<RacePositionScript>().getOpponentList();
        foreach (Rigidbody rb in opponentsList)
        {
            if (rb != this.rigidBodyComponent)
            {
               //Checking if the line is blocked by another opponent
               if ( (this.rigidBodyComponent.transform.position.x > rb.transform.position.x - rb.transform.localScale.x ) && 
                    (this.rigidBodyComponent.transform.position.x < rb.transform.position.x + rb.transform.localScale.x ) || 
                    this.ai_action == AI_Driving_Action.Overpass_Right || this.ai_action == AI_Driving_Action.Overpass_Left)
               {
                    //Check if you are right behind that opponent.
                    if ( (this.rigidBodyComponent.transform.position.z + (this.rigidBodyComponent.transform.localScale.z/2)) < (rb.transform.position.z- rb.transform.localScale.z/2)&&
                       (this.rigidBodyComponent.transform.position.z + (this.rigidBodyComponent.transform.localScale.z/2)) >= (rb.transform.position.z- rb.transform.localScale.z/2) - 10)
                    {
                        //If you can overpass the opponent through your right side.
                        if(rb.transform.position.x + ((rb.transform.localScale.x/2) + this.rigidBodyComponent.transform.localScale.x) < outOfRoadFactor && 
                           this.rigidBodyComponent.transform.position.x-(this.rigidBodyComponent.transform.localScale.x/2) < (rb.transform.position.x + (rb.transform.localScale.x))
                           && this.ai_action != AI_Driving_Action.Overpass_Left)
                        {
                            //Overpass through the right side
                            this.aiCar.TurnRight();
                            if (this.ai_action != AI_Driving_Action.Overpass_Right)
                            {
                                this.ai_action = AI_Driving_Action.Overpass_Right;
                            }
                        }
                        //If you can overpass the opponent through your left side.
                        else if (rb.transform.position.x - ((rb.transform.localScale.x/2) - this.rigidBodyComponent.transform.localScale.x) > (outOfRoadFactor*-1) && 
                           this.rigidBodyComponent.transform.position.x -(this.rigidBodyComponent.transform.localScale.x/2)  > (rb.transform.position.x - (rb.transform.localScale.x))
                           && this.ai_action != AI_Driving_Action.Overpass_Right)
                        {
                            //Overpass through the left side
                            this.aiCar.TurnLeft();
                            if (this.ai_action != AI_Driving_Action.Overpass_Left)
                            {
                                this.ai_action = AI_Driving_Action.Overpass_Left;
                            }
                        }
                        //If you got this far, it means either you overpassed, or still overpassing, or your opponent got too ahead of you.
                        else if(this.ai_action == AI_Driving_Action.Overpass_Right || this.ai_action == AI_Driving_Action.Overpass_Left)
                        {
                            //Straighten your wheel, and don't curve anymore, the overpass maneuver is over.
                            this.rigidBodyComponent.transform.Rotate(0,this.rigidBodyComponent.transform.rotation.eulerAngles.y*-1,0);
                            this.ai_action = AI_Driving_Action.NA;
                        }
                        //If you are here about to execute the else statement, it means you have nowhere to overpass
                        // you are surrounded by opponents or whatever, the road is blocked.                        
                        else
                        {
                            //Decrease the car speed, brake, so that you won't hit.
                            this.aiCar.Decelerate(0.05f);
                        }
                    }
               }
            }
        }
    }
    bool AI_Draft_Processing()
    {
        if (this.ai_difficulty == AI_Difficulty.Easy)
        {
            if (this.AI_Draft_Decision < 40) // 40% chances to initiate DraftManeuver on Easy Difficulty.
            {
                return true;
            }
        }
        else if (this.ai_difficulty == AI_Difficulty.Medium)
        {
            if (this.AI_Draft_Decision < 55) // 55% chances to initiate DraftManeuver on Medium Difficulty.
            {
                return true;
            }
        }
        else if (this.ai_difficulty == AI_Difficulty.Hard)
        {
            if (this.AI_Draft_Decision < 70) // 70% chances to initiate DraftManeuver on Hard Difficulty.
            {
                return true;
            }
        }
        return false;
    }
    void AI_Looking_To_Draft()
    {
        RacePositionScript racePosScript = GameObject.Find("RacePosition").GetComponent<RacePositionScript>();
        List <Rigidbody> opponentsList = racePosScript.getOpponentList();
        if(racePosScript.getPlayerPosition(this.rigidBodyComponent) > 1 && this.ai_action != AI_Driving_Action.Return_To_Road)
        {
            foreach (Rigidbody rb in opponentsList)
            {
                if (rb != this.rigidBodyComponent)
                {
                    if (this.rigidBodyComponent.transform.position.z + 10 < rb.position.z &&
                           this.rigidBodyComponent.transform.position.z >= rb.position.z - 50.0f)
                    {
                        //IF entered here, it means you are not drafting but you could have been.
                        
                        //Check if the opponent ahead is on the road.
                        if (rb.transform.position.x - (rb.transform.localScale.x/2) > (outOfRoadFactor*-1) && 
                            rb.transform.position.x - (rb.transform.localScale.x/2) < outOfRoadFactor)
                        {
                            if (this.AI_Draft_Processing() && anyOpponentInTheWay(opponentsList,rb) == false)
                            {
                                this.initiatingDraftManeuver(rb);
                                break;
                            }
                        }
                    }
                }
            }

        }      
    }
    void AI_Thinking()
    {
        //This function will make opponents look like thinking about Drafting Maneuver.
        if ((GameObject.Find("RaceTime").GetComponent<RaceTimeScript>().getTime()) % 3 > 1.0f
        && (GameObject.Find("RaceTime").GetComponent<RaceTimeScript>().getTime()) % 3 <  1.3f)
        {
            this.AI_Draft_Decision = getRandDecision();
        }
    }
    bool anyOpponentInTheWay(List <Rigidbody> opponentsList, Rigidbody draftableOpponent)
    {
        foreach (Rigidbody rb in opponentsList)
        {
            if (rb != this.rigidBodyComponent)
            {
                    if (this.rigidBodyComponent.transform.position.z > rb.position.z - 10 &&
                           this.rigidBodyComponent.transform.position.z < rb.position.z + 10)
                   {
                       if (draftableOpponent != rb)
                       {
                           //Checking if no one is drafting the draftable opponent.
                           if (rb.transform.position.x > (draftableOpponent.transform.position.x -
                           draftableOpponent.transform.localScale.x) && rb.transform.position.x < (draftableOpponent.transform.position.x +
                           draftableOpponent.transform.localScale.x))
                           {
                               return true;
                           }
                           
                           //Checking if the line is blocked by another opponent
                           if ((rb.transform.position.x > draftableOpponent.transform.position.x && 
                                             rb.transform.position.x < this.rigidBodyComponent.transform.position.x) || 
                               (rb.transform.position.x < draftableOpponent.transform.position.x && 
                                             rb.transform.position.x > this.rigidBodyComponent.transform.position.x))
                           {
                               return true;
                           }
                           
                       }
                   }
            }
        }
        return false;
    }
    void initiatingDraftManeuver(Rigidbody rb)
    { 
        if (rb.transform.position.x - (rb.transform.localScale.x/2) > this.rigidBodyComponent.transform.position.x)
        {
            this.aiCar.TurnRight();
        }
        else if (rb.transform.position.x+(rb.transform.localScale.x/2) < this.rigidBodyComponent.transform.position.x)
        {
            this.aiCar.TurnLeft();
        }
        else if (this.rigidBodyComponent.transform.rotation.eulerAngles.y != 0)
        {
            this.rigidBodyComponent.transform.Rotate(0,this.rigidBodyComponent.transform.rotation.eulerAngles.y*-1, 0);
        }
    }
    void getBackOnTheRoad()
    {
        //The road is placed on X = 0, and it's scaled to 28, so it goes from 14.0f to -14.0f
        //outOfRoadFactor is calculated in the start() function, taking that into account.
        if (this.rigidBodyComponent.transform.position.x > outOfRoadFactor || this.rigidBodyComponent.transform.position.x < outOfRoadFactor*-1
            || this.ai_action == AI_Driving_Action.Return_To_Road)
        {
            if (this.rigidBodyComponent.transform.position.x > outOfRoadFactor) 
            {
                this.aiCar.TurnLeft();
                if (this.ai_action != AI_Driving_Action.Return_To_Road)
                {
                    this.ai_action = AI_Driving_Action.Return_To_Road;
                }
            }
            else if (this.rigidBodyComponent.transform.position.x < outOfRoadFactor*-1)
            {
                this.aiCar.TurnRight();
                if (this.ai_action != AI_Driving_Action.Return_To_Road)
                {
                    this.ai_action = AI_Driving_Action.Return_To_Road;
                }
            }
            else if (this.rigidBodyComponent.transform.rotation.eulerAngles.y != 0)
            {
                //you got back on the road, straighten your steering wheel.
                this.rigidBodyComponent.transform.Rotate(0,this.rigidBodyComponent.transform.rotation.eulerAngles.y*-1,0);
                this.ai_action = AI_Driving_Action.NA;
            }   
        }
    }
    void Easy_Difficulty_Shift_Processing()
    {
        if (this.aiCar.getCarShiftStatus() == CarScript.ShiftStatus.EarlyShift)
        {
            if (this.AI_Shift_Decision >= 0 && this.AI_Shift_Decision < 20) // 20% chances that AI will shift early.
            {
                this.aiCar.switchGear();
                this.AI_Shift_Decision = getRandDecision();
            }
        }
        else if (this.aiCar.getCarShiftStatus() == CarScript.ShiftStatus.GoodShift)
        {
            if (this.AI_Shift_Decision >= 20 && this.AI_Shift_Decision < 60) // 40%  chances that AI will shift good.
            {
                this.aiCar.switchGear();
                this.AI_Shift_Decision = getRandDecision();
            }
        }
        else if (this.aiCar.getCarShiftStatus() == CarScript.ShiftStatus.PerfectShift)
        {
            if (this.AI_Shift_Decision >= 60 && this.AI_Shift_Decision < 80) // 20% chances that AI will shift perfect.
            {
                this.aiCar.switchGear();
                this.AI_Shift_Decision = getRandDecision();
            }
        }
        else if (this.aiCar.getCarShiftStatus() == CarScript.ShiftStatus.OverShift) // the remaining 20% chances that AI will shift over.
        {
            this.aiCar.switchGear();
            this.AI_Shift_Decision = getRandDecision();
        }
    }
    void Medium_Difficulty_Shift_Processing()
    {
        if (this.aiCar.getCarShiftStatus() == CarScript.ShiftStatus.EarlyShift)
        {
            if (this.AI_Shift_Decision >= 0 && this.AI_Shift_Decision < 15) // 15% chances that AI will shift early.
            {
                this.aiCar.switchGear();
                this.AI_Shift_Decision = getRandDecision();
            }
        }
        else if (this.aiCar.getCarShiftStatus() == CarScript.ShiftStatus.GoodShift)
        {
            if (this.AI_Shift_Decision >= 15 && this.AI_Shift_Decision < 50) // 35% chances that AI will shift good.
            {
                this.aiCar.switchGear();
                this.AI_Shift_Decision = getRandDecision();
            }
        }
        else if (this.aiCar.getCarShiftStatus() == CarScript.ShiftStatus.PerfectShift)
        {
            if (this.AI_Shift_Decision >= 50 && this.AI_Shift_Decision < 85) // 35% chances that AI will shift perfect.
            {
                this.aiCar.switchGear();
                this.AI_Shift_Decision = getRandDecision();
            }
        }
        else if (this.aiCar.getCarShiftStatus() == CarScript.ShiftStatus.OverShift) // the remaining 15% chances that AI will shift over.
        {
            this.aiCar.switchGear();
            this.AI_Shift_Decision = getRandDecision();
        }
    }
    void Hard_Difficulty_Shift_Processing()
    {
        if (this.aiCar.getCarShiftStatus() == CarScript.ShiftStatus.EarlyShift)
        {
            if (this.AI_Shift_Decision >=0 && this.AI_Shift_Decision < 15) // 15% chances AI will shift early.
            {
                this.aiCar.switchGear();
                this.AI_Shift_Decision = getRandDecision();
            }
        }
        else if (this.aiCar.getCarShiftStatus() == CarScript.ShiftStatus.GoodShift)
        {
            if (this.AI_Shift_Decision >= 15 && this.AI_Shift_Decision < 35) // 20% chances that AI will shift good.
            {
                this.aiCar.switchGear();
                this.AI_Shift_Decision = getRandDecision();
            }
        }
        else if (this.aiCar.getCarShiftStatus() == CarScript.ShiftStatus.PerfectShift)
        {
            if (this.AI_Shift_Decision >= 35 && this.AI_Shift_Decision < 85) // 50% chances that AI will shift perfect.
            {
                this.aiCar.switchGear();
                this.AI_Shift_Decision = getRandDecision();
            }
        }
        else if (this.aiCar.getCarShiftStatus() == CarScript.ShiftStatus.OverShift) // the remaining 15% chances that AI will shift over.
        {
            this.aiCar.switchGear();
            this.AI_Shift_Decision = getRandDecision();
        }
    }
    static uint getRandDecision(int max=100)
    {
        var random = new System.Random();
        return (uint)random.Next(max);
    }
    
}
