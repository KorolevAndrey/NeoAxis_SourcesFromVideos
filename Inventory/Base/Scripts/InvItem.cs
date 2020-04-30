using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using NeoAxis;
using NeoAxis.Editor;

namespace Project
{
	public class InvItem 
	{
		
		public String Name;
		public int Index;
		public int Current;
		public int Max;
		public double ExpireTime = 0;

        public InvItem(string name, int index, int current, int max)
        {
                Name = name;
				Index = index;
                Current = current;
                Max = max;
        }
		
		
		

	}
}