using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class moveobject : MonoBehaviour
{
    public float speed = 5f; // set default speed

    // Update is called once per frame
    void Update()
    {
       // if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
    }
}
