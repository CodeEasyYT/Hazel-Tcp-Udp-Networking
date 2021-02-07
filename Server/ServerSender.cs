using UnityEngine;

public static class ServerSender
{
    #region Send Data
    private static void SendUDPReliableData(Packet _packet, int clientId)
    {
        Server.Instance.clients[clientId].connection.SendBytes(_packet.ToArray(), Hazel.SendOption.FragmentedReliable);
    }

    private static void SendUDPReliableDataToAll(Packet _packet)
    {
        for (int i = 0; i < ServerStarter.Instance.maxPlayers; i++)
        {
            if (!Server.Instance.clients[i].isEmpty)
                SendUDPReliableData(_packet, i);
        }
    }

    private static void SendUDPReliableDataToAll(Packet _packet, int _exceptClient)
    {
        for (int i = 0; i < ServerStarter.Instance.maxPlayers; i++)
        {
            if (!Server.Instance.clients[i].isEmpty)
                if(i != _exceptClient)
                    SendUDPReliableData(_packet, i);
        }
    }

    private static void SendUDPUnreliableData(Packet _packet, int clientId)
    {
        Server.Instance.clients[clientId].connection.SendBytes(_packet.ToArray(), Hazel.SendOption.None);
    }

    private static void SendUDPUnreliableDataToAll(Packet _packet)
    {
        for (int i = 0; i < ServerStarter.Instance.maxPlayers; i++)
        {
            if (!Server.Instance.clients[i].isEmpty)
                SendUDPUnreliableData(_packet, i);
        }
    }

    private static void SendUDPUnreliableDataToAll(Packet _packet, int _exceptClient)
    {
        for (int i = 0; i < ServerStarter.Instance.maxPlayers; i++)
        {
            if (!Server.Instance.clients[i].isEmpty)
                if (i != _exceptClient)
                    SendUDPUnreliableData(_packet, i);
        }
    }
    #endregion

    public static void SendClientsId(int _id)
    {
        using(Packet _packet = new Packet((int)ServerSend.ClientId))
        {
            _packet.Write(_id);

            SendUDPReliableData(_packet, _id);
        }
    }

    public static void NewClientJoined(int _id, Vector3 _position)
    {
        using (Packet _packet = new Packet((int)ServerSend.SpawnPlayer))
        {
            _packet.Write(_id);
            _packet.Write(_position);

            SendUDPReliableDataToAll(_packet);
        }
    }

    public static void NewClientSendToNewlyJoinedClient(int _id, int _to)
    {
        using (Packet _packet = new Packet((int)ServerSend.SpawnPlayer))
        {
            _packet.Write(_id);
            _packet.Write(Server.Instance.clients[_id].player.transform.position);

            SendUDPReliableData(_packet, _to);
        }
    }

    public static void PlayerPosition(int _id)
    {
        using (Packet _packet = new Packet((int)ServerSend.PlayerMovement))
        {
            _packet.Write(_id);
            _packet.Write(Server.Instance.clients[_id].player.transform.position);
            _packet.Write(Server.Instance.clients[_id].player.transform.rotation);

            SendUDPUnreliableDataToAll(_packet, _id);
        }
    }
}