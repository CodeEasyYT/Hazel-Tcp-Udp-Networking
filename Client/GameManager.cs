using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();

    public GameObject localPrefab;
    public GameObject otherPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.LogWarning($"Double instances? (GameManager) instance has been found, deleting current one. Scene: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
            Destroy(this);
        }
    }

    public void SpawnPlayer(int _id, Vector3 _position)
    {
        GameObject _player;
        if (_id == Client.Instance.id)
        {
            _player = Instantiate(localPrefab, _position, Quaternion.identity);
        }
        else
        {
            _player = Instantiate(otherPrefab, _position, Quaternion.identity);
        }

        _player.GetComponent<PlayerManager>().Initialize(_id);
        players.Add(_id, _player.GetComponent<PlayerManager>());
    }
}