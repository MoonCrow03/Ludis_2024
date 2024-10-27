using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteUI : MonoBehaviour
{
    [SerializeField] private Scenes m_NextScene;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void LoadNextLevel()
    {
        GameManager.Instance.LoadScene(m_NextScene);
    }
}