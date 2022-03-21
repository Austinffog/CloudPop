using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkController : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); //connect game to the master server
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Base connected to " + PhotonNetwork.CloudRegion + "server."); //print server location
        //base.OnConnectedToMaster();
    }

}
