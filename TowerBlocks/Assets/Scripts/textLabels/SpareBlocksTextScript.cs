using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpareBlocksTextScript : MonoBehaviour
{
	TMPro.TMP_Text spareBlocksTextComponent;
    // Start is called before the first frame update
    void Start()
    {
        this.spareBlocksTextComponent = this.gameObject.GetComponent<TMPro.TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void updateSpareBlocks(int spareBlocks)
	{
		this.spareBlocksTextComponent = this.gameObject.GetComponent<TMPro.TMP_Text>(); // initialize this here just in case this faction gets called before the HUD is displayed
		//the start() function is called when this gameObject is displayed on the screen for the first time.
		this.spareBlocksTextComponent.text = "SpareBlocks: " + spareBlocks;
	}
}
