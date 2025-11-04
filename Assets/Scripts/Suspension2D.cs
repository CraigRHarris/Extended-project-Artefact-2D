using UnityEngine;

public class Suspension2D : MonoBehaviour
{
    public Rigidbody2D carBody;
    float Transform tireTransform;
    public float suspensionRestDist = 0.5f;  // Resting distance of suspension
    public float springStrength = 20000f;    // How stiff the spring is
    public float springDamper = 1500f;       // How much damping (resists oscillation)
    public LayerMask groundLayer;

    
    float suspensionRestDist;
    float tireRay;



    private void FixedUpdate()
    {
        // Direction of suspension (local up of the tire)
        Vector2 springDir = tireTransform.up;

        // Start and direction of the suspension ray
        Vector2 rayOrigin = tireTransform.position;
        Vector2 rayDir = -springDir; // usually downwards from the tire

        // Perform a raycast down to detect the ground
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDir, suspensionRestDist, groundLayer);

        if (hit)
        {
            // How much the suspension is compressed
            float offset = suspensionRestDist - hit.distance;

            // Get the tire’s world velocity
            Vector2 tireWorldVel = carBody.GetPointVelocity(tireTransform.position);

            // Velocity along the spring direction (project onto spring direction)
            float vel = Vector2.Dot(springDir, tireWorldVel);

            // Calculate spring force (Hooke’s Law) with damping
            float forceMag = (offset * springStrength) - (vel * springDamper);

            // Apply force upwards at tire position
            carBody.AddForceAtPosition(springDir * forceMag, tireTransform.position);
        }
    }

    private void OnDrawGizmos()
    {
        // Draw ray in editor
        if (tireTransform != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(tireTransform.position, tireTransform.position - tireTransform.up * suspensionRestDist);
        }
    }

//clamp the offset and force
//    offset = Mathf.Clamp(offset, 0, suspensionRestDist);
//if (offset > 0f)
//{
//    float force = (offset * springStrength) - (vel * springDamper);
//    carRigidBody.AddForceAtPosition(springDir* force, tireTransform.position);
//}





    //// Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    // suspension spring force
    //    if (rayDidHit)
    //    {
    //        //world-space direction of the spring force.
    //        Vector2 springDir = tireTransform.up;

    //        //world-space velocity of this tire
    //        Vector2 tireWorldVel = carRigidBody.GetPointVelocity(tireTransform.postion);

    //        // calculate offset from the raycast
    //        float offset = suspensionRestDist - tireRay.distance;

    //        // calculate velocity along the sring direction
    //        //note that springDir is a unit vector, so this returns the magnitude of tireWorldVel
    //        // as projected onto springDir
    //        float vel = Vector2.Dot(springDir, tireWorldvel);

    //        //calculate the magnitude of the dampenedspring force!
    //        float force = (offset * springStrength) - (vel * springDamper);

    //        //apply the force at the ;oction of this tire , in the dirction of the suspension
    //        carRigidBody.AddForceAtPostion(springDir *  force, tireTransform.position);
    //    }


    //}
}
