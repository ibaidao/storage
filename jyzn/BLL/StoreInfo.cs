using System;
using System.Collections.Generic;
using System.Linq;
using Models;

namespace BLL
{
    /// <summary>
    /// 数据库中仓库数据的交互
    /// </summary>
    public class StoreInfo
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="loc"></param>
        /// <param name="dataID"></param>
        /// <returns></returns>
        public ErrorCode AddPoint(string name, Location loc, out int dataID)
        {
            dataID = -1;
            ErrorCode result = ErrorCode.OK;
            string strWhere = string.Format(" Point='{0}' ", loc.ToString());
            StorePoints pointCheck = DbEntity.DStorePoints.GetSingleEntity(strWhere, null);
            if (pointCheck != null)
            {
                result = ErrorCode.AddDuplicateItem;
                return result;
            }

            object itemID = DbEntity.DStorePoints.Insert(new StorePoints()
            {
                Name = name,
                Point = loc.ToString(),
                StoreID = Models.GlobalVariable.STORE_ID,
                Status = 0,
                Type = (short)StoreComponentType.CrossCorner
            });
            dataID = Convert.ToInt32(itemID);
            if (dataID < 0)
            {
                result = ErrorCode.DatabaseHandler;
            }

            return result;
        }

        /// <summary>
        /// 新增仓库充电桩/拣货台
        /// </summary>
        /// <param name="type">拣货台/补货台/充电桩</param>
        /// <param name="code">节点编码</param>
        /// <param name="data">节点数据</param>
        /// <param name="locXYZ">节点坐标</param>
        public void AddChargerPickStation(StoreComponentType type, string code, int data, Location locXYZ)
        {
            //Station站点信息更新
            string strWhere = string.Format(" LocationID = {0} ", data);
            Station item = DbEntity.DStation.GetSingleEntity(strWhere, null);
            if (item == null)
            {
                DbEntity.DStation.Insert(new Station()
                {
                    Code = code,
                    LocationID = data,
                    Status = 1,
                    Type = (short)type,
                    Location = locXYZ.ToString (),//graph.GetHeadNodeByData(locIdx).Location.ToString(),
                    Remarks = ""
                });
            }
            else
            {
                item.Remarks = string.Format("修改类型：原类型{0}，改为{1}", item.Type, (short)type);
                item.Type = (short)type;
                DbEntity.DStation.Update(item);
            }
            //节点数据更新
            StorePoints point = DbEntity.DStorePoints.GetSingleEntity(data);
            point.Type = (short)type;
            DbEntity.DStorePoints.Update(point);
        }

        /// <summary>
        /// 增加路径
        /// </summary>
        /// <param name="one">一端数据</param>
        /// <param name="two">另一端</param>
        /// <param name="storeType">类型(单向/双向)</param>
        /// <param name="weight"></param>
        public ErrorCode AddPath(int one, int two, StoreComponentType storeType, int weight)
        {
            ErrorCode result = ErrorCode.OK;

            object itemID = DbEntity.DStorePaths.Insert(new StorePaths()
            {
                Weight = (short)weight,
                Status = (short)StoreComponentStatus.OK,
                StoreID = Models.GlobalVariable.STORE_ID,
                OnePoint = one,
                TwoPoint = two,
                Type = (short)storeType
            });

            if (Convert.ToInt32(itemID) < 0)
            {
                result = ErrorCode.DatabaseHandler;
            }

            return result;
        }

        public void UpdateIniFile(string section,string key, string value)
        {
            Utilities.IniFile.WriteIniData(section, key, value);
        }
    }
}
