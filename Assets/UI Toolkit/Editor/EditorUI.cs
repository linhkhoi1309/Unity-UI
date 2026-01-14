using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class EditorUI : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("Window/UI Toolkit/EditorUI")]
    public static void ShowExample()
    {
        EditorUI wnd = GetWindow<EditorUI>();
        wnd.titleContent = new GUIContent("EditorUI");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        VisualElement label = new Label("List of options");
        root.Add(label);
        
        VisualElement cloudyOption = new VisualElement();
        Toggle cloudyToggle = new Toggle("Cloudy");
        cloudyOption.Add(cloudyToggle);
        root.Add(cloudyOption);

        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);
    }
}
