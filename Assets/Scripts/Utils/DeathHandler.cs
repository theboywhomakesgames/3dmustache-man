using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathHandler : MonoBehaviour
{
    public float beforeRestart = 1;

    private void Start()
    {
        GetComponent<Person>().OnDeath += OnDeath;
    }

    private void OnDeath()
    {
        Invoke("ReStartLevel", beforeRestart);
    }

    private void ReStartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
