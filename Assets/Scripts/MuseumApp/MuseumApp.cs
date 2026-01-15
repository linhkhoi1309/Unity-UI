using System;
using System.Collections.Generic;
using UITKUtils;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class MuseumApp : MonoBehaviour
{ 
    private const string ToggleMenuButtonName = "ToggleMenu";
    private const string ToggleAppThemeButtonName = "ToggleAppTheme";
    private const string MenuContainerElementName = "MenuContainer";
    private const string HeaderLabelName = "Header";
    private const string TagLineLabelName = "TagLine";
    private const string ArticleTitleLabelName = "ArticleTitle";
    private const string ArticleTagLabelName = "TagText";
    private const string ArticleSummaryLabelName = "ArticleSummary";
    private const string TilesTopContainerElementName = "tilesTopContainer";
    private const string TilesMiddleContainerElementName = "tilesMiddleContainer";
    private const string TileImageElementName = "TileImage";
    private const string TileTitleElementName = "TileTitle";
    private const string MenuHiddenClass = "menu-container-hidden";

    public ThemeStyleSheet lightTheme;
    public ThemeStyleSheet darkTheme;
    public MuseumPlaceholderData data;
    public VisualTreeAsset tileItemTemplate;
    
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

        if (data == null)
            Debug.LogError($"Missing reference to a {nameof(MuseumPlaceholderData)} asset");

        SetData();
    }

    private void SetData()
    {
        var header = m_Root.Q<Label>(HeaderLabelName);
        Validation.CheckQuery(header, HeaderLabelName);
        if (header != null) header.text = data.header;
        
        var tagLine = m_Root.Q<Label>(TagLineLabelName);
        Validation.CheckQuery(tagLine, TagLineLabelName);
        if (tagLine != null) tagLine.text = data.tagLine;

        int index = 0;
        m_Root.Query(classes: "article").ForEach(element =>
        {
            if (index >= data.articles.Count)
                return;
            var articleTitle = element.Q<Label>(ArticleTitleLabelName);
            Validation.CheckQuery(articleTitle, ArticleTitleLabelName);
            if (articleTitle != null) articleTitle.text = data.articles[index].title;
            
            var articleTag = element.Q<Label>(ArticleTagLabelName);
            Validation.CheckQuery(articleTag, ArticleTagLabelName);
            if (articleTag != null) articleTag.text = data.articles[index].tag;
            
            var articleSummary = element.Q<Label>(ArticleSummaryLabelName);
            Validation.CheckQuery(articleSummary, ArticleSummaryLabelName);
            if (articleSummary != null) articleSummary.text = data.articles[index].description;
            
            index++;
        });

        if (tileItemTemplate == null)
        {
            Debug.LogWarning($"Missing reference for {nameof(tileItemTemplate)}");
            return;
        }

        var tilesTopContainer = m_Root.Q(TilesTopContainerElementName);
        Validation.CheckQuery(tilesTopContainer, TilesTopContainerElementName);
        if (tilesTopContainer != null) FillTileList(data.tilesTop, tilesTopContainer);

        var tilesMiddleContainer = m_Root.Q(TilesMiddleContainerElementName);
        Validation.CheckQuery(tilesMiddleContainer, TilesMiddleContainerElementName);
        if (tilesMiddleContainer != null) FillTileList(data.tilesMiddle, tilesMiddleContainer);
    }

    void FillTileList(List<TileData> list, VisualElement container)
    {
        container.Clear();

        foreach (var tileData in list)
        {
            tileItemTemplate.CloneTree(container, out int firstElementIndex, out int _);
            var tileTitle = container[firstElementIndex].Q<Label>(TileTitleElementName);
            Validation.CheckQuery(tileTitle, TileTitleElementName);
            if (tileTitle != null) tileTitle.text = tileData.title;
            
            var tileImage = container[firstElementIndex].Q(TileImageElementName);
            Validation.CheckQuery(tileImage, TileImageElementName);
            if (tileImage != null) tileImage.style.backgroundImage = Background.FromSprite(tileData.icon);
        }
    }

    // This allows changes made to the data via the Inspector to refresh live.
    // Unlike when using UI Toolkit's Data Binding, in the event of data update from other sources,
    // this would need to be incremented manually
    private int m_LastVersion;
 
    public void Update()
    {
        if (data.version != m_LastVersion)
        {
            m_LastVersion = data.version;
            SetData();
        }
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
