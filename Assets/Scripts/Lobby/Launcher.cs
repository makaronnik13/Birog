// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Launcher.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Networking Demos
// </copyright>
// <summary>
//  Used in "PUN Basic tutorial" to connect, and join/create room automatically
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System;

namespace Photon.Pun.Demo.PunBasics
{
    /// <summary>
    /// Launch manager. Connect, join a random room or create one if none or all full.
    /// </summary>
	public class Launcher : MonoBehaviourPunCallbacks 
    {

        #region Private Serializable Fields
        public string BattleLevelName;
        public TMP_InputField NameInputField;
        public Counter Counter;
        [Header("Inside Room Panel")]
        public GameObject InsideRoomPanel;
        public GameObject PlayerListEntryPrefab;
        public Button ReadyButton;

        [Tooltip("The Ui Panel to let the user enter name, connect and play")]
		[SerializeField]
		private GameObject controlPanel;

		[Tooltip("The maximum number of players per room")]
		[SerializeField]
		private byte maxPlayersPerRoom = 4;


		#endregion

		#region Private Fields
		/// <summary>
		/// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon, 
		/// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
		/// Typically this is used for the OnConnectedToMaster() callback.
		/// </summary>
		bool isConnecting;

		/// <summary>
		/// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
		/// </summary>
		string gameVersion = "1";

		#endregion

		#region MonoBehaviour CallBacks

		/// <summary>
		/// MonoBehaviour method called on GameObject by Unity during early initialization phase.
		/// </summary>
		void Awake()
		{

			// #Critical
			// this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
			PhotonNetwork.AutomaticallySyncScene = true;
            NameInputField.text = "Игрок" + UnityEngine.Random.Range(0,999);

        }

		#endregion


		#region Public Methods

		/// <summary>
		/// Start the connection process. 
		/// - If already connected, we attempt joining a random room
		/// - if not yet connected, Connect this application instance to Photon Cloud Network
		/// </summary>
		public void Connect()
		{

            PhotonNetwork.LocalPlayer.NickName = NameInputField.text;

			// keep track of the will to join a room, because when we come back from the game we will get a callback that we are connected, so we need to know what to do then
			isConnecting = true;

			// hide the Play button for visual consistency
			controlPanel.SetActive(false);
            InsideRoomPanel.SetActive(true);
            
			// we check if we are connected or not, we join if we are , else we initiate the connection to the server.
			if (PhotonNetwork.IsConnected)
			{
				Debug.Log("Joining Room...");
				// #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
				PhotonNetwork.JoinRandomRoom();
			}else{

				Debug.Log("Connecting...");
				
				// #Critical, we must first and foremost connect to Photon Online Server.
			    PhotonNetwork.GameVersion = this.gameVersion;
				PhotonNetwork.ConnectUsingSettings();
			}
		}

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (changedProps.ContainsKey(DefaultResources.PLAYER_IS_READY))
            {
                if (targetPlayer == PhotonNetwork.LocalPlayer)
                {
                    if ((bool)changedProps[DefaultResources.PLAYER_IS_READY])
                    {
                        ReadyButton.GetComponent<Image>().color = Color.green;
                        ReadyButton.GetComponentInChildren<TextMeshProUGUI>().text = "не готов";
                    }
                    else
                    {
                        ReadyButton.GetComponent<Image>().color = Color.white;
                        ReadyButton.GetComponentInChildren<TextMeshProUGUI>().text = "готов";
                    }
                }

                foreach (PlayerListEntry entry in FindObjectsOfType<PlayerListEntry>())
                {
                    if (entry.OwnerId == targetPlayer.ActorNumber)
                    {
                        entry.ChangeReadyState((bool)changedProps[DefaultResources.PLAYER_IS_READY]);
                    }
                }
            }




            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            if (changedProps.ContainsKey(DefaultResources.PLAYER_IS_READY))
            {
                if (CheckAllPlayerLoadedLevel())
                {
                    photonView.RPC("LaunchTimer", RpcTarget.All, new object[] { });
                }
            }
        }

        [PunRPC]
        private void LaunchTimer()
        {
            ReadyButton.gameObject.SetActive(false);
            if (PhotonNetwork.IsMasterClient)
            {
                Counter.StartCount(3, () => { PhotonNetwork.LoadLevel(BattleLevelName); });
            }
            else
            {
                Counter.StartCount(3);
            }
            
        }


        private bool CheckAllPlayerLoadedLevel()
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (!(bool)player.CustomProperties[DefaultResources.PLAYER_IS_READY])
                {
                    return false;
                }
            }

            return true;
        }
        #endregion


        #region MonoBehaviourPunCallbacks CallBacks
        // below, we implement some callbacks of PUN
        // you can find PUN's callbacks in the class MonoBehaviourPunCallbacks


        /// <summary>
        /// Called after the connection to the master is established and authenticated
        /// </summary>
        public override void OnConnectedToMaster()
		{
            // we don't want to do anything if we are not attempting to join a room. 
			// this case where isConnecting is false is typically when you lost or quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
			// we don't want to do anything.
			if (isConnecting)
			{
				
				Debug.Log("Now this client is connected and could join a room");
		
				// #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
				PhotonNetwork.JoinRandomRoom();
			}
		}

		/// <summary>
		/// Called when a JoinRandom() call failed. The parameter provides ErrorCode and message.
		/// </summary>
		/// <remarks>
		/// Most likely all rooms are full or no rooms are available. <br/>
		/// </remarks>
		public override void OnJoinRandomFailed(short returnCode, string message)
		{
			
			Debug.Log("No random room available, so we create one");

			// #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
			PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = this.maxPlayersPerRoom});
		}


		/// <summary>
		/// Called after disconnecting from the Photon server.
		/// </summary>
		public override void OnDisconnected(DisconnectCause cause)
		{	
			isConnecting = false;
			controlPanel.SetActive(true);
            InsideRoomPanel.SetActive(false);
        }

		/// <summary>
		/// Called when entering a room (by creating or joining it). Called on all clients (including the Master Client).
		/// </summary>
		/// <remarks>
		/// This method is commonly used to instantiate player characters.
		/// If a match has to be started "actively", you can call an [PunRPC](@ref PhotonView.RPC) triggered by a user's button-press or a timer.
		///
		/// When this is called, you can usually already access the existing players in the room via PhotonNetwork.PlayerList.
		/// Also, all custom properties should be already available as Room.customProperties. Check Room..PlayerCount to find out if
		/// enough players are in the room to start playing.
		/// </remarks>
		public override void OnJoinedRoom()
		{
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                GameObject entry = Instantiate(PlayerListEntryPrefab);
                entry.transform.SetParent(InsideRoomPanel.transform);
                entry.transform.localScale = Vector3.one;
                entry.transform.localPosition = Vector3.zero;
                entry.GetComponent<PlayerListEntry>().Initialize(p);
            }

            Hashtable props = new Hashtable
            {
                {DefaultResources.PLAYER_IS_READY, false}
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);


            ReadyButton.gameObject.SetActive(true);
        }

        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.Log("OnPlayerEnteredRoom() " + other.NickName); // not seen if you're the player connecting
            ///
            GameObject entry = Instantiate(PlayerListEntryPrefab);
            entry.transform.SetParent(InsideRoomPanel.transform);
            entry.transform.localScale = Vector3.one;
            entry.transform.localPosition = Vector3.zero;
            entry.GetComponent<PlayerListEntry>().Initialize(other);
        }
        #endregion

        public void ChangeReadyState()
        {
            Hashtable props = new Hashtable
            {
                {DefaultResources.PLAYER_IS_READY, !(bool)PhotonNetwork.LocalPlayer.CustomProperties[DefaultResources.PLAYER_IS_READY]}
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }
    }
}