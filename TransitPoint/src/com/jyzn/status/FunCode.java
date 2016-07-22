package com.jyzn.status;

public enum FunCode {
	POLLING_STATUS						(0x10),//查询状态（上-->下）
	CHARGE_ARRANGEMENT					(0x20),//安排充电（上-->下）
	MOVING_AND_WAIT						(0x21),//移动到位置并等待（上-->下）
	FOUND_SHELVES						(0x22),//找货架（上-->下）
	CARRY_SHELVES_TO_PICKING_TABLE		(0x23),//送货架到捡货台（上-->下）
	CARRY_SHELVES_BACKTO_STORAGE_AREA	(0x24),//送回货架到仓储区（上-->下）
	CURRENT_STATUS						(0x30),//当前状态（下-->上）
	LOW_POWER							(0x31),//电量低（下-->上）
	MEET_OBSTACLE						(0x32),//遇到障碍（下-->上）
	OVERLOAD							(0x33),//超重（下-->上）
	SHELVES_UNSTABLE					(0x34),//货物不稳（下-->上）
	UNKNOW_EXCEPTION  					(0x39);//未知异常（下-->上）
	
	protected int code;
	
	private FunCode(int code) {
		this.code = code;
	}
	
	public int getCode() {
		return code;
	}
	
	public static FunCode getFunCode(int code) {
		FunCode funCode = null;
		switch (code) {
		case 0x10:
			funCode = POLLING_STATUS;
			break;
		case 0x20:
			funCode = CHARGE_ARRANGEMENT;
			break;
		case 0x21:
			funCode = MOVING_AND_WAIT;
			break;
		case 0x22:
			funCode = FOUND_SHELVES;
			break;
		case 0x23:
			funCode = CARRY_SHELVES_TO_PICKING_TABLE;
			break;
		case 0x24:
			funCode = CARRY_SHELVES_BACKTO_STORAGE_AREA;
			break;
		case 0x30:
			funCode = CURRENT_STATUS;
			break;
		case 0x31:
			funCode = FunCode.LOW_POWER;
			break;
		case 0x32:
			funCode = MEET_OBSTACLE;
			break;
		case 0x33:
			funCode = OVERLOAD;
			break;
		case 0x34:
			funCode = SHELVES_UNSTABLE;
			break;
		case 0x39:
			funCode = UNKNOW_EXCEPTION;
			break;
		}
		
		return funCode;
	}
}
