using UnityEngine;

[RequireComponent(typeof(Server))]
public class ServerStarter : MonoBehaviour
{
    public static ServerStarter Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.LogWarning($"Double instances? (ServerStarter) instance has been found, deleting current one. Scene: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
            Destroy(this);
        }
    }

    private Server server;

    public int port = 35465;
    public int maxPlayers = 10;

    private void Start()
    {
        server = GetComponent<Server>();
        server.StartServer(port, maxPlayers);
    }
}