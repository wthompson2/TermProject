using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpInfo : MonoBehaviour
{
    public float speed = 0.0f; // the speed (i.e., velocity) at which the player is currently moving
    public Vector3 direction = Vector3.zero; // a vector describing the direction the player is currently moving
    public Vector3 normalizedDirection = Vector3.zero; // a normalized vector describing the direction the player is currently moving
    public Vector3 distance = Vector3.zero; // the distance the player should move at this time

    public float baseSpeed; // the base walking speed of the animation
}
