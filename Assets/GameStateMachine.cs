using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine : MonoBehaviourPun
{

    private static GameStateMachine _instance;
    public static GameStateMachine Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<GameStateMachine>();
            }
            return _instance;
        }
    }


    public bool CanPlayerPickCardFromEncounterDeck(EncounterDeck deck, Player player)
    {
        return true; //fake
    }
}