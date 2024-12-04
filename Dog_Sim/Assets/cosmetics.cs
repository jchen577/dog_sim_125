using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cosmetics : MonoBehaviour
{
    public static bool yellow, orange, green, toggleHat;
    public GameObject hat, dog;
    public Renderer meshRenderer;
    public Material material;
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = dog.GetComponent<Renderer>();
        material = meshRenderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        hat.SetActive(toggleHat);

    }

    public void yellowColor()
    {
        yellow = true;
        orange = false;
        green = false;
    }
    public void orangeColor()
    {
        orange = true;
        yellow = false;
        green = false;
    }
    public void greenColor()
    {
        green = true;
        yellow = false;
        orange = false;
    }
    public void defaultColor()
    {
        yellow = false;
        green = false;
        orange = false;
    }
    public void hatEnabled()
    {
        toggleHat = true;
    }
    public void hetDisabled()
    {
        toggleHat = false;
    }
}
