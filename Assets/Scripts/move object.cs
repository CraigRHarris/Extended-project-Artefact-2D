using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class moveobject : MonoBehaviour
{
    public float speed = 5f; // set default speed

    public Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.AddForceX(-speed);
    }
}
