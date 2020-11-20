using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    public Text timer;
    float roundTime = 180.0f;
    
    void Start()
    {
        PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(-16.94f, 9f, 4.34f), Quaternion.identity, 0);
    }

    void Update()
    {
        roundTime -= Time.deltaTime;
        timer.text = "Time Left: " + roundTime.ToString();
    }
}
