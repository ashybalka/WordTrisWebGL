using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class FigureController : MonoBehaviour
{
    public char figureChar;

    private float maxDownTime = 0.6f;
    private float maxLeftTime = 0.6f;
    private float maxRightTime = 0.6f;

    private float curDownTime = 0f;
    private float curLeftTime = 0f;
    private float curRightTime = 0f;

    private GameManager gameManager;
    private RayController rayController;

    public bool isCollided = false;
    public bool isFalling = true;

    private float downSpeed = 1f;
    private float leftSpeed = 5f;
    private float rightSpeed = 5f;


    public bool isBomb = false;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        rayController = GameObject.Find("RayCasters").GetComponent<RayController>();
    }

    void Update()
    {
        if (!isCollided && isFalling)
        {
            var leftRay = Physics2D.Raycast(new Vector2(transform.position.x - 0.51f, transform.position.y - 0.51f), Vector2.left, 0.48f);
            if (leftRay.collider == null)
            {
                Moveleft();
            }

            var rightRay = Physics2D.Raycast(new Vector2(transform.position.x + 0.51f, transform.position.y - 0.51f), Vector2.right, 0.48f);
            if (rightRay.collider == null)
            {
                MoveRight();
            }
        }    



        var downRay = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.51f), Vector2.down, 0.48f);
        if (downRay.collider == null )
        {
            FallingDown();
        }
    }

    public void Moveleft()
    {
        curLeftTime += Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            leftSpeed = 5f;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            leftSpeed = 0f;
        }

        if (Input.GetKey(KeyCode.A) && curLeftTime >= (maxLeftTime / leftSpeed) && transform.position.x > -4f)
        {
            transform.position += new Vector3(-1f, 0, 0);
            curLeftTime = 0f;
        }
    }

    public void MoveRight()
    {
        curRightTime += Time.deltaTime;

        if (Input.GetKey(KeyCode.D))
        {
            rightSpeed = 5f;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            rightSpeed = 0f;
        }

        if (Input.GetKey(KeyCode.D) && curRightTime >= (maxRightTime / rightSpeed) && transform.position.x < 4f)
        {
            transform.position += new Vector3(1f, 0, 0);
            curRightTime = 0f;
        }
    }

    public void FallingDown()
    {
        curDownTime += Time.deltaTime;

        if (Input.GetKey(KeyCode.S))
        {
            downSpeed = 5f;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            downSpeed = 1f;
        }

        float levelSpeed = 1 + (gameManager.level / 10);

        if (curDownTime >= (maxDownTime / downSpeed / levelSpeed) && transform.position.y > -3.5)
        {
            transform.position += new Vector3(0, -1f, 0);
            curDownTime = 0f;
        }
    }


    public void OnCollisionStay2D(Collision2D collision)
    {
        if (isBomb)
        {
            gameManager.ExplodeBomb(gameObject.transform.position);
        }


        if (!isCollided && !isBomb)
        {
            if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Figure"))
            {
                if (gameObject.transform.position.y > 5)
                {
                    gameManager.GameOver();
                }

                isCollided = true;
                if (isFalling)
                {
                    gameManager.FalingRutine();
                }
                isFalling = false;
            }
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (isCollided)
        {
            isCollided = false;
        }
    }

    public void SetCharToTMP(char letter)
    {
        gameObject.GetComponentInChildren<TMP_Text>().text = letter.ToString();
    }
}
