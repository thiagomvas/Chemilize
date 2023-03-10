using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ElementInfoChart : EditorWindow
{
    // UI Width Settings
    private int idWidth = 50;
    private int nameWidth = 250;
    private int buyCostWidth = 100;
    private int sellValueWidth = 100;
    private int recipesWidth = 400;

    Vector2 scrollPos;
    private List<ElementSO> elements = new List<ElementSO>();
    private List<RecipeSO> recipes = new List<RecipeSO>();
    private List<StructureSO> structures = new List<StructureSO>();
    private ReferencesSO references;
    [MenuItem("Chemilize/Elements Info")]
    public static void OpenWindow()
    {
        GetWindow<ElementInfoChart>("Elements Info Chart");
    }
    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Fetch Elements and Recipes"))
        {
            elements = SOUtils.FindAllOfType<ElementSO>("t:ElementSO", "Assets/Elements");
            elements = elements.OrderBy(x => x.id).ToList();
            recipes = SOUtils.FindAllOfType<RecipeSO>("t:RecipeSO", "Assets/Elements");
            structures = SOUtils.FindAllOfType<StructureSO>("t:StructureSO", "Assets/Elements");

            references = SOUtils.FindAllOfType<ReferencesSO>("t:ReferencesSO", "Assets/Elements")[0];
        }
        if (GUILayout.Button("Update IDs"))
        {
            elements = elements.OrderBy(x => x.id).ToList();

            for (int i = 0; i < elements.Count; i++)
            {
                elements[i].id = i;
            }

            if (Selection.activeGameObject.TryGetComponent<ReferencesManager>(out ReferencesManager rm)) rm.elements = elements;
        }
        if (GUILayout.Button($"Update References Manager"))
        {

            EditorUtility.SetDirty(references);
            structures = structures.OrderBy(x => x.id).ToList();

            for (int i = 0; i < structures.Count; i++)
            {
                EditorUtility.SetDirty(structures[i]);
                structures[i].id = i;
            }
            references.elements = elements;
            references.structures = structures;
            references.recipes = recipes;
            SaveChanges();
            

        }

        if (GUILayout.Button("Save")) SaveChanges();

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        GUILayout.Label($"ID", GUILayout.Width(idWidth));
        GUILayout.Label($"Name", GUILayout.Width(nameWidth));
        GUILayout.Label("Buy Cost", GUILayout.Width(buyCostWidth));
        GUILayout.Label("Sell Value", GUILayout.Width(sellValueWidth));
        GUILayout.Label("Recipes", GUILayout.Width(recipesWidth));

        GUILayout.EndHorizontal();


        GUILayout.Space(10);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        
        foreach(ElementSO e in elements)
        {
            EditorUtility.SetDirty(e);
            GUILayout.BeginHorizontal();

            GUILayout.Label($"{e.id}", GUILayout.Width(idWidth));
            GUILayout.Label($"{e.name}", GUILayout.Width(nameWidth));
            e.buyCost = EditorGUILayout.IntField(e.buyCost, GUILayout.Width(buyCostWidth));
            e.sellValue = EditorGUILayout.IntField(e.sellValue, GUILayout.Width(sellValueWidth));

            bool hasRecipe = false;
            string ingredients = "Ingredients: ";
            RecipeSO rec = null;
            foreach(RecipeSO recipe in recipes)
            {
                for (int i = 0; i < recipe.outputs.Length; i++) 
                    if (recipe.outputs[i].element.id == e.id)
                    {
                        for(int j = 0; j < recipe.items.Length; j++)
                        {
                            ingredients += $"{recipe.items[j].amount}x {recipe.items[j].element.name}, ";
                        }
                        rec = recipe;
                        hasRecipe = true;
                    }
                if (hasRecipe) break;
            }

            if (hasRecipe) GUILayout.Label(ingredients, GUILayout.Width(recipesWidth)) ;
            else GUILayout.Label("No recipe detected!");
            if(rec != null && rec.madeInMachine != null) GUILayout.Label($"Made in {rec.madeInMachine.name}") ;


            GUILayout.EndHorizontal();
        
        }
        EditorGUILayout.EndScrollView();

        GUILayout.Label($"Loaded {elements.Count}x elements, {recipes.Count}x recipes");
    }
    public override void SaveChanges()
    {
        AssetDatabase.SaveAssets();

        Debug.Log($"{this} saved successfully!!!");
        base.SaveChanges();
    }
}

    public static class SOUtils
    {

        public static List<T> FindAllOfType<T>(string filter, string folder = "Assets")
            where T : ScriptableObject
        {
            return AssetDatabase.FindAssets(filter, new[] { folder })
                .Select(guid => AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToList();
        }
    }
