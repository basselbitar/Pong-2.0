using Alteruna;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : AttributesSync
{
    public ParticleSystem windEffect;
   

    public void PlayWindEffect(bool forward) {
        if(forward) {
            windEffect.transform.SetPositionAndRotation(new Vector3(-9.2f, 0, 0), Quaternion.Euler(0, 90f, 0));
        }
        else {
            windEffect.transform.SetPositionAndRotation(new Vector3(9.2f, 0, 0), Quaternion.Euler(0, -90f, 0));
        }
        windEffect.GetComponent<ParticleSystem>().Play();
    }
}
