using UnityEngine;

public class OpenOnExamine : MonoBehaviour, IExamine
{
    private TransformAnimation _transformAnimation;

    public void Examine()
    {
        _transformAnimation.Play(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        _transformAnimation = GetComponent<TransformAnimation>();
    }
}
