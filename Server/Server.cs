using Hazel;
using Hazel.Udp;
//using Hazel.Tcp;
using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Server : MonoBehaviour
{
    public static Server Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.LogWarning($"Double instances? (Server) instance has been found, deleting current one. Scene: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
            Destroy(this);
        }
    }

    private int port;
    private int maxPlayers;
    private bool isConnected = false;

    //public TcpConnectionListener connection;
    public UdpConnectionListener connection;

    public List<Client> clients;

    public int clientCount;

    public delegate void OnDataReceived(int _id, Packet _packet);
    public Dictionary<int, OnDataReceived> dataReceiveHandler;

    public void StartServer(int port, int maxPlayers)
    {
        this.port = port;
        this.maxPlayers = maxPlayers;

#pragma warning disable CS0618 // Tür veya üye artık kullanılmıyor
        connection = new UdpConnectionListener(IPAddress.Any, this.port);
        //connection = new TcpConnectionListener(IPAddress.Any, this.port);
#pragma warning restore CS0618 // Tür veya üye artık kullanılmıyor

        connection.NewConnection += NewConnection;

        Debug.Log("Server starting... (Setting client slots up)");

        clients = new List<Client>();
        clientCount = 0;

        for (int i = 0; i < maxPlayers; i++)
        {
            clients.Add(new Client());
        }

        Debug.Log("Server starting... (Setting receive slots up)");

        ConfigureDataReceiveEvents();

        Debug.Log("Server starting...");

        connection.Start();
        isConnected = true;

        Debug.Log("Server started!");
    }

    private void ConfigureDataReceiveEvents()
    {
        dataReceiveHandler = new Dictionary<int, OnDataReceived>
        {
            { (int)ServerReceive.PlayerMovement, ServerHandler.PlayerPosition }
        };
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Closing...");
        connection.Close();
        Debug.Log("Closing... (Setting client slots)");
        for (int i = 0; i < maxPlayers; i++)
        {
            clients[i].KillMyself();
            clients[i] = new Client();
        }
    }

    private void NewConnection(object sender, NewConnectionEventArgs args)
    {
        int id = -1;

        for (int i = 0; i < maxPlayers; i++)
        {
            if(clients[i].isEmpty)
            {
                id = i;
                break;
            }
        }

        if (id == -1)
        {
            Debug.Log("New connection from " + args.Connection.EndPoint + " but the server is full!");
            args.Connection.Close();
            return;
        }

        Debug.Log("New connection: " + args.Connection.EndPoint + " | " + id);

        try
        {
            Client newClient = new Client(id, args.Connection);
            clients[id] = newClient;
            newClient.Ready();
            clientCount++;
        }
        catch(Exception ex)
        {
            Debug.Log(ex);
        }
    }

    public void Connection_Disconnected(Client sender, DisconnectedEventArgs e)
    {
        if (e.Exception == null)
        {
            Debug.Log("Disconnection: " + sender.id + " | Client nicely disconnected!");
        }
        else
        {
            Debug.Log("Disconnection: " + sender.id + " | " + e.Exception);
        }

        sender.KillMyself();
        clients[sender.id] = new Client();
        clientCount--;
    }
}