using System.Collections.Generic;
using UnityEngine;

namespace ExposureUnnoticed2.Master.Item
{
	[CreateAssetMenu(fileName = "MItem", menuName = "ScriptableObjects/MItem")]
	public class MItem : ScriptableObject
	{
		[SerializeField]
		private MItemRecord[] records;

		private static Dictionary<ItemType, MItemRecord> dict;

		private static void CheckInitialize()
		{
			// Recovered from GameAssembly.dll RVA 0x467730
			if (dict != null) return;
			dict = new Dictionary<ItemType, MItemRecord>();
			foreach (MItemRecord record in global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.MItem.records)
			{
			    dict.Add(record.Type, record);
			}
		}

		public static MItemRecord Get(ItemType type)
		{
			// Recovered from GameAssembly.dll RVA 0x467900
			CheckInitialize();
			return dict.TryGetValue(type, out MItemRecord record) ? record : null;
		}

		public static Dictionary<ItemType, MItemRecord> GetAll()
		{
			// Recovered from GameAssembly.dll RVA 0x4679A0
			CheckInitialize();
			return dict;
		}

		public static void UseItemProcess(MItemRecord record)
		{
            // Complete original 0x4679F0, including its 11-entry table and all out-of-table branches.
            switch ((int)record.Type)
            {
                case -1: return;
                case 0:
                    if (!global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.IsPeeing)
                    {
                        global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.ChangeItemNum(record.Type, -1);
                        global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.TransAction((global::ExposureUnnoticed2.Master.Action.ActionType)10012);
                        global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.AddAddMoisture(global::ExposureUnnoticed2.Master.Common.MCommon.Instance.AddMoistureWater);
                        return;
                    }
                    global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("item_peeing_not_water"));
                    return;
                case 1:
                    if (global::ExposureUnnoticed2.Object3D.ScenePlops.Escalator.EscalatorManager.Instance != null &&
                        global::ExposureUnnoticed2.Object3D.ScenePlops.Escalator.EscalatorManager.Instance.IsPushed(global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.GameObject))
                    {
                        global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("item_cant_use_dildo_escalator"));
                        return;
                    }
                    global::ExposureUnnoticed2.ObjectUI.ChooseDildoPanelView.ChooseDildoPanelView.Open();
                    return;
                case 2:
                    if (!global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsActiveInvisible)
                    {
                        global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.ChangeItemNum(record.Type, -1);
                        global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.LastUseItemType = (ItemType)2;
                        global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.TransAction((global::ExposureUnnoticed2.Master.Action.ActionType)5);
                        return;
                    }
                    global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("item_already_active_invisible"));
                    return;
                case 3:
                    if (!global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsActiveFutanari)
                    {
                        global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.LastUseItemType = (ItemType)3;
                        global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.TransAction((global::ExposureUnnoticed2.Master.Action.ActionType)5);
                        return;
                    }
                    global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("item_already_active_futanari"));
                    return;
                case 4:
                    if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsActiveFutanari)
                    {
                        global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.LastUseItemType = (ItemType)4;
                        global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.TransAction((global::ExposureUnnoticed2.Master.Action.ActionType)5);
                        return;
                    }
                    global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("item_already_inactive_futanari"));
                    return;
                case 5:
                    global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.LastUseItemType = (ItemType)5;
                    global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.CommonEquipEndTime = Time.time + 3f;
                    global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.TransAction((global::ExposureUnnoticed2.Master.Action.ActionType)10008);
                    global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.OnEndCommonEquip = () => global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.BodyPaintController.OnUseItem();
                    return;
                case 6:
                    if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsActiveInvisible)
                    {
                        global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.ChangeItemNum(record.Type, -1);
                        global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.LastUseItemType = (ItemType)6;
                        global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.TransAction((global::ExposureUnnoticed2.Master.Action.ActionType)5);
                        return;
                    }
                    global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("item_already_inactive_invisible"));
                    return;
                case 7:
                    if (!global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsActiveInvisible)
                    {
                        global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.LastUseItemType = (ItemType)7;
                        global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.TransAction((global::ExposureUnnoticed2.Master.Action.ActionType)5);
                        return;
                    }
                    global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("item_already_active_invisible"));
                    return;
                case 8:
                    global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.LastUseItemType = (ItemType)8;
                    global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.CommonEquipEndTime = Time.time + 3f;
                    global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.TransAction((global::ExposureUnnoticed2.Master.Action.ActionType)10008);
                    global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.OnEndCommonEquip = () => global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.BodyPaintController.OnUseItem();
                    return;
                case 9:
                    global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.LastUseItemType = (ItemType)9;
                    global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.CommonEquipEndTime = Time.time + 3f;
                    global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.TransAction((global::ExposureUnnoticed2.Master.Action.ActionType)10008);
                    global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.OnEndCommonEquip = () => global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.BodyPaintController.OnUseWashItem();
                    return;
                case 10000: return;
                case 10001: return;
                case 10002:
                    if (!global::ExposureUnnoticed2.Master.Stage.MStage.Get(global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.CurrentStageType).IsUseDrone)
                    {
                        global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("item_cant_use_drone"));
                        return;
                    }
                    if (!global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsRelease((global::ExposureUnnoticed2.Master.RankRelease.RankReleaseType)106))
                    {
                        global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("item_not_enough_rank_drone"));
                        return;
                    }
                    global::ExposureUnnoticed2.ObjectUI.DroneMissionPanel.DroneMissionPanelView.Open();
                    return;
                case 99999:
                    global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.RiseRp(100f);
                    return;
                case 100000:
                    global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.Player.Pca.Mental.RecoveryMental();
                    return;
                case 100001:
                    global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.RiseRp(10000f);
                    return;
                case 100002:
                    global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.ExposeExp = unchecked(global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.ExposeExp + 1000);
                    global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.Player.Pca.PlayerState.CurrentRank = global::ExposureUnnoticed2.Master.Rank.MRank.CalcCurrentRank(global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.ExposeExp);
                    return;
                case 100003:
                    global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.ExposeExp = unchecked(global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.ExposeExp + 100);
                    global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.Player.Pca.PlayerState.CurrentRank = global::ExposureUnnoticed2.Master.Rank.MRank.CalcCurrentRank(global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.ExposeExp);
                    return;
                default: throw new System.ArgumentOutOfRangeException();
            }
        }
	}
}
