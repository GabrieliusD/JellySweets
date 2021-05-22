using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Joystick joystick;
    public float movementSpeed = 10.0f;
    Rigidbody2D charBody;
    float horizontalMove = 0.0f;
    float VerticalMove = 0.0f;
    public Collider2D colliderSphere;
    bool attacked = false;
    float timeNextAttack = 0.4f;
    float totalTime = 0.0f;
    float closeAttackRange = 1.0f;
    public Animator animator;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Character Controller Created");
        charBody = GetComponent<Rigidbody2D>();
        joystick = GameObject.FindGameObjectWithTag("JoyStick").GetComponent<Joystick>();
        colliderSphere = GetComponentInChildren<CircleCollider2D>();
        player = GetComponent<Player>();
        Attack();
        animator.Play("None");
    }

    private void OnEnable()
    {
        animator.Play("None");
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = joystick.Horizontal * movementSpeed;
        VerticalMove = joystick.Vertical * movementSpeed;

        charBody.velocity = new Vector2(horizontalMove, VerticalMove);
        if(attacked)
        {
            totalTime += Time.deltaTime;
            if(totalTime >= timeNextAttack)
            {
                AttackDone();
            }

        }
    }

    public void Attack()
    {
        colliderSphere.enabled = true;
        attacked = true;
        findAGroupofCloseEnemies();
        animator.Play("attack");
        AudioManager.instance.Play("CloseAttack");

    }

    public void findAGroupofCloseEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        List<GameObject> enInRange = new List<GameObject>();
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(enemy.transform.position, this.transform.position);
            if (distance < closeAttackRange)
            {
                enInRange.Add(enemy);
            }
        }
        
        foreach (var e in enInRange)
        {
            e.GetComponent<Enemy>().takeDamage(player.attackDamage);
            AudioManager.instance.Play("CloseAttackHit");
        }

    }
    public void AttackDone()
    {
        colliderSphere.enabled = false;
        attacked = false;
        totalTime = 0.0f;
        animator.StopPlayback();
        animator.Play("None");
    }
}
