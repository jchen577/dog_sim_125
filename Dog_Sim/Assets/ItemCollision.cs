using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemCollision : MonoBehaviour
{
    public bool isGrounded;
    public TMP_Text score;
    public static int scoreNumber;
    public int scoreValue = 1;
    public float boundary = 0.75f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(transform.position.y);
        if(transform.position.y < boundary && !isGrounded){
            Debug.Log("cool");
            isGrounded = true;
            scoreNumber += scoreValue;
            score.text = scoreNumber.ToString();
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {

    }
}
