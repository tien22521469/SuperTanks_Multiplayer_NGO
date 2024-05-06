
# Lập trình mạng cùng với Unity 
***
## Phạm vi 
Dự án được thực hiện trong phạm vi môn học Lập trình mạng căn bản NT106.O22 - UIT K17

## Mục tiêu
- Tìm hiểu về lập trình mạng căn bản xây dựng và hiểu được cấu trúc xử lí các gói tin trong mạng
- Cơ bản về thư viện Netcode for Game Object
- Game có thể chơi nhiều người chơi 

## Nguồn gốc
Game gốc được tham khảo từ https://github.com/colonelsalt/SuperTanks

### Giao diện 

Khi mở game lên thì sẽ xuất hiện 2 đối tượng xe tăng - màu xanh và đỏ, và chơi trên cùng một máy 
- nhấp chuột trái sẽ bắn xe xanh , phải sẽ bắn xe đỏ
Luật chơi 
- Có 2 thanh ghi điểm ở 2 góc trái và phải để ghi điểm số của từng đối tượng.
- Khi một đối tượng bị tác động bởi một vật thể nổ thì thanh điểm của bên còn lại sẽ được 

### Hạn chế

- Chỉ chơi được một máy duy nhất và trên máy đó.
- Chỉ có 2 đối tượng chơi với nhau.

===
## Phát triển
### Chủ đề
- Thiết kế lại cảnh game
- Thêm nhiều người chơi 
- Thiết kế lại luật chơi 
- Thêm tính năng chat 

### Chuẩn bị
- MSP Unity StarterPack https://onedrive.live.com/?authkey=%21AFgnoQPxEyxmN34&cid=73E165CE52BCC8D8&id=73E165CE52BCC8D8%21983&parId=root&o=OneUp
>_You can import custom asset packs by clicking on Assets -> Import Package -> Custom Package… in Unity’s menu bar at the top of the screen._

&copy; https://learn.microsoft.com/vi-vn/archive/blogs/uk_faculty_connection/making-games-with-c-and-unity-beginners-tutorial

- Tải thư viện Netcode for Game Object : com.unity.netcode.gameobjects

&copy; https://docs-multiplayer.unity3d.com/netcode/current/installation/

- Sử dụng ParrelSync để có thể tạo một bản sau tương tự project gốc để có thể tương tác với nhau (bản sau sẽ tự lưu giống bản gốc sau mỗi lần Save) : 

&copy; https://github.com/VeriorPies/ParrelSync

## Thực hiện 

### Thiết kế lại cảnh

### Thêm nhiều người chơi 

#### Điều chỉnh di chuyển
- Scipt Player.cs là điểu khiển di chuyển của đối tượng

```cpp
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Netcode;

public class Player : NetworkBehaviour
{
    // How quickly player moves forward and back
    private float speed = 10f;

    // How quickly player rotates (degrees per second)

    private float rotationSpeed = 180f;

    private Rigidbody body;
    // Use this for initialization
    void Start()
    {
        // Retrieve reference to this GameObject's Rigidbody component
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }


    //Xử lí di chuyển của player
    private void HandleMovement()
    {
        if (!IsOwner) return;
        if (!SuperTanksGameManager.Instance.IsGamePlaying()) return;
         // Check if GameInput.Instance is null
        if (GameInput.Instance == null)
        {
            Debug.LogError("GameInput.Instance is null");
            return;
        }
        // Get movement input value
        float movementInput = GameInput.Instance.GetMovementInput();
       
        // Determine amount to move based on current forward direction and speed
        Vector3 movement = transform.forward * movementInput * speed * Time.deltaTime;

        // Move our Rigidbody to this position
        body.MovePosition(body.position + movement);

        // Get rotation input value
        float rotationInput = GameInput.Instance.GetRotationInput();

        // Determine number of degrees to turn based on rotation speed
        float degreesToTurn = rotationInput * rotationSpeed * Time.deltaTime;

        // Get Quaternion equivalent of this amount of rotation around the y-axis
        Quaternion rotation = Quaternion.Euler(0f, degreesToTurn, 0f);

        // Rotate our Rigidbody by this amount
        body.MoveRotation(body.rotation * rotation);
    }
}
```
- Scipt MissileGun.cs
```cpp
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
```

- Script MissileExplosion.cs
```cpp
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
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

        parent.DestroyMissileServerRpc();   
    }
    [ServerRpc]
    private void InstantiateCollisionEnterServerRpc()
    {
        GameObject hitImpact=Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        NetworkObject hitImpactNetworkObject = hitImpact.GetComponent<NetworkObject>();
        hitImpactNetworkObject.Spawn(); 
        hitImpactNetworkObject.transform.localEulerAngles =new Vector3(0f,0f,-90f);
    }
}

```
### Thiết kế lại luật chơi 
