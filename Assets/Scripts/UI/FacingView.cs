using UnityEngine;
using UnityEngine.UI;

public class FacingView : MonoBehaviour
{
    public Image[] FacingGraphics;

    private FacingModel facingModel;
    private Image currentFacingGraphic;

    public void ChangeModel(FacingModel newFacingModel)
    {
        if (facingModel != null) facingModel.TargetFacingChanged -= UpdateFacing;
        if (newFacingModel == null) return;
        facingModel = newFacingModel;
        facingModel.TargetFacingChanged += UpdateFacing;
        if(gameObject.name == "TargetStatus") Debug.Log("target facing view's model id: "+facingModel.id);

        UpdateFacing(facingModel.CurrentFacing);
    }

    private void UpdateFacing(Facing newFacing)
    {
        if(gameObject.name == "TargetStatus") Debug.Log(newFacing);
        if (currentFacingGraphic != null) currentFacingGraphic.enabled = false;
        if (newFacing.Index == -1) return;
        currentFacingGraphic = FacingGraphics[newFacing.Index];
        currentFacingGraphic.enabled = true;
    }

    private void Awake() 
    {
        foreach (Image i in FacingGraphics)
            i.enabled = false;
    }
}
