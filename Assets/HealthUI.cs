using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    public static HealthUI instance;

    public int currentHealth;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateUI(int newAmount)
    {
        
        for (int i = 0; i < 3; i++)
        {
            if (i < newAmount)
            {
                if (i > currentHealth - 1)
                {
                    transform.GetChild(i).GetComponent<Animator>().SetBool("HasHealed", true);
                    transform.GetChild(i).GetComponent<Animator>().SetBool("BoneBreak", false);
                }
            }
            else
            {
                if (i < currentHealth)
                {
                    transform.GetChild(i).GetComponent<Animator>().SetBool("HasHealed", false);
                    transform.GetChild(i).GetComponent<Animator>().SetBool("BoneBreak", true);
                }
            }
            
        }

        /*
        if (currentHealth > newAmount)
        {
            for (int i = currentHealth; i < newAmount; i++)
            {
                transform.GetChild(i - 1).GetComponent<Animator>().SetBool("HasHealed", true);
                transform.GetChild(i - 1).GetComponent<Animator>().SetBool("BoneBreak", false);
            }
        }
        else
        {
            for (int i = newAmount; i < currentHealth; i++)
            {
                transform.GetChild(i).GetComponent<Animator>().SetBool("HasHealed", false);
                transform.GetChild(i).GetComponent<Animator>().SetBool("BoneBreak", true);
            }
        }
        */

        currentHealth = newAmount;
    }




}
