using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderFallingLetter : MonoBehaviour
{
    private float curDownTime = 0f;
    private float maxDownTime = 0.1f;
    private Vector3 targetPosition;
    private float fallSpeed = 7f; 

    private LetterFactory letterFactory;

    void Start()
    {
        letterFactory = GameObject.Find("LetterFactory").GetComponent<LetterFactory>();
        targetPosition = transform.position;
    }

    void Update()
    {
        if (letterFactory.isLoaded)
        {
            curDownTime += Time.deltaTime;

            if (curDownTime >= maxDownTime && transform.position.y > 0)
            {
                targetPosition = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
                curDownTime = 0f;
            }

            transform.position = Vector3.Lerp(transform.position, targetPosition, fallSpeed * Time.deltaTime);

            if (transform.position.y < 0)
            {
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            }
        }
    }
}
