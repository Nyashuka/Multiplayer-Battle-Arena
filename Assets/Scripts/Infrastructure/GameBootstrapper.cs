using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
    public class GameBootstrapper : MonoBehaviour
    {
        public void Start()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}