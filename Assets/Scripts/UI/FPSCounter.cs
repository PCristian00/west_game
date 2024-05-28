using TMPro;
using UnityEngine;
public class FPSCounter : MonoBehaviour
{
    public float deltaTime;
    public TextMeshProUGUI fpsInfo;

    private void Start()
    {
        fpsInfo = gameObject.GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsInfo.text = Mathf.Ceil(fps).ToString();
    }
}