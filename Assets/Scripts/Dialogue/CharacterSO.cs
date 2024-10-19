using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CharacterSO", menuName = "Scriptable Objects/Character", order = 1)]
public class CharacterSO : ScriptableObject
{
    public string characterName;
    public Sprite Neutral;
}