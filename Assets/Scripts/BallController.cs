using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class BallController : MonoBehaviour
{
    public float initialSpeed = 6f;
    private float currentSpeed;
    private Vector3 startPosition;
    private int leftcount = 0;
    private int rightcount = 0;

    
    public bool pause = false;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI highScoreText;
    private int HighLeft = 0;
    private int HighRight = 0;

    public GameObject LeftWinTextObject;
    public GameObject RightWinTextObject;

    private float timeUntilSurprise = 20f;
    private float startTime;
    private bool surpriseActive = false;
    public GameObject surpriseText;
    public AudioSource audioSource;

    private Vector3 originalScale;

    private bool isDecreasedSize = false;
    private float sizeDecreaseDuration = 20f;
    private float sizeDecreaseTimer = 0f;



    private float stationaryTimer = 0f;

    void Start()
    {
        startTime = Time.time;
        surpriseText = GameObject.Find("SurpriseText"); 
        surpriseText.SetActive(false);

        originalScale = transform.localScale;
        
        startPosition = transform.position;
        currentSpeed = initialSpeed;

        LaunchBall();

        audioSource = GetComponent<AudioSource>();

        leftcount = 0;
        rightcount = 0;

        highScoreText.text = "HighScore : " + "\n" + leftcount + "   |   " + rightcount;

        SetCountText();

        LeftWinTextObject.SetActive(false);
        RightWinTextObject.SetActive(false);

       

    
    }

   void Update()
    {


        CheckGoals();

        if (isDecreasedSize)
        {
            sizeDecreaseTimer += Time.deltaTime;

            if (sizeDecreaseTimer >= sizeDecreaseDuration)
            {
                ResetBallSize();
            }
        }

        if (!pause)
        { 

            if (!surpriseActive && Time.time >= timeUntilSurprise)
            {
                ActivateSurpriseEvent();
            }

            if (GetComponent<Rigidbody>().velocity.magnitude >= 2f && surpriseActive)
            {
                surpriseActive = false;
                surpriseText.SetActive(false);
                ResetSurpriseTimer();
            }

            if (GetComponent<Rigidbody>().velocity.magnitude < 2f)
            {
                stationaryTimer += Time.deltaTime;

                if ((stationaryTimer >= 0.5f && stationaryTimer < 1.5f) && Time.time - startTime >= 3f)
                {
                    if (!surpriseActive)
                    {
                        ActivateSurpriseEvent();
                    }
                }
                else if (stationaryTimer >= 1.5f)
                {
                    surpriseActive = false;
                    surpriseText.SetActive(false);
                    ResetSurpriseTimer();
                }
            }
            else
            {
                stationaryTimer = 0f;
            }
        }
    }

    public void DecreaseBallSize()
    {
        
        transform.localScale *= 0.5f; 
    }


    void ActivateSurpriseEvent()
    {
    surpriseActive = true;
    surpriseText.SetActive(true);
    audioSource.Play();

    GetComponent<Rigidbody>().velocity = Vector3.zero;
    GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

    StartCoroutine(RestartBallAfterDelay());
}

    void ResetBallSize()
    {
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        isDecreasedSize = false;
        sizeDecreaseTimer = 0f;
    }


IEnumerator RestartBallAfterDelay()
{
   
    yield return new WaitForSeconds(1.5f);

    if (transform.position.x < 0)
    {
        GetComponent<Rigidbody>().AddForce(Vector3.right * 20f, ForceMode.VelocityChange);
    }
    else
    {
        GetComponent<Rigidbody>().AddForce(Vector3.left * 20f, ForceMode.VelocityChange);
    }

    
    surpriseActive = false;
    surpriseText.SetActive(false);

}

void ResetSurpriseTimer()
{
    timeUntilSurprise = Time.time + UnityEngine.Random.Range(20f, 40f);
}

            
    

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Panel") )
        {
            BounceAndAccelerate(collision.contacts[0].normal);
        }
        if (collision.gameObject.CompareTag("Border"))
        {
            Bounce(collision.contacts[0].normal);
        }
    }

    void CheckGoals()
    {
        if (transform.position.x < -11f)
        {
            ScorePoint("RightPlayer");
            
        }
        else if (transform.position.x > 10.5f) 
        {
            ScorePoint("LeftPlayer");
        }
    }

    void ScorePoint(string playerTag)
    {
        
        transform.position = startPosition;
        currentSpeed = initialSpeed;

        Debug.Log(playerTag + " scores a point!");

        if (playerTag == "LeftPlayer")
        {           
            leftcount = leftcount + 1;
        }
        else
        {
            rightcount = rightcount + 1;
        }

        if (leftcount > rightcount)
        {
            countText.color = Color.cyan;
        }
        else if (leftcount < rightcount)
        {
            countText.color = Color.yellow;
        }
        else
        {
            countText.color = Color.white;
        }

        SetCountText();

        LaunchBall();
    }

    void BounceAndAccelerate(Vector3 collisionNormal)
    {
        float maxAngle = 10f;
        float randomAngle = UnityEngine.Random.Range(1f, maxAngle);
    
        Vector3 reflectedVelocity = Vector3.Reflect(GetComponent<Rigidbody>().velocity, collisionNormal);

        
        Quaternion rotation = Quaternion.AngleAxis(randomAngle, Vector3.forward);
        reflectedVelocity = rotation * reflectedVelocity;

        GetComponent<Rigidbody>().velocity = reflectedVelocity.normalized * currentSpeed;

        currentSpeed += 1.5f; //
    }

    void Bounce(Vector3 collisionNormal)
    {
        float maxAngle = 10f;
        float randomAngle = UnityEngine.Random.Range(1f, maxAngle);
    
        Vector3 reflectedVelocity = Vector3.Reflect(GetComponent<Rigidbody>().velocity, collisionNormal);
       
        Quaternion rotation = Quaternion.AngleAxis(randomAngle, Vector3.forward);
        reflectedVelocity = rotation * reflectedVelocity;

        GetComponent<Rigidbody>().velocity = reflectedVelocity.normalized * currentSpeed;

    }




    void LaunchBall()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        Vector3 initialDirection = new Vector3(UnityEngine.Random.Range(0.5f, 1f), UnityEngine.Random.Range(-1f, 1f), 0f).normalized;
        rb.velocity = initialDirection * currentSpeed;
        rb.angularVelocity = Vector3.zero;
        rb.freezeRotation = true;
    }

    void SetCountText() 
    {
        countText.text = leftcount + "            " + rightcount;

        if (leftcount >= 7)
        {
            LeftWinTextObject.SetActive(true);
            currentSpeed = 0f;
            StartCoroutine(RestartGameAfterDelay());
        }
        else if (rightcount >= 7)   
        {
            RightWinTextObject.SetActive(true);
            currentSpeed = 0f;
            StartCoroutine(RestartGameAfterDelay());
        }
    }

    IEnumerator RestartGameAfterDelay()
    {
        pause = true;
        if (HighLeft == HighRight)
        {
            HighLeft = leftcount;
            HighRight = rightcount;
        }
        else
        {
            if (Math.Min(HighLeft, HighRight) > Math.Min(leftcount, rightcount))
            {
                HighLeft = leftcount;
                HighRight = rightcount;
            }
        }

        highScoreText.text =  "HighScore : " + "\n" + HighLeft + "   |   " + HighRight;

        yield return new WaitForSeconds(3f);

        leftcount = 0;
        rightcount = 0;
        SetCountText();

        LeftWinTextObject.SetActive(false);
        RightWinTextObject.SetActive(false);

        pause = false;
        currentSpeed = 6f;

        LaunchBall();
    }

    public void IncreaseBallSpeed(float speedIncrease)
    {
        currentSpeed += speedIncrease;
    }
    
}
