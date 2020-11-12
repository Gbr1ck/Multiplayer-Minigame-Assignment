using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Com.QuarantineGang.QGMultiplayer
{
    
public class Launcher : MonoBehaviourPunCallbacks
{
    //defines maximum player size, and serializes so we can change it in unity on the fly
    [Tooltip("The maximum number of players per room.")]
    [SerializeField]
    private byte maxPlayersPerRoom = 4;

    //variable to check if the player is currently connecting, so it won't automatically connect when they leave the room
    bool isConnecting;

    //syncs the scene with all players
    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    //changes UI elements based on whether player is currently connecting to a room
    void Start()
    {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }
    //joins one of the active rooms, if available
    public void Connect()
    {
        progressLabel.SetActive(true);
        controlPanel.SetActive(false);
        if (PhotonNetwork.IsConnected){
            PhotonNetwork.JoinRandomRoom();
        }
        else{
            isConnecting = PhotonNetwork.ConnectUsingSettings();
        }
    }

    [Tooltip("The UI Panel to let the user enter name, connect and play")]
    [SerializeField]
    private GameObject controlPanel;
    [Tooltip("The UI Label to inform the user that the connection is in progress")]
    [SerializeField]
    private GameObject progressLabel;

    //begins connection
    public override void OnConnectedToMaster(){
        Debug.Log("Launcher: OnConnectedToMaster called");
        if (isConnecting){
            PhotonNetwork.JoinRandomRoom();
            isConnecting = false;
        }
    }
    //necessary function for logging when a player disconnects, and keeps the game from automatically reconnecting
    public override void OnDisconnected(DisconnectCause cause){
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
        isConnecting = false;
        Debug.LogWarningFormat("Launcher: Disconnected with reason {0}", cause);
    }
    //creates a new room if none are available
    public override void OnJoinRandomFailed(short returnCode, string message){
        Debug.Log("Launcher: No random room available, creating new one...");
        PhotonNetwork.CreateRoom(null, new RoomOptions{MaxPlayers = maxPlayersPerRoom});
    }
    //once a room is joined, loads the level
    public override void OnJoinedRoom(){
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1){
            PhotonNetwork.LoadLevel("Main");
        }
        Debug.Log("Launcher: Room created, client connected");
    }

}
}