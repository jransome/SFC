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
      Debug.Log("facing model "+id);
  }

  public Facing CurrentFacing
  {
    get { return currentFacing; }
    set
    {
      if (currentFacing == value) return;
      Debug.Log(id + "id - index" + value.Index);
      currentFacing = value;
      TargetFacingChanged(value);
    }
  }
}
