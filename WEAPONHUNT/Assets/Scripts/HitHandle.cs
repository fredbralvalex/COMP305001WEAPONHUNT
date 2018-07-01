using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitHandle : MonoBehaviour {
    
    private IEnumerator KillOnAnimationEnd()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
        enabled = false;
    }

    void FixedUpdate() {
        StartCoroutine(KillOnAnimationEnd());
    }
}
