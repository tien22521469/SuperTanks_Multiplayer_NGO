using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileGun : MonoBehaviour
{

    public int player = 1;

    public GameObject missilePrefab;

    public Transform fireTransform;

    public float fireForce = 2000;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    { 
        if (Input.GetButtonDown("Fire" + player))
        {
            GameObject missile = Instantiate(missilePrefab, fireTransform.position, transform.rotation);

            Rigidbody missileBody = missile.GetComponent<Rigidbody>();

            missileBody.AddForce(transform.forward * fireForce);

        }
    }
}
