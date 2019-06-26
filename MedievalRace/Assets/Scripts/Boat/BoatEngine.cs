using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatEngine : MonoBehaviour
{
    public PanAndZoom panAndZoom;

    public float speed = 1f;
    public float controlForce = 100f;
    public float maxVelocity = 10f;

    void Start()
    {
        panAndZoom.onSwipe += Control;
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * speed);
        if (gameObject.GetComponent<Rigidbody>().velocity.magnitude > maxVelocity)
        {
            gameObject.GetComponent<Rigidbody>().velocity = maxVelocity * gameObject.GetComponent<Rigidbody>().velocity.normalized;
        }
    }

    public void Control(Vector2 direction)
    {
        Vector3 recalculatedDirection = new Vector3(0f, direction.x * controlForce, 0f) * Time.deltaTime;
        gameObject.GetComponent<Rigidbody>().AddTorque(recalculatedDirection);
    }
}
