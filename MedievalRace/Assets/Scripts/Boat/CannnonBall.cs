using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannnonBall : MonoBehaviour
{
    public float startForce = 10f;

    private void Start()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * startForce);
    }

    public void OnCollisionStay(Collision collision)
    {
        Destroy(gameObject);
    }
}
