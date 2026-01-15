using System;
using System.Collections.Generic;
using UITKUtils;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class MuseumApp_DataBindings : MonoBehaviour
{ 
    private const string ToggleMenuButtonName = "ToggleMenu";
    private const string ToggleAppThemeButtonName = "ToggleAppTheme";
    private const string MenuContainerElementName = "MenuContainer";
    private const string MenuHiddenClass = "menu-container-hidden";

    private const string PopupContainerName = "PopupContainer";
    private const string PopupBackgroundName = "PopupBackground";

    private const string PopupContainerHiddenClassName = "popup-container-hidden";
    private const string PopupBackgroundHiddenClassName = "popup-background-hidden";

    private const string TileClassName = "tile";

    private VisualElement m_Popup;
    private VisualElement m_Background;
    private List<VisualElement> m_Tiles;

    public ThemeStyleSheet lightTheme;
    public ThemeStyleSheet darkTheme;

    VisualElement m_Root;
    ScrollView m_MuseumScrollView;
    Button m_ToggleMenuButton;
    Button m_ToggleThemeMenuButton;
    VisualElement m_Menu;

    [NonSerialized] private UIDocument m_UIDocument;
    [NonSerialized] private ThemeStyleSheet m_InitialTheme;

    void Awake()
    {
        m_UIDocument = GetComponent<UIDocument>();
        m_Root = m_UIDocument.rootVisualElement;
        m_InitialTheme = m_UIDocument.panelSettings?.themeStyleSheet;

    }

    private void OnDisable()
    {
        m_UIDocument.panelSettings.themeStyleSheet = m_InitialTheme;
    }

    void OnEnable()
    {
        m_ToggleMenuButton = m_Root.Q<Button>(ToggleMenuButtonName);
        Validation.CheckQuery(m_ToggleMenuButton, ToggleMenuButtonName);
        m_ToggleMenuButton?.RegisterCallback<ClickEvent>(ToggleMenu);

        m_Menu = m_Root.Q<VisualElement>(MenuContainerElementName);
        Validation.CheckQuery(m_Menu, MenuContainerElementName);
        m_Menu?.AddToClassList(MenuHiddenClass);

        m_ToggleThemeMenuButton = m_Menu?.Q<Button>(ToggleAppThemeButtonName);
        Validation.CheckQuery(m_ToggleThemeMenuButton, ToggleAppThemeButtonName);

        m_Popup = m_Root.Q<VisualElement>(PopupContainerName);
        m_Background = m_Root.Q<VisualElement>(PopupBackgroundName);
        m_Background.RegisterCallback<ClickEvent>(OnBackgroundClick);

        m_Tiles = m_Root.Query<VisualElement>(className: TileClassName).ToList();
        RegisterTileCallbacks();

    }

    private void RegisterTileCallbacks(){
        for (int i = 0; i < m_Tiles.Count; i++)
        {
            m_Tiles[i].RegisterCallback<ClickEvent>(OnTileClick);
        }
    }

    private void OnTileClick(ClickEvent evt)
    {
        if (!m_Popup.ClassListContains(PopupContainerHiddenClassName)){
            return;
        }

        m_Popup.RemoveFromClassList(PopupContainerHiddenClassName);
        m_Background.RemoveFromClassList(PopupBackgroundHiddenClassName);

    }


    private void OnBackgroundClick(ClickEvent evt)
    {
        if (m_Popup.ClassListContains(PopupContainerHiddenClassName)){
            return;
        }

        m_Popup.AddToClassList(PopupContainerHiddenClassName);
        m_Background.AddToClassList(PopupBackgroundHiddenClassName);
    }

    void ToggleMenu(ClickEvent evt)
    {
        if (m_Menu.ClassListContains(MenuHiddenClass))
        {
            m_ToggleThemeMenuButton?.RegisterCallback<ClickEvent>(ToggleTheme);
            m_Menu.RemoveFromClassList(MenuHiddenClass);
        }
        else
        {
            m_ToggleThemeMenuButton?.UnregisterCallback<ClickEvent>(ToggleTheme);
            m_Menu.AddToClassList(MenuHiddenClass);
        }
    }



    private void ToggleTheme(ClickEvent evt)
    {
        if (darkTheme == null)
            Debug.LogWarning($"Missing {nameof(darkTheme)} Reference");
        
        if (lightTheme == null)
            Debug.LogWarning($"Missing {nameof(lightTheme)} Reference");
              
        if (darkTheme == null || lightTheme == null)
            return;

        PanelSettings settings = m_UIDocument.panelSettings; 
        settings.themeStyleSheet = settings.themeStyleSheet == darkTheme ? lightTheme : darkTheme;
    }
}
