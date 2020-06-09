using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

using ComplexGame;

public class VRInteract : MonoBehaviour
{
    
    public OVRGrabbable grabbable;
    
    private bool isGrabbed = false;
    private Rigidbody rigidBody;
    private Interact interact;


    void Start()
    {
        grabbable = GetComponent<OVRGrabbable>();
        Assert.IsNotNull(grabbable);

        rigidBody = GetComponent<Rigidbody>();
        Assert.IsNotNull(rigidBody);

        interact = GetComponent<Interact>();
        Assert.IsNotNull(interact);
    }

    void FixedUpdate()
    {
        CheckIsGrabbed();
    }


    void CheckIsGrabbed()
    {
        bool grab_status = grabbable.isGrabbed;

        if(isGrabbed == false && grab_status == true)
        {
            isGrabbed = true;
            interact.OnMouseDown();
            return;
        }

        if(isGrabbed == true && grab_status == false)
        {
            //rotation interacta ayarlanmıyorsa onu da ayarlamamız lazım burada
            isGrabbed = false;
            interact.OnMouseUp();
            rigidBody.velocity = Vector3.zero;
            rigidBody.angularVelocity = Vector3.zero;
            return;
        }
    }

}
