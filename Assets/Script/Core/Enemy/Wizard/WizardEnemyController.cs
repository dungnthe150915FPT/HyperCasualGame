using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardEnemyController : EnemyController
{
    public override void setupEnemy()
    {
        WizardEnemy wizardEnemy = new WizardEnemy();
        wizardEnemy.Id = "wizard_ene_001";
        wizardEnemy.NameDisplay = "Wizard";
        wizardEnemy.MaxHealth = 100f;
        wizardEnemy.CurrentHealth = 100f;
        wizardEnemy.AttackDamage = 10f;
        wizardEnemy.AttackSpeed = 1f;
        wizardEnemy.AttackRange = 1f;
        wizardEnemy.MoveSpeed = 5f;
        wizardEnemy.JumpHeight = 1f;

        setupEnemyController(wizardEnemy);
    }

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

    //internal override void shouldAttackPlayer(Component sender, object data)
    //{
    //    base.shouldAttackPlayer(sender, data);
    //}

    //internal override void playerOutOfSight(Component sender, object data)
    //{
    //    base.playerOutOfSight(sender, data);
    //}

    //internal override void playerStayInSight(Component sender, object data)
    //{
    //    base.playerStayInSight(sender, data);
    //}
}
