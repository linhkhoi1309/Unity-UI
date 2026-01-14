using UnityEngine;
using UnityEngine.UIElements;

public class TabMenu : MonoBehaviour
{
    const string tabClassname = "tab";
    const string currentlySelectedTabClassName = "selectedTab";
    const string unselectedContentClassName = "unselectedTabContent";
    const string contentNameSuffix = "Content";
    VisualElement root;

    private void OnEnable() {
        UIDocument menu = GetComponent<UIDocument>();
        root = menu.rootVisualElement;
        RegisterTabCallbacks();
    }

    public void RegisterTabCallbacks()
    {
        UQueryBuilder<Label> tabs = GetAllTabs();
        tabs.ForEach((Label tab) =>
        {
            tab.RegisterCallback<ClickEvent>(TabOnClick);
        });
    }

    void TabOnClick(ClickEvent evt)
    {
        Label clickedTab = evt.currentTarget as Label;
        if(!TabIsCurrentlySelected(clickedTab))
        {
            GetAllTabs().Where((tab) => tab != clickedTab && TabIsCurrentlySelected(tab)).ForEach(UnselectTab);
            SelectTab(clickedTab);
        }
    }

    static bool TabIsCurrentlySelected(Label tab)
    {
        return tab.ClassListContains(currentlySelectedTabClassName);
    }

    UQueryBuilder<Label> GetAllTabs()
    {
        return root.Query<Label>(className: tabClassname);
    }

    void SelectTab(Label tab)
    {
        tab.AddToClassList(currentlySelectedTabClassName);
        VisualElement content = FindContent(tab);
        content.RemoveFromClassList(unselectedContentClassName);
    }

    void UnselectTab(Label tab)
    {
        tab.RemoveFromClassList(currentlySelectedTabClassName);
        VisualElement content = FindContent(tab);
        content.AddToClassList(unselectedContentClassName);
    }

    static string GenerateContentName(Label tab) => tab.name+contentNameSuffix;

    VisualElement FindContent(Label tab)
    {
        return root.Q(GenerateContentName(tab));
    }
}
