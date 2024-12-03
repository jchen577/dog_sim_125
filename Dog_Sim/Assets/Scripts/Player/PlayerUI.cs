using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// code from https://www.youtube.com/watch?v=gPPGnpV1Y1c&ab_channel=NattyGameDev

public class PlayerUI: MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI promptText;

    // Update is called once per frame
    public void UpdateText(string promptMessage) {
        promptText.text = promptMessage;
    }
}
