using System.Collections;
using UnityEngine;

public class RotationAnimation : MonoBehaviour
{


    [SerializeField] private Vector3 rotationAxis = Vector3.right;
    [SerializeField] private float rotationDegrees = 0f;


    [SerializeField] private AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);


    private Quaternion startRotation;

    private Quaternion targetRotation;

    private Transform _transform;

    // public bool looping = false;



    private void Start()
    {
        _transform = GetComponent<Transform>();
        startRotation = _transform.localRotation;
        targetRotation = startRotation * Quaternion.AngleAxis(rotationDegrees, rotationAxis);
    }


    public void Play(float duration, bool forward)
    {
        StopAllCoroutines();
        StartCoroutine(Animate(duration, forward));
    }

    public void PlayComplete(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(AnimateComplete(duration));

        //while (looping)
        //{
        //    StartCoroutine(AnimateComplete(duration));
        //}
    }

    IEnumerator Animate(float duration, bool forward)
    {
        Quaternion currentStartRotation = _transform.localRotation;
        Quaternion currentTargetRotation = targetRotation;

        float t = 0f;

        if (forward)
        {
            t = 0f;
            while (t < 0.5f)
            {
                yield return null;
                t += Time.deltaTime / duration;
                _transform.localRotation = Quaternion.Lerp(currentStartRotation, currentTargetRotation, curve.Evaluate(t));
            }
        }

        if (!forward)
        {
            // PARTE RITORNO A INIZIO

            currentTargetRotation = startRotation;

            currentStartRotation = targetRotation;


            while (t < 1f)
            {
                yield return null;
                t += Time.deltaTime / duration;
                _transform.localRotation = Quaternion.Lerp(currentStartRotation, currentTargetRotation, curve.Evaluate(t));
            }
        }
    }

    IEnumerator AnimateComplete(float duration)
    {
        Quaternion currentStartRotation = _transform.localRotation;

        Quaternion currentTargetRotation = targetRotation;
        float t = 0f;
        while (t < 0.5f)
        {
            yield return null;
            t += Time.deltaTime / duration;
            _transform.localRotation = Quaternion.Lerp(currentStartRotation, currentTargetRotation, curve.Evaluate(t));
        }

        // PARTE RITORNO A INIZIO

        currentTargetRotation = startRotation;

        currentStartRotation = targetRotation;

        while (t < 1f)
        {
            yield return null;
            t += Time.deltaTime / duration;
            _transform.localRotation = Quaternion.Lerp(currentStartRotation, currentTargetRotation, curve.Evaluate(t));
        }
    }

    // TROVARE UN MODO PER TORNARE PIù VELOCEMENTE ALL'INIZIO SE IL GIOCATORE SPARA (CYLINDER RELOAD)
}
