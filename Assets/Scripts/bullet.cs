using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    // Start is called before the first frame update
    public float bulletSpeed = 10.0f;
    public float attackDamage = 0;
    public float lifeSpawn = 1.0f;
    float timeElapsed;
    Rigidbody2D bulletBody;
    void Start()
    {
        bulletBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        if(timeElapsed >= lifeSpawn)
        {
            Destroy(this.gameObject);
        }
        bulletBody.velocity = transform.right * bulletSpeed;
    }
}
