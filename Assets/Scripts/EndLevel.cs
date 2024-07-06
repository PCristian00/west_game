using UnityEngine;

public class EndLevel : MonoBehaviour
{
    public GameObject panel;
    public GameStateDependentObject dependentObject;

    void Start()
    {
        dependentObject = GetComponent<GameStateDependentObject>();
    }

    private void OnEnable()
    {
        if (dependentObject)
            if (dependentObject._targetGameState == GameManager.instance.CurrentGameState)
            {
                Time.timeScale = 0.2f;
                Invoke(nameof(ShowPanel), Time.timeScale * 2.5f);
            }
    }

    void ShowPanel()
    {
        gameObject.SetActive(false);
        panel.SetActive(true);
    }
}
