using Alteruna;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    private Spawner _spawner;
    void Start()
    {
        _spawner = FindObjectOfType<Spawner>();
    }

    public GameObject SpawnBall() {
       return _spawner.Spawn(0, Vector3.zero, Quaternion.identity, new Vector3(0.4f, 0.4f, 1f));
    }
}
