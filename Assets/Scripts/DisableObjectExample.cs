using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisableObjectExample : MonoBehaviour
{
    public TextMeshProUGUI initialMessage;
    private float activeTime = 3f;

    private void Awake()
    {
        initialMessage.text = "Collect 8 notes to survive...";
    }
    void Update()
    {
        if (initialMessage.IsActive())
        {
            StartCoroutine(DisableObj());
        }
    }
    IEnumerator DisableObj()
    {
        yield return new WaitForSeconds(activeTime);
        initialMessage.gameObject.SetActive(false);
    }
}

