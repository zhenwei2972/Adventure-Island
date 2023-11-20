using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugField : MonoBehaviour
{
    [SerializeField] TMP_Text _debugTextfield;

    void OnEnable()
    {
        Utils.OnDebugMessage += SetText;
    }

    void OnDisable()
    {
        Utils.OnDebugMessage -= SetText;
    }

    void SetText(string debugMessage) 
    {
        _debugTextfield.text = $"{debugMessage} \n {_debugTextfield.text}";
    }
}
