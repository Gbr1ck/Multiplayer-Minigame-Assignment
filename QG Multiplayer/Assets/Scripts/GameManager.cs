using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    
    void Start()
    {
        PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(-16.94f, 9f, 4.34f), Quaternion.identity, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
