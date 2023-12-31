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
    [HideInInspector]
    // for speed powerups
    public float baseSpeed;
    private Rigidbody rb;
    private int pickupCount;
    private bool gameOver = false;
    GameObject resetPoint;
    bool resetting = false;
    Color originalColour;
    bool grounded = true;

    //Controllers
    GameController gameController;
    CameraController cameraController;
    Timer timer;

    [Header("UI")]
    public GameObject inGamePanel;
    public GameObject gameOverPanel;
    public GameObject pausePanel;
    public TMP_Text scoreText;
    public TMP_Text timerText;
    public TMP_Text winTimeText;

    // Start is called before the first frame update
    void Start()
    {
        // base speed before speed powerup
        baseSpeed = speed;
        rb = GetComponent<Rigidbody>();
        // get the number of pickups in scene
        pickupCount = GameObject.FindGameObjectsWithTag("Pick Up").Length;
        gameOverScreen.SetActive(false);
        //run the check pickups function
        SetCountText();
        winTimeText.text = "";
        // get reset point
        resetPoint = GameObject.Find("Reset Point");
        originalColour = GetComponent<Renderer>().material.color;

        gameController = FindObjectOfType<GameController>();
        cameraController = FindObjectOfType<CameraController>();

        //get the timer object
        timer = FindObjectOfType<Timer>();
        if (gameController.gameType == GameType.SpeedRun)
            StartCoroutine(timer.StartCountdown());

        timer.StartTimer();
        // turnon our in game panel
        inGamePanel.SetActive(true);
        // turn off win panel
        gameOverPanel.SetActive(false);
        Time.timeScale = 1;
    }

    private void Update()
    {
        timerText.text = "Time: " + timer.GetTime().ToString("F2");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (resetting)
            return;

        if (gameController.gameType == GameType.SpeedRun && !timer.IsTiming())
            return;

        if (gameController.controlType == ControlType.WorldTilt)
            return;

        if (grounded)
        { 
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);

            if(cameraController.cameraStyle == CameraStyle.Free)
            {
            //rotate player to direction of camera
            transform.eulerAngles = Camera.main.transform.eulerAngles;
            //translates input vector into coordinates
            movement = transform.TransformDirection(movement);
            }
        
            rb.AddForce(movement * speed);
        }   

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Pick Up")
        {
            Destroy(other.gameObject);
            //decrement the pickup count
            pickupCount -= 1;
            //run the check pickups function
            SetCountText();
        }

        if (other.tag == "Light")
        {
            Destroy(other.gameObject);
        }

        if(other.gameObject.CompareTag("Powerup"))
        {
            other.GetComponent<Powerup>().UsePowerup();
            other.gameObject.transform.position = Vector3.down * 1000;
        }

        if (other.gameObject.CompareTag("Powerup"))
        {
            other.GetComponent<Powerup>().UsePowerup();
            other.gameObject.transform.position = Vector3.down * 1000;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Respawn"))
        {
            StartCoroutine(ResetPlayer());
        }
    }

    public IEnumerator ResetPlayer()
    {
        // pause execution of code until resume
        resetting = true;
        GetComponent<Renderer>().material.color = Color.black;
        rb.velocity = Vector3.zero;
        Vector3 startPos = transform.position;
        var i = 0.0f;
        var rate = 1.0f;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            transform.position = Vector3.Lerp(startPos, resetPoint.transform.position, i);
            yield return null;
        }
        GetComponent<Renderer>().material.color = originalColour;
        resetting = false;
    }

    void SetCountText()
    {
        //display the amount of pckups left in our scene
        scoreText.text = "Keys Left: " + pickupCount;

        if (pickupCount == 0)
        {
            WinGame();
        }
    }

    void WinGame()
    {
        gameOverScreen.SetActive(true);
        winTimeText.text = "Escape Successful!";

        if (gameController.gameType == GameType.SpeedRun)
            timer.StopTimer();

        //set game over to time
        gameOver = true;
        //stop the timer
        timer.StopTimer();
        // turn on win panel
        gameOverPanel.SetActive(true);
        // turn off our in game panel
        inGamePanel.SetActive(false);
        //display timer on the win time text
        winTimeText.text = "Your Time: " + timer.GetTime().ToString("F2");

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
