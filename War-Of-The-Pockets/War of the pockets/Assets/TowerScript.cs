using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerScript : MonoBehaviour
{
    public Sprite _Image;
    [SerializeField] private SpriteRenderer _renderer;
    private bool _canShoot = true;
    private bool _countingDown = false;
    public LayerMask enemyLayer;
    public Transform radius;
    public float _firerate;
    public GameObject managerReference;
    private int rounds;
    public int type;
    public GameObject bulletPrefab;
    public GameObject rocketPrefab;
    //private Collider2D[] enemies;


    // Start is called before the first frame update
    void Start()
    {
        _renderer.sprite =  _Image;
        rounds = managerReference.GetComponent<ManagerScript>().rounds;
    }

    // Update is called once per frame
    void Update()
    {
        
        Collider2D[] enemyCols = Physics2D.OverlapCircleAll(transform.position, radius.localScale.x/2, enemyLayer);

        if(type != 3)
        {
            int index = 0;
            int indexDest = 0;
            for(int i = 0; i < enemyCols.Length; i++)
            {
                if(enemyCols[i].gameObject.GetComponent<EnemyScript>()._destination > indexDest)
                {
                    indexDest = enemyCols[i].gameObject.GetComponent<EnemyScript>()._destination;
                    index = i;
                }
            }
            try
            {
                transform.right = enemyCols[index].gameObject.transform.position - transform.position;
            }
            catch
            {
                
            }
            if(_canShoot == true && enemyCols.Length > 0)
            {
                shoot(enemyCols, index);
                StartCoroutine(shootCountDown(1/_firerate));
            }
            if(managerReference.GetComponent<ManagerScript>().rounds > rounds + 4)
            {
                rounds = managerReference.GetComponent<ManagerScript>().rounds;
                _firerate += 0.15f;
            }
        }
        else if(type == 3)
        {
            if(_canShoot == true && enemyCols.Length > 0)
            {
                shoot(enemyCols, 0);
                StartCoroutine(shootCountDown(1/_firerate));
            }
        }
    }

    private void shoot(Collider2D[] cols, int colIndex)
    {
        GetComponent<AudioSource>().Play();
        if(type == 3)
        {
            for(int i = 0; i < cols.Length; i++)
            {
                cols[i].gameObject.GetComponent<EnemyScript>().takeDamage();
            }
        }
        else if(type == 2)
        {
            GameObject boolet = Instantiate(rocketPrefab, transform.position, Quaternion.identity);
            boolet.GetComponent<projectileScript>()._speed = 4;
            boolet.GetComponent<projectileScript>().target = cols[colIndex].gameObject;
        }
        else
        {
            GameObject boolet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            boolet.GetComponent<projectileScript>()._speed = 3;
            boolet.GetComponent<projectileScript>().target = cols[colIndex].gameObject;
        }
    }

    private IEnumerator shootCountDown(float time)
    {
        
        _canShoot = false;
        if(_countingDown == true)
        {
            yield break;
        }
        _countingDown = true;
        yield return new WaitForSeconds(time);
        _canShoot = true;
        _countingDown = false;
    }

}
