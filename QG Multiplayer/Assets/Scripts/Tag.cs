using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Tag : MonoBehaviourPun
{
    public GameObject player;
    public Material taggedMaterial;
    public Material defaultMaterial;

    [PunRPC]
    void TaggedColor(){
        player.GetComponent<MeshRenderer>().material = taggedMaterial;
        player.tag = "TaggedPlayer";
    }
    [PunRPC]
    void DefaultColor(){
        player.GetComponent<MeshRenderer>().material = defaultMaterial;
        player.tag = "Player";
    }

    void Start(){
        if (photonView.IsMine && player.tag == "Player"){
            this.photonView.RPC("DefaultColor", RpcTarget.All);
        }
        else if (photonView.IsMine && player.tag == "TaggedPlayer"){
            this.photonView.RPC("TaggedColor", RpcTarget.All);
        }
    }
    
    void OnTriggerEnter(Collider other){
        if (photonView.IsMine == false){
            return;
        }
        //getting tagged by another player
        else if (other.tag == "TaggedPlayer" && player.tag == "Player"){
            this.photonView.RPC("TaggedColor", RpcTarget.All);
            Debug.Log("I've been tagged");
        }
        //tagging another player
        else if (other.tag == "Player" && player.tag == "TaggedPlayer"){
            this.photonView.RPC("DefaultColor", RpcTarget.All);
            Debug.Log("I tagged someone");
        }
    }
}

