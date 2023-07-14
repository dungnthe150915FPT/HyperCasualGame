using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardEnemyController : EnemyController
{
    internal override void startAttackPlayer(GameObject player)
    {
        base.startAttackPlayer(player);
        animator.SetTrigger("attack");
    }

    internal override void stopAttackPlayer()
    {
        base.stopAttackPlayer();
        animator.SetTrigger("idle");
    }

    internal override void moveByVelocity(float x)
    {
        base.moveByVelocity(x);
        animator.SetBool("isRun", true);
    }

    internal override void setupEnemyController()
    {
        moveSpeed = 10f;
        maxHealth = 100f;
        currentHealth = maxHealth;
        nameDisplay = "Wizard";
        base.setupEnemyController();
    }

    internal override void OnDie()
    {
        playAnimation("die", "trigger");
        base.OnDie();
    }

    internal override void OnHitByBullet(GameObject bullet)
    {
        playAnimation("hurt", "trigger");
        base.OnHitByBullet(bullet);
    }
}
