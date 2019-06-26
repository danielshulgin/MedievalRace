using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatCollision : MonoBehaviour
{
    public int damage = 10;

    public BoatStats boatStats;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "CannonBall")
        {
            boatStats.Damage(damage);
        }
    }

}
