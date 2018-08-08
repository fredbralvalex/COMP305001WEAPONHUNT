using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyUnitySingletonAudio : MonoBehaviour {

    private static MyUnitySingletonAudio instance = null;
    public static MyUnitySingletonAudio Instance
    {
        get { return instance; }
    }
    void Awake()
    {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        } else {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
