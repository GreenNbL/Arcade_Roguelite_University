using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    private Transform player;
    public float chaseRadius = 10f;
    public float stoppingDistance = 1f;
    public float speed = 2f;
    public LayerMask obstacleMask;
    public float avoidanceDistance = 2f;
    private Collider2D enemyCollider;
    public float health;
    public float maxHealth = 100f;
    public float attackDamage = 10f;
    public int level = 1;
    public float attackSpeed = 1f;
    private bool isChasing = false;
    private float attackCooldown = 0f;
    public int score;
    public Animator animator;
    public bool died = false;
    public Image healthBar;
    public Image zzz;
    public Image bewilderment;
    public Image warning;
    public List<PrefabProbability> prefabs;
    private Vector2 lastSeenPosition;
    private float idleTimer = 0f;
    private float stopTimer = 0f;
    private float warningTimer = 0f;
    private bool isIdling = false;
    private bool wasChasing = false;
    private float stopDuration = 1f;
    private float warningDuration = 1f;
    private float idleDuration = 2f;
    private Vector2 spawnPosition; // Новая переменная для хранения позиции спавна

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyCollider = GetComponent<Collider2D>();
        AdjustStatsBasedOnLevel();
        health = maxHealth;
        bewilderment.enabled = false;
        zzz.enabled = true; // Изображение zzz изначально включено
        warning.enabled = false;
        spawnPosition = transform.position; // Запоминаем позицию спавна
        prefabs = prefabs.OrderBy(p => p.GetProbability()).ToList();
        healthBar.enabled = false;
    }
    public void levelUp()
    {
        ++level;
        AdjustStatsBasedOnLevel();
    }
    void Update()
    {
        if (!died)
        {
            attackCooldown -= Time.deltaTime;
            healthBar.fillAmount = health / maxHealth;
            float distanceToPlayer = 1000;
            if (!GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().isInvisible)
                 distanceToPlayer = Vector2.Distance(player.position, transform.position);
            isChasing = distanceToPlayer <= chaseRadius;
            if(warning.enabled )
                warningTimer += Time.deltaTime;
            if (!isChasing && warningTimer >= warningDuration)
            {
                warningTimer = 0;
                warning.enabled = false;
                zzz.enabled = false;
            }
            if (isChasing)
            {
                warningTimer += Time.deltaTime;
                warning.enabled = true;
                bewilderment.enabled = false;
                zzz.enabled = false; // Выключаем zzz при преследовании
                if (warningTimer >= warningDuration)
                {
                    warning.enabled = false;
                    idleTimer = 0f;
                    stopTimer = 0f;
                    isIdling = false;
                    
                    healthBar.enabled = true; // Включаем индикатор здоровья
                    
                    if (IsPathClear(player.position))
                    {
                        wasChasing = true;
                        lastSeenPosition = player.position;
                        ChasePlayer();
                    }
                    else
                    {
                        AvoidObstacles();
                    }

                    TryAttackPlayer(distanceToPlayer);
                }
            }
            else if (!isIdling && wasChasing)
            {
                warningTimer = 0;
                warning.enabled = false;
                bewilderment.enabled = false;
                // Debug.Log(" Двигаемся к последней известной позиции");
                StartIdling();
            }else if (isIdling)
            {
                warningTimer = 0;
                warning.enabled = false;
                bewilderment.enabled = false;
                HandleIdling();
            }
        }
        else
        {
            healthBar.fillAmount = 0;
        }
    }

    private void StartIdling()
    {
        healthBar.enabled = false; // Выключить индикатор здоровья
        //isIdling = true;
        bewilderment.enabled = true;
        stopTimer += Time.deltaTime;
        if (Vector2.Distance(transform.position, lastSeenPosition) < 0.01f)
        {
            //Debug.Log("Возвращаемся на старое место");
            isIdling = true;
            bewilderment.enabled = false;
            stopTimer = 0f;
        }
        if (stopTimer >= stopDuration)
        {
            transform.position = Vector2.MoveTowards(transform.position, lastSeenPosition, speed / 2 * Time.deltaTime);
        }
    }

    private void HandleIdling()
    {
        idleTimer += Time.deltaTime;

        if (idleTimer >= idleDuration)
        {
            ReturnToSpawn(); // Возврат на место спавна
            //SpawnItems();
            if (Vector2.Distance(transform.position, spawnPosition) < 0.01f)
            {
                isIdling = false;
                idleTimer = 0f;
                zzz.enabled = true; // Включаем zzz на месте спавна
                wasChasing = false;
            }
        }
    }
   
    private void ReturnToSpawn()
    {
        transform.position = Vector2.MoveTowards(transform.position, spawnPosition, speed/2 * Time.deltaTime);
    }

    private void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector2.Distance(player.position, transform.position);
        if (distanceToPlayer > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            if (direction.x > 0 && transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1);
            }
            else if (direction.x < 0 && transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1);
            }
        }
    }

    private void AvoidObstacles()
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        Vector2 rightDirection = new Vector2(directionToPlayer.y, -directionToPlayer.x);
        Vector2 leftDirection = new Vector2(-directionToPlayer.y, directionToPlayer.x);

        if (IsPathClear((Vector2)transform.position + rightDirection * avoidanceDistance))
        {
            transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + rightDirection, speed * Time.deltaTime);
        }
        else if (IsPathClear((Vector2)transform.position + leftDirection * avoidanceDistance))
        {
            transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + leftDirection, speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position - directionToPlayer, speed * Time.deltaTime);
        }
    }

    private bool IsPathClear(Vector2 targetPosition)
    {
        float radius = GetColliderRadius();
        float distance = Vector2.Distance(transform.position, targetPosition);
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, radius, (targetPosition - (Vector2)transform.position).normalized, distance, obstacleMask);
        return hit.collider == null;
    }

    private float GetColliderRadius()
    {
        float radius = 0.5f;
        if (enemyCollider is CircleCollider2D circleCollider)
        {
            radius = circleCollider.radius;
        }
        else if (enemyCollider is BoxCollider2D boxCollider)
        {
            radius = boxCollider.size.x / 2;
        }
        return radius;
    }

    private void TryAttackPlayer(float distanceToPlayer)
    {
        if (distanceToPlayer <= stoppingDistance && attackCooldown <= 0f)
        {
            animator.SetTrigger("attack");
            attackCooldown = 1f / attackSpeed;
        }
    }

    public void DealDamage()
    {
        float distanceToPlayer = Vector2.Distance(player.position, transform.position);
        if (distanceToPlayer <= stoppingDistance)
        {
            player.gameObject.GetComponent<HeroStotistic>().damageHero((int)attackDamage);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
            died = true;
            enemyCollider.enabled = false;
        }
    }

    public void GetUp()
    {
        died = false;
        enemyCollider.enabled = true;
        wasChasing = false;
        healthBar.enabled = false;
        zzz.enabled = true;
    }
    private void Die()
    {
        animator.SetTrigger("died");
        player.gameObject.GetComponent<HeroStotistic>().increadeScore(score);
    }

    public void Grave()
    {
        transform.localScale = new Vector3(0.3f, 0.3f, 1);
    }

    private void AdjustStatsBasedOnLevel()
    {
        if (level > 1)
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1);
            maxHealth += level * 10;
            health = maxHealth;
            attackDamage += level * 4;
            speed += level * 0.2f;
            attackSpeed += level * 0.1f;
        }
    }

    public void Respawn()
    {
        healthBar.enabled = false;
        bewilderment.enabled = false;
        zzz.enabled = true; // Изображение zzz изначально включено
        warning.enabled = false;
        warningTimer = 0f;
        idleTimer = 0f;
        stopTimer = 0f;
        isIdling = false;
        wasChasing = false;
        transform.position = spawnPosition;
    }
    private void SpawnItems()
    {
        foreach (var prefabProb in prefabs)
        {
            float rand = Random.value;
            BoxCollider2D collider = prefabProb.GetComponent<BoxCollider2D>();
            Vector3 position = transform.position + Vector3.up + transform.forward;

            if (rand <= prefabProb.GetProbability())
            {
                Instantiate(prefabProb.GetPrefab(), position, Quaternion.identity);
                break;
            }
        }
    }
}
