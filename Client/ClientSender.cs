using UnityEngine;

public static class ClientSender
{
    #region Send Data
    private static void SendUDPReliableData(Packet _packet)
    {
        Client.Instance.connection.SendBytes(_packet.ToArray(), Hazel.SendOption.FragmentedReliable);
    }

    private static void SendUDPUnreliableData(Packet _packet)
    {
        Client.Instance.connection.SendBytes(_packet.ToArray(), Hazel.SendOption.None);
    }
    #endregion

    public static void SendPosition(Vector3 position, Quaternion rotation)
    {
        using(Packet _packet = new Packet((int)ClientSend.PlayerMovement))
        {
            _packet.Write(position);
            _packet.Write(rotation);

            SendUDPUnreliableData(_packet);
        }
    }
}