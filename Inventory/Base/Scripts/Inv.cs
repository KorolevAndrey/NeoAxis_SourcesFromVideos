using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using NeoAxis;
using NeoAxis.Editor;

namespace Project
{
	public class Inv : NeoAxis.Component
	{
		public class ImagePath {
			public String Name;
			public String Path;

            public ImagePath(string name, string path)
            {
                Name = name;
                Path = path;
            }
        }
		
		
		public List<InvItem> items = new List<InvItem>();
		public int LastIndex = 0;
		
		/// <summary>Max inventory cells-1</summary>
		public int MaxIndex = 4;
		public int CellMaxItems = 99;

        /// <summary>Type Name, amount. Example - Water, 10</summary>
		public List<String> InitialItems = new List<string>();
		public bool CalcInitialItemsDone = false;
		public bool CalcImagePathsDone = false;
		public bool AllItems = true;
		public bool ExceedMaxItems = false;
		public bool DebugMessages = true;
		public String imagePathRoot = "Import";


		public List<ImagePath> imagePaths = new List<ImagePath>();


		private void log(String msg)
		{
			if(DebugMessages) {
				ScreenMessages.Add(msg);
			}
		}

        public void CalcInitialItems() 
        {
			if (CalcInitialItemsDone) return;
			CalcInitialItemsDone = true;
        	int indx = 0;
			foreach(var item in InitialItems) {
				String[] itemParams = InvUtils.splitStr(item);
				items.Add(new InvItem(itemParams[0], indx, int.Parse(itemParams[1]), CellMaxItems));
				LastIndex = indx;
				indx = indx + 1;            
			}
			log(this.Name + " CalcInitialItems done");
		}
		
		public void CalcImagePaths() {
			if (CalcImagePathsDone) return;
			CalcImagePathsDone = true;
			imagePaths.Add(new ImagePath("Empty", imagePathRoot + "\\Empty.png"));
			imagePaths.Add(new ImagePath("Blocked", imagePathRoot + "\\Blocked.png"));
			imagePaths.Add(new ImagePath("Water", imagePathRoot + "\\Water.png"));
        	imagePaths.Add(new ImagePath("Seed", imagePathRoot + "\\Seed.png"));
        	imagePaths.Add(new ImagePath("Potato", imagePathRoot + "\\Potato.png"));
        	log(this.Name + " CalcImagePaths done");
		}

        protected override void OnEnabledInSimulation()
        {
//			CalcImagePaths();

//			CalcInitialItems();
			
			log(this.Name + " Init Ok");
        	
            base.OnEnabledInSimulation();
        }

        protected override void OnUpdate(float delta)
        {
            base.OnUpdate(delta);
        }

        protected override void OnSimulationStep()
        {
            base.OnSimulationStep();
        }
    }
}