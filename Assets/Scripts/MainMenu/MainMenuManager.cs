using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Live2D.Cubism.Core;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private const string BUTTONS = "Buttons";
    [SerializeField] private CubismModel menuModel;
    [SerializeField] private Vector2 background;
    [SerializeField] private GameObject Flashlight;
    [SerializeField] private MainMenuButtons buttons;
    [SerializeField] private LoadingCanvas loadingCanvas;

    private Animator menuAnimator;
    private MenuControls menuControls;
    private bool inButtons;

    private void Awake()
    {
        menuControls = new MenuControls();

        menuControls.Menu.Select.performed += SelectPerformed;
    }

    private void Start()
    {
        menuAnimator = menuModel.gameObject.GetComponent<Animator>();
        inButtons = false;
    }

    private void OnEnable()
    {
        menuControls.Enable();
    }

    private void OnDisable()
    {
        menuControls.Disable();
    }

    private void SelectPerformed(InputAction.CallbackContext obj) {
        if (inButtons && loadingCanvas)
        {
            loadingCanvas.SetupLoadScene(Loader.SceneName.Level1);
        }
    }

    private void LateUpdate()
    {
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        Vector2 backgroundSize = new Vector2(background.x, background.y);
        Vector2 mousePosition = GetMousePosition();
        menuModel.Parameters[0].Value = mousePosition.x / screenSize.x * 200 - 100;
        menuModel.Parameters[1].Value = mousePosition.y / screenSize.y * 200 - 100;
        float flx = mousePosition.x / screenSize.x * backgroundSize.x * 2f - backgroundSize.x;
        float fly = mousePosition.y / screenSize.y * backgroundSize.y * 2f - backgroundSize.y;
        Flashlight.transform.position = new Vector2(flx, fly);
    }

    public void TurnOnObject(string objectName)
    {
        if (objectName.CompareTo(BUTTONS) != 0)
        {
            menuAnimator.SetBool(objectName, true);
        } else inButtons = true;
    }

    public void TurnOffObject(string objectName) 
    {
        if (objectName.CompareTo(BUTTONS) != 0)
        {
            menuAnimator.SetBool(objectName, false);
        } else inButtons = false;
    }

    public Vector2 GetMousePosition()
    {
        return menuControls.Menu.Position.ReadValue<Vector2>();
    }
}
