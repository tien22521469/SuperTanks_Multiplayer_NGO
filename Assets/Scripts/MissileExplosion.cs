using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileExplosion : MonoBehaviour
{

    public GameObject explosionPrefab;

    public float explosionRadius = 5;

    public float explosionForce = 1000;

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        Collider[] collidersHit = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider collider in collidersHit)
        {
            Rigidbody targetBody = collider.GetComponent<Rigidbody>();

            if (targetBody == null)
            {
                continue;
            }

            targetBody.AddExplosionForce(explosionForce, transform.position, explosionRadius);


        }

        Destroy(gameObject);

    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
