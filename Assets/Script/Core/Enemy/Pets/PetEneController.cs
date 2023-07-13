using Assets.Script.Core.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetEneController : EnemyController
{
    private PetEnemy enemy;
    public new PetEnemy Enemy { get => enemy; set => enemy = value; }
    public override void setupEnemy()
    {
        PetEnemy enemy = new PetEnemy();
        enemy.Id = "pet_ene_001";
        enemy.NameDisplay = "Pet";
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
