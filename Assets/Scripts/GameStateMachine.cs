using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine : MonoBehaviourPun
{
    public Action<Player> OnPlayerTurnStarted = (playerId) => { };
    public Action OnRoundStarted = ()=> { };

    private Queue<Player> _playersQueue = new Queue<Player>();

    private Player _firstPlayer = null;
    private Player _currentPlayer = null;

    public Player NextPlayer
    {
        get
        {
            _currentPlayer = _playersQueue.Dequeue();
            _playersQueue.Enqueue(_currentPlayer);
            return _currentPlayer;
        }
    }

    public Player CurrentPlayer
    {
        get
        {
            if (_currentPlayer == null)
            {
                _currentPlayer = NextPlayer;
            }

            return _currentPlayer;
        }
    }

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

    public void StartGame(List<Player> playersQueue)
    {
        _playersQueue = new Queue<Player>(playersQueue);
        _firstPlayer = playersQueue.ToArray()[0];
        StartRound();
    }

    public void EndPlayerTurn()
    {
        if (_playersQueue.ToArray()[0] == _firstPlayer)
        {
            StartRound();
        }
        else
        {
            StartPlayerTurn(NextPlayer);
        }
    }

    private void StartRound()
    {
        OnRoundStarted();
        StartPlayerTurn(NextPlayer);
    }

    private void StartPlayerTurn(Player i)
    {
        OnPlayerTurnStarted(i);
    }
}