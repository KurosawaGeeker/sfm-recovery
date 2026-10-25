using ExposureUnnoticed2.Master.Action;

namespace ExposureUnnoticed2.ObjectUI.InventoryPanel
{
	public interface IEquipableMaster
	{
		string NameKey { get; }

		string DescriptionKey { get; }

		ActionType TransAttachActionType();

		ActionType TransDetachActionType();
	}
}
