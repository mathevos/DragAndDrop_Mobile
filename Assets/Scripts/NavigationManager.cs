using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationManager : MonoBehaviour
{
    static NavigationManager instance;
    [SerializeField] private int sceneDestination; // Index de la scène à charger

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeDeScene(int sceneDestination)
    {
        SceneManager.LoadScene(sceneDestination);
    }


}
