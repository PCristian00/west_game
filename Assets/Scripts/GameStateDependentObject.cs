using UnityEngine;

public class GameStateDependentObject : MonoBehaviour
{
    public GameManager.GameState _targetGameState;

    private void Start()
    {
        GameManager.instance.OnCurrentGameStateChanged += OnGameStateChanged;
        OnGameStateChanged(GameManager.instance.CurrentGameState);
    }

    private void OnDestroy()
    {
        GameManager.instance.OnCurrentGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameManager.GameState state)
    {
        gameObject.SetActive(state == _targetGameState);
    }
}
