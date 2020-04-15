using System;
using Godot;

public class InventoryItem : SelectableItem {
	// Action to perform when double click
	public override void DoubleClick () {
		GameRoot.inventory.Use (mySlot.item);
	}
}