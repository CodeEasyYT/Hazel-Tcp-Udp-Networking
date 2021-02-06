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
            if(!Server.Instance.clients[i].isEmpty)
                Server.Instance.clients[i].connection.SendBytes(_packet.ToArray(), Hazel.SendOption.FragmentedReliable);
        }
    }

    private static void SendUDPReliableDataToAll(Packet _packet, int _exceptClient)
    {
        for (int i = 0; i < ServerStarter.Instance.maxPlayers; i++)
        {
            if (!Server.Instance.clients[i].isEmpty)
                if(i != _exceptClient)
                    Server.Instance.clients[i].connection.SendBytes(_packet.ToArray(), Hazel.SendOption.FragmentedReliable);
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
                Server.Instance.clients[i].connection.SendBytes(_packet.ToArray(), Hazel.SendOption.None);
        }
    }

    private static void SendUDPUnreliableDataToAll(Packet _packet, int _exceptClient)
    {
        for (int i = 0; i < ServerStarter.Instance.maxPlayers; i++)
        {
            if (!Server.Instance.clients[i].isEmpty)
                if (i != _exceptClient)
                    Server.Instance.clients[i].connection.SendBytes(_packet.ToArray(), Hazel.SendOption.None);
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
}