using System.Collections.Generic;
using ExposureUnnoticed2.Master.Accessory;
using UnityEngine;

namespace ExposureUnnoticed2.Master.AdultGoods
{
	[CreateAssetMenu(fileName = "MAdultGoods", menuName = "ScriptableObjects/MAdultGoods")]
	public class MAdultGoods : ScriptableObject
	{
		[SerializeField]
		private MAdultGoodsRecord[] records;

		private static Dictionary<MAdultGoodsType, MAdultGoodsRecord> dict;

		private static void CheckInitialize()
		{
			// Recovered from GameAssembly.dll RVA 0x46C980
			if (dict != null) return;
			dict = new Dictionary<MAdultGoodsType, MAdultGoodsRecord>();
			foreach (MAdultGoodsRecord record in global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.mAdultGoods.records)
			{
			    dict.Add(record.Type, record);
			}
		}

		public static MAdultGoodsRecord Get(MAdultGoodsType type)
		{
			// Recovered from GameAssembly.dll RVA 0x46CB50
			CheckInitialize();
			return dict.TryGetValue(type, out MAdultGoodsRecord record) ? record : null;
		}

		public static Dictionary<MAdultGoodsType, MAdultGoodsRecord> GetAll()
		{
			// Recovered from GameAssembly.dll RVA 0x46CBF0
			CheckInitialize();
			return dict;
		}

		public static void UseItemProcess(MAdultGoodsRecord record)
		{
            // Recovered complete original native body.
            switch ((int)record.Type)
            {
                case 0: case 1: case 6: case 7: case 8: case 9: case 10:
                    global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)17, -1f, false);
                    global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.ChangeAccessoryIfAble(record);
                    break;
                case 2:
                    global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)48, -1f, false);
                    global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.ChangeAccessoryIfAble(record);
                    break;
                case 3: case 5:
                    if (!global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsEquipAdultGoods(record.Type))
                        global::ExposureUnnoticed2.ObjectUI.ChooseHandcuffsPanel.ChooseHandcuffsPanelView.Open(record.Type);
                    break;
                case 4:
                    global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)48, -1f, false);
                    global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.Player.Pca.PlayerState.CompleteAttachHandcuffsTime = Time.time + global::ExposureUnnoticed2.Master.Common.MCommon.Instance.AttachHandcuffsTime;
                    global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.Player.Pca.PlayerState.LastAttachedHandcuffType = (MAdultGoodsType)4;
                    global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.TransAction((global::ExposureUnnoticed2.Master.Action.ActionType)10019);
                    break;
            }
        }
	}
}
