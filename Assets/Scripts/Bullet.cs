using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public int Dmg = 1;
    public float Speed = 20f;
    public float ExpireTime = 5f;
    public int Team = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * Speed, ForceMode2D.Impulse);
        StartCoroutine(ExpireAfterTime());
    }

    IEnumerator ExpireAfterTime()
    {
        yield return new WaitForSeconds(ExpireTime);
        Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollision2D(Collision2D collision)
    {
        Entity target = this.GetComponent<Entity>();
        if (target)
        {
            target.Damage(Dmg, Team);
        }
        Destroy(gameObject);
    }
}
