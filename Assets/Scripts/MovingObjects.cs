using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjects : MonoBehaviour
{
    public float waitTime = 1;
    public float moveSpeed = 5;
    public Transform[] moveToPositions;
    int currentPosition = 0;

    PlayerController playerController;

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(MoveInDirection());
        playerController = FindObjectOfType<PlayerController>();
    }

    IEnumerator MoveInDirection()
    {
        Vector3 _newPos = moveToPositions[currentPosition].position;
        while (Vector3.Distance(transform.position, _newPos) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, _newPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(waitTime);

        if (currentPosition != moveToPositions.Length - 1)
            currentPosition = currentPosition + 1;
        else
            currentPosition = 0;

        StartCoroutine(MoveInDirection());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(playerController.ResetPlayer());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
