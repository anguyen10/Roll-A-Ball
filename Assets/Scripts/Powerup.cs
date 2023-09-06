using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    // enum is a datatype that we can specify its value and use
    public enum PowerupType {SpeedUp, SpeedDown, Grow, Shrink}

    public PowerupType myPowerup;           //objects powerup type
    public float powerupDuration = 7f;      // duration of powerup
    PlayerController playerController;      // reference to player controller

    void Start()
    {
        // find and assign player controller to this local ref
        playerController = FindObjectOfType<PlayerController>();
    }

    public void UsePowerup()
    {
        //if powerup is speedup, increase playercontroller speed by double
        if (myPowerup == PowerupType.SpeedUp)
            playerController.speed = playerController.baseSpeed * 2;

        // if PU is speeddown, decrese speed by 3
        if (myPowerup == PowerupType.SpeedDown)
            playerController.speed = playerController.baseSpeed / 3;

        //if grow, increase size by 3
        // also need to moveup on y axis to avoid falling through ground
        if (myPowerup == PowerupType.Grow)
        {
            Vector3 temp = playerController.gameObject.transform.position;
            temp.y = 1;
            playerController.gameObject.transform.position = temp;
            playerController.gameObject.transform.localScale = Vector3.one * 3;
        }

        //if shrink, decrease size by 2
        if (myPowerup == PowerupType.Shrink)
            playerController.gameObject.transform.localScale = Vector3.one / 2;
        

        StartCoroutine(ResetPowerup());
    }

    IEnumerator ResetPowerup()
    {
        yield return new WaitForSeconds(powerupDuration);

        if (myPowerup == PowerupType.SpeedUp || myPowerup == PowerupType.SpeedDown)
            playerController.speed = playerController.baseSpeed;

        if (myPowerup == PowerupType.Grow || myPowerup == PowerupType.Shrink)
        {
            playerController.gameObject.transform.localScale = Vector3.one;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
