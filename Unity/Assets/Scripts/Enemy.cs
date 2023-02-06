using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float enemyHealth;

    [SerializeField]
    private float movementSpeed;

    private int killReward;
    private int damage;

    private MoneyManager moneyManager;
    private PlayerHealth playerHealth;

    private GameObject targetTile;

    private void Awake()
    {
        Enemies.enemies.Add(gameObject);
    }

    public void Start()
    {
        initializeEnemy();
    }

    private void initializeEnemy()
    {
        targetTile = MapGenerator.startTile;
        killReward = 100;
        damage = (int)enemyHealth/10;
        moneyManager = GameObject.FindWithTag("MoneyManager").GetComponent<MoneyManager>();
        playerHealth = GameObject.FindWithTag("PlayerHealth").GetComponent<PlayerHealth>();
    }

    public void takeDamage(float ammount)
    {
        enemyHealth -= ammount;

        if (enemyHealth <= 0)
        {
            die();
            moneyManager.AddMoney(killReward);
        }
    }

    private void die()
    {
        Enemies.enemies.Remove(gameObject);
        Destroy(transform.gameObject);
    }

    private void moveEnemy()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetTile.transform.position, movementSpeed * Time.deltaTime);
    }

    private void checkPosition()
    {
        if (targetTile != null && targetTile != MapGenerator.endTile)
        {
            float distance = (transform.position - targetTile.transform.position).magnitude;

            if (distance < 0.001f)
            {
                int currentIndex = MapGenerator.pathTiles.IndexOf(targetTile);
                Debug.Log(currentIndex);

                targetTile = MapGenerator.pathTiles[currentIndex + 1];
            }
        }
        else if(targetTile != null && targetTile == MapGenerator.endTile)
        {
            float distance = (transform.position - targetTile.transform.position).magnitude;

            if (distance < 0.001f)
            {
                playerHealth.DamagePlayer(damage);
                die();
            }
        }
    }

    private void Update()
    {
        checkPosition();
        moveEnemy();

        takeDamage(0);
    }
}
