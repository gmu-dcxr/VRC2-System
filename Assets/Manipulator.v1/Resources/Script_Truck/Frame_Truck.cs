using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class Frame_Truck : MonoBehaviour {
	public bool Equipment_Bool = true;
	public bool view_Equipment = true;
	public bool controllerWheel2x2_Bool = true;
	public bool color_Equipment = true;
	//Point Frame Group
	public Transform point_Frame;
	Transform point_Frame_Plus;
	GameObject big_Frame;
	GameObject small_Frame;
	GameObject frame_Plus;
	public bool frameBig_Bool = true;
	public bool frameSmall_Bool = true;
	public bool framePlus_Bool = true;
	public bool view_Frame_Bool = true;
	public bool view_FramePlus = true;
	public bool view_Plus = true;
	public int frame_Int = 0;
	//Point Cabin Group
	Transform point_Cabin;
	GameObject cabin_Big;
	GameObject cabin_Small;
	public bool cabinBig_Bool = true;
	public bool cabinSmall_Bool = true;
	public bool view_Cabin = true;
	public int cabin_Int = 0;
	//Point Panel Cabin Group
	Transform point_PanelCabin;
	GameObject panelCabin_1;
	GameObject panelCabin_2;
	GameObject panelCabin_3;
	GameObject panelCabin_4;
	GameObject panelCabin_5;
	GameObject panelCabin_6;
	GameObject panelCabin_7;
	public int panelCabin_Int = 0;
	public bool iconTuningCabin_Bool = true;
	//Point Bumper Cabin Group
	Transform point_BumperCabin;
	GameObject cabin_Bumper_1;
	GameObject cabin_Bumper_2;
	public	GameObject cabin_Bumper_3;
	public int cabin_Bumper_Int = 0;
	public int materialBumper_Int = 0;
	//Point DSG1 Cabin Group
	Transform point_DSG1_Small;
	Transform point_DSG1_Big;
	GameObject dsg1;
	public bool dsg1_Bool = true;
	//Point DSG 2_3 Cabin Group
	Transform point_DSG23;
	GameObject dsg2_S;
	GameObject dsg3_B;
	public bool dsg23_Bool = true;
	//Point DSG4 Cabin Group
	Transform point_DSG4_Small;
	Transform point_DSG4_Big;
	GameObject dsg4;
	public bool dsg4_Bool = true;
	//Point Trunk Cabin Group
	Transform point_Trunk;
	GameObject trunk;
	public bool trunk_Bool = true;
	public bool view_Trunk_Bool = true;
	//Point TrunkBig_Small Cabin Group
	Transform point_TrunkBig_Small;
	GameObject trunk_Small;
	GameObject trunk_Big;
	public bool trunkBS_Bool = true;
	//Point Cowl Cabin Group
	GameObject cowl;
	public bool cowl_Bool = true;
	//Point Light Cabin Group
	Transform point_Light;
	GameObject light2;
	GameObject light3Big;
	GameObject light3Small;
	public bool light2_Bool = true;
	public bool light3_Bool = true;
	//Material Cabin
	public bool iconColor_Bool = true;
	Material Cabin_Blue_Plus_diffuse_R;
	Material Cabin_Blue_diffuse_R;
	Material Cabin_Brown_Plus_diffuse_R;
	Material Cabin_Brown_diffuse_R;
	Material Cabin_Green_Plus_diffuse_R;
	Material Cabin_Green_diffuse_R;
	Material Cabin_Red_Plus_diffuse_R;
	Material Cabin_Red_diffuse_R;
	Material Cabin_White_Plus_diffuse_R;
	Material Cabin_White_diffuse_R;
	Material Cabin_Yellow_Plus_diffuse_R;
	Material Cabin_Yellow_diffuse_R;
	public int colorCabin_Int = 0;
	//Material Dop
	Material dop_Blue_diffuse_R;
	Material dop_Brown_diffuse_R;
	Material dop_Green_diffuse_R;
	Material dop_Red_diffuse_R;
	Material dop_White_diffuse_R;
	Material dop_Yellow_diffuse_R;
	public int colorDop_Int = 0;
	//Material Panel
	Material Pan_Cabin_Blue_Plus_diffuse_R;
	Material Pan_Cabin_Blue_diffuse_R;
	Material Pan_Cabin_Brown_Plus_diffuse_R;
	Material Pan_Cabin_Brown_diffuse_R;
	Material Pan_Cabin_Green_Plus_diffuse_R;
	Material Pan_Cabin_Green_diffuse_R;
	Material Pan_Cabin_Red_Plus_diffuse_R;
	Material Pan_Cabin_Red_diffuse_R;
	Material Pan_Cabin_White_Plus_diffuse_R;
	Material Pan_Cabin_White_diffuse_R;
	Material Pan_Cabin_Yellow_Plus_diffuse_R;
	Material Pan_Cabin_Yellow_diffuse_R;
	public int colorPanelCabin_Int = 0;
	Material Manipulator_Blue_diffuse_R;
	Material Manipulator_White_diffuse_R;
	Material Manipulator_Red_diffuse_R;
	Material Manipulator_Yellow_diffuse_R;
	Material Manipulator1_Blue_diffuse_R;
	Material Manipulator1_White_diffuse_R;
	Material Manipulator1_Red_diffuse_R;
	Material Manipulator1_Yellow_diffuse_R;
	//Wheel Big Frame
	public Transform Point_bridge_4x4_BigFrame;
	public Transform Point_bridge_2x2_BigFrame;
	public GameObject bridge_4x4_Big;
	public GameObject bridge_2x2_Big;
	public bool bridge_Bool_4x4 = true;
	public int bridge_Int = 0;
	//Wheel Small Frame
	public Transform Point_bridge_2x2_SmallFrame;
	public GameObject bridge_2x2_Small;
	public bool bridge_Bool_2x2 = true;
	public bool iconTuningFrame_Bool = true;
	public bool view_Frame_Tuning = true;
	//Wing
	public Transform point_Wing_Small_FrameBig;
	public Transform point_Wing_Big_FrameBig;
	public Transform point_Wing_FrameSmall;
	public GameObject wingSmall;
	public GameObject wingBig;
	public bool wing_Bool = true;
	public bool view_Wing_Bool = true;
	//Bumper
	public Transform point_Bumper_FrameBig;
	public Transform point_Bumper_FrameSmall;
	public Transform point_Bumper_FramePlus;
	public GameObject bumper1;
	public GameObject bumper2;
	public GameObject bumper3;
	public bool bumper1_Bool = true;
	public bool bumper2_Bool = true;
	public bool bumper3_Bool = true;
	public bool view_Hitch = true;
	//Hitch
	public Transform point_Hitch_FrameBig;
	public Transform point_Hitch_FrameSmall;
	public Transform point_Hitch_FramePlus;
	public GameObject hitch;
	public bool hitch_Bool = true;
	//Point Frame Big
	Transform point_Air_Right_FrameBig;
	Transform point_Air_Left_FrameBig;
	Transform point_Battery_Right_FrameBig;
	Transform point_Battery_Left_FrameBig;
	Transform point_Tank_Left_FrameBig;
	Transform point_Tank_Right_FrameBig;
	Transform point_BoxBig_Left_FrameBig;
	Transform point_BoxBig_Right_FrameBig;
	Transform point_S_Wheel_FrameBig;
	Transform point_BoxSmall_Left_FrameBig;
	Transform point_BoxSmall_Right_FrameBig;
	Transform point_SWheel_Left_A_FrameBig;
	Transform point_SWheel_Left_B_FrameBig;
	//Point Frame Small
	Transform point_BoxSmall_Left_FrameSmall;
	Transform point_BoxSmall_Right_FrameSmall;
	Transform point_S_Whhel_FrameSmall;
	Transform point_S_Whhel_Left_FrameSmall;
	Transform point_Tank_Left_FrameSmall;
	Transform point_Tank_Right_FrameSmall;
	Transform point_BoxBig_Left_FrameSmall;
	Transform point_BoxBig_Right_FrameSmall;
	Transform point_Battery_Left_FrameSmall;
	Transform point_Battery_Right_FrameSmall;
	Transform point_Air_Left_FrameSmall;
	Transform point_Air_Right_FrameSmall;
	//Point Frame Plus
	Transform point_Tank_Wheel_FramePlus;
	Transform point_BoxBig_Left_FramePlus;
	Transform point_BoxBig_Right_FramePlus;
	//Point EQUIPMENT
	Transform point_Manipulator_FrameSmall;
	Transform point_Manipulator_FrameBig_CabSmall;
	Transform point_Manipulator_FrameBig_CabBig;
	//Ari
	GameObject air_Left;
	GameObject air_Right;
	public bool air_Left_Bool = true;
	public bool air_Right_Bool = true;
	//Battery
	GameObject battery_Left;
	GameObject battery_Right;
	public bool battery_Left_Bool = true;
	public bool battery_Right_Bool = true;
	//BoxSmall
	GameObject boxSmall_Left;
	GameObject boxSmall_Right;
	public bool boxSmall_Left_Bool = true;
	public bool boxSmall_Right_Bool = true;
	//Box Big
	GameObject boxBig_Left;
	GameObject boxBig_Right;
	public bool boxBig_L_Bool = true;
	public bool boxBig_R_Bool = true;
	public bool boxBig_L_Plus_Bool = true;
	public bool boxBig_R_Plus_Bool = true;
	//Tank
	GameObject tank;
	GameObject tank_Left;
	GameObject tank_Right;
	public bool tank_Left_Bool = true;
	public bool tank_Right_Bool = true;
	public bool tank_Plus_Bool = true;
	//S Wheel
	GameObject reserveS;
	GameObject reserve_Left;
	public bool reserveS_Bool = true;
	public bool reserve_Left_Bool = true;
	public bool reserve_LeftBack_Bool = true;
	public bool view_Reserve_Left = true;
	public bool reserve_For_Bool = true;
	//EQUIPMENT
	GameObject manipulator1;
	GameObject manipulator2;
	GameObject manipulator3;
	public bool manipulator1_Bool = true;
	public bool manipulator2_Bool = true;
	public bool manipulator3_Bool = true;
	public bool block_Instantiate_Manip = false;
	public bool check_OnBody = true;
	public int colorRed_Manip = 0;
	public Material matManip3;
	//Body
	public bool body_Bool = true;
	public int body_Int = 0;
	public bool view_Body = true;
	Transform point_Body_FrameSmall;
	Transform point_Body_FrameBaig;
	GameObject Body_FrameBig_CabBig;
	GameObject Body_FrameBig_CabBig_Man_Plus;
	GameObject Body_FrameBig_CabBig_Plus;
	GameObject Body_FrameBig_CabSmall;
	GameObject Body_FrameBig_CabSmall_Man;
	GameObject Body_FrameBig_CabSmall_Man_Plus;
	GameObject Body_FrameBig_CabSmall_Plus;
	GameObject Body_FrameBigl_CabBig_Man;
	GameObject Body_FrameSmall_CabBig;
	GameObject Body_FrameSmall_CabSmall;
	GameObject Body_FrameSmall_CabSmall_Man;
	//Protection
	Transform point_Protection;
	GameObject FB_CB_2x2;
	GameObject FB_CB_2x2_M;
	GameObject FB_CB_4x4;
	GameObject FB_CB_4x4_M;
	GameObject FB_CS_2x2;
	GameObject FB_CS_2x2_M;
	GameObject FB_CS_4x4;
	GameObject FB_CS_4x4_M;
	GameObject FS_CB;
	GameObject FS_CS;
	GameObject FS_CS_M;
	public bool protection_Bool = true;
	public int protection_Int = 0;
	//Сardan
	Transform point_Сardan;
	GameObject Сardan_FB_CB;
	GameObject Сardan_FB_CS;
	GameObject Сardan_FS_CB;
	GameObject Сardan_FS_CS;
	//Dop3
	GameObject Cradle;
	GameObject BodyDop3;
	public bool view_Crable = true;
	public bool Cradle_Bool = true;
	public bool BodyDop3_Bool = true;
	public Material matCrable;
	//Controller Truck
	public bool controllerTruck_Bool = true;

	public void Start(){
		//Load Frame Group
		big_Frame = Resources.Load ("Prefab_Truck/Frame_Big", typeof(GameObject))as GameObject;
		small_Frame = Resources.Load ("Prefab_Truck/Frame_Small", typeof(GameObject))as GameObject;
		frame_Plus = Resources.Load ("Prefab_Truck/Frame_Plus", typeof(GameObject))as GameObject;
		point_Frame = transform.GetChild (0);
		point_Frame_Plus = transform.GetChild (1);
		//Load Cabin Group
		cabin_Small = Resources.Load ("Prefab_Truck/Cabin1", typeof(GameObject))as GameObject;
		cabin_Big = Resources.Load ("Prefab_Truck/Cabin2", typeof(GameObject))as GameObject;
		point_Cabin = transform.GetChild (2);
		//Point Panel Cabin Group
		point_PanelCabin = transform.GetChild(3);
		panelCabin_1 = Resources.Load ("Prefab_Truck/PanelCabin_1", typeof(GameObject))as GameObject;
		panelCabin_2 = Resources.Load ("Prefab_Truck/PanelCabin_2", typeof(GameObject))as GameObject;
		panelCabin_3 = Resources.Load ("Prefab_Truck/PanelCabin_3", typeof(GameObject))as GameObject;
		panelCabin_4 = Resources.Load ("Prefab_Truck/PanelCabin_4", typeof(GameObject))as GameObject;
		panelCabin_5 = Resources.Load ("Prefab_Truck/PanelCabin_5", typeof(GameObject))as GameObject;
		panelCabin_6 = Resources.Load ("Prefab_Truck/PanelCabin_6", typeof(GameObject))as GameObject;
		panelCabin_7 = Resources.Load ("Prefab_Truck/PanelCabin_7", typeof(GameObject))as GameObject;
		//Point Bumper Cabin Group
		point_BumperCabin = transform.GetChild(4);
		cabin_Bumper_1 = Resources.Load ("Prefab_Truck/F_Bumper1", typeof(GameObject))as GameObject;
		cabin_Bumper_2 = Resources.Load ("Prefab_Truck/F_Bumper2", typeof(GameObject))as GameObject;
		cabin_Bumper_3 = Resources.Load ("Prefab_Truck/F_Bumper3", typeof(GameObject))as GameObject;
		//Point DSG1 Cabin Group
		point_DSG1_Small = transform.GetChild(8);
		point_DSG1_Big = transform.GetChild (9);
		dsg1 = Resources.Load ("Prefab_Truck/DSG1", typeof(GameObject))as GameObject;
		//Point DSG 2_3 Cabin Group
		point_DSG23 = transform.GetChild(10);
		dsg2_S = Resources.Load ("Prefab_Truck/DSG2", typeof(GameObject))as GameObject;
		dsg3_B = Resources.Load ("Prefab_Truck/DSG3", typeof(GameObject))as GameObject;
		//Point DSG4 Cabin Group
		point_DSG4_Small = transform.GetChild(6);
		point_DSG4_Big = transform.GetChild (7);
		dsg4 = Resources.Load ("Prefab_Truck/DSG4", typeof(GameObject))as GameObject;
		//Point Trunk Cabin Group
		point_Trunk = transform.GetChild(11);
		trunk = Resources.Load ("Prefab_Truck/Trunk", typeof(GameObject))as GameObject;
		//Point TrunkBig_Small Cabin Group
		point_TrunkBig_Small = transform.GetChild(12);
		trunk_Small = Resources.Load ("Prefab_Truck/Trunk_Small", typeof(GameObject))as GameObject;
		trunk_Big = Resources.Load ("Prefab_Truck/Trunk_Big", typeof(GameObject))as GameObject;
		cowl = Resources.Load ("Prefab_Truck/Cowl", typeof(GameObject))as GameObject;
		//Point Light Cabin Group
		point_Light = transform.GetChild(5);
		light2 = Resources.Load ("Prefab_Truck/Light_2", typeof(GameObject))as GameObject;
		light3Big = Resources.Load ("Prefab_Truck/Light_3Big", typeof(GameObject))as GameObject;
		light3Small = Resources.Load ("Prefab_Truck/Light_3Small", typeof(GameObject))as GameObject;
		//Wheel Big Frame
		Point_bridge_4x4_BigFrame = transform.GetChild(13);
		Point_bridge_2x2_BigFrame = transform.GetChild (14);
		bridge_4x4_Big = Resources.Load ("Prefab_Truck/Bridge_Back_B", typeof(GameObject))as GameObject;
		bridge_2x2_Big = Resources.Load ("Prefab_Truck/Bridge_Back_D", typeof(GameObject))as GameObject;
		//Wheel Small Frame
		Point_bridge_2x2_SmallFrame = transform.GetChild(15);
		bridge_2x2_Small = Resources.Load ("Prefab_Truck/Bridge_Back_F", typeof(GameObject))as GameObject;
		//Wing
		point_Wing_Small_FrameBig = transform.GetChild(16);
		point_Wing_Big_FrameBig = transform.GetChild(17);
		point_Wing_FrameSmall = transform.GetChild(18);
		wingSmall = Resources.Load ("Prefab_Truck/Wing_Small", typeof(GameObject))as GameObject;
		wingBig = Resources.Load ("Prefab_Truck/Wing_Big", typeof(GameObject))as GameObject;
		//Bumper
		point_Bumper_FrameBig = transform.GetChild(19);
		point_Bumper_FrameSmall = transform.GetChild(20);
		point_Bumper_FramePlus = transform.GetChild (24);
		bumper1 = Resources.Load ("Prefab_Truck/Bumper1", typeof(GameObject))as GameObject;
		bumper2 = Resources.Load ("Prefab_Truck/Bumper2", typeof(GameObject))as GameObject;
		bumper3 = Resources.Load ("Prefab_Truck/Bumper3", typeof(GameObject))as GameObject;
		//Hitch
		point_Hitch_FrameBig = transform.GetChild(22);
		point_Hitch_FrameSmall = transform.GetChild(21);
		point_Hitch_FramePlus = transform.GetChild (23);
		hitch = Resources.Load ("Prefab_Truck/Hitch", typeof(GameObject))as GameObject;
		//Body
		Body_FrameBig_CabBig = Resources.Load ("Prefab_Equipment/Body_FrameBig_CabBig", typeof(GameObject))as GameObject;
		Body_FrameBig_CabBig_Man_Plus = Resources.Load ("Prefab_Equipment/Body_FrameBig_CabBig_Man_Plus", typeof(GameObject))as GameObject;
		Body_FrameBig_CabBig_Plus = Resources.Load ("Prefab_Equipment/Body_FrameBig_CabBig_Plus", typeof(GameObject))as GameObject;
		Body_FrameBig_CabSmall = Resources.Load ("Prefab_Equipment/Body_FrameBig_CabSmall", typeof(GameObject))as GameObject;
		Body_FrameBig_CabSmall_Man = Resources.Load ("Prefab_Equipment/Body_FrameBig_CabSmall_Man", typeof(GameObject))as GameObject;
		Body_FrameBig_CabSmall_Man_Plus = Resources.Load ("Prefab_Equipment/Body_FrameBig_CabSmall_Man_Plus", typeof(GameObject))as GameObject;
		Body_FrameBig_CabSmall_Plus = Resources.Load ("Prefab_Equipment/Body_FrameBig_CabSmall_Plus", typeof(GameObject))as GameObject;
		Body_FrameBigl_CabBig_Man = Resources.Load ("Prefab_Equipment/Body_FrameBigl_CabBig_Man", typeof(GameObject))as GameObject;
		Body_FrameSmall_CabBig = Resources.Load ("Prefab_Equipment/Body_FrameSmall_CabBig", typeof(GameObject))as GameObject;
		Body_FrameSmall_CabSmall = Resources.Load ("Prefab_Equipment/Body_FrameSmall_CabSmall", typeof(GameObject))as GameObject;
		Body_FrameSmall_CabSmall_Man = Resources.Load ("Prefab_Equipment/Body_FrameSmall_CabSmall_Man", typeof(GameObject))as GameObject;
		//Crable
		Cradle = Resources.Load ("Prefab_Equipment/Cradle", typeof(GameObject))as GameObject;
		//BodyDop3
		BodyDop3 = Resources.Load ("Prefab_Equipment/BodyDop3", typeof(GameObject))as GameObject;
		//Point Frame Big
		point_Air_Right_FrameBig = transform.GetChild(25);
		point_Air_Left_FrameBig = transform.GetChild(26);
		point_Battery_Right_FrameBig = transform.GetChild(27);
		point_Battery_Left_FrameBig = transform.GetChild(28);
		point_Tank_Left_FrameBig = transform.GetChild(29);
		point_Tank_Right_FrameBig = transform.GetChild(30);
		point_BoxBig_Left_FrameBig = transform.GetChild(31);
		point_BoxBig_Right_FrameBig = transform.GetChild(32);
		point_S_Wheel_FrameBig = transform.GetChild(33);
		point_BoxSmall_Left_FrameBig = transform.GetChild(34);
		point_BoxSmall_Right_FrameBig = transform.GetChild(35);
		point_SWheel_Left_A_FrameBig = transform.GetChild(36);
		point_SWheel_Left_B_FrameBig = transform.GetChild(37);
		//Point Frame Small
		point_BoxSmall_Left_FrameSmall = transform.GetChild(38);
		point_BoxSmall_Right_FrameSmall = transform.GetChild(39);
		point_S_Whhel_FrameSmall = transform.GetChild(40);
		point_S_Whhel_Left_FrameSmall = transform.GetChild(41);
		point_Tank_Left_FrameSmall = transform.GetChild(42);
		point_Tank_Right_FrameSmall = transform.GetChild(43);
		point_BoxBig_Left_FrameSmall = transform.GetChild(44);
		point_BoxBig_Right_FrameSmall = transform.GetChild(45);
		point_Battery_Left_FrameSmall = transform.GetChild(46);
		point_Battery_Right_FrameSmall = transform.GetChild(47);
		point_Air_Left_FrameSmall = transform.GetChild(48);
		point_Air_Right_FrameSmall = transform.GetChild(49);
		//Point Frame Plus
		point_Tank_Wheel_FramePlus = transform.GetChild(50);
		point_BoxBig_Left_FramePlus = transform.GetChild(51);
		point_BoxBig_Right_FramePlus = transform.GetChild(52);
		//EQUIPMENT
		point_Manipulator_FrameSmall = transform.GetChild(53);
		point_Manipulator_FrameBig_CabSmall = transform.GetChild(55);
		point_Manipulator_FrameBig_CabBig = transform.GetChild(54);
		//Body
		point_Body_FrameSmall = transform.GetChild(57);
		point_Body_FrameBaig = transform.GetChild(56);
		//Protection
		point_Protection = transform.GetChild(58);
		//Cardan
		point_Сardan = transform.GetChild(59);
		//Ari
		air_Left = Resources.Load ("Prefab_Truck/Air_L", typeof(GameObject))as GameObject;
		air_Right = Resources.Load ("Prefab_Truck/Air_R", typeof(GameObject))as GameObject;
		//Battery
		battery_Left = Resources.Load ("Prefab_Truck/Battery_L", typeof(GameObject))as GameObject;
		battery_Right = Resources.Load ("Prefab_Truck/Battery_R", typeof(GameObject))as GameObject;
		//BoxSmall
		boxSmall_Left = Resources.Load ("Prefab_Truck/Box_Left_Small", typeof(GameObject))as GameObject;
		boxSmall_Right = Resources.Load ("Prefab_Truck/Box_Right_Small", typeof(GameObject))as GameObject;
		boxBig_Left = Resources.Load ("Prefab_Truck/Box_Big_L", typeof(GameObject))as GameObject;
		boxBig_Right = Resources.Load ("Prefab_Truck/Box_Big_R", typeof(GameObject))as GameObject;
		//Tank
		tank = Resources.Load ("Prefab_Truck/Tank", typeof(GameObject))as GameObject;
		tank_Left = Resources.Load ("Prefab_Truck/Tank_L", typeof(GameObject))as GameObject;
		tank_Right = Resources.Load ("Prefab_Truck/Tank_R", typeof(GameObject))as GameObject;
		//Reserve
		reserveS = Resources.Load ("Prefab_Truck/Spare wheel_B", typeof(GameObject))as GameObject;
		reserve_Left = Resources.Load ("Prefab_Truck/Spare wheel_A_L", typeof(GameObject))as GameObject;
		//EQUIPMENT
		manipulator1 = Resources.Load ("Prefab_Equipment/Manipulator_1", typeof(GameObject))as GameObject;
		manipulator2 = Resources.Load ("Prefab_Equipment/Manipulator_2", typeof(GameObject))as GameObject;
		manipulator3 = Resources.Load ("Prefab_Equipment/Manipulator_3", typeof(GameObject))as GameObject;
		//Materials
		Cabin_Blue_Plus_diffuse_R = Resources.Load ("Material/Cabin_Blue(Plus)_diffuse_R", typeof(Material))as Material;
		Cabin_Blue_diffuse_R = Resources.Load ("Material/Cabin_Blue_diffuse_R", typeof(Material))as Material;
		Cabin_Brown_Plus_diffuse_R = Resources.Load ("Material/Cabin_Brown(Plus)_diffuse_R", typeof(Material))as Material;
		Cabin_Brown_diffuse_R = Resources.Load ("Material/Cabin_Brown_diffuse_R", typeof(Material))as Material;
		Cabin_Green_Plus_diffuse_R = Resources.Load ("Material/Cabin_Green(Plus)_diffuse_R", typeof(Material))as Material;
		Cabin_Green_diffuse_R = Resources.Load ("Material/Cabin_Green_diffuse_R", typeof(Material))as Material;
		Cabin_Red_Plus_diffuse_R = Resources.Load ("Material/Cabin_Red(Plus)_diffuse_R", typeof(Material))as Material;
		Cabin_Red_diffuse_R = Resources.Load ("Material/Cabin_Red_diffuse_R", typeof(Material))as Material;
		Cabin_White_Plus_diffuse_R = Resources.Load ("Material/Cabin_White(Plus)_diffuse_R", typeof(Material))as Material;
		Cabin_White_diffuse_R = Resources.Load ("Material/Cabin_White_diffuse_R", typeof(Material))as Material;
		Cabin_Yellow_Plus_diffuse_R = Resources.Load ("Material/Cabin_Yellow(Plus)_diffuse_R", typeof(Material))as Material;
		Cabin_Yellow_diffuse_R = Resources.Load ("Material/Cabin_Yellow_diffuse_R", typeof(Material))as Material;
		dop_Blue_diffuse_R = Resources.Load ("Material/Dop_Blue_diffuse_R", typeof(Material))as Material;
		dop_Brown_diffuse_R = Resources.Load ("Material/Dop_Brown_diffuse_R", typeof(Material))as Material;
		dop_Green_diffuse_R = Resources.Load ("Material/Dop_Green_diffuse_R", typeof(Material))as Material;
		dop_Red_diffuse_R = Resources.Load ("Material/Dop_Red_diffuse_R", typeof(Material))as Material;
		dop_White_diffuse_R = Resources.Load ("Material/Dop_White_diffuse_R", typeof(Material))as Material;
		dop_Yellow_diffuse_R = Resources.Load ("Material/Dop_Yellow_diffuse_R", typeof(Material))as Material;
		Pan_Cabin_Blue_Plus_diffuse_R = Resources.Load ("Material/Panel_Cabin_Blue(Plus)_diffuse_R", typeof(Material))as Material;
		Pan_Cabin_Blue_diffuse_R = Resources.Load ("Material/Panel_Cabin_Blue_diffuse_R", typeof(Material))as Material;
		Pan_Cabin_Brown_Plus_diffuse_R = Resources.Load ("Material/Panel_Cabin_Brown(Plus)_diffuse_R", typeof(Material))as Material;
		Pan_Cabin_Brown_diffuse_R = Resources.Load ("Material/Panel_Cabin_Brown_diffuse_R", typeof(Material))as Material;
		Pan_Cabin_Green_Plus_diffuse_R = Resources.Load ("Material/Panel_Cabin_Green(Plus)_diffuse_R", typeof(Material))as Material;
		Pan_Cabin_Green_diffuse_R = Resources.Load ("Material/Panel_Cabin_Green_diffuse_R", typeof(Material))as Material;
		Pan_Cabin_Red_Plus_diffuse_R = Resources.Load ("Material/Panel_Cabin_Red(Plus)_diffuse_R", typeof(Material))as Material;
		Pan_Cabin_Red_diffuse_R = Resources.Load ("Material/Panel_Cabin_Red_diffuse_R", typeof(Material))as Material;
		Pan_Cabin_White_Plus_diffuse_R = Resources.Load ("Material/Panel_Cabin_White(Plus)_diffuse_R", typeof(Material))as Material;
		Pan_Cabin_White_diffuse_R = Resources.Load ("Material/Panel_Cabin_White_diffuse_R", typeof(Material))as Material;
		Pan_Cabin_Yellow_Plus_diffuse_R = Resources.Load ("Material/Panel_Cabin_Yellow(Plus)_diffuse_R", typeof(Material))as Material;
		Pan_Cabin_Yellow_diffuse_R = Resources.Load ("Material/Panel_Cabin_Yellow_diffuse_R", typeof(Material))as Material;
		//EQUIPMENT
		Manipulator_Blue_diffuse_R = Resources.Load ("Material/Manip_Blue_diffuse_R", typeof(Material))as Material;
		Manipulator_White_diffuse_R = Resources.Load ("Material/Manip_White_diffuse_R", typeof(Material))as Material;
		Manipulator_Red_diffuse_R = Resources.Load ("Material/Manip_Red_diffuse_R", typeof(Material))as Material;
		Manipulator_Yellow_diffuse_R = Resources.Load ("Material/Manip_Yellow_diffuse_R", typeof(Material))as Material;
		Manipulator1_Blue_diffuse_R = Resources.Load ("Material/Manip1_Blue_diffuse_R", typeof(Material))as Material;
		Manipulator1_White_diffuse_R = Resources.Load ("Material/Manip1_White_diffuse_R", typeof(Material))as Material;
		Manipulator1_Red_diffuse_R = Resources.Load ("Material/Manip1_Red_diffuse_R", typeof(Material))as Material;
		Manipulator1_Yellow_diffuse_R = Resources.Load ("Material/Manip1_Yellow_diffuse_R", typeof(Material))as Material;
		matCrable = Resources.Load ("Material/Dop3_diffuse_Edit", typeof(Material))as Material;
		//Protection
		FB_CB_2x2 = Resources.Load ("Prefab_Equipment/FB_CB_2x2", typeof(GameObject))as GameObject;
		FB_CB_2x2_M = Resources.Load ("Prefab_Equipment/FB_CB_2x2_M", typeof(GameObject))as GameObject;
		FB_CB_4x4 = Resources.Load ("Prefab_Equipment/FB_CB_4x4", typeof(GameObject))as GameObject;
		FB_CB_4x4_M = Resources.Load ("Prefab_Equipment/FB_CB_4x4_M", typeof(GameObject))as GameObject;
		FB_CS_2x2 = Resources.Load ("Prefab_Equipment/FB_CS_2x2", typeof(GameObject))as GameObject;
		FB_CS_2x2_M = Resources.Load ("Prefab_Equipment/FB_CS_2x2_M", typeof(GameObject))as GameObject;
		FB_CS_4x4 = Resources.Load ("Prefab_Equipment/FB_CS_4x4", typeof(GameObject))as GameObject;
		FB_CS_4x4_M = Resources.Load ("Prefab_Equipment/FB_CS_4x4_M", typeof(GameObject))as GameObject;
		FS_CB = Resources.Load ("Prefab_Equipment/FS_CB", typeof(GameObject))as GameObject;
		FS_CS = Resources.Load ("Prefab_Equipment/FS_CS", typeof(GameObject))as GameObject;
		FS_CS_M = Resources.Load ("Prefab_Equipment/FS_CS_M", typeof(GameObject))as GameObject;
		//Сardan
		Сardan_FB_CB = Resources.Load ("Prefab_Truck/Сardan_FB_CB", typeof(GameObject))as GameObject;
		Сardan_FB_CS = Resources.Load ("Prefab_Truck/Сardan_FB_CS", typeof(GameObject))as GameObject;
		Сardan_FS_CB = Resources.Load ("Prefab_Truck/Сardan_FS_CB", typeof(GameObject))as GameObject;
		Сardan_FS_CS = Resources.Load ("Prefab_Truck/Сardan_FS_CS", typeof(GameObject))as GameObject;
	}
	//Frame_________________________________________________________________________________________________
	public void FrameBig(){
		if (frameBig_Bool == true) {
			if (frameSmall_Bool == false) {
				foreach (Transform obj_SmallFrame in point_Frame) {
					DestroyImmediate (point_Frame.GetChild (0).gameObject);
				}
				frameSmall_Bool = true;
				DestroyObj ();
			}
			var obj_BigFrame = Instantiate (big_Frame, point_Frame.position, Quaternion.identity);
			obj_BigFrame.transform.parent = point_Frame;
			frame_Int = 1;
			view_FramePlus = false;
			frameBig_Bool = false;
			view_Frame_Tuning = false;
			//Bumper
			if (point_Bumper_FramePlus.childCount > 0) {
				foreach (Transform obj_D1 in point_Bumper_FramePlus) {
					DestroyImmediate (point_Bumper_FramePlus.GetChild (0).gameObject);
				}
				if (bumper1_Bool == false) {
					var obj_B1 = Instantiate (bumper1, point_Bumper_FrameBig.position, Quaternion.identity);
					obj_B1.transform.parent = point_Bumper_FrameBig;
				} else if (bumper2_Bool == false) {
					var obj_B2 = Instantiate (bumper2, point_Bumper_FrameBig.position, Quaternion.identity);
					obj_B2.transform.parent = point_Bumper_FrameBig;
				} else if (bumper3_Bool == false) {
					var obj_B3 = Instantiate (bumper3, point_Bumper_FrameBig.position, Quaternion.identity);
					obj_B3.transform.parent = point_Bumper_FrameBig;
				}
			} else if (point_Bumper_FrameSmall.childCount > 0) {
				foreach (Transform obj_D1 in point_Bumper_FrameSmall) {
					DestroyImmediate (point_Bumper_FrameSmall.GetChild (0).gameObject);
				}
				if (bumper1_Bool == false) {
					var obj_B1 = Instantiate (bumper1, point_Bumper_FrameBig.position, Quaternion.identity);
					obj_B1.transform.parent = point_Bumper_FrameBig;
				} else if (bumper2_Bool == false) {
					var obj_B2 = Instantiate (bumper2, point_Bumper_FrameBig.position, Quaternion.identity);
					obj_B2.transform.parent = point_Bumper_FrameBig;
				} else if (bumper3_Bool == false) {
					var obj_B3 = Instantiate (bumper3, point_Bumper_FrameBig.position, Quaternion.identity);
					obj_B3.transform.parent = point_Bumper_FrameBig;
				}
			}
			//Hitch
			if (hitch_Bool == false) {
				NextHitch ();
			}
		//Bridge
			if (bridge_Bool_2x2 == false) {
				foreach (Transform obj_D1 in Point_bridge_2x2_SmallFrame) {
					DestroyImmediate (Point_bridge_2x2_SmallFrame.GetChild (0).gameObject);
				}
				var obj_B1 = Instantiate (bridge_2x2_Big, Point_bridge_2x2_BigFrame.position, Quaternion.identity);
				obj_B1.transform.parent = Point_bridge_2x2_BigFrame;
				view_Reserve_Left = false;
				//Wing
				if (point_Wing_FrameSmall.childCount > 0) {
					foreach (Transform obj_DW1 in point_Wing_FrameSmall) {
						DestroyImmediate (point_Wing_FrameSmall.GetChild (0).gameObject);
					}
					var obj_Wing1 = Instantiate (wingSmall, point_Wing_Small_FrameBig.position, Quaternion.identity);
					obj_Wing1.transform.parent = point_Wing_Small_FrameBig;
				}
			}
			view_Equipment = false;
			//Manipulator
			if (point_Manipulator_FrameSmall.childCount > 0) {
				foreach (Transform obj_ManipD in point_Manipulator_FrameSmall) {
					DestroyImmediate (point_Manipulator_FrameSmall.GetChild (0).gameObject);
				}
				if (manipulator1_Bool == false) {
					var obj_Manip1 = Instantiate (manipulator1, point_Manipulator_FrameBig_CabSmall.position, Quaternion.identity);
					obj_Manip1.transform.parent = point_Manipulator_FrameBig_CabSmall;
				} else if (manipulator2_Bool == false) {
					var obj_Manip2 = Instantiate (manipulator2, point_Manipulator_FrameBig_CabSmall.position, Quaternion.identity);
					obj_Manip2.transform.parent = point_Manipulator_FrameBig_CabSmall;
				}
				else if (manipulator3_Bool == false) {
					var obj_Manip3 = Instantiate (manipulator3, point_Manipulator_FrameBig_CabSmall.position, Quaternion.identity);
					obj_Manip3.transform.parent = point_Manipulator_FrameBig_CabSmall;
				}
				if (colorRed_Manip != 0) {
					if (colorRed_Manip == 1) {
						ColorBlue_Manip ();
					} else if (colorRed_Manip == 2) {
						ColorRed_Manip ();
					} else if (colorRed_Manip == 3) {
						ColorWhite_Manip ();
					} else if (colorRed_Manip == 4) {
						ColorYellow_Manip ();
					}
				}
			}
			//Block Instantiate Manipulator
			block_Instantiate_Manip = false;
			//Body
			if (body_Bool == false) {
				GreateBody ();
			}
			//Protection
			if (protection_Bool == false) {
				GreateProtection ();
			}
			//Cardan
			Cardan();
		//Add Transform
			if (gameObject.GetComponent<ControllerTruck> () != null) {
				GreateController ();
			}
			ManipulatorController ();
		//Check Manip
			CheckManipulator();
			//Check Crable
			CheckCrable ();
			AddConponentArrow ();
			//Add Target Camera
			transform.GetChild (62).gameObject.GetComponent<CameraToManipulator> ().target = transform;
		} else if (frameBig_Bool == false) {
			iconTuningFrame_Bool = true;
			DestroyObj ();
			foreach (Transform obj_BigFrame in point_Frame) {
				DestroyImmediate (point_Frame.GetChild (0).gameObject);
			}
			//Frame Plus
			if (framePlus_Bool == false) {
				foreach (Transform obj_FramePlus in point_Frame_Plus) {
					DestroyImmediate (point_Frame_Plus.GetChild (0).gameObject);
				}
				if (framePlus_Bool == false && bumper1_Bool == false) {
					foreach (Transform obj_D1 in point_Bumper_FramePlus) {
						DestroyImmediate (point_Bumper_FramePlus.GetChild (0).gameObject);
					}
					if (bumper1_Bool == false) {
						bumper1_Bool = true;
						view_Hitch = true;
					}
				}
				if (framePlus_Bool == false && bumper2_Bool == false) {
					foreach (Transform obj_D2 in point_Bumper_FramePlus) {
						DestroyImmediate (point_Bumper_FramePlus.GetChild (0).gameObject);
					}
					if (bumper2_Bool == false) {
						bumper2_Bool = true;
						view_Hitch = true;
					}
				}
				if (framePlus_Bool == false && bumper3_Bool == false) {
					foreach (Transform obj_D2 in point_Bumper_FramePlus) {
						DestroyImmediate (point_Bumper_FramePlus.GetChild (0).gameObject);
					}
					if (bumper3_Bool == false) {
						bumper3_Bool = true;
						view_Hitch = true;
					}
				}
				framePlus_Bool = true;
				view_Plus = true;
			}
			view_FramePlus = true;
			frameBig_Bool = true;
			view_Frame_Tuning = true;
			//Bumper
			if (point_Bumper_FrameBig.childCount > 0) {
				foreach (Transform obj_D1 in point_Bumper_FrameBig) {
					DestroyImmediate (point_Bumper_FrameBig.GetChild (0).gameObject);
				}
				if (bumper1_Bool == false) {
					bumper1_Bool = true;
					view_Hitch = true;
				} else if (bumper2_Bool == false) {
					bumper2_Bool = true;
					view_Hitch = true;
				} else if (bumper3_Bool == false) {
					bumper3_Bool = true;
					view_Hitch = true;
				}
			}
			//Hitch
			if (hitch_Bool == false) {
				foreach (Transform obj_D1 in point_Hitch_FrameBig) {
					DestroyImmediate (point_Hitch_FrameBig.GetChild (0).gameObject);
				}
				hitch_Bool = true;
			}
		//Destroy Bridge 2x2 end 4x4
			if (Point_bridge_2x2_BigFrame.childCount > 0) {
				foreach (Transform obj_DB1 in Point_bridge_2x2_BigFrame) {
					DestroyImmediate (Point_bridge_2x2_BigFrame.GetChild (0).gameObject);
				}
				bridge_Bool_2x2 = true;
				bridge_Int = 0;
				view_Reserve_Left = true;
				view_Wing_Bool = true;
			} else if (Point_bridge_4x4_BigFrame.childCount > 0) {
				foreach (Transform obj_DB2 in Point_bridge_4x4_BigFrame) {
					DestroyImmediate (Point_bridge_4x4_BigFrame.GetChild (0).gameObject);
				}
				bridge_Bool_4x4 = true;
				bridge_Int = 0;
				view_Wing_Bool = true;
			}
			//Destoy Wing
			if (point_Wing_Big_FrameBig.childCount > 0) {
				foreach (Transform obj_DW1 in point_Wing_Big_FrameBig) {
					DestroyImmediate (point_Wing_Big_FrameBig.GetChild (0).gameObject);
				}
				wing_Bool = true;
			} else if (point_Wing_Small_FrameBig.childCount > 0) {
				foreach (Transform obj_DW2 in point_Wing_Small_FrameBig) {
					DestroyImmediate (point_Wing_Small_FrameBig.GetChild (0).gameObject);
				}
				wing_Bool = true;
			}
			frame_Int = 0;
			Equipment_Bool = true;
			view_Equipment = true;
		//Destroy Manipulator
			if (point_Manipulator_FrameBig_CabSmall.childCount > 0) {
				foreach (Transform obj_Manip1 in point_Manipulator_FrameBig_CabSmall) {
					DestroyImmediate (point_Manipulator_FrameBig_CabSmall.GetChild (0).gameObject);
				}
			} else if (point_Manipulator_FrameBig_CabBig.childCount > 0) {
				foreach (Transform obj_Manip2 in point_Manipulator_FrameBig_CabBig) {
					DestroyImmediate (point_Manipulator_FrameBig_CabBig.GetChild (0).gameObject);
				}
			}
			if (manipulator1_Bool == false) {
				manipulator1_Bool = true;
			} else if (manipulator2_Bool == false) {
				manipulator2_Bool = true;
			}else if (manipulator3_Bool == false) {
				manipulator3_Bool = true;
			}
			if (view_Crable == false) {
				view_Crable = true;
			}
			//Destroy Body
			if (body_Bool == false) {
				Body ();
			}
			//Protection
			if (protection_Bool == false) {
				Protection ();
			}
			//Destroy Cardan
			foreach (Transform obj_Car in point_Сardan) {
				DestroyImmediate (point_Сardan.GetChild (0).gameObject);
			}
			//Block Manip
			if (controllerTruck_Bool == false) {
				gameObject.GetComponent<ControllerTruck> ().blockManip_Bool = false;
			}
		//Block Start Truck
			if(controllerTruck_Bool == false){
				gameObject.GetComponent<ControllerTruck> ().startTruck = false;
			}
			if (check_OnBody == false) {
				check_OnBody = true;
			}
			if (BodyDop3_Bool == false) {
				BodyDop3_Bool = true;
			}
			if (Cradle_Bool == false) {
				Cradle_Bool = true;
			}
		}
}
	public void FrameSmall(){
		if (frameSmall_Bool == true) {
			if (frameBig_Bool == false) {
				foreach (Transform obj_BigFrame in point_Frame) {
					DestroyImmediate (point_Frame.GetChild (0).gameObject);
				}
				DestroyObj ();
				//Frame Plus
				if (framePlus_Bool == false) {
					foreach (Transform obj_FramePlus in point_Frame_Plus) {
						DestroyImmediate (point_Frame_Plus.GetChild (0).gameObject);
					}
					if (framePlus_Bool == false && bumper1_Bool == false) {
						foreach (Transform obj_D1 in point_Bumper_FramePlus) {
							DestroyImmediate (point_Bumper_FramePlus.GetChild (0).gameObject);
						}
						var obj_B1 = Instantiate (bumper1, point_Bumper_FrameSmall.position, Quaternion.identity);
						obj_B1.transform.parent = point_Bumper_FrameSmall;
					}
					if (framePlus_Bool == false && bumper2_Bool == false) {
						foreach (Transform obj_D2 in point_Bumper_FramePlus) {
							DestroyImmediate (point_Bumper_FramePlus.GetChild (0).gameObject);
						}
						var obj_B2 = Instantiate (bumper2, point_Bumper_FrameSmall.position, Quaternion.identity);
						obj_B2.transform.parent = point_Bumper_FrameSmall;
					}
					if (framePlus_Bool == false && bumper3_Bool == false) {
						foreach (Transform obj_D2 in point_Bumper_FramePlus) {
							DestroyImmediate (point_Bumper_FramePlus.GetChild (0).gameObject);
						}
						var obj_B3 = Instantiate (bumper3, point_Bumper_FrameSmall.position, Quaternion.identity);
						obj_B3.transform.parent = point_Bumper_FrameSmall;
					}
					framePlus_Bool = true;
					view_Plus = true;
				}
				//Destroy Box Big Plus
				if (boxBig_L_Plus_Bool == false) {
					foreach (Transform obj_DB1 in point_BoxBig_Left_FramePlus) {
						DestroyImmediate (point_BoxBig_Left_FramePlus.GetChild (0).gameObject);
					}
					boxBig_L_Plus_Bool = true;
				}
				if (boxBig_R_Plus_Bool == false) {
					foreach (Transform obj_DB1 in point_BoxBig_Right_FramePlus) {
						DestroyImmediate (point_BoxBig_Right_FramePlus.GetChild (0).gameObject);
					}
					boxBig_R_Plus_Bool = true;
				}
				view_FramePlus = true;
				frameBig_Bool = true;
			}
			var obj_SmallFrame = Instantiate (small_Frame, point_Frame.position, Quaternion.identity);
			obj_SmallFrame.transform.parent = point_Frame;
			frame_Int = 2;
			frameSmall_Bool = false;
			view_Frame_Tuning = false;
			//Bumper
			if (point_Bumper_FramePlus.childCount > 0) {
				foreach (Transform obj_D1 in point_Bumper_FramePlus) {
					DestroyImmediate (point_Bumper_FramePlus.GetChild (0).gameObject);
				}
				if (bumper1_Bool == false) {
					var obj_B1 = Instantiate (bumper1, point_Bumper_FrameBig.position, Quaternion.identity);
					obj_B1.transform.parent = point_Bumper_FrameBig;
				} else if (bumper2_Bool == false) {
					var obj_B2 = Instantiate (bumper2, point_Bumper_FrameBig.position, Quaternion.identity);
					obj_B2.transform.parent = point_Bumper_FrameBig;
				} else if (bumper3_Bool == false) {
					var obj_B3 = Instantiate (bumper3, point_Bumper_FrameBig.position, Quaternion.identity);
					obj_B3.transform.parent = point_Bumper_FrameBig;
				}
			} else if (point_Bumper_FrameBig.childCount > 0) {
				foreach (Transform obj_D1 in point_Bumper_FrameBig) {
					DestroyImmediate (point_Bumper_FrameBig.GetChild (0).gameObject);
				}
				if (bumper1_Bool == false) {
					var obj_B1 = Instantiate (bumper1, point_Bumper_FrameSmall.position, Quaternion.identity);
					obj_B1.transform.parent = point_Bumper_FrameSmall;
				} else if (bumper2_Bool == false) {
					var obj_B2 = Instantiate (bumper2, point_Bumper_FrameSmall.position, Quaternion.identity);
					obj_B2.transform.parent = point_Bumper_FrameSmall;
				} else if (bumper3_Bool == false) {
					var obj_B3 = Instantiate (bumper3, point_Bumper_FrameSmall.position, Quaternion.identity);
					obj_B3.transform.parent = point_Bumper_FrameSmall;
				}
			}
			//Hitch
			if (hitch_Bool == false) {
				NextHitch ();
			}
		//Bridge
			if (bridge_Bool_2x2 == false && bridge_Bool_4x4 == true) {
				foreach (Transform obj_D1 in Point_bridge_2x2_BigFrame) {
					DestroyImmediate (Point_bridge_2x2_BigFrame.GetChild (0).gameObject);
				}
				view_Reserve_Left = true;
				var obj_B1 = Instantiate (bridge_2x2_Small, Point_bridge_2x2_SmallFrame.position, Quaternion.identity);
				obj_B1.transform.parent = Point_bridge_2x2_SmallFrame;
			//Wing
				if (point_Wing_Small_FrameBig.childCount > 0) {
					foreach (Transform obj_DW1 in point_Wing_Small_FrameBig) {
						DestroyImmediate (point_Wing_Small_FrameBig.GetChild (0).gameObject);
					}
					var obj_Wing1 = Instantiate (wingSmall, point_Wing_FrameSmall.position, Quaternion.identity);
					obj_Wing1.transform.parent = point_Wing_FrameSmall;
				}
				//Reserve Left
				if(point_SWheel_Left_A_FrameBig.childCount > 0){
					foreach (Transform obj_D in point_SWheel_Left_A_FrameBig) {
						DestroyImmediate (point_SWheel_Left_A_FrameBig.GetChild (0).gameObject);
					}
					reserve_LeftBack_Bool = true;
				}
			}
			if (bridge_Bool_4x4 == false) {
				foreach (Transform obj_D1 in Point_bridge_4x4_BigFrame) {
					DestroyImmediate (Point_bridge_4x4_BigFrame.GetChild (0).gameObject);
				}
				bridge_Bool_4x4 = true;
				var obj_B1 = Instantiate (bridge_2x2_Small, Point_bridge_2x2_SmallFrame.position, Quaternion.identity);
				obj_B1.transform.parent = Point_bridge_2x2_SmallFrame;
				bridge_Bool_2x2 = false;
				if (point_Wing_Big_FrameBig.childCount > 0) {
					foreach (Transform obj_DW2 in point_Wing_Big_FrameBig) {
						DestroyImmediate (point_Wing_Big_FrameBig.GetChild (0).gameObject);
					}
					var obj_Wing1 = Instantiate (wingSmall, point_Wing_FrameSmall.position, Quaternion.identity);
					obj_Wing1.transform.parent = point_Wing_FrameSmall;
				}
			}
			view_Equipment = false;
			//Manipulator
			if (point_Manipulator_FrameBig_CabBig.childCount > 0) {
				foreach (Transform obj_ManipD in point_Manipulator_FrameBig_CabBig) {
					DestroyImmediate (point_Manipulator_FrameBig_CabBig.GetChild (0).gameObject);
				}
				if (manipulator1_Bool == false) {
					manipulator1_Bool = true;
				} else if (manipulator2_Bool == false) {
					manipulator2_Bool = true;
				}else if (manipulator3_Bool == false) {
					manipulator3_Bool = true;
				}
				//Block Manip
				if (controllerTruck_Bool == false) {
					gameObject.GetComponent<ControllerTruck> ().blockManip_Bool = false;
				}
				check_OnBody = true;
			} else if (point_Manipulator_FrameBig_CabSmall.childCount > 0) {
				foreach (Transform obj_ManipD1 in point_Manipulator_FrameBig_CabSmall) {
					DestroyImmediate (point_Manipulator_FrameBig_CabSmall.GetChild (0).gameObject);
				}
				if (manipulator1_Bool == false) {
					var obj_Manip1 = Instantiate (manipulator1, point_Manipulator_FrameSmall.position, Quaternion.identity);
					obj_Manip1.transform.parent = point_Manipulator_FrameSmall;
				} else if (manipulator2_Bool == false) {
					var obj_Manip2 = Instantiate (manipulator2, point_Manipulator_FrameSmall.position, Quaternion.identity);
					obj_Manip2.transform.parent = point_Manipulator_FrameSmall;
				}
				else if (manipulator3_Bool == false) {
					var obj_Manip3 = Instantiate (manipulator3, point_Manipulator_FrameSmall.position, Quaternion.identity);
					obj_Manip3.transform.parent = point_Manipulator_FrameSmall;
				}
				if (colorRed_Manip != 0) {
					if (colorRed_Manip == 1) {
						ColorBlue_Manip ();
					} else if (colorRed_Manip == 2) {
						ColorRed_Manip ();
					} else if (colorRed_Manip == 3) {
						ColorWhite_Manip ();
					} else if (colorRed_Manip == 4) {
						ColorYellow_Manip ();
					}
				}
			}
			//Block Instantiate Manipulator
			if (cabinBig_Bool == false) {
				block_Instantiate_Manip = true;
			}
			//Body
			if (body_Bool == false) {
				GreateBody ();
			}
			//Protection
			if (protection_Bool == false) {
				GreateProtection ();
			}
		//Cardan
			Cardan();
			//Add Transform
			if (gameObject.GetComponent<ControllerTruck> () != null) {
				GreateController ();
			}
			ManipulatorController ();
			//Check Manip
			CheckManipulator();
			//Check Crable
			CheckCrable ();
			AddConponentArrow ();
			//Add Target Camera
			transform.GetChild (62).gameObject.GetComponent<CameraToManipulator> ().target = transform;
			if (cabin_Int == 2) {
				if (Cradle_Bool == false) {
					Cradle_Bool = true;
				}
				if (view_Crable == false) {
					view_Crable = true;
				}
			}
		} else if (frameSmall_Bool == false) {
			iconTuningFrame_Bool = true;
			DestroyObj ();
			foreach (Transform obj_SmallFrame in point_Frame) {
				DestroyImmediate (point_Frame.GetChild (0).gameObject);
			}
			frameSmall_Bool = true;
			view_Frame_Tuning = true;
			//Bumper
			if (point_Bumper_FrameSmall.childCount > 0) {
				foreach (Transform obj_D1 in point_Bumper_FrameSmall) {
					DestroyImmediate (point_Bumper_FrameSmall.GetChild (0).gameObject);
				}
				if (bumper1_Bool == false) {
					bumper1_Bool = true;
					view_Hitch = true;
				} else if (bumper2_Bool == false) {
					bumper2_Bool = true;
					view_Hitch = true;
				} else if (bumper3_Bool == false) {
					bumper3_Bool = true;
					view_Hitch = true;
				}
			}
			//Hitch
			if (hitch_Bool == false) {
				foreach (Transform obj_D1 in point_Hitch_FrameSmall) {
					DestroyImmediate (point_Hitch_FrameSmall.GetChild (0).gameObject);
				}
				hitch_Bool = true;
			}
			//Destroy Bridge 2x2
		     if (Point_bridge_2x2_SmallFrame.childCount > 0) {
				foreach (Transform obj_D1 in Point_bridge_2x2_SmallFrame) {
					DestroyImmediate (Point_bridge_2x2_SmallFrame.GetChild (0).gameObject);
				}
				bridge_Bool_2x2 = true;
				bridge_Int = 0;
				view_Wing_Bool = true;
			}
		//Destoy Wing
			if (point_Wing_FrameSmall.childCount > 0) {
				foreach (Transform obj_DW in point_Wing_FrameSmall) {
					DestroyImmediate (point_Wing_FrameSmall.GetChild (0).gameObject);
				}
				wing_Bool = true;
			}
			frame_Int = 0;
			Equipment_Bool = true;
			view_Equipment = true;
		//Destroy Manipulator
			if (point_Manipulator_FrameSmall.childCount > 0) {
				foreach (Transform obj_Manip in point_Manipulator_FrameSmall) {
					DestroyImmediate (point_Manipulator_FrameSmall.GetChild (0).gameObject);
				}
			}
			if (manipulator1_Bool == false) {
				manipulator1_Bool = true;
			} else if (manipulator2_Bool == false) {
				manipulator2_Bool = true;
			}else if (manipulator3_Bool == false) {
				manipulator3_Bool = true;
			}
			//Destroy Body
			if (body_Bool == false) {
				Body ();
			}
			//Protection
			if (protection_Bool == false) {
				Protection ();
			}
			//Destroy Cardan
			foreach (Transform obj_Car in point_Сardan) {
				DestroyImmediate (point_Сardan.GetChild (0).gameObject);
			}
			//Block Manip
			if (controllerTruck_Bool == false) {
				gameObject.GetComponent<ControllerTruck> ().blockManip_Bool = false;
			}
			//Block Start Truck
			if(controllerTruck_Bool == false){
				gameObject.GetComponent<ControllerTruck> ().startTruck = false;
			}
			if (check_OnBody == false) {
				check_OnBody = true;
			}
			if (view_Crable == false) {
				view_Crable = true;
			}
			if (BodyDop3_Bool == false) {
				BodyDop3_Bool = true;
			}
			if (Cradle_Bool == false) {
				Cradle_Bool = true;
			}
		}
	}
	public void FramePlus(){
		if (framePlus_Bool == true) {
			var obj_FramePlus = Instantiate (frame_Plus, point_Frame_Plus.position, Quaternion.identity);
			obj_FramePlus.transform.parent = point_Frame_Plus;
			framePlus_Bool = false;
			view_Plus = false;
			//Bumper
			if (point_Bumper_FrameBig.childCount > 0) {
				foreach (Transform obj_D1 in point_Bumper_FrameBig) {
					DestroyImmediate (point_Bumper_FrameBig.GetChild (0).gameObject);
				}
				if (bumper1_Bool == false) {
					var obj_B1 = Instantiate (bumper1, point_Bumper_FramePlus.position, Quaternion.identity);
					obj_B1.transform.parent = point_Bumper_FramePlus;
				} else if (bumper2_Bool == false) {
					var obj_B2 = Instantiate (bumper2, point_Bumper_FramePlus.position, Quaternion.identity);
					obj_B2.transform.parent = point_Bumper_FramePlus;
				} else if (bumper3_Bool == false) {
					var obj_B3 = Instantiate (bumper3, point_Bumper_FramePlus.position, Quaternion.identity);
					obj_B3.transform.parent = point_Bumper_FramePlus;
				}
			} else if (point_Bumper_FrameSmall.childCount > 0) {
				foreach (Transform obj_D1 in point_Bumper_FramePlus) {
					DestroyImmediate (point_Bumper_FramePlus.GetChild (0).gameObject);
				}
				if (bumper1_Bool == false) {
					var obj_B1 = Instantiate (bumper1, point_Bumper_FrameSmall.position, Quaternion.identity);
					obj_B1.transform.parent = point_Bumper_FrameSmall;
				} else if (bumper2_Bool == false) {
					var obj_B2 = Instantiate (bumper2, point_Bumper_FrameSmall.position, Quaternion.identity);
					obj_B2.transform.parent = point_Bumper_FrameSmall;
				} else if (bumper3_Bool == false) {
					var obj_B3 = Instantiate (bumper3, point_Bumper_FrameSmall.position, Quaternion.identity);
					obj_B3.transform.parent = point_Bumper_FrameSmall;
				}
			}
			//Hitch
			if (hitch_Bool == false) {
				NextHitch ();
			}
			//Body
			if (body_Bool == false) {
				GreateBody ();
			}
		} else if (framePlus_Bool == false) {
			foreach (Transform obj_FramePlus in point_Frame_Plus) {
				DestroyImmediate (point_Frame_Plus.GetChild (0).gameObject);
			}
			//Bumper
			if (point_Bumper_FramePlus.childCount > 0) {
				foreach (Transform obj_D1 in point_Bumper_FramePlus) {
					DestroyImmediate (point_Bumper_FramePlus.GetChild (0).gameObject);
				}
				if (bumper1_Bool == false) {
					var obj_B1 = Instantiate (bumper1, point_Bumper_FrameBig.position, Quaternion.identity);
					obj_B1.transform.parent = point_Bumper_FrameBig;
				} else if (bumper2_Bool == false) {
					var obj_B2 = Instantiate (bumper2, point_Bumper_FrameBig.position, Quaternion.identity);
					obj_B2.transform.parent = point_Bumper_FrameBig;
				} else if (bumper3_Bool == false) {
					var obj_B3 = Instantiate (bumper3, point_Bumper_FrameBig.position, Quaternion.identity);
					obj_B3.transform.parent = point_Bumper_FrameBig;
				}
			}
			framePlus_Bool = true;
			view_Plus = true;
			//Destroy Box Big Plus
			if (boxBig_L_Plus_Bool == false) {
				foreach (Transform obj_DB1 in point_BoxBig_Left_FramePlus) {
					DestroyImmediate (point_BoxBig_Left_FramePlus.GetChild (0).gameObject);
				}
				boxBig_L_Plus_Bool = true;
			}
			if (boxBig_R_Plus_Bool == false) {
				foreach (Transform obj_DB1 in point_BoxBig_Right_FramePlus) {
					DestroyImmediate (point_BoxBig_Right_FramePlus.GetChild (0).gameObject);
				}
				boxBig_R_Plus_Bool = true;
			}
			//Destroy Tenk end Reserve
			if (point_Tank_Wheel_FramePlus.childCount > 0) {
				foreach (Transform obj_DTW in point_Tank_Wheel_FramePlus) {
					DestroyImmediate (point_Tank_Wheel_FramePlus.GetChild (0).gameObject);
				}
				if (reserveS_Bool == false) {
					reserveS_Bool = true;
				} else if (tank_Plus_Bool == false) {
					tank_Plus_Bool = true;
				}
			}
		//Hitch
			if (point_Hitch_FramePlus.childCount > 0) {
				foreach (Transform obj_H in point_Hitch_FramePlus) {
					DestroyImmediate (point_Hitch_FramePlus.GetChild (0).gameObject);
				}
				var obj_H1 = Instantiate (hitch, point_Hitch_FrameBig.position, Quaternion.identity);
				obj_H1.transform.parent = point_Hitch_FrameBig;
			}
			//Body
			if (body_Bool == false) {
				GreateBody ();
			}
		}
	}
	//Bridge_________________________________________________________________________________________________
	public void Bridge2x2(){
		if (bridge_Bool_2x2 == true) {
			if (frame_Int == 1) {
				if (bridge_Bool_4x4 == false) {
					foreach (Transform obj_DB1 in Point_bridge_4x4_BigFrame) {
						DestroyImmediate (Point_bridge_4x4_BigFrame.GetChild (0).gameObject);
					}
					bridge_Bool_4x4 = true;
				}
				var obj_B1 = Instantiate (bridge_2x2_Big, Point_bridge_2x2_BigFrame.position, Quaternion.identity);
				obj_B1.transform.parent = Point_bridge_2x2_BigFrame;
				view_Reserve_Left = false;
				//Wing
				if (wing_Bool == false) {
					if (point_Wing_FrameSmall.childCount > 0) {
						foreach (Transform obj_DW1 in point_Wing_FrameSmall) {
							DestroyImmediate (point_Wing_FrameSmall.GetChild (0).gameObject);
						}
					} else if (point_Wing_Big_FrameBig.childCount > 0) {
						foreach (Transform obj_DW2 in point_Wing_Big_FrameBig) {
							DestroyImmediate (point_Wing_Big_FrameBig.GetChild (0).gameObject);
						}
					}
					var obj_Wing1 = Instantiate (wingSmall, point_Wing_Small_FrameBig.position, Quaternion.identity);
					obj_Wing1.transform.parent = point_Wing_Small_FrameBig;
				}
			}
			else if (frame_Int == 2) {
				var obj_B2 = Instantiate (bridge_2x2_Small, Point_bridge_2x2_SmallFrame.position, Quaternion.identity);
				obj_B2.transform.parent = Point_bridge_2x2_SmallFrame;
				//Wing
				if (wing_Bool == false) {
					if (point_Wing_Small_FrameBig.childCount > 0) {
						foreach (Transform obj_DW1 in point_Wing_Small_FrameBig) {
							DestroyImmediate (point_Wing_Small_FrameBig.GetChild (0).gameObject);
						}
					} else if (point_Wing_Big_FrameBig.childCount > 0) {
						foreach (Transform obj_DW2 in point_Wing_Big_FrameBig) {
							DestroyImmediate (point_Wing_Big_FrameBig.GetChild (0).gameObject);
						}
					}
					var obj_Wing1 = Instantiate (wingSmall, point_Wing_Small_FrameBig.position, Quaternion.identity);
					obj_Wing1.transform.parent = point_Wing_Small_FrameBig;
				}
			}
			bridge_Bool_2x2 = false;
			bridge_Int = 1;
			view_Wing_Bool = false;
			//Protection
			if (protection_Bool == false) {
				GreateProtection ();
			}
			//Add Transform
			if (gameObject.GetComponent<ControllerTruck> () != null) {
				GreateController ();
			}
			//Block Start Truck
			if(controllerTruck_Bool == false){
				gameObject.GetComponent<ControllerTruck> ().startTruck = true;
			}
		} else if (bridge_Bool_2x2 == false) {
			if (Point_bridge_2x2_BigFrame.childCount > 0) {
				foreach (Transform obj_DB1 in Point_bridge_2x2_BigFrame) {
					DestroyImmediate (Point_bridge_2x2_BigFrame.GetChild (0).gameObject);
				}
				view_Reserve_Left = true;
			
			} else if (Point_bridge_2x2_SmallFrame.childCount > 0) {
				foreach (Transform obj_DB2 in Point_bridge_2x2_SmallFrame) {
					DestroyImmediate (Point_bridge_2x2_SmallFrame.GetChild (0).gameObject);
				}
			
			}
			bridge_Bool_2x2 = true;
			bridge_Int = 0;
			view_Wing_Bool = true;
		//Destroy Wing
			if (point_Wing_Small_FrameBig.childCount > 0) {
				foreach (Transform obj_DW1 in point_Wing_Small_FrameBig) {
					DestroyImmediate (point_Wing_Small_FrameBig.GetChild (0).gameObject);
				}
				wing_Bool = true;
			} else if (point_Wing_FrameSmall.childCount > 0) {
				foreach (Transform obj_DW2 in point_Wing_FrameSmall) {
					DestroyImmediate (point_Wing_FrameSmall.GetChild (0).gameObject);
				}
				wing_Bool = true;
			}
			//Protection
			if (protection_Bool == false) {
				Protection ();
			}
			//Block Start Truck
			if(controllerTruck_Bool == false){
				gameObject.GetComponent<ControllerTruck> ().startTruck = false;
			}
		}
}
	public void Bridge4x4(){
		if (bridge_Bool_4x4 == true && frame_Int == 1) {
				if (Point_bridge_2x2_BigFrame.childCount > 0) {
				foreach (Transform obj_DB1 in Point_bridge_2x2_BigFrame) {
						DestroyImmediate (Point_bridge_2x2_BigFrame.GetChild (0).gameObject);
					}
				view_Reserve_Left = true;
				bridge_Bool_2x2 = true;
				//Reserve Left
				if(point_SWheel_Left_A_FrameBig.childCount > 0){
					foreach (Transform obj_D in point_SWheel_Left_A_FrameBig) {
						DestroyImmediate (point_SWheel_Left_A_FrameBig.GetChild (0).gameObject);
					}
					reserve_LeftBack_Bool = true;
				}
			}
				var obj_B3 = Instantiate (bridge_4x4_Big, Point_bridge_4x4_BigFrame.position, Quaternion.identity);
				obj_B3.transform.parent = Point_bridge_4x4_BigFrame;
			bridge_Bool_4x4 = false;
			bridge_Int = 2;
			view_Wing_Bool = false;
			// Wing
			if (wing_Bool == false) {
				if (point_Wing_FrameSmall.childCount > 0) {
					foreach (Transform obj_DW1 in point_Wing_FrameSmall) {
						DestroyImmediate (point_Wing_FrameSmall.GetChild (0).gameObject);
					}
				} else if (point_Wing_Small_FrameBig.childCount > 0) {
					foreach (Transform obj_DW2 in point_Wing_Small_FrameBig) {
						DestroyImmediate (point_Wing_Small_FrameBig.GetChild (0).gameObject);
					}
				}
				var obj_Wing2 = Instantiate (wingBig, point_Wing_Big_FrameBig.position, Quaternion.identity);
				obj_Wing2.transform.parent = point_Wing_Big_FrameBig;
			}
			//Protection
			if (protection_Bool == false) {
				GreateProtection ();
			}
			//Add Transform
			if (gameObject.GetComponent<ControllerTruck> () != null) {
				GreateController ();
			}
			//Block Start Truck
			if(controllerTruck_Bool == false){
				gameObject.GetComponent<ControllerTruck> ().startTruck = true;
			}
		} else if (bridge_Bool_4x4 == false) {
			foreach (Transform obj_B3 in Point_bridge_4x4_BigFrame) {
				DestroyImmediate (Point_bridge_4x4_BigFrame.GetChild (0).gameObject);
			}
			bridge_Bool_4x4 = true;
			bridge_Int = 0;
			view_Wing_Bool = true;
			//Destroy Wing
			if (point_Wing_Big_FrameBig.childCount > 0) {
				foreach (Transform obj_DW3 in point_Wing_Big_FrameBig) {
					DestroyImmediate (point_Wing_Big_FrameBig.GetChild (0).gameObject);
				}
				wing_Bool = true;
			}
			//Protection
			if (protection_Bool == false) {
				Protection ();
			}
			//Block Start Truck
			if(controllerTruck_Bool == false){
				gameObject.GetComponent<ControllerTruck> ().startTruck = false;
			}
		}
	}
	//Wing_________________________________________________________________________________________________
	public void Wing(){
		if (wing_Bool == true) {
			if (frame_Int == 1 && bridge_Int == 1) {
				var obj_W1 = Instantiate (wingSmall, point_Wing_Small_FrameBig.position, Quaternion.identity);
				obj_W1.transform.parent = point_Wing_Small_FrameBig;
			} else if (frame_Int == 2) {
				var obj_W1 = Instantiate (wingSmall, point_Wing_FrameSmall.position, Quaternion.identity);
				obj_W1.transform.parent = point_Wing_FrameSmall;
			} else if (frame_Int == 1 && bridge_Int == 2) {
				var obj_W1 = Instantiate (wingBig, point_Wing_Big_FrameBig.position, Quaternion.identity);
				obj_W1.transform.parent = point_Wing_Big_FrameBig;
			}
			wing_Bool = false;
		} else if (wing_Bool == false) {
			if (point_Wing_Big_FrameBig.childCount > 0) {
				foreach (Transform obj_DW1 in point_Wing_Big_FrameBig) {
					DestroyImmediate (point_Wing_Big_FrameBig.GetChild (0).gameObject);
				}
			} else if (point_Wing_FrameSmall.childCount > 0) {
				foreach (Transform obj_DW2 in point_Wing_FrameSmall) {
					DestroyImmediate (point_Wing_FrameSmall.GetChild (0).gameObject);
				}
			} else if (point_Wing_Small_FrameBig.childCount > 0) {
				foreach (Transform obj_DW3 in point_Wing_Small_FrameBig) {
					DestroyImmediate (point_Wing_Small_FrameBig.GetChild (0).gameObject);
				}
			}
			wing_Bool = true;
		}
	}
	//Bumper1_________________________________________________________________________________________________
	public void Bumper1(){
		if (bumper1_Bool == true) {
			if (frame_Int == 1 && framePlus_Bool == true) {             //Frame Big
				if (point_Bumper_FrameBig.childCount > 0) {
					foreach (Transform obj_D1 in point_Bumper_FrameBig) {
						DestroyImmediate (point_Bumper_FrameBig.GetChild (0).gameObject);
					}
				}
				var obj_B1 = Instantiate (bumper1, point_Bumper_FrameBig.position, Quaternion.identity);
				obj_B1.transform.parent = point_Bumper_FrameBig;
			} else if (frame_Int == 1 && framePlus_Bool == false) {     //Frame Plus
				if (point_Bumper_FramePlus.childCount > 0) {
					foreach (Transform obj_D2 in point_Bumper_FramePlus) {
						DestroyImmediate (point_Bumper_FramePlus.GetChild (0).gameObject);
					}
				}
				var obj_B2 = Instantiate (bumper1, point_Bumper_FramePlus.position, Quaternion.identity);
				obj_B2.transform.parent = point_Bumper_FramePlus;
			} else if (frame_Int == 2) {                                //Frame Small
				if (point_Bumper_FrameSmall.childCount > 0) {
					foreach (Transform obj_D3 in point_Bumper_FrameSmall) {
						DestroyImmediate (point_Bumper_FrameSmall.GetChild (0).gameObject);
					}
				}
				var obj_B3 = Instantiate (bumper1, point_Bumper_FrameSmall.position, Quaternion.identity);
				obj_B3.transform.parent = point_Bumper_FrameSmall;
			}
			if (bumper2_Bool == false) {
				bumper2_Bool = true;
			} else if (bumper3_Bool == false) {
				bumper3_Bool = true;
			}
			bumper1_Bool = false;
			view_Hitch = false;
			LightBumper ();
		} else if (bumper1_Bool == false) {
			if (point_Bumper_FrameBig.childCount > 0) {
				foreach (Transform obj_D1 in point_Bumper_FrameBig) {
					DestroyImmediate (point_Bumper_FrameBig.GetChild (0).gameObject);
				}
				//Hitch
				foreach (Transform obj_DH1 in point_Hitch_FrameBig) {
					DestroyImmediate (point_Hitch_FrameBig.GetChild (0).gameObject);
				}
				hitch_Bool = true;
			} else if (point_Bumper_FramePlus.childCount > 0) {
				foreach (Transform obj_D1 in point_Bumper_FramePlus) {
					DestroyImmediate (point_Bumper_FramePlus.GetChild (0).gameObject);
				}
				//Hitch
				foreach (Transform obj_DH2 in point_Hitch_FramePlus) {
					DestroyImmediate (point_Hitch_FramePlus.GetChild (0).gameObject);
				}
				hitch_Bool = true;
			} else if (point_Bumper_FrameSmall.childCount > 0) {
				foreach (Transform obj_D1 in point_Bumper_FrameSmall) {
					DestroyImmediate (point_Bumper_FrameSmall.GetChild (0).gameObject);
				}
				//Hitch
				foreach (Transform obj_DH3 in point_Hitch_FrameSmall) {
					DestroyImmediate (point_Hitch_FrameSmall.GetChild (0).gameObject);
				}
				hitch_Bool = true;
			}
			bumper1_Bool = true;
			view_Hitch = true;
		}
	}
	//Bumper2_________________________________________________________________________________________________
	public void Bumper2(){
		if (bumper2_Bool == true) {
			if (frame_Int == 1 && framePlus_Bool == true) {             //Frame Big
				if (point_Bumper_FrameBig.childCount > 0) {
					foreach (Transform obj_D1 in point_Bumper_FrameBig) {
						DestroyImmediate (point_Bumper_FrameBig.GetChild (0).gameObject);
					}
				}
				var obj_B1 = Instantiate (bumper2, point_Bumper_FrameBig.position, Quaternion.identity);
				obj_B1.transform.parent = point_Bumper_FrameBig;
			} else if (frame_Int == 1 && framePlus_Bool == false) {     //Frame Plus
				if (point_Bumper_FramePlus.childCount > 0) {
					foreach (Transform obj_D2 in point_Bumper_FramePlus) {
						DestroyImmediate (point_Bumper_FramePlus.GetChild (0).gameObject);
					}
				}
				var obj_B2 = Instantiate (bumper2, point_Bumper_FramePlus.position, Quaternion.identity);
				obj_B2.transform.parent = point_Bumper_FramePlus;
			} else if (frame_Int == 2) {                                //Frame Small
				if (point_Bumper_FrameSmall.childCount > 0) {
					foreach (Transform obj_D3 in point_Bumper_FrameSmall) {
						DestroyImmediate (point_Bumper_FrameSmall.GetChild (0).gameObject);
					}
				}
				var obj_B3 = Instantiate (bumper2, point_Bumper_FrameSmall.position, Quaternion.identity);
				obj_B3.transform.parent = point_Bumper_FrameSmall;
			}
			if (bumper1_Bool == false) {
				bumper1_Bool = true;
			} else if (bumper3_Bool == false) {
				bumper3_Bool = true;
			}
			bumper2_Bool = false;
			view_Hitch = false;
			LightBumper ();
		} else if (bumper2_Bool == false) {
			if (point_Bumper_FrameBig.childCount > 0) {
				foreach (Transform obj_D1 in point_Bumper_FrameBig) {
					DestroyImmediate (point_Bumper_FrameBig.GetChild (0).gameObject);
				}
				//Hitch
				foreach (Transform obj_DH1 in point_Hitch_FrameBig) {
					DestroyImmediate (point_Hitch_FrameBig.GetChild (0).gameObject);
				}
				hitch_Bool = true;
			} else if (point_Bumper_FramePlus.childCount > 0) {
				foreach (Transform obj_D1 in point_Bumper_FramePlus) {
					DestroyImmediate (point_Bumper_FramePlus.GetChild (0).gameObject);
				}
				//Hitch
				foreach (Transform obj_DH2 in point_Hitch_FramePlus) {
					DestroyImmediate (point_Hitch_FramePlus.GetChild (0).gameObject);
				}
				hitch_Bool = true;
			} else if (point_Bumper_FrameSmall.childCount > 0) {
				foreach (Transform obj_D1 in point_Bumper_FrameSmall) {
					DestroyImmediate (point_Bumper_FrameSmall.GetChild (0).gameObject);
				}
				//Hitch
				foreach (Transform obj_DH3 in point_Hitch_FrameSmall) {
					DestroyImmediate (point_Hitch_FrameSmall.GetChild (0).gameObject);
				}
				hitch_Bool = true;
			}
			bumper2_Bool = true;
			view_Hitch = true;
		}
	}
	//Bumper3_________________________________________________________________________________________________
	public void Bumper3(){
		if (bumper3_Bool == true) {
			if (frame_Int == 1 && framePlus_Bool == true) {             //Frame Big
				if (point_Bumper_FrameBig.childCount > 0) {
					foreach (Transform obj_D1 in point_Bumper_FrameBig) {
						DestroyImmediate (point_Bumper_FrameBig.GetChild (0).gameObject);
					}
				}
				var obj_B1 = Instantiate (bumper3, point_Bumper_FrameBig.position, Quaternion.identity);
				obj_B1.transform.parent = point_Bumper_FrameBig;
			} else if (frame_Int == 1 && framePlus_Bool == false) {     //Frame Plus
				if (point_Bumper_FramePlus.childCount > 0) {
					foreach (Transform obj_D2 in point_Bumper_FramePlus) {
						DestroyImmediate (point_Bumper_FramePlus.GetChild (0).gameObject);
					}
				}
				var obj_B2 = Instantiate (bumper3, point_Bumper_FramePlus.position, Quaternion.identity);
				obj_B2.transform.parent = point_Bumper_FramePlus;
			} else if (frame_Int == 2) {                                //Frame Small
				if (point_Bumper_FrameSmall.childCount > 0) {
					foreach (Transform obj_D3 in point_Bumper_FrameSmall) {
						DestroyImmediate (point_Bumper_FrameSmall.GetChild (0).gameObject);
					}
				}
				var obj_B3 = Instantiate (bumper3, point_Bumper_FrameSmall.position, Quaternion.identity);
				obj_B3.transform.parent = point_Bumper_FrameSmall;
			}
			if (bumper2_Bool == false) {
				bumper2_Bool = true;
			} else if (bumper1_Bool == false) {
				bumper1_Bool = true;
			}
			bumper3_Bool = false;
			view_Hitch = false;
			LightBumper ();
		} else if (bumper3_Bool == false) {
			if (point_Bumper_FrameBig.childCount > 0) {
				foreach (Transform obj_D1 in point_Bumper_FrameBig) {
					DestroyImmediate (point_Bumper_FrameBig.GetChild (0).gameObject);
				}
				//Hitch
				foreach (Transform obj_DH1 in point_Hitch_FrameBig) {
					DestroyImmediate (point_Hitch_FrameBig.GetChild (0).gameObject);
				}
				hitch_Bool = true;
			} else if (point_Bumper_FramePlus.childCount > 0) {
				foreach (Transform obj_D1 in point_Bumper_FramePlus) {
					DestroyImmediate (point_Bumper_FramePlus.GetChild (0).gameObject);
				}
				//Hitch
				foreach (Transform obj_DH2 in point_Hitch_FramePlus) {
					DestroyImmediate (point_Hitch_FramePlus.GetChild (0).gameObject);
				}
				hitch_Bool = true;
			} else if (point_Bumper_FrameSmall.childCount > 0) {
				foreach (Transform obj_D1 in point_Bumper_FrameSmall) {
					DestroyImmediate (point_Bumper_FrameSmall.GetChild (0).gameObject);
				}
				//Hitch
				foreach (Transform obj_DH3 in point_Hitch_FrameSmall) {
					DestroyImmediate (point_Hitch_FrameSmall.GetChild (0).gameObject);
				}
				hitch_Bool = true;
			}
			bumper3_Bool = true;
			view_Hitch = true;
		}
	}
	public void NextHitch(){
		if (frame_Int == 1 && framePlus_Bool == true) {
			if (point_Hitch_FramePlus.childCount > 0) {
				foreach (Transform obj_DH1 in point_Hitch_FramePlus) {
					DestroyImmediate (point_Hitch_FramePlus.GetChild (0).gameObject);
				}
			} else if (point_Hitch_FrameSmall.childCount > 0) {
				foreach (Transform obj_DH2 in point_Hitch_FrameSmall) {
					DestroyImmediate (point_Hitch_FrameSmall.GetChild (0).gameObject);
				}
			}
			var obj_H1 = Instantiate (hitch, point_Hitch_FrameBig.position, Quaternion.identity);
			obj_H1.transform.parent = point_Hitch_FrameBig;
		}else if(frame_Int == 1 && framePlus_Bool == false){
			if (point_Hitch_FrameBig.childCount > 0) {
				foreach (Transform obj_DH1 in point_Hitch_FrameBig) {
					DestroyImmediate (point_Hitch_FrameBig.GetChild (0).gameObject);
				}
			} else if (point_Hitch_FrameSmall.childCount > 0) {
				foreach (Transform obj_DH2 in point_Hitch_FrameSmall) {
					DestroyImmediate (point_Hitch_FrameSmall.GetChild (0).gameObject);
				}
			}
			var obj_H2 = Instantiate (hitch, point_Hitch_FramePlus.position, Quaternion.identity);
			obj_H2.transform.parent = point_Hitch_FramePlus;
		}else if(frame_Int == 2){
			if (point_Hitch_FramePlus.childCount > 0) {
				foreach (Transform obj_DH1 in point_Hitch_FramePlus) {
					DestroyImmediate (point_Hitch_FramePlus.GetChild (0).gameObject);
				}
			} else if (point_Hitch_FrameBig.childCount > 0) {
				foreach (Transform obj_DH2 in point_Hitch_FrameBig) {
					DestroyImmediate (point_Hitch_FrameBig.GetChild (0).gameObject);
				}
			}
			var obj_H3 = Instantiate (hitch, point_Hitch_FrameSmall.position, Quaternion.identity);
			obj_H3.transform.parent = point_Hitch_FrameSmall;
		}
	}
	//Hitch_________________________________________________________________________________________________
	public void Hitch(){
		if (hitch_Bool == true) {
			if (point_Bumper_FrameBig.childCount > 0) {
				var obj_H1 = Instantiate (hitch, point_Hitch_FrameBig.position, Quaternion.identity);
				obj_H1.transform.parent = point_Hitch_FrameBig;
			} else if (point_Bumper_FramePlus.childCount > 0) {
				var obj_H2 = Instantiate (hitch, point_Hitch_FramePlus.position, Quaternion.identity);
				obj_H2.transform.parent = point_Hitch_FramePlus;
			} else if (point_Bumper_FrameSmall.childCount > 0) {
				var obj_H3 = Instantiate (hitch, point_Hitch_FrameSmall.position, Quaternion.identity);
				obj_H3.transform.parent = point_Hitch_FrameSmall;
			}
			hitch_Bool = false;
		} else if (hitch_Bool == false) {
			if (point_Bumper_FrameBig.childCount > 0) {
				foreach (Transform obj_DH1 in point_Hitch_FrameBig) {
					DestroyImmediate (point_Hitch_FrameBig.GetChild (0).gameObject);
				}
			} else if (point_Bumper_FramePlus.childCount > 0) {
				foreach (Transform obj_DH2 in point_Hitch_FramePlus) {
					DestroyImmediate (point_Hitch_FramePlus.GetChild (0).gameObject);
				}
			} else if (point_Bumper_FrameSmall.childCount > 0) {
				foreach (Transform obj_DH3 in point_Hitch_FrameSmall) {
					DestroyImmediate (point_Hitch_FrameSmall.GetChild (0).gameObject);
				}
			}
			hitch_Bool = true;
		}
	}
	//Air_Left______________________________________________________________________________________________
	public void AirLeft(){
		if (air_Left_Bool == true) {
			if (frame_Int == 1) {
				if (point_Battery_Left_FrameBig.childCount > 0) {
					foreach (Transform obj_DB1 in point_Battery_Left_FrameBig) {
						DestroyImmediate (point_Battery_Left_FrameBig.GetChild (0).gameObject);
					}
					battery_Left_Bool = true;
				}
					var obj_AirL1 = Instantiate (air_Left, point_Air_Left_FrameBig.position, Quaternion.identity);
					obj_AirL1.transform.parent = point_Air_Left_FrameBig;
			} else if (frame_Int == 2) {
				if (point_Battery_Left_FrameSmall.childCount > 0) {
					foreach (Transform obj_DB2 in point_Battery_Left_FrameSmall) {
						DestroyImmediate (point_Battery_Left_FrameSmall.GetChild (0).gameObject);
					}
					battery_Left_Bool = true;
				}
					var obj_AirL2 = Instantiate (air_Left, point_Air_Left_FrameSmall.position, Quaternion.identity);
					obj_AirL2.transform.parent = point_Air_Left_FrameSmall;
			}
			air_Left_Bool = false;
		} else if (air_Left_Bool == false) {
			if (frame_Int == 1) {
				foreach (Transform obj_DB1 in point_Air_Left_FrameBig) {
					DestroyImmediate (point_Air_Left_FrameBig.GetChild (0).gameObject);
				}
			} else if (frame_Int == 2) {
				foreach (Transform obj_DB2 in point_Air_Left_FrameSmall) {
					DestroyImmediate (point_Air_Left_FrameSmall.GetChild (0).gameObject);
				}
			}
			air_Left_Bool = true;
		}
	}
	//Air_Right______________________________________________________________________________________________
	public void AirRight(){
		if (air_Right_Bool == true) {
			if (frame_Int == 1) {
				if (point_Battery_Right_FrameBig.childCount > 0) {
					foreach (Transform obj_DB1 in point_Battery_Right_FrameBig) {
						DestroyImmediate (point_Battery_Right_FrameBig.GetChild (0).gameObject);
					}
					battery_Right_Bool = true;
				}
				var obj_AirR1 = Instantiate (air_Right, point_Air_Right_FrameBig.position, Quaternion.identity);
				obj_AirR1.transform.parent = point_Air_Right_FrameBig;
			} else if (frame_Int == 2) {
				if (point_Battery_Right_FrameSmall.childCount > 0) {
					foreach (Transform obj_DB2 in point_Battery_Right_FrameSmall) {
						DestroyImmediate (point_Battery_Right_FrameSmall.GetChild (0).gameObject);
					}
					battery_Right_Bool = true;
				}
				var obj_AirR2 = Instantiate (air_Right, point_Air_Right_FrameSmall.position, Quaternion.identity);
				obj_AirR2.transform.parent = point_Air_Right_FrameSmall;
			}
			air_Right_Bool = false;
		} else if (air_Right_Bool == false) {
			if (frame_Int == 1) {
				foreach (Transform obj_DB1 in point_Air_Right_FrameBig) {
					DestroyImmediate (point_Air_Right_FrameBig.GetChild (0).gameObject);
				}
			} else if (frame_Int == 2) {
				foreach (Transform obj_DB2 in point_Air_Right_FrameSmall) {
					DestroyImmediate (point_Air_Right_FrameSmall.GetChild (0).gameObject);
				}
			}
			air_Right_Bool = true;
		}
	}
	//Battery Left____________________________________________________________________________________________
	public void BatteryLeft(){
		if (battery_Left_Bool == true) {
			if (frame_Int == 1) {
				if (point_Air_Left_FrameBig.childCount > 0) {
					foreach (Transform obj_DA1 in point_Air_Left_FrameBig) {
						DestroyImmediate (point_Air_Left_FrameBig.GetChild (0).gameObject);
					}
					air_Left_Bool = true;
				}
				var obj_BatL1 = Instantiate (battery_Left, point_Battery_Left_FrameBig.position, Quaternion.identity);
				obj_BatL1.transform.parent = point_Battery_Left_FrameBig;
			} else if (frame_Int == 2) {
				if (point_Air_Left_FrameSmall.childCount > 0) {
					foreach (Transform obj_DA2 in point_Air_Left_FrameSmall) {
						DestroyImmediate (point_Air_Left_FrameSmall.GetChild (0).gameObject);
					}
					air_Left_Bool = true;
				}
				var obj_BatL2 = Instantiate (battery_Left, point_Battery_Left_FrameSmall.position, Quaternion.identity);
				obj_BatL2.transform.parent = point_Battery_Left_FrameSmall;
			}
			battery_Left_Bool = false;
		} else if (battery_Left_Bool == false) {
			if (frame_Int == 1) {
				foreach (Transform obj_DB1 in point_Battery_Left_FrameBig) {
					DestroyImmediate (point_Battery_Left_FrameBig.GetChild (0).gameObject);
				}
			} else if (frame_Int == 2) {
				foreach (Transform obj_DB2 in point_Battery_Left_FrameSmall) {
					DestroyImmediate (point_Battery_Left_FrameSmall.GetChild (0).gameObject);
				}
			}
			battery_Left_Bool = true;
		}
	}
	//Battery Right____________________________________________________________________________________________
	public void BatteryRight(){
		if (battery_Right_Bool == true) {
			if (frame_Int == 1) {
				if (point_Air_Right_FrameBig.childCount > 0) {
					foreach (Transform obj_DA1 in point_Air_Right_FrameBig) {
						DestroyImmediate (point_Air_Right_FrameBig.GetChild (0).gameObject);
					}
					air_Right_Bool = true;
				}
				var obj_BatL1 = Instantiate (battery_Right, point_Battery_Right_FrameBig.position, Quaternion.identity);
				obj_BatL1.transform.parent = point_Battery_Right_FrameBig;
			} else if (frame_Int == 2) {
				if (point_Air_Right_FrameSmall.childCount > 0) {
					foreach (Transform obj_DA2 in point_Air_Right_FrameSmall) {
						DestroyImmediate (point_Air_Right_FrameSmall.GetChild (0).gameObject);
					}
					air_Right_Bool = true;
				}
				var obj_BatL2 = Instantiate (battery_Right, point_Battery_Right_FrameSmall.position, Quaternion.identity);
				obj_BatL2.transform.parent = point_Battery_Right_FrameSmall;
			}
			battery_Right_Bool = false;
		} else if (battery_Right_Bool == false) {
			if (frame_Int == 1) {
				foreach (Transform obj_DB1 in point_Battery_Right_FrameBig) {
					DestroyImmediate (point_Battery_Right_FrameBig.GetChild (0).gameObject);
				}
			} else if (frame_Int == 2) {
				foreach (Transform obj_DB2 in point_Battery_Right_FrameSmall) {
					DestroyImmediate (point_Battery_Right_FrameSmall.GetChild (0).gameObject);
				}
			}
			battery_Right_Bool = true;
		}
	}
	//BoxSmall Left____________________________________________________________________________________________
	public void BoxSmall_Left(){
		if (boxSmall_Left_Bool == true) {
			if (frame_Int == 1) {
				var obj_BoxSL1 = Instantiate (boxSmall_Left, point_BoxSmall_Left_FrameBig.position, Quaternion.identity);
				obj_BoxSL1.transform.parent = point_BoxSmall_Left_FrameBig;
			} else if (frame_Int == 2) {
				var obj_BoxSL2 = Instantiate (boxSmall_Left, point_BoxSmall_Left_FrameSmall.position, Quaternion.identity);
				obj_BoxSL2.transform.parent = point_BoxSmall_Left_FrameSmall;
			}
			boxSmall_Left_Bool = false;
		} else if (boxSmall_Left_Bool == false) {
			if (frame_Int == 1) {
				foreach (Transform obj_DB1 in point_BoxSmall_Left_FrameBig) {
					DestroyImmediate (point_BoxSmall_Left_FrameBig.GetChild (0).gameObject);
				}
			} else if (frame_Int == 2) {
				foreach (Transform obj_DB2 in point_BoxSmall_Left_FrameSmall) {
					DestroyImmediate (point_BoxSmall_Left_FrameSmall.GetChild (0).gameObject);
				}
			}
			boxSmall_Left_Bool = true;
		}
	}
	//BoxSmall Right____________________________________________________________________________________________
	public void BoxSmall_Right(){
		if (boxSmall_Right_Bool == true) {
			if (frame_Int == 1) {
				var obj_BoxSR1 = Instantiate (boxSmall_Right, point_BoxSmall_Right_FrameBig.position, Quaternion.identity);
				obj_BoxSR1.transform.parent = point_BoxSmall_Right_FrameBig;
			} else if (frame_Int == 2) {
				var obj_BoxSR2 = Instantiate (boxSmall_Right, point_BoxSmall_Right_FrameSmall.position, Quaternion.identity);
				obj_BoxSR2.transform.parent = point_BoxSmall_Right_FrameSmall;
			}
			boxSmall_Right_Bool = false;
		} else if (boxSmall_Right_Bool == false) {
			if (frame_Int == 1) {
				foreach (Transform obj_DB1 in point_BoxSmall_Right_FrameBig) {
					DestroyImmediate (point_BoxSmall_Right_FrameBig.GetChild (0).gameObject);
				}
			} else if (frame_Int == 2) {
				foreach (Transform obj_DB2 in point_BoxSmall_Right_FrameSmall) {
					DestroyImmediate (point_BoxSmall_Right_FrameSmall.GetChild (0).gameObject);
				}
			}
			boxSmall_Right_Bool = true;
		}
	}
	//BoxBig Left____________________________________________________________________________________________
	public void BoxBig_Left(){
		if (boxBig_L_Bool == true) {
			if (frame_Int == 1) {
				if (point_SWheel_Left_B_FrameBig.childCount > 0) {
					foreach (Transform obj_D in point_SWheel_Left_B_FrameBig) {
						DestroyImmediate (point_SWheel_Left_B_FrameBig.GetChild (0).gameObject);
					}
					reserve_Left_Bool = true;
				}
				if (point_Tank_Left_FrameBig.childCount > 0) {
					foreach (Transform obj_DB1 in point_Tank_Left_FrameBig) {
						DestroyImmediate (point_Tank_Left_FrameBig.GetChild (0).gameObject);
					}
					tank_Left_Bool = true;
				}
				var obj_BoxB1 = Instantiate (boxBig_Left, point_BoxBig_Left_FrameBig.position, Quaternion.identity);
				obj_BoxB1.transform.parent = point_BoxBig_Left_FrameBig;
			} else if (frame_Int == 2) {
				if (point_S_Whhel_Left_FrameSmall.childCount > 0) {
					foreach (Transform obj_D2 in point_S_Whhel_Left_FrameSmall) {
						DestroyImmediate (point_S_Whhel_Left_FrameSmall.GetChild (0).gameObject);
					}
					reserve_Left_Bool = true;
				}
				if (point_Tank_Left_FrameSmall.childCount > 0) {
					foreach (Transform obj_DB1 in point_Tank_Left_FrameSmall) {
						DestroyImmediate (point_Tank_Left_FrameSmall.GetChild (0).gameObject);
					}
					tank_Left_Bool = true;
				}
				var obj_BoxB2 = Instantiate (boxBig_Left, point_BoxBig_Left_FrameSmall.position, Quaternion.identity);
				obj_BoxB2.transform.parent = point_BoxBig_Left_FrameSmall;
			}
			boxBig_L_Bool = false;
		} else if (boxBig_L_Bool == false) {
			if (frame_Int == 1) {
				foreach (Transform obj_DB1 in point_BoxBig_Left_FrameBig) {
					DestroyImmediate (point_BoxBig_Left_FrameBig.GetChild (0).gameObject);
				}
			} else if (frame_Int == 2) {
				foreach (Transform obj_DB2 in point_BoxBig_Left_FrameSmall) {
					DestroyImmediate (point_BoxBig_Left_FrameSmall.GetChild (0).gameObject);
				}
			}
			boxBig_L_Bool = true;
		}
	}
	//BoxBig Right____________________________________________________________________________________________
	public void BoxBig_Right(){
		if (boxBig_R_Bool == true) {
			if (frame_Int == 1) {
				if (point_Tank_Right_FrameBig.childCount > 0) {
					foreach (Transform obj_DB1 in point_Tank_Right_FrameBig) {
						DestroyImmediate (point_Tank_Right_FrameBig.GetChild (0).gameObject);
					}
					tank_Right_Bool = true;
				}
				var obj_BoxB1 = Instantiate (boxBig_Right, point_BoxBig_Right_FrameBig.position, Quaternion.identity);
				obj_BoxB1.transform.parent = point_BoxBig_Right_FrameBig;
			} else if (frame_Int == 2) {
				if (point_Tank_Right_FrameSmall.childCount > 0) {
					foreach (Transform obj_DB1 in point_Tank_Right_FrameSmall) {
						DestroyImmediate (point_Tank_Right_FrameSmall.GetChild (0).gameObject);
					}
					tank_Right_Bool = true;
				}
				var obj_BoxB2 = Instantiate (boxBig_Right, point_BoxBig_Right_FrameSmall.position, Quaternion.identity);
				obj_BoxB2.transform.parent = point_BoxBig_Right_FrameSmall;
			}
			boxBig_R_Bool = false;
		} else if (boxBig_R_Bool == false) {
			if (frame_Int == 1) {
				foreach (Transform obj_DB1 in point_BoxBig_Right_FrameBig) {
					DestroyImmediate (point_BoxBig_Right_FrameBig.GetChild (0).gameObject);
				}
			} else if (frame_Int == 2) {
				foreach (Transform obj_DB2 in point_BoxBig_Right_FrameSmall) {
					DestroyImmediate (point_BoxBig_Right_FrameSmall.GetChild (0).gameObject);
				}
			}
			boxBig_R_Bool = true;
		}
	}
	//BoxBig Left (Plus)____________________________________________________________________________________________
	public void BoxBig_Left_Plus(){
		if (framePlus_Bool == false) {
			if (boxBig_L_Plus_Bool == true) {
				var obj_BoxB1 = Instantiate (boxBig_Left, point_BoxBig_Left_FramePlus.position, Quaternion.identity);
				obj_BoxB1.transform.parent = point_BoxBig_Left_FramePlus;
				boxBig_L_Plus_Bool = false;
			} else if (boxBig_L_Plus_Bool == false) {
				foreach (Transform obj_DB1 in point_BoxBig_Left_FramePlus) {
					DestroyImmediate (point_BoxBig_Left_FramePlus.GetChild (0).gameObject);
				}
				boxBig_L_Plus_Bool = true;
			}
		}
	}
	//BoxBig Right (Plus)____________________________________________________________________________________________
	public void BoxBig_Right_Plus(){
		if (framePlus_Bool == false) {
			if (boxBig_R_Plus_Bool == true) {
				var obj_BoxB1 = Instantiate (boxBig_Left, point_BoxBig_Right_FramePlus.position, Quaternion.identity);
				obj_BoxB1.transform.parent = point_BoxBig_Right_FramePlus;
				boxBig_R_Plus_Bool = false;
			} else if (boxBig_R_Plus_Bool == false) {
				foreach (Transform obj_DB1 in point_BoxBig_Right_FramePlus) {
					DestroyImmediate (point_BoxBig_Right_FramePlus.GetChild (0).gameObject);
				}
				boxBig_R_Plus_Bool = true;
			}
		}
	}
	//Tank Left____________________________________________________________________________________________
	public void Tank_Left(){
		if (tank_Left_Bool == true) {
			if (frame_Int == 1) {
				if (point_SWheel_Left_B_FrameBig.childCount > 0) {
					foreach (Transform obj_D in point_SWheel_Left_B_FrameBig) {
						DestroyImmediate (point_SWheel_Left_B_FrameBig.GetChild (0).gameObject);
					}
					reserve_Left_Bool = true;
				}
				if (point_BoxBig_Left_FrameBig.childCount > 0) {
					foreach (Transform obj_DB1 in point_BoxBig_Left_FrameBig) {
						DestroyImmediate (point_BoxBig_Left_FrameBig.GetChild (0).gameObject);
					}
					boxBig_L_Bool = true;
				}
				var obj_Tank_L = Instantiate (tank_Left, point_Tank_Left_FrameBig.position, Quaternion.identity);
				obj_Tank_L.transform.parent = point_Tank_Left_FrameBig;
			} else if (frame_Int == 2) {
				if (point_S_Whhel_Left_FrameSmall.childCount > 0) {
					foreach (Transform obj_D2 in point_S_Whhel_Left_FrameSmall) {
						DestroyImmediate (point_S_Whhel_Left_FrameSmall.GetChild (0).gameObject);
					}
					reserve_Left_Bool = true;
				}
					if (point_BoxBig_Left_FrameSmall.childCount > 0) {
					foreach (Transform obj_DB1 in point_BoxBig_Left_FrameSmall) {
						DestroyImmediate (point_BoxBig_Left_FrameSmall.GetChild (0).gameObject);
					}
					boxBig_L_Bool = true;
				}
				var obj_Tank_R = Instantiate (tank_Left, point_Tank_Left_FrameSmall.position, Quaternion.identity);
				obj_Tank_R.transform.parent = point_Tank_Left_FrameSmall;
			}
			tank_Left_Bool = false;
		} else if (tank_Left_Bool == false) {
			if (frame_Int == 1) {
				foreach (Transform obj_DTank_L in point_Tank_Left_FrameBig) {
					DestroyImmediate (point_Tank_Left_FrameBig.GetChild (0).gameObject);
				}
			} else if (frame_Int == 2) {
				foreach (Transform obj_DTank_L in point_Tank_Left_FrameSmall) {
					DestroyImmediate (point_Tank_Left_FrameSmall.GetChild (0).gameObject);
				}
			}
			tank_Left_Bool = true;
		}
	}
	//Reserve Back Left Back____________________________________________________________________________________________
	public void ReserveLeftBack(){
		if (reserve_LeftBack_Bool == true) {
			if (bridge_Bool_2x2 == false) {
				var obj_Tank_R = Instantiate (reserve_Left, point_SWheel_Left_A_FrameBig.position, Quaternion.identity);
				obj_Tank_R.transform.parent = point_SWheel_Left_A_FrameBig;
			}
			reserve_LeftBack_Bool = false;
		} else if (reserve_LeftBack_Bool == false) {
			foreach (Transform obj_D in point_SWheel_Left_A_FrameBig) {
				DestroyImmediate (point_SWheel_Left_A_FrameBig.GetChild (0).gameObject);
			}
			reserve_LeftBack_Bool = true;
		}
	}
	//Tank Right____________________________________________________________________________________________
	public void Tank_Right(){
		if (tank_Right_Bool == true) {
			if (frame_Int == 1) {
				if (point_BoxBig_Right_FrameBig.childCount > 0) {
					foreach (Transform obj_DB1 in point_BoxBig_Right_FrameBig) {
						DestroyImmediate (point_BoxBig_Right_FrameBig.GetChild (0).gameObject);
					}
					boxBig_R_Bool = true;
				}
				var obj_Tank_R = Instantiate (tank_Right, point_Tank_Right_FrameBig.position, Quaternion.identity);
				obj_Tank_R.transform.parent = point_Tank_Right_FrameBig;
			} else if (frame_Int == 2) {
				if (point_BoxBig_Right_FrameSmall.childCount > 0) {
					foreach (Transform obj_DB1 in point_BoxBig_Right_FrameSmall) {
						DestroyImmediate (point_BoxBig_Right_FrameSmall.GetChild (0).gameObject);
					}
					boxBig_R_Bool = true;
				}
				var obj_Tank_R = Instantiate (tank_Right, point_Tank_Right_FrameSmall.position, Quaternion.identity);
				obj_Tank_R.transform.parent = point_Tank_Right_FrameSmall;
			}
			tank_Right_Bool = false;
		} else if (tank_Right_Bool == false) {
			if (frame_Int == 1) {
				foreach (Transform obj_DTank_R in point_Tank_Right_FrameBig) {
					DestroyImmediate (point_Tank_Right_FrameBig.GetChild (0).gameObject);
				}
			} else if (frame_Int == 2) {
				foreach (Transform obj_DTank_R in point_Tank_Right_FrameSmall) {
					DestroyImmediate (point_Tank_Right_FrameSmall.GetChild (0).gameObject);
				}
			}
			tank_Right_Bool = true;
		}
	}
	//Tank Plus____________________________________________________________________________________________
	public void TankPlus(){
		if (framePlus_Bool == false) {
			if (tank_Plus_Bool == true) {
				if (point_Tank_Wheel_FramePlus.childCount > 0) {
					foreach (Transform obj_DWheel in point_Tank_Wheel_FramePlus) {
						DestroyImmediate (point_Tank_Wheel_FramePlus.GetChild (0).gameObject);
					}
					reserveS_Bool = true;
				}
				var obj_Tank = Instantiate (tank, point_Tank_Wheel_FramePlus.position, Quaternion.identity);
				obj_Tank.transform.parent = point_Tank_Wheel_FramePlus;
				tank_Plus_Bool = false;
			} else if (tank_Plus_Bool == false) {
				foreach (Transform obj_DTank in point_Tank_Wheel_FramePlus) {
					DestroyImmediate (point_Tank_Wheel_FramePlus.GetChild (0).gameObject);
				}
				tank_Plus_Bool = true;
			}
		}
	}
	//Reserve____________________________________________________________________________________________
	public void ReserveS(){
		if (framePlus_Bool == false) {
			if (reserveS_Bool == true) {
				if (point_Tank_Wheel_FramePlus.childCount > 0) {
					foreach (Transform obj_DTank in point_Tank_Wheel_FramePlus) {
						DestroyImmediate (point_Tank_Wheel_FramePlus.GetChild (0).gameObject);
					}
					tank_Plus_Bool = true;
				}
				var obj_Reserve = Instantiate (reserveS, point_Tank_Wheel_FramePlus.position, Quaternion.identity);
				obj_Reserve.transform.parent = point_Tank_Wheel_FramePlus;
				reserveS_Bool = false;
			} else if (reserveS_Bool == false) {
				foreach (Transform obj_DR in point_Tank_Wheel_FramePlus) {
					DestroyImmediate (point_Tank_Wheel_FramePlus.GetChild (0).gameObject);
				}
				reserveS_Bool = true;
			}
		}
	}
	public void Reserve_Left(){
		if (reserve_Left_Bool == true) {
			if (frame_Int == 1) {
				if (point_BoxBig_Left_FrameBig.childCount > 0) {
					foreach (Transform obj_D1 in point_BoxBig_Left_FrameBig) {
						DestroyImmediate (point_BoxBig_Left_FrameBig.GetChild (0).gameObject);
					}
					boxBig_L_Bool = true;
				} else if (point_Tank_Left_FrameBig.childCount > 0) {
					foreach (Transform obj_D2 in point_Tank_Left_FrameBig) {
						DestroyImmediate (point_Tank_Left_FrameBig.GetChild (0).gameObject);
					}
					tank_Left_Bool = true;
				}
				var obj_Reserve = Instantiate (reserve_Left, point_SWheel_Left_B_FrameBig.position, Quaternion.identity);
				obj_Reserve.transform.parent = point_SWheel_Left_B_FrameBig;
			} else if (frame_Int == 2) {
				if (point_BoxBig_Left_FrameSmall.childCount > 0) {
					foreach (Transform obj_D1 in point_BoxBig_Left_FrameSmall) {
						DestroyImmediate (point_BoxBig_Left_FrameSmall.GetChild (0).gameObject);
					}
					boxBig_L_Bool = true;
				} else if (point_Tank_Left_FrameSmall.childCount > 0) {
					foreach (Transform obj_D1 in point_Tank_Left_FrameSmall) {
						DestroyImmediate (point_Tank_Left_FrameSmall.GetChild (0).gameObject);
					}
					tank_Left_Bool = true;
				}
				var obj_Reserve = Instantiate (reserve_Left, point_S_Whhel_Left_FrameSmall.position, Quaternion.identity);
				obj_Reserve.transform.parent = point_S_Whhel_Left_FrameSmall;
			}
			reserve_Left_Bool = false;
		} else if (reserve_Left_Bool == false) {
				if (frame_Int == 1) {
					foreach (Transform obj_D1 in point_SWheel_Left_B_FrameBig) {
						DestroyImmediate (point_SWheel_Left_B_FrameBig.GetChild (0).gameObject);
					}
				} else if (frame_Int == 2) {
					foreach (Transform obj_D2 in point_S_Whhel_Left_FrameSmall) {
						DestroyImmediate (point_S_Whhel_Left_FrameSmall.GetChild (0).gameObject);
					}
				}
			reserve_Left_Bool = true;
		}
	}
	//Reserve For____________________________________________________________________________________________
	public void ReserveFor(){
		if (reserve_For_Bool == true) {
			if (frame_Int == 1) {
				var obj_ReserveFor1 = Instantiate (reserveS, point_S_Wheel_FrameBig.position, Quaternion.identity);
				obj_ReserveFor1.transform.parent = point_S_Wheel_FrameBig;
			} else if (frame_Int == 2) {
				var obj_ReserveFor2 = Instantiate (reserveS, point_S_Whhel_FrameSmall.position, Quaternion.identity);
				obj_ReserveFor2.transform.parent = point_S_Whhel_FrameSmall;
			}
			reserve_For_Bool = false;
		} else if (reserve_For_Bool == false) {
			if (frame_Int == 1) {
				foreach (Transform obj_D1 in point_S_Wheel_FrameBig) {
					DestroyImmediate (point_S_Wheel_FrameBig.GetChild (0).gameObject);
				}
			} else if (frame_Int == 2) {
				foreach (Transform obj_D2 in point_S_Whhel_FrameSmall) {
					DestroyImmediate (point_S_Whhel_FrameSmall.GetChild (0).gameObject);
				}
			}
			reserve_For_Bool = true;
		}
	}
	//Destroy GameObject end Next Object______________________________________________________________________________________________
	public void DestroyObj(){
		//Air
		if (point_Air_Left_FrameBig.childCount > 0) {
			foreach (Transform obj_DA1 in point_Air_Left_FrameBig) {
				DestroyImmediate (point_Air_Left_FrameBig.GetChild (0).gameObject);
			}
			air_Left_Bool = true;
		}
		if (point_Air_Right_FrameBig.childCount > 0) {
			foreach (Transform obj_DA2 in point_Air_Right_FrameBig) {
				DestroyImmediate (point_Air_Right_FrameBig.GetChild (0).gameObject);
			}
			air_Right_Bool = true;
		}
		if (point_Air_Left_FrameSmall.childCount > 0) {
			foreach (Transform obj_DA3 in point_Air_Left_FrameSmall) {
				DestroyImmediate (point_Air_Left_FrameSmall.GetChild (0).gameObject);
			}
			air_Left_Bool = true;
		}
		if (point_Air_Right_FrameSmall.childCount > 0) {
			foreach (Transform obj_DA4 in point_Air_Right_FrameSmall) {
				DestroyImmediate (point_Air_Right_FrameSmall.GetChild (0).gameObject);
			}
			air_Right_Bool = true;
		}
		//Battery
		if (point_Battery_Left_FrameBig.childCount > 0) {
			foreach (Transform obj_DA1 in point_Battery_Left_FrameBig) {
				DestroyImmediate (point_Battery_Left_FrameBig.GetChild (0).gameObject);
			}
			battery_Left_Bool = true;
		}
		if (point_Battery_Right_FrameBig.childCount > 0) {
			foreach (Transform obj_DA2 in point_Battery_Right_FrameBig) {
				DestroyImmediate (point_Battery_Right_FrameBig.GetChild (0).gameObject);
			}
			battery_Right_Bool = true;
		}
		if (point_Battery_Left_FrameSmall.childCount > 0) {
			foreach (Transform obj_DA3 in point_Battery_Left_FrameSmall) {
				DestroyImmediate (point_Battery_Left_FrameSmall.GetChild (0).gameObject);
			}
			battery_Left_Bool = true;
		}
		if (point_Battery_Right_FrameSmall.childCount > 0) {
			foreach (Transform obj_DA4 in point_Battery_Right_FrameSmall) {
				DestroyImmediate (point_Battery_Right_FrameSmall.GetChild (0).gameObject);
			}
			battery_Right_Bool = true;
		}
	//BoxSmall
		if (point_BoxSmall_Left_FrameBig.childCount > 0) {
			foreach (Transform obj_DB1 in point_BoxSmall_Left_FrameBig) {
				DestroyImmediate (point_BoxSmall_Left_FrameBig.GetChild (0).gameObject);
			}
			boxSmall_Left_Bool = true;
		}
		if (point_BoxSmall_Right_FrameBig.childCount > 0) {
			foreach (Transform obj_DB2 in point_BoxSmall_Right_FrameBig) {
				DestroyImmediate (point_BoxSmall_Right_FrameBig.GetChild (0).gameObject);
			}
			boxSmall_Right_Bool = true;
		}
		if (point_BoxSmall_Left_FrameSmall.childCount > 0) {
			foreach (Transform obj_DB3 in point_BoxSmall_Left_FrameSmall) {
				DestroyImmediate (point_BoxSmall_Left_FrameSmall.GetChild (0).gameObject);
			}
			boxSmall_Left_Bool = true;
		}
		if (point_BoxSmall_Right_FrameSmall.childCount > 0) {
			foreach (Transform obj_DB4 in point_BoxSmall_Right_FrameSmall) {
				DestroyImmediate (point_BoxSmall_Right_FrameSmall.GetChild (0).gameObject);
			}
			boxSmall_Right_Bool = true;
		}
	//BoxBig
		if (point_BoxBig_Left_FrameBig.childCount > 0) {
			foreach (Transform obj_DB1 in point_BoxBig_Left_FrameBig) {
				DestroyImmediate (point_BoxBig_Left_FrameBig.GetChild (0).gameObject);
			}
			boxBig_L_Bool = true;
		}
		if (point_BoxBig_Left_FrameSmall.childCount > 0) {
			foreach (Transform obj_DB2 in point_BoxBig_Left_FrameSmall) {
				DestroyImmediate (point_BoxBig_Left_FrameSmall.GetChild (0).gameObject);
			}
			boxBig_L_Bool = true;
		}
		if (point_BoxBig_Right_FrameBig.childCount > 0) {
			foreach (Transform obj_DB3 in point_BoxBig_Right_FrameBig) {
				DestroyImmediate (point_BoxBig_Right_FrameBig.GetChild (0).gameObject);
			}
			boxBig_R_Bool = true;
		}
		if (point_BoxBig_Right_FrameSmall.childCount > 0) {
			foreach (Transform obj_DB4 in point_BoxBig_Right_FrameSmall) {
				DestroyImmediate (point_BoxBig_Right_FrameSmall.GetChild (0).gameObject);
			}
			boxBig_R_Bool = true;
		}
		//BoxBig(Plus)
		if (point_BoxBig_Left_FramePlus.childCount > 0) {
			foreach (Transform obj_DB1 in point_BoxBig_Left_FramePlus) {
				DestroyImmediate (point_BoxBig_Left_FramePlus.GetChild (0).gameObject);
			}
		}
		if (point_BoxBig_Right_FramePlus.childCount > 0) {
			foreach (Transform obj_DB2 in point_BoxBig_Right_FramePlus) {
				DestroyImmediate (point_BoxBig_Right_FramePlus.GetChild (0).gameObject);
			}
		}
	//Tank
		if (point_Tank_Left_FrameBig.childCount > 0) {
			foreach (Transform obj_DT1 in point_Tank_Left_FrameBig) {
				DestroyImmediate (point_Tank_Left_FrameBig.GetChild (0).gameObject);
			}
			tank_Left_Bool = true;
		}
		if (point_Tank_Right_FrameBig.childCount > 0) {
			foreach (Transform obj_DT1 in point_Tank_Right_FrameBig) {
				DestroyImmediate (point_Tank_Right_FrameBig.GetChild (0).gameObject);
			}
			tank_Right_Bool = true;
		}
		if (point_Tank_Left_FrameSmall.childCount > 0) {
			foreach (Transform obj_DT1 in point_Tank_Left_FrameSmall) {
				DestroyImmediate (point_Tank_Left_FrameSmall.GetChild (0).gameObject);
			}
			tank_Left_Bool = true;
		}
		if (point_Tank_Right_FrameSmall.childCount > 0) {
			foreach (Transform obj_DT1 in point_Tank_Right_FrameSmall) {
				DestroyImmediate (point_Tank_Right_FrameSmall.GetChild (0).gameObject);
			}
			tank_Right_Bool = true;
		}
	//Tenk end Reserve
		if (point_Tank_Wheel_FramePlus.childCount > 0) {
			foreach (Transform obj_DTW in point_Tank_Wheel_FramePlus) {
				DestroyImmediate (point_Tank_Wheel_FramePlus.GetChild (0).gameObject);
			}
			if (reserveS_Bool == false) {
				reserveS_Bool = true;
			} else if (tank_Plus_Bool == false) {
				tank_Plus_Bool = true;
			}
		}
	//Reserve Left
		if (point_SWheel_Left_B_FrameBig.childCount > 0) {
			foreach (Transform obj_D in point_SWheel_Left_B_FrameBig) {
				DestroyImmediate (point_SWheel_Left_B_FrameBig.GetChild (0).gameObject);
			}
			reserve_Left_Bool = true;
		}
		if (point_S_Whhel_Left_FrameSmall.childCount > 0) {
				foreach (Transform obj_D in point_S_Whhel_Left_FrameSmall) {
					DestroyImmediate (point_S_Whhel_Left_FrameSmall.GetChild (0).gameObject);
				}
				reserve_Left_Bool = true;
			}
		if(point_SWheel_Left_A_FrameBig.childCount > 0){
			foreach (Transform obj_D in point_SWheel_Left_A_FrameBig) {
				DestroyImmediate (point_SWheel_Left_A_FrameBig.GetChild (0).gameObject);
			}
			reserve_LeftBack_Bool = true;
	}
		if (point_S_Wheel_FrameBig.childCount > 0) {
			foreach (Transform obj_D1 in point_S_Wheel_FrameBig) {
				DestroyImmediate (point_S_Wheel_FrameBig.GetChild (0).gameObject);
			}
			reserve_For_Bool = true;
		}
		if (point_S_Whhel_FrameSmall.childCount > 0) {
			foreach (Transform obj_D2 in point_S_Whhel_FrameSmall) {
				DestroyImmediate (point_S_Whhel_FrameSmall.GetChild (0).gameObject);
			}
			reserve_For_Bool = true;
		}
	}
	//Cabin_________________________________________________________________________________________________
	public void CabinSmall(){
		if (cabinSmall_Bool == true) {
			if (point_Cabin.childCount == 0) {
				panelCabin_Int = 1;
				cabin_Bumper_Int = 3;
			}
			if (cabinBig_Bool == false) {
				foreach (Transform obj_CabinBig in point_Cabin) {
					DestroyImmediate (point_Cabin.GetChild (0).gameObject);
				}
				cabinBig_Bool = true;
			}
			var obj_CabinSmall = Instantiate (cabin_Small, point_Cabin.position, Quaternion.identity);
			obj_CabinSmall.transform.parent = point_Cabin;
			//Start Panel Cabin
			if (point_PanelCabin.childCount == 0) {
				var obj_PanelStart = Instantiate (panelCabin_1, point_PanelCabin.position, Quaternion.identity);
				obj_PanelStart.transform.parent = point_PanelCabin;
			}
			//Start Bumper Cabin
			if (point_BumperCabin.childCount == 0) {
				var obj_BumperStart = Instantiate (cabin_Bumper_3, point_BumperCabin.position, Quaternion.identity);
				obj_BumperStart.transform.parent = point_BumperCabin;
			}
			cabin_Int = 1;
			view_Cabin = false;
			//DSG1
			if (dsg1_Bool == false) {
				foreach (Transform obj_DSG1_BigCab in point_DSG1_Big) {
					DestroyImmediate (point_DSG1_Big.GetChild (0).gameObject);
				}
				var obj_DSG1_SmallCab = Instantiate (dsg1, point_DSG1_Small.position, Quaternion.identity);
				obj_DSG1_SmallCab.transform.parent = point_DSG1_Small;
			}
			//DSG4
			if (dsg4_Bool == false) {
				foreach (Transform obj_DSG4_BigCab in point_DSG4_Big) {
					DestroyImmediate (point_DSG4_Big.GetChild (0).gameObject);
				}
				var obj_DSG4_SmallCab = Instantiate (dsg4, point_DSG4_Small.position, Quaternion.identity);
				obj_DSG4_SmallCab.transform.parent = point_DSG4_Small;
			}
			//Trunk
			if (trunkBS_Bool == false) {
				foreach (Transform obj_TrunkBS in point_TrunkBig_Small) {
					DestroyImmediate (point_TrunkBig_Small.GetChild (0).gameObject);
				}
				var obj_TrunkSmall = Instantiate (trunk_Small, point_TrunkBig_Small.position, Quaternion.identity);
				obj_TrunkSmall.transform.parent = point_TrunkBig_Small;
			}
			//Light
			if(light3_Bool == false){
				foreach (Transform obj_Light3BS in point_Light) {
					DestroyImmediate (point_Light.GetChild (0).gameObject);
				}
				var obj_Light3Small = Instantiate (light3Small, point_Light.position, Quaternion.identity);
				obj_Light3Small.transform.parent = point_Light;
			}
		//Manipulator
			if (point_Manipulator_FrameBig_CabBig.childCount > 0 && frame_Int == 1) {
				foreach (Transform obj_ManipD in point_Manipulator_FrameBig_CabBig) {
					DestroyImmediate (point_Manipulator_FrameBig_CabBig.GetChild (0).gameObject);
				}
				if (manipulator1_Bool == false) {
					var obj_Manip1 = Instantiate (manipulator1, point_Manipulator_FrameBig_CabSmall.position, Quaternion.identity);
					obj_Manip1.transform.parent = point_Manipulator_FrameBig_CabSmall;
				} else if (manipulator2_Bool == false) {
					var obj_Manip2 = Instantiate (manipulator2, point_Manipulator_FrameBig_CabSmall.position, Quaternion.identity);
					obj_Manip2.transform.parent = point_Manipulator_FrameBig_CabSmall;
				} else if (manipulator3_Bool == false) {
					var obj_Manip3 = Instantiate (manipulator3, point_Manipulator_FrameBig_CabSmall.position, Quaternion.identity);
					obj_Manip3.transform.parent = point_Manipulator_FrameBig_CabSmall;
				}
				if (colorRed_Manip != 0) {
					if (colorRed_Manip == 1) {
						ColorBlue_Manip ();
					} else if (colorRed_Manip == 2) {
						ColorRed_Manip ();
					} else if (colorRed_Manip == 3) {
						ColorWhite_Manip ();
					} else if (colorRed_Manip == 4) {
						ColorYellow_Manip ();
					}
				}
			}		
			//Block Instantiate Manipulator
			block_Instantiate_Manip = false;
			cabinSmall_Bool = false;
			view_Body = false;
			//Body
			if (body_Bool == false) {
				GreateBody ();
			}
			//Protection
			if (protection_Bool == false) {
				GreateProtection ();
			}
		//Cardan
			Cardan();
			LightPanel ();
			ManipulatorController ();
			//Check Manip
			CheckManipulator();
			//Check Crable
			CheckCrable ();
			AddConponentArrow ();
			//Add Target Camera
			transform.GetChild (62).gameObject.GetComponent<CameraToManipulator> ().target = transform;
		} else if (cabinSmall_Bool == false) {
			foreach (Transform obj_CabinSmall in point_Cabin) {
				DestroyImmediate (point_Cabin.GetChild (0).gameObject);
			}
			//Start Panel Cabin
			foreach (Transform obj_PanelStart in point_PanelCabin) {
				DestroyImmediate (point_PanelCabin.GetChild (0).gameObject);
			}
			//Start Bumper Cabin
			foreach (Transform obj_BumperStart in point_BumperCabin) {
				DestroyImmediate (point_BumperCabin.GetChild (0).gameObject);
			}
			iconTuningCabin_Bool = true;
			view_Cabin = true;
			//DSG1
			if (point_DSG1_Small.childCount > 0) {
				foreach (Transform obj_DSG1_SmallCab in point_DSG1_Small) {
					DestroyImmediate (point_DSG1_Small.GetChild (0).gameObject);
				}
				dsg1_Bool = true;
			} else if (point_DSG1_Big.childCount > 0) {
				foreach (Transform obj_DSG1_BigCab in point_DSG1_Big) {
					DestroyImmediate (point_DSG1_Big.GetChild (0).gameObject);
				}
				dsg1_Bool = true;
			}
			//DSG4
			if (point_DSG4_Small.childCount > 0) {
				foreach (Transform obj_DSG4_SmallCab in point_DSG4_Small) {
					DestroyImmediate (point_DSG4_Small.GetChild (0).gameObject);
				}
				dsg4_Bool = true;
			} else if (point_DSG4_Big.childCount > 0) {
				foreach (Transform obj_DSG4_BigCab in point_DSG4_Big) {
					DestroyImmediate (point_DSG4_Big.GetChild (0).gameObject);
				}
				dsg1_Bool = true;
			}
			//DSG 2 3
			if (dsg23_Bool == false) {
				foreach (Transform obj_DSG23 in point_DSG23) {
					DestroyImmediate (point_DSG23.GetChild (0).gameObject);
				}
				dsg23_Bool = true;
			}
			//Trunk
			if (trunk_Bool == false) {
				foreach (Transform obj_Trunk in point_Trunk) {
					DestroyImmediate (point_Trunk.GetChild (0).gameObject);
				}
				view_Trunk_Bool = true;
				trunk_Bool = true;
			}
			//Trunk Small Big
			if (trunkBS_Bool == false) {
				foreach (Transform obj_TrunkBS in point_TrunkBig_Small) {
					DestroyImmediate (point_TrunkBig_Small.GetChild (0).gameObject);
					trunkBS_Bool = true;
				}
			}
			//Cowl
			if (cowl_Bool == false) {
				foreach (Transform obj_Cowl in point_TrunkBig_Small) {
					DestroyImmediate (point_TrunkBig_Small.GetChild (0).gameObject);
				}
				cowl_Bool = true;
			}
			//Light
			if (light3_Bool == false) {
				foreach (Transform obj_Light3BS in point_Light) {
					DestroyImmediate (point_Light.GetChild (0).gameObject);
				}
				light3_Bool = true;
			}
			if (light2_Bool == false) {
				foreach (Transform obj_Light3BS in point_Light) {
					DestroyImmediate (point_Light.GetChild (0).gameObject);
				}
				light2_Bool = true;
			}
			cabinSmall_Bool = true;
			iconColor_Bool = true;
			cabin_Int = 0;
			cabin_Bumper_Int = 0;
			colorCabin_Int = 1;
			view_Body = true;
			//Destroy Body
			if (body_Bool == false) {
				Body ();
			}
			//Protection
			if (protection_Bool == false) {
				Protection ();
			}
		//Destroy Cardan
			foreach (Transform obj_Car in point_Сardan) {
				DestroyImmediate (point_Сardan.GetChild (0).gameObject);
			}
			if (BodyDop3_Bool == false) {
				BodyDop3_Bool = true;
			}
			if (Cradle_Bool == false) {
				Cradle_Bool = true;
			}
		}
	}
	public void CabinBig(){
		if (cabinBig_Bool == true) {
			if (point_Cabin.childCount == 0) {
				panelCabin_Int = 1;
				cabin_Bumper_Int = 3;
			}
			if (cabinSmall_Bool == false) {
				foreach (Transform obj_CabinSmall in point_Cabin) {
					DestroyImmediate (point_Cabin.GetChild (0).gameObject);
				}
				cabinSmall_Bool = true;
			}
			var obj_CabinBig = Instantiate (cabin_Big, point_Cabin.position, Quaternion.identity);
			obj_CabinBig.transform.parent = point_Cabin;
			//Start Panel Cabin
			if (point_PanelCabin.childCount == 0) {
				var obj_PanelStart = Instantiate (panelCabin_1, point_PanelCabin.position, Quaternion.identity);
				obj_PanelStart.transform.parent = point_PanelCabin;
			}
			//Start Bumper Cabin
			if (point_BumperCabin.childCount == 0) {
				var obj_BumperStart = Instantiate (cabin_Bumper_3, point_BumperCabin.position, Quaternion.identity);
				obj_BumperStart.transform.parent = point_BumperCabin;
			}
			cabin_Int = 2;
			view_Cabin = false;
			//DSG1
			if (dsg1_Bool == false) {
				foreach (Transform obj_DSG1_SmallCab in point_DSG1_Small) {
					DestroyImmediate (point_DSG1_Small.GetChild (0).gameObject);
				}
				var obj_DSG1_BigCab = Instantiate (dsg1, point_DSG1_Big.position, Quaternion.identity);
				obj_DSG1_BigCab.transform.parent = point_DSG1_Big;
			}
			//DSG4
			if (dsg4_Bool == false) {
				foreach (Transform obj_DSG4_SmallCab in point_DSG4_Small) {
					DestroyImmediate (point_DSG4_Small.GetChild (0).gameObject);
				}
				var obj_DSG4_BigCab = Instantiate (dsg4, point_DSG4_Big.position, Quaternion.identity);
				obj_DSG4_BigCab.transform.parent = point_DSG4_Big;
			}
			//Trunk
			if (trunkBS_Bool == false) {
				foreach (Transform obj_TrunkBS in point_TrunkBig_Small) {
					DestroyImmediate (point_TrunkBig_Small.GetChild (0).gameObject);
				}
				var obj_TrunkBig = Instantiate (trunk_Big, point_TrunkBig_Small.position, Quaternion.identity);
				obj_TrunkBig.transform.parent = point_TrunkBig_Small;
			}
			//Light
			if (light3_Bool == false) {
				foreach (Transform obj_Light3BS in point_Light) {
					DestroyImmediate (point_Light.GetChild (0).gameObject);
				}
				var obj_Light3Big = Instantiate (light3Big, point_Light.position, Quaternion.identity);
				obj_Light3Big.transform.parent = point_Light;
			}
			//Manipulator
			if (point_Manipulator_FrameBig_CabSmall.childCount > 0 && frame_Int == 1) {
				foreach (Transform obj_ManipD in point_Manipulator_FrameBig_CabSmall) {
					DestroyImmediate (point_Manipulator_FrameBig_CabSmall.GetChild (0).gameObject);
				}
				if (manipulator1_Bool == false) {
					var obj_Manip1 = Instantiate (manipulator1, point_Manipulator_FrameBig_CabBig.position, Quaternion.identity);
					obj_Manip1.transform.parent = point_Manipulator_FrameBig_CabBig;
				} else if (manipulator2_Bool == false) {
					var obj_Manip2 = Instantiate (manipulator2, point_Manipulator_FrameBig_CabBig.position, Quaternion.identity);
					obj_Manip2.transform.parent = point_Manipulator_FrameBig_CabBig;
				} else if (manipulator3_Bool == false) {
					var obj_Manip3 = Instantiate (manipulator3, point_Manipulator_FrameBig_CabBig.position, Quaternion.identity);
					obj_Manip3.transform.parent = point_Manipulator_FrameBig_CabBig;
				}
				if (colorRed_Manip != 0) {
					if (colorRed_Manip == 1) {
						ColorBlue_Manip ();
					} else if (colorRed_Manip == 2) {
						ColorRed_Manip ();
					} else if (colorRed_Manip == 3) {
						ColorWhite_Manip ();
					} else if (colorRed_Manip == 4) {
						ColorYellow_Manip ();
					}
				}
			}
			//Destroy Manipulator If Cabin big FrameSmall
			if (frame_Int == 2 && point_Manipulator_FrameSmall.childCount > 0) {
				foreach (Transform obj_Manip in point_Manipulator_FrameSmall) {
					DestroyImmediate (point_Manipulator_FrameSmall.GetChild (0).gameObject);
				}
				if (manipulator1_Bool == false) {
					manipulator1_Bool = true;
				} else if (manipulator2_Bool == false) {
					manipulator2_Bool = true;
				}else if (manipulator3_Bool == false) {
					manipulator3_Bool = true;
				}
				check_OnBody = true;
				//Block Manip
				if (controllerTruck_Bool == false) {
					gameObject.GetComponent<ControllerTruck> ().blockManip_Bool = false;
				}
				if (view_Crable == false) {
					view_Crable = true;
				}
				//Destroy Crable
				foreach (Transform obj_Car in point_Manipulator_FrameSmall) {
						DestroyImmediate (point_Manipulator_FrameSmall.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(5).transform.GetChild(0).gameObject);
					}
				if (Cradle_Bool == false) {
					Cradle_Bool = true;
				}
			}
			//Block Instantiate Manipulator
			if (frame_Int == 2) {
				block_Instantiate_Manip = true;
			}
			cabinBig_Bool = false;
			view_Body = false;
			//Body
			if (body_Bool == false) {
				GreateBody ();
			}
			//Protection
			if (protection_Bool == false) {
				GreateProtection ();
			}
			Cardan ();
			LightPanel ();
			ManipulatorController ();
			//Check Manip
			CheckManipulator();
			//Check Crable
			CheckCrable ();
			AddConponentArrow ();
			//Add Target Camera
			transform.GetChild (62).gameObject.GetComponent<CameraToManipulator> ().target = transform;
		} else if (cabinBig_Bool == false) {
			foreach (Transform obj_CabinBig in point_Cabin) {
				DestroyImmediate (point_Cabin.GetChild (0).gameObject);
			}
			//Start Panel Cabin
			foreach (Transform obj_PanelStart in point_PanelCabin) {
				DestroyImmediate (point_PanelCabin.GetChild (0).gameObject);
			}
			//Start Bumper Cabin
			foreach (Transform obj_BumperStart in point_BumperCabin) {
				DestroyImmediate (point_BumperCabin.GetChild (0).gameObject);
			}
			iconTuningCabin_Bool = true;
			view_Cabin = true;
			//DSG1
			if (point_DSG1_Small.childCount > 0) {
				foreach (Transform obj_DSG1_SmallCab in point_DSG1_Small) {
					DestroyImmediate (point_DSG1_Small.GetChild (0).gameObject);
				}
				dsg1_Bool = true;
			} else if (point_DSG1_Big.childCount > 0) {
				foreach (Transform obj_DSG1_BigCab in point_DSG1_Big) {
					DestroyImmediate (point_DSG1_Big.GetChild (0).gameObject);
				}
				dsg1_Bool = true;
			}
			//DSG4
			if (point_DSG4_Small.childCount > 0) {
				foreach (Transform obj_DSG4_SmallCab in point_DSG4_Small) {
					DestroyImmediate (point_DSG4_Small.GetChild (0).gameObject);
				}
				dsg4_Bool = true;
			} else if (point_DSG4_Big.childCount > 0) {
				foreach (Transform obj_DSG4_BigCab in point_DSG4_Big) {
					DestroyImmediate (point_DSG4_Big.GetChild (0).gameObject);
				}
				dsg4_Bool = true;
			}
			//DSG 2 3
			if (dsg23_Bool == false) {
				foreach (Transform obj_DSG23 in point_DSG23) {
					DestroyImmediate (point_DSG23.GetChild (0).gameObject);
				}
				dsg23_Bool = true;
			}
			//Trunk
			if (trunk_Bool == false) {
				foreach (Transform obj_Trunk in point_Trunk) {
					DestroyImmediate (point_Trunk.GetChild (0).gameObject);
				}
				view_Trunk_Bool = true;
				trunk_Bool = true;
			}
			//Trunk Small Big
			if (trunkBS_Bool == false) {
				foreach (Transform obj_TrunkBS in point_TrunkBig_Small) {
					DestroyImmediate (point_TrunkBig_Small.GetChild (0).gameObject);
					trunkBS_Bool = true;
				}
			}
			//Cowl
			if (cowl_Bool == false) {
				foreach (Transform obj_Cowl in point_TrunkBig_Small) {
					DestroyImmediate (point_TrunkBig_Small.GetChild (0).gameObject);
				}
				cowl_Bool = true;
			}
			//Light
			if (light3_Bool == false) {
				foreach (Transform obj_Light3BS in point_Light) {
					DestroyImmediate (point_Light.GetChild (0).gameObject);
				}
				light3_Bool = true;
			}
			if (light2_Bool == false) {
				foreach (Transform obj_Light3BS in point_Light) {
					DestroyImmediate (point_Light.GetChild (0).gameObject);
				}
				light2_Bool = true;
			}
			cabinBig_Bool = true;
			iconColor_Bool = true;
			cabin_Int = 0;
			//Block Instantiate Manipulator
			block_Instantiate_Manip = false;
			cabin_Bumper_Int = 0;
			colorCabin_Int = 1;
			view_Body = true;
			//Destroy Body
			if (body_Bool == false) {
				Body ();
			}
			//Protection
			if (protection_Bool == false) {
				Protection ();
			}
			//Destroy Cardan
			foreach (Transform obj_Car in point_Сardan) {
				DestroyImmediate (point_Сardan.GetChild (0).gameObject);
			}
			if (check_OnBody == false) {
				check_OnBody = true;
			}
			if (BodyDop3_Bool == false) {
				BodyDop3_Bool = true;
			}
			if (Cradle_Bool == false) {
				Cradle_Bool = true;
			}
		}
	}
	// Panel Cabin_________________________________________________________________________________________________
	public void PanelCabin(){
		if (panelCabin_Int == 1) {
			foreach (Transform obj_PanelCab_1D in point_PanelCabin) {
				DestroyImmediate (point_PanelCabin.GetChild (0).gameObject);
			}
			var obj_PanelCab_1 = Instantiate (panelCabin_1, point_PanelCabin.position, Quaternion.identity);
			obj_PanelCab_1.transform.parent = point_PanelCabin;
			CheckMaterialPanel ();
		} else if (panelCabin_Int == 2) {
			foreach (Transform obj_PanelCab_2D in point_PanelCabin) {
				DestroyImmediate (point_PanelCabin.GetChild (0).gameObject);
			}
			var obj_PanelCab_2 = Instantiate (panelCabin_2, point_PanelCabin.position, Quaternion.identity);
			obj_PanelCab_2.transform.parent = point_PanelCabin;
			CheckMaterialPanel ();
		} else if (panelCabin_Int == 3) {
			foreach (Transform obj_PanelCab_3D in point_PanelCabin) {
				DestroyImmediate (point_PanelCabin.GetChild (0).gameObject);
			}
			var obj_PanelCab_3 = Instantiate (panelCabin_3, point_PanelCabin.position, Quaternion.identity);
			obj_PanelCab_3.transform.parent = point_PanelCabin;
			CheckMaterialPanel ();
		} else if (panelCabin_Int == 4) {
			foreach (Transform obj_PanelCab_4D in point_PanelCabin) {
				DestroyImmediate (point_PanelCabin.GetChild (0).gameObject);
			}
			var obj_PanelCab_4 = Instantiate (panelCabin_4, point_PanelCabin.position, Quaternion.identity);
			obj_PanelCab_4.transform.parent = point_PanelCabin;
			CheckMaterialPanel ();
		} else if (panelCabin_Int == 5) {
			foreach (Transform obj_PanelCab_5D in point_PanelCabin) {
				DestroyImmediate (point_PanelCabin.GetChild (0).gameObject);
			}
			var obj_PanelCab_5 = Instantiate (panelCabin_5, point_PanelCabin.position, Quaternion.identity);
			obj_PanelCab_5.transform.parent = point_PanelCabin;
			CheckMaterialPanel ();
		} else if (panelCabin_Int == 6) {
			foreach (Transform obj_PanelCab_6D in point_PanelCabin) {
				DestroyImmediate (point_PanelCabin.GetChild (0).gameObject);
			}
			var obj_PanelCab_6 = Instantiate (panelCabin_6, point_PanelCabin.position, Quaternion.identity);
			obj_PanelCab_6.transform.parent = point_PanelCabin;
			CheckMaterialPanel ();
		} else if (panelCabin_Int == 7) {
			foreach (Transform obj_PanelCab_7D in point_PanelCabin) {
				DestroyImmediate (point_PanelCabin.GetChild (0).gameObject);
			}
			var obj_PanelCab_7 = Instantiate (panelCabin_7, point_PanelCabin.position, Quaternion.identity);
			obj_PanelCab_7.transform.parent = point_PanelCabin;
			CheckMaterialPanel ();
		}
		LightPanel ();
	}
	//Bumper Cabin_________________________________________________________________________________________________
	public void CabinBumper(){
		if (cabin_Bumper_Int == 1) {
			foreach (Transform obj_Bumper_1D in point_BumperCabin) {
				DestroyImmediate (point_BumperCabin.GetChild (0).gameObject);
			}
			var obj_Bumper_1 = Instantiate (cabin_Bumper_1, point_BumperCabin.position, Quaternion.identity);
			obj_Bumper_1.transform.parent = point_BumperCabin;
			//DSG 2 3
			if (dsg23_Bool == false) {
				foreach (Transform obj_DSG23 in point_DSG23) {
					DestroyImmediate (point_DSG23.GetChild (0).gameObject);
				}
				var obj_DSG3B = Instantiate (dsg3_B, point_DSG23.position, Quaternion.identity);
				obj_DSG3B.transform.parent = point_DSG23;
			}
			if (colorCabin_Int == 1 || colorCabin_Int == 7) {
				point_BumperCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = dop_White_diffuse_R;
			} else if (colorCabin_Int == 2 || colorCabin_Int == 8) {
				point_BumperCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = dop_Red_diffuse_R;
			} else if (colorCabin_Int == 3 || colorCabin_Int == 9) {
				point_BumperCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = dop_Yellow_diffuse_R;
			} else if (colorCabin_Int == 4 || colorCabin_Int == 10) {
				point_BumperCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = dop_Brown_diffuse_R;
			} else if (colorCabin_Int == 5 || colorCabin_Int == 11) {
				point_BumperCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = dop_Blue_diffuse_R;
			} else if (colorCabin_Int == 6 || colorCabin_Int == 12) {
				point_BumperCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = dop_Green_diffuse_R;
			}
			cabin_Bumper_Int = 1;
			materialBumper_Int = 1;
		} else if (cabin_Bumper_Int == 2) {
			foreach (Transform obj_Bumper_2D in point_BumperCabin) {
				DestroyImmediate (point_BumperCabin.GetChild (0).gameObject);
			}
			var obj_Bumper_2 = Instantiate (cabin_Bumper_2, point_BumperCabin.position, Quaternion.identity);
			obj_Bumper_2.transform.parent = point_BumperCabin;
			//DSG 2 3
			if (dsg23_Bool == false) {
				foreach (Transform obj_DSG23 in point_DSG23) {
					DestroyImmediate (point_DSG23.GetChild (0).gameObject);
				}
				var obj_DSG2S = Instantiate (dsg2_S, point_DSG23.position, Quaternion.identity);
				obj_DSG2S.transform.parent = point_DSG23;
			}
			if (colorCabin_Int == 1 || colorCabin_Int == 7) {
				point_BumperCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = dop_White_diffuse_R;
			} else if (colorCabin_Int == 2 || colorCabin_Int == 8) {
				point_BumperCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = dop_Red_diffuse_R;
			} else if (colorCabin_Int == 3 || colorCabin_Int == 9) {
				point_BumperCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = dop_Yellow_diffuse_R;
			} else if (colorCabin_Int == 4 || colorCabin_Int == 10) {
				point_BumperCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = dop_Brown_diffuse_R;
			} else if (colorCabin_Int == 5 || colorCabin_Int == 11) {
				point_BumperCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = dop_Blue_diffuse_R;
			} else if (colorCabin_Int == 6 || colorCabin_Int == 12) {
				point_BumperCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = dop_Green_diffuse_R;
			}
			cabin_Bumper_Int = 2;
			materialBumper_Int = 1;
		} else if (cabin_Bumper_Int == 3) {
			foreach (Transform obj_Bumper_3D in point_BumperCabin) {
				DestroyImmediate (point_BumperCabin.GetChild (0).gameObject);
			}
			var obj_Bumper_3 = Instantiate (cabin_Bumper_3, point_BumperCabin.position, Quaternion.identity);
			obj_Bumper_3.transform.parent = point_BumperCabin;
			//DSG 2 3
			if (dsg23_Bool == false) {
				foreach (Transform obj_DSG23 in point_DSG23) {
					DestroyImmediate (point_DSG23.GetChild (0).gameObject);
				}
				var obj_DSG2S = Instantiate (dsg2_S, point_DSG23.position, Quaternion.identity);
				obj_DSG2S.transform.parent = point_DSG23;
			}
			cabin_Bumper_Int = 3;
			materialBumper_Int = 0;
		}
	}
	//DSG1_________________________________________________________________________________________________
	public void DSG1(){
		if (dsg1_Bool == true) {
			if (cabin_Int == 1) {
				var obj_DSG1_SmallCab = Instantiate (dsg1, point_DSG1_Small.position, Quaternion.identity);
				obj_DSG1_SmallCab.transform.parent = point_DSG1_Small;
				if (colorCabin_Int == 1 || colorCabin_Int == 7) {
					point_DSG1_Small.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_White_diffuse_R;
				} else if (colorCabin_Int == 2 || colorCabin_Int == 8) {
					point_DSG1_Small.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Red_diffuse_R;
				} else if (colorCabin_Int == 3 || colorCabin_Int == 9) {
					point_DSG1_Small.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Yellow_diffuse_R;
				} else if (colorCabin_Int == 4 || colorCabin_Int == 10) {
					point_DSG1_Small.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Brown_diffuse_R;
				} else if (colorCabin_Int == 5 || colorCabin_Int == 11) {
					point_DSG1_Small.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Blue_diffuse_R;
				} else if (colorCabin_Int == 6 || colorCabin_Int == 12) {
					point_DSG1_Small.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Green_diffuse_R;
				}
			}
			if (cabin_Int == 2) {
				var obj_DSG1_BigCab = Instantiate (dsg1, point_DSG1_Big.position, Quaternion.identity);
				obj_DSG1_BigCab.transform.parent = point_DSG1_Big;
				if (colorCabin_Int == 1 || colorCabin_Int == 7) {
					point_DSG1_Big.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_White_diffuse_R;
				} else if (colorCabin_Int == 2 || colorCabin_Int == 8) {
					point_DSG1_Big.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Red_diffuse_R;
				} else if (colorCabin_Int == 3 || colorCabin_Int == 9) {
					point_DSG1_Big.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Yellow_diffuse_R;
				} else if (colorCabin_Int == 4 || colorCabin_Int == 10) {
					point_DSG1_Big.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Brown_diffuse_R;
				} else if (colorCabin_Int == 5 || colorCabin_Int == 11) {
					point_DSG1_Big.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Blue_diffuse_R;
				} else if (colorCabin_Int == 6 || colorCabin_Int == 12) {
					point_DSG1_Big.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Green_diffuse_R;
				}
			}
			dsg1_Bool = false;
		} else if (dsg1_Bool == false) {
			if (point_DSG1_Small.childCount > 0) {
				foreach (Transform obj_DSG1_SmallCab in point_DSG1_Small) {
					DestroyImmediate (point_DSG1_Small.GetChild (0).gameObject);
				}
			} else if (point_DSG1_Big.childCount > 0) {
				foreach (Transform obj_DSG1_BigCab in point_DSG1_Big) {
					DestroyImmediate (point_DSG1_Big.GetChild (0).gameObject);
				}
			}
			dsg1_Bool = true;
		}
	}
	//DSG23_________________________________________________________________________________________________
	public void DSG23(){
		if (dsg23_Bool == true) {
			if (cabin_Bumper_Int == 1) {
				var obj_DSG3B = Instantiate (dsg3_B, point_DSG23.position, Quaternion.identity);
				obj_DSG3B.transform.parent = point_DSG23;
			} else if (cabin_Bumper_Int == 2 || cabin_Bumper_Int == 3) {
				var obj_DSG2S = Instantiate (dsg2_S, point_DSG23.position, Quaternion.identity);
				obj_DSG2S.transform.parent = point_DSG23;
			}
			dsg23_Bool = false;
		} else if (dsg23_Bool == false) {
			foreach (Transform obj_DSG23 in point_DSG23) {
				DestroyImmediate (point_DSG23.GetChild (0).gameObject);
			}
			dsg23_Bool = true;
		}
	}
	//DSG4_________________________________________________________________________________________________
	public void DSG4(){
		if (dsg4_Bool == true) {
			if (cabin_Int == 1) {
				var obj_DSG4_SmallCab = Instantiate (dsg4, point_DSG4_Small.position, Quaternion.identity);
				obj_DSG4_SmallCab.transform.parent = point_DSG4_Small;
			}
			if (cabin_Int == 2) {
				var obj_DSG4_BigCab = Instantiate (dsg4, point_DSG4_Big.position, Quaternion.identity);
				obj_DSG4_BigCab.transform.parent = point_DSG4_Big;
			}
			dsg4_Bool = false;
		} else if (dsg4_Bool == false) {
			if (point_DSG4_Small.childCount > 0) {
				foreach (Transform obj_DSG4_SmallCab in point_DSG4_Small) {
					DestroyImmediate (point_DSG4_Small.GetChild (0).gameObject);
				}
			} else if (point_DSG4_Big.childCount > 0) {
				foreach (Transform obj_DSG4_BigCab in point_DSG4_Big) {
					DestroyImmediate (point_DSG4_Big.GetChild (0).gameObject);
				}
			}
			dsg4_Bool = true;
		}
	}
	//Trunk_________________________________________________________________________________________________
	public void Trunk(){
		if (trunk_Bool == true) {
			if (light2_Bool == false) {
				foreach (Transform obj_Light3BS in point_Light) {
					DestroyImmediate (point_Light.GetChild (0).gameObject);
				}
				light2_Bool = true;
			}
			if (light3_Bool == false) {
				foreach (Transform obj_Light3BS in point_Light) {
					DestroyImmediate (point_Light.GetChild (0).gameObject);
				}
				light3_Bool = true;
			}
			var obj_Trunk = Instantiate (trunk, point_Trunk.position, Quaternion.identity);
			obj_Trunk.transform.parent = point_Trunk;
			view_Trunk_Bool = false;
			trunk_Bool = false;
		} else if (trunk_Bool == false) {
			foreach (Transform obj_Trunk in point_Trunk) {
				DestroyImmediate (point_Trunk.GetChild (0).gameObject);
			}
			//Trunk Small Big
			if (trunkBS_Bool == false || cowl_Bool == false) {
				foreach (Transform obj_TrunkBS in point_TrunkBig_Small) {
					DestroyImmediate (point_TrunkBig_Small.GetChild (0).gameObject);
					trunkBS_Bool = true;
				}
				cowl_Bool = true;
			}
			view_Trunk_Bool = true;
			trunk_Bool = true;
		}
	}
	//Trunk Big Small_________________________________________________________________________________________________
	public void Trunk_Big_Small(){
		if (trunkBS_Bool == true) {
			if (cowl_Bool == false) {
				Cowl ();
			}
			if (cabin_Int == 1) {
				var obj_TrunkSmall = Instantiate (trunk_Small, point_TrunkBig_Small.position, Quaternion.identity);
				obj_TrunkSmall.transform.parent = point_TrunkBig_Small;
			}else if (cabin_Int == 2) {
				var obj_TrunkBig = Instantiate (trunk_Big, point_TrunkBig_Small.position, Quaternion.identity);
				obj_TrunkBig.transform.parent = point_TrunkBig_Small;
			}
			if (colorCabin_Int == 1 || colorCabin_Int == 7) {
				point_TrunkBig_Small.GetChild (0).transform.GetChild(0).gameObject.GetComponent<Renderer> ().material = dop_White_diffuse_R;
			} else if (colorCabin_Int == 2 || colorCabin_Int == 8) {
				point_TrunkBig_Small.GetChild (0).transform.GetChild(0).gameObject.GetComponent<Renderer> ().material = dop_Red_diffuse_R;
			} else if (colorCabin_Int == 3 || colorCabin_Int == 9) {
				point_TrunkBig_Small.GetChild (0).transform.GetChild(0).gameObject.GetComponent<Renderer> ().material = dop_Yellow_diffuse_R;
			} else if (colorCabin_Int == 4 || colorCabin_Int == 10) {
				point_TrunkBig_Small.GetChild (0).transform.GetChild(0).gameObject.GetComponent<Renderer> ().material = dop_Brown_diffuse_R;
			} else if (colorCabin_Int == 5 || colorCabin_Int == 11) {
				point_TrunkBig_Small.GetChild (0).transform.GetChild(0).gameObject.GetComponent<Renderer> ().material = dop_Blue_diffuse_R;
			} else if (colorCabin_Int == 6 || colorCabin_Int == 12) {
				point_TrunkBig_Small.GetChild (0).transform.GetChild(0).gameObject.GetComponent<Renderer> ().material = dop_Green_diffuse_R;
			}
			trunkBS_Bool = false;
		} else if (trunkBS_Bool == false) {
			foreach (Transform obj_TrunkBS in point_TrunkBig_Small) {
				DestroyImmediate (point_TrunkBig_Small.GetChild (0).gameObject);
			}
			trunkBS_Bool = true;
		}
	}
	//Cowl_________________________________________________________________________________________________
	public void Cowl(){
		if (cowl_Bool == true) {
			if (trunkBS_Bool == false) {
				Trunk_Big_Small ();
			}
			var obj_Cowl = Instantiate (cowl, point_TrunkBig_Small.position, Quaternion.identity);
			obj_Cowl.transform.parent = point_TrunkBig_Small;
			cowl_Bool = false;
			if (colorCabin_Int == 1 || colorCabin_Int == 7) {
				point_TrunkBig_Small.GetChild (0).transform.GetChild(0).gameObject.GetComponent<Renderer> ().material = dop_White_diffuse_R;
			} else if (colorCabin_Int == 2 || colorCabin_Int == 8) {
				point_TrunkBig_Small.GetChild (0).transform.GetChild(0).gameObject.GetComponent<Renderer> ().material = dop_Red_diffuse_R;
			} else if (colorCabin_Int == 3 || colorCabin_Int == 9) {
				point_TrunkBig_Small.GetChild (0).transform.GetChild(0).gameObject.GetComponent<Renderer> ().material = dop_Yellow_diffuse_R;
			} else if (colorCabin_Int == 4 || colorCabin_Int == 10) {
				point_TrunkBig_Small.GetChild (0).transform.GetChild(0).gameObject.GetComponent<Renderer> ().material = dop_Brown_diffuse_R;
			} else if (colorCabin_Int == 5 || colorCabin_Int == 11) {
				point_TrunkBig_Small.GetChild (0).transform.GetChild(0).gameObject.GetComponent<Renderer> ().material = dop_Blue_diffuse_R;
			} else if (colorCabin_Int == 6 || colorCabin_Int == 12) {
				point_TrunkBig_Small.GetChild (0).transform.GetChild(0).gameObject.GetComponent<Renderer> ().material = dop_Green_diffuse_R;
			}
		} else if (cowl_Bool == false) {
			foreach (Transform obj_Cowl in point_TrunkBig_Small) {
				DestroyImmediate (point_TrunkBig_Small.GetChild (0).gameObject);
			}
			cowl_Bool = true;
		}
	}
	//Light_________________________________________________________________________________________________
	public void Light2(){
		if (light2_Bool == true) {
			if (trunk_Bool == false) {
				Trunk ();
			}
			if (light3_Bool == false) {
				Light3 ();
			}
			var obj_Light2 = Instantiate (light2, point_Light.position, Quaternion.identity);
			obj_Light2.transform.parent = point_Light;
			light2_Bool = false;
		} else if (light2_Bool == false) {
			foreach (Transform obj_Light2 in point_Light) {
				DestroyImmediate (point_Light.GetChild (0).gameObject);
			}
			light2_Bool = true;
			if (cowl_Bool == false) {
				cowl_Bool = true;
			}
		}
	}
	public void Light3(){
		if (light3_Bool == true) {
			if (trunk_Bool == false) {
				Trunk ();
			}
			if (light2_Bool == false) {
				Light2 ();
			}
			if (cabin_Int == 1) {
				var obj_Light3Small = Instantiate (light3Small, point_Light.position, Quaternion.identity);
				obj_Light3Small.transform.parent = point_Light;
			}
			if (cabin_Int == 2) {
				var obj_Light3Big = Instantiate (light3Big, point_Light.position, Quaternion.identity);
				obj_Light3Big.transform.parent = point_Light;
			}
			light3_Bool = false;
		} else if (light3_Bool == false) {
			foreach (Transform obj_Light3BS in point_Light) {
				DestroyImmediate (point_Light.GetChild (0).gameObject);
			}
			light3_Bool = true;
			if (cowl_Bool == false) {
				cowl_Bool = true;
			}
		}
	}
	//Material_________________________________________________________________________________________________
	public void CabinMaterial(){
		if(cabinBig_Bool == false || cabinSmall_Bool == false){
		if (colorCabin_Int == 1) {
			point_Cabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_White_diffuse_R;
			if (panelCabin_Int == 1 || panelCabin_Int == 2 || panelCabin_Int == 3) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_White_diffuse_R;
			} else if (panelCabin_Int == 4 || panelCabin_Int == 5 || panelCabin_Int == 6 || panelCabin_Int == 7) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Pan_Cabin_White_diffuse_R;
			}
			if (materialBumper_Int == 1) {
				point_BumperCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = dop_White_diffuse_R;
			}
				if (cowl_Bool == false || trunkBS_Bool == false) {
				point_TrunkBig_Small.GetChild (0).transform.GetChild(0).gameObject.GetComponent<Renderer> ().material = dop_White_diffuse_R;
			}
			
			if (point_DSG1_Small.childCount > 0) {
				point_DSG1_Small.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_White_diffuse_R;
			}
			if (point_DSG1_Big.childCount > 0) {
				point_DSG1_Big.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_White_diffuse_R;
			}
		}
		if (colorCabin_Int == 2) {
			point_Cabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Red_diffuse_R;
			if (panelCabin_Int == 1 || panelCabin_Int == 2 || panelCabin_Int == 3) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Red_diffuse_R;
			} else if (panelCabin_Int == 4 || panelCabin_Int == 5 || panelCabin_Int == 6 || panelCabin_Int == 7) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Pan_Cabin_Red_diffuse_R;
			}
			if (materialBumper_Int == 1) {
				point_BumperCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = dop_Red_diffuse_R;
			}
				if (cowl_Bool == false || trunkBS_Bool == false) {
				point_TrunkBig_Small.GetChild (0).transform.GetChild(0).gameObject.GetComponent<Renderer> ().material = dop_Red_diffuse_R;
			}
			if (point_DSG1_Small.childCount > 0) {
				point_DSG1_Small.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Red_diffuse_R;
			}
			if (point_DSG1_Big.childCount > 0) {
				point_DSG1_Big.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Red_diffuse_R;
			}
		}
		if (colorCabin_Int == 3) {
			point_Cabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Yellow_diffuse_R;
			if (panelCabin_Int == 1 || panelCabin_Int == 2 || panelCabin_Int == 3) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Yellow_diffuse_R;
			} else if (panelCabin_Int == 4 || panelCabin_Int == 5 || panelCabin_Int == 6 || panelCabin_Int == 7) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Pan_Cabin_Yellow_diffuse_R;
			}
			if (materialBumper_Int == 1) {
				point_BumperCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = dop_Yellow_diffuse_R;
			}
				if (cowl_Bool == false || trunkBS_Bool == false) {
				point_TrunkBig_Small.GetChild (0).transform.GetChild(0).gameObject.GetComponent<Renderer> ().material = dop_Yellow_diffuse_R;
			}
			if (point_DSG1_Small.childCount > 0) {
				point_DSG1_Small.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Yellow_diffuse_R;
			}
			if (point_DSG1_Big.childCount > 0) {
				point_DSG1_Big.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Yellow_diffuse_R;
			}
		}
		if (colorCabin_Int == 4) {
			point_Cabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Brown_diffuse_R;
			if (panelCabin_Int == 1 || panelCabin_Int == 2 || panelCabin_Int == 3) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Brown_diffuse_R;
			} else if (panelCabin_Int == 4 || panelCabin_Int == 5 || panelCabin_Int == 6 || panelCabin_Int == 7) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Pan_Cabin_Brown_diffuse_R;
			}
			if (materialBumper_Int == 1) {
				point_BumperCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = dop_Brown_diffuse_R;
			}
				if (cowl_Bool == false || trunkBS_Bool == false) {
				point_TrunkBig_Small.GetChild (0).transform.GetChild(0).gameObject.GetComponent<Renderer> ().material = dop_Brown_diffuse_R;
			}
			if (point_DSG1_Small.childCount > 0) {
				point_DSG1_Small.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Brown_diffuse_R;
			}
			if (point_DSG1_Big.childCount > 0) {
				point_DSG1_Big.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Brown_diffuse_R;
			}
		}
		if (colorCabin_Int == 5) {
			point_Cabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Blue_diffuse_R;
			if (panelCabin_Int == 1 || panelCabin_Int == 2 || panelCabin_Int == 3) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Blue_diffuse_R;
			}else if (panelCabin_Int == 4 || panelCabin_Int == 5 || panelCabin_Int == 6 || panelCabin_Int == 7) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Pan_Cabin_Blue_diffuse_R;
			}
			if (materialBumper_Int == 1) {
				point_BumperCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = dop_Blue_diffuse_R;
			}
				if (cowl_Bool == false || trunkBS_Bool == false) {
				point_TrunkBig_Small.GetChild (0).transform.GetChild(0).gameObject.GetComponent<Renderer> ().material = dop_Blue_diffuse_R;
			}
			if (point_DSG1_Small.childCount > 0) {
				point_DSG1_Small.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Blue_diffuse_R;
			}
			if (point_DSG1_Big.childCount > 0) {
				point_DSG1_Big.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Blue_diffuse_R;
			}
		}
		if (colorCabin_Int == 6) {
			point_Cabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Green_diffuse_R;
			if (panelCabin_Int == 1 || panelCabin_Int == 2 || panelCabin_Int == 3) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Green_diffuse_R;
			}else if (panelCabin_Int == 4 || panelCabin_Int == 5 || panelCabin_Int == 6 || panelCabin_Int == 7) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Pan_Cabin_Green_diffuse_R;
			}
			if (materialBumper_Int == 1) {
				point_BumperCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = dop_Green_diffuse_R;
			}
				if (cowl_Bool == false || trunkBS_Bool == false) {
				point_TrunkBig_Small.GetChild (0).transform.GetChild(0).gameObject.GetComponent<Renderer> ().material = dop_Green_diffuse_R;
			}
			if (point_DSG1_Small.childCount > 0) {
				point_DSG1_Small.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Green_diffuse_R;
			}
			if (point_DSG1_Big.childCount > 0) {
				point_DSG1_Big.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Green_diffuse_R;
			}
		}
		if (colorCabin_Int == 7) {
			point_Cabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_White_Plus_diffuse_R;
			if (panelCabin_Int == 1 || panelCabin_Int == 2 || panelCabin_Int == 3) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_White_Plus_diffuse_R;
			} else if (panelCabin_Int == 4 || panelCabin_Int == 5 || panelCabin_Int == 6 || panelCabin_Int == 7) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Pan_Cabin_White_Plus_diffuse_R;
			}
			if (materialBumper_Int == 1) {
				point_BumperCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = dop_White_diffuse_R;
			}
				if (cowl_Bool == false || trunkBS_Bool == false) {
				point_TrunkBig_Small.GetChild (0).transform.GetChild(0).gameObject.GetComponent<Renderer> ().material = dop_White_diffuse_R;
			}
			if (point_DSG1_Small.childCount > 0) {
				point_DSG1_Small.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_White_Plus_diffuse_R;
			}
			if (point_DSG1_Big.childCount > 0) {
				point_DSG1_Big.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_White_Plus_diffuse_R;
			}
		}
		if (colorCabin_Int == 8) {
			point_Cabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Red_Plus_diffuse_R;
			if (panelCabin_Int == 1 || panelCabin_Int == 2 || panelCabin_Int == 3) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Red_Plus_diffuse_R;
			} else if (panelCabin_Int == 4 || panelCabin_Int == 5 || panelCabin_Int == 6 || panelCabin_Int == 7) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Pan_Cabin_Red_Plus_diffuse_R;
			}
			if (materialBumper_Int == 1) {
				point_BumperCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = dop_Red_diffuse_R;
			}
				if (cowl_Bool == false || trunkBS_Bool == false) {
				point_TrunkBig_Small.GetChild (0).transform.GetChild(0).gameObject.GetComponent<Renderer> ().material = dop_Red_diffuse_R;
			}
			if (point_DSG1_Small.childCount > 0) {
				point_DSG1_Small.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Red_Plus_diffuse_R;
			}
			if (point_DSG1_Big.childCount > 0) {
				point_DSG1_Big.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Red_Plus_diffuse_R;
			}
		}
		if (colorCabin_Int == 9) {
			point_Cabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Yellow_Plus_diffuse_R;
			if (panelCabin_Int == 1 || panelCabin_Int == 2 || panelCabin_Int == 3) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Yellow_Plus_diffuse_R;
			} else if (panelCabin_Int == 4 || panelCabin_Int == 5 || panelCabin_Int == 6 || panelCabin_Int == 7) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Pan_Cabin_Yellow_Plus_diffuse_R;
			}
			if (materialBumper_Int == 1) {
				point_BumperCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = dop_Yellow_diffuse_R;
			}
				if (cowl_Bool == false || trunkBS_Bool == false) {
				point_TrunkBig_Small.GetChild (0).transform.GetChild(0).gameObject.GetComponent<Renderer> ().material = dop_Yellow_diffuse_R;
			}
			if (point_DSG1_Small.childCount > 0) {
				point_DSG1_Small.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Yellow_Plus_diffuse_R;
			}
			if (point_DSG1_Big.childCount > 0) {
				point_DSG1_Big.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Yellow_Plus_diffuse_R;
			}
		}
		if (colorCabin_Int == 10) {
			point_Cabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Brown_Plus_diffuse_R;
			if (panelCabin_Int == 1 || panelCabin_Int == 2 || panelCabin_Int == 3) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Brown_Plus_diffuse_R;
			} else if (panelCabin_Int == 4 || panelCabin_Int == 5 || panelCabin_Int == 6 || panelCabin_Int == 7) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Pan_Cabin_Brown_Plus_diffuse_R;
			}
			if (materialBumper_Int == 1) {
				point_BumperCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = dop_Brown_diffuse_R;
			}
				if (cowl_Bool == false || trunkBS_Bool == false) {
				point_TrunkBig_Small.GetChild (0).transform.GetChild(0).gameObject.GetComponent<Renderer> ().material = dop_Brown_diffuse_R;
			}
			if (point_DSG1_Small.childCount > 0) {
				point_DSG1_Small.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Brown_Plus_diffuse_R;
			}
			if (point_DSG1_Big.childCount > 0) {
				point_DSG1_Big.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Brown_Plus_diffuse_R;
			}
		}
		if (colorCabin_Int == 11) {
			point_Cabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Blue_Plus_diffuse_R;
			if (panelCabin_Int == 1 || panelCabin_Int == 2 || panelCabin_Int == 3) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Blue_Plus_diffuse_R;
			}else if (panelCabin_Int == 4 || panelCabin_Int == 5 || panelCabin_Int == 6 || panelCabin_Int == 7) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Pan_Cabin_Blue_Plus_diffuse_R;
			}
			if (materialBumper_Int == 1) {
				point_BumperCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = dop_Blue_diffuse_R;
			}
				if (cowl_Bool == false || trunkBS_Bool == false) {
				point_TrunkBig_Small.GetChild (0).transform.GetChild(0).gameObject.GetComponent<Renderer> ().material = dop_Blue_diffuse_R;
			}
			if (point_DSG1_Small.childCount > 0) {
				point_DSG1_Small.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Blue_Plus_diffuse_R;
			}
			if (point_DSG1_Big.childCount > 0) {
				point_DSG1_Big.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Blue_Plus_diffuse_R;
			}
		}
		if (colorCabin_Int == 12) {
			point_Cabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Green_Plus_diffuse_R;
			if (panelCabin_Int == 1 || panelCabin_Int == 2 || panelCabin_Int == 3) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Green_Plus_diffuse_R;
			}else if (panelCabin_Int == 4 || panelCabin_Int == 5 || panelCabin_Int == 6 || panelCabin_Int == 7) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Pan_Cabin_Green_Plus_diffuse_R;
			}
			if (materialBumper_Int == 1) {
				point_BumperCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = dop_Green_diffuse_R;
			}
				if (cowl_Bool == false || trunkBS_Bool == false) {
				point_TrunkBig_Small.GetChild (0).transform.GetChild(0).gameObject.GetComponent<Renderer> ().material = dop_Green_diffuse_R;
			}
			if (point_DSG1_Small.childCount > 0) {
				point_DSG1_Small.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Green_Plus_diffuse_R;
			}
			if (point_DSG1_Big.childCount > 0) {
				point_DSG1_Big.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Green_Plus_diffuse_R;
			}
		}
	}
}
	public void CheckMaterialPanel(){
		if (panelCabin_Int == 1 || panelCabin_Int == 2 || panelCabin_Int == 3) {
			if (colorCabin_Int == 1) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_White_diffuse_R;
			} else if (colorCabin_Int == 2) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Red_diffuse_R;
			} else if (colorCabin_Int == 3) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Yellow_diffuse_R;
			} else if (colorCabin_Int == 4) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Brown_diffuse_R;
			} else if (colorCabin_Int == 5) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Blue_diffuse_R;
			} else if (colorCabin_Int == 6) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Green_diffuse_R;
			} else if (colorCabin_Int == 7) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_White_Plus_diffuse_R;
			} else if (colorCabin_Int == 8) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Red_Plus_diffuse_R;
			} else if (colorCabin_Int == 9) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Yellow_Plus_diffuse_R;
			} else if (colorCabin_Int == 10) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Brown_Plus_diffuse_R;
			} else if (colorCabin_Int == 11) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Blue_Plus_diffuse_R;
			} else if (colorCabin_Int == 12) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Cabin_Green_Plus_diffuse_R;
			}
		}else if (panelCabin_Int == 4 || panelCabin_Int == 5 || panelCabin_Int == 6 || panelCabin_Int == 7) {
			if (colorCabin_Int == 1) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Pan_Cabin_White_diffuse_R;
			} else if (colorCabin_Int == 2) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Pan_Cabin_Red_diffuse_R;
			} else if (colorCabin_Int == 3) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Pan_Cabin_Yellow_diffuse_R;
			} else if (colorCabin_Int == 4) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Pan_Cabin_Brown_diffuse_R;
			} else if (colorCabin_Int == 5) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Pan_Cabin_Blue_diffuse_R;
			} else if (colorCabin_Int == 6) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Pan_Cabin_Green_diffuse_R;
			} else if (colorCabin_Int == 7) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Pan_Cabin_White_Plus_diffuse_R;
			} else if (colorCabin_Int == 8) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Pan_Cabin_Red_Plus_diffuse_R;
			} else if (colorCabin_Int == 9) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Pan_Cabin_Yellow_Plus_diffuse_R;
			} else if (colorCabin_Int == 10) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Pan_Cabin_Brown_Plus_diffuse_R;
			} else if (colorCabin_Int == 11) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Pan_Cabin_Blue_Plus_diffuse_R;
			} else if (colorCabin_Int == 12) {
				point_PanelCabin.GetChild (0).gameObject.GetComponent<Renderer> ().material = Pan_Cabin_Green_Plus_diffuse_R;
			}
		}
	}
	//EQUIPMENT__________Manipulator1_________________________________________________________________________________________________
	public void Manipulator1(){
		if (manipulator1_Bool == true) {
			//If Manipulator On to Dostroy
			if (point_Manipulator_FrameSmall.childCount > 0) {
				foreach (Transform obj_M1 in point_Manipulator_FrameSmall) {
					DestroyImmediate (point_Manipulator_FrameSmall.GetChild (0).gameObject);
				}
			} else if (point_Manipulator_FrameBig_CabSmall.childCount > 0) {
				foreach (Transform obj_M2 in point_Manipulator_FrameBig_CabSmall) {
					DestroyImmediate (point_Manipulator_FrameBig_CabSmall.GetChild (0).gameObject);
				}
			} else if (point_Manipulator_FrameBig_CabBig.childCount > 0) {
				foreach (Transform obj_M3 in point_Manipulator_FrameBig_CabBig) {
					DestroyImmediate (point_Manipulator_FrameBig_CabBig.GetChild (0).gameObject);
				}
			}
			if (manipulator2_Bool == false) {
				manipulator2_Bool = true;
			} else if (manipulator3_Bool == false) {
				manipulator3_Bool = true;
			}
			if (frame_Int == 1 && cabin_Int == 0) {
				var obj_Manip1 = Instantiate (manipulator1, point_Manipulator_FrameBig_CabSmall.position, Quaternion.identity);
				obj_Manip1.transform.parent = point_Manipulator_FrameBig_CabSmall;
			}
			if (frame_Int == 2 && cabin_Int == 0) {
				var obj_Manip2 = Instantiate (manipulator1, point_Manipulator_FrameSmall.position, Quaternion.identity);
				obj_Manip2.transform.parent = point_Manipulator_FrameSmall;
			}
			if (frame_Int == 2 && cabin_Int == 1) {
				var obj_Manip5 = Instantiate (manipulator1, point_Manipulator_FrameSmall.position, Quaternion.identity);
				obj_Manip5.transform.parent = point_Manipulator_FrameSmall;
			}
			if (frame_Int == 1 && cabin_Int == 1) {
				var obj_Manip3 = Instantiate (manipulator1, point_Manipulator_FrameBig_CabSmall.position, Quaternion.identity);
				obj_Manip3.transform.parent = point_Manipulator_FrameBig_CabSmall;
			}
			if (frame_Int == 1 && cabin_Int == 2) {
				var obj_Manip4 = Instantiate (manipulator1, point_Manipulator_FrameBig_CabBig.position, Quaternion.identity);
				obj_Manip4.transform.parent = point_Manipulator_FrameBig_CabBig;
			}
			manipulator1_Bool = false;
			check_OnBody = false;
			//Body
			if (body_Bool == false) {
				GreateBody ();
			}
			if (colorRed_Manip != 0) {
				if (colorRed_Manip == 1) {
					ColorBlue_Manip ();
				} else if (colorRed_Manip == 2) {
					ColorRed_Manip ();
				} else if (colorRed_Manip == 3) {
					ColorWhite_Manip ();
				} else if (colorRed_Manip == 4) {
					ColorYellow_Manip ();
				}
			}
			//Protection
			if (protection_Bool == false) {
				GreateProtection ();
			}
			ManipulatorController ();
			//Check Manip
			CheckManipulator ();
			//Block Manip
			if (controllerTruck_Bool == false) {
				gameObject.GetComponent<ControllerTruck> ().blockManip_Bool = true;
			}
			if (view_Crable == false) {
				view_Crable = true;
			}
			if (Cradle_Bool == false) {
				Cradle_Bool = true;
			}
			AddConponentArrow ();
		} else if (manipulator1_Bool == false) {
			// Manipulator  Dostroy
			if (point_Manipulator_FrameSmall.childCount > 0) {
				foreach (Transform obj_M1 in point_Manipulator_FrameSmall) {
					DestroyImmediate (point_Manipulator_FrameSmall.GetChild (0).gameObject);
				}
			} else if (point_Manipulator_FrameBig_CabSmall.childCount > 0) {
				foreach (Transform obj_M2 in point_Manipulator_FrameBig_CabSmall) {
					DestroyImmediate (point_Manipulator_FrameBig_CabSmall.GetChild (0).gameObject);
				}
			} else if (point_Manipulator_FrameBig_CabBig.childCount > 0) {
				foreach (Transform obj_M3 in point_Manipulator_FrameBig_CabBig) {
					DestroyImmediate (point_Manipulator_FrameBig_CabBig.GetChild (0).gameObject);
				}
			}
			manipulator1_Bool = true;
			check_OnBody = true;
			//Body
			if (body_Bool == false) {
				GreateBody ();
			}
			colorRed_Manip = 0;
			//Block Manip
			if (controllerTruck_Bool == false) {
				gameObject.GetComponent<ControllerTruck> ().blockManip_Bool = false;
			}
		}
	}
	//EQUIPMENT__________Manipulator2_________________________________________________________________________________________________
	public void Manipulator2(){
		if (manipulator2_Bool == true) {
		//If Manipulator On to Dostroy
		if (point_Manipulator_FrameSmall.childCount > 0) {
			foreach (Transform obj_M1 in point_Manipulator_FrameSmall) {
				DestroyImmediate (point_Manipulator_FrameSmall.GetChild (0).gameObject);
			}
		} else if (point_Manipulator_FrameBig_CabSmall.childCount > 0) {
			foreach (Transform obj_M2 in point_Manipulator_FrameBig_CabSmall) {
				DestroyImmediate (point_Manipulator_FrameBig_CabSmall.GetChild (0).gameObject);
			}
		} else if (point_Manipulator_FrameBig_CabBig.childCount > 0) {
			foreach (Transform obj_M3 in point_Manipulator_FrameBig_CabBig) {
				DestroyImmediate (point_Manipulator_FrameBig_CabBig.GetChild (0).gameObject);
			}
		}
			if (manipulator1_Bool == false) {
				manipulator1_Bool = true;
			} else if (manipulator3_Bool == false) {
				manipulator3_Bool = true;
			}
			if (frame_Int == 1 && cabin_Int == 0) {
				var obj_Manip1 = Instantiate (manipulator2, point_Manipulator_FrameBig_CabSmall.position, Quaternion.identity);
				obj_Manip1.transform.parent = point_Manipulator_FrameBig_CabSmall;
			}
			if (frame_Int == 2 && cabin_Int == 0) {
				var obj_Manip2 = Instantiate (manipulator2, point_Manipulator_FrameSmall.position, Quaternion.identity);
				obj_Manip2.transform.parent = point_Manipulator_FrameSmall;
			}
			if(frame_Int == 2 && cabin_Int == 1){
				var obj_Manip5 = Instantiate (manipulator2, point_Manipulator_FrameSmall.position, Quaternion.identity);
				obj_Manip5.transform.parent = point_Manipulator_FrameSmall;
			}
			if (frame_Int == 1 && cabin_Int == 1) {
				var obj_Manip3 = Instantiate (manipulator2, point_Manipulator_FrameBig_CabSmall.position, Quaternion.identity);
				obj_Manip3.transform.parent = point_Manipulator_FrameBig_CabSmall;
			}
			if (frame_Int == 1 && cabin_Int == 2) {
				var obj_Manip4 = Instantiate (manipulator2, point_Manipulator_FrameBig_CabBig.position, Quaternion.identity);
				obj_Manip4.transform.parent = point_Manipulator_FrameBig_CabBig;
			}
			manipulator2_Bool = false;
			check_OnBody = false;
			//Body
			if (body_Bool == false) {
				GreateBody ();
			}
			if (colorRed_Manip != 0) {
				if (colorRed_Manip == 1) {
					ColorBlue_Manip ();
				} else if (colorRed_Manip == 2) {
					ColorRed_Manip ();
				} else if (colorRed_Manip == 3) {
					ColorWhite_Manip ();
				} else if (colorRed_Manip == 4) {
					ColorYellow_Manip ();
				}
			}
			//Protection
			if (protection_Bool == false) {
				GreateProtection ();
			}
			ManipulatorController ();
			//Check Manip
			CheckManipulator();
			//Block Manip
			if (controllerTruck_Bool == false) {
				gameObject.GetComponent<ControllerTruck> ().blockManip_Bool = true;
			}
			if (view_Crable == false) {
				view_Crable = true;
			}
			if (Cradle_Bool == false) {
				Cradle_Bool = true;
			}
			AddConponentArrow ();
		} else if (manipulator2_Bool == false) {
			// Manipulator  Dostroy
			if (point_Manipulator_FrameSmall.childCount > 0) {
				foreach (Transform obj_M1 in point_Manipulator_FrameSmall) {
					DestroyImmediate (point_Manipulator_FrameSmall.GetChild (0).gameObject);
				}
			} else if (point_Manipulator_FrameBig_CabSmall.childCount > 0) {
				foreach (Transform obj_M2 in point_Manipulator_FrameBig_CabSmall) {
					DestroyImmediate (point_Manipulator_FrameBig_CabSmall.GetChild (0).gameObject);
				}
			} else if (point_Manipulator_FrameBig_CabBig.childCount > 0) {
				foreach (Transform obj_M3 in point_Manipulator_FrameBig_CabBig) {
					DestroyImmediate (point_Manipulator_FrameBig_CabBig.GetChild (0).gameObject);
				}
			}
			manipulator2_Bool = true;
			check_OnBody = true;
			//Body
			if (body_Bool == false) {
				GreateBody ();
			}
			colorRed_Manip = 0;
			//Block Manip
			if (controllerTruck_Bool == false) {
				gameObject.GetComponent<ControllerTruck> ().blockManip_Bool = false;
			}
		}
	}
	//EQUIPMENT__________Manipulator3_________________________________________________________________________________________________
	public void Manipulator3(){
		if (manipulator3_Bool == true) {
			//If Manipulator On to Dostroy
			if (point_Manipulator_FrameSmall.childCount > 0) {
				foreach (Transform obj_M1 in point_Manipulator_FrameSmall) {
					DestroyImmediate (point_Manipulator_FrameSmall.GetChild (0).gameObject);
				}
			} else if (point_Manipulator_FrameBig_CabSmall.childCount > 0) {
				foreach (Transform obj_M2 in point_Manipulator_FrameBig_CabSmall) {
					DestroyImmediate (point_Manipulator_FrameBig_CabSmall.GetChild (0).gameObject);
				}
			} else if (point_Manipulator_FrameBig_CabBig.childCount > 0) {
				foreach (Transform obj_M3 in point_Manipulator_FrameBig_CabBig) {
					DestroyImmediate (point_Manipulator_FrameBig_CabBig.GetChild (0).gameObject);
				}
			}
			if (manipulator1_Bool == false) {
				manipulator1_Bool = true;
			} else if (manipulator2_Bool == false) {
				manipulator2_Bool = true;
			}
			if (frame_Int == 1 && cabin_Int == 0) {
				var obj_Manip1 = Instantiate (manipulator3, point_Manipulator_FrameBig_CabSmall.position, Quaternion.identity);
				obj_Manip1.transform.parent = point_Manipulator_FrameBig_CabSmall;
			}
			if (frame_Int == 2 && cabin_Int == 0) {
				var obj_Manip2 = Instantiate (manipulator3, point_Manipulator_FrameSmall.position, Quaternion.identity);
				obj_Manip2.transform.parent = point_Manipulator_FrameSmall;
			}
			if(frame_Int == 2 && cabin_Int == 1){
				var obj_Manip5 = Instantiate (manipulator3, point_Manipulator_FrameSmall.position, Quaternion.identity);
				obj_Manip5.transform.parent = point_Manipulator_FrameSmall;
			}
			if (frame_Int == 1 && cabin_Int == 1) {
				var obj_Manip3 = Instantiate (manipulator3, point_Manipulator_FrameBig_CabSmall.position, Quaternion.identity);
				obj_Manip3.transform.parent = point_Manipulator_FrameBig_CabSmall;
			}
			if (frame_Int == 1 && cabin_Int == 2) {
				var obj_Manip4 = Instantiate (manipulator3, point_Manipulator_FrameBig_CabBig.position, Quaternion.identity);
				obj_Manip4.transform.parent = point_Manipulator_FrameBig_CabBig;
			}
			manipulator3_Bool = false;
			check_OnBody = false;
			//Body
			if (body_Bool == false) {
				GreateBody ();
			}
			if (colorRed_Manip != 0) {
				if (colorRed_Manip == 1) {
					ColorBlue_Manip ();
				} else if (colorRed_Manip == 2) {
					ColorRed_Manip ();
				} else if (colorRed_Manip == 3) {
					ColorWhite_Manip ();
				} else if (colorRed_Manip == 4) {
					ColorYellow_Manip ();
				}
			}
			//Protection
			if (protection_Bool == false) {
				GreateProtection ();
			}
			ManipulatorController ();
			//Check Manip
			CheckManipulator();
			//Block Manip
			if (controllerTruck_Bool == false) {
				gameObject.GetComponent<ControllerTruck> ().blockManip_Bool = true;
			}
			view_Crable = false;
			AddConponentArrow ();
		} else if (manipulator3_Bool == false) {
			// Manipulator  Dostroy
			if (point_Manipulator_FrameSmall.childCount > 0) {
				foreach (Transform obj_M1 in point_Manipulator_FrameSmall) {
					DestroyImmediate (point_Manipulator_FrameSmall.GetChild (0).gameObject);
				}
			} else if (point_Manipulator_FrameBig_CabSmall.childCount > 0) {
				foreach (Transform obj_M2 in point_Manipulator_FrameBig_CabSmall) {
					DestroyImmediate (point_Manipulator_FrameBig_CabSmall.GetChild (0).gameObject);
				}
			} else if (point_Manipulator_FrameBig_CabBig.childCount > 0) {
				foreach (Transform obj_M3 in point_Manipulator_FrameBig_CabBig) {
					DestroyImmediate (point_Manipulator_FrameBig_CabBig.GetChild (0).gameObject);
				}
			}
			manipulator3_Bool = true;
			check_OnBody = true;
			//Body
			if (body_Bool == false) {
				GreateBody ();
			}
			colorRed_Manip = 0;
			//Block Manip
			if (controllerTruck_Bool == false) {
				gameObject.GetComponent<ControllerTruck> ().blockManip_Bool = false;
			}
			view_Crable = true;
			if (Cradle_Bool == false) {
				Cradle_Bool = true;
			}
		}
	}
	public void ColorBlue_Manip(){
		if (manipulator3_Bool == false) {
			matManip3 = Manipulator1_Blue_diffuse_R;
		}
		matManip3 = Manipulator1_Blue_diffuse_R;
		if (point_Manipulator_FrameBig_CabBig.childCount > 0) {
			foreach (Renderer ren in point_Manipulator_FrameBig_CabBig.GetComponentsInChildren<Renderer>()) {
				if (manipulator1_Bool == false || manipulator2_Bool == false) {
					ren.material = Manipulator_Blue_diffuse_R;
				} else if (manipulator3_Bool == false) {
					Manipulator3_Material ();
				}
			}
		} else if (point_Manipulator_FrameBig_CabSmall.childCount > 0) {
			foreach (Renderer ren in point_Manipulator_FrameBig_CabSmall.GetComponentsInChildren<Renderer>()) {
				if (manipulator1_Bool == false || manipulator2_Bool == false) {
					ren.material = Manipulator_Blue_diffuse_R;
				} else if (manipulator3_Bool == false) {
					Manipulator3_Material ();
				}
			}
		} else if (point_Manipulator_FrameSmall.childCount > 0) {
			foreach (Renderer ren in point_Manipulator_FrameSmall.GetComponentsInChildren<Renderer>()) {
				if (manipulator1_Bool == false || manipulator2_Bool == false) {
					ren.material = Manipulator_Blue_diffuse_R;
				} else if (manipulator3_Bool == false) {
					Manipulator3_Material ();
				}
			}
		}
		colorRed_Manip = 1;
	}
	public void ColorRed_Manip(){
		if (manipulator3_Bool == false) {
			matManip3 = Manipulator1_Red_diffuse_R;
		}
		if (point_Manipulator_FrameBig_CabBig.childCount > 0) {
			foreach (Renderer ren in point_Manipulator_FrameBig_CabBig.GetComponentsInChildren<Renderer>()) {
				if (manipulator1_Bool == false || manipulator2_Bool == false) {
					ren.material = Manipulator_Red_diffuse_R;
				} else if (manipulator3_Bool == false) {
					Manipulator3_Material ();
				}
			}
		} else if (point_Manipulator_FrameBig_CabSmall.childCount > 0) {
			foreach (Renderer ren in point_Manipulator_FrameBig_CabSmall.GetComponentsInChildren<Renderer>()) {
				if (manipulator1_Bool == false || manipulator2_Bool == false) {
					ren.material = Manipulator_Red_diffuse_R;
				} else if (manipulator3_Bool == false) {
					Manipulator3_Material ();
				}
			}
		} else if (point_Manipulator_FrameSmall.childCount > 0) {
			foreach (Renderer ren in point_Manipulator_FrameSmall.GetComponentsInChildren<Renderer>()) {
				if (manipulator1_Bool == false || manipulator2_Bool == false) {
					ren.material = Manipulator_Red_diffuse_R;
				} else if (manipulator3_Bool == false) {
					Manipulator3_Material ();
				}
			}
		}
		colorRed_Manip = 2;
	}
	public void ColorWhite_Manip(){
		if (manipulator3_Bool == false) {
			matManip3 = Manipulator1_White_diffuse_R;
		}
		if (point_Manipulator_FrameBig_CabBig.childCount > 0) {
			foreach (Renderer ren in point_Manipulator_FrameBig_CabBig.GetComponentsInChildren<Renderer>()) {
				if (manipulator1_Bool == false || manipulator2_Bool == false) {
					ren.material = Manipulator_White_diffuse_R;
				} else if (manipulator3_Bool == false) {
					Manipulator3_Material ();
				}
			}
		} else if (point_Manipulator_FrameBig_CabSmall.childCount > 0) {
			foreach (Renderer ren in point_Manipulator_FrameBig_CabSmall.GetComponentsInChildren<Renderer>()) {
				if (manipulator1_Bool == false || manipulator2_Bool == false) {
					ren.material = Manipulator_White_diffuse_R;
				} else if (manipulator3_Bool == false) {
					Manipulator3_Material ();
				}
			}
		} else if (point_Manipulator_FrameSmall.childCount > 0) {
			foreach (Renderer ren in point_Manipulator_FrameSmall.GetComponentsInChildren<Renderer>()) {
				if (manipulator1_Bool == false || manipulator2_Bool == false) {
					ren.material = Manipulator_White_diffuse_R;
				} else if (manipulator3_Bool == false) {
					Manipulator3_Material ();
				}
			}
		}
		colorRed_Manip = 3;
	}
	public void ColorYellow_Manip(){
		if (manipulator3_Bool == false) {
			matManip3 = Manipulator1_Yellow_diffuse_R;
		}
		if (point_Manipulator_FrameBig_CabBig.childCount > 0) {
			foreach (Renderer ren in point_Manipulator_FrameBig_CabBig.GetComponentsInChildren<Renderer>()) {
				if (manipulator1_Bool == false || manipulator2_Bool == false) {
					ren.material = Manipulator_Yellow_diffuse_R;
				} else if (manipulator3_Bool == false) {
					Manipulator3_Material ();
				}
			}
		} else if (point_Manipulator_FrameBig_CabSmall.childCount > 0) {
			foreach (Renderer ren in point_Manipulator_FrameBig_CabSmall.GetComponentsInChildren<Renderer>()) {
				if (manipulator1_Bool == false || manipulator2_Bool == false) {
					ren.material = Manipulator_Yellow_diffuse_R;
				} else if (manipulator3_Bool == false) {
					Manipulator3_Material ();
				}
			}
		} else if (point_Manipulator_FrameSmall.childCount > 0) {
			foreach (Renderer ren in point_Manipulator_FrameSmall.GetComponentsInChildren<Renderer>()) {
				if (manipulator1_Bool == false || manipulator2_Bool == false) {
					ren.material = Manipulator_Yellow_diffuse_R;
				} else if (manipulator3_Bool == false) {
					Manipulator3_Material ();
				}
			}
		}
		colorRed_Manip = 4;
	}
	public void Manipulator3_Material(){
		if (point_Manipulator_FrameSmall.childCount > 0) {
			point_Manipulator_FrameSmall.GetChild (0).gameObject.GetComponent<Renderer> ().material = matManip3;
			point_Manipulator_FrameSmall.GetChild (0).transform.GetChild (0).gameObject.GetComponent<Renderer> ().material = matManip3;
			point_Manipulator_FrameSmall.GetChild (0).transform.GetChild (1).gameObject.GetComponent<Renderer> ().material = matManip3;
			point_Manipulator_FrameSmall.GetChild (0).transform.GetChild (2).gameObject.GetComponent<Renderer> ().material = matManip3;
			point_Manipulator_FrameSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).gameObject.GetComponent<Renderer> ().material = matManip3;
			point_Manipulator_FrameSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).gameObject.GetComponent<Renderer> ().material = matManip3;
			point_Manipulator_FrameSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).gameObject.GetComponent<Renderer> ().material = matManip3;
			point_Manipulator_FrameSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).gameObject.GetComponent<Renderer> ().material = matManip3;
			point_Manipulator_FrameSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).gameObject.GetComponent<Renderer> ().material = matManip3;
			foreach (Renderer ren in point_Manipulator_FrameSmall.GetChild(0).transform.GetChild(1).GetComponentsInChildren<Renderer>()) {
				ren.material = matManip3;
			}
			foreach (Renderer ren in point_Manipulator_FrameSmall.GetChild(0).transform.GetChild(2).GetComponentsInChildren<Renderer>()) {
				ren.material = matManip3;
			}
			foreach (Renderer ren in point_Manipulator_FrameSmall.GetChild(0).transform.GetChild(3).GetComponentsInChildren<Renderer>()) {
				ren.material = matManip3;
			}
			foreach (Renderer ren in point_Manipulator_FrameSmall.GetChild(0).transform.GetChild(4).GetComponentsInChildren<Renderer>()) {
				ren.material = matManip3;
			}
		} else if (point_Manipulator_FrameBig_CabSmall.childCount > 0) {
			point_Manipulator_FrameBig_CabSmall.GetChild (0).gameObject.GetComponent<Renderer> ().material = matManip3;
			point_Manipulator_FrameBig_CabSmall.GetChild (0).transform.GetChild (0).gameObject.GetComponent<Renderer> ().material = matManip3;
			point_Manipulator_FrameBig_CabSmall.GetChild (0).transform.GetChild (1).gameObject.GetComponent<Renderer> ().material = matManip3;
			point_Manipulator_FrameBig_CabSmall.GetChild (0).transform.GetChild (2).gameObject.GetComponent<Renderer> ().material = matManip3;
			point_Manipulator_FrameBig_CabSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).gameObject.GetComponent<Renderer> ().material = matManip3;
			point_Manipulator_FrameBig_CabSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).gameObject.GetComponent<Renderer> ().material = matManip3;
			point_Manipulator_FrameBig_CabSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).gameObject.GetComponent<Renderer> ().material = matManip3;
			point_Manipulator_FrameBig_CabSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).gameObject.GetComponent<Renderer> ().material = matManip3;
			point_Manipulator_FrameBig_CabSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).gameObject.GetComponent<Renderer> ().material = matManip3;
			foreach (Renderer ren in point_Manipulator_FrameBig_CabSmall.GetChild(0).transform.GetChild(1).GetComponentsInChildren<Renderer>()) {
				ren.material = matManip3;
			}
			foreach (Renderer ren in point_Manipulator_FrameBig_CabSmall.GetChild(0).transform.GetChild(2).GetComponentsInChildren<Renderer>()) {
				ren.material = matManip3;
			}
			foreach (Renderer ren in point_Manipulator_FrameBig_CabSmall.GetChild(0).transform.GetChild(3).GetComponentsInChildren<Renderer>()) {
				ren.material = matManip3;
			}
			foreach (Renderer ren in point_Manipulator_FrameBig_CabSmall.GetChild(0).transform.GetChild(4).GetComponentsInChildren<Renderer>()) {
				ren.material = matManip3;
			}
		} else if (point_Manipulator_FrameBig_CabBig.childCount > 0) {
			point_Manipulator_FrameBig_CabBig.GetChild (0).gameObject.GetComponent<Renderer> ().material = matManip3;
			point_Manipulator_FrameBig_CabBig.GetChild (0).transform.GetChild (0).gameObject.GetComponent<Renderer> ().material = matManip3;
			point_Manipulator_FrameBig_CabBig.GetChild (0).transform.GetChild (1).gameObject.GetComponent<Renderer> ().material = matManip3;
			point_Manipulator_FrameBig_CabBig.GetChild (0).transform.GetChild (2).gameObject.GetComponent<Renderer> ().material = matManip3;
			point_Manipulator_FrameBig_CabBig.GetChild (0).transform.GetChild (0).transform.GetChild (0).gameObject.GetComponent<Renderer> ().material = matManip3;
			point_Manipulator_FrameBig_CabBig.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).gameObject.GetComponent<Renderer> ().material = matManip3;
			point_Manipulator_FrameBig_CabBig.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).gameObject.GetComponent<Renderer> ().material = matManip3;
			point_Manipulator_FrameBig_CabBig.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).gameObject.GetComponent<Renderer> ().material = matManip3;
			point_Manipulator_FrameBig_CabBig.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).gameObject.GetComponent<Renderer> ().material = matManip3;
			foreach (Renderer ren in point_Manipulator_FrameBig_CabBig.GetChild(0).transform.GetChild(1).GetComponentsInChildren<Renderer>()) {
				ren.material = matManip3;
			}
			foreach (Renderer ren in point_Manipulator_FrameBig_CabBig.GetChild(0).transform.GetChild(2).GetComponentsInChildren<Renderer>()) {
				ren.material = matManip3;
			}
			foreach (Renderer ren in point_Manipulator_FrameBig_CabBig.GetChild(0).transform.GetChild(3).GetComponentsInChildren<Renderer>()) {
				ren.material = matManip3;
			}
			foreach (Renderer ren in point_Manipulator_FrameBig_CabBig.GetChild(0).transform.GetChild(4).GetComponentsInChildren<Renderer>()) {
				ren.material = matManip3;
			}
		}
	}
	//EQUIPMENT__________Body_________________________________________________________________________________________________
	public void Body(){
		if (body_Bool == true) {
			GreateBody ();
			body_Bool = false;
		} else if (body_Bool == false) {
			if (point_Body_FrameBaig.childCount > 0) {
				foreach (Transform obj_B1 in point_Body_FrameBaig) {
					DestroyImmediate (point_Body_FrameBaig.GetChild (0).gameObject);
				}
			}
			if (point_Body_FrameSmall.childCount > 0) {
				foreach (Transform obj_B2 in point_Body_FrameSmall) {
					DestroyImmediate (point_Body_FrameSmall.GetChild (0).gameObject);
				}
			}
			body_Int = 0;
			body_Bool = true;
			if (BodyDop3_Bool == false) {
				BodyDop3_Bool = true;
			}
		}
	}
	public void GreateBody(){
		if (point_Body_FrameBaig.childCount > 0) {
			foreach (Transform obj_B1 in point_Body_FrameBaig) {
				DestroyImmediate (point_Body_FrameBaig.GetChild (0).gameObject);
			}
		}
		if (point_Body_FrameSmall.childCount > 0) {
			foreach (Transform obj_B2 in point_Body_FrameSmall) {
				DestroyImmediate (point_Body_FrameSmall.GetChild (0).gameObject);
			}
		}
		if (frame_Int == 1 && cabin_Int == 2 && framePlus_Bool == true && check_OnBody == true) {  //FrameBig end CabBig
			var obj_Body1 = Instantiate (Body_FrameBig_CabBig, point_Body_FrameBaig.position, Quaternion.identity);
			obj_Body1.transform.parent = point_Body_FrameBaig;
			body_Int = 1;
		}
		if (frame_Int == 1 && cabin_Int == 1 && framePlus_Bool == true && check_OnBody == true) {  //FrameBig end CabSmall
			var obj_Body1 = Instantiate (Body_FrameBig_CabSmall, point_Body_FrameBaig.position, Quaternion.identity);
			obj_Body1.transform.parent = point_Body_FrameBaig;
			body_Int = 2;
		}
		if (frame_Int == 2 && cabin_Int == 1 && check_OnBody == true) {  //FrameSmall end CabSmall
			var obj_Body1 = Instantiate (Body_FrameSmall_CabSmall, point_Body_FrameSmall.position, Quaternion.identity);
			obj_Body1.transform.parent = point_Body_FrameSmall;
			body_Int = 3;
		}
		if (frame_Int == 2 && cabin_Int == 1 && check_OnBody == false) {  //FrameSmall end CabSmall end Manipulator
			var obj_Body1 = Instantiate (Body_FrameSmall_CabSmall_Man, point_Body_FrameSmall.position, Quaternion.identity);
			obj_Body1.transform.parent = point_Body_FrameSmall;
			body_Int = 4;
		}
		if (frame_Int == 2 && cabin_Int == 2) {  //FrameSmall end CabBig
			var obj_Body1 = Instantiate (Body_FrameSmall_CabBig, point_Body_FrameSmall.position, Quaternion.identity);
			obj_Body1.transform.parent = point_Body_FrameSmall;
			body_Int = 5;
		}
		if (frame_Int == 1 && cabin_Int == 2 && framePlus_Bool == false && check_OnBody == true) {  //FrameBig end CabBig end FramePlus
			var obj_Body1 = Instantiate (Body_FrameBig_CabBig_Plus, point_Body_FrameBaig.position, Quaternion.identity);
			obj_Body1.transform.parent = point_Body_FrameBaig;
			body_Int = 6;
		}
		if (frame_Int == 1 && cabin_Int == 1 && framePlus_Bool == false && check_OnBody == true) {  //FrameBig end CabSmall end FramePlus
			var obj_Body1 = Instantiate (Body_FrameBig_CabSmall_Plus, point_Body_FrameBaig.position, Quaternion.identity);
			obj_Body1.transform.parent = point_Body_FrameBaig;
			body_Int = 7;
		}
		if (frame_Int == 1 && cabin_Int == 1 && framePlus_Bool == true && check_OnBody == false) {  //FrameBig end CabSmall end Manipulator
			var obj_Body1 = Instantiate (Body_FrameBig_CabSmall_Man, point_Body_FrameBaig.position, Quaternion.identity);
			obj_Body1.transform.parent = point_Body_FrameBaig;
			body_Int = 8;
		}
		if (frame_Int == 1 && cabin_Int == 2 && framePlus_Bool == true && check_OnBody == false) {  //FrameBig end CabBig end Manipulator
			var obj_Body1 = Instantiate (Body_FrameBigl_CabBig_Man, point_Body_FrameBaig.position, Quaternion.identity);
			obj_Body1.transform.parent = point_Body_FrameBaig;
			body_Int = 9;
		}
		if (frame_Int == 1 && cabin_Int == 2 && framePlus_Bool == false && check_OnBody == false) {  //FrameBig end CabBig end Manipulator end FramePlus
			var obj_Body1 = Instantiate (Body_FrameBig_CabBig_Man_Plus, point_Body_FrameBaig.position, Quaternion.identity);
			obj_Body1.transform.parent = point_Body_FrameBaig;
			body_Int = 10;
		}
		if (frame_Int == 1 && cabin_Int == 1 && framePlus_Bool == false && check_OnBody == false) {  //FrameBig end CabSmall end Manipulator end FramePlus
			var obj_Body1 = Instantiate (Body_FrameBig_CabSmall_Man_Plus, point_Body_FrameBaig.position, Quaternion.identity);
			obj_Body1.transform.parent = point_Body_FrameBaig;
			body_Int = 11;
		}
	//BodyDop3
		if (BodyDop3_Bool == false) {
			if (frame_Int == 1) {
				var bodyDop1 = Instantiate (BodyDop3, point_Body_FrameBaig.GetChild(0).transform.GetChild(1).position, Quaternion.identity);
				bodyDop1.transform.parent = point_Body_FrameBaig.GetChild (0).transform.GetChild (1).transform;
			} else if (frame_Int == 2) {
				var bodyDop1 = Instantiate (BodyDop3, point_Body_FrameSmall.GetChild(0).transform.GetChild(1).position, Quaternion.identity);
				bodyDop1.transform.parent = point_Body_FrameSmall.GetChild (0).transform.GetChild (1).transform;
			}
		}
	}
	//Protection___________________________________________________________________________________________________________
	public void Protection(){
		if (protection_Bool == true) {
			GreateProtection ();
			protection_Bool = false;
		} else if (protection_Bool == false) {
			foreach (Transform obj_Pro in point_Protection) {
				DestroyImmediate (point_Protection.GetChild (0).gameObject);
			}
			protection_Bool = true;
		}
	}
	public void GreateProtection(){
			if (point_Protection.childCount > 0) {
				foreach (Transform obj_Pro in point_Protection) {
					DestroyImmediate (point_Protection.GetChild (0).gameObject);
				}
			}
			if (frame_Int == 1 && cabin_Int == 1 && check_OnBody == false && bridge_Int == 1) {  // FrameBig end CabinSmall 2x2_Manip
			var obj_P0 = Instantiate (FB_CS_2x2_M, point_Protection.position, Quaternion.identity);
			obj_P0.transform.parent = point_Protection;
		}
			if (frame_Int == 1 && cabin_Int == 2 && check_OnBody == false && bridge_Int == 1) {  // FrameBig end CabinBig 2x2_Manip
			var obj_P1 = Instantiate (FB_CB_2x2_M, point_Protection.position, Quaternion.identity);
			obj_P1.transform.parent = point_Protection;
		}
			if (frame_Int == 1 && cabin_Int == 1 && check_OnBody == true && bridge_Int == 1) {  // FrameBig end CabinSmall 2x2
			var obj_P2 = Instantiate (FB_CS_2x2, point_Protection.position, Quaternion.identity);
			obj_P2.transform.parent = point_Protection;
		}
			if (frame_Int == 1 && cabin_Int == 2 && check_OnBody == true && bridge_Int == 1) {  // FrameBig end CabinBig 2x2
			var obj_P3 = Instantiate (FB_CB_2x2, point_Protection.position, Quaternion.identity);
			obj_P3.transform.parent = point_Protection;
		}
			if (frame_Int == 1 && cabin_Int == 1 && check_OnBody == true && bridge_Int == 2) {  // FrameBig end CabinSmall 4x4
			var obj_P4 = Instantiate (FB_CS_4x4, point_Protection.position, Quaternion.identity);
			obj_P4.transform.parent = point_Protection;
		}
			if (frame_Int == 1 && cabin_Int == 2 && check_OnBody == true && bridge_Int == 2) {  // FrameBig end CabinBig 4x4
			var obj_P5 = Instantiate (FB_CB_4x4, point_Protection.position, Quaternion.identity);
			obj_P5.transform.parent = point_Protection;
		}
			if (frame_Int == 1 && cabin_Int == 1 && check_OnBody == false && bridge_Int == 2) {  // FrameBig end CabinSmall 4x4_Manip
			var obj_P6 = Instantiate (FB_CS_4x4_M, point_Protection.position, Quaternion.identity);
			obj_P6.transform.parent = point_Protection;
		}
			if (frame_Int == 1 && cabin_Int == 2 && check_OnBody == false && bridge_Int == 2) {  // FrameBig end CabinBig 4x4_Manip
			var obj_P7 = Instantiate (FB_CB_4x4_M, point_Protection.position, Quaternion.identity);
			obj_P7.transform.parent = point_Protection;
		}
			if (frame_Int == 2 && cabin_Int == 2 && check_OnBody == true && bridge_Int == 1) {  // FrameSmall end CabinBig
			var obj_P8 = Instantiate (FS_CB, point_Protection.position, Quaternion.identity);
			obj_P8.transform.parent = point_Protection;
		}
			if (frame_Int == 2 && cabin_Int == 1 && check_OnBody == true && bridge_Int == 1) {  // FrameSmall end CabinSmal
			var obj_P9 = Instantiate (FS_CS, point_Protection.position, Quaternion.identity);
			obj_P9.transform.parent = point_Protection;
		}
			if (frame_Int == 2 && cabin_Int == 1 && check_OnBody == false && bridge_Int == 1) {  // FrameSmall end CabinSmall_Manip
			var obj_P10 = Instantiate (FS_CS_M, point_Protection.position, Quaternion.identity);
			obj_P10.transform.parent = point_Protection;
		}
	}
	public void Cardan(){
		if (point_Сardan.childCount > 0) {
			foreach (Transform obj_Car in point_Сardan) {
				DestroyImmediate (point_Сardan.GetChild (0).gameObject);
			}
		}
			if (frame_Int == 1 && cabin_Int == 2) {  //FB_CB
				var obj_Card1 = Instantiate (Сardan_FB_CB, point_Сardan.position, Quaternion.identity);
				obj_Card1.transform.parent = point_Сardan;
			}
			if (frame_Int == 1 && cabin_Int == 1) {  //FB_CS
				var obj_Card2 = Instantiate (Сardan_FB_CS, point_Сardan.position, Quaternion.identity);
				obj_Card2.transform.parent = point_Сardan;
			}
			if (frame_Int == 2 && cabin_Int == 2) {  //FS_CB
				var obj_Card3 = Instantiate (Сardan_FS_CB, point_Сardan.position, Quaternion.identity);
				obj_Card3.transform.parent = point_Сardan;
			}
			if (frame_Int == 2 && cabin_Int == 1) {  //FS_CS
				var obj_Card4 = Instantiate (Сardan_FS_CS, point_Сardan.position, Quaternion.identity);
				obj_Card4.transform.parent = point_Сardan;
			}
		}
	//Controller___________________________________________________________________________________________________________
	public void ControllerTruck(){
		if (controllerTruck_Bool == true) {
			gameObject.AddComponent<ControllerTruck> ();
			GreateController ();
			LightPanel ();
			LightBumper ();
			if (manipulator1_Bool == false || manipulator2_Bool == false || manipulator3_Bool == false) {
				ManipulatorController ();
				gameObject.GetComponent<ControllerTruck> ().blockManip_Bool = true;
			}
			if (bridge_Bool_2x2 == false || bridge_Bool_4x4 == false) {
				gameObject.GetComponent<ControllerTruck> ().startTruck = true;
			}
			AddConponentArrow ();
			controllerTruck_Bool = false;
		} else if (controllerTruck_Bool == false) {
			controllerTruck_Bool = true;
		}
	}
	public void GreateController(){
		gameObject.GetComponent<ControllerTruck> ().centerOffMass = point_Frame.GetChild (0).transform.GetChild (13);
	//Wheel Forward
		gameObject.GetComponent<ControllerTruck> ().wheelTransformFL = transform.GetChild (60).transform.GetChild (0);
		gameObject.GetComponent<ControllerTruck> ().wheelTransformFR = transform.GetChild (60).transform.GetChild (1);
		gameObject.GetComponent<ControllerTruck> ().wheelCol_FL = transform.GetChild (60).transform.GetChild (3).GetComponent<WheelCollider>();
		gameObject.GetComponent<ControllerTruck> ().wheelCol_FR = transform.GetChild (60).transform.GetChild (2).GetComponent<WheelCollider>();
		//Wheel Back
		if (Point_bridge_2x2_BigFrame.childCount > 0) {
			gameObject.GetComponent<ControllerTruck> ().wheelTransformBL_For = Point_bridge_2x2_BigFrame.GetChild (0).transform.GetChild (0);
			gameObject.GetComponent<ControllerTruck> ().wheelTransformBR_For = Point_bridge_2x2_BigFrame.GetChild (0).transform.GetChild (1);
			gameObject.GetComponent<ControllerTruck> ().wheelCol_BL_For = Point_bridge_2x2_BigFrame.GetChild (0).transform.GetChild (2).GetComponent<WheelCollider>();
			gameObject.GetComponent<ControllerTruck> ().wheelCol_BR_For = Point_bridge_2x2_BigFrame.GetChild (0).transform.GetChild (3).GetComponent<WheelCollider>();
		} else if (Point_bridge_2x2_SmallFrame.childCount > 0) {
			gameObject.GetComponent<ControllerTruck> ().wheelTransformBL_For = Point_bridge_2x2_SmallFrame.GetChild (0).transform.GetChild (0);
			gameObject.GetComponent<ControllerTruck> ().wheelTransformBR_For = Point_bridge_2x2_SmallFrame.GetChild (0).transform.GetChild (1);
			gameObject.GetComponent<ControllerTruck> ().wheelCol_BL_For = Point_bridge_2x2_SmallFrame.GetChild (0).transform.GetChild (2).GetComponent<WheelCollider>();
			gameObject.GetComponent<ControllerTruck> ().wheelCol_BR_For = Point_bridge_2x2_SmallFrame.GetChild (0).transform.GetChild (3).GetComponent<WheelCollider>();
		} else if (Point_bridge_4x4_BigFrame.childCount > 0) {
			gameObject.GetComponent<ControllerTruck> ().wheelTransformBL_For = Point_bridge_4x4_BigFrame.GetChild (0).transform.GetChild (3);
			gameObject.GetComponent<ControllerTruck> ().wheelTransformBR_For = Point_bridge_4x4_BigFrame.GetChild (0).transform.GetChild (2);
			gameObject.GetComponent<ControllerTruck> ().wheelTransformBL_Back = Point_bridge_4x4_BigFrame.GetChild (0).transform.GetChild (0);
			gameObject.GetComponent<ControllerTruck> ().wheelTransformBR_Back = Point_bridge_4x4_BigFrame.GetChild (0).transform.GetChild (1);
			gameObject.GetComponent<ControllerTruck> ().wheelCol_BL_For = Point_bridge_4x4_BigFrame.GetChild (0).transform.GetChild (4).GetComponent<WheelCollider>();
			gameObject.GetComponent<ControllerTruck> ().wheelCol_BR_For = Point_bridge_4x4_BigFrame.GetChild (0).transform.GetChild (5).GetComponent<WheelCollider>();
			gameObject.GetComponent<ControllerTruck> ().wheelCol_BL_Back = Point_bridge_4x4_BigFrame.GetChild (0).transform.GetChild (7).GetComponent<WheelCollider>();
			gameObject.GetComponent<ControllerTruck> ().wheelCol_BR_Back = Point_bridge_4x4_BigFrame.GetChild (0).transform.GetChild (6).GetComponent<WheelCollider>();
		}
	//Add Sound
		gameObject.GetComponent<ControllerTruck>().s_Engine = transform.GetChild(61).transform.GetChild(0).GetComponent<AudioSource>();
		gameObject.GetComponent<ControllerTruck>().s_Revers = transform.GetChild(61).transform.GetChild(2).GetComponent<AudioSource>();
		gameObject.GetComponent<ControllerTruck>().s_Turn = transform.GetChild(61).transform.GetChild(1).GetComponent<AudioSource>();
		gameObject.GetComponent<ControllerTruck>().s_StartStop = transform.GetChild(61).transform.GetChild(3).GetComponent<AudioSource>();
	}
	public void LightBumper(){
		if (gameObject.GetComponent<ControllerTruck> () != null) {
			if (point_Bumper_FrameBig.childCount > 0) {
				gameObject.GetComponent<ControllerTruck> ().revers_L = point_Bumper_FrameBig.GetChild (0).transform.GetChild (4).GetComponent<Light> ();
				gameObject.GetComponent<ControllerTruck> ().revers_R = point_Bumper_FrameBig.GetChild (0).transform.GetChild (5).GetComponent<Light> ();
				gameObject.GetComponent<ControllerTruck> ().stop_L = point_Bumper_FrameBig.GetChild (0).transform.GetChild (2).GetComponent<Light> ();
				gameObject.GetComponent<ControllerTruck> ().stop_R = point_Bumper_FrameBig.GetChild (0).transform.GetChild (3).GetComponent<Light> ();
				gameObject.GetComponent<ControllerTruck> ().turnSignal_L = point_Bumper_FrameBig.GetChild (0).transform.GetChild (0).GetComponent<Light> ();
				gameObject.GetComponent<ControllerTruck> ().turnSignal_R = point_Bumper_FrameBig.GetChild (0).transform.GetChild (1).GetComponent<Light> ();
			} else if (point_Bumper_FramePlus.childCount > 0) {
				gameObject.GetComponent<ControllerTruck> ().revers_L = point_Bumper_FramePlus.GetChild (0).transform.GetChild (4).GetComponent<Light> ();
				gameObject.GetComponent<ControllerTruck> ().revers_R = point_Bumper_FramePlus.GetChild (0).transform.GetChild (5).GetComponent<Light> ();
				gameObject.GetComponent<ControllerTruck> ().stop_L = point_Bumper_FramePlus.GetChild (0).transform.GetChild (2).GetComponent<Light> ();
				gameObject.GetComponent<ControllerTruck> ().stop_R = point_Bumper_FramePlus.GetChild (0).transform.GetChild (3).GetComponent<Light> ();
				gameObject.GetComponent<ControllerTruck> ().turnSignal_L = point_Bumper_FramePlus.GetChild (0).transform.GetChild (0).GetComponent<Light> ();
				gameObject.GetComponent<ControllerTruck> ().turnSignal_R = point_Bumper_FramePlus.GetChild (0).transform.GetChild (1).GetComponent<Light> ();
			} else if (point_Bumper_FrameSmall.childCount > 0) {
				gameObject.GetComponent<ControllerTruck> ().revers_L = point_Bumper_FrameSmall.GetChild (0).transform.GetChild (4).GetComponent<Light> ();
				gameObject.GetComponent<ControllerTruck> ().revers_R = point_Bumper_FrameSmall.GetChild (0).transform.GetChild (5).GetComponent<Light> ();
				gameObject.GetComponent<ControllerTruck> ().stop_L = point_Bumper_FrameSmall.GetChild (0).transform.GetChild (2).GetComponent<Light> ();
				gameObject.GetComponent<ControllerTruck> ().stop_R = point_Bumper_FrameSmall.GetChild (0).transform.GetChild (3).GetComponent<Light> ();
				gameObject.GetComponent<ControllerTruck> ().turnSignal_L = point_Bumper_FrameSmall.GetChild (0).transform.GetChild (0).GetComponent<Light> ();
				gameObject.GetComponent<ControllerTruck> ().turnSignal_R = point_Bumper_FrameSmall.GetChild (0).transform.GetChild (1).GetComponent<Light> ();
			}
		}
	}
	public void LightPanel(){
		if (gameObject.GetComponent<ControllerTruck> () != null) {
		if (point_PanelCabin.childCount > 0) {
				gameObject.GetComponent<ControllerTruck> ().turnSignalPanel_L = point_PanelCabin.GetChild (0).transform.GetChild (0).GetComponent<Light> ();
				gameObject.GetComponent<ControllerTruck> ().turnSignalPanel_R = point_PanelCabin.GetChild (0).transform.GetChild (1).GetComponent<Light> ();
		}
	}
}
	public void ManipulatorController(){
			if (point_Manipulator_FrameBig_CabBig.childCount > 0) {
			point_Manipulator_FrameBig_CabBig.GetChild (0).GetComponent<Manipulator> ().support_FL_Horizontal = point_Manipulator_FrameBig_CabBig.GetChild (0).transform.GetChild (1);
			point_Manipulator_FrameBig_CabBig.GetChild (0).GetComponent<Manipulator> ().support_FR_Horizontal = point_Manipulator_FrameBig_CabBig.GetChild (0).transform.GetChild (2);
			point_Manipulator_FrameBig_CabBig.GetChild (0).GetComponent<Manipulator> ().support_FL_Vertical = point_Manipulator_FrameBig_CabBig.GetChild (0).transform.GetChild (1).transform.GetChild(0);
			point_Manipulator_FrameBig_CabBig.GetChild (0).GetComponent<Manipulator> ().support_FR_Vertical = point_Manipulator_FrameBig_CabBig.GetChild (0).transform.GetChild (2).transform.GetChild(0);
			//Arrow
			point_Manipulator_FrameBig_CabBig.GetChild (0).GetComponent<Manipulator> ().arrowRotation = point_Manipulator_FrameBig_CabBig.GetChild (0).transform.GetChild (0);
			point_Manipulator_FrameBig_CabBig.GetChild (0).GetComponent<Manipulator> ().arrowForward1 = point_Manipulator_FrameBig_CabBig.GetChild (0).transform.GetChild (0).transform.GetChild (0);
			point_Manipulator_FrameBig_CabBig.GetChild (0).GetComponent<Manipulator> ().arrowForward2 = point_Manipulator_FrameBig_CabBig.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild(0);
			point_Manipulator_FrameBig_CabBig.GetChild (0).GetComponent<Manipulator> ().arrowForward3 = point_Manipulator_FrameBig_CabBig.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild(0).transform.GetChild(0);
			point_Manipulator_FrameBig_CabBig.GetChild (0).GetComponent<Manipulator> ().arrowForward4 = point_Manipulator_FrameBig_CabBig.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0);
			//Piston
			point_Manipulator_FrameBig_CabBig.GetChild (0).GetComponent<Manipulator> ().pistonUp = point_Manipulator_FrameBig_CabBig.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild(1);
			if (manipulator1_Bool == false || manipulator2_Bool == false) {
				point_Manipulator_FrameBig_CabBig.GetChild (0).GetComponent<Manipulator> ().pistonDown = point_Manipulator_FrameBig_CabBig.GetChild (0).transform.GetChild (0).transform.GetChild (2);
				point_Manipulator_FrameBig_CabBig.GetChild (0).transform.GetChild (3).GetComponent<HookManip> ().lineArrow1For = point_Manipulator_FrameBig_CabBig.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (2);
				point_Manipulator_FrameBig_CabBig.GetChild (0).transform.GetChild (3).GetComponent<HookManip> ().lineArrow2For = point_Manipulator_FrameBig_CabBig.GetChild (0).transform.GetChild (0).transform.GetChild (3);
			}
			//Arrow 5 end Piston
			if (manipulator3_Bool == false) {
				point_Manipulator_FrameBig_CabBig.GetChild (0).GetComponent<Manipulator> ().arrowForward5 = point_Manipulator_FrameBig_CabBig.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0);
				point_Manipulator_FrameBig_CabBig.GetChild (0).GetComponent<Manipulator> ().pistonDown = point_Manipulator_FrameBig_CabBig.GetChild (0).transform.GetChild (0).transform.GetChild (1);
				point_Manipulator_FrameBig_CabBig.GetChild (0).GetComponent<Manipulator> ().wingManip = point_Manipulator_FrameBig_CabBig.GetChild (0).transform.GetChild (4).transform.GetChild(10);
			}
			point_Manipulator_FrameBig_CabBig.GetChild (0).GetComponent<Manipulator> ().obj_Manipulator = transform;
		}
			if (point_Manipulator_FrameBig_CabSmall.childCount > 0) {
			point_Manipulator_FrameBig_CabSmall.GetChild (0).GetComponent<Manipulator> ().support_FL_Horizontal = point_Manipulator_FrameBig_CabSmall.GetChild (0).transform.GetChild (1);
			point_Manipulator_FrameBig_CabSmall.GetChild (0).GetComponent<Manipulator> ().support_FR_Horizontal = point_Manipulator_FrameBig_CabSmall.GetChild (0).transform.GetChild (2);
			point_Manipulator_FrameBig_CabSmall.GetChild (0).GetComponent<Manipulator> ().support_FL_Vertical = point_Manipulator_FrameBig_CabSmall.GetChild (0).transform.GetChild (1).transform.GetChild(0);
			point_Manipulator_FrameBig_CabSmall.GetChild (0).GetComponent<Manipulator> ().support_FR_Vertical = point_Manipulator_FrameBig_CabSmall.GetChild (0).transform.GetChild (2).transform.GetChild(0);
			//Arrow
			point_Manipulator_FrameBig_CabSmall.GetChild (0).GetComponent<Manipulator> ().arrowRotation = point_Manipulator_FrameBig_CabSmall.GetChild (0).transform.GetChild (0);
			point_Manipulator_FrameBig_CabSmall.GetChild (0).GetComponent<Manipulator> ().arrowForward1 = point_Manipulator_FrameBig_CabSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0);
			point_Manipulator_FrameBig_CabSmall.GetChild (0).GetComponent<Manipulator> ().arrowForward2 = point_Manipulator_FrameBig_CabSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild(0);
			point_Manipulator_FrameBig_CabSmall.GetChild (0).GetComponent<Manipulator> ().arrowForward3 = point_Manipulator_FrameBig_CabSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild(0).transform.GetChild(0);
			point_Manipulator_FrameBig_CabSmall.GetChild (0).GetComponent<Manipulator> ().arrowForward4 = point_Manipulator_FrameBig_CabSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0);
			//Piston
			point_Manipulator_FrameBig_CabSmall.GetChild (0).GetComponent<Manipulator> ().pistonUp = point_Manipulator_FrameBig_CabSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild(1);
			if (manipulator1_Bool == false || manipulator2_Bool == false) {
				point_Manipulator_FrameBig_CabSmall.GetChild (0).GetComponent<Manipulator> ().pistonDown = point_Manipulator_FrameBig_CabSmall.GetChild (0).transform.GetChild (0).transform.GetChild (2);
				point_Manipulator_FrameBig_CabSmall.GetChild (0).transform.GetChild (3).GetComponent<HookManip> ().lineArrow1For = point_Manipulator_FrameBig_CabSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (2);
				point_Manipulator_FrameBig_CabSmall.GetChild (0).transform.GetChild (3).GetComponent<HookManip> ().lineArrow2For = point_Manipulator_FrameBig_CabSmall.GetChild (0).transform.GetChild (0).transform.GetChild (3);
			}
			//Arrow 5 end Piston
			if (manipulator3_Bool == false) {
				point_Manipulator_FrameBig_CabSmall.GetChild (0).GetComponent<Manipulator> ().arrowForward5 = point_Manipulator_FrameBig_CabSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0);
				point_Manipulator_FrameBig_CabSmall.GetChild (0).GetComponent<Manipulator> ().pistonDown = point_Manipulator_FrameBig_CabSmall.GetChild (0).transform.GetChild (0).transform.GetChild (1);
				point_Manipulator_FrameBig_CabSmall.GetChild (0).GetComponent<Manipulator> ().wingManip = point_Manipulator_FrameBig_CabSmall.GetChild (0).transform.GetChild (4).transform.GetChild(10);
			}
			point_Manipulator_FrameBig_CabSmall.GetChild (0).GetComponent<Manipulator> ().obj_Manipulator = transform;
		}
			if (point_Manipulator_FrameSmall.childCount > 0) {
			point_Manipulator_FrameSmall.GetChild (0).GetComponent<Manipulator> ().support_FL_Horizontal = point_Manipulator_FrameSmall.GetChild (0).transform.GetChild (1);
			point_Manipulator_FrameSmall.GetChild (0).GetComponent<Manipulator> ().support_FR_Horizontal = point_Manipulator_FrameSmall.GetChild (0).transform.GetChild (2);
			point_Manipulator_FrameSmall.GetChild (0).GetComponent<Manipulator> ().support_FL_Vertical = point_Manipulator_FrameSmall.GetChild (0).transform.GetChild (1).transform.GetChild(0);
			point_Manipulator_FrameSmall.GetChild (0).GetComponent<Manipulator> ().support_FR_Vertical = point_Manipulator_FrameSmall.GetChild (0).transform.GetChild (2).transform.GetChild(0);
			//Arrow
			point_Manipulator_FrameSmall.GetChild (0).GetComponent<Manipulator> ().arrowRotation = point_Manipulator_FrameSmall.GetChild (0).transform.GetChild (0);
			point_Manipulator_FrameSmall.GetChild (0).GetComponent<Manipulator> ().arrowForward1 = point_Manipulator_FrameSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0);
			point_Manipulator_FrameSmall.GetChild (0).GetComponent<Manipulator> ().arrowForward2 = point_Manipulator_FrameSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild(0);
			point_Manipulator_FrameSmall.GetChild (0).GetComponent<Manipulator> ().arrowForward3 = point_Manipulator_FrameSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild(0).transform.GetChild(0);
			point_Manipulator_FrameSmall.GetChild (0).GetComponent<Manipulator> ().arrowForward4 = point_Manipulator_FrameSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0);
			//Piston
			point_Manipulator_FrameSmall.GetChild (0).GetComponent<Manipulator> ().pistonUp = point_Manipulator_FrameSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild(1);
			if (manipulator1_Bool == false || manipulator2_Bool == false) {
				point_Manipulator_FrameSmall.GetChild (0).GetComponent<Manipulator> ().pistonDown = point_Manipulator_FrameSmall.GetChild (0).transform.GetChild (0).transform.GetChild (2);
				point_Manipulator_FrameSmall.GetChild (0).transform.GetChild (3).GetComponent<HookManip> ().lineArrow1For = point_Manipulator_FrameSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (2);
				point_Manipulator_FrameSmall.GetChild (0).transform.GetChild (3).GetComponent<HookManip> ().lineArrow2For = point_Manipulator_FrameSmall.GetChild (0).transform.GetChild (0).transform.GetChild (3);
			}
			//Arrow 5 end Piston
			if (manipulator3_Bool == false) {
				point_Manipulator_FrameSmall.GetChild (0).GetComponent<Manipulator> ().arrowForward5 = point_Manipulator_FrameSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0);
				point_Manipulator_FrameSmall.GetChild (0).GetComponent<Manipulator> ().pistonDown = point_Manipulator_FrameSmall.GetChild (0).transform.GetChild (0).transform.GetChild (1);
				point_Manipulator_FrameSmall.GetChild (0).GetComponent<Manipulator> ().wingManip = point_Manipulator_FrameSmall.GetChild (0).transform.GetChild (4).transform.GetChild(10);
			}
			point_Manipulator_FrameSmall.GetChild (0).GetComponent<Manipulator> ().obj_Manipulator = transform;
		}
		if (gameObject.GetComponent<ControllerTruck> () != null) {
			if (point_Manipulator_FrameBig_CabBig.childCount > 0) {
				point_Manipulator_FrameBig_CabBig.GetChild (0).GetComponent<Manipulator> ().m_ScriptM = gameObject.GetComponent<ControllerTruck> ();
				point_Manipulator_FrameBig_CabBig.GetChild (0).transform.GetChild (3).GetComponent<HookManip> ().m_ScriptHook_1 = point_Manipulator_FrameBig_CabBig.GetChild (0).gameObject.GetComponent<Manipulator> ();
				point_Manipulator_FrameBig_CabBig.GetChild (0).transform.GetChild (3).GetComponent<HookManip> ().m_ScriptHook_2 = gameObject.GetComponent<ControllerTruck> ();
			} else if (point_Manipulator_FrameBig_CabSmall.childCount > 0) {
				point_Manipulator_FrameBig_CabSmall.GetChild (0).GetComponent<Manipulator> ().m_ScriptM = gameObject.GetComponent<ControllerTruck> ();
				point_Manipulator_FrameBig_CabSmall.GetChild (0).transform.GetChild (3).GetComponent<HookManip> ().m_ScriptHook_1 = point_Manipulator_FrameBig_CabSmall.GetChild (0).gameObject.GetComponent<Manipulator> ();
				point_Manipulator_FrameBig_CabSmall.GetChild (0).transform.GetChild (3).GetComponent<HookManip> ().m_ScriptHook_2 = gameObject.GetComponent<ControllerTruck> ();
			} else if (point_Manipulator_FrameSmall.childCount > 0) {
				point_Manipulator_FrameSmall.GetChild (0).GetComponent<Manipulator> ().m_ScriptM = gameObject.GetComponent<ControllerTruck> ();
				point_Manipulator_FrameSmall.GetChild (0).transform.GetChild (3).GetComponent<HookManip> ().m_ScriptHook_1 = point_Manipulator_FrameSmall.GetChild (0).gameObject.GetComponent<Manipulator> ();
				point_Manipulator_FrameSmall.GetChild (0).transform.GetChild (3).GetComponent<HookManip> ().m_ScriptHook_2 = gameObject.GetComponent<ControllerTruck> ();
			}
		}
	}
	public void CheckManipulator(){
		if (manipulator1_Bool == false) {
			if (point_Manipulator_FrameBig_CabBig.childCount > 0) {
				point_Manipulator_FrameBig_CabBig.GetChild (0).GetComponent<Manipulator> ().checkManip = 1;
			} else if (point_Manipulator_FrameBig_CabSmall.childCount > 0) {
				point_Manipulator_FrameBig_CabSmall.GetChild (0).GetComponent<Manipulator> ().checkManip = 1;
			} else if (point_Manipulator_FrameSmall.childCount > 0) {
				point_Manipulator_FrameSmall.GetChild (0).GetComponent<Manipulator> ().checkManip = 1;
			}
		} else if (manipulator2_Bool == false) {
			if (point_Manipulator_FrameBig_CabBig.childCount > 0) {
				point_Manipulator_FrameBig_CabBig.GetChild (0).GetComponent<Manipulator> ().checkManip = 2;
			} else if (point_Manipulator_FrameBig_CabSmall.childCount > 0) {
				point_Manipulator_FrameBig_CabSmall.GetChild (0).GetComponent<Manipulator> ().checkManip = 2;
			} else if (point_Manipulator_FrameSmall.childCount > 0) {
				point_Manipulator_FrameSmall.GetChild (0).GetComponent<Manipulator> ().checkManip = 2;
			}
		} else if (manipulator3_Bool == false) {
			if (point_Manipulator_FrameBig_CabBig.childCount > 0) {
				point_Manipulator_FrameBig_CabBig.GetChild (0).GetComponent<Manipulator> ().checkManip = 3;
			} else if (point_Manipulator_FrameBig_CabSmall.childCount > 0) {
				point_Manipulator_FrameBig_CabSmall.GetChild (0).GetComponent<Manipulator> ().checkManip = 3;
			} else if (point_Manipulator_FrameSmall.childCount > 0) {
				point_Manipulator_FrameSmall.GetChild (0).GetComponent<Manipulator> ().checkManip = 3;
			}
		}
	}
	public void AddBodyDop3(){
		if (body_Bool == false) {
			if (BodyDop3_Bool == true) {
				if (frame_Int == 1) {
					var bodyDop1 = Instantiate (BodyDop3, point_Body_FrameBaig.GetChild(0).transform.GetChild(1).position, Quaternion.identity);
					bodyDop1.transform.parent = point_Body_FrameBaig.GetChild (0).transform.GetChild (1).transform;
				} else if (frame_Int == 2) {
					var bodyDop1 = Instantiate (BodyDop3, point_Body_FrameSmall.GetChild(0).transform.GetChild(1).position, Quaternion.identity);
					bodyDop1.transform.parent = point_Body_FrameSmall.GetChild (0).transform.GetChild (1).transform;
				}
				BodyDop3_Bool = false;
			} else if (BodyDop3_Bool == false) {
				if (frame_Int == 1) {
					foreach (Transform obj_Car in point_Body_FrameBaig) {
						DestroyImmediate (point_Body_FrameBaig.GetChild(0).transform.GetChild(1).transform.GetChild(0).gameObject);
					}
				} else if (frame_Int == 2) {
					foreach (Transform obj_Car in point_Body_FrameSmall) {
						DestroyImmediate (point_Body_FrameSmall.GetChild(0).transform.GetChild(1).transform.GetChild(0).gameObject);
					}
				}
				BodyDop3_Bool = true;
			}
		}
	}
	public void AddCrable(){
		if (Cradle_Bool == true) {
			if (point_Manipulator_FrameSmall.childCount > 0) {
				var cr = Instantiate (Cradle, point_Manipulator_FrameSmall.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(5).position, Quaternion.identity);
				cr.transform.parent = point_Manipulator_FrameSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (5).transform;
				if (point_Manipulator_FrameSmall.childCount != 0) {
					cr.GetComponent<Crable> ().m_ScriptManipulator = point_Manipulator_FrameSmall.GetChild (0).GetComponent<Manipulator>();
				}
				if (gameObject.GetComponent<ControllerTruck> () != null) {
					gameObject.GetComponent<ControllerTruck>().m_ScriptCrable = point_Manipulator_FrameSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (5).transform.GetChild(0).GetComponent<Crable>();
				}
			} else if (point_Manipulator_FrameBig_CabSmall.childCount > 0) {
				var cr = Instantiate (Cradle, point_Manipulator_FrameBig_CabSmall.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(5).position, Quaternion.identity);
				cr.transform.parent = point_Manipulator_FrameBig_CabSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (5).transform;
				if (point_Manipulator_FrameBig_CabSmall.childCount != 0) {
					cr.GetComponent<Crable> ().m_ScriptManipulator = point_Manipulator_FrameBig_CabSmall.GetChild (0).GetComponent<Manipulator>();
				}
				if (gameObject.GetComponent<ControllerTruck> () != null) {
					gameObject.GetComponent<ControllerTruck>().m_ScriptCrable = point_Manipulator_FrameBig_CabSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (5).transform.GetChild(0).GetComponent<Crable>();
				}
			} else if (point_Manipulator_FrameBig_CabBig.childCount > 0) {
				var cr = Instantiate (Cradle, point_Manipulator_FrameBig_CabBig.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(5).position, Quaternion.identity);
				cr.transform.parent = point_Manipulator_FrameBig_CabBig.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (5).transform;
				if (point_Manipulator_FrameBig_CabBig.childCount != 0) {
					cr.GetComponent<Crable> ().m_ScriptManipulator = point_Manipulator_FrameBig_CabBig.GetChild (0).GetComponent<Manipulator>();
				}
				if (gameObject.GetComponent<ControllerTruck> () != null) {
					gameObject.GetComponent<ControllerTruck>().m_ScriptCrable = point_Manipulator_FrameBig_CabBig.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (5).transform.GetChild(0).GetComponent<Crable>();
				}
			}
			Cradle_Bool = false;
		} else if (Cradle_Bool == false) {
			if (point_Manipulator_FrameSmall.childCount > 0) {
				foreach (Transform obj_Car in point_Manipulator_FrameSmall) {
					DestroyImmediate (point_Manipulator_FrameSmall.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(5).transform.GetChild(0).gameObject);
				}
			} else if (point_Manipulator_FrameBig_CabSmall.childCount > 0) {
				foreach (Transform obj_Car in point_Manipulator_FrameBig_CabSmall) {
					DestroyImmediate (point_Manipulator_FrameBig_CabSmall.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(5).transform.GetChild(0).gameObject);
				}
			} else if (point_Manipulator_FrameBig_CabBig.childCount > 0) {
				foreach (Transform obj_Car in point_Manipulator_FrameBig_CabBig) {
					DestroyImmediate (point_Manipulator_FrameBig_CabBig.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(5).transform.GetChild(0).gameObject);
				}
			}
			Cradle_Bool = true;
		}
	}
	public void CheckCrable(){
		if (Cradle_Bool == false) {
			if (frame_Int == 1 && cabin_Int == 1) {
				var cr = Instantiate (Cradle, point_Manipulator_FrameBig_CabSmall.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(5).position, Quaternion.identity);
				cr.transform.parent = point_Manipulator_FrameBig_CabSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (5).transform;
				if (point_Manipulator_FrameBig_CabSmall.childCount != 0) {
					cr.GetComponent<Crable> ().m_ScriptManipulator = point_Manipulator_FrameBig_CabSmall.GetChild (0).GetComponent<Manipulator>();
				}
				if (gameObject.GetComponent<ControllerTruck> () != null) {
					gameObject.GetComponent<ControllerTruck>().m_ScriptCrable = point_Manipulator_FrameBig_CabSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (5).transform.GetChild(0).GetComponent<Crable>();
				}
			} else if (frame_Int == 1 && cabin_Int == 2) {
				var cr = Instantiate (Cradle, point_Manipulator_FrameBig_CabBig.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(5).position, Quaternion.identity);
				cr.transform.parent = point_Manipulator_FrameBig_CabBig.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (5).transform;
				if (point_Manipulator_FrameBig_CabBig.childCount != 0) {
					cr.GetComponent<Crable> ().m_ScriptManipulator = point_Manipulator_FrameBig_CabBig.GetChild (0).GetComponent<Manipulator>();
				}
				if (gameObject.GetComponent<ControllerTruck> () != null) {
					gameObject.GetComponent<ControllerTruck>().m_ScriptCrable = point_Manipulator_FrameBig_CabBig.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (5).transform.GetChild(0).GetComponent<Crable>();
				}
			} else if (frame_Int == 2 && cabin_Int == 1) {
				var cr = Instantiate (Cradle, point_Manipulator_FrameSmall.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(5).position, Quaternion.identity);
				cr.transform.parent = point_Manipulator_FrameSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (5).transform;
				if (point_Manipulator_FrameSmall.childCount != 0) {
					cr.GetComponent<Crable> ().m_ScriptManipulator = point_Manipulator_FrameSmall.GetChild (0).GetComponent<Manipulator>();
				}
				if (gameObject.GetComponent<ControllerTruck> () != null) {
					gameObject.GetComponent<ControllerTruck>().m_ScriptCrable = point_Manipulator_FrameSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (5).transform.GetChild(0).GetComponent<Crable>();
				}
			}
		}
	}
	public void AddConponentArrow(){
		if (gameObject.GetComponent<ControllerTruck> () != null) {
			if (manipulator1_Bool == false || manipulator2_Bool == false) {
				if (point_Manipulator_FrameSmall.childCount > 0) {
					point_Manipulator_FrameSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).GetComponent<CheckOnCollider> ().m_ScriptManip = point_Manipulator_FrameSmall.GetChild(0).GetComponent<Manipulator> ();
					point_Manipulator_FrameSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).GetComponent<CheckOnCollider> ().m_ScriptManip = point_Manipulator_FrameSmall.GetChild (0).GetComponent<Manipulator> ();
					point_Manipulator_FrameSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).GetComponent<CheckOnCollider> ().m_ScriptManip = point_Manipulator_FrameSmall.GetChild (0).GetComponent<Manipulator> ();
					point_Manipulator_FrameSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).GetComponent<CheckOnCollider> ().m_ScriptManip = point_Manipulator_FrameSmall.GetChild (0).GetComponent<Manipulator> ();
					point_Manipulator_FrameSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).GetComponent<CheckOnCollider> ().m_ScriptManip = point_Manipulator_FrameSmall.GetChild (0).GetComponent<Manipulator> ();
				} else if (point_Manipulator_FrameBig_CabSmall.childCount > 0) {
					point_Manipulator_FrameBig_CabSmall.GetChild (0).transform.GetChild (0).transform.GetChild(0).GetComponent<CheckOnCollider> ().m_ScriptManip = point_Manipulator_FrameBig_CabSmall.GetChild (0).GetComponent<Manipulator> ();
					point_Manipulator_FrameBig_CabSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).GetComponent<CheckOnCollider> ().m_ScriptManip = point_Manipulator_FrameBig_CabSmall.GetChild (0).GetComponent<Manipulator> ();
					point_Manipulator_FrameBig_CabSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).GetComponent<CheckOnCollider> ().m_ScriptManip = point_Manipulator_FrameBig_CabSmall.GetChild (0).GetComponent<Manipulator> ();
					point_Manipulator_FrameBig_CabSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).GetComponent<CheckOnCollider> ().m_ScriptManip = point_Manipulator_FrameBig_CabSmall.GetChild (0).GetComponent<Manipulator> ();
					point_Manipulator_FrameBig_CabSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).GetComponent<CheckOnCollider> ().m_ScriptManip = point_Manipulator_FrameBig_CabSmall.GetChild (0).GetComponent<Manipulator> ();
				} else if (point_Manipulator_FrameBig_CabBig.childCount > 0) {
					point_Manipulator_FrameBig_CabBig.GetChild (0).transform.GetChild (0).transform.GetChild (0).GetComponent<CheckOnCollider> ().m_ScriptManip = point_Manipulator_FrameBig_CabBig.GetChild (0).GetComponent<Manipulator> ();
					point_Manipulator_FrameBig_CabBig.GetChild (0).transform.GetChild (0).transform.GetChild (0).GetComponent<CheckOnCollider> ().m_ScriptManip = point_Manipulator_FrameBig_CabBig.GetChild (0).GetComponent<Manipulator> ();
					point_Manipulator_FrameBig_CabBig.GetChild (0).transform.GetChild (0).transform.GetChild (0).GetComponent<CheckOnCollider> ().m_ScriptManip = point_Manipulator_FrameBig_CabBig.GetChild (0).GetComponent<Manipulator> ();
					point_Manipulator_FrameBig_CabBig.GetChild (0).transform.GetChild (0).transform.GetChild (0).GetComponent<CheckOnCollider> ().m_ScriptManip = point_Manipulator_FrameBig_CabBig.GetChild (0).GetComponent<Manipulator> ();
					point_Manipulator_FrameBig_CabBig.GetChild (0).transform.GetChild (0).transform.GetChild (0).GetComponent<CheckOnCollider> ().m_ScriptManip = point_Manipulator_FrameBig_CabBig.GetChild (0).GetComponent<Manipulator> ();
				}
			} else if (manipulator3_Bool == false) {
				if (point_Manipulator_FrameSmall.childCount > 0) {
					point_Manipulator_FrameSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).GetComponent<CheckOnCollider> ().m_ScriptManip = point_Manipulator_FrameSmall.GetChild(0).GetComponent<Manipulator> ();
					point_Manipulator_FrameSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).GetComponent<CheckOnCollider> ().m_ScriptManip = point_Manipulator_FrameSmall.GetChild (0).GetComponent<Manipulator> ();
					point_Manipulator_FrameSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).GetComponent<CheckOnCollider> ().m_ScriptManip = point_Manipulator_FrameSmall.GetChild (0).GetComponent<Manipulator> ();
					point_Manipulator_FrameSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).GetComponent<CheckOnCollider> ().m_ScriptManip = point_Manipulator_FrameSmall.GetChild (0).GetComponent<Manipulator> ();
					point_Manipulator_FrameSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).GetComponent<CheckOnCollider> ().m_ScriptManip = point_Manipulator_FrameSmall.GetChild (0).GetComponent<Manipulator> ();
				} else if (point_Manipulator_FrameBig_CabSmall.childCount > 0) {
					point_Manipulator_FrameBig_CabSmall.GetChild (0).transform.GetChild (0).transform.GetChild(0).GetComponent<CheckOnCollider> ().m_ScriptManip = point_Manipulator_FrameBig_CabSmall.GetChild (0).GetComponent<Manipulator> ();
					point_Manipulator_FrameBig_CabSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).GetComponent<CheckOnCollider> ().m_ScriptManip = point_Manipulator_FrameBig_CabSmall.GetChild (0).GetComponent<Manipulator> ();
					point_Manipulator_FrameBig_CabSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).GetComponent<CheckOnCollider> ().m_ScriptManip = point_Manipulator_FrameBig_CabSmall.GetChild (0).GetComponent<Manipulator> ();
					point_Manipulator_FrameBig_CabSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).GetComponent<CheckOnCollider> ().m_ScriptManip = point_Manipulator_FrameBig_CabSmall.GetChild (0).GetComponent<Manipulator> ();
					point_Manipulator_FrameBig_CabSmall.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).GetComponent<CheckOnCollider> ().m_ScriptManip = point_Manipulator_FrameBig_CabSmall.GetChild (0).GetComponent<Manipulator> ();
				} else if (point_Manipulator_FrameBig_CabBig.childCount > 0) {
					point_Manipulator_FrameBig_CabBig.GetChild (0).transform.GetChild (0).transform.GetChild (0).GetComponent<CheckOnCollider> ().m_ScriptManip = point_Manipulator_FrameBig_CabBig.GetChild (0).GetComponent<Manipulator> ();
					point_Manipulator_FrameBig_CabBig.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).GetComponent<CheckOnCollider> ().m_ScriptManip = point_Manipulator_FrameBig_CabBig.GetChild (0).GetComponent<Manipulator> ();
					point_Manipulator_FrameBig_CabBig.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).GetComponent<CheckOnCollider> ().m_ScriptManip = point_Manipulator_FrameBig_CabBig.GetChild (0).GetComponent<Manipulator> ();
					point_Manipulator_FrameBig_CabBig.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).GetComponent<CheckOnCollider> ().m_ScriptManip = point_Manipulator_FrameBig_CabBig.GetChild (0).GetComponent<Manipulator> ();
					point_Manipulator_FrameBig_CabBig.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).transform.GetChild (0).GetComponent<CheckOnCollider> ().m_ScriptManip = point_Manipulator_FrameBig_CabBig.GetChild (0).GetComponent<Manipulator> ();
				}
			}
		}
	}
}