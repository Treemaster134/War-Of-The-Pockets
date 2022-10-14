using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float _speed;
    public Sprite _Image;
    [SerializeField] private SpriteRenderer _renderer;
    public int _destination;
    public Vector3[] _destinations;
    private bool _canMove = true;
    private bool countingDown = false;
    public float _health;
    public GameObject managerReference;
    



    // Start is called before the first frame update
    void Start()
    {
        _renderer.sprite =  managerReference.GetComponent<ManagerScript>().enemySprites[(int)_health - 1];
        for(int i = 0; i < _destinations.Length; i++)
        {
            //Debug.Log("ADDING");
            _destinations[i].x += 0.5f;
            _destinations[i].y += 0.5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.right = _destinations[_destination] - transform.position;
        if(transform.position == new Vector3(6.5f,5.5f, 0f))
        {
            managerReference.GetComponent<ManagerScript>().health -= 5;
            managerReference.GetComponent<ManagerScript>().enemyAmount -= 1;
            Destroy(gameObject);
        }
        if(Vector3.Distance(transform.position, _destinations[_destination]) < Mathf.Epsilon)
        {
            _destination += 1;
            StartCoroutine(moveCountDown(1/_speed));
        }
        else
        {
            if(_canMove == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, _destinations[_destination], _speed * Time.deltaTime);
            }
        }

        if(_health < 1)
        {
            managerReference.GetComponent<ManagerScript>().money += 5;
            managerReference.GetComponent<ManagerScript>().enemyAmount -= 1;
            Destroy(gameObject);
        }
    }

    public void takeDamage()
    {
        if(_health - 1 > 0)
        {
            managerReference.GetComponent<ManagerScript>().money += 5;
        }
        GetComponent<AudioSource>().Play();
        _health -= 1;
        _speed = _health;
        _renderer.sprite = managerReference.GetComponent<ManagerScript>().enemySprites[(int)_health - 1];
        
    }

    private IEnumerator moveCountDown(float time)
    {
        
        _canMove = false;
        if(countingDown == true)
        {
            yield break;
        }
        countingDown = true;
        yield return new WaitForSeconds(time);
        _canMove = true;
        countingDown = false;
    }
}
