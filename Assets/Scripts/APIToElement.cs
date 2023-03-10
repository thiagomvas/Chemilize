using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Globalization;

public class APIToElement : MonoBehaviour
{
 //[SerializeField] private bool FetchData;
 //[SerializeField] private int AtomicNumber;
 //// Start is called before the first frame update
 //
 //private void Update()
 //{
 //    if(FetchData)
 //    {
 //        FetchData = false;
 //        StartCoroutine(GetData(AtomicNumber));
 //
 //    }
 //}
 //
 //IEnumerator GetData(int atomicNumber)
 //{
 //
 //    Debug.Log("Processing");
 //    WWW _www = new WWW($"https://neelpatel05.pythonanywhere.com/element/atomicnumber?atomicnumber={atomicNumber}");
 //    yield return _www;
 //    if (_www.error == null)
 //    {
 //        ProcessData(_www.text);
 //    }
 //    else Debug.Log("Something went wrong");
 //}
 //
 //void ProcessData(string data)
 //{
 //    JsonElementClass j = JsonUtility.FromJson<JsonElementClass>(data);
 //
 //    ElementSO element = ScriptableObject.CreateInstance<ElementSO>();
 //    string path = $"Assets/Elements/{j.atomicNumber}_{j.name}.asset";
 //
 //    string[] mass = j.atomicMass.Split("(", System.StringSplitOptions.RemoveEmptyEntries);
 //
 //    NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
 //    if(ColorUtility.TryParseHtmlString("#" + j.cpkHexColor, out Color c)) element.cpkHexColor  = c;
 //
 //    //Assign data
 //    element.symbol                     = j.symbol;
 //    element.name                       = j.name;
 //    element.atomicMass                 = float.Parse(mass[0], nfi);
 //    element.atomicNumber               = int.Parse(j.atomicNumber);
 //    element.boilingPoint               = int.Parse(j.boilingPoint);
 //
 //
 //    element.density                    = j.density;
 //    element.electronicConfiguration    = j.electronicConfiguration;
 //    element.groupBlock                 = j.groupBlock;
 //    element.ionizationEnergy           = int.Parse(j.ionizationEnergy);
 //    element.meltingPoint               = j.meltingPoint;
 //
 //    element.standardState              = (ElementSO.PhysicalState) System.Enum.Parse(typeof(ElementSO.PhysicalState), j.standardState);
 //
 //    element.yearDiscovered             = j.yearDiscovered;
 //
 //    AssetDatabase.CreateAsset(element, path);
 //    AssetDatabase.SaveAssets();
 //    AssetDatabase.Refresh();
 //    EditorUtility.FocusProjectWindow();
 //    Selection.activeObject = element;
 //}
 //
}

[System.Serializable]
public class JsonElementClass
{
    public string symbol;
    public string name;
    public string atomicMass;
    public string atomicNumber;
    public string atomicRadius;
    public string boilingPoint;
    public string bondingType;
    public string cpkHexColor;
    public float density;
    public string electronAffinity;
    public int electronegativity;
    public string electronicConfiguration;
    public string groupBlock;
    public string ionRadius;
    public string ionizationEnergy;
    public int meltingPoint;
    public string oxidationStates;
    public string standardState;
    public string vanDerWaalsRadius;
    public string yearDiscovered;

}
