using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UsernameManager : NetworkBehaviour
{
    private BasicSpawner _basicSpawner;
    private TMP_Text _username;

    public override void Spawned()
    {
        _basicSpawner = BasicSpawner.FindInstance();
        Debug.Log("IAfterSpawned.AfterSpawned");
        _username = GetComponent<TMP_Text>();
        Debug.Log(_username);
        _username.text = _basicSpawner.Username;
        Debug.Log(_username.text);
    }
}
