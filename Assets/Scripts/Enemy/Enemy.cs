using UnityEngine;

public class Enemy : Entity
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // This is to make it so that the enemy will only shoot at a certain distance
    private float _distanceToShoot = 5f;
    private Transform _player;

    void Start()
    {
        this.Speed = 5f;
        this.Hp = 10;
        this.Dmg = 1;
        _player = FindAnyObjectByType<Player>().transform; // this finds the player's transform/location
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(_player.position, transform.position) <= _distanceToShoot)
        {
            Fire();
        }
    }


}
