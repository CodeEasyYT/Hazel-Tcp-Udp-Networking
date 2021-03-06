﻿using UnityEngine;

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
        Quaternion _rotation = _packet.ReadQuaternion();

        ThreadManager.ExecuteOnMainThread(() =>
        {
            GameManager.Instance.players[_id].transform.position = _position;
            GameManager.Instance.players[_id].transform.rotation = _rotation;
        });
    }

    public static void OnClientDisconnected(Packet _packet)
    {
        int _id = _packet.ReadInt();

        ThreadManager.ExecuteOnMainThread(() =>
        {
            GameManager.Instance.DespawnPlayer(_id);
        });
    }
}