using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RessourceManager : MonoBehaviour
{
    [Serializable]
    public class GameUi
    {
        public GameObject Canvas;
        public TMP_Text Username;
    }

    public GameUi gameUi;

    public static RessourceManager FindInstance()
    {
        return FindObjectOfType<RessourceManager>();
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
