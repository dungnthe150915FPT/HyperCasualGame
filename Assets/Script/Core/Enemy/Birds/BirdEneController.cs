using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdEneController : EnemyController
{
    private BirdEnemy enemy;
    public new BirdEnemy Enemy { get => enemy; set => enemy = value; }
    public override void setupEnemy()
    {
        BirdEnemy enemy = new BirdEnemy();
        enemy.Id = "bird_ene_001";
        enemy.NameDisplay = "Blue Bird";
        enemy.MaxHealth = 100f;
        enemy.CurrentHealth = 100f;
        enemy.AttackDamage = 10f;
        enemy.AttackSpeed = 1f;
        enemy.AttackRange = 1f;
        enemy.MoveSpeed = 20f;
        enemy.JumpHeight = 1f;
        enemy.FlySpeed = 1f;
        setupEnemyController(enemy);
    }
}
