using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine : MonoBehaviourPun
{
    public Action<int> OnPlayerTurnStarted = (playerId) => { };
    public Action OnRoundStarted = ()=> { };

    private Queue<int> _playersQueue = new Queue<int>();

    private int _firstPlayer = -1;
    private int _currentPlayer = -1;

    public int NextPlayer
    {
        get
        {
            _currentPlayer = _playersQueue.Dequeue();
            _playersQueue.Enqueue(_currentPlayer);
            return _currentPlayer;
        }
    }

    public int CurrentPlayer
    {
        get
        {
            if (_currentPlayer == -1)
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

    public void StartGame(List<int> playersQueue)
    {
        _playersQueue = new Queue<int>(playersQueue);
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

    private void StartPlayerTurn(int i)
    {
        OnPlayerTurnStarted(i);
    }
}