using Assets.Script.Core.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    private BaseCharacter character;
    private new CapsuleCollider2D collider;
    private new Rigidbody2D rigidbody;
    public GameObject prefab;
    private PlayerInput playerInput;
    void Start()
    {
        collider = gameObject.AddComponent<CapsuleCollider2D>();
        rigidbody = gameObject.AddComponent<Rigidbody2D>();

        // resize capsule collider to fit sprite size

        // create an instance of prefab and add it to child of this gameobject
        GameObject instanceAnimated = Instantiate(prefab, transform.position, Quaternion.identity);
        instanceAnimated.transform.parent = transform;

        playerInput = gameObject.AddComponent<PlayerInput>();

        // get input action asset from resources
        InputActionAsset inputActionAsset = Resources.Load<InputActionAsset>("InputActionAssets/PlayerInputActions");

        inputActionAsset.FindActionMap("Player").FindAction("Move").performed += Move;

        playerInput.actions = inputActionAsset;
        // set behavior of player input
        playerInput.notificationBehavior = PlayerNotifications.InvokeUnityEvents;
        playerInput.actions.Enable();
        


    }

    void Update()
    {
        // lock rotation
        transform.rotation = Quaternion.identity;
    }

    void Move(InputAction.CallbackContext ctx)
    {
        rigidbody.velocity = ctx.ReadValue<Vector2>() * 10f;
    }

}
