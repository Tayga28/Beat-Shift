//This Script Will Be Attached To The Parent Of This Obstacle That's Why I Have To Do All This Find Gameobject Stuff

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandingBox : MonoBehaviour
{
    private Transform expandingBox;
    private Vector3 startScale;
    public Vector3 finishScale;
    public bool repeatable;
    public float expandSpeed = 2f;
    public float speedModifier = 1f;
    public float duration = 5f;
    // Start is called before the first frame update
    void Awake()
    {
        expandingBox = GetComponent<Transform>();
        repeatable = true;
    }

    IEnumerator Start ()
    {
        startScale = expandingBox.localScale;
        while (repeatable)
        {
            yield return LerpBoxStretch(startScale, finishScale, duration);
            yield return LerpBoxStretch(finishScale, startScale, duration);
        }
    }

    public IEnumerator LerpBoxStretch(Vector3 a, Vector3 b, float time)
    {
        float i = 0.0f;
        float rate = (1.0f / time) * expandSpeed;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            expandingBox.localScale = Vector3.Lerp(a, b, i);
            yield return null;
        }
    }
}
