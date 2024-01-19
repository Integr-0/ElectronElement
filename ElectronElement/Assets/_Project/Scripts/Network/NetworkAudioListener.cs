using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(AudioListener))]
public class NetworkAudioListener : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) GetComponent<AudioListener>().enabled = false;
    }
}