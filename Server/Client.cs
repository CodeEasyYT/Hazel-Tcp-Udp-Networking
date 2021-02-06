using Hazel;

public class Client
{
    public int id;
    public bool isEmpty;
    public Connection connection;

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
    }

    public void Ready()
    {
        ServerSender.SendClientsId(id);
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

    }
}