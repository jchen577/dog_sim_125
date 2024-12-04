using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemDestroy : MonoBehaviour
{
    public ScoreManage ScoreManager;
    public bool hasTouchedPlayer = false;

    void Start(){
        GameObject manager = GameObject.Find("ScoreManager"); 
        ScoreManager = manager.GetComponent<ScoreManage>();
    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player"){
            hasTouchedPlayer = true;
        }
        else{
            ItemDestroy currObj = collision.gameObject.GetComponent<ItemDestroy>();
            if(currObj!=null){
                if(currObj.hasTouchedPlayer){
                    hasTouchedPlayer=true;
                }
            }
        }
        
        if(collision.transform.tag == "Ground" && hasTouchedPlayer){
            ScoreManager.updateScore(100);
            Destroy(gameObject);
        }
    }
}
