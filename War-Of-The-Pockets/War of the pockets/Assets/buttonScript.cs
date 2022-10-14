using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonScript : MonoBehaviour
{
    public int _type;
    public GameObject manager;

    public void buy()
    {
        manager.GetComponent<ManagerScript>().buyUnit(_type);
    }
    
}
