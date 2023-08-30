using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    private bool up, down, left, right, blaster;
    [SerializeField] float speed;
    Rigidbody2D rb;
    [SerializeField] float angle;
    [SerializeField] float angleChange;
    [SerializeField] float speedChange;
    [SerializeField] float decChange;
    [SerializeField] float maxSpeed;
    [SerializeField] GameObject bullet;
    long startTime;
    long elapsed;
    bool changeLives;
    [SerializeField] List<AudioClip> audioClips;
    public AudioSource audioSource;
    AudioClip clip;
    [SerializeField] GameObject flame;
    [SerializeField] GameObject flameDown1;
    [SerializeField] GameObject flameDown2;
    [SerializeField] GameObject explosion; 
    [SerializeField] GameObject flash;


    void Start()
    {
        bullet.gameObject.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
        maxSpeed = 0;
        startTime = DateTime.Now.Ticks-150000000;
        gameObject.transform.position = new Vector2(0, 0);
        flame.GetComponent<SpriteRenderer>().enabled = false;
        flameDown1.GetComponent<SpriteRenderer>().enabled = false;
        flameDown2.GetComponent<SpriteRenderer>().enabled = false;
        explosion.GetComponent<SpriteRenderer>().enabled = false;
        flash.GetComponent<SpriteRenderer>().enabled = false;

    }

    // Update is called once per frame
    void Update()
    {

        elapsed = (DateTime.Now.Ticks - startTime) / 10000;


        if (Input.GetKeyDown(KeyCode.W)) {
            flame.GetComponent<SpriteRenderer>().enabled = true;
        up = true;
    }
        if (Input.GetKeyDown(KeyCode.A))
            left = true;
        if (Input.GetKeyDown(KeyCode.S))
        {
            flameDown1.GetComponent<SpriteRenderer>().enabled = true;
            flameDown2.GetComponent<SpriteRenderer>().enabled = true;
            down = true;

        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            right = true;

        }
        if (Input.GetKeyDown(KeyCode.Space) && elapsed >= 1200)
        {//&& elapsed >= time
            RestartTime();
            startTime = DateTime.Now.Ticks;
            blaster = true;
            StartCoroutine(DelayedShoot());


        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            flame.GetComponent<SpriteRenderer>().enabled = false;
            up = false;
        }
        if (Input.GetKeyUp(KeyCode.A))
            left = false;
        if (Input.GetKeyUp(KeyCode.S))
        {
            flameDown1.GetComponent<SpriteRenderer>().enabled = false;
            flameDown2.GetComponent<SpriteRenderer>().enabled = false;
            down = false;
        }
        if (Input.GetKeyUp(KeyCode.D))
            right = false;
        if (Input.GetKeyUp(KeyCode.Space))
            blaster = false;



    }
    public void FixedUpdate()
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
        if (left)
        {
            angle += angleChange ;
        }
        if (right)
        {
            angle -= angleChange;
        }
        if (up)
        {
            speed += (speedChange) * Time.deltaTime;
            rb.velocity = new Vector3(rb.velocity.x + transform.right.x* speed * Time.deltaTime, rb.velocity.y + transform.right.y * speed * Time.deltaTime, transform.position.z);

        }

        if (down)
        {
            speed -= (speedChange) * Time.deltaTime;
            rb.velocity = new Vector3(rb.velocity.x + -transform.right.x * speed * Time.deltaTime, rb.velocity.y + -transform.right.y * speed * Time.deltaTime, transform.position.z);


        }
        if (angle < 0)
            angle += 360;
        angle = angle % 360;
        //     rb.transform.Rotate(0,0,angle * Time.deltaTime);
       rb.rotation = angle;
     // rb.rotation += angle;
    }
    public void RestartTime()
    {
        startTime = DateTime.Now.Ticks;
        elapsed = 0;
    }
    IEnumerator DelayedShoot()
    {
        while (blaster)
        {
            flash.GetComponent<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(.01f);
            flash.GetComponent<SpriteRenderer>().enabled = false;

            GameObject b = Instantiate(bullet, transform.position + transform.up * .001f, transform.rotation);
            b.SetActive(true);
            b.GetComponent<Rigidbody2D>().velocity = transform.right * 5.5f;
            clip = audioClips[0];
            audioSource.clip = clip;
            audioSource.Play();
            Destroy(b, 2f);
            yield return new WaitForSeconds(.8f);
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Asteroid") && elapsed >= 1500)
        {
            //WORK HERE
            startTime = DateTime.Now.Ticks;
            clip = audioClips[1];
            audioSource.clip = clip;
            audioSource.Play();
            changeLives = true;
            StartCoroutine(DeathAnimation());

        }

    }
    public bool ChangeLives()
    {
        return changeLives;
    }
    public void SetLives()
    {
        changeLives = false;
    }
    public void PlayAudioExplode()
    {
        clip = audioClips[2];
        audioSource.clip = clip;
        audioSource.Play();
    }
    IEnumerator DeathAnimation()
    {
       explosion.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(.2f);
        explosion.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(.8f);
        gameObject.transform.position = new Vector2(0, 0);
        rb.velocity = Vector3.zero;

        down = false;
        up = false;
        left = false;
        right = false;
        speed = 0;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(.4f);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(.1f);
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(.05f);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(.05f);
        gameObject.GetComponent<SpriteRenderer>().enabled = true;


    }
   public IEnumerator finalDeath()
    {
        PlayAudioExplode();
        explosion.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(.2f);
        explosion.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.SetActive(false);
        elapsed = 1500;
        rb.velocity = Vector3.zero;
        down = false;
        up = false;
        left = false;
        right = false;
        speed = 0;
        gameObject.transform.position = new Vector2(0, 0);




    }
}