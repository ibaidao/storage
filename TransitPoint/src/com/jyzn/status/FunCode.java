package com.jyzn.status;

public enum FunCode {
	POLLING_STATUS						(0x10),//��ѯ״̬����-->�£�
	CHARGE_ARRANGEMENT					(0x20),//���ų�磨��-->�£�
	MOVING_AND_WAIT						(0x21),//�ƶ���λ�ò��ȴ�����-->�£�
	FOUND_SHELVES						(0x22),//�һ��ܣ���-->�£�
	CARRY_SHELVES_TO_PICKING_TABLE		(0x23),//�ͻ��ܵ����̨����-->�£�
	CARRY_SHELVES_BACKTO_STORAGE_AREA	(0x24),//�ͻػ��ܵ��ִ�������-->�£�
	CURRENT_STATUS						(0x30),//��ǰ״̬����-->�ϣ�
	LOW_POWER							(0x31),//�����ͣ���-->�ϣ�
	MEET_OBSTACLE						(0x32),//�����ϰ�����-->�ϣ�
	OVERLOAD							(0x33),//���أ���-->�ϣ�
	SHELVES_UNSTABLE					(0x34),//���ﲻ�ȣ���-->�ϣ�
	UNKNOW_EXCEPTION  					(0x39);//δ֪�쳣����-->�ϣ�
	
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
