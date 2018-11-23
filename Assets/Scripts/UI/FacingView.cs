using UnityEngine;

public class FacingView : MonoBehaviour
{
    private FacingModel facingModel;

    public void ChangeModel(FacingModel newFacingModel)
    {
        if (facingModel != null) facingModel.TargetFacingChanged -= UpdateFacing;
        facingModel = newFacingModel;
        facingModel.TargetFacingChanged += UpdateFacing;
    }

    public void UpdateFacing(Facing newFacing)
    {
        Debug.Log("facing changed");
    }
}
