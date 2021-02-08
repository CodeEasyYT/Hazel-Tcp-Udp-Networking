using TMPro;
using UnityEngine;

public class StartUI : MonoBehaviour
{
    public TMP_InputField ipText;

    public void Connect()
    {
        if(!ipText.text.Contains(":"))
        {
            ipText.transform.parent.gameObject.SetActive(false);
            Client.Instance.ip = ipText.text;
            Client.Instance.Connect();
        }
        else
        {
            string[] ipAndPort = ipText.text.Split(':');
            ipText.transform.parent.gameObject.SetActive(false);
            Client.Instance.ip = ipAndPort[0];
            Client.Instance.port = int.Parse(ipAndPort[1]);

            Debug.Log(ipAndPort[0]);
            Debug.Log(ipAndPort[1]);
            Client.Instance.Connect();
        }
    }
}