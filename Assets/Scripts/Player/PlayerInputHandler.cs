using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private InputActionAsset playerControls;

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction sprintAction;
    private InputAction attackAction;

    public Vector2 MoveInput{
        get;
        private set;
    }
    public Vector2 LookInput{
        get;
        private set;
    }
    public float SprintValue{
        get;
        private set;
    }
    public bool AttackInput{
        get;
        private set;
    }

    public static PlayerInputHandler Instance{
        get;
        private set;
    }
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }

        moveAction = playerControls.FindActionMap("PlayerMap").FindAction("Move");
        lookAction = playerControls.FindActionMap("PlayerMap").FindAction("Look");
        sprintAction = playerControls.FindActionMap("PlayerMap").FindAction("Sprint");
        attackAction = playerControls.FindActionMap("PlayerMap").FindAction("Attack");
        RegisterInputActions();
    }   
    void RegisterInputActions(){
        moveAction.performed += context => MoveInput = context.ReadValue<Vector2>();
        moveAction.canceled += context => MoveInput = Vector2.zero;

        lookAction.performed += context => LookInput = context.ReadValue<Vector2>();
        lookAction.canceled += context => LookInput = Vector2.zero;

        sprintAction.performed += context => SprintValue = context.ReadValue<float>();
        sprintAction.canceled += context => SprintValue = 0f;

        attackAction.performed += context => AttackInput = true;
        attackAction.canceled += context => AttackInput = false;
    }
    private void OnEnable() {
        moveAction.Enable();
        lookAction.Enable();
        sprintAction.Enable();
        attackAction.Enable();
    }
    private void OnDisable(){
        moveAction.Disable();
        lookAction.Disable();
        sprintAction.Disable();
        attackAction.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
