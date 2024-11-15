using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CharacterSO", menuName = "Scriptable Objects/Character", order = 1)]
public class CharacterSO : ScriptableObject
{
    public enum CharacterExpression
    {
        Neutral, Angry, Curious, Happy, Sad, Lazy
    }

    [Serializable]
    public struct ExpressionList {
        public CharacterExpression expression;
        public Sprite sprite;
    }

    public string characterName;
    public List<ExpressionList> characterExpressions = new List<ExpressionList>();

    public Sprite GetCharacterSprite(CharacterExpression expression = CharacterExpression.Neutral)
    {
        foreach (ExpressionList exp in characterExpressions)
        {
            if (exp.expression == expression) { 
                return exp.sprite;
            }
        }
        Debug.LogError("CharacterSO.GetCharacterSprite: Sprite does not exist for given Character Expression.");
        return null;
    }
}