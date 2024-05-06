using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;

public class MissileExplosion : NetworkBehaviour
{
    public MissileGun parent;

    public GameObject explosionPrefab;

    public float explosionRadius = 5;

    public float explosionForce = 1000;
   
    

    private void OnCollisionEnter(Collision collision)
    {
        if(!IsOwner) return;
        InstantiateCollisionEnterServerRpc();
        //Sẽ không thực hiện khi đối tượng nổ của đối tượng tên lửa không được phá huỷ loại bỏ ra khỏi cảnh
        //Nếu đối tượng nổ được loại ra khỏi cảnh thì sẽ loại bỏ được đối tượng cảnh tên lửa trong cảnh 
        parent.DestroyMissileServerRpc();   
    }
    //thực thi khi là máy chủ
    [ServerRpc]
    private void InstantiateCollisionEnterServerRpc()
    {
        GameObject hitImpact=Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        NetworkObject networkObject= hitImpact.GetComponent<NetworkObject>();
        networkObject.Spawn();
        hitImpact.transform.localEulerAngles =new Vector3(0f,0f,-90f);

        AutoDetroy autoDetroy = networkObject.GetComponent<AutoDetroy>();
    }
}
