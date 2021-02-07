using Hazel;
using UnityEngine;

// TODO: Remove
[System.Serializable]
public class Client
{
    public int id;
    public bool isEmpty;
    public Connection connection;

    public Player player;

    private bool waitingToBeKilled = false;

    public Client()
    {
        isEmpty = true;
    }

    public Client(int id, Connection connection)
    {
        this.id = id;

        this.connection = connection;
        isEmpty = false;

        connection.Disconnected += Connection_Disconnected;
        connection.DataReceived += Connection_DataReceived;

        ThreadManager.ExecuteOnMainThread(() =>
        {
            player = NetworkManager.Instance.SpawnPlayer(Vector3.zero);
            player.Initialize(id);
        });
    }

    public void Ready()
    {
        ServerSender.SendClientsId(id);

        ThreadManager.ExecuteOnMainThread(() =>
        {
            ServerSender.NewClientJoined(id, player.transform.position);

            for (int i = 0; i < Server.Instance.clients.Count; i++)
            {
                if (!Server.Instance.clients[i].isEmpty && i != id)
                    ServerSender.NewClientSendToNewlyJoinedClient(i, id);
            }
        });
    }

    private void Connection_DataReceived(object sender, DataReceivedEventArgs e)
    {
        Packet newPacket = new Packet(e.Bytes);
        int _packetId = newPacket.ReadInt();

        Server.Instance.dataReceiveHandler[_packetId](id, newPacket);
    }

    private void Connection_Disconnected(object sender, DisconnectedEventArgs e)
    {
        Server.Instance.Connection_Disconnected(this, e);
    }

    public void KillMyself()
    {
        ThreadManager.ExecuteOnMainThread(() =>
        {
            Object.Destroy(player.gameObject);
        });

        waitingToBeKilled = true;
    }

    public void Update()
    {
        if (waitingToBeKilled)
            return;

        try
        {
            ServerSender.PlayerPosition(id);
        }
        catch(System.Exception ex)
        {
            Debug.LogError(ex);
        }
    }
}