using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private Transform play;
    [SerializeField] private Transform options;
    [SerializeField] private Transform about;

    public enum MainMenuButton { 
        None = -1, Play = 0, Options = 1, About = 2
    }

    public MainMenuButton GetClosestButton(Vector2 mousePosition)
    {
        Vector3 screenPos = Camera.main.ScreenToWorldPoint(mousePosition);
        float mousey = screenPos.y;
        float playy = play.position.y;
        float optionsy = options.position.y;
        float abouty = about.position.y;
        MainMenuButton ret = MainMenuButton.Play;
        float distance = Mathf.Abs(mousey - playy);
        if (Mathf.Abs(mousey - optionsy) < distance)
        {
            ret = MainMenuButton.Options;
            distance = Mathf.Abs(mousey - optionsy);
        }
        if (Mathf.Abs(mousey - abouty) < distance)
        {
            ret = MainMenuButton.About;
        }
        return ret;
    }
}
