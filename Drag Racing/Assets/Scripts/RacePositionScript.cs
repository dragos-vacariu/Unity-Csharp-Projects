using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RacePositionScript : MonoBehaviour
{
    List <Rigidbody> opponentsList;
    Text racePositionTextComponent;
    // Start is called before the first frame update
    void Start()
    {
        this.opponentsList = new List <Rigidbody>();
        if (GameObject.Find("Player").GetComponent<Rigidbody>() != null)
        {
            opponentsList.Add(GameObject.Find("Player").GetComponent<Rigidbody>());
        }
        if (GameObject.Find("AIopponent1") != null && GameObject.Find("AIopponent1").GetComponent<Rigidbody>() != null)
        {
            opponentsList.Add(GameObject.Find("AIopponent1").GetComponent<Rigidbody>());
        }
        if (GameObject.Find("AIopponent2") != null && GameObject.Find("AIopponent2").GetComponent<Rigidbody>() != null)
        {
            opponentsList.Add(GameObject.Find("AIopponent2").GetComponent<Rigidbody>());
        }
        if (GameObject.Find("AIopponent3") != null && GameObject.Find("AIopponent3").GetComponent<Rigidbody>() != null)
        {
            opponentsList.Add(GameObject.Find("AIopponent3").GetComponent<Rigidbody>());
        }
        this.racePositionTextComponent = GetComponent<UnityEngine.UI.Text>();
        this.racePositionTextComponent.text = "Position: " + this.getPlayerPosition(GameObject.Find("Player").GetComponent<Rigidbody>());
    }
    
    public int getPlayerPosition(Rigidbody somePlayer)
    {
        int playerPosition = opponentsList.Count;
        if (opponentsList.IndexOf(somePlayer) >= 0)
        {
            foreach (Rigidbody rb in opponentsList)
            {
                if(somePlayer.transform.position.z > rb.transform.position.z)
                {
                    playerPosition--;
                }
            }
            return playerPosition;
        }
        else
            return -1;
    }
    public bool draftingBehindOpponent(Rigidbody somePlayer)
    {
        bool somePlayerIsDrafting = false; 
        if (opponentsList.IndexOf(somePlayer) >= 0 && this.getPlayerPosition(somePlayer) > 1)
        {
            foreach (Rigidbody rb in opponentsList)
            {
                if (rb != somePlayer)
                {
                    if (somePlayer.transform.position.z >= rb.position.z - 50.0f && 
                                                            somePlayer.transform.position.z < rb.position.z)
                    {
                        if(somePlayer.transform.position.x <= (rb.transform.position.x + (rb.transform.localScale.x/2)) &&
                            somePlayer.transform.position.x >= (rb.transform.position.x - (rb.transform.localScale.x/2))  )
                        {
                            somePlayerIsDrafting = true; // already drafting.
                            break;
                        }
                    }
                }
            }
            return somePlayerIsDrafting;
        }
        else
            return somePlayerIsDrafting;
    }
    public List <Rigidbody> getOpponentList()
    {
        return this.opponentsList;
    }
    // Update is called once per frame
    void Update()
    {
        this.racePositionTextComponent.text = "Position: " + this.getPlayerPosition(GameObject.Find("Player").GetComponent<Rigidbody>());
    }


}
