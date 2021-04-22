using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    Transform CameraTransformComponent;
    Vector3 CameraInitialPosition;
    public Vector3 CameraInitialRotationVector;
    Vector3 playerCarInitialPosition;
    // Start is called before the first frame update
    enum CameraLook
    {
        Front,
        Back,
        Left,
        Right,
    }
    CameraLook cameraLook;
    
    enum CameraType
    {
        Normal,
        Far,
        Hood,
        Bumper,
    }
    CameraType cameraType;
    
    void Start()
    {
        this.CameraTransformComponent = GetComponent<Transform>();
        this.CameraInitialPosition = this.CameraTransformComponent.position;
        this.CameraInitialRotationVector = this.CameraTransformComponent.rotation.eulerAngles;
        this.playerCarInitialPosition = GameObject.Find("Player").transform.position;
        this.cameraLook = CameraLook.Front;
        this.cameraType = CameraType.Normal;
    }

    // Update is called once per frame
    void Update()
    {
        //this.CameraFollowingPlayer(); //not needed unless the camera is not a child of the Player
        this.LookAround();
        this.changeCamera();
    }
    
    void CameraFollowingPlayer()
    {
        Vector3 newCameraPosition = CameraTransformComponent.position;
        newCameraPosition.z = GameObject.Find("Player").transform.position.z + (this.CameraInitialPosition.z - this.playerCarInitialPosition.z);
        newCameraPosition.x = GameObject.Find("Player").transform.position.x;
        this.CameraTransformComponent.position = newCameraPosition; 
    }
    
    void changeCamera()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (this.cameraType == CameraType.Normal)
            {
                this.cameraType = CameraType.Far;
            }
            else if(this.cameraType == CameraType.Far)
            {
                this.cameraType = CameraType.Hood;
            }
            else if(this.cameraType == CameraType.Hood)
            {
                this.cameraType = CameraType.Bumper;
            }
            else
            {
                this.cameraType = CameraType.Normal;
            }
            //This will make sure the correct camera is displayed according to the side the player is looking.
            if (this.cameraLook==CameraLook.Back)
            {
                this.setCameraLookBehind();
            }
            else if(this.cameraLook==CameraLook.Front)
            {
                this.setCameraLookForward();
            }
            else if(this.cameraLook==CameraLook.Left)
            {
                this.setCameraLookLeft();
            }
            else if(this.cameraLook==CameraLook.Right)
            {
                this.setCameraLookRight();
            }
        }
    }
    void FarCameraLookForward()
    {
        GameObject gameobj = GameObject.Find("Player");
        Vector3 newCameraPosition = gameobj.transform.position;
        newCameraPosition.y += 2.9f*gameobj.transform.localScale.y;
        newCameraPosition.z -= 2.5f*gameobj.transform.localScale.z;
        this.CameraTransformComponent.position = newCameraPosition;
    }
    void FarCameraLookBehind()
    {
        GameObject gameobj = GameObject.Find("Player");
        Vector3 newCameraPosition = gameobj.transform.position;
        newCameraPosition.y += 2.0f*gameobj.transform.localScale.y;
        newCameraPosition.z += 3.5f*gameobj.transform.localScale.z;
        this.CameraTransformComponent.position = newCameraPosition;
    }
    void FarCameraLookLeft()
    {
        GameObject gameobj = GameObject.Find("Player");
        Vector3 newCameraPosition = gameobj.transform.position;
        newCameraPosition.y += 2.9f*gameobj.transform.localScale.y;
        newCameraPosition.x += 3.0f*gameobj.transform.localScale.z;
        this.CameraTransformComponent.position = newCameraPosition;
    }
    void FarCameraLookRight()
    {
        GameObject gameobj = GameObject.Find("Player");
        Vector3 newCameraPosition = gameobj.transform.position;
        newCameraPosition.y += 2.9f*gameobj.transform.localScale.y;
        newCameraPosition.x -= 3.0f*gameobj.transform.localScale.z;
        this.CameraTransformComponent.position = newCameraPosition;
    }
    void NormalCameraLookForward()
    {
        GameObject gameobj = GameObject.Find("Player");
        Vector3 newCameraPosition = gameobj.transform.position;
        newCameraPosition.y += 1.9f*gameobj.transform.localScale.y;
        newCameraPosition.z -= 1.8f*gameobj.transform.localScale.z;
        this.CameraTransformComponent.position = newCameraPosition;
    }
    void NormalCameraLookBehind()
    {
        GameObject gameobj = GameObject.Find("Player");
        Vector3 newCameraPosition = gameobj.transform.position;
        newCameraPosition.y += 1.2f*gameobj.transform.localScale.y;
        newCameraPosition.z += 2.5f*gameobj.transform.localScale.z;
        this.CameraTransformComponent.position = newCameraPosition;
    }
    void NormalCameraLookLeft()
    {
        GameObject gameobj = GameObject.Find("Player");
        Vector3 newCameraPosition = gameobj.transform.position;
        newCameraPosition.y += 1.9f*gameobj.transform.localScale.y;
        newCameraPosition.x += 2.5f*gameobj.transform.localScale.z;
        this.CameraTransformComponent.position = newCameraPosition;
    }
    void NormalCameraLookRight()
    {
        GameObject gameobj = GameObject.Find("Player");
        Vector3 newCameraPosition = gameobj.transform.position;
        newCameraPosition.y += 1.9f*gameobj.transform.localScale.y;
        newCameraPosition.x -= 2.5f*gameobj.transform.localScale.z;
        this.CameraTransformComponent.position = newCameraPosition;
    }
    void HoodCameraLookForward()
    {
        GameObject gameobj = GameObject.Find("Player");
        Vector3 newCameraPosition = gameobj.transform.position;
        newCameraPosition.y += 0.9f*gameobj.transform.localScale.y;
        newCameraPosition.z -= 0.1f*gameobj.transform.localScale.z;
        this.CameraTransformComponent.position = newCameraPosition;
    }
    void HoodCameraLookLeft()
    {
        GameObject gameobj = GameObject.Find("Player");
        Vector3 newCameraPosition = gameobj.transform.position;
        newCameraPosition.y += 0.5f*gameobj.transform.localScale.y;
        newCameraPosition.z += 0.5f*gameobj.transform.localScale.z;
        newCameraPosition.x += 0.1f*gameobj.transform.localScale.z;
        this.CameraTransformComponent.position = newCameraPosition;
    }
    void HoodCameraLookRight()
    {
        GameObject gameobj = GameObject.Find("Player");
        Vector3 newCameraPosition = gameobj.transform.position;
        newCameraPosition.y += 0.5f*gameobj.transform.localScale.y;
        newCameraPosition.z += 0.5f*gameobj.transform.localScale.z;
        newCameraPosition.x -= 0.1f*gameobj.transform.localScale.z;
        this.CameraTransformComponent.position = newCameraPosition;
    }
    void BumperCameraLookForward()
    {
        GameObject gameobj = GameObject.Find("Player");
        Vector3 newCameraPosition = gameobj.transform.position;
        newCameraPosition.y += 0.5f*gameobj.transform.localScale.y;
        newCameraPosition.z += 0.5f*gameobj.transform.localScale.z;
        this.CameraTransformComponent.position = newCameraPosition;
    }
    void BumperCameraLookBehind()
    {
        GameObject gameobj = GameObject.Find("Player");
        Vector3 newCameraPosition = gameobj.transform.position;
        newCameraPosition.y += 0.2f*gameobj.transform.localScale.y;
        newCameraPosition.z += 0.7f*gameobj.transform.localScale.z;
        this.CameraTransformComponent.position = newCameraPosition;
    }
    void LookAround()
    {
        this.LookLeft();
        this.LookRight();
        this.LookBack();
    }
    void LookBack()
    {
        if(Input.GetKeyDown(KeyCode.Tab) && this.cameraLook == CameraLook.Front)
        {
            this.setCameraLookBehind();
            this.CameraTransformComponent.Rotate(0,-180,0);
            this.cameraLook = CameraLook.Back;
        }
        else if (Input.GetKeyUp(KeyCode.Tab) && this.cameraLook == CameraLook.Back)
        {
            this.setCameraLookForward();
            this.CameraTransformComponent.Rotate(0,180,0);
            this.cameraLook = CameraLook.Front;
        }
    }
    void LookLeft()
    {
        if(Input.GetKeyDown(KeyCode.Q) && this.cameraLook == CameraLook.Front)
        {
            this.CameraTransformComponent.Rotate(0,-90,0);
            //after rotation X degrees will become -Z degrees, but adding the initial X value on Z to bring it to 0 degrees,
            //(the value which was before the transformation)
            this.CameraTransformComponent.Rotate(0,0,this.CameraInitialRotationVector.x);
            this.cameraLook = CameraLook.Left;
            this.setCameraLookLeft();
        }
        else if (Input.GetKeyUp(KeyCode.Q) && this.cameraLook == CameraLook.Left)
        {
            this.CameraTransformComponent.Rotate(0,90,0);
            //after rotation undo the changes suffered on X axis with the previous rotation
            this.CameraTransformComponent.Rotate(this.CameraInitialRotationVector.x,0,0);
            this.cameraLook = CameraLook.Front;
            this.setCameraLookForward();
        }
    }
    void LookRight()
    {
        if(Input.GetKeyDown(KeyCode.E) && this.cameraLook == CameraLook.Front)
        {
            this.CameraTransformComponent.Rotate(0,90,0);
            //after rotation X degrees will become Z degrees, but multiplying initial X to -1 and then adding it to Z 
            //will bring Z back to the value it was before,
            this.CameraTransformComponent.Rotate(0,0,this.CameraInitialRotationVector.x*-1);
            this.cameraLook = CameraLook.Right;
            this.setCameraLookRight();
        }
        else if (Input.GetKeyUp(KeyCode.E) && this.cameraLook == CameraLook.Right)
        {
            this.CameraTransformComponent.Rotate(0,-90,0);
            //after rotation undo the changes suffered on X axis with the previous rotation
            this.CameraTransformComponent.Rotate(this.CameraInitialRotationVector.x,0,0);
            this.cameraLook = CameraLook.Front;
            this.setCameraLookForward();
        }
    }
    void setCameraLookBehind()
    {
        if (this.cameraType == CameraType.Normal)
        {
            this.NormalCameraLookBehind();
        }
        else if (this.cameraType == CameraType.Far)
        {
            this.FarCameraLookBehind();
        }
        else if (this.cameraType == CameraType.Hood)
        {
            //bumper camera look behind exactly from the hood.
            this.BumperCameraLookForward();
        }
        else if (this.cameraType == CameraType.Bumper)
        {
            this.BumperCameraLookBehind();
        }
    }
    void setCameraLookLeft()
    {
        if (this.cameraType == CameraType.Normal)
        {
            this.NormalCameraLookLeft();
        }
        else if (this.cameraType == CameraType.Far)
        {
            this.FarCameraLookLeft();
        }
        else if (this.cameraType == CameraType.Hood)
        {
            this.HoodCameraLookLeft();
        }
        else if (this.cameraType == CameraType.Bumper)
        {
            //The bumper camera didn't need an adaptation for looking on left side, it is placed perfectly as it is.
            this.BumperCameraLookForward();
        }
    }
    void setCameraLookRight()
    {
        if (this.cameraType == CameraType.Normal)
        {
            this.NormalCameraLookRight();
        }
        else if (this.cameraType == CameraType.Far)
        {
            this.FarCameraLookRight();
        }
        else if (this.cameraType == CameraType.Hood)
        {
            this.HoodCameraLookRight();
        }
        else if (this.cameraType == CameraType.Bumper)
        {
            //The bumper camera didn't need an adaptation for looking on right side, it is placed perfectly as it is.
            this.BumperCameraLookForward();
        }
    }
    void setCameraLookForward()
    {
        if (this.cameraType == CameraType.Normal)
        {
            this.NormalCameraLookForward();
        }
        else if (this.cameraType == CameraType.Far)
        {
            this.FarCameraLookForward();
        }
        else if (this.cameraType == CameraType.Hood)
        {
            this.HoodCameraLookForward();
        }
        else if (this.cameraType == CameraType.Bumper)
        {
            this.BumperCameraLookForward();
        }
    }
}