using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.SceneManagement;

public class Spike : MonoBehaviour
{
    //checks collision
    private void OnTriggerEnter(Collider other)
    {
        //reports collision
        Debug.Log("Spike");
        //gets current scene
        Scene activeScene = SceneManager.GetActiveScene();
        //reloads current scene
        SceneManager.LoadScene(activeScene.name);
    }
}
