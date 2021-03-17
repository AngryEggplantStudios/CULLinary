using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A tiny script for an object to keep facing the player.
 */
public class FacePlayer : MonoBehaviour {
    public Transform itemTransform;
    public Transform player;

    void Update() {
        transform.LookAt(player);
    }
}
