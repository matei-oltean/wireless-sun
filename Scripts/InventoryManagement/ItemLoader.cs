using System;
using System.Collections.Generic;
using Godot;
using Newtonsoft.Json;

namespace Item {
	public static class Manager {
		public static string dataPath = "res://Data/Items";
		public static Type[] itemCategories = new Type[] { typeof (ItemResource), typeof (EquipementResource), typeof (FoodResource), typeof (WeaponResource) };

		public static List<ItemResource> itemDataList = new List<ItemResource> ();
		private static Dictionary<string, ItemId> itemNames = new Dictionary<string, ItemId> ();

		public static void Load () {
			foreach (Type category in itemCategories) {
				string categoryPath;
				if (category == typeof (ItemResource))
					categoryPath = dataPath;
				else
					categoryPath = dataPath.PlusFile (category.Name.Remove (category.Name.Length - "Resource".Length));
				var categoryDir = new Directory ();
				if (!categoryDir.DirExists (categoryPath))
					GD.PrintErr ("The item category directory does not exist: \"" + categoryPath + "\"");
				categoryDir.Open (categoryPath);
				categoryDir.ListDirBegin (true, true);
				string itemPath;
				do {
					itemPath = categoryDir.GetNext ();
					if (itemPath.EndsWith (".tres")) {
						var loadedResource = (GD.Load (categoryPath.PlusFile (itemPath)) as ItemResource);
						if (loadedResource == null)
							GD.PrintErr ("Resource not found : ", categoryPath.PlusFile (itemPath));
						loadedResource.name = itemPath.Remove (itemPath.Length - ".tres".Length);
						itemDataList.Add (loadedResource);
					}
				} while (itemPath != "");

			}

			ushort rawItemId = 0;
			foreach (ItemResource item in itemDataList) {
				item.id = new ItemId (rawItemId);
				itemNames[item.name] = item.id;
				rawItemId += 1;
			}
		}

		public static ItemResource GetItem (ItemId id) {
			return GetItem<ItemResource> (id);
		}
		public static ItemResource GetItem (string name) {
			return GetItem<ItemResource> (name);
		}
		public static T GetItem<T> (ItemId id) where T : ItemResource {
			return (T) itemDataList[id.id];
		}
		public static T GetItem<T> (string name) where T : ItemResource {
			ItemId id;
			if (name != null && itemNames.TryGetValue (name.ToLower (), out id))
				return (T) GetItem<T> (id);
			GD.PrintErr ("Error: Trying to get a non existing item : " + name);
			return (T) ItemResource.NULL; // TODO: Once Godot is updated (and fixes the GD.Print(null) crash) we can return null here
		}

		public static ItemId GetId (string name) {
			ItemId id;
			if (name != null && itemNames.TryGetValue (name.ToLower (), out id))
				return id;
			GD.PrintErr ("Error: Trying to get a non existing item : " + name);
			return ItemId.NULL;
		}
	}

	public class ItemCategory {
		public byte id = 0;

		[JsonProperty ("category")]
		public string name;
		public ItemResource[] items;
		public ushort stackSize;
	}

	public struct ItemId {
		public ushort id;

		public static readonly ItemId NULL = new ItemId (ushort.MaxValue);
		public ItemId (ushort id) {
			this.id = id;
		}

		public ItemResource data { get { return Manager.GetItem (this); } }

		// Overrides

		public override bool Equals (object obj) {
			if (!(obj is ItemId)) {
				return false;
			}

			return id == ((ItemId) obj).id;
		}

		public override int GetHashCode () {
			return this.id.GetHashCode ();
		}

		public static bool operator == (ItemId a, ItemId b) {
			return a.Equals (b);
		}

		public static bool operator != (ItemId a, ItemId b) {
			return !a.Equals (b);
		}

		public override string ToString () {
			if (this == NULL) return "";
			return Manager.GetItem (this).name;
		}

		public bool IsNull () { return this == NULL; }
	}

	public static class ItemExtensions {
		public static string ToPrint (this IEnumerable<ItemStack> stacks) {
			return "[" + string.Join (", ", stacks) + "]";
		}
	}
}