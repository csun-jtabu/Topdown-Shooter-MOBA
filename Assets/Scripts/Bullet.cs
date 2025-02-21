using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public int Dmg = 1;
    public float Speed = 10f;
    public float ExpireTime = 5f;
    public int Team = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ExpireAfterTime();
    }

    IEnumerator ExpireAfterTime()
    {
        yield return new WaitForSeconds(ExpireTime);
        Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        // needs to move forward constantly at speed
    }
}
