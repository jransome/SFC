using System;

public class FacingModel
{
    private Facing currentFacing;

    public event Action<Facing> TargetFacingChanged = delegate { };

    public Facing CurrentFacing { 
      get { return currentFacing; }
      set {
        if (currentFacing == value) return;
        currentFacing = value;
        TargetFacingChanged(value);
      } 
    }
}
