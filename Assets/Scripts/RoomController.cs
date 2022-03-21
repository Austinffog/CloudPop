using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RoomController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private int multiplayerSceneIndex;

    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this); //make script a callback target
        //base.OnEnable();
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
        //base.OnDisable();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room. Multiplayer game has begun.");
        StartGame();
        //base.OnJoinedRoom();
    }

    private void StartGame()
    {
        if (PhotonNetwork.IsMasterClient) //checks if it is hosting the game
        {
            Debug.Log("Starting Game");
            PhotonNetwork.LoadLevel(multiplayerSceneIndex);        
        }
    }
}
