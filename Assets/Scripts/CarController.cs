using Unity.VisualScripting;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float acceleration = 5f;
    public float deceleration = 5f;
    public float maxSpeed = 60f;
    public float upDownSpeed = 2f;
    private float currentSpeed = 0f;
    public float speedMultiplier = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {



    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);

        }
        if (Input.GetKey(KeyCode.D))
        {
            currentSpeed -= acceleration * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);
        }

    
        //if(Input.GetKey(KeyCode.b)) //something to use for braking
        //{
        //    currentSpeed = Lerp(currentSpeed, currentSpeed, 0f);
        //}
        
        //transform.Translate(currentSpeed * Vector2.left * Time.deltaTime);
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddForce(currentSpeed * Vector2.left * Time.deltaTime * speedMultiplier);

        //if (Input.GetKey(KeyCode.W))
        //{
        //    transform.Translate(Vector2.up * upDownSpeed * Time.deltaTime);
        //}
        //else if (Input.GetKey(KeyCode.S))
        //{
        //    transform.Translate(Vector2.down * upDownSpeed * Time.deltaTime);
        //}
    }
}
