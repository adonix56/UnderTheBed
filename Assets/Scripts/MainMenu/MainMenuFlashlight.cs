using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuFlashlight : MonoBehaviour
{
    [SerializeField] public MainMenuManager manager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        manager.TurnOnObject(collision.gameObject.name);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        manager.TurnOffObject(collision.gameObject.name);
    }
}
