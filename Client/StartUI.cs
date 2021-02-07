using TMPro;
using UnityEngine;

public class StartUI : MonoBehaviour
{
    public TMP_InputField ipText;

    public void Connect()
    {
        ipText.transform.parent.gameObject.SetActive(false);
        Client.Instance.ip = ipText.text;
        Client.Instance.Connect();
    }
}