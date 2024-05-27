using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(ParticleSystem))]
public class DestroyParticleSystem : MonoBehaviour
{
    private void Awake()
    {
        Destroy(gameObject, GetComponent<ParticleSystem>().main.duration);
    }
}
