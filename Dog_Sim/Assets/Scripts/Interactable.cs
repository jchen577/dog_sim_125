using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// code from https://www.youtube.com/watch?v=gPPGnpV1Y1c&ab_channel=NattyGameDev

public class Interactable: MonoBehaviour {
    public string promptMessage;

    public void BaseInteract() {
        Interact();
    }

    protected virtual void Interact() {

    }
}
