using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class Game : MonoBehaviour
{
    // Start is called before the first frame update
    bool up,down, left, right;  
    [SerializeField] Player player;
    [SerializeField] GameObject live1;
    [SerializeField] GameObject live2;
    [SerializeField] GameObject live3;
    [SerializeField] Text points;
    [SerializeField] Text t;
    [SerializeField] Asteroid small;
    [SerializeField] Asteroid medium;
    [SerializeField] Asteroid large;
    int score;
    bool stopRun;
    int lives;
    long startTime;
    long elapsed;
    long spawnrate;
    int countTimer;
    Random r;

    void Start()
    {
        Restart();

    }
    // Update is called once per frame
    void Update()
    {

        elapsed = (DateTime.Now.Ticks - startTime) / 10000;
        points.text = "Points: " + score;

        if(elapsed >= 5000-countTimer)
        {
            r = new Random();
            int randPos = (int) r.Next(1,5);
            r = new Random();
            int randAst = (int) r.Next(1, 4);

            Asteroid tempA = null;
            switch (randAst)
            {
                case 1:
                    tempA = small;
                    break;
                case 2:
                    tempA = medium;
                    break;
                case 3:
                    tempA = large;
                    break;
            }


            switch (randPos)
            {//spawn from
                case 1:
                    GameObject a = Instantiate(tempA.gameObject, new Vector3(-9,-4), transform.rotation); //bottomleft
                    a.SetActive(true);
                    a.GetComponent<Rigidbody2D>().velocity = transform.right * 1.5f+ transform.up * 1.5f;
                    break;
                case 2:
                    // code block
                    GameObject b = Instantiate(tempA.gameObject, new Vector3(9, -4), transform.rotation);//bottomright
                    b.SetActive(true);
                    b.GetComponent<Rigidbody2D>().velocity = -transform.right * 1.5f+ transform.up * 1.5f;

                    break;
                case 3:
                    GameObject c = Instantiate(tempA.gameObject, new Vector3(9, 4), transform.rotation);//topleft
                    c.SetActive(true);
                    c.GetComponent<Rigidbody2D>().velocity = transform.right * 1.5f+ -transform.up * 1.5f;
                    break;
                case 4:
                    GameObject d = Instantiate(tempA.gameObject, new Vector3(-9, -4), transform.rotation);//topright
                    d.SetActive(true);
                    d.GetComponent<Rigidbody2D>().velocity = -transform.right * 1.5f+ -transform.up * 1.5f;
                   
                    break;
            }
            countTimer+=10;
            startTime = DateTime.Now.Ticks;

        }
        
        if (!stopRun)
            {
            if (player.ChangeLives()) //CHECK LIVES HERE, NOT INCREMENTING CORRECTLUY
            {
                --lives;
                player.SetLives();
            }
            if (lives == 2)
                live3.SetActive(false);
            // GetComponent<SpriteRenderer>().enabled = false;

            if (lives == 1)
                live2.SetActive(false);

            if (lives <= 0)
            {
                StartCoroutine(player.finalDeath());
                live1.SetActive(false);
                t.text = "You Lose!";
                stopRun = true;
                up = false;
                down = false;
                left = false;
                right = false;

            }
       }

        if (stopRun && Input.GetKeyUp(KeyCode.R))
                Restart();
    }
    public int GetScore()
    {
        return score;
    }
    public void SetScore(int score)
    {
        this.score = score;
    }

    public void Restart()
    {
        stopRun = false;
        small.gameObject.SetActive(false);
        medium.gameObject.SetActive(false);
        large.gameObject.SetActive(false);
        score = 0;
        lives = 3;
        elapsed = 0;

        points.text = "Points: " + score;
        t.text = "";
        live1.SetActive(true);
        live2.SetActive(true);
        live3.SetActive(true);
        startTime = DateTime.Now.Ticks;
        spawnrate = 1;
        r = new Random();
        player.gameObject.SetActive(true);
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero; 
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Asteroid");
        for (int i = 0; i < enemies.Length; i++)
        {
            Destroy(enemies[i]);
        }
        player.GetComponent<SpriteRenderer>().enabled = true;

        countTimer = 0;
    }
    }
