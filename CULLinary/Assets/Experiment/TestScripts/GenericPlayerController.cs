using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericPlayerController : MonoBehaviour
{

  public GenericCharacterBehaviour[] behaviours;

  private void Start()
  {
    Cursor.visible = false;
    Cursor.lockState = CursorLockMode.Locked;
  }
  private void Update()
  {
    foreach (GenericCharacterBehaviour charBh in behaviours)
    {
      charBh.InvokeBehaviour();
    }
  }
}
