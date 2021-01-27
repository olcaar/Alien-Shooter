using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    // config param
    [Header("Player Movement")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 1f;
    [SerializeField] int health = 200;
    

    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileFiringPeriod = 0.15f;


    [Header("VFX & SFX")]
    [SerializeField] AudioClip hitSound;
    [Range(0f, 1f)] [SerializeField] float hitVolume;
    [SerializeField] AudioClip deathSound;
    [SerializeField] GameObject explotionVFX;
    [SerializeField] float durationOfExplotion = 1f;
    [SerializeField] AudioClip shotSound;
    [Range(0f, 1f)] [SerializeField] float shootVolume = 0.7f;


    Coroutine firingCoroutine;

    float XMin;
    float xMax;
    float yMin;
    float yMax;

    // cached reference
    
    void Start()
    {
        SetUpMoveBoundaries();
    }

    void Update()
    {
        Move();
        Fire();
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
           firingCoroutine = StartCoroutine(FireContinuousluy());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }
    IEnumerator FireContinuousluy()
    {
        while (true)
        {
            AudioSource.PlayClipAtPoint(shotSound, Camera.main.transform.position,shootVolume); //play SFX
            GameObject laser = Instantiate(
                laserPrefab,
                transform.position,
                Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);           
            yield return new WaitForSeconds(projectileFiringPeriod);

        }


    }
    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed; ;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, XMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);

        transform.position = new Vector2(newXPos, newYPos);
    }
    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        XMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);
    }
    private void ProcessHit(DamageDealer damageDealer)
    {
        AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position, hitVolume);
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }
    private void Die()
    {    
        GameObject boom = Instantiate(explotionVFX, transform.position, transform.rotation);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position); //play SFX
        Destroy(gameObject);
        Destroy(boom, durationOfExplotion);
        FindObjectOfType<Level>().LoadGamerOver();
    }
    public int GetHealth()
    {
        return health;
    }
}
