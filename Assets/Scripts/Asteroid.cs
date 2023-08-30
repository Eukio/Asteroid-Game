using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using static UnityEngine.UI.Image;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class Asteroid : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] int size;
    [SerializeField] GameObject small;
    [SerializeField] GameObject med;

    public static int SMALL = 0;
    public static int MEDIUM = 1;
    public static int BIG = 2;
    int health;
    int points;
    int speed;
    Rigidbody2D rb;
    long startTime;
    long elapsed;
    double time;
    Game g;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (size == SMALL)
        {
            health = 1;
            points = 2;

        }
        else if (size == MEDIUM)
        {
            health = 2;
            points = 5;
        }
        else if (size == BIG)
        {
            health = 3;
            points = 10;
        }

        g = (Game)FindObjectOfType(typeof(Game));

    }

    // Update is called once per frame
    void Update()
    { //Not displaying sprite


        /*
         *  
         */
    }
    private void FixedUpdate()
    {
        if (rb.position.x > 10.25)
        {

            rb.position = new Vector2(-10.25f, rb.position.y);
        }

        if (rb.position.x < -10.25)
        {
            rb.position = new Vector2(10.25f, rb.position.y);

        }
        if (rb.position.y > 5)
        {
            rb.position = new Vector2(rb.position.x, -3.7f);
        }
        if (rb.position.y < -4.76)
        {
            rb.position = new Vector2(rb.position.x, 4.76f);

        }
        if (health <= 0)
        {
            Debug.Log("Hello");
            g.SetScore(g.GetScore() + points);


            if (size == BIG)
            {

                StartCoroutine(DelayedVelocity(med));


            }
            if (size == MEDIUM)
            {
                StartCoroutine(DelayedVelocity(small));
            }
                Destroy(gameObject);
           
        }
    }
    public int GetHealth()
    {
        return health;
    }
    public void SetHealth()
    {
        health--;
    }

    IEnumerator DelayedVelocity(GameObject type)
    {
        {

            {

                GameObject a1 = Instantiate(type, transform.position + transform.up * .001f, transform.rotation);
                GameObject a2 = Instantiate(type, transform.position + transform.up * .001f, transform.rotation);
                a1.SetActive(true);
                a2.SetActive(true);
                a1.GetComponent<Rigidbody2D>().velocity = transform.right*1.3f;
                a2.GetComponent<Rigidbody2D>().velocity = rb.velocity;

                Debug.Log("V2: " + a2.GetComponent<Rigidbody2D>().velocity);

                yield return new WaitForSeconds(10f);
              
            }
        }
    }
}
