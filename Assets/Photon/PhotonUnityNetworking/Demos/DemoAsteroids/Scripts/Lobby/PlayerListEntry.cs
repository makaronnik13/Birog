// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerListEntry.cs" company="Exit Games GmbH">
//   Part of: Asteroid Demo,
// </copyright>
// <summary>
//  Player List Entry
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using System;
using Photon.Pun;

public class PlayerListEntry : MonoBehaviour
    {
        [Header("UI References")]
        public TMPro.TextMeshProUGUI PlayerNameText;
        public TMPro.TextMeshProUGUI PlayerClassText;

        public Image PlayerClassImage;

        private Material _glowMaterial;
        private Material glowMaterial
        {
            get
            {
                if (!_glowMaterial)
                {
                    _glowMaterial = new Material(GetComponent<Image>().material);
                    GetComponent<Image>().material = _glowMaterial;
                }
                return _glowMaterial;
            }
        }

        public int ownerId;
        public int OwnerId
        {
            get
            {
                return ownerId;
            }
        }

        #region UNITY

        public void OnEnable()
        {
            PlayerNumbering.OnPlayerNumberingChanged += OnPlayerNumberingChanged;
        }

        public void Start()
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == ownerId)
            {
                    Hashtable props = new Hashtable() {{DefaultResources.PLAYER_IS_READY, false}};
                    PhotonNetwork.LocalPlayer.SetCustomProperties(props);

                    if (PhotonNetwork.IsMasterClient)
                    {
                       // FindObjectOfType<LobbyMainPanel>().LocalPlayerPropertiesUpdated();
                    }
 
            }
        }

        public void OnDisable()
        {
            PlayerNumbering.OnPlayerNumberingChanged -= OnPlayerNumberingChanged;
        }

        #endregion

        public void Initialize(Player player)
        {
            ownerId = player.ActorNumber;
            PlayerNameText.text = player.NickName;           
            BattlerClass bClass = DefaultResources.GetClassById((int)player.CustomProperties[DefaultResources.PLAYER_CLASS]);
            PlayerClassImage.sprite = bClass.BattlerImage;
            PlayerClassText.text = bClass.BattlerName;

    }

        private void OnPlayerNumberingChanged()
        {
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                if (p.ActorNumber == ownerId)
                {
                    //PlayerClassImage.color = AsteroidsGame.GetColor(p.GetPlayerNumber());
                }
            }
        }

        public void ChangeReadyState(bool v)
        {
            if (v)
            {
                glowMaterial.color = Color.green;
            }
            else
            {
                glowMaterial.color = Color.red;
            }
        }
    }