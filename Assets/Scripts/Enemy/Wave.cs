using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemies;
    [SerializeField]
    private float[] spawndelay;
    private float timer = 0.0f;
    private int iterator = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(iterator < enemies.Length)
        {
            timer += Time.deltaTime;
            if(timer > spawndelay[iterator])
            {
                timer = 0.0f;
                enemies[iterator].SetActive(true);
            }
        }
        else
        {
            bool deactivate = true;
            foreach(GameObject enemy in enemies)
            {
                if (enemy.activeSelf)
                {
                    deactivate = false;
                    break;
                }
            }
            if (deactivate)
                Destroy(gameObject);
        }
    }
}
