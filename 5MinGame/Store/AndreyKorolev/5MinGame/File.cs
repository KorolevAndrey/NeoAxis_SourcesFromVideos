using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using NeoAxis;

namespace Project
{
	public class File : NeoAxis.UIControl
	{

		public int EnemiesKill = 0;
		public int Level = 1;
		public String CheckPoint = "";
		private bool hasWeapon = false;


		public void SaveGame(String filename) {
            TextBlock block = new TextBlock();
            TextBlock b1 = block.AddChild( "EnemiesKill", EnemiesKill.ToString() );
            TextBlock b2 = block.AddChild( "Level", Level.ToString() );
            TextBlock b3 = block.AddChild( "CheckPoint", CheckPoint );
			
			var sc = Component_Scene.First;
			var chr = (Component_Character)sc?.GetComponentByPath("Character");            
            
            if(chr.ItemGetEnabledFirst() != null) {
				hasWeapon = true;
			}
            
            TextBlock b4 = block.AddChild( "hasWeapon", hasWeapon.ToString() );

			TextBlockUtility.SaveToVirtualFile(block, "Store\\AndreyKorolev\\5MinGame\\Saves\\" + filename + ".lvldata");
		}

		public void LoadGame(String filename) {
			if (!VirtualFile.Exists("Store\\AndreyKorolev\\5MinGame\\Saves\\" + filename + ".lvldata"))
			{
				ScreenMessages.Add("No file");
				return;
			}
			TextBlock block = TextBlockUtility.LoadFromVirtualFile("Store\\AndreyKorolev\\5MinGame\\Saves\\" + filename + ".lvldata");
			EnemiesKill = int.Parse( block.FindChild("EnemiesKill").Data );
			ScreenMessages.Add("EnemiesKill = " + EnemiesKill);
			Level = int.Parse( block.FindChild("Level").Data );
			ScreenMessages.Add("Level = " + Level);
			CheckPoint = block.FindChild("CheckPoint").Data;
			ScreenMessages.Add("CheckPoint = " + CheckPoint);
			hasWeapon = bool.Parse( block.FindChild("hasWeapon").Data );
			ScreenMessages.Add("hasWeapon = " + hasWeapon);

			ShowWin(false);
			AddEK(0);
			if(Level == 1) {
				LoadSc("File");
			} else if(Level == 2) {
				LoadSc("File2");
			} else {
				ScreenMessages.Add("No level");
			}

			var sc = Component_Scene.First;
			var chpoint = (Component_Sensor)sc?.GetComponentByPath(CheckPoint);
			var chr = (Component_Character)sc?.GetComponentByPath("Character");
			var chrcol = (Component_RigidBody)sc?.GetComponentByPath("Character\\Collision Body");
			chrcol.Transform = chrcol.TransformV.UpdatePosition(new Vector3(chpoint.TransformV.Position.X, chpoint.TransformV.Position.Y, chr.TransformV.Position.Z)); 
						
			GetWeapon(sc, chr, hasWeapon);			
			
			ScreenMessages.Add("Load Done");
		}


		public void GetWeapon(Component_Scene sc, Component_Character chr, bool hasWeapon) {
			var wpn = (Component_Weapon)sc?.GetComponentByPath("Weapon");

			if (hasWeapon)
			{
				chr.ItemTake(wpn);
				chr.ItemActivate(wpn);
			} else {
				if(chr.ItemGetEnabledFirst() != null) {
					chr.ItemDrop(chr.ItemGetEnabledFirst());
				}
			}
					
		}
		

        public void SetCheckPoint(String val) {
			CheckPoint = val;
		}

		public void ShowWin(bool show) {
			var txt = GetComponent<UIText>("WIN Text");
			txt.Visible = show;
		}

		public void AddEK(int val) {
			EnemiesKill = EnemiesKill + val;
			var txt = GetComponent<UIText>("Enemies kill");
			txt.Text = EnemiesKill.ToString();
		}


		public void LoadSc(String sc)
		{
			string nextLevel = "Store\\AndreyKorolev\\5MinGame\\"+sc+".scene";

			if (VirtualFile.Exists(nextLevel))
			{	
				
				PlayScreen.Instance.Load(nextLevel, false);
				ResetCmr();
			} else {
				ScreenMessages.Add("No scene");
			}
		}


		public void LoadNext()
		{
			if (Level > 1) {
				ScreenMessages.Add("No next scene");
				return;
			}
			string nextLevel = "Store\\AndreyKorolev\\5MinGame\\File2.scene";

			if (VirtualFile.Exists(nextLevel))
			{
				Level = 2;				
				
				PlayScreen.Instance.Load(nextLevel, false);
				ResetCmr();
			} else {
				ScreenMessages.Add("No scene");
			}
		}
		
		
		public void ResetCmr() {
			PlayScreen.Instance.OpenOrCloseLevelWindow();
		}

	}
}