using UnityEngine;

[CreateAssetMenu(fileName = "GameDatabase", menuName = "ScriptableObjects/GameDatabase", order = 1)]
public class GameDatabase : ScriptableObject
{
    [Header("Player Bullet Stats")]
    public float playerBulletSpeed = 1500f;
    public int playerTotalShots = 1;
    public int playerBulletDamage = 10;

    [Header("Enemy Stats")]
    public int enemyHealthPoint = 50;
    public float enemySpeed = 2f;
    public float enemyShootingInRange = 4f;
    public float enemyFireRate = 1f;
    public float enemyFlankDistance = 2f;
    public float enemyMaxLineOfSite = 5f;
    public float enemyKnockbackForce = 5f;
}
