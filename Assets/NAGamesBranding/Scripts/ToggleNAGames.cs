using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleNAGames : Singleton<ToggleNAGames>
{
   public void SetCanvasActive(bool setActive)
   {
       GetComponent<Canvas>().enabled = setActive;
   }
}
