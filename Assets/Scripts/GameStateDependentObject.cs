using UnityEngine;

public class GameStateDependentObject : MonoBehaviour
{
    [SerializeField] private GameManager.GameState _targetGameState;

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
        Debug.Log("GameStateDependent ON");
        gameObject.SetActive(state == _targetGameState);
    }
}
