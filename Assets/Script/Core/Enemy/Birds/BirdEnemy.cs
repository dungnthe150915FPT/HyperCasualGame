using Assets.Script.Core.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdEnemy : BaseEnemy
{
    private float flySpeed;
    public float FlySpeed { get => flySpeed; set => flySpeed = value; }
}
