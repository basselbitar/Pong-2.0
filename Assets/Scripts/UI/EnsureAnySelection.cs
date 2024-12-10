using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnsureAnySelection : MonoBehaviour
{
    private GameObject _lastSelectedGameObject;
    
    // Update is called once per frame
    void Update()
    {
        if( !SomethingIsSelected()) {
            EventSystem.current.SetSelectedGameObject(_lastSelectedGameObject);
        }
    }

    private bool SomethingIsSelected() {
        if(EventSystem.current.currentSelectedGameObject == null) {
            return false;
        } else {
            _lastSelectedGameObject = EventSystem.current.currentSelectedGameObject;
        }
        return true;
    }
}
