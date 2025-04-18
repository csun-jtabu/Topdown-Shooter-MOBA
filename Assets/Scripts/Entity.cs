using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour
{
    public int MaxHp = 10;
    public int Hp;
    public float Speed = 0f;
    public int Team = 1;
    public int Dmg = 1;
    protected float AttackSpeed = 1f;
    protected bool CanFire = true;
    public GameObject BulletPrefab;
    public AudioClip BulletSound;


    public virtual void Damage(int damage, int team)
    {
        if (team != this.Team)
        {
            this.Hp -= damage;
            if (this.Hp <= 0)
                Destroy(this.gameObject);
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
            AudioSource.PlayClipAtPoint(BulletSound, transform.position, 1f);
            // fires bullet forward in direction parent is facing
            Vector3 firePoint = transform.position + transform.up * 0.75f;
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
        Hp = MaxHp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
