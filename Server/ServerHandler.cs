public static class ServerHandler
{
    public static void TestReceived(int id, Packet _packet)
    {
        string testString = _packet.ReadString();
        string exText = _packet.ReadString();

        UnityEngine.Debug.Log(testString);
        UnityEngine.Debug.Log(exText);
    }
}