using UnityEngine;

public class Enemy : MonoBehaviour
{
    // config params
    [Header("configs")]
    [SerializeField] float health = 100f;
    [SerializeField] int enemyScore = 100;
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;    
    [SerializeField] GameObject enemyShotPrefab;
    [SerializeField] float projectileSpeed = -10f;

    [Header("VFX & SFX")]
    [SerializeField] AudioClip shotSound;
    [Range(0f, 1f)] [SerializeField] float shootVolume = 0.7f;
    [SerializeField] AudioClip deathSound;
    [Range(0f, 1f)] [SerializeField] float deathVolume = 0.7f;
    [SerializeField] GameObject explotionVFX;
    [SerializeField] float durationOfExplotion = 1f;

    //cached reference
    GameSession gameSession;


    void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        gameSession = FindObjectOfType<GameSession>();
    }

    void Update()
    {
        CountDownAndShot();
    }

    private void CountDownAndShot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }
    private void Fire()
    {
        AudioSource.PlayClipAtPoint(shotSound, Camera.main.transform.position, shootVolume); //play SFX
        GameObject shot = Instantiate(
                enemyShotPrefab,
                transform.position,
                Quaternion.identity) as GameObject;
        shot.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);
    }
    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {            
            Die();
        }
    }
    private void Die()
    {
        gameSession.AddToScore(enemyScore);
        Destroy(gameObject);
        GameObject boom = Instantiate(explotionVFX, transform.position, transform.rotation);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathVolume); 
        Destroy(boom, durationOfExplotion);
    }
}
