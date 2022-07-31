using TMPro;
using UnityEngine;

public class GetPlayerRecord : MonoBehaviour
{
    private void Start()
    {
        if (PlayerPrefs.HasKey("RecordScore"))
        {
            gameObject.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("RecordScore").ToString();
        }
        else
        {
            gameObject.GetComponent<TextMeshProUGUI>().text = "0";
        }
    }
}
