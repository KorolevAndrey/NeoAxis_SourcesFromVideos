using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using NeoAxis;

namespace Project
{
	public class File : NeoAxis.UIControl
	{
		private bool drag = false;
		private NeoAxis.UIControl dragimg = null;
		private NeoAxis.UIControl dragparent = null;
		private double dragsx = 0;
		private double dragsy = 0;
		private double dragex = 0;
		private double dragey = 0;
		private double dragox = 0;
		private double dragoy = 0;
		private UIMeasureValueRectangle cpos = new UIMeasureValueRectangle();

		private String windFrom = "";
		private String windTo = "";
		private String indxFrom = "";
		private String indxTo = "";

		public void showdimg(NeoAxis.UIImage img, bool show) 
		{
			var dimg = (UIImage)this.GetComponentByPath("DragImage");
			if(show) {
				dragimg = img;
				dimg.Margin = cpos; 
				dimg.Enabled = true;
				//ScreenMessages.Add("showdimg true, " + img?.GetPathFromRoot());
			} else {
				dragimg = null;
                dimg.Margin = cpos;
                dimg.Enabled = false;
				//ScreenMessages.Add("showdimg false, " + img?.GetPathFromRoot());
            }
		}
		
		public void showdimg(NeoAxis.UIImage img, bool show, String imagePath) 
		{
			var dimg = (UIImage)this.GetComponentByPath("DragImage");
			if(show) {
				dragimg = img;
				dimg.Margin = cpos;
				dimg.SourceImage = ReferenceUtility.MakeReference<Component_Image>(imagePath);
				dimg.Enabled = true;
				//ScreenMessages.Add("showdimg true, " + img?.GetPathFromRoot());
			} else {
				dragimg = null;
                dimg.Margin = cpos;
                var imagePath1 = InvUtils.getImagePath(SimulationApp.PlayerInv, "Empty");
                dimg.SourceImage = ReferenceUtility.MakeReference<Component_Image>(imagePath1);
                dimg.Enabled = false;
				//ScreenMessages.Add("showdimg false, " + img?.GetPathFromRoot());
            }
		}


        private void mouseDown(NeoAxis.UIControl sender, NeoAxis.EMouseButtons button) 
        {
        	if(!drag) {
				drag = true;
				dragparent = (UIWindow)sender.Parent;
				windFrom = dragparent.Name;
				indxFrom = sender.Name;
				ScreenMessages.Add("windFrom = " + windFrom + ", indxFrom = " + indxFrom);
				var imagePath = InvUtils.getImagePath(SimulationApp.PlayerInv, int.Parse(indxFrom)); 
				showdimg((UIImage)sender, true, imagePath);
				

				dragsx = MousePosition.X;
				dragsy = MousePosition.Y;
				dragox = dragsx - ((UIImage)sender).GetScreenPosition().X;
				dragoy = dragsy - ((UIImage)sender).GetScreenPosition().Y;

				//ScreenMessages.Add("MouseDown " + dragsx + ", " + dragsy + ", " + sender.GetPathFromRoot());
			} else {				
				indxTo = sender.Name;
				windTo = sender.Parent?.Name;				
                ScreenMessages.Add("windTo = " + windTo + ", indxTo = " + indxTo);
                
                if(windFrom == "Window 1" && windTo == "Window 1" && indxFrom != "" && indxTo != "") {
					/*var prevName = SimulationApp.PlayerInv.items[int.Parse(indxFrom)].Name;
					var prevcur = SimulationApp.PlayerInv.items[int.Parse(indxFrom)].Current;
					var res1 = InvUtils.GetItems(SimulationApp.PlayerInv, SimulationApp.PlayerInv.items[int.Parse(indxFrom)].Name, int.Parse(indxFrom), SimulationApp.PlayerInv.items[int.Parse(indxFrom)].Current);
					if(res1 >= 0) {
						var res2 = InvUtils.AddItems(SimulationApp.PlayerInv, prevName, int.Parse(indxTo), prevcur);
					}*/
					var res = InvUtils.tryMoveItems(SimulationApp.PlayerInv, int.Parse(indxFrom), int.Parse(indxTo));
					if(res >= 0) {
						var res1 = InvUtils.MoveItems(SimulationApp.PlayerInv, int.Parse(indxFrom), int.Parse(indxTo));
					}
				}

				windFrom = "";
				windTo = "";
				indxFrom = "";
				indxTo = "";
                
			    showdimg((UIImage)sender, false, "");
				dragparent = null;
				drag = false;
				RedrawImages();
                //ScreenMessages.Add("MouseDown1" + ", " + sender.GetPathFromRoot());
			}			
			
		}

        private void mouseUp(NeoAxis.UIControl sender, NeoAxis.EMouseButtons button) 
        {
//        	if(drag) {
//			    showdimg((UIImage)sender, false);
//				dragparent = null;
//				drag = false;
                
//                ScreenMessages.Add("MouseUp" + ", " + sender.GetPathFromRoot());
//			}
		}

        private void mouseMove(NeoAxis.UIControl sender) 
        {
        	if(drag && dragimg == sender) {
        			dragex = MousePosition.X - dragox;
			        dragey = MousePosition.Y - dragoy;

					var pos = cpos;
					pos.Left = dragex;
					pos.Top = dragey;
					((UIImage)this.GetComponentByPath("DragImage")).Margin = pos;
					
					//ScreenMessages.Add("MouseMove " + dragex + ", " + dragey + ", " + sender.GetPathFromRoot());
			}        	
		}


        protected override void OnEnabledInSimulation()
        {
        	
        	var dimg = (UIImage)this.GetComponentByPath("DragImage");
			cpos = dimg.Margin.Value;

			var img1 = (UIImage)(this.GetComponentByPath("Window 1\\0"));

			img1.MouseDown += Image_MouseDown1;
			img1.MouseUp += Image_MouseUp1;
			img1.MouseMove += Image_MouseMove1;
			var impath = InvUtils.getImagePath(SimulationApp.PlayerInv, 0);
			if(impath != null && impath != "") {
				img1.SourceImage = ReferenceUtility.MakeReference<Component_Image>(impath);
			} else {
				img1.SourceImage = dimg.SourceImage;
			}


			var img2 = (UIImage)(this.GetComponentByPath("Window 2\\0"));

			img2.MouseDown += Image_MouseDown2;
			img2.MouseUp += Image_MouseUp2;
			img2.MouseMove += Image_MouseMove2;

			for (int j = 0; j < 4; j ++) 
			{
				for (int i = 0; i < 5; i++)
				{
					if(!((i == 0) && (j == 0))) {
						var imgn2 = (UIImage)img2.Clone();
						imgn2.Name = (i + j * 5).ToString();
						imgn2.Enabled = true;
						var pos1 = img2.Margin.Value;
						pos1.Left = pos1.Left + i * (img2.Size.Value.X + 10);
						pos1.Top = pos1.Top + j * (img2.Size.Value.Y + 10);
						imgn2.Margin = pos1;
						imgn2.MouseDown += Image_MouseDown2;
						imgn2.MouseUp += Image_MouseUp2;
						imgn2.MouseMove += Image_MouseMove2;
						imgn2.SourceImage = dimg.SourceImage;
						img2.Parent.AddComponent(imgn2);
					}
				}
			}

			for (int i = 1; i < 5; i++)
			{
				var imgn1 = (UIImage)img1.Clone();
				imgn1.Name = (i).ToString();
				imgn1.Enabled = true;
				var pos = img1.Margin.Value;
				pos.Left = pos.Left + i * (img1.Size.Value.X + 10);
				imgn1.Margin = pos;
				imgn1.MouseDown += Image_MouseDown1;
				imgn1.MouseUp += Image_MouseUp1;
				imgn1.MouseMove += Image_MouseMove1;
				
				var impath1 = InvUtils.getImagePath(SimulationApp.PlayerInv, i);
				if(impath1 != null && impath1 != "") {
					imgn1.SourceImage = ReferenceUtility.MakeReference<Component_Image>(impath1);
				} else {
					imgn1.SourceImage = dimg.SourceImage;
				}
				img1.Parent.AddComponent(imgn1);
	    	}



            base.OnEnabledInSimulation();
        }
        
        
        public void RedrawImages() 
        {
        	
    	    var dimg = (UIImage)this.GetComponentByPath("DragImage");

        	for (int j = 0; j < 4; j ++) 
			{
				for (int i = 0; i < 5; i++)
				{
					var n = (i + j * 5).ToString();
					var imgn2 = (UIImage)this.GetComponentByPath("Window 2\\" + n);
					imgn2.SourceImage = dimg.SourceImage;
				}
			}
			
			for (int i = 0; i < 5; i++)
			{
				var n1 = (i).ToString();
				var imgn1 = (UIImage)this.GetComponentByPath("Window 1\\" + n1);
				var impath1 = InvUtils.getImagePath(SimulationApp.PlayerInv, i);
				if(impath1 != null && impath1 != "") {
					imgn1.SourceImage = ReferenceUtility.MakeReference<Component_Image>(impath1);
				} else {
					imgn1.SourceImage = dimg.SourceImage;
				}
	    	}
			
			
		}
        

        public void Image_MouseDown1(NeoAxis.UIControl sender, NeoAxis.EMouseButtons button, ref bool handled)
        {
			mouseDown(sender, button);
        }

        public void Image_MouseUp1(NeoAxis.UIControl sender, NeoAxis.EMouseButtons button, ref bool handled)
        {
			mouseUp(sender, button);
        }

        public void Image_MouseMove1(NeoAxis.UIControl sender)
        {
			mouseMove(sender);
        }



        public void Image_MouseDown2(NeoAxis.UIControl sender, NeoAxis.EMouseButtons button, ref bool handled)
        {
			mouseDown(sender, button);
        }

        public void Image_MouseUp2(NeoAxis.UIControl sender, NeoAxis.EMouseButtons button, ref bool handled)
        {
			mouseUp(sender, button);
        }

        public void Image_MouseMove2(NeoAxis.UIControl sender)
        {
			mouseMove(sender);
        }

        protected override bool OnKeyDown(KeyEvent e)
        {
        	if( e.Key == EKeys.Escape )
			{
             	if(drag) {
    			    showdimg(null, false);
    				dragparent = null;
    				drag = false;
                
                   //ScreenMessages.Add("Esc global");
    			}

				return true;
			}
        	
            return base.OnKeyDown(e);
        }

        public void Button_Click(NeoAxis.UIButton sender)
        {
			foreach(var itm in SimulationApp.PlayerInv.items) {
				ScreenMessages.Add("test - " + itm.Name + ", " + itm.Index + ", " + itm.Current + ", " + itm.Max);
			}
			
        }

        //        protected override bool OnMouseDown(EMouseButtons button)
        //        {

        //        	if(drag) {
        //			    showdimg(null, false);
        //				dragparent = null;
        //				drag = false;

        //                ScreenMessages.Add("MouseDown global");
        //			}


        //            return base.OnMouseDown(button);
        //        }
    }
}