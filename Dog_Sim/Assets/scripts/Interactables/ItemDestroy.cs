using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemDestroy : MonoBehaviour
{
    public ScoreManage ScoreManager;

    void Start(){
        GameObject manager = GameObject.Find("ScoreManager"); 
        ScoreManager = manager.GetComponent<ScoreManage>();
    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Ground"){
            ScoreManager.updateScore(100);
            Destroy(gameObject);
        }
    }
}
