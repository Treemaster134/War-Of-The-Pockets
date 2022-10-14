using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileScript : MonoBehaviour
{
    public GameObject target;
    public float _speed;
    public int type;
    public LayerMask enemyLayer;
    [SerializeField] private GameObject boomPrefab;
    private bool alive = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(alive == true)
        {
            try
            {
                transform.right = target.transform.position - transform.position;
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, _speed * Time.deltaTime);
            }
            catch(System.Exception)
            {
                Destroy(gameObject);
            }
            
            Collider2D[] enemyCols = Physics2D.OverlapCircleAll(transform.position, 0.1f/2, enemyLayer);
            
            if(enemyCols.Length > 0)
            {
                alive = false;
                enemyCols[0].gameObject.GetComponent<EnemyScript>().takeDamage();
                if(type == 1)
                {
                    Instantiate(boomPrefab, transform.position, Quaternion.identity);
                    
                    for(int i = 0; i < enemyCols.Length; i++)
                    {
                        enemyCols[i].gameObject.GetComponent<EnemyScript>().takeDamage();
                    }
                }
                
            }
        }
        else
        {
            if (gameObject != null)
            {    
                // Do something  
                Destroy(gameObject);
            } 
        }
    }

}
