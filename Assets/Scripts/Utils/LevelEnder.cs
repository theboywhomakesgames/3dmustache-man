using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnder : MonoBehaviour
{
    public Light light;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 9)
        {
            light.DOIntensity(100, 2);
            Invoke(nameof(LoadNext), 2);
        }
    }

    private void LoadNext()
    {
        int bi = SceneManager.GetActiveScene().buildIndex;
        if(bi < SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(bi + 1);
        }
    }
}
