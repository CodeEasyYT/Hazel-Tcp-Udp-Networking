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

    public static void Test()
    {
        using(Packet _packet = new Packet((int)ClientSend.Test))
        {
            _packet.Write("Test");
            _packet.Write("Yo wassup welcome to CodeEasyYT!");

            SendUDPReliableData(_packet);
        }
    }
}