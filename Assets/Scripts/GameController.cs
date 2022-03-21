using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CreatePlayer();
    }

    void CreatePlayer()
    {
        Debug.Log("Creating Player");
        //creating player object, finding object in prefabs folder named player
        PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player"), Vector3.zero, Quaternion.identity);
    }
}
