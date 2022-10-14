using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ManagerScript : MonoBehaviour
{
    public Vector3[] destinations;
    [SerializeField] private int startEnemyAmount;
    [SerializeField] private Sprite[] unitSprites;
    public Sprite[] enemySprites;
    [SerializeField] private int[] unitPrices;
    public int enemyAmount = 0;
    public int rounds;

    public GameObject enemyPrefab;
    public GameObject playerPrefab;

    public int money = 20;
    public int health;

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI hideButtonText;
    [SerializeField] private TextMeshProUGUI roundsText;
    private bool uiHidden = false;
    private bool placingUnit = false;
    private int[] unitBeingPlaced = {0, 0};

    [SerializeField] GameObject[] placementCursor;
    [SerializeField] private Image uiBar;
    [SerializeField] private GameObject nextRoundButton;
    private bool roundEnded = false;
    public GameObject gameOverUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(health > 0)
        {
            
            moneyText.text = "Money: " + "$" + money;
            healthText.text = "Health: "+ health;
            roundsText.text = "Rounds: " +  rounds;

            if(placingUnit)
            {
                placementCursor[0].SetActive(true);
                placementCursor[1].GetComponent<SpriteRenderer>().sprite = unitSprites[unitBeingPlaced[0]];
                placementCursor[2].transform.localScale = new Vector3(unitBeingPlaced[1], unitBeingPlaced[1], 0);
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                placementCursor[0].transform.position = mousePos;
                if(Input.GetMouseButtonDown(0))
                {
                    placeUnit(unitBeingPlaced[0]);
                }
            }
            else
            {
                placementCursor[0].SetActive(false);
            }

            if(Input.GetKeyDown(KeyCode.Q) && placingUnit)
            {
                sellUnit(unitBeingPlaced[0]);
                placingUnit = false;
            }

            if(health < 1)
            {
                Die();
            }

            if(roundEnded == false)
            {
                enemyAmount = GameObject.FindGameObjectsWithTag("Enemy").Length;
            }

            if(enemyAmount < 1 && roundEnded == false)
            {
                rounds += 1;
                money += 10;
                roundEnded = true;
                nextRoundButton.SetActive(true);
            }
            else if(enemyAmount > 1)
            {
                nextRoundButton.SetActive(false);
            }
        }
        else
        {
            Die();
        }
    }

    

    public void spawnWave()
    {
        enemyAmount = startEnemyAmount + rounds * 2;
        roundEnded = false;
        List<Vector3> tempDest = new List<Vector3>();
        List<Vector3> destToList = new List<Vector3>(destinations);
        int startX = 9 + enemyAmount;
        for(int i = 0; i < enemyAmount; i++)
        {
            tempDest.Add(new Vector3(startX - i, -4, 0));
        }
        for(int i = 0; i < destinations.Length; i++)
        {    
            tempDest.Add(destToList[i]);
        }
        
        for(int i = 0; i < enemyAmount; i++)
        {
            Vector3 curPos = tempDest[enemyAmount - i - 2];
            GameObject curEnemy = Instantiate(enemyPrefab, curPos, Quaternion.identity);
            curEnemy.GetComponent<EnemyScript>()._destinations = tempDest.ToArray();
            curEnemy.GetComponent<EnemyScript>()._destination = enemyAmount - i;
            curEnemy.GetComponent<EnemyScript>().managerReference = gameObject;
            if(i < 10)
            {
                curEnemy.GetComponent<EnemyScript>()._speed = 1;
                curEnemy.GetComponent<EnemyScript>()._health = 1;
            }
            else if(i > 9 && i < 20)
            {
                curEnemy.GetComponent<EnemyScript>()._speed = 2;
                curEnemy.GetComponent<EnemyScript>()._health = 2;
            }
            else if(i > 19 && i < 30)
            {
                curEnemy.GetComponent<EnemyScript>()._speed = 3;
                curEnemy.GetComponent<EnemyScript>()._health = 3;
            }
            else if(i > 29 && i < 40)
            {
                curEnemy.GetComponent<EnemyScript>()._speed = 4;
                curEnemy.GetComponent<EnemyScript>()._health = 4;
            }
            else if(i > 39 && i < 50)
            {
                curEnemy.GetComponent<EnemyScript>()._speed = 6;
                curEnemy.GetComponent<EnemyScript>()._health = 5;
            }
            
        }
    }

    public void buyUnit( int type)
    {
        if(money - unitPrices[type] > 0)
        {
            money -= unitPrices[type];
            unitBeingPlaced[0] = type;
            switch(type)
            {
                case 0:
                    unitBeingPlaced[1] =  4;
                    break;
                case 1:
                    unitBeingPlaced[1] =  5;
                    break;
                case 2:
                    unitBeingPlaced[1] =  8;
                    break;
                case 3:
                    unitBeingPlaced[1] =  4;
                    break;
            }
            placingUnit = true;
        }
    }

    public void placeUnit(int type)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if(hit.transform.gameObject.layer == 8)
        {
            Debug.Log("stinky");
            placingUnit = false;
            sellUnit(type);
            return;
        }

        if(Mathf.Round(mousePos.x) < mousePos.x)
        {
            mousePos.x = Mathf.Round(mousePos.x);
            mousePos.x += 0.5f;
        }
        else
        {
            mousePos.x = Mathf.Round(mousePos.x);
            mousePos.x -= 0.5f;
        }

        if(Mathf.Round(mousePos.y) < mousePos.y)
        {
            mousePos.y = Mathf.Round(mousePos.y);
            mousePos.y += 0.5f;
        }
        else
        {
            mousePos.y = Mathf.Round(mousePos.y);
            mousePos.y -= 0.5f;
        }

        GameObject curUnit = Instantiate(playerPrefab, mousePos, Quaternion.identity);
        curUnit.GetComponent<TowerScript>()._Image = unitSprites[type];
        curUnit.GetComponent<TowerScript>()._Image = unitSprites[type];
        curUnit.GetComponent<TowerScript>().type = type;
        switch(type)
        {
            case 0:
                curUnit.GetComponent<TowerScript>()._firerate = 1f;
                break;
            case 1:
                curUnit.GetComponent<TowerScript>()._firerate = 2;
                break;
            case 2:
                curUnit.GetComponent<TowerScript>()._firerate = 1;
                break;
            case 3:
                curUnit.GetComponent<TowerScript>()._firerate = 0.25f;
                break;
        }
        curUnit.GetComponent<TowerScript>().radius.localScale = new Vector3(unitBeingPlaced[1], unitBeingPlaced[1], 0);
        curUnit.GetComponent<TowerScript>().managerReference = gameObject;
        placingUnit = false;
    }

    void sellUnit(int type)
    {
        money += unitPrices[type];
    }

    public void hideUnhideUI()
    {
        uiHidden = !uiHidden;
        if(uiHidden == true)
        {
            hideButtonText.text = ">>";
            Vector3 barPos = new Vector3(0,85,0);
            uiBar.GetComponent<RectTransform>().anchoredPosition = barPos;
        }
        else
        {
            hideButtonText.text = "<<";
            Vector3 barPos = new Vector3(0,-102.5f,0);
            uiBar.GetComponent<RectTransform>().anchoredPosition = barPos;
        }
    }

    public void returnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
        PlayerPrefs.SetInt("HighScore", rounds);
    }

    void Die()
    {
        gameOverUI.SetActive(true);
    }


}
