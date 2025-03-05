using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour
{
    public int MaxHp = 10;
    protected int Hp;
    public float Speed = 0f;
    public int Team = 1;
    public int Dmg = 1;
    protected float AttackSpeed = 1f;
    protected bool CanFire = true;
    public GameObject BulletPrefab;


    public virtual void Damage(int dmg, int team)
    {
        if (team != this.Team)
        {
            this.Hp -= dmg;
            if (this.Hp <= 0)
                Destroy(this);
        }
    }


    IEnumerator WeaponCooldown()
    {
        yield return new WaitForSeconds(AttackSpeed);
        CanFire = true;
    }

    public void Fire()
    {
        if (CanFire)
        {
            // fires bullet forward in direction parent is facing
            Vector3 firePoint = transform.position;
            //offset fire position to front of entity
            firePoint.y += 0.6f;
            GameObject bulletObject = Instantiate(BulletPrefab, firePoint, transform.rotation);
            Bullet bullet = bulletObject.GetComponent<Bullet>();
            bullet.Team = this.Team;
            bullet.Dmg = this.Dmg;
            CanFire = false;
            StartCoroutine(WeaponCooldown());
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
