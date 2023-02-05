using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private string currentNote = "C4";

    public string noteTag = "rightNote";
    public float baseHealth = 100;
    public float healthDrainPerSec = 10f;
    public float healAmount = 20f;
    [SerializeField] private Slider _playerHealthSlider;

    private float health;
    private int comboCount;
    private CircleCollider2D playerCollider;
    private Rigidbody2D rb;
    public float TargetHeight = 0;
    private Vector3 refVel = Vector3.zero;

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

        rb.MovePosition(Vector3.SmoothDamp(rb.position, new Vector3(rb.position.x, TargetHeight, rb.position.y), ref refVel, 0.05f));

        //check if player has ran out of health
        if (health <= 0)
        {
            Debug.Log("Player has Died");
            die();
        }
    }

    private void FixedUpdate()
    {
        SubtractHealth(Time.deltaTime * healthDrainPerSec);
    }

    private void MoveHeight(int direction)
    {
        MusicPlatformGroup musicPlatformGroup = MusicPlatformGroup.Instance;
        int row = musicPlatformGroup.GetRowIdx(currentNote) + direction;
        TargetHeight = musicPlatformGroup.rows[Mathf.Clamp(row, 0, musicPlatformGroup.rows.Length - 1)].transform.position.y;
        currentNote = musicPlatformGroup.GetRowName(row);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if the player travels through right note, add health
        if (collision.gameObject.tag == noteTag)
        {
            AddHealth(healAmount);
        }
    }

    public void SubtractHealth(float amount)
    {
        health -= amount;
        _playerHealthSlider.value = health;
    }
    public void AddHealth(float amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, 110);
        _playerHealthSlider.value = health;
    }

    private void die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
