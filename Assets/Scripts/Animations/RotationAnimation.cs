using System.Collections;
using UnityEngine;

// RINOMINARE IN RotateAnimation O SIMILI
public class RotationAnimation : MonoBehaviour
{

    // NOTA: Copiata da TransformAnimation
    // PER ORA GESTISCE SOLO ROTAZIONI
    // VA DA UN MOVIMENTO AD UN ALTRO E TORNA INDIETRO

    // [SerializeField] private Vector3 translation = Vector3.zero;
    [SerializeField] private Vector3 rotationAxis = Vector3.right;
    [SerializeField] private float rotationDegrees = 0f;
    // [SerializeField] private Vector3 targetScale = Vector3.one;
    // [SerializeField] private float duration = 1f;

    [SerializeField] private AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    // private Vector3 startPosition;
    private Quaternion startRotation;
    // private Vector3 startScale;

    // private Vector3 targetPosition;
    private Quaternion targetRotation;

    private Transform _transform;



    private void Start()
    {
        _transform = GetComponent<Transform>();
        //  startPosition = _transform.localPosition;
        startRotation = _transform.localRotation;
        // startScale = _transform.localScale;

        // targetPosition = startPosition + translation;
        targetRotation = startRotation * Quaternion.AngleAxis(rotationDegrees, rotationAxis);

        //Play();
    }

    //private Coroutine currentCoroutine;
    public void Play(float duration, bool forward)
    {
        StopAllCoroutines();
        StartCoroutine(Animate(duration, forward));
    }

    public void PlayComplete(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(AnimateComplete(duration));
    }

    IEnumerator Animate(float duration, bool forward)
    {
        // PRIMO MOVIMENTO

        // Vector3 currentStartPosition = forward ? startPosition : targetPosition ;
        // Vector3 currentStartScale = forward ? startScale : targetScale ;
        // Quaternion currentStartRotation = forward ? startRotation : targetRotation ;

        // Vector3 currentStartPosition = _transform.localPosition;
        // Vector3 currentStartScale = _transform.localScale;
        Quaternion currentStartRotation = _transform.localRotation;

        // Vector3 currentTargetPosition = forward ? targetPosition : startPosition;
        // Vector3 currentTargetScale = forward ? targetScale : startScale;
        Quaternion currentTargetRotation = targetRotation;
        float t = 0f;

        if (forward)
        {
            t = 0f;
            while (t < 0.5f)
            {
                //Debug.Log("In movimento");
                yield return null;
                t += Time.deltaTime / duration;

                //_transform.localPosition = Vector3.Lerp(currentStartPosition, currentTargetPosition, curve.Evaluate(t));
                //_transform.localScale = Vector3.Lerp(currentStartScale, currentTargetScale, curve.Evaluate(t));
                _transform.localRotation = Quaternion.Lerp(currentStartRotation, currentTargetRotation, curve.Evaluate(t));
            }
        }

        if (!forward)
        {
            // PARTE RITORNO A INIZIO

            currentTargetRotation = startRotation;

            currentStartRotation = targetRotation;

            //t = 0f;
            while (t < 1f)
            {
                //Debug.Log("Ritorno a pos iniziale");
                yield return null;
                t += Time.deltaTime / duration;

                //_transform.localPosition = Vector3.Lerp(currentStartPosition, currentTargetPosition, curve.Evaluate(t));
                //_transform.localScale = Vector3.Lerp(currentStartScale, currentTargetScale, curve.Evaluate(t));
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
