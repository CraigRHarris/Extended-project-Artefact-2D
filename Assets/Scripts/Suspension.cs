using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public float springStrength = 2.00f;
    public float springDamper = 10.00f;

    float rayDidHit;

    public float carRigidBody;
    float tireTransform;
    float suspensionRestDist;
    float tireRay;
   
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // suspension spring force
        if (rayDidHit)
        {
            //world-space direction of the spring force.
            Vector3 springDir = tireTransform.up;

            //world-space velocity of this tire
            Vector3 tireWorldVel = carRigidBody.GetPointVelocity(tireTransform.postion);

            // calculate offset from the raycast
            float offset = suspensionRestDist - tireRay.distance;

            // calculate velocity along the sring direction
            //note that springDir is a unit vector, so this returns the magnitude of tireWorldVel
            // as projected onto springDir
            float vel = Vector3.Dot(springDir, tireWorldvel);

            //calculate the magnitude of the dampenedspring force!
            float force = (offset * springStrength) - (vel * springDamper);

            //apply the force at the ;oction of this tire , in the dirction of the suspension
            carRigidBody.AddForceAtPostion(springDir *  force, tireTransform.position);
        }


    }
}
