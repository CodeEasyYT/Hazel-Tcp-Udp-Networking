using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;

    public void Initialize(int id)
    {
        this.id = id;
    }

    private void FixedUpdate()
    {
        Server.Instance.clients[id].Update();
    }
}