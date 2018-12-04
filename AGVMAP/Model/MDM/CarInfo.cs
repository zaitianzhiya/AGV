using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.MDM
{
	[Serializable]
	public class CarInfo : CarBaseStateInfo
	{
		public int TaskDetailID
		{
			get;
			set;
		}

		public DateTime ThroughPreLMTime
		{
			get;
			set;
		}

		public double TimesAfterThroughPreLM
		{
			get
			{
				return (DateTime.Now - this.ThroughPreLMTime).TotalMilliseconds;
			}
		}

		public bool IsSelect
		{
			get;
			set;
		}

		public string CarName
		{
			get;
			set;
		}

		public int OnCarRouteID
		{
			get;
			set;
		}

		public string StandbyLandMark
		{
			get;
			set;
		}

		public LandmarkInfo CurrentLandMark
		{
			get;
			set;
		}

		public LandmarkInfo NextLandMark
		{
			get;
			set;
		}

		public LandmarkInfo NextNextLandMark
		{
			get;
			set;
		}

		public LandmarkInfo ThirdLandMark
		{
			get;
			set;
		}

		public string ThirdLandMarkCode
		{
			get
			{
				return (this.ThirdLandMark == null) ? "" : this.ThirdLandMark.LandmarkCode;
			}
		}

		public string NextNextLandMarkCode
		{
			get
			{
				bool flag = this.NextNextLandMark == null;
				string result;
				if (flag)
				{
					result = "";
				}
				else
				{
					result = this.NextNextLandMark.LandmarkCode;
				}
				return result;
			}
		}

		public string CurrentLandCode
		{
			get
			{
				bool flag = this.CurrentLandMark == null;
				string result;
				if (flag)
				{
					result = "";
				}
				else
				{
					result = this.CurrentLandMark.LandmarkCode;
				}
				return result;
			}
		}

		public string NextLandCode
		{
			get
			{
				bool flag = this.NextLandMark == null;
				string result;
				if (flag)
				{
					result = "";
				}
				else
				{
					result = this.NextLandMark.LandmarkCode;
				}
				return result;
			}
		}

		public string DispatchTaskNo
		{
			get
			{
				bool flag = this.DispatchTask == null;
				string result;
				if (flag)
				{
					result = "";
				}
				else
				{
					result = this.DispatchTask.dispatchNo;
				}
				return result;
			}
		}

		public string PreDispatchTaskNo
		{
			get
			{
				bool flag = this.PreDispatchTask == null;
				string result;
				if (flag)
				{
					result = "";
				}
				else
				{
					result = this.PreDispatchTask.dispatchNo;
				}
				return result;
			}
		}

		public DispatchTaskInfo DispatchTask
		{
			get;
			set;
		}

		public DispatchTaskInfo PreDispatchTask
		{
			get;
			set;
		}

		public DateTime FinishLogicTime
		{
			get;
			set;
		}

		public DateTime DispatchTaskTime
		{
			get;
			set;
		}

		public List<string> LockLandMarkList
		{
			get;
			set;
		}

		public string LockLandMarks
		{
			get
			{
				return string.Join(",", this.LockLandMarkList.Distinct<string>().ToArray<string>());
			}
		}

		public string CurrLogicStr
		{
			get
			{
				bool flag = base.CurrLogic == LogicEnum.StandbyToStock;
				string result;
				if (flag)
				{
					result = "待命点到备料点";
				}
				else
				{
					bool flag2 = base.CurrLogic == LogicEnum.StockToFeeding;
					if (flag2)
					{
						result = "备料点到上料";
					}
					else
					{
						bool flag3 = base.CurrLogic == LogicEnum.FeedingToStock;
						if (flag3)
						{
							result = "上料到备料";
						}
						else
						{
							bool flag4 = base.CurrLogic == LogicEnum.FeedingToStandby;
							if (flag4)
							{
								result = "上料到原点";
							}
							else
							{
								bool flag5 = base.CurrLogic == LogicEnum.StandbyToReclaiming;
								if (flag5)
								{
									result = "原点到下料点";
								}
								else
								{
									bool flag6 = base.CurrLogic == LogicEnum.ReclaimingToATemporary;
									if (flag6)
									{
										result = "下料点到暂存区";
									}
									else
									{
										bool flag7 = base.CurrLogic == LogicEnum.ATemporaryToReclaiming;
										if (flag7)
										{
											result = "暂存区到下料点";
										}
										else
										{
											bool flag8 = base.CurrLogic == LogicEnum.ATemporaryToStandby;
											if (flag8)
											{
												result = "暂存区到原点";
											}
											else
											{
												bool flag9 = base.CurrLogic == LogicEnum.StandbyToATemporary;
												if (flag9)
												{
													result = "原点到A暂存区";
												}
												else
												{
													bool flag10 = base.CurrLogic == LogicEnum.ATemporaryToHeadFeeding;
													if (flag10)
													{
														result = "暂存区到线头上料";
													}
													else
													{
														bool flag11 = base.CurrLogic == LogicEnum.HeadFeedingToHeadReclaiming;
														if (flag11)
														{
															result = "线头上料到线头取空盒";
														}
														else
														{
															bool flag12 = base.CurrLogic == LogicEnum.HeadReclaimingToAEmptyTemporary;
															if (flag12)
															{
																result = "线头取空盒到A空盒区";
															}
															else
															{
																bool flag13 = base.CurrLogic == LogicEnum.AEmptyTemporaryToATemporary;
																if (flag13)
																{
																	result = "A空盒区到A暂存区";
																}
																else
																{
																	bool flag14 = base.CurrLogic == LogicEnum.AEmptyTemporaryToStandby;
																	if (flag14)
																	{
																		result = "A空盒区到原点";
																	}
																	else
																	{
																		bool flag15 = base.CurrLogic == LogicEnum.StandbyToBEmptyTemporary;
																		if (flag15)
																		{
																			result = "原点到B空盒";
																		}
																		else
																		{
																			bool flag16 = base.CurrLogic == LogicEnum.BEmptyTemporaryToEndFeeding;
																			if (flag16)
																			{
																				result = "B区空盒去到线尾上空盒";
																			}
																			else
																			{
																				bool flag17 = base.CurrLogic == LogicEnum.EndFeedingToEndReclaiming;
																				if (flag17)
																				{
																					result = "线尾上空盒到线尾取料";
																				}
																				else
																				{
																					bool flag18 = base.CurrLogic == LogicEnum.EndReclaimingToBTemporary;
																					if (flag18)
																					{
																						result = "线尾取料到B暂存区";
																					}
																					else
																					{
																						bool flag19 = base.CurrLogic == LogicEnum.BTemporaryToBEmptyTemporary;
																						if (flag19)
																						{
																							result = "B暂存区到B空盒区";
																						}
																						else
																						{
																							bool flag20 = base.CurrLogic == LogicEnum.BTemporaryToStandy;
																							if (flag20)
																							{
																								result = "B暂存区到待命点";
																							}
																							else
																							{
																								bool flag21 = base.CurrLogic == LogicEnum.StandbyToBTemporary;
																								if (flag21)
																								{
																									result = "原点到B暂存区";
																								}
																								else
																								{
																									bool flag22 = base.CurrLogic == LogicEnum.BTemporaryToCableFeeding;
																									if (flag22)
																									{
																										result = "B暂存区到锁膜上料点";
																									}
																									else
																									{
																										bool flag23 = base.CurrLogic == LogicEnum.CableFeedingToBTemporary;
																										if (flag23)
																										{
																											result = "锁膜上料点到B暂存区";
																										}
																										else
																										{
																											bool flag24 = base.CurrLogic == LogicEnum.CableFeedingToStandby;
																											if (flag24)
																											{
																												result = "锁膜上料点到原点";
																											}
																											else
																											{
																												bool flag25 = base.CurrLogic == LogicEnum.StandbyToCableReclaiming;
																												if (flag25)
																												{
																													result = "原点到锁膜取空盒";
																												}
																												else
																												{
																													bool flag26 = base.CurrLogic == LogicEnum.CableReclaimingToBEmptyTemporary;
																													if (flag26)
																													{
																														result = "锁膜下料点到B空盒暂存区";
																													}
																													else
																													{
																														bool flag27 = base.CurrLogic == LogicEnum.BEmptyTemporaryToCableReclaiming;
																														if (flag27)
																														{
																															result = "B空盒暂存区到锁膜下料点";
																														}
																														else
																														{
																															bool flag28 = base.CurrLogic == LogicEnum.BEmptyTemporaryToStandby;
																															if (flag28)
																															{
																																result = "B空盒暂存区到原点";
																															}
																															else
																															{
																																result = "";
																															}
																														}
																													}
																												}
																											}
																										}
																									}
																								}
																							}
																						}
																					}
																				}
																			}
																		}
																	}
																}
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
				return result;
			}
		}

		public string CurrLogicSysStr
		{
			get
			{
				bool flag = base.CurrLogicSys == LogicEnum.StandbyToStock;
				string result;
				if (flag)
				{
					result = "待命点到备料点";
				}
				else
				{
					bool flag2 = base.CurrLogicSys == LogicEnum.StockToFeeding;
					if (flag2)
					{
						result = "备料点到上料";
					}
					else
					{
						bool flag3 = base.CurrLogicSys == LogicEnum.FeedingToStock;
						if (flag3)
						{
							result = "上料到备料";
						}
						else
						{
							bool flag4 = base.CurrLogicSys == LogicEnum.FeedingToStandby;
							if (flag4)
							{
								result = "上料到原点";
							}
							else
							{
								bool flag5 = base.CurrLogicSys == LogicEnum.StandbyToReclaiming;
								if (flag5)
								{
									result = "原点到下料点";
								}
								else
								{
									bool flag6 = base.CurrLogicSys == LogicEnum.ReclaimingToATemporary;
									if (flag6)
									{
										result = "下料点到暂存区";
									}
									else
									{
										bool flag7 = base.CurrLogicSys == LogicEnum.ATemporaryToReclaiming;
										if (flag7)
										{
											result = "暂存区到下料点";
										}
										else
										{
											bool flag8 = base.CurrLogicSys == LogicEnum.ATemporaryToStandby;
											if (flag8)
											{
												result = "暂存区到原点";
											}
											else
											{
												bool flag9 = base.CurrLogicSys == LogicEnum.StandbyToATemporary;
												if (flag9)
												{
													result = "原点到A暂存区";
												}
												else
												{
													bool flag10 = base.CurrLogicSys == LogicEnum.ATemporaryToHeadFeeding;
													if (flag10)
													{
														result = "暂存区到线头上料";
													}
													else
													{
														bool flag11 = base.CurrLogicSys == LogicEnum.HeadFeedingToHeadReclaiming;
														if (flag11)
														{
															result = "线头上料到线头取空盒";
														}
														else
														{
															bool flag12 = base.CurrLogicSys == LogicEnum.HeadReclaimingToAEmptyTemporary;
															if (flag12)
															{
																result = "线头取空盒到A空盒区";
															}
															else
															{
																bool flag13 = base.CurrLogicSys == LogicEnum.AEmptyTemporaryToATemporary;
																if (flag13)
																{
																	result = "A空盒区到A暂存区";
																}
																else
																{
																	bool flag14 = base.CurrLogicSys == LogicEnum.AEmptyTemporaryToStandby;
																	if (flag14)
																	{
																		result = "A空盒区到原点";
																	}
																	else
																	{
																		bool flag15 = base.CurrLogicSys == LogicEnum.StandbyToBEmptyTemporary;
																		if (flag15)
																		{
																			result = "原点到B空盒";
																		}
																		else
																		{
																			bool flag16 = base.CurrLogicSys == LogicEnum.BEmptyTemporaryToEndFeeding;
																			if (flag16)
																			{
																				result = "B区空盒去到线尾上空盒";
																			}
																			else
																			{
																				bool flag17 = base.CurrLogicSys == LogicEnum.EndFeedingToEndReclaiming;
																				if (flag17)
																				{
																					result = "线尾上空盒到线尾取料";
																				}
																				else
																				{
																					bool flag18 = base.CurrLogicSys == LogicEnum.EndReclaimingToBTemporary;
																					if (flag18)
																					{
																						result = "线尾取料到B暂存区";
																					}
																					else
																					{
																						bool flag19 = base.CurrLogicSys == LogicEnum.BTemporaryToBEmptyTemporary;
																						if (flag19)
																						{
																							result = "B暂存区到B空盒区";
																						}
																						else
																						{
																							bool flag20 = base.CurrLogicSys == LogicEnum.BTemporaryToStandy;
																							if (flag20)
																							{
																								result = "B暂存区到待命点";
																							}
																							else
																							{
																								bool flag21 = base.CurrLogic == LogicEnum.StandbyToBTemporary;
																								if (flag21)
																								{
																									result = "原点到B暂存区";
																								}
																								else
																								{
																									bool flag22 = base.CurrLogic == LogicEnum.BTemporaryToCableFeeding;
																									if (flag22)
																									{
																										result = "B暂存区到锁膜上料点";
																									}
																									else
																									{
																										bool flag23 = base.CurrLogic == LogicEnum.CableFeedingToBTemporary;
																										if (flag23)
																										{
																											result = "锁膜上料点到B暂存区";
																										}
																										else
																										{
																											bool flag24 = base.CurrLogic == LogicEnum.CableFeedingToStandby;
																											if (flag24)
																											{
																												result = "锁膜上料点到原点";
																											}
																											else
																											{
																												bool flag25 = base.CurrLogic == LogicEnum.StandbyToCableReclaiming;
																												if (flag25)
																												{
																													result = "原点到锁膜取空盒";
																												}
																												else
																												{
																													bool flag26 = base.CurrLogic == LogicEnum.CableReclaimingToBEmptyTemporary;
																													if (flag26)
																													{
																														result = "锁膜下料点到B空盒暂存区";
																													}
																													else
																													{
																														bool flag27 = base.CurrLogic == LogicEnum.BEmptyTemporaryToCableReclaiming;
																														if (flag27)
																														{
																															result = "B空盒暂存区到锁膜下料点";
																														}
																														else
																														{
																															bool flag28 = base.CurrLogic == LogicEnum.BEmptyTemporaryToStandby;
																															if (flag28)
																															{
																																result = "B空盒暂存区到原点";
																															}
																															else
																															{
																																result = "";
																															}
																														}
																													}
																												}
																											}
																										}
																									}
																								}
																							}
																						}
																					}
																				}
																			}
																		}
																	}
																}
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
				return result;
			}
		}

		public string ExcuteTaksNo
		{
			get;
			set;
		}

		public int TrafficLockType
		{
			get;
			set;
		}

		public CarInfo()
		{
			this.CurrentLandMark = new LandmarkInfo();
			this.NextLandMark = new LandmarkInfo();
			this.NextNextLandMark = new LandmarkInfo();
			this.LockLandMarkList = new List<string>();
			this.StandbyLandMark = "";
			this.CarName = "";
			this.DispatchTask = new DispatchTaskInfo();
			this.PreDispatchTask = new DispatchTaskInfo();
			base.Route = new List<LandmarkInfo>();
			this.ExcuteTaksNo = "";
			this.TaskDetailID = -1;
		}

		public override string ToString()
		{
			return string.Concat(new string[]
			{
				base.AgvID.ToString(),
				"号AGV  当前线路:",
				base.CurrRoute.ToString(),
				" 当前地标：",
				base.CurrSite.ToString(),
				" 下个地标：",
				this.NextLandCode,
				" 下下个地标：",
				this.NextNextLandMarkCode,
				" 锁住地标：（",
				this.LockLandMarks,
				"）当前业务：[",
				base.CurrLogic.ToString(),
				"],当前任务号：",
				this.DispatchTaskNo,
				" 前置任务号：",
				this.PreDispatchTaskNo
			});
		}
	}
}
