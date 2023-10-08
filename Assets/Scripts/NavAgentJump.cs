using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavAgentJump : MonoBehaviour
{
    public AnimationCurve Curve = new AnimationCurve();
    private float duration = 5.0f;
        // Start is called before the first frame update
    void Start() 
    {
        NavMeshAgent nav = GetComponent<NavMeshAgent>();
        nav.autoTraverseOffMeshLink = false;
        if (nav.isOnOffMeshLink) {
            OffMeshLinkData data = nav.currentOffMeshLinkData;
            Vector3 startPos = nav.transform.position;
            Vector3 endPos = data.endPos + Vector3.up * nav.baseOffset;
            float normalizedTime = 0.0f;
            while (normalizedTime < 1.0f)
            {
                float yOffset = Curve.Evaluate(normalizedTime);
                nav.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
                normalizedTime += Time.deltaTime / duration;
            }
            nav.CompleteOffMeshLink();
        }
    }
}
