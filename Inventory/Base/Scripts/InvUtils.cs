using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using NeoAxis;
using NeoAxis.Editor;
using System.Linq;

namespace Project
{
	public static class InvUtils
	{
		public static bool DebugMessages = true;
		
		private static void log(String msg)
		{
			if(DebugMessages) {
				ScreenMessages.Add(msg);
			}
		}
		
        public static String[] splitStr(String inStr) 
        {
			String[] outList;
			outList = inStr?.Split(',');
			return outList;
        }


        public static int[] splitInt(String inStr) 
        {
			String[] strList = splitStr(inStr);
			int[] outList = new int[10];
			int i = 0;

            foreach(var val in strList) 
            {
				outList[i] = int.Parse( val );
				i = i + 1;
			}

			return outList;
        }

        public static String getImagePath(Inv inv, String Name) 
        {
			String res = "";
			try {
				res = inv.imagePaths.First(i => i.Name == Name).Path;
			} catch(Exception e) {
				log(e.Message);
			}
			return res;
		}

        public static String getImagePath(Inv inv, int indx) 
        {
			String res = "";
			String Name = "";
			try {
				Name = inv.items.First(i => i.Index == indx).Name;
				res = getImagePath(inv, Name);
			} catch(Exception e) {
				log(e.Message);
			}
			return res;
		}


        public static int AddItems(Inv inv, String name, int prefIndex, int current) 
		{
			int res = AddItems(inv.items, inv.AllItems, inv.ExceedMaxItems, inv.LastIndex, name, prefIndex, current, inv.MaxIndex, inv.CellMaxItems);
			return res;
		}

		
		public static int AddItems(List<InvItem> items, bool AllItems, bool ExceedMaxItems, int LastIndex, String name, int prefIndex, int current, int MaxIndex, int max = 99) 
		{
			log("AddItems");
			InvItem foundItem = null;
			try {
				foundItem = items?.First<InvItem>(i => i.Name == name && i.Current < i.Max && (prefIndex == -1 || i.Index == prefIndex));
			} catch(Exception e) {
				log(e.Message);
			}
			if(foundItem == null && AllItems) {
				LastIndex = LastIndex + 1;
				if(LastIndex <= MaxIndex) {
					items.Add(new InvItem(name, LastIndex, current, max));
				} else {
					return -1;
				}
				return 1;
			} else {
				foundItem.Current = foundItem.Current + current;
				if(foundItem.Current > foundItem.Max) {
					var currentVal = foundItem.Current - foundItem.Max; 
					foundItem.Current = foundItem.Max;	
					if(ExceedMaxItems) {
						LastIndex = LastIndex + 1;
						if (LastIndex <= MaxIndex)
						{
							items.Add(new InvItem(name, LastIndex, currentVal, max));
						}
						else
						{
							return -2;
						}
					}
					return 2;
				}
			}
			return 0;
		}


		public static int GetItems(Inv inv, String name, int prefIndex, int current) 
		{
			int res = GetItems(inv.items, name, prefIndex, current);
			return res;
		}

		public static int GetItems(List<InvItem> items, String name, int prefIndex, int current) 
		{
			log("GetItems");
			InvItem foundItem = null;
			try {
				foundItem = items?.First<InvItem>(i => i.Name == name && i.Current >= current && (prefIndex == -1 || i.Index == prefIndex));
			} catch(Exception e) {
				log(e.Message);
			}
			if(foundItem == null) {
				return -1;
			} else {
				foundItem.Current = foundItem.Current - current;
				if(foundItem.Current <= 0) {
					//foundItem.Name = "";
					//foundItem.Max = 0;
					//foundItem.Current = 0;
					items.Remove(foundItem);
					return 1;
				}
			}
			return 0;
		}


        public static int tryGetItems(Inv inv, String name, int current)
        {
			int res = tryGetItems(inv.items, name, current);
			return res;
	    }

		public static int tryGetItems(List<InvItem> items, String name, int current) 
		{
			log("tryGetItems");
			InvItem foundItem = null;
			try {
				foundItem = items?.First<InvItem>(i => i.Name == name && i.Current >= current);
			} catch(Exception e) {
				log(e.Message);
			}
			if(foundItem == null) {
				return -1;
			}
			return 0;
		}


		public static int tryMoveItems(Inv inv, int IndexFrom, int IndexTo) 
		{			
			log("tryMoveItems");
			InvItem foundItem = null;
			InvItem foundItem1 = null;
			try {
				foundItem = inv.items.Find(i => i.Index == IndexFrom);
				foundItem1 = inv.items.Find(i => i.Index == IndexTo);
			} catch(Exception e) {
				log(e.Message);
				return -1;
			}
			
			return 0;
		}
		
		public static int MoveItems(Inv inv, int IndexFrom, int IndexTo) 
		{			
			log("MoveItems");
			int foundItemid;
			int foundItemid1;
			try {		    
				lock(inv.items) {
					foundItemid = inv.items.FindIndex(i => i.Index == IndexFrom);
					foundItemid1 = inv.items.FindIndex(i => i.Index == IndexTo);
					
					inv.items[foundItemid].Index = IndexTo;
					inv.items[foundItemid1].Index = IndexFrom;
				}	
			} catch(Exception e) {
				log(e.Message);
				return -1;
			}
			
			return 0;
		}		
		
	}
}