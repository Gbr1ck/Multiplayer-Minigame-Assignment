using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Tag : MonoBehaviourPunCallbacks
{
    public GameObject player;
    public Material taggedMaterial;
    public Material defaultMaterial;
    [SerializeField]
    bool isTagged = false;
    
    void OnTriggerEnter(Collider other){
        if (photonView.IsMine == false){
            return;
        }
        else if (other.tag == "Player"){
            if (isTagged == true){
                isTagged = false;
                player.GetComponent<MeshRenderer>().material = defaultMaterial;
            }
            else if (isTagged == false){
                isTagged = true;
                player.GetComponent<MeshRenderer>().material = taggedMaterial;
            }
        }
    }
}
