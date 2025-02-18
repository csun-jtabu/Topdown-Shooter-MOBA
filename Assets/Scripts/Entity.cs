using UnityEngine;

public class Entity : MonoBehaviour
{
    public int MaxHp = 10;
    protected int Hp;
    public float Speed = 0f;
    public int Team = 0;
    public int Dmg = 1;
    protected float AttackSpeed = 1f;


    public virtual void Damage(int dmg, int team)
    {
        if (team != this.Team)
        {
            this.Hp -= dmg;
            if (this.Hp <= 0)
                Destroy(this);
        }
    }

    public void Fire(Vector2 dir)
    {

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
