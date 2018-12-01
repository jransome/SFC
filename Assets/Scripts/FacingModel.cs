using System;
using UnityEngine;

public class FacingModel
{
  private static int counter = 0;
  private Facing currentFacing = Facing.None;
  public int id;

  public event Action<Facing> TargetFacingChanged = delegate { };

  public FacingModel()
  {
      counter++;
      id = counter;
  }

  public Facing CurrentFacing
  {
    get { return currentFacing; }
    set
    {
      if (currentFacing == value) return;
      currentFacing = value;
      TargetFacingChanged(value);
    }
  }
}
