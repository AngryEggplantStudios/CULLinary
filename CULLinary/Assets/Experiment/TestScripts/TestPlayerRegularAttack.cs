using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerRegularAttack : GenericCharacterBehaviour
{
  public GenericCharacterBehaviour[] suppressedBh;
  private Animator animator;
  override public void InvokeBehaviour()
  {
    foreach (GenericCharacterBehaviour charBh in suppressedBh)
    {
      if (charBh.GetIsInvoked())
      {
        return;
      }
    }
    if (Input.GetKey(KeyCode.F))
    {
      this.SetIsInvoked(true);
      animator.SetBool("isPunch", true);
    }
    else
    {
      this.SetIsInvoked(false);
      animator.SetBool("isPunch", false);
    }
  }

  void Start()
  {
    animator = GetComponentInChildren<Animator>();
  }

}
