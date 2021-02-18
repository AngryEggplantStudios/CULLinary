using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class GenericCharacterBehaviour : MonoBehaviour
{
  private bool isInvoked = false;
  public bool GetIsInvoked()
  {
    return this.isInvoked;
  }

  public void SetIsInvoked(bool value)
  {
    this.isInvoked = value;
  }

  public abstract void InvokeBehaviour();
}
