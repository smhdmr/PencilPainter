using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkBottle : MonoBehaviour
{
    public Color inkColor;
    void Start()
    {
        transform.GetChild(0).GetComponent<Renderer>().material.color = inkColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Player>().CollectInk(inkColor);
        Destroy(gameObject);
    }

}
