using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    public float rotationSpeed = 30f;
    public float powerUpDuration = 10f;
    public bool increasesSpeed = true; // Indicates whether the power-up increases the ball speed

    private bool isCaught = false;
    private float timeElapsed = 0f;
    private float movementSpeed;

    void Start()
    {
        if (increasesSpeed)
        {
            movementSpeed = 3f;
        }
        else
        {
        }
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        if (!isCaught)
        {
            float xPosition = Mathf.PingPong(Time.time * movementSpeed, 8f) - 4f;
            float yPosition = Mathf.PingPong(Time.time * movementSpeed, 8f) - 4f;
            transform.position = new Vector3(xPosition, yPosition, 0f);
        }

        if (isCaught)
        {
            timeElapsed += Time.deltaTime;

            if (timeElapsed >= powerUpDuration)
            {
                ResetPowerUp();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            isCaught = true;
            ApplyPowerUpEffect(other.GetComponent<Collider>());
            
            if (gameObject != null)
            {
                gameObject.SetActive(false); 
            }
        }
    }

    void ApplyPowerUpEffect(Collider ballCollider)
    {

        if (ballCollider != null)
        {
            BallController ballController = ballCollider.gameObject.GetComponent<BallController>();

            if (ballController != null)
            {
                ballController.IncreaseBallSpeed(5f);
            }
        }
    }

    void ResetPowerUp()
    {
        isCaught = false;
        timeElapsed = 0f;

        float xPosition = Random.Range(-4f, 4f);
        float yPosition = Random.Range(-4f, 4f);
        transform.position = new Vector3(xPosition, yPosition, 0f);

        gameObject.SetActive(true);
    }
}