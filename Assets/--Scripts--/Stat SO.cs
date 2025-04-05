using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Stat", menuName = "Character Stats")]
public class StatSO : ScriptableObject
{
    public int health;
    public int attack;
    public int defense;
    public int intelligence;
    public int willpower;
}
