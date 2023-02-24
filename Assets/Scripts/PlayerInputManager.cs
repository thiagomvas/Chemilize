using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance { get; private set; }
    [SerializeField] private BuildingMenu bm;
    [SerializeField] private BuildingTool tool;
    [SerializeField] private UIManager um;
    PlayerControls controls;
    MouseLook ml;
    [SerializeField] private TextMeshProUGUI tmpro;
    [SerializeField] private GameObject guideMenu;
    bool gamePaused = false;

    public bool BuildingToolOn;
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject); else Instance = this;
        controls = new PlayerControls();
    }
    private void Start()
    {
        ml = tool.GetComponent<MouseLook>();
    }
    private void Update()
    {
        tmpro.text = $"Cash: ${SaveData.Current.currency}";
    }



    private void OnOpenMenu(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
    }

    private void OnQuickSave(InputAction.CallbackContext c)
    {
        if (!c.performed) return;
        SerializationManager.SaveGame($"save", SaveData.Current);
        Debug.Log("Quick Save!");
    }

    private void OnBuildToolToggle(InputAction.CallbackContext c)
    {
        if (!c.performed) return;
        BuildingToolOn = !BuildingToolOn;
        tool.enabled = BuildingToolOn;
        tool.hammer.SetActive(BuildingToolOn);
    }

    private void OnGuideMenuToggle(InputAction.CallbackContext c)
    {
        if (!c.performed) return;
        guideMenu.SetActive(!guideMenu.activeSelf);
        Cursor.lockState = guideMenu.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
        ml.enabled = !guideMenu.activeSelf;
    }

    private void OnPauseMenu(InputAction.CallbackContext c)
    {
        if (!c.performed) return;
        gamePaused = !gamePaused;
        UIManager.Instance.ToggleMenu(UIManager.Instance.pauseMenu);
        Cursor.lockState = gamePaused ? CursorLockMode.None : CursorLockMode.Locked;
        Time.timeScale = gamePaused ? 0f : 1f ;
        ml.UpdateSettings();

    }

    private void OnEnable()
    {
        controls.Enable();
        controls.Player.OpenBuildMenu.performed += OnOpenMenu;
        controls.Player.QuickSave.performed += OnQuickSave;
        controls.Player.BuildingTool.performed += OnBuildToolToggle;
        controls.Player.GuideMenu.performed += OnGuideMenuToggle;
        controls.Player.Pause.performed += OnPauseMenu;
    }

    private void OnApplicationQuit()
    {
        SerializationManager.SaveGame("save", SaveData.Current);
    }
    private void OnDisable() => controls.Disable();
}
