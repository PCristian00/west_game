using System.Collections;
using UnityEngine;

public class TransformAnimation : MonoBehaviour
{

    [SerializeField] private Vector3 translation = Vector3.zero;
    [SerializeField] private Vector3 rotationAxis = Vector3.right;
    [SerializeField] private float rotationDegrees = 0f;
    [SerializeField] private Vector3 targetScale = Vector3.zero;
    public float duration = 1f;

    [SerializeField] private AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Vector3 startPosition;
    private Quaternion startRotation;
    private Vector3 startScale;

    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private Transform _transform;



    private void Start()
    {
        _transform = GetComponent<Transform>();
        startPosition = _transform.localPosition;
        startRotation = _transform.localRotation;
        startScale = _transform.localScale;

        if (targetScale == Vector3.zero) targetScale = startScale;

        targetPosition = startPosition + translation;
        targetRotation = startRotation * Quaternion.AngleAxis(rotationDegrees, rotationAxis);
    }

    public void Play(bool forward = true)
    {
        StopAllCoroutines();
        StartCoroutine(Animate(forward));
    }

    IEnumerator Animate(bool forward = true)
    {

        Vector3 currentStartPosition = _transform.localPosition;
        Vector3 currentStartScale = _transform.localScale;
        Quaternion currentStartRotation = _transform.localRotation;

        Vector3 currentTargetPosition = forward ? targetPosition : startPosition;
        Vector3 currentTargetScale = forward ? targetScale : startScale;
        Quaternion currentTargetRotation = forward ? targetRotation : startRotation;

        float t = 0f;
        while (t < 1f)
        {
            yield return null;
            t += Time.deltaTime / duration;

            _transform.localPosition = Vector3.Lerp(currentStartPosition, currentTargetPosition, curve.Evaluate(t));
            _transform.localScale = Vector3.Lerp(currentStartScale, currentTargetScale, curve.Evaluate(t));
            _transform.localRotation = Quaternion.Lerp(currentStartRotation, currentTargetRotation, curve.Evaluate(t));
        }
    }
}
