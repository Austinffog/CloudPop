using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

namespace com.cloudpop
{
    public class GameManager : MonoBehaviour
    {
        //public string playerPrefab;
        public Transform[] spawn;

        private void Start()
        {
            Spawn();
        }

        public void Spawn()
        {
            //creating player object, finding object in prefabs folder named player
            Transform spawnPoint = spawn[Random.Range(0, spawn.Length)];
            PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player"), spawnPoint.position, spawnPoint.rotation);
            //PhotonNetwork.Instantiate(playerPrefab, spawn.position, spawn.rotation);
        }

    }
}
