using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using static Unity.Burst.Intrinsics.Arm;

public class MissileGun : NetworkBehaviour
{
    public int player = 1;

    public float fireForce = 2000;

    [SerializeField] public GameObject missilePrefab;

    [SerializeField] public Transform fireTransform;
    //để kiểm soát các thực thể tên lửa
    [SerializeField] public List<GameObject> spawnedMissile = new List<GameObject>();


    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        if (!SuperTanksGameManager.Instance.IsGamePlaying()) return;

        if (Input.GetButtonDown("Fire"+player))
        { 
            CreateMissileServerRpc();
        }    
  
    }
    //Tạo Rpc để tạo tên lửa
    [ServerRpc]
    public void CreateMissileServerRpc()
    {
        // lấy tên lửa từ prefab và vị trí bắn
        GameObject missile = Instantiate(missilePrefab, fireTransform.position, transform.rotation);
        
        spawnedMissile.Add(missile);

        missile.GetComponent<MissileExplosion>().parent = this;
        NetworkObject networkObject = missile.GetComponent<NetworkObject>();   

        networkObject.GetComponent<NetworkObject>().Spawn();

        Rigidbody missileRigidbody = networkObject.GetComponent<Rigidbody>();

        missileRigidbody.AddForce(fireTransform.forward * fireForce);

    }
    //Tạo Rpc để hủy tên lửa
    [ServerRpc(RequireOwnership = false)]
    public void DestroyMissileServerRpc()
    {
        GameObject toDestroy = spawnedMissile[0];
        NetworkObject destroyNetworkObject = toDestroy.GetComponent<NetworkObject>();
        destroyNetworkObject.Despawn();
        spawnedMissile.Remove(toDestroy);
        Destroy(toDestroy);
    }
}
