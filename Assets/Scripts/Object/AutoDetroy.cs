using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AutoDetroy : NetworkBehaviour
{
    public float delayBeforeDestroy = 1f;

    private void Start()
    {
        DestroyAfterDelayServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void DestroyAfterDelayServerRpc()
    {
        GetComponent<NetworkObject>();
        Destroy(gameObject, delayBeforeDestroy);
    }
}
