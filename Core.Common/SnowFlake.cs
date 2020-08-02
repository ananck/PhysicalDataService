using System;
using System.Collections.Generic;
using System.Text;

namespace SnowFlakeID
{
    /// <summary>
    /// 分布式自增ID雪花算法
    /// </summary>
    public class SnowFlake
    {
        /// <summary>
        /// 起始时间戳 1900-01-01
        /// </summary>
        private static long START_STMP = 1480166465631L;

        #region 返回ID每一部分占用的位数
        private static int MACHINE_BIT = 5;   //机器标识占用的位数
        private static int DATACENTER_BIT = 5;//数据中心占用的位数
        private static int SEQUENCE_BIT = 12; //序列号占用的位数
        #endregion

        #region 返回ID每部分最大的值
        private static long MAX_MACHINE_NUM = -1L ^ (-1L << MACHINE_BIT); //左位移MACHINE_BIT位 31
        private static long MAX_DATACENTER_NUM = -1L ^ (-1L << DATACENTER_BIT); //左位移DATACENTER_BIT位 31
        private static long MAX_SEQUENCE = -1L ^ (-1L << SEQUENCE_BIT); //左位移SEQUENCE_BIT位 4057
        #endregion

        #region 每部分向左位移
        private static int MACHINE_LEFT = MACHINE_BIT;
        private static int DATACENTER_LEFT = MACHINE_LEFT + SEQUENCE_BIT;
        private static int TIMESTMP_LEFT = DATACENTER_LEFT + DATACENTER_BIT;
        #endregion

        #region 基础数据
        /// <summary>
        /// //数据中心
        /// </summary>
        private long datacenterId;

        /// <summary>
        /// 机器标识
        /// </summary>
        private long machineId;

        /// <summary>
        /// 序列号
        /// </summary>
        private long sequence = 0L;

        /// <summary>
        /// 上一次时间戳
        /// </summary>
        private long lastStmp = -1L;
        #endregion

        public SnowFlake(long datacenterId, long machineId)
        {
            if (datacenterId > MAX_DATACENTER_NUM || datacenterId < 0)
            {
                throw new ArgumentException("datacenterId大于MAX_DATACENTER_NUM或者小于0。");
            }
            if (machineId > MAX_MACHINE_NUM || machineId < 0)
            {
                throw new ArgumentException("machineId大于MAX_MACHINE_NUM或者小于0。");
            }

            this.datacenterId = datacenterId;
            this.machineId = machineId;
        }

        /// <summary>
        /// 生产下一个ID
        /// </summary>
        /// <returns></returns>
        public long nextId()
        {
            var currTimeStamp = timeGen();
            if (currTimeStamp < lastStmp)
            {
                throw new InvalidTimeZoneException("时钟回滚，拒绝生成id。");
            }

            if (currTimeStamp == lastStmp)
            {
                //相同毫秒内，序列号自增
                sequence = (sequence + 1) & MAX_SEQUENCE;
                //同一毫秒的序列数已经达到最大
                if (sequence == 0L)
                {
                    //重新获取当前时间戳
                    currTimeStamp = timeGen();
                }
            }
            else
            {
                //不同毫秒内，序列号置为0
                sequence = 0L;
            }

            lastStmp = currTimeStamp;

            return (currTimeStamp - START_STMP) << TIMESTMP_LEFT 
                    | datacenterId << DATACENTER_LEFT 
                    | machineId << MACHINE_LEFT 
                    | sequence;
        }

        /// <summary>
        /// 获取当前时间戳
        /// </summary>
        /// <returns></returns>
        private long timeGen()
        { 
            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }
    }
}
