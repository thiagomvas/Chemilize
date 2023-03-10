using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SaveManager : MonoBehaviour
{
    [SerializeField] private GameObject loadBtn;
    [SerializeField] private bool loadSaveOnStart;

    private void Awake()
    {
        if (loadSaveOnStart) SerializationManager.LoadGame("save.save");        
    }
    private void Start()
    {
       if(SerializationManager.CheckFile("save.save") && loadBtn != null)
        {
            loadBtn.SetActive(true);
        }
    }



    public void NewSave()
    {
        SerializationManager.DeleteSave("save.save");
        SceneManager.LoadScene(1);
    }

    public void LoadSave()
    {
        SerializationManager.LoadGame("save.save");
        SceneManager.LoadScene(1);
    }
    
}
