using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R_HideOnPlay : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(false);
    }
}
