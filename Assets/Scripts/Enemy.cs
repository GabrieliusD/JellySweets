using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatIncreasePerNight
{
    public float health = 5.0f;
    public float attackDamage = 0.5f;
    public float speed = 0.2f;
    public float attackSpeed = 0.1f;
    public float sugarDrop = 2.0f;
}

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public StatIncreasePerNight statIncrease;
    Rigidbody2D enemy;
    int night;
    public float health;
    public float attackDamage;
    public float speed;
    public float attackSpeed;
    public float sugarDrop = 20.0f;
    float totalTime;
    bool inRangetoAttack = false;
    Base playerBase = null;
    void Start()
    {
        int night = ValueManager.instance.Night;
        health = health + (statIncrease.health * night);
        attackDamage = attackDamage + (statIncrease.attackDamage * night);
        speed = Mathf.Clamp(speed + (statIncrease.speed * night), 0, 2.0f);
        attackSpeed = Mathf.Clamp(attackSpeed - (statIncrease.attackSpeed * night), 0.5f, 10.0f);
        sugarDrop = sugarDrop + (statIncrease.sugarDrop * night);
        enemy = GetComponent<Rigidbody2D>();
        enemy.velocity = new Vector2(-speed, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            ValueManager.instance.addToEarned((int)(sugarDrop*(1+Base.instance.sugarPercent)));
            ValueManager.instance.enemyDeath();
            AudioManager.instance.Play("EnemyKilled");
            Destroy(this.gameObject);
        }
        if(inRangetoAttack)
        {
            if (totalTime >= attackSpeed)
            {
                playerBase.TakeDamage(attackDamage);
                AudioManager.instance.Play("EnemyHit");
                totalTime -= attackSpeed;
            }
            totalTime += Time.deltaTime;
        }
    }
    public void takeDamage(float amount)
    {
        this.health -= amount;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Collision Enter");
        //Debug.Log("Enemy hit");
        if(collision.tag == "bullet")
        {
            AudioManager.instance.Play("EnemyHit");
            this.health -= collision.GetComponent<bullet>().attackDamage;
            Destroy(collision.gameObject);

        }
        //if (collision.tag == "PlayerAttackCollider")
        //{
        //    this.health -= collision.gameObject.GetComponentInParent<Player>().attackDamage;
        //    AudioManager.instance.Play("CloseAttackHit");
        //}
        if (collision.tag == "Wall")
        {
            enemy.velocity = Vector2.zero;
            inRangetoAttack = true;
            playerBase = collision.GetComponent<Base>();

        }

    }
}
