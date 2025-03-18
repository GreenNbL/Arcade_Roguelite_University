using UnityEngine;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    private Transform player; // ������ �� ������ ������
    public float chaseRadius = 10f; // ������ �������������
    public float stoppingDistance = 1f; // ����������, �� ������� ���� ��������������� �� ������
    public float speed = 2f; // �������� �����
    public LayerMask obstacleMask; // ����� ��� ����������� �����������
    public float avoidanceDistance = 2f; // ���������� ��� ����������� �����������

    private Collider2D enemyCollider; // ��������� �����
    public float health; // �������� �����
    public float maxHealth = 100f; // �������� �����
    public float attackDamage = 10f; // ���� ����� �����
    public int level = 1; // ������� �����
    public float attackSpeed = 1f; // �������� ����� (���������� ������ � �������)

    private bool isChasing = false; // ������ �������������
    private float attackCooldown = 0f; // �������� ����� �������

    public int score;

    public Animator animator;

    private bool died = false;

    public Image healthBar;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyCollider = GetComponent<Collider2D>(); // ������������� ����������
        AdjustStatsBasedOnLevel(); // ��������� ���������� � ����������� �� ������
        health=maxHealth;
    }

    void Update()
    {
        if (!died)
        {
            attackCooldown -= Time.deltaTime; // ���������� ������� �� ��������� �����
            healthBar.fillAmount = (float)health / (float)maxHealth;
            float distanceToPlayer = Vector2.Distance(player.position, transform.position);
            isChasing = distanceToPlayer <= chaseRadius;

            if (isChasing)
            {
                // ��������, ���� �� �������� ����� ������ � �������
                if (IsPathClear(player.position))
                {
                    ChasePlayer();
                }
                else
                {
                    AvoidObstacles();
                }

                // ������������� ��������� �� �����
                TryAttackPlayer(distanceToPlayer);
            }
        }
        else
        {
            healthBar.fillAmount =0;
        }
    }

    /*private void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        // �������� ���������� �� ������ ��� ���������
        float distanceToPlayer = Vector2.Distance(player.position, transform.position);

        if (distanceToPlayer > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }*/
    private void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        // �������� ���������� �� ������ ��� ���������
        float distanceToPlayer = Vector2.Distance(player.position, transform.position);

        if (distanceToPlayer > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

            // ������� ����� � ������� ��������
            if (direction.x > 0 && transform.localScale.x > 0)
            {
                // ���� ����� ��������� ������ � ���� ������� �����, �� ������������ ������
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1); // �������������� ������
            }
            else if (direction.x < 0 && transform.localScale.x < 0)
            {
                // ���� ����� ��������� ����� � ���� ������� ������, �� ������������ �����
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1); // �������������� �����
            }
        }
    }



    private void AvoidObstacles()
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        Vector2 rightDirection = new Vector2(directionToPlayer.y, -directionToPlayer.x); // ����������� ������
        Vector2 leftDirection = new Vector2(-directionToPlayer.y, directionToPlayer.x); // ����������� �����

        // �������� ����������� �� ������� �����������
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
            // ���� ��� ����������� �������������, ������ ������ �����
            transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position - directionToPlayer, speed * Time.deltaTime);
        }
    }

    private bool IsPathClear(Vector2 targetPosition)
    {
        float radius = GetColliderRadius();
        float distance = Vector2.Distance(transform.position, targetPosition);
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, radius, (targetPosition - (Vector2)transform.position).normalized, distance, obstacleMask);

        return hit.collider == null; // ���� ��������
    }

    private float GetColliderRadius()
    {
        float radius = 0.5f; // �������� �� ���������
        if (enemyCollider is CircleCollider2D circleCollider)
        {
            radius = circleCollider.radius; // ������ �����
        }
        else if (enemyCollider is BoxCollider2D boxCollider)
        {
            radius = boxCollider.size.x / 2; // �������� ������
        }
        return radius;
    }

    // ����� ��� ��������� ����� ������
    private void TryAttackPlayer(float distanceToPlayer)
    {
        if (distanceToPlayer <= stoppingDistance && attackCooldown <= 0f)
        {
            animator.SetTrigger("attack");
            attackCooldown = 1f / attackSpeed; // ����� ����� �������, ���������� �� �������� �����
        }
    }
    // ����� ��� ��������� �����
    public void DealDamage()
    {
        float distanceToPlayer = Vector2.Distance(player.position, transform.position);

        if (distanceToPlayer <= stoppingDistance)
        {
            player.gameObject.GetComponent<HeroStotistic>().damageHero((int)attackDamage);
            Debug.Log("���� ������� ������!");
        }
    }

    // ����� ��������� ����� �� �����
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

    // ����� ��� ��������� ������ �����
    private void Die()
    {
        // ����� ����� �������� ������, ������� ����� ����������� ��� ������ �����
        //Destroy(gameObject); // ���������� ������ �����
        animator.SetTrigger("died");
    }
    public void Grave()
    {
        transform.localScale = new Vector3(0.3f, 0.3f, 1);
    }
    // ����� ��� ���������� ��������� � ������������ � �������
    private void AdjustStatsBasedOnLevel()
    {
        // ����������� �������� � ���� � ����������� �� ������
        health += level * 10; // ������: ���������� �������� �� 10 �� �������
        attackDamage += level * 2; // ������: ���������� ����� �� 2 �� �������
        speed += level * 0.5f; // ����� ���� �������� ��� ���������� ��������
        attackSpeed += level * 0.1f; // ���������� �������� �����
    }
}
