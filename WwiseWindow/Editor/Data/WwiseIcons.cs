using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WS
{
    public  class WwiseIcons 
    {
       public const float kIconWidth = 18f;

		private UnityEngine.Texture2D m_textureWwiseAcousticTextureIcon;
		private UnityEngine.Texture2D m_textureWwiseAuxBusIcon;
		private UnityEngine.Texture2D m_textureWwiseBusIcon;
		private UnityEngine.Texture2D m_textureWwiseEventIcon;
		private UnityEngine.Texture2D m_textureWwiseFolderIcon;
		private UnityEngine.Texture2D m_textureWwiseGameParameterIcon;
		private UnityEngine.Texture2D m_textureWwisePhysicalFolderIcon;
		private UnityEngine.Texture2D m_textureWwiseProjectIcon;
		private UnityEngine.Texture2D m_textureWwiseSoundbankIcon;
		private UnityEngine.Texture2D m_textureWwiseStateIcon;
		private UnityEngine.Texture2D m_textureWwiseStateGroupIcon;
		private UnityEngine.Texture2D m_textureWwiseSwitchIcon;
		private UnityEngine.Texture2D m_textureWwiseSwitchGroupIcon;
		private UnityEngine.Texture2D m_textureWwiseWorkUnitIcon;
		private UnityEngine.Texture2D m_textureWwiseTriggerIcon;

		public WwiseIcons()
		{
			LoadIcons();
		}
		
	protected UnityEngine.Texture2D GetTexture(string texturePath)
	{
		try
		{
			return UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Texture2D>(texturePath);
		}
		catch (System.Exception ex)
		{
			UnityEngine.Debug.LogError(string.Format("WwiseUnity: Failed to find local texture: {0}", ex));
			return null;
		}
	}

	public void LoadIcons()
	{
		var tempWwisePath = "Assets/Wwise/API/Editor/WwiseWindows/TreeViewIcons/";

		m_textureWwiseAcousticTextureIcon = GetTexture(tempWwisePath + "acoustictexture_nor.png");
		m_textureWwiseAuxBusIcon = GetTexture(tempWwisePath + "auxbus_nor.png");
		m_textureWwiseBusIcon = GetTexture(tempWwisePath + "bus_nor.png");
		m_textureWwiseEventIcon = GetTexture(tempWwisePath + "event_nor.png");
		m_textureWwiseFolderIcon = GetTexture(tempWwisePath + "folder_nor.png");
		m_textureWwiseGameParameterIcon = GetTexture(tempWwisePath + "gameparameter_nor.png");
		m_textureWwisePhysicalFolderIcon = GetTexture(tempWwisePath + "physical_folder_nor.png");
		m_textureWwiseProjectIcon = GetTexture(tempWwisePath + "wproj.png");
		m_textureWwiseSoundbankIcon = GetTexture(tempWwisePath + "soundbank_nor.png");
		m_textureWwiseStateIcon = GetTexture(tempWwisePath + "state_nor.png");
		m_textureWwiseStateGroupIcon = GetTexture(tempWwisePath + "stategroup_nor.png");
		m_textureWwiseSwitchIcon = GetTexture(tempWwisePath + "switch_nor.png");
		m_textureWwiseSwitchGroupIcon = GetTexture(tempWwisePath + "switchgroup_nor.png");
		m_textureWwiseWorkUnitIcon = GetTexture(tempWwisePath + "workunit_nor.png");
		m_textureWwiseTriggerIcon = GetTexture(tempWwisePath + "trigger_nor.png");
	}

	public Texture2D GetIcon(WwiseObjectType type)
	{
		switch (type)
		{
			case WwiseObjectType.AcousticTexture:
				return m_textureWwiseAcousticTextureIcon;
			case WwiseObjectType.AuxBus:
				return m_textureWwiseAuxBusIcon;
			case WwiseObjectType.Bus:
				return m_textureWwiseBusIcon;
			case WwiseObjectType.Event:
				return m_textureWwiseEventIcon;
			case WwiseObjectType.Folder:
				return m_textureWwiseFolderIcon;
			case WwiseObjectType.GameParameter:
				return m_textureWwiseGameParameterIcon;
			case WwiseObjectType.PhysicalFolder:
				return m_textureWwisePhysicalFolderIcon;
			case WwiseObjectType.Project:
				return m_textureWwiseProjectIcon;
			case WwiseObjectType.Soundbank:
				return m_textureWwiseSoundbankIcon;
			case WwiseObjectType.State:
				return m_textureWwiseStateIcon;
			case WwiseObjectType.StateGroup:
				return m_textureWwiseStateGroupIcon;
			case WwiseObjectType.Switch:
				return m_textureWwiseSwitchIcon;
			case WwiseObjectType.SwitchGroup:
				return m_textureWwiseSwitchGroupIcon;
			case WwiseObjectType.WorkUnit:
				return m_textureWwiseWorkUnitIcon;
			case WwiseObjectType.Trigger:
				return m_textureWwiseTriggerIcon;
			default:
				return m_textureWwisePhysicalFolderIcon;
		}
	}
    }
}
