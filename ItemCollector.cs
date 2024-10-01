using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    private int bannanaCounter = 0;

    [SerializeField] private Text BannanasText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bannana"))
        {
            Destroy(collision.gameObject);
            bannanaCounter++;
            BannanasText.text = "Bannanas: " + bannanaCounter;
        }
    }
}
