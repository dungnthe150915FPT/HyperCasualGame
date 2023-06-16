//using Assets.Script.Core.Character;
//using Assets.Script.Core.Library;
//using UnityEditor.Animations;
//using UnityEngine;
//using UnityEngine.InputSystem;
//using Cinemachine;
//using System;

//namespace Assets.Script.Core.Character
//{
//    public class CharacterController2 : MonoBehaviour
//    {
//        private BaseCharacter character = new BaseCharacter();
//        private new CapsuleCollider2D collider;
//        private CapsuleCollider2D triggerCollider;
//        private new Rigidbody2D rigidbody;
//        private GameObject prefab;
//        private PlayerInput playerInput;

//        private Animator animator;

//        private Vector2 previousVelocity;

//        private CinemachineVirtualCamera virtualCamera;

//        private void Start()
//        {
//            // Rigidbody2D
//            setupRigidBody();

//            // Collider2D and Trigger
//            setupColliderAndTrigger();

//            // Virtual Camera
//            setupVirtualCamera();

//            // Animated
//            setupAnimated();

//            // Input Action
//            setupInputAction();
//        }

//        void Update()
//        {
//            // lock rotation
//            transform.rotation = Quaternion.identity;
//            onFaceSide();

//            // log to console state of character
//            //Debug.Log("Character State: " + character.CharacterState);

//        }

//        private void FixedUpdate()
//        {
//            Vector2 accelebration = rigidbody.velocity - previousVelocity;

//            if (accelebration.magnitude > 0.1f)
//            {
//                character.IsAccelerating = true;
//                animator.SetBool(CONST.ANIMATOR_CONTROLLER_PARAMETER_IS_ACCELERATING, true);
//            }
//            else if (accelebration.magnitude < 0.1f)
//            {
//                character.IsAccelerating = false;
//                animator.SetBool(CONST.ANIMATOR_CONTROLLER_PARAMETER_IS_ACCELERATING, false);
//            }

//            // check gameobject is falling or not
//            if (rigidbody.velocity.y < -0.5f && !character.IsInGround)
//            {
//                character.CharacterState = CharacterState.Falling;
//                AnimatedLibrary.SetParameter(character.CharacterState, animator);
//            }

//            if (character.IsInGround && !character.IsAccelerating)
//            {
//                character.CharacterState = CharacterState.Idle;
//                AnimatedLibrary.SetParameter(CharacterState.Idle, animator);
//            }
//            if (character.IsInGround && Input.GetKey(KeyCode.LeftShift))
//            {
//                if (rigidbody.velocity.x < 0)
//                {
//                    rigidbody.velocity = new Vector2(-1, 0) * 20f;
//                }
//                else if (rigidbody.velocity.x > 0)
//                {
//                    rigidbody.velocity = new Vector2(1, 0) * 20f;
//                }
//            }
//            //// if in ground, is accelerating, set state to walking or running
//            //else if (character.IsInGround && character.IsAccelerating && character.CharacterState != CharacterState.Running)
//            //{
//            //    character.CharacterState = CharacterState.Walking;
//            //    AnimatedLibrary.SetParameter(CharacterState.Walking, animator);
//            //}
//            //else if (character.IsInGround && character.IsAccelerating && character.CharacterState != CharacterState.Walking)
//            //{
//            //    character.CharacterState = CharacterState.Running;
//            //    AnimatedLibrary.SetParameter(CharacterState.Running, animator);
//            //}
//        }

//        void onFaceSide()
//        {
//            if (rigidbody.velocity.x < 0)
//            {
//                // flip sprite to left
//                transform.localScale = new Vector3(-1, 1, 1);
//            }
//            else if (rigidbody.velocity.x > 0)
//            {
//                // flip sprite to right
//                transform.localScale = new Vector3(1, 1, 1);
//            }

//        }

//        void Jump(InputAction.CallbackContext ctx)
//        {
//            // check if character is in ground, if yes, set state to jumping
//            if (character.IsInGround)
//            {
//                character.CharacterState = CharacterState.Jumping;
//                AnimatedLibrary.SetParameter(character.CharacterState, animator);
//                rigidbody.AddForce(new Vector2(0, 10f), ForceMode2D.Impulse);
//                character.IsInGround = false;
//                animator.SetBool(CONST.ANIMATOR_CONTROLLER_PARAMETER_IS_IN_GROUND, false);
//            }
//        }




//        private void setupAnimated()
//        {
//            //load prefab from resources
//            prefab = Resources.Load<GameObject>(CONST.PREFAB_ANIMATED_PATH);

//            // create an instance of prefab and add it to child of this gameobject
//            GameObject instanceAnimated = Instantiate(prefab, transform.position, Quaternion.identity);

//            instanceAnimated.transform.position =
//                new Vector3(transform.position.x,
//                transform.position.y + CONST.INSTANCED_PREFAB_ANIMATION_POSITION.Y,
//                transform.position.z);
//            instanceAnimated.transform.parent = transform;

//            // load animator controller from resources
//            AnimatorController animatorController = Resources.Load<AnimatorController>(CONST.ANIMATOR_CONTROLLER_PATH);

//            animator = instanceAnimated.GetComponent<Animator>();
//            // set animator controller to animator
//            animator.runtimeAnimatorController = animatorController;

//            // play idle animation
//            animator.Play("idle", 0, 0f);
//            character.CharacterState = CharacterState.Idle;
//            AnimatedLibrary.SetParameter(character.CharacterState, animator);
//        }


//        private void setupVirtualCamera()
//        {

//            // find virtualCamera in hierarchy
//            virtualCamera = GameObject.Find(CONST.VIRTUAL_CAMERA_NAME).GetComponent<CinemachineVirtualCamera>();
//            if (virtualCamera == null) Debug.Log("Virtual Camera not found. Creating new one.");

//            virtualCamera.Follow = gameObject.transform;
//            // set mode of virtual camera to orthographic
//            virtualCamera.m_Lens.Orthographic = true;
//            // set size orthographic of virtual camera
//            virtualCamera.m_Lens.OrthographicSize = 10f;



//            //virtualCamera.m_Lens.FieldOfView = 80;
//            //virtualCamera.m_Lens.OrthographicSize = 3f;
//            // body of virtual camera is Framing Transposer
//            CinemachineFramingTransposer framingTrans = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
//            framingTrans.m_TrackedObjectOffset = new Vector3(5, 2, 0);
//            framingTrans.m_XDamping = 0;
//            framingTrans.m_YDamping = 0;
//            framingTrans.m_ZDamping = 0;
//        }


//        private void setupColliderAndTrigger()
//        {
//            // add trigger collider
//            collider = gameObject.AddComponent<CapsuleCollider2D>();
//            collider.size = new Vector2(0.5f, 1.5f);
//            triggerCollider = gameObject.AddComponent<CapsuleCollider2D>();
//            triggerCollider.isTrigger = true;
//            triggerCollider.size = new Vector2(0.7f, 1.7f);
//        }


//        private void setupRigidBody()
//        {
//            rigidbody = gameObject.AddComponent<Rigidbody2D>();
//            rigidbody.mass = 1;
//            rigidbody.angularDrag = 0;
//        }

//        // event enter trigger collider 2d 
//        private void OnTriggerEnter2D(Collider2D collision)
//        {
//            if (collision.gameObject.tag == CONST.TAG_FLOOR)
//            {
//                // log to console with delta time
//                //Debug.Log("OnTriggerEnter2D: " + Time.deltaTime);
//                animator.SetBool(CONST.ANIMATOR_CONTROLLER_PARAMETER_IS_FALLING, false);
//                character.IsInGround = true;
//                animator.SetBool(CONST.ANIMATOR_CONTROLLER_PARAMETER_IS_IN_GROUND, true);
//            }
//        }

//        // event exit trigger collider 2d
//        private void OnTriggerExit2D(Collider2D collision)
//        {
//            if (collision.gameObject.tag == CONST.TAG_FLOOR)
//            {
//                // log to console with delta time
//                //Debug.Log("OnTriggerExit2D: " + Time.deltaTime);
//                if (rigidbody.velocity.y > 0.01)
//                {
//                    character.CharacterState = CharacterState.Jumping;
//                    character.IsInGround = false;
//                    animator.SetBool(CONST.ANIMATOR_CONTROLLER_PARAMETER_IS_IN_GROUND, false);
//                    AnimatedLibrary.SetParameter(character.CharacterState, animator);
//                }
//            }
//        }

//        private void setupInputAction()
//        {
//            playerInput = gameObject.AddComponent<PlayerInput>();
//            playerInput.notificationBehavior = PlayerNotifications.InvokeUnityEvents;
//            // get input action asset from resources
//            InputActionAsset inputActionAsset = Resources.Load<InputActionAsset>(CONST.PLAYER_INPUT_ACTIONS_PATH);
//            InputActionMap actionMapPlayer = inputActionAsset.FindActionMap(CONST.ACTIONMAP_PLAYER);

//            actionMapPlayer.FindAction(CONST.ACTION_MOVE).started += MoveStarted;
//            actionMapPlayer.FindAction(CONST.ACTION_MOVE).canceled += MoveCanceled;

//            actionMapPlayer.FindAction(CONST.ACTION_JUMP).performed += Jump;

//            actionMapPlayer.FindAction(CONST.ACTION_RUN).started += RunStarted;
//            actionMapPlayer.FindAction(CONST.ACTION_RUN).canceled += RunCanceled;

//            playerInput.actions = inputActionAsset;
//            playerInput.actions.Enable();

//            previousVelocity = rigidbody.velocity;
//        }

//        private void RunCanceled(InputAction.CallbackContext context)
//        {
//            throw new NotImplementedException();
//        }

//        private void RunStarted(InputAction.CallbackContext context)
//        {
//            throw new NotImplementedException();
//        }

//        private void MoveCanceled(InputAction.CallbackContext context)
//        {
//            throw new NotImplementedException();
//        }

//        private void MoveStarted(InputAction.CallbackContext context)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
