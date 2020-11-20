using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Tag : MonoBehaviourPunCallbacks
{
    public GameObject player;
    public Color taggedColor = Color.yellow;
    public Color defaultColor = Color.gray;
    void Start(){
        if (photonView.IsMine){
            photonView.RPC("DefaultColor", RpcTarget.All);
        }
    }
    
    void OnTriggerEnter(Collider other){
        if (photonView.IsMine == false){
            return;
        }
        //getting tagged by another player
        else if (other.tag == "TaggedPlayer" && player.tag == "Player"){
            photonView.RPC("TaggedColor", RpcTarget.All);
            player.tag = "TaggedPlayer";
        }
        //tagging another player
        else if (other.tag == "Player" && player.tag == "TaggedPlayer"){
            photonView.RPC("DefaultColor", RpcTarget.All);
            player.tag = "Player";
        }
    }
    [PunRPC]
    public void TaggedColor(){
        player.GetComponent<MeshRenderer>().material.color = taggedColor;
    }
    [PunRPC]
    public void DefaultColor(){
        player.GetComponent<MeshRenderer>().material.color = defaultColor;
    }
}
