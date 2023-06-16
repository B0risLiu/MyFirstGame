using Assets.Scripts.Enum;
using System;
using UnityEngine;

[Serializable]
public class EnemyNameAndAmount
{
    [SerializeField] private EnemyName _name;
    [SerializeField] private int _amount;

    public EnemyName Name => _name;
    public int Amount => _amount;
}
