using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class EnemyAttack : MonoBehaviour
{
    [SerializeField] public float attackDamage;

    abstract public void attackPlayerStart();
    abstract public void attackPlayerDealDamage();
    abstract public void attackPlayerEnd();


}
