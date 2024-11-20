using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoor: MonoBehaviour {
    [SerializeField]
    private GameObject door;

    private bool doorOpen;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            OpenDoor();
            Debug.Log("Entered");
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            CloseDoor();
            Debug.Log("Exited");
        }
    }

    private void OpenDoor() {
        doorOpen = true;
        door.GetComponent<Animator>().SetBool("isOpen", doorOpen);
    }

    private void CloseDoor() {
        doorOpen = false;
        door.GetComponent<Animator>().SetBool("isOpen", doorOpen);
    }
}
