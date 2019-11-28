using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{
    DayNightCycle time;

    public GameObject[] campfireStages;

    int dayCreated;

    void Awake()
    {
        time = GameObject.Find("Sun and Moon").GetComponent<DayNightCycle>();
    }

    void Start()
    {
        dayCreated = time.dayCount;
    }

    void Update()
    {
        int age = time.dayCount - dayCreated;

        if (age < campfireStages.Length)
        {
            if (campfireStages[age].activeSelf == false)
            {
                for (int i = 0; i < campfireStages.Length; i++)
                {
                    if (i == age)
                        campfireStages[i].SetActive(true);
                    else
                        campfireStages[i].SetActive(false);
                }
            }
        }
        else
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
