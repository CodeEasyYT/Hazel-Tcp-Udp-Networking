using Hazel;
using Hazel.Udp;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Client : MonoBehaviour
{
    public static Client Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.LogWarning($"Double instances? (Client) instance has been found, deleting current one. Scene: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
            Destroy(this);
        }
    }

    public string ip = "127.0.0.1";
    public int port = 35465;

    public int id;

    private bool isConnected;
    public UdpConnection connection;

    public delegate void OnDataReceived(Packet _packet);
    public Dictionary<int, OnDataReceived> dataReceiveHandler;

    private void Start()
    {
        Thread connectThread = new Thread(new ThreadStart(Connect));
        connectThread.Start();
    }

    public void Connect()
    {
        connection = new UdpClientConnection(new NetworkEndPoint(ip, port));

        connection.Disconnected += Disconnected;
        connection.DataReceived += Connection_DataReceived;

        Debug.Log("Connecting...");
        ConfigureDataReceiveEvents();

        try
        {
            connection.Connect();
            isConnected = true;
            Debug.Log("Connected!");
        }
        catch(Exception ex)
        {
            Debug.Log("Unable to connect to the server! (" + ex + ")");
        }
    }

    private void ConfigureDataReceiveEvents()
    {
        dataReceiveHandler = new Dictionary<int, OnDataReceived>
        {
            { (int)ClientReceive.ClientId, ClientHandle.OnIdReceived }
        };
    }

    private void Connection_DataReceived(object sender, DataReceivedEventArgs e)
    {
        Packet newPacket = new Packet(e.Bytes);
        int _packetId = newPacket.ReadInt();

        dataReceiveHandler[_packetId](newPacket);
    }

    private void OnApplicationQuit()
    {
        if(isConnected)
        {
            Disconnect();
        }
    }

    private void Disconnected(object sender, DisconnectedEventArgs args)
    {
        Debug.Log($"Disconnected: {args.Exception}");
        isConnected = false;
    }

    public void Disconnect()
    {
        connection.Close();
        Debug.Log("Disconnected!");
        isConnected = false;
    }
}