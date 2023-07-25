using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("UI Stuff")]
    public GameObject gameOverScreen;
    public float speed = 5.0f;
    private Rigidbody rb;
    private int countText;
    private Timer timer;
    private bool gameOver = false;

    [Header("UI")]
    public GameObject inGamePanel;
    public GameObject gameOverPanel;
    public TMP_Text scoreText;
    public TMP_Text timerText;
    public TMP_Text winTimeText;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // get the number of pickups in scene
        countText = GameObject.FindGameObjectsWithTag("Pick Up").Length;
        gameOverScreen.SetActive(false);
        //run the check pickups function
        SetCountText();
        //get the timer object
        timer = FindObjectOfType<Timer>();
        timer.StartTimer();
        // turnon our in game panel
        inGamePanel.SetActive(true);
        // turn off win panel
        gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        timerText.text = "Time: " + timer.GetTime().ToString("F2");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameOver == true)
            return;

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        rb.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Pick Up")
        {
            Destroy(other.gameObject);
            //decrement the pickup count
            countText -= 1;
            //run the check pickups function
            SetCountText();
        }
    }

    void SetCountText()
    {
        //display the amount of pckups left in our scene
        scoreText.text = "Pickups Left: " + countText;

        if (countText == 0)
        {
            WinGame();
        }
    }

    void WinGame()
    {
        gameOverScreen.SetActive(true);
        winTimeText.text = "Escape Successful!";

        //set game over to time
        gameOver = true;
        //stop the timer
        timer.StopTimer();
        // turn on win panel
        gameOverPanel.SetActive(true);
        // turn off our in game panel
        inGamePanel.SetActive(false);
        //display timer on the win time text
        winTimeText.text = "Your time was: " + timer.GetTime().ToString("F2");

        //set velocity of rigidbody to zero
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    //Temporary - remove when doing modules in A2
    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene
            (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
