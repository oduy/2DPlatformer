using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

namespace ODUY
{
    public class PUNGameManager : MonoBehaviourPunCallbacks
    {

        #region Public Fields

        static public PUNGameManager Instance;

        #endregion

        #region Private Fields

        private GameObject instance;

        [Tooltip("The prefab to use for representing the player")]
        [SerializeField]private GameObject playerRed = default;

        [SerializeField]private GameObject playerBlue = default;

        #endregion

        #region Photon Callbacks
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }
        #endregion

        void Start()
        {
            Instance = this;


            if (playerRed == null || playerBlue == null)
            { // #Tip Never assume public properties of Components are filled up properly, always check and inform the developer of it.

                Debug.LogError("<Color=Red><b>Missing</b></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {
                int number = PhotonNetwork.LocalPlayer.ActorNumber;
                switch (number)
                {
                    case 1:
                        {
                            if (MainPlayer.LocalPlayerInstance == null)
                            {
                                PhotonNetwork.Instantiate(this.playerRed.name, new Vector3(0f, 1f, 0f), Quaternion.identity, 0);
                            }
                            break;
                        }
                    case 2:
                        {
                            if (MainPlayer.LocalPlayerInstance == null)
                            {
                                PhotonNetwork.Instantiate(this.playerBlue.name, new Vector3(0f, 1f, 0f), Quaternion.identity, 0);
                            }
                            break;
                        }
                }

             
            }

        }

        #region Photon Callbacks
        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


                LoadArena();
            }
        }


        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

            }
        }
        #endregion


        #region Private Methods
        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }

            PhotonNetwork.LoadLevel(1);
        }
        #endregion


        #region Public Methods
        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        #endregion
    }

}
