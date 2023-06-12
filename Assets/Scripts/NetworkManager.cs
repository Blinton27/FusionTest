using System;
using System.Collections;
using System.Collections.Generic;
using Fusion.Sockets;
using Fusion;
using UnityEngine;
using System.Threading.Tasks;

public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField]
    private GameObject _playerPrefab;
    private NetworkRunner _runner;
   void Awake()
    {
        _runner= GetComponent<NetworkRunner>();

        //persistent
        DontDestroyOnLoad(gameObject);
    }

    public async void CreateOnJoinGame()
    {
        await StartGame();
    }




    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"{player.PlayerId} joined.");
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) 
    {
        Debug.Log($"{player.PlayerId} left.");
    }
        public void OnInput(NetworkRunner runner, NetworkInput input) { }
        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
        public void OnConnectedToServer(NetworkRunner runner) { }
        public void OnDisconnectedFromServer(NetworkRunner runner) { }
        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
        public void OnSceneLoadDone(NetworkRunner runner) { }
        public void OnSceneLoadStart(NetworkRunner runner) { }


    private async Task StartGame()
    {
        var result = await _runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.AutoHostOrClient,
            
        }); // Task<StartGameResult>

        if (result.Ok)
        {
            // all good
        }

        else
        {
            Debug.LogError($"Failed to Start: {result.ShutdownReason}");
        }

    }
    
}
