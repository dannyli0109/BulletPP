using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake current;
    bool shaking = false;
    Transform holder;

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        holder = transform;
    }
    public void Shake(float magX, float magY, float time, AnimationCurve curve)
    {
        if (!shaking)
        {
            StartCoroutine(DoShake(magX, magY, time, curve));
        }
    }


    IEnumerator DoShake(float magX, float magY, float time, AnimationCurve curve)
    {
        float elapsed = 0;
        shaking = true;
        while (elapsed < time)
        {
            float shakeX = Random.Range(-magX, magX);
            float shakeY = Random.Range(-magY, magY);
            shakeX *= curve.Evaluate(elapsed);
            shakeY *= curve.Evaluate(elapsed);
            holder.localPosition = new Vector3(shakeX, shakeY);
            elapsed += Time.deltaTime;
            yield return null;
        }
        holder.localPosition = new Vector3(0, 0);
        shaking = false;
        yield return null;
    }


}
