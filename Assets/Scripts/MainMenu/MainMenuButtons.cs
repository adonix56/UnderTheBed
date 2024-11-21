using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private Transform play;
    [SerializeField] private Transform options;
    [SerializeField] private Transform about;

    public int GetClosestButton(Vector2 mousePosition)
    {
        Debug.Log($"{Camera.main.ScreenToWorldPoint(mousePosition)} compare to {play.position} and {options.position}");
        return 0;
    }
}
