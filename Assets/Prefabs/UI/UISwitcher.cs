using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISwitcher : MonoBehaviour
{
    [SerializeField] Transform DefaultSubUI;
    Transform CurrentActivatedUI;
    void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.parent == transform) { child.gameObject.SetActive(false);}
        }
        SetActiveUI(DefaultSubUI);
    }
    public void SetActiveUI(Transform newActiveUI)
    {
        if(newActiveUI==CurrentActivatedUI) { return; }
        if(CurrentActivatedUI!=null) { CurrentActivatedUI.gameObject.SetActive(false); }
        newActiveUI.gameObject.SetActive(true);
        CurrentActivatedUI = newActiveUI;
    }
    
}
