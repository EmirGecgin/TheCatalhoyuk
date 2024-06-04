using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput _playerInput;
    [SerializeField] private Transform[] LaneTransforms;
    [SerializeField] private float moveSpeed = 20;
    [SerializeField] private float JumpHeight = 2.5f;
    [SerializeField][Range(0, 1)] float GroundCheckRadius = 0.2f;
    [SerializeField] LayerMask groundCheckMask;
    [SerializeField] Transform groundCheckedTransform;
    [SerializeField] Vector3 blockageCheckHalfExtend;
    [SerializeField] string blockageCheckTag = "Threat";

    [SerializeField] private float normalGravityMultiplier = 1.0f;
    [SerializeField] private float airGravityMultiplier = 3.0f;

    [SerializeField] private InGameUI playerUI;

    [Header("Audio")] 
    [SerializeField] private AudioClip JumpAudioClip;
    [SerializeField] private AudioClip MoveAudioClip;
    [SerializeField] private AudioSource ActionAudSrc;

    private Vector3 Destination;

    private int currentLaneIndex;

    [SerializeField] private Animator animator;

    private Camera playerCamera;
    private Vector3 playerCameraOffSet;
    private Rigidbody _rigidbody;

    private bool isTransitioning = false;
    private float transitionProgress = 0.0f;
    [SerializeField] private float transitionDuration = 0.5f;

    private void OnEnable()
    {
        if (_playerInput == null)
        {
            _playerInput = new PlayerInput();
        }
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    void Start()
    {
        _playerInput.gameplay.Move.performed += MovePerformed;
        _playerInput.gameplay.Jump.performed += JumpPerformed;
        _playerInput.gameplay.Pause.performed += togglePause;
        for (int i = 0; i < LaneTransforms.Length; i++)
        {
            if (LaneTransforms[i].position == transform.position)
            {
                currentLaneIndex = i;
                Destination = LaneTransforms[i].position;
            }
        }
        animator.GetComponent<Animator>();

        playerCamera = Camera.main;
        playerCameraOffSet = playerCamera.transform.position - transform.position;

        _rigidbody = GetComponent<Rigidbody>();
    }

    private void togglePause(InputAction.CallbackContext context)
    {

        GameMode gameMode = GameplayStatics.GetGameMode();
        if (gameMode != null && !gameMode.isGameOver())
        {
            gameMode.TogglePause();
            playerUI.SignalPause(gameMode.IsGamePaused());
        }
    }

    private void JumpPerformed(InputAction.CallbackContext context)
    {
        if (IsOnGround())
        {
            if (_rigidbody != null)
            {
                float jumpUpSpeed = Mathf.Sqrt(2 * JumpHeight * Physics.gravity.magnitude);
                _rigidbody.AddForce(new Vector3(0, jumpUpSpeed, 0), ForceMode.VelocityChange);
                ActionAudSrc.clip = JumpAudioClip;
                ActionAudSrc.Play();
            }
        }
    }

    private void MovePerformed(InputAction.CallbackContext obj)
    {
        float inputValue = obj.ReadValue<float>();
        Debug.Log($"Move action performed, with value {inputValue}");
        int goalIndex = currentLaneIndex;
        if (inputValue > 0)
        {
            if (goalIndex == LaneTransforms.Length - 1) return;
            goalIndex++;
        }
        else
        {
            if (currentLaneIndex == 0) return;
            goalIndex--;
        }

        Vector3 goalPos = LaneTransforms[goalIndex].position;
        if (GameplayStatics.IsPositionOccupied(goalPos, blockageCheckHalfExtend, blockageCheckTag))
        {
            return;
        }

        ActionAudSrc.clip = MoveAudioClip;
        ActionAudSrc.Play();
        
        currentLaneIndex = goalIndex;
        Destination = goalPos;

        // Başlangıç noktası ve geçişi başlat
        isTransitioning = true;
        transitionProgress = 0.0f;
    }

    void Update()
    {
        if (!IsOnGround())
        {
            animator.SetBool("isOnGround", false);
        }
        else
        {
            animator.SetBool("isOnGround", true);
        }
    }

    private void FixedUpdate()
    {
        if (!IsOnGround())
        {
            _rigidbody.AddForce(Vector3.down * Physics.gravity.magnitude * (airGravityMultiplier - normalGravityMultiplier), ForceMode.Acceleration);
        }

        if (isTransitioning)
        {
            transitionProgress += Time.fixedDeltaTime / transitionDuration;
            if (transitionProgress >= 1.0f)
            {
                transitionProgress = 1.0f;
                isTransitioning = false;
            }

            float transformX = Mathf.Lerp(transform.position.x, Destination.x, transitionProgress);
            transform.position = new Vector3(transformX, transform.position.y, transform.position.z);
        }
    }

    private void LateUpdate()
    {
        playerCamera.transform.position = transform.position + playerCameraOffSet;
    }

    bool IsOnGround()
    {
        return Physics.CheckSphere(groundCheckedTransform.position, GroundCheckRadius, groundCheckMask);
    }
}
