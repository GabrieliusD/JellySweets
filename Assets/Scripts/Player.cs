using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterObject charObj;
    public float health;
    public float attackDamage;
    public float range;
    public GameObject bullet;
    public float bulletSpeed;
    public float attackSpeed;
    float totalTime = 0;
    public bool allowShooting;
    // Start is called before the first frame update
    void Start()
    {
        //gameObject.AddComponent<CharacterController>();
        Debug.Log("Character was created " + charObj.name);
        GetComponent<SpriteRenderer>().sprite = charObj.icon;
    }

    // Update is called once per frame
    void Update()
    {
        if (allowShooting)
        {
            GameObject enemyObj = ClosestEnemy();
            if (enemyObj != null)
                Attack(enemyObj);
        }
    }
    void Attack(GameObject enemy)
    {
        if(totalTime>=attackSpeed)
        {
            Vector2 dir = (enemy.transform.position - this.transform.position);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.Euler(0, 0, angle);
            GameObject b = Instantiate(bullet, this.transform.position, q);
            b.GetComponent<bullet>().attackDamage = attackDamage;
            AudioManager.instance.Play("Shoot");
            totalTime -= attackSpeed;
        }
        totalTime += Time.deltaTime;

    }

    void PlayAttackSound()
    {

    }
    GameObject ClosestEnemy()
    {
        GameObject closest = null;
        float closestDistance = float.MaxValue;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        if(enemies.Length == 0)
        {
            return null;
        }
        foreach(GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(enemy.transform.position, this.transform.position);
            if(distance < closestDistance)
            {
                closestDistance = distance;
                closest = enemy;
            }
        }
        float inRange = Vector2.Distance(closest.transform.position, this.transform.position);
        if (inRange < range)
            return closest;
        else return null;
    }
}
