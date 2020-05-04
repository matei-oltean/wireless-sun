using System;
using Godot;

public class PickableControl : ControlComponent {
	public static float GATHERING_TIME = 0.1f;

	[Export] public string item;
	[Export] public ushort quantity = 1;

	PlayerControl gatheringPlayer = null;

	public override void _Ready () {
		base._Ready ();
	}

	public void SetStack (string item, ushort quantity) {
		this.item = item;
		this.quantity = quantity;
		GetNode<Sprite> ("Sprite").Texture = Item.Manager.GetId (item).data.icon;
	}

	public void Gather (ControlComponent gatheringPlayer) {
		Tween tween = GetNode<Tween> ("Tween");
		tween.InterpolateProperty (MyPiece, "global_position", MyPiece.GlobalPosition, gatheringPlayer.GlobalPosition, GATHERING_TIME);
		if (gatheringPlayer.IsMaster) {
			tween.InterpolateCallback (this, GATHERING_TIME, nameof (AddToInventory));
			if (IsTrueMaster)
				tween.InterpolateCallback (this, GATHERING_TIME, nameof (Rpc), nameof (_OnDied));
			else
				tween.InterpolateCallback (this, GATHERING_TIME, nameof (_OnDied));
		}
		tween.Start ();
	}

	public void AddToInventory () {
		GameRoot.inventory.Add (Item.Manager.GetId (item), quantity);
	}

	public new Godot.Collections.Dictionary<string, object> MakeSave () {
		var saveObject = base.MakeSave ();
		saveObject["Item"] = item;
		saveObject["Quantity"] = (int) quantity;
		return saveObject;
	}

	public new void LoadData (Godot.Collections.Dictionary<string, object> saveObject) {
		base.LoadData (saveObject);
		SetStack (saveObject["Item"].ToString (), Convert.ToUInt16 (saveObject["Quantity"]));
	}
}