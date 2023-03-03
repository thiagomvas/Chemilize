using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }


    [SerializeField] private GameObject postProcess;
    public bool openMenu;
    GameObject activeMenu;
    [Header("Menus")]
    public GameObject pauseMenu;
    public GameObject computerMenu;
    [Header("References")]
    [SerializeField] private TextMeshProUGUI sensText;
    [SerializeField] private Slider sensitivitySlider;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this); else Instance = this;
    }
    private void Start()
    {
        SerializationManager.LoadSettings("settings.save");
        sensitivitySlider.value = SettingsData.Current.mouseSensitivity;
        ChangeSensitivity();
    }

    public void ToggleMenu(GameObject menu)
    {
        if(!openMenu)
        {
            activeMenu = menu;
            activeMenu.SetActive(true);
            openMenu = true;
            postProcess.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            ReferencesManager.Instance.mouseLook.enabled = false;
            ReferencesManager.Instance.movement.enabled = false;
            ReferencesManager.Instance.interaction.enabled = false;
        }
        else
        {
            activeMenu.SetActive(false);
            openMenu = false;
            postProcess.SetActive(false);
            activeMenu = null;
            Cursor.lockState = CursorLockMode.Locked;
            ReferencesManager.Instance.mouseLook.enabled = true;
            ReferencesManager.Instance.movement.enabled = true;
            ReferencesManager.Instance.interaction.enabled = true;
        }
        
    }

    public void ChangeSensitivity()
    {
        sensText.text = $"Sensitivity: {SettingsData.Current.mouseSensitivity}";
        SettingsData.Current.mouseSensitivity = Mathf.FloorToInt(sensitivitySlider.value);
        
        SerializationManager.SaveSettings("settings", SettingsData.Current);
        ReferencesManager.Instance.mouseLook.UpdateSettings();
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
        SerializationManager.SaveGame("save", SaveData.Current);
    }

}
