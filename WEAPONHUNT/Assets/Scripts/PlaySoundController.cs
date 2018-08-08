using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundController : MonoBehaviour {

    public AudioSource source;
    Renderer rend;

    public void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            rend.enabled = true;
        }
    }

    public void PlayAudioSource()
    {
        source.Play(0);
        if (rend != null)
        {
            rend.enabled = false;
        }
        //gameObject.SetActive(false);
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 10, gameObject.transform.position.z);
        Destroy(gameObject, source.clip.length);
    }
}
