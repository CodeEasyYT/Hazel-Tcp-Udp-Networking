using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int id;

    public bool localManager = false;

    public void Initialize(int id)
    {
        this.id = id;

        if(localManager)
        {
            GetComponent<PlayerController>().Initialize(this);
        }
    }

    private void FixedUpdate()
    {
        if(localManager)
        {
            ClientSender.SendPosition(transform.position, transform.rotation);
        }
    }
}