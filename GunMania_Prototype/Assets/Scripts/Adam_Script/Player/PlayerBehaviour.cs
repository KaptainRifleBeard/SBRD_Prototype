using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{

    //Variables
    public float movementSpeed;
    public GameObject cam;
    public GameObject bulletSpawnPoint;
    public float waitToShoot;
    public GameObject playerObject;
    public GameObject bullet;
    private Transform bulletSpawn;
    public float points;
    public float fireRate;
    //private float nextFire = 0f;
    public float maxHealth = 100f;
    public float health;
    public bool player2Wins;
    public Healthbar healthBar;
    public int FoodCarried;

    //Methods

    public void Start()
    {
        points = 0f;
        health = maxHealth;
        player2Wins = false;
        healthBar.SetMaxHealth(maxHealth);
        FoodCarried = 0;
    }

    private void Update()
    {
        Plane playerPlane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float hitDist = 0.0f;

        if (playerPlane.Raycast(ray,out hitDist))
        {
            Vector3 targetPoint = ray.GetPoint(hitDist);
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
            targetRotation.x = 0;
            targetRotation.z = 0;
            playerObject.transform.rotation = Quaternion.Slerp(playerObject.transform.rotation, targetRotation, 7f * Time.deltaTime);
        }

        //Shooting
        if (Input.GetMouseButton(0) && FoodCarried >= 1)
        {
            Shoot();
            FoodCarried -= 1;
        }

        //Player Taking Damage
        healthBar.SetHeatlh(health);

        //Player Death
        if (health <= 0)
        {
            Die();
        }
    }

    private void FixedUpdate()
    {
        //Player Movement
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * movementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * movementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * movementSpeed * Time.deltaTime);
        }
    }

    void Shoot()
    {
        bulletSpawn = Instantiate(bullet.transform, bulletSpawnPoint.transform.position, Quaternion.identity);
        bulletSpawn.rotation = bulletSpawnPoint.transform.rotation;
    }

    public void Die()
    {
        Debug.Log("You died");
        Destroy(this.gameObject);
        player2Wins = true;
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "FoodPickup" && FoodCarried < 2)
        {
            FoodCarried += 1;
            Debug.Log(FoodCarried);
        }
    }*/

}