public static class ServerHandler
{
    public static void PlayerPosition(int _id, Packet _packet)
    {
        ThreadManager.ExecuteOnMainThread(() =>
        {
            Server.Instance.clients[_id].player.transform.position = _packet.ReadVector3();
            Server.Instance.clients[_id].player.transform.rotation = _packet.ReadQuaternion();
        });
    }
}