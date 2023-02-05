using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private string currentNote = "C4";

    public string noteTag = "rightNote";
    public float baseHealth = 100;
    public float healthDrainPerSec = 10f;
    public float healAmount = 20f;

    private float health;
    private int comboCount;
    private CircleCollider2D playerCollider;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        playerCollider = gameObject.GetComponent<CircleCollider2D>();
        health = baseHealth;
        comboCount = 0;
        currentNote = "C4";
        rb = GetComponent<Rigidbody2D>();
        MoveHeight(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            // move up
            MoveHeight(1);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveHeight(-1);
            // TODO: move smoothly to the next line
        }

        //check if player has ran out of health
        if (health <= 0)
        {
            Debug.Log("Player has Died");
            die();
        }
    }

    private void FixedUpdate()
    {
        subractHealth(Time.deltaTime * healthDrainPerSec);
    }

    private void MoveHeight(int direction)
    {
        int row = MusicPlatformGroup.Instance.GetRowIdx(currentNote) + direction;
        float height = MusicPlatformGroup.Instance.rows[row].transform.position.y;
        rb.MovePosition(new Vector3(-9, height, 0));
        //transform.position = new Vector3(-9, height, 0); // TODO: move player to desired space on screen.
        currentNote = MusicPlatformGroup.Instance.GetRowName(row);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if the player travels through right note, add health
        if (collision.gameObject.tag == noteTag)
        {
            addHealth(healAmount);
        }
    }

    private void subractHealth(float amount)
    {
        health -= amount;
    }
    private void addHealth(float amount)
    {
        health += amount;
    }

    private void die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
