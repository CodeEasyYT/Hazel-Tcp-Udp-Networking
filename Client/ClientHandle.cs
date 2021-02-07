using UnityEngine;

public static class ClientHandle
{
    public static void OnIdReceived(Packet _packet)
    {
        Client.Instance.id = _packet.ReadInt();
    }

    public static void OnSpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        ThreadManager.ExecuteOnMainThread(() =>
        {
            GameManager.Instance.SpawnPlayer(_id, _position);
        });
    }

    public static void OnPlayerMove(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        ThreadManager.ExecuteOnMainThread(() =>
        {
            GameManager.Instance.players[_id].transform.position = _position;
        });
    }
}