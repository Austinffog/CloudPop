using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

namespace com.cloudpop
{
    public class Pause : MonoBehaviour
    {
        public static bool paused = false;
        private bool disconnecting = false;

        public void TogglePause()
        {
            if (disconnecting)
            {
                return;
            }
            paused = !paused;

            transform.GetChild(0).gameObject.SetActive(paused); //get child element and set active when paused
            Cursor.lockState = (paused) ? CursorLockMode.None : CursorLockMode.Confined; 
            Cursor.visible = paused; //cursor is visible when paused

        }

        public void Quit()
        {
            disconnecting = true;
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene(0);
        }

    }
}
