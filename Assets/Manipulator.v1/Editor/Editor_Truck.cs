using UnityEngine;
using UnityEditor;
[ExecuteInEditMode]
[CustomEditor(typeof(Frame_Truck))]
public class Editor_Truck : Editor {
	private Frame_Truck myScript;
	//Frame
	Texture iconBig_Frame;
	Texture iconSmall_Frame;
	Texture iconFrame_Plus;
	//Cabin
	Texture iconCabin_Small;
	Texture iconCabin_Big;
	//Panel Cabin
	Texture iconTuningCabin;
	Texture iconPanelCabin_1;
	Texture iconPanelCabin_2;
	Texture iconPanelCabin_3;
	Texture iconPanelCabin_4;
	Texture iconPanelCabin_5;
	Texture iconPanelCabin_6;
	Texture iconPanelCabin_7;
	//Bumper Cabin
	Texture iconBumperCabin_1;
	Texture iconBumperCabin_2;
	Texture iconBumperCabin_3;
	//Tuning Cabin
	Texture iconDSG1;
	Texture iconDSG4;
	Texture iconDSG2_3;
	//Trunk
	Texture iconTrunk;
	Texture iconCowl;
	Texture iconTrunk_Small_Big;
	//Light Cabin
	Texture iconLaght2;
	Texture iconLight3;
	// Color
	Texture iconColor;
	Texture color_Blue;
	Texture color_Blue_Plus;
	Texture color_Brown;
	Texture color_Brown_Plus;
	Texture color_Green;
	Texture color_Green_Plus;
	Texture color_Red;
	Texture color_Red_Plus;
	Texture color_White;
	Texture color_White_Plus;
	Texture color_Yellow;
	Texture color_Yellow_Plus;
	//Wheel
	Texture iconBridge_4x4;
	Texture icon_Bridge2x2;
	//Wing
	Texture icon_Wing;
	//Bumper
	Texture icon_Bumper1;
	Texture icon_Bumper2;
	Texture icon_Bumper3;
	//Hitch
	Texture icon_Hitch;
	//Air
	Texture icon_Air_L;
	Texture icon_Air_R;
	//Battery
	Texture icon_Battery_L;
	Texture icon_Battery_R;
	//BoxSmall
	Texture icon_BoxSmall_L;
	Texture icon_BoxSmall_R;
	//Box Big
	Texture icon_BoxBig_L;
	Texture icon_BoxBig_R;
	Texture icon_BoxBig_Plus_L;
	Texture icon_BoxBig_Plus_R;
	//Tank
	Texture icon_Tank_L;
	Texture icon_Tank_R;
	Texture icon_Tank_Plus;
	//Reserve
	Texture icon_Reserve_Plus;
	Texture icon_Reserve_L;
	Texture icon_Reserve_L_Back;
	Texture icon_Reserve_For;
	//EQUIPMENT
	Texture icon_Equipment;
	Texture icon_Manipulator1;
	Texture icon_Manipulator2;
	Texture icon_Manipulator3;
	//Body
	Texture icon_Body;
	//Protection
	Texture icon_Protection;
	//Info
	Texture icon_Info;
	Texture icon_Warning;
		bool warning = true;
		bool info = true;
	//Controller
	Texture icon_Controller;
	private bool viewController = true;
	Texture icon_BodyDop3;
	Texture icon_Crable;
	void Awake(){
		icon_Info = Resources.Load ("UI_Truck/Info", typeof(Texture2D))as Texture2D;
		icon_Warning = Resources.Load ("UI_Truck/Warning", typeof(Texture2D))as Texture2D;
		iconBig_Frame = Resources.Load ("UI_Truck/Big_Frame", typeof(Texture2D))as Texture2D;
		iconSmall_Frame = Resources.Load ("UI_Truck/Small_Frame", typeof(Texture2D))as Texture2D;
		iconFrame_Plus = Resources.Load ("UI_Truck/Frame_Plus", typeof(Texture2D))as Texture2D;
		iconCabin_Small = Resources.Load ("UI_Truck/Cabin1", typeof(Texture2D))as Texture2D;
		iconCabin_Big = Resources.Load ("UI_Truck/Cabin2", typeof(Texture2D))as Texture2D;
		iconTuningCabin = Resources.Load ("UI_Truck/Icon_TuningCab", typeof(Texture2D))as Texture2D;
		iconPanelCabin_1 = Resources.Load ("UI_Truck/PanelCab_1", typeof(Texture2D))as Texture2D;
		iconPanelCabin_2 = Resources.Load ("UI_Truck/PanelCab_2", typeof(Texture2D))as Texture2D;
		iconPanelCabin_3 = Resources.Load ("UI_Truck/PanelCab_3", typeof(Texture2D))as Texture2D;
		iconPanelCabin_4 = Resources.Load ("UI_Truck/PanelCab_4", typeof(Texture2D))as Texture2D;
		iconPanelCabin_5 = Resources.Load ("UI_Truck/PanelCab_5", typeof(Texture2D))as Texture2D;
		iconPanelCabin_6 = Resources.Load ("UI_Truck/PanelCab_6", typeof(Texture2D))as Texture2D;
		iconPanelCabin_7 = Resources.Load ("UI_Truck/PanelCab_7", typeof(Texture2D))as Texture2D;
		iconBumperCabin_1 = Resources.Load ("UI_Truck/F_Bumper1", typeof(Texture2D))as Texture2D;
		iconBumperCabin_2 = Resources.Load ("UI_Truck/F_Bumper2", typeof(Texture2D))as Texture2D;
		iconBumperCabin_3 = Resources.Load ("UI_Truck/F_Bumper3", typeof(Texture2D))as Texture2D;
		iconDSG1 = Resources.Load ("UI_Truck/DSG1", typeof(Texture2D))as Texture2D;
		iconDSG4 = Resources.Load ("UI_Truck/DSG4", typeof(Texture2D))as Texture2D;
		iconDSG2_3 = Resources.Load ("UI_Truck/DSG_2_3", typeof(Texture2D))as Texture2D;
		iconTrunk = Resources.Load ("UI_Truck/Trunk", typeof(Texture2D))as Texture2D;
		iconCowl = Resources.Load ("UI_Truck/Cowl", typeof(Texture2D))as Texture2D;
		iconTrunk_Small_Big = Resources.Load ("UI_Truck/Trunk_Big_Small", typeof(Texture2D))as Texture2D;
		iconLaght2 = Resources.Load ("UI_Truck/Light2", typeof(Texture2D))as Texture2D;
		iconLight3 = Resources.Load ("UI_Truck/Light3", typeof(Texture2D))as Texture2D;
		iconColor = Resources.Load ("UI_Truck/Icon_Color", typeof(Texture2D))as Texture2D;
		icon_Bridge2x2 = Resources.Load ("UI_Truck/Bridge2x2", typeof(Texture2D))as Texture2D;
		iconBridge_4x4 = Resources.Load ("UI_Truck/Bridge4x4", typeof(Texture2D))as Texture2D;
		icon_Wing = Resources.Load ("UI_Truck/Wing", typeof(Texture2D))as Texture2D;
		icon_Bumper1 = Resources.Load ("UI_Truck/Bumper1", typeof(Texture2D))as Texture2D;
		icon_Bumper2 = Resources.Load ("UI_Truck/Bumper2", typeof(Texture2D))as Texture2D;
		icon_Bumper3 = Resources.Load ("UI_Truck/Bumper3", typeof(Texture2D))as Texture2D;
		icon_Hitch = Resources.Load ("UI_Truck/Hitch", typeof(Texture2D))as Texture2D;
		icon_Air_L = Resources.Load ("UI_Truck/Air_L", typeof(Texture2D))as Texture2D;
		icon_Air_R = Resources.Load ("UI_Truck/Air_R", typeof(Texture2D))as Texture2D;
		icon_Battery_L = Resources.Load ("UI_Truck/Battery_L", typeof(Texture2D))as Texture2D;
		icon_Battery_R = Resources.Load ("UI_Truck/Battery_R", typeof(Texture2D))as Texture2D;
		icon_BoxSmall_L = Resources.Load ("UI_Truck/Box2L", typeof(Texture2D))as Texture2D;
		icon_BoxSmall_R = Resources.Load ("UI_Truck/Box2R", typeof(Texture2D))as Texture2D;
		icon_BoxBig_L = Resources.Load ("UI_Truck/Box1_L", typeof(Texture2D))as Texture2D;
		icon_BoxBig_R = Resources.Load ("UI_Truck/Box1_R", typeof(Texture2D))as Texture2D;
		icon_BoxBig_Plus_L = Resources.Load ("UI_Truck/Box1_L(Plus)", typeof(Texture2D))as Texture2D;
		icon_BoxBig_Plus_R = Resources.Load ("UI_Truck/Box1_R(Plus)", typeof(Texture2D))as Texture2D;
		icon_Tank_L = Resources.Load ("UI_Truck/Tank2_L", typeof(Texture2D))as Texture2D;
		icon_Tank_R = Resources.Load ("UI_Truck/Tank2_R", typeof(Texture2D))as Texture2D;
		icon_Tank_Plus = Resources.Load ("UI_Truck/Tank", typeof(Texture2D))as Texture2D;
		icon_Reserve_Plus = Resources.Load ("UI_Truck/Reserve1", typeof(Texture2D))as Texture2D;
		icon_Reserve_L = Resources.Load ("UI_Truck/Reserve2_L", typeof(Texture2D))as Texture2D;
		icon_Reserve_L_Back = Resources.Load ("UI_Truck/Reserve2_R", typeof(Texture2D))as Texture2D;
		icon_Reserve_For = Resources.Load ("UI_Truck/Reserve2", typeof(Texture2D))as Texture2D;
		icon_Equipment = Resources.Load ("UI_Truck/Icon_Equipment", typeof(Texture2D))as Texture2D;
		icon_Manipulator1 = Resources.Load ("UI_Truck/Manip1", typeof(Texture2D))as Texture2D;
		icon_Manipulator2 = Resources.Load ("UI_Truck/Manip2", typeof(Texture2D))as Texture2D;
		icon_Manipulator3 = Resources.Load ("UI_Truck/Manip3", typeof(Texture2D))as Texture2D;
		icon_Body = Resources.Load ("UI_Truck/Body", typeof(Texture2D))as Texture2D;
		icon_Protection = Resources.Load ("UI_Truck/Protection", typeof(Texture2D))as Texture2D;
		icon_Controller = Resources.Load ("UI_Truck/ControllerTruck", typeof(Texture2D))as Texture2D;
		icon_BodyDop3 = Resources.Load ("UI_Truck/BodyDop3", typeof(Texture2D))as Texture2D;
		icon_Crable = Resources.Load ("UI_Truck/Crable", typeof(Texture2D))as Texture2D;
		color_Blue = Resources.Load ("UI_Truck/Color_Blue", typeof(Texture2D))as Texture2D;
		color_Blue_Plus = Resources.Load ("UI_Truck/color_Blue(Plus)", typeof(Texture2D))as Texture2D;
		color_Brown = Resources.Load ("UI_Truck/Color_Brown", typeof(Texture2D))as Texture2D;
		color_Brown_Plus = Resources.Load ("UI_Truck/Color_Brown(Plus)", typeof(Texture2D))as Texture2D;
		color_Green = Resources.Load ("UI_Truck/Color_Green", typeof(Texture2D))as Texture2D;
		color_Green_Plus = Resources.Load ("UI_Truck/Color_Green(Plus)", typeof(Texture2D))as Texture2D;
		color_Red = Resources.Load ("UI_Truck/Color_Red", typeof(Texture2D))as Texture2D;
		color_Red_Plus = Resources.Load ("UI_Truck/Color_Red(Plus)", typeof(Texture2D))as Texture2D;
		color_White = Resources.Load ("UI_Truck/Color_White", typeof(Texture2D))as Texture2D;
		color_White_Plus = Resources.Load ("UI_Truck/Color_White(Plus)", typeof(Texture2D))as Texture2D;
		color_Yellow = Resources.Load ("UI_Truck/Color_Yellow", typeof(Texture2D))as Texture2D;
		color_Yellow_Plus = Resources.Load ("UI_Truck/Color_Yellow(Plus)", typeof(Texture2D))as Texture2D;
	}
	public void OnEnable(){
		myScript = (Frame_Truck)target;
		if (myScript.frameBig_Bool == false) {
			myScript.view_FramePlus = false;
		}
		if (myScript.gameObject.GetComponent<ControllerTruck> () != null) {
			myScript.controllerTruck_Bool = false;
		} else if (myScript.gameObject.GetComponent<ControllerTruck> () == null) {
			myScript.controllerTruck_Bool = true;
		}
	}
	public override void OnInspectorGUI(){
		if (myScript.frame_Int != 0 && myScript.bridge_Int != 0) {
			viewController = false;
		} else {
			viewController = true;
		}
		//	base .DrawDefaultInspector ();
		//Frame_________________________________________________________________________________________________
		GUILayout.BeginHorizontal ();
		if (GUILayout.Button (iconBig_Frame, GUILayout.Width (60), GUILayout.Height (60))) {
			myScript.FrameBig ();
		}
		if (GUILayout.Button (iconSmall_Frame, GUILayout.Width (60), GUILayout.Height (60))) {
			myScript.FrameSmall ();
		}
		EditorGUI.BeginDisabledGroup (myScript.view_Frame_Tuning);
		if (GUILayout.Button (iconTuningCabin, GUILayout.Width (120), GUILayout.Height (60))) {
			if (myScript.iconTuningFrame_Bool == true) {
				myScript.iconTuningFrame_Bool = false;
			} else if (myScript.iconTuningFrame_Bool == false) {
				myScript.iconTuningFrame_Bool = true;
			}
		}
		EditorGUI.EndDisabledGroup ();
		GUILayout.EndHorizontal ();
		//Tuning Frame__________________________________________________________________________________________
		if (myScript.iconTuningFrame_Bool == false) {
			GUILayout.BeginHorizontal ();
			if (GUILayout.Button (icon_Bridge2x2, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.Bridge2x2 ();
			}
			if (GUILayout.Button (iconBridge_4x4, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.Bridge4x4 ();
			}
			EditorGUI.BeginDisabledGroup (myScript.view_Wing_Bool);
			if (GUILayout.Button (icon_Wing, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.Wing ();
			}
			EditorGUI.EndDisabledGroup ();
			EditorGUI.BeginDisabledGroup (myScript.view_FramePlus);
			if (GUILayout.Button (iconFrame_Plus, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.FramePlus ();
			}
			EditorGUI.EndDisabledGroup ();
			EditorGUI.BeginDisabledGroup (myScript.view_Hitch);
			if (GUILayout.Button (icon_Hitch, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.Hitch ();
			}
			EditorGUI.EndDisabledGroup ();
			GUILayout.EndHorizontal ();
			GUILayout.BeginHorizontal ();
			if (GUILayout.Button (icon_Bumper1, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.Bumper1 ();
			
			}
			if (GUILayout.Button (icon_Bumper2, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.Bumper2 ();

			}
			if (GUILayout.Button (icon_Bumper3, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.Bumper3 ();

			}
			if (GUILayout.Button (icon_Air_L, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.AirLeft ();
			}
			if (GUILayout.Button (icon_Air_R, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.AirRight ();
			}
			GUILayout.EndHorizontal ();
			GUILayout.BeginHorizontal ();
			if (GUILayout.Button (icon_Battery_L, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.BatteryLeft ();
			}
			if (GUILayout.Button (icon_Battery_R, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.BatteryRight ();
			}
			if (GUILayout.Button (icon_BoxSmall_L, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.BoxSmall_Left ();
			}
			if (GUILayout.Button (icon_BoxSmall_R, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.BoxSmall_Right ();
			}
			if (GUILayout.Button (icon_Reserve_L, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.Reserve_Left ();
			}
			GUILayout.EndHorizontal ();
			GUILayout.BeginHorizontal ();
			if (GUILayout.Button (icon_BoxBig_L, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.BoxBig_Left ();
			}
			if (GUILayout.Button (icon_BoxBig_R, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.BoxBig_Right ();
			}
			EditorGUI.BeginDisabledGroup (myScript.view_Plus);
			if (GUILayout.Button (icon_BoxBig_Plus_L, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.BoxBig_Left_Plus ();
			}
			if (GUILayout.Button (icon_BoxBig_Plus_R, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.BoxBig_Right_Plus ();
			}
			EditorGUI.EndDisabledGroup ();
			EditorGUI.BeginDisabledGroup (myScript.view_Reserve_Left);
			if (GUILayout.Button (icon_Reserve_L_Back, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.ReserveLeftBack ();
			}
			EditorGUI.EndDisabledGroup ();
			GUILayout.EndHorizontal ();
			GUILayout.BeginHorizontal ();
			if (GUILayout.Button (icon_Tank_L, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.Tank_Left ();
			}
			if (GUILayout.Button (icon_Tank_R, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.Tank_Right ();
			}
			EditorGUI.BeginDisabledGroup (myScript.view_Plus);
			if (GUILayout.Button (icon_Tank_Plus, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.TankPlus ();
			}
			if (GUILayout.Button (icon_Reserve_Plus, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.ReserveS ();
			}
			EditorGUI.EndDisabledGroup ();
			if (GUILayout.Button (icon_Reserve_For, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.ReserveFor ();
			}
			GUILayout.EndHorizontal ();
		}
		//Cabin_________________________________________________________________________________________________
		GUILayout.BeginHorizontal ();
		if (GUILayout.Button (iconCabin_Small, GUILayout.Width (60), GUILayout.Height (60))) {
			myScript.CabinSmall ();
			myScript.CabinMaterial ();
			myScript.CabinBumper ();
		}
		if (GUILayout.Button (iconCabin_Big, GUILayout.Width (60), GUILayout.Height (60))) {
			myScript.CabinBig ();
			myScript.CabinMaterial ();
			myScript.CabinBumper ();
		}
		EditorGUI.BeginDisabledGroup (myScript.view_Cabin);
		if (GUILayout.Button (iconTuningCabin, GUILayout.Width (120), GUILayout.Height (60))) {
			if (myScript.iconTuningCabin_Bool == true) {
				myScript.iconTuningCabin_Bool = false;
			} else if (myScript.iconTuningCabin_Bool == false) {
				myScript.iconTuningCabin_Bool = true;
			}
		}
		EditorGUI.EndDisabledGroup ();
		GUILayout.EndHorizontal ();
		// Tuning Cabin_________________________________________________________________________________________________
		if (myScript.iconTuningCabin_Bool == false) {
			GUILayout.BeginHorizontal ();
			if (GUILayout.Button (iconPanelCabin_1, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.panelCabin_Int = 1;
				myScript.PanelCabin ();

			}
			if (GUILayout.Button (iconPanelCabin_2, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.panelCabin_Int = 2;
				myScript.PanelCabin ();

			}
			if (GUILayout.Button (iconPanelCabin_3, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.panelCabin_Int = 3;
				myScript.PanelCabin ();

			}
			if (GUILayout.Button (iconPanelCabin_4, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.panelCabin_Int = 4;
				myScript.PanelCabin ();

			}
			if (GUILayout.Button (iconPanelCabin_5, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.panelCabin_Int = 5;
				myScript.PanelCabin ();

			}
			GUILayout.EndHorizontal ();
			GUILayout.BeginHorizontal ();
			if (GUILayout.Button (iconPanelCabin_6, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.panelCabin_Int = 6;
				myScript.PanelCabin ();

			}
			if (GUILayout.Button (iconPanelCabin_7, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.panelCabin_Int = 7;
				myScript.PanelCabin ();

			}
			if (GUILayout.Button (iconBumperCabin_1, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.cabin_Bumper_Int = 1;
				myScript.CabinBumper ();
			}
			if (GUILayout.Button (iconBumperCabin_2, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.cabin_Bumper_Int = 2;
				myScript.CabinBumper ();
			}
			if (GUILayout.Button (iconBumperCabin_3, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.cabin_Bumper_Int = 3;
				myScript.CabinBumper ();
			}
			GUILayout.EndHorizontal ();
			GUILayout.BeginHorizontal ();
			if (GUILayout.Button (iconDSG1, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.DSG1 ();
			}
			if (GUILayout.Button (iconDSG4, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.DSG4 ();
			}
			if (GUILayout.Button (iconDSG2_3, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.DSG23 ();
			}
			if (GUILayout.Button (iconTrunk, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.Trunk ();
			}
			EditorGUI.BeginDisabledGroup (myScript.view_Trunk_Bool);
			if (GUILayout.Button (iconTrunk_Small_Big, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.Trunk_Big_Small ();
			}
			EditorGUI.EndDisabledGroup ();
			GUILayout.EndHorizontal ();
			GUILayout.BeginHorizontal ();
			EditorGUI.BeginDisabledGroup (myScript.view_Trunk_Bool);
			if (GUILayout.Button (iconCowl, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.Cowl ();
			}
			EditorGUI.EndDisabledGroup ();
			if (GUILayout.Button (iconLaght2, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.Light2 ();
			}
			if (GUILayout.Button (iconLight3, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.Light3 ();
			}
			//Color_________________________________________________________________________________________________
			EditorGUI.BeginDisabledGroup (myScript.view_Cabin);
			if (GUILayout.Button (iconColor, GUILayout.Width (123), GUILayout.Height (60))) {
				if (myScript.iconColor_Bool == true) {
					myScript.iconColor_Bool = false;
				} else if (myScript.iconColor_Bool == false) {
					myScript.iconColor_Bool = true;
				}
			}
			EditorGUI.EndDisabledGroup ();
			GUILayout.EndHorizontal ();
		}
		if (myScript.iconColor_Bool == false) {
			GUILayout.BeginHorizontal ();
			if (GUILayout.Button (color_White, GUILayout.Width (40), GUILayout.Height (40))) {
				myScript.colorCabin_Int = 1;
				myScript.CabinMaterial ();
			}
			if (GUILayout.Button (color_Red, GUILayout.Width (40), GUILayout.Height (40))) {
				myScript.colorCabin_Int = 2;
				myScript.CabinMaterial ();
			}
			if (GUILayout.Button (color_Yellow, GUILayout.Width (40), GUILayout.Height (40))) {
				myScript.colorCabin_Int = 3;
				myScript.CabinMaterial ();
			}
			if (GUILayout.Button (color_Brown, GUILayout.Width (40), GUILayout.Height (40))) {
				myScript.colorCabin_Int = 4;
				myScript.CabinMaterial ();
			}
			if (GUILayout.Button (color_Blue, GUILayout.Width (40), GUILayout.Height (40))) {
				myScript.colorCabin_Int = 5;
				myScript.CabinMaterial ();
			}
			if (GUILayout.Button (color_Green, GUILayout.Width (40), GUILayout.Height (40))) {
				myScript.colorCabin_Int = 6;
				myScript.CabinMaterial ();
			}
			GUILayout.EndHorizontal ();
			GUILayout.BeginHorizontal ();
			if (GUILayout.Button (color_White_Plus, GUILayout.Width (40), GUILayout.Height (40))) {
				myScript.colorCabin_Int = 7;
				myScript.CabinMaterial ();
			}
			if (GUILayout.Button (color_Red_Plus, GUILayout.Width (40), GUILayout.Height (40))) {
				myScript.colorCabin_Int = 8;
				myScript.CabinMaterial ();
			}
			if (GUILayout.Button (color_Yellow_Plus, GUILayout.Width (40), GUILayout.Height (40))) {
				myScript.colorCabin_Int = 9;
				myScript.CabinMaterial ();
			}
			if (GUILayout.Button (color_Brown_Plus, GUILayout.Width (40), GUILayout.Height (40))) {
				myScript.colorCabin_Int = 10;
				myScript.CabinMaterial ();
			}
			if (GUILayout.Button (color_Blue_Plus, GUILayout.Width (40), GUILayout.Height (40))) {
				myScript.colorCabin_Int = 11;
				myScript.CabinMaterial ();
			}
			if (GUILayout.Button (color_Green_Plus, GUILayout.Width (40), GUILayout.Height (40))) {
				myScript.colorCabin_Int = 12;
				myScript.CabinMaterial ();
			}
			GUILayout.EndHorizontal ();
		}
		//EQUIPMENT______________________________________________________________________________________'
		EditorGUI.BeginDisabledGroup (myScript.view_Equipment);
		GUILayout.BeginHorizontal ();
		if (GUILayout.Button (icon_Equipment, GUILayout.Width (124), GUILayout.Height (60))) {
			if (myScript.Equipment_Bool == true) {
				myScript.Equipment_Bool = false;
			} else if (myScript.Equipment_Bool == false) {
				myScript.Equipment_Bool = true;
			}
		}
		EditorGUI.EndDisabledGroup ();
		EditorGUI.BeginDisabledGroup (viewController);
		if (GUILayout.Button (icon_Controller, GUILayout.Width (121), GUILayout.Height (60))) {
			if (myScript.controllerTruck_Bool == false && myScript.gameObject.GetComponent<ControllerTruck>() != null) {
				myScript.gameObject.GetComponent<ControllerTruck>().blockManip_Bool = false;
				DestroyImmediate (myScript.gameObject.GetComponent<ControllerTruck> ());
			}
			myScript.ControllerTruck ();
			EditorGUIUtility.ExitGUI ();
		}
		EditorGUI.EndDisabledGroup ();
		GUILayout.EndHorizontal ();
		if (myScript.Equipment_Bool == false) {
			GUILayout.BeginHorizontal ();
			EditorGUI.BeginDisabledGroup (myScript.block_Instantiate_Manip);
			if (GUILayout.Button (icon_Manipulator1, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.Manipulator1 ();
			}
			if (GUILayout.Button (icon_Manipulator2, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.Manipulator2 ();
			}
			if (GUILayout.Button (icon_Manipulator3, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.Manipulator3 ();
			}
			EditorGUI.EndDisabledGroup ();
			EditorGUI.BeginDisabledGroup (myScript.view_Body);
			if (GUILayout.Button (icon_Body, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.Body ();
			}
			EditorGUI.EndDisabledGroup ();
			EditorGUI.BeginDisabledGroup (myScript.body_Bool);
			if (GUILayout.Button (icon_Protection, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.Protection ();
			}
			EditorGUI.EndDisabledGroup ();
			GUILayout.EndHorizontal ();
			GUILayout.BeginHorizontal ();
			EditorGUI.BeginDisabledGroup (myScript.body_Bool);
			if (GUILayout.Button (icon_BodyDop3, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.AddBodyDop3 ();
			}
			EditorGUI.EndDisabledGroup ();
			EditorGUI.BeginDisabledGroup (myScript.view_Crable);
			if (GUILayout.Button (icon_Crable, GUILayout.Width (60), GUILayout.Height (60))) {
				myScript.AddCrable ();
			}
			EditorGUI.EndDisabledGroup ();
			GUILayout.EndHorizontal ();
			if (GUILayout.Button (iconColor, GUILayout.Width (123), GUILayout.Height (60))) {
				if (myScript.color_Equipment == true) {
					myScript.color_Equipment = false;
				} else if (myScript.color_Equipment == false) {
					myScript.color_Equipment = true;
				}
			}
			if (myScript.color_Equipment == false) {
				GUILayout.BeginHorizontal ();
				if (GUILayout.Button (color_Blue, GUILayout.Width (40), GUILayout.Height (40))) {
					myScript.ColorBlue_Manip ();
				}
				if (GUILayout.Button (color_Red, GUILayout.Width (40), GUILayout.Height (40))) {
					myScript.ColorRed_Manip ();
				}
				if (GUILayout.Button (color_White, GUILayout.Width (40), GUILayout.Height (40))) {
					myScript.ColorWhite_Manip ();
				}
				if (GUILayout.Button (color_Yellow, GUILayout.Width (40), GUILayout.Height (40))) {
					myScript.ColorYellow_Manip ();
				}
				GUILayout.EndHorizontal ();
			}
		}
		GUILayout.BeginHorizontal ();
		if (GUILayout.Button (icon_Info, GUILayout.Width (40), GUILayout.Height (40))) {
			if (info == true) {
				info = false;
			} else if (info == false) {
				info = true;
			}
		}
		if (GUILayout.Button (icon_Warning, GUILayout.Width (40), GUILayout.Height (40))) {
			if (warning == true) {
				warning = false;
			} else if (warning == false) {
				warning = true;
			}
		}
		GUILayout.EndHorizontal ();
		if (warning == false) {
			EditorGUILayout.HelpBox ("Do not change the order of the child objects and do not delete the child objects of the [_Point] as this will lead to incorrect operation of the tool.", MessageType.Warning);
		}
		if (info == false) {
			EditorGUILayout.HelpBox ("If you need to add an object to the hierarchy, add it to the end of the hierarchy list. After you have assembled the model you need, you can delete all empty [_point[ child objects. and script [Frame_Truck]", MessageType.Info);
			EditorGUILayout.HelpBox ("For the operation of the UI system, you must add [EventSystem]", MessageType.Info);
		}
		if (GUI.changed) {
			EditorUtility.SetDirty (myScript);
		}
	}
}
