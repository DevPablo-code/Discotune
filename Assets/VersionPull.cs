using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VersionPull : MonoBehaviour
{
    void Start()
    {
        this.GetComponent<TextMeshProUGUI>().text = Application.version;
    }
}
