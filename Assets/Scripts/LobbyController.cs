using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class LobbyController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text buttonText;
    [SerializeField]
    private int roomSize;
    private bool connected, starting;

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true; //synchronizes current scene with Photon network
        connected = true;
        buttonText.text = "Begin Game";
        //base.OnConnectedToMaster();
    }

    public void GameButton()
    {
        if (connected) //check if game has connected to the master server
            {
            if (!starting)
            {
                starting = true;
                buttonText.text = "Starting Game. Click Again to Cancel";
                PhotonNetwork.JoinRandomRoom(); //attempt joining a room
            }
            else
            {
                starting = false;
                buttonText.text = "Begin Game";
                PhotonNetwork.LeaveRoom(); //cancel the request
            }
        }
        else
        {
            Debug.Log("Not Connected to Server!");
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join a room... creating room.");
        CreateRoom();
         //base.OnJoinRandomFailed(returnCode, message);
    }

    void CreateRoom()
    {
        /*
         * get random room number
         * room is visible to other copies, room is open and limits size in room
         * asks photonNetwork to create a room
         * if room number is already taken , a similar method will be called and process will start over
         * */
        Debug.Log("Creeating Room.");
        int randomRoomNumber = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
        PhotonNetwork.CreateRoom("Room" + randomRoomNumber, roomOps);
        Debug.Log(randomRoomNumber);

    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create a room, trying again");
        CreateRoom();
    }

}
