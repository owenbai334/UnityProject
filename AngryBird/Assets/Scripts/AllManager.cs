using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AllManager : MonoBehaviour
{
   public void Begin()
   {
        SceneManager.LoadScene("Levil");
   } 
   public void Quit()
   {
        Application.Quit();
   }
}
