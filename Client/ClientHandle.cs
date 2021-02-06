public static class ClientHandle
{
    public static void OnIdReceived(Packet _packet)
    {
        Client.Instance.id = _packet.ReadInt();

        ClientSender.Test();
    }
}