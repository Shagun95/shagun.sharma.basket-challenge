using System;
using UnityEngine;

public class BasketBallController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BasketTrigger")
        {
            Debug.Log("Point Scored!");
        }
    }
}
