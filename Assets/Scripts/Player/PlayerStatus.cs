using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPattern;

public class PlayerStatus : MonoBehaviour
{
    [field: SerializeField]
    [field: Range(0, 10)]
    public float WalkSpeed { get; set; }
    [field: SerializeField]
    [field: Range(0, 10)]
    public float RunSpeed { get; set; }

    [field: SerializeField]
    [field: Range(0, 10)]
    public float RotateSpeed { get; set; }

    public ObservableProperty<bool> IsAming { get; private set; } = new();
    public ObservableProperty<bool> IsMoving { get; private set; } = new();
    public ObservableProperty<bool> IsAttacking { get; private set; } = new();
}
