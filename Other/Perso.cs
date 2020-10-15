using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using NeoAxis;
using NeoAxis.Editor;
using System.Linq;

namespace Project
{
    public class Perso : NeoAxis.UIWindow
    {
        protected override void OnEnabledInSimulation()
        {

            if (Components["ButtonClose"] != null)
                ((UIButton)Components["ButtonClose"]).Click += ISButtonClose_Click;
            CreateCaracterisique();
        }

        void ISButtonClose_Click( UIButton sender )
        {
         //ModeWindowInventaire = false;
         Dispose();
        }
        private void CreateCaracterisique()
        {
            Log.Warning("Ecran perso du jeu");
            UIControl Panneau_caracteristique = (UIControl) GetComponent<UIControl>("Panneau_caracteristique");
            UIImage cartouche_caracterisique = (UIImage)Panneau_caracteristique.GetComponent<UIImage>("caracterisique_image");
            double marge_top_first = 0.0955178451138557;
            double hauteur_cartouche = 0.1;
            double largeur_cartouche = 0.33;
            double marge_left_first = 0;
            List<UIImage> List_cartouche_caracteristique = new List<UIImage>();
            for (int i=0 ; i <= 4;i++)
            {
                for (int j=0 ; j <=2;j++)
                {
                    Log.Warning("Creation cartouche {0}",i);
                    List_cartouche_caracteristique.Add((UIImage)cartouche_caracterisique.Clone());
                    Panneau_caracteristique.AddComponent(List_cartouche_caracteristique.Last());
                    List_cartouche_caracteristique.Last().PropertySet("Margin", new UIMeasureValueRectangle
                        {
                            Measure = UIMeasure.Parent,
                            Top = marge_top_first * (i + 1) + hauteur_cartouche * (i),
                            Left = marge_left_first * (j + 1) + largeur_cartouche * (j)
                        });
                }
            }
        }
    }
}