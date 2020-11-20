using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Tag : MonoBehaviourPun
{
    public GameObject player;
    public Color taggedColor = Color.yellow;
    public Color defaultColor = Color.gray;

    [PunRPC]
    void TaggedColor(){
        player.GetComponent<MeshRenderer>().material.color = taggedColor;
    }
    [PunRPC]
    void DefaultColor(){
        player.GetComponent<MeshRenderer>().material.color = defaultColor;
    }

    void Start(){
        if (photonView.IsMine){
            this.photonView.RPC("DefaultColor", RpcTarget.All);
        }
    }
    
    void OnTriggerEnter(Collider other){
        if (photonView.IsMine == false){
            return;
        }
        //getting tagged by another player
        else if (other.tag == "TaggedPlayer" && player.tag == "Player"){
            this.photonView.RPC("TaggedColor", RpcTarget.All);
            player.tag = "TaggedPlayer";
        }
        //tagging another player
        else if (other.tag == "Player" && player.tag == "TaggedPlayer"){
            this.photonView.RPC("DefaultColor", RpcTarget.All);
            player.tag = "Player";
        }
    }
}
