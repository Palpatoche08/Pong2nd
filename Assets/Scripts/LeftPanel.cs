using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LeftPanel : MonoBehaviour
{
    public float moveSpeed = 5f; 
    public float maxYPosition = 4.5f;
    private bool goUp = false;
    private bool goDown = false;

    public AudioSource bounceSound;


    void Update()
    { 
        CheckInput();     
        MovePanel();
    }

    private void CheckInput()
    {
        if(transform.position.x < 0){
            goUp = Input.GetKey(KeyCode.W);
            goDown = Input.GetKey(KeyCode.S);
        }
        else{
            goUp = Input.GetKey(KeyCode.UpArrow);
            goDown = Input.GetKey(KeyCode.DownArrow);
        }

    }

    private void MovePanel()
    {   
        Rigidbody rb = GetComponent<Rigidbody>();

        Vector3 movement = Vector3.zero;
        if (goUp)
            movement = Vector3.up;
        else if (goDown)
            movement = Vector3.down;

        rb.velocity = movement * moveSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Border") || collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
        }

        if (collision.gameObject.CompareTag("Ball"))
        {
            // Play the bounce sound when the ball collides with the left panel
            if (bounceSound != null)
            {
                // Get the speed of the ball
                float ballSpeed = collision.relativeVelocity.magnitude;

                
                float pitch = Mathf.Clamp(ballSpeed / 10f, 0.5f, 2f);  
                bounceSound.pitch = pitch;

                bounceSound.Play();
            }
        }
    }

     public void RaisePanelHeight(float targetScaleY)
    {
        Vector3 newScale = transform.localScale;
        newScale.y = targetScaleY;
        transform.localScale = newScale;
    }

    public void ShrinkPanel(float targetScaleY)
    {
        Vector3 newScale = transform.localScale;
        newScale.y = targetScaleY;
        transform.localScale = newScale;
    }

}