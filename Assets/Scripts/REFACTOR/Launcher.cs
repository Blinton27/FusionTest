using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Launcher : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] int _sceneToLoad;
    [SerializeField] private NetworkPrefabRef _playerPrefab;
    [SerializeField] private NetworkPrefabRef _poolBallPrefab;
    private NetworkRunner _runner;
    private string _sessionName;
    private RessourceManager _ressourceManager; 
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
    private bool _mouseButton0;
    private bool _mouseButton1;
    private string _username;
    private NetworkObject _spawnedPlayer;

    private void Awake()
    {
        _ressourceManager = RessourceManager.FindInstance();
    }

    public void TriggerStartGame(string gameMode)
    {
        if (gameMode != null)
        {
            if (gameMode == "Host")
            {
                StartGame(GameMode.Host);
            }
            else if (gameMode == "Join")
            {
                StartGame(GameMode.Client);
            }
        }
    }

    async void StartGame(GameMode mode)
    {
        // Create the Fusion runner and let it know that we will be providing user input
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        // Start or join (depends on gamemode) a session with a specific name
        var result = await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = _sessionName,
            Scene = _sceneToLoad,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });

        if (result.Ok)
        {
            // trouver si on a bien crée/rejoint une room
            _ressourceManager.gameUi.Canvas.SetActive(false);
            Debug.Log(_ressourceManager.gameUi.Username.text);
        }
        else
        {
            // afficher message d'erreur
            Debug.LogError($"Failed to Start: {result.ShutdownReason}");
        }
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        Debug.Log("OnSceneLoadDone");
        // spawn the ball
        NetworkObject poolBall = runner.Spawn(_poolBallPrefab, new Vector3(0, 1.4f, 6), Quaternion.identity);
        poolBall.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log(player);
        if (runner.IsServer)
        {
            //_username = _usernameField.text;
            // Create a unique position for the player
            Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.DefaultPlayers) * 3, 1, 0);
            _spawnedPlayer = runner.Spawn(_playerPrefab, spawnPosition, new Quaternion(0, 180, 0, 0), player);
            // Keep track of the player avatars so we can remove it when they disconnect
            _spawnedCharacters.Add(player, _spawnedPlayer);
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        // Find and remove the players avatar
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
    }

    /*public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }*/
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
    public void OnSceneLoadStart(NetworkRunner runner) { }
    /*public void OnSceneLoadDone(NetworkRunner runner) { }*/

    private void Update()
    {
        _mouseButton0 = _mouseButton0 | Input.GetMouseButton(0);
        _mouseButton1 = _mouseButton1 || Input.GetMouseButton(1);
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new NetworkInputData();

        if (Input.GetKey(KeyCode.Z))
            data.direction += Vector3.forward;

        if (Input.GetKey(KeyCode.S))
            data.direction += Vector3.back;

        if (Input.GetKey(KeyCode.Q))
            data.direction += Vector3.left;

        if (Input.GetKey(KeyCode.D))
            data.direction += Vector3.right;
        /*if (_mouseButton0)
            data.buttons |= NetworkInputData.MOUSEBUTTON1;
        _mouseButton0 = false;
        if (_mouseButton1)
            data.buttons |= NetworkInputData.MOUSEBUTTON2;
        _mouseButton1 = false;*/

        input.Set(data);

    }
}
