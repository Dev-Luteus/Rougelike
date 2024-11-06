using System;
using UnityEditor;
using UnityEngine;

public class Food : MonoBehaviour, IInteractable
{
    private GameObject _gameObject;
    public void SetGameObject(GameObject gameObject)
    {
        _gameObject = gameObject;
    }
    public void OnEnter()
    {
        //add to list ( if inventory )  
        Destroy(_gameObject);
        Debug.Log("Collected!");
    }
}