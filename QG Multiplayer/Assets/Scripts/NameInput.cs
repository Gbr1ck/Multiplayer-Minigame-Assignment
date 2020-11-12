using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

namespace Com.QuarantineGang.QGMultiplayer
{
[RequireComponent(typeof(InputField))]
public class NameInput : MonoBehaviour
{
    //a simple constant for use in later code
    const string playerNamePrefKey = "PlayerName";

    //below code gets the player's nickname for use in the Photon room
    void Start()
    {
        string defaultName = string.Empty;
        InputField _inputField = this.GetComponent<InputField>();
        if (_inputField!=null){
            if (PlayerPrefs.HasKey(playerNamePrefKey)){
                defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                _inputField.text = defaultName;
            }
        }
        PhotonNetwork.NickName = defaultName;
    }
    //this code gets the value from the inputfield and saves the player's chosen nickname even if the game is closed
    public void SetPlayerName(string value){
        if (string.IsNullOrEmpty(value)){
            Debug.LogError("Player Name is null or empty");
            return;
        }
        PhotonNetwork.NickName = value;

        PlayerPrefs.SetString(playerNamePrefKey, value);
    }
}
}