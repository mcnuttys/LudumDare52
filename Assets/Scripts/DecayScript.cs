using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecayScript : MonoBehaviour
{
    [SerializeField] private float decayTime = 10;
    private float age;

    void Update()
    {
        age += Time.deltaTime;
        if (age > decayTime) Destroy(gameObject);
    }
}
