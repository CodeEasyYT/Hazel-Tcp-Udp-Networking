﻿using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance;

    public GameObject clientPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.LogWarning($"Double instances? (NetworkManager) instance has been found, deleting current one. Scene: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
            Destroy(this);
        }
    }

    public Player SpawnPlayer(Vector3 position)
    {
        Player newPlayer;
        newPlayer = Instantiate(clientPrefab, position, Quaternion.identity).GetComponent<Player>();
        return newPlayer;
    }
}