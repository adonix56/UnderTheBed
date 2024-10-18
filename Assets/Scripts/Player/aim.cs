using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aim : MonoBehaviour
{
    public GameObject light;
    private Vector3 mousePos;
    private bool lightOn = false;
    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        //gets current mouse position
        mousePos = Input.mousePosition;
        //gets rotation
        Vector3 rotation = mousePos - transform.position;

        //gets rotation and converts to degrees
        float rotationZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        //sets rotation based on point for cone
        transform.rotation = Quaternion.Euler(0, 0, rotationZ);

        if (Input.GetButtonDown("light") && lightOn == true)
        {
            lightOn = false;
            light.SetActive(false);
        }
        else if (Input.GetButtonDown("light") && lightOn == false)
        {
            lightOn = true;
            light.SetActive(true);
        }
    }
}
