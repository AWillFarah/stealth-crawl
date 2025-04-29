using UnityEngine;
using TMPro;
public class MessageLogManager : MonoBehaviour
{
    public static MessageLogManager S;
    public TMP_Text messageLog;
    void Start()
    {
        S = this;
    }

    // Update is called once per frame
    public void AddMessageToLog(string msg)
    {
        TMP_Text log = Instantiate(messageLog, transform);
        log.text = msg;
    }
}
