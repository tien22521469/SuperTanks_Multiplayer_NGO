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


    private void Start()
    {
        if (!SuperTanksGameManager.Instance.IsGamePlaying()) return;

        GameInput.Instance.HaveMissileAction += (sender, e) => FireMissile();
    }

    // Update is called once per frame
    void FireMissile()
    {
        if (!IsOwner) return;
        /*
        if (Input.GetButtonDown("Fire"+player))
        { 
            Debug.Log("Fire");
            CreateMissileServerRpc();
            //Thông báo ra console
            
        }    */
        CreateMissileServerRpc();

  
    }
    //Tạo Rpc để tạo tên lửa
    [ServerRpc(RequireOwnership = false)]
    public void CreateMissileServerRpc()
    {


        // lấy tên lửa từ prefab và vị trí bắn
        GameObject missile = Instantiate(missilePrefab, fireTransform.position, transform.rotation);
        
        spawnedMissile.Add(missile);

        missile.GetComponent<MissileExplosion>().parent = this;

        NetworkObject networkObject = missile.GetComponent<NetworkObject>();   

        networkObject.GetComponent<NetworkObject>().Spawn();
        //tạo lực cho tên lửa
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
        //in đối tượng đó ra console

    }
}
