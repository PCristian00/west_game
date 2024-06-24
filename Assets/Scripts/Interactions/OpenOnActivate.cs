using UnityEngine;

public class OpenOnActivate : MonoBehaviour, IActivate
{
    private TransformAnimation _transformAnimation;

    public void Activate()
    {
        _transformAnimation.Play(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        _transformAnimation = GetComponent<TransformAnimation>();
    }
}
