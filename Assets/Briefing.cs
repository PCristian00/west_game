using TMPro;
using UnityEngine;

public class Briefing : MonoBehaviour
{
    private TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();

        text.text = GameManager.instance.BriefingInfo();
    }
}
