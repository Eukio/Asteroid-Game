using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    bool hit;
    Rigidbody2D r;
    private Asteroid a;
    void Start()
    {
        r = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        if (hit)
        {
            Destroy(gameObject);
            hit = false;
        }
        if (r.position.x > 10.25)
        {
            r.position = new Vector2(-10.25f, r.position.y);
        }

        if (r.position.x < -10.25)
        {
            r.position = new Vector2(10.25f, r.position.y);
        }
            if (r.position.y > 5)
            {
                r.position = new Vector2(r.position.x, -3.7f);
            }
            if (r.position.y < -4.76)
        {
            r.position = new Vector2(r.position.x, 4.76f);

        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Asteroid") && collision.GetComponent("Asteroid") as Asteroid)
        {
            a = (collision.gameObject.GetComponent("Asteroid") as Asteroid);
            Debug.Log("Here: " + a.GetHealth());
            a.SetHealth();

                hit = true;

        }
    }
   
    public bool Hit()
    {
        return hit;
    }
    public void setHit(bool hit)
    {
        this.hit = hit;
    }
}
