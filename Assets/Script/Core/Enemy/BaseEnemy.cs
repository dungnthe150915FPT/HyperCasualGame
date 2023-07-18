using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Core.Enemy
{
    public class BaseEnemy
    {
        // identity
        private string id;
        private string nameDisplay;

        // stats
        private float maxHealth;
        private float currentHealth;
        private float attackDamage;
        private float attackSpeed;
        private float attackRange;
        private float moveSpeed;
        private float jumpHeight;

        public BaseEnemy() { }

        // getter and setter
        public string Id { get => id; set => id = value; }
        public string NameDisplay { get => nameDisplay; set => nameDisplay = value; }
        public float MaxHealth { get => maxHealth; set => maxHealth = value; }
        public float CurrentHealth { get => currentHealth; set => currentHealth = value; }
        public float AttackDamage { get => attackDamage; set => attackDamage = value; }
        public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
        public float AttackRange { get => attackRange; set => attackRange = value; }
        public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
        public float JumpHeight { get => jumpHeight; set => jumpHeight = value; }
    }
}
