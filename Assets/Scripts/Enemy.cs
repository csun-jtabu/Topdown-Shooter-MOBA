using UnityEngine;

public class Enemy : Entity
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.Speed = 5f;
        this.Hp = 5;
        this.Dmg = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
