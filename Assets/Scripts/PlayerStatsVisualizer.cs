using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerStatsVisualizer : MonoBehaviourPunCallbacks
{
    public GameObject ArmorIcon;
    public TMPro.TextMeshProUGUI HpText, ArmorText;

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (targetPlayer == PhotonNetwork.LocalPlayer)
        {
            if (changedProps.ContainsKey(DefaultResources.PLAYER_HP))
            {
                object hp;
                changedProps.TryGetValue(DefaultResources.PLAYER_HP, out hp);
                HpText.text = (int)hp + "";
            }

            if (changedProps.ContainsKey(DefaultResources.PLAYER_ARMOR))
            {
                object armor;
                changedProps.TryGetValue(DefaultResources.PLAYER_ARMOR, out armor);
                ArmorIcon.SetActive((int)armor>0);
                ArmorText.text = (int)armor + "";
            }
        }
    }
}
