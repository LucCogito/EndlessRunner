using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rooftop : MonoBehaviour
{
    [field:SerializeField]
    public float Width { get; private set; }
    [field: SerializeField]
    public Rigidbody2D Rigidbody { get; private set; }

    private void OnBecameInvisible()
    {
        if (transform.position.x < 0f)
            Destroy(gameObject);
    }
}
