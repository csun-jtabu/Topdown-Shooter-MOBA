using UnityEngine;

public class Player : Entity
{
    [SerializeField]
    private int MaxShield = 10;
    [SerializeField]
    private int Shield = 10;
    private float Spread = 0f;
    [SerializeField]
    private float RespawnTimer = 3f;


    public override void Damage(int dmg, int team)
    {
        if (team != this.Team)
        {
            if (Shield == 0)
            {
                this.Hp -= dmg;
                if (this.Hp <= 0)
                    Respawn();
            }
            else if (Shield >= dmg)
                Shield -= dmg;
            else
            {
                dmg -= Shield;
                Shield = 0;
                this.Damage(dmg, team);
            }
        }
    }

    public void Respawn()
    {

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.Speed = 5f;
        this.Hp = 10;
        this.MaxHp = 10;
        this.Dmg = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
