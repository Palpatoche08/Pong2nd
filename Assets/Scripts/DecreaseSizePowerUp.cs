using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecreaseSizePowerUp : MonoBehaviour
{
    public float movementRange = 6f;  
    public float movementSpeed = 3f;

    private bool isMovingUp = true;

    void Start()
    {
        transform.position = new Vector3(1.2f, -6f, 0f);
    }

    void Update()
    {
        MoveUpDown();
    }

    void MoveUpDown()
    {
        float newY = isMovingUp ? Mathf.PingPong(Time.time * movementSpeed, movementRange) - 6f : 6f;
        transform.position = new Vector3(1.2f, newY, 0f);

        if (newY >= 6f || newY <= -6f)
        {
            isMovingUp = !isMovingUp;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
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
                ballController.DecreaseBallSize();
            }
        }
    }
}
