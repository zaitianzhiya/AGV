using System;

namespace Model.MDM
{
	public enum LogicEnum
	{
		None,
		StandbyToStock,
		StockToFeeding,
		FeedingToStock,
		FeedingToStandby,
		StandbyToReclaiming,
		ReclaimingToATemporary,
		ATemporaryToReclaiming,
		ATemporaryToStandby,
		StandbyToATemporary,
		ATemporaryToHeadFeeding,
		HeadFeedingToHeadReclaiming,
		HeadReclaimingToAEmptyTemporary,
		AEmptyTemporaryToATemporary,
		AEmptyTemporaryToStandby,
		StandbyToBEmptyTemporary,
		BEmptyTemporaryToEndFeeding,
		EndFeedingToEndReclaiming,
		EndReclaimingToBTemporary,
		BTemporaryToBEmptyTemporary,
		BTemporaryToStandy,
		StandbyToBTemporary,
		BTemporaryToCableFeeding,
		CableFeedingToBTemporary,
		CableFeedingToStandby,
		StandbyToCableReclaiming,
		CableReclaimingToBEmptyTemporary,
		BEmptyTemporaryToCableReclaiming,
		BEmptyTemporaryToStandby
	}
}
