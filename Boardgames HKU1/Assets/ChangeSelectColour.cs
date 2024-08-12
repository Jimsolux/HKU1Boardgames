using UnityEngine;

public class ChangeSelectColour : MonoBehaviour
{
    [SerializeField] private Renderer material1;

    private Color activeDefaultColor;
    [SerializeField] private Color notInlineColor;
    [SerializeField] private Color inLineColor;
    [SerializeField] private Color selectedColor;

    private enum currentColor
    {
        defaultc,
        selected
    }
    private currentColor instance;

    private void Awake()
    {
        material1 = GetComponent<Renderer>();
        //ChangeToDefaultColor();
        ChangeInLineColor(false);
    }

    public void ChangeToDefaultColor() // switches to active defaultcolor
    {
        material1 = GetComponent<Renderer>();
        instance = currentColor.defaultc;
        material1.material.color = activeDefaultColor;
    }

    public void ChangeToSelectedColor()
    {
        material1 = GetComponent<Renderer>();
        instance = currentColor.selected;
        material1.material.color = selectedColor;
    }



    public void ChangeInLineColor(bool value)
    {
        SetActiveDefaultColor(value);// First, sets the right color to be the defaultcolor
        if (instance == currentColor.defaultc)  // if its already defaultcolor, changetodefaultcolor.
        {
            ChangeToDefaultColor();
        }
    }

    private void SetActiveDefaultColor(bool value)
    {
        if (value) activeDefaultColor = inLineColor;
        else if (!value) activeDefaultColor = notInlineColor;
    }

}
