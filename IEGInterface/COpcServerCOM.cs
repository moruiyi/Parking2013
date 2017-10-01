using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Opc;
using Opc.Da;
using OpcCom;

namespace IEGInterface
{
    public class COpcServerCOM
    {
        private Opc.IDiscovery m_Discovery; //发送定义枚举基于COM服务器的接口，用来搜索所有的此类服务器
        private Opc.Da.Server m_Server = null;  //定义数据存取服务器
        private Opc.Da.SubscriptionState subsState = null; //定义组状态，相当于OPC规范中组的参数
        private Opc.Da.Subscription subscription = null; //定义组对象

        public COpcServerCOM() 
        {
            m_Discovery = new OpcCom.ServerEnumerator();
        }
        /// <summary>
        /// OPC建立连接
        /// </summary>
        public void mOPC_ConnToServer() 
        {
            try 
            {
                string hostName = IEGInterface.Properties.Settings.Default.MachineName;
                //查询服务器
                m_Server=null;
                Opc.Server[] servers = m_Discovery.GetAvailableServers(Specification.COM_DA_20, hostName, null);
                if (servers != null) 
                {
                    foreach (Opc.Da.Server sr in servers) 
                    {
                        //获取需要连接的OPC数据存取服务器
                        if (String.Compare(sr.Name, hostName + ".OPC.SimaticNET", true) == 0) 
                        {
                            m_Server = sr;
                            break;
                        }
                    }
                }

                if (m_Server != null) 
                {
                    m_Server.Connect();
                }
            }
            catch(Exception ex)
            {
                throw new Exception("OPC连接函数（mOPC_ConnToServer）异常：" + ex.ToString());
            }
        }
        /// <summary>
        /// OPC断开连接
        /// </summary>
        public void mOpc_DisConn()
        {
            try
            {
                if (m_Server != null) 
                {
                    subscription.RemoveItems(subscription.Items);
                    //结束：释放各资源
                    m_Server.CancelSubscription(subscription); //m_server前文已说明，通知服务器要求删除组。 
                    subscription.Dispose();   //强制.NET资源回收站回收该subscription的所有资源。
                    m_Server.Disconnect();
                }
            }
            catch (Exception ex) 
            {
                throw new Exception("OPC断开函数（mOpc_DisConn）异常：" + ex.ToString());
            }
        }
        /// <summary>
        /// 属性，OPC是否连接
        /// </summary>
        public bool CheckOpcConn 
        {
            get 
            {
                return m_Server.IsConnected;
            }
        }
        /// <summary>
        /// 建立订阅
        /// </summary>
        public void CreateSubscription() 
        {
            try 
            {
                subsState = new SubscriptionState();
                subsState.Name = "OPCNJ";//组名
                subsState.ServerHandle = null;//服务器给该组分配的句柄。
                subsState.ClientHandle = Guid.NewGuid().ToString();//客户端给该组分配的句柄
                subsState.Active = true;
                subsState.UpdateRate = 100;//刷新频率为1秒。
                subsState.Deadband = 0;// 死区值，设为0时，服务器端该组内任何数据变化都通知组。
                subsState.Locale = null;//不设置地区值。

                subscription = (Opc.Da.Subscription)m_Server.CreateSubscription(subsState);
                Item[] items = new Item[10];
                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = new Item();
                    items[i].ClientHandle = Guid.NewGuid().ToString();  //客户端给该数据项分配的句柄。
                    items[i].ItemPath = null;//该数据项在服务器中的路径
                }
                items[0].ItemName = IEGInterface.Properties.Settings.Default.strSendBuffer;
                items[1].ItemName = IEGInterface.Properties.Settings.Default.strSendFlag;
                items[2].ItemName = IEGInterface.Properties.Settings.Default.strReceiveBuffer;
                items[3].ItemName = IEGInterface.Properties.Settings.Default.strReceiveFlag;
                items[4].ItemName = IEGInterface.Properties.Settings.Default.strEtv1Alarms;
                items[5].ItemName = IEGInterface.Properties.Settings.Default.strEtv2Alarms;
                items[6].ItemName = IEGInterface.Properties.Settings.Default.strHall1Alarms;
                items[7].ItemName = IEGInterface.Properties.Settings.Default.strHall2Alarms;
                items[8].ItemName = IEGInterface.Properties.Settings.Default.strHall3Alarms;
                items[9].ItemName = IEGInterface.Properties.Settings.Default.strHall4Alarms;

                subscription.AddItems(items);
            }
            catch(Exception ex)
            {
                throw new Exception("OPC建立订阅(CreateSubscription)异常：" + ex.ToString());
            }
        }
        /// <summary>
        /// 读取一个字型数据
        /// </summary>
        /// <param name="data">字型数据</param>
        /// <param name="itemNum">需读取的项号</param>
        /// <returns></returns>
        public bool Read(ref Int16 data,int itemNum)
        {
            try 
            {
                Item[] m_items = new Item[1];
                m_items[0]=subscription.Items[itemNum];
                ItemValueResult[] values = subscription.Read(m_items);
                if (values[0].Quality == Quality.Good)
                {
                    data = (Int16)values[0].Value;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex) 
            {
                throw new Exception("opc函数Read异常："+ex.Message);
            }
        }

        /// <summary>
        /// 读取Int16字型数组数据
        /// </summary>
        /// <param name="data">数组数据</param>
        /// <param name="itemNum">项</param>
        /// <returns></returns>
        public bool Readnew(ref Int16[] data, int itemNum) 
        {
            try 
            {
                Item[] m_items = new Item[1];
                m_items[0] = subscription.Items[itemNum];
                ItemValueResult[] values = subscription.Read(m_items);
                if (values[0].Quality == Quality.Good)
                {
                    data = (Int16[])values[0].Value;
                    return true;
                }
                else 
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                throw new Exception("opc函数Readnew异常："+ex.ToString());
            }
        }
        /// <summary>
        /// 读取byte字节型数组数据
        /// </summary>
        /// <param name="data">字节型数组数据</param>
        /// <param name="itemNum">项</param>
        /// <returns></returns>
        public bool ReadnewBytes(ref byte[] data, int itemNum) 
        {
            try 
            {
                if (CheckOpcConn)
                {
                    Item[] m_items = new Item[1];
                    m_items[0] = subscription.Items[itemNum];
                    ItemValueResult[] values = subscription.Read(m_items);
                    if (values[0].Quality == Quality.Good)
                    {
                        data = (byte[])values[0].Value;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else 
                {
                    return false;
                }
            }
            catch (Exception ex) 
            {
                throw new Exception("opc函数ReadnewBytes异常：" + ex.ToString());
            }
        }
        /// <summary>
        /// 写入一个int16字型数据
        /// </summary>
        /// <param name="data">待写入的字数据</param>
        /// <param name="itemNum">待写入项</param>
        /// <returns></returns>
        public bool Write(Int16 data,int itemNum) 
        {
            try 
            {
                ItemValue[] values = new ItemValue[1];
                values[0] = new ItemValue((Opc.ItemIdentifier)subscription.Items[itemNum]);
                values[0].Value = data;
                Opc.IdentifiedResult[] idr = subscription.Write(values);
                if (idr != null && idr.Length > 0)
                {
                    return true;
                }
                else 
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                throw new Exception("opc函数Write异常："+ex.ToString());
            }
        }
        /// <summary>
        /// 写入INT16型字数组数据
        /// </summary>
        /// <param name="data">字型数组数据</param>
        /// <param name="itemNum">项</param>
        /// <returns></returns>
        public bool Writenew(Int16[] data,int itemNum)
        {
            try 
            {
                ItemValue[] values=new ItemValue[1];
                values[0] = new ItemValue((Opc.ItemIdentifier)subscription.Items[itemNum]);
                values[0].Value = data;
                Opc.IdentifiedResult[] idr = subscription.Write(values);
                if (idr != null && idr.Length > 0)
                {
                    return true;
                }
                else 
                {
                    return false;
                }
            }
            catch (Exception ex) 
            {
                throw new Exception("opc函数Writenew异常：" + ex.ToString());
            }
        }
        /// <summary>
        /// 写入byte字节型数组数据
        /// </summary>
        /// <param name="data">字节型数组数据</param>
        /// <param name="itemNum">项</param>
        /// <returns></returns>
        public bool WritenewBytes(byte[] data, int itemNum)
        {
            try
            {
                if (CheckOpcConn)
                {
                    ItemValue[] values = new ItemValue[1];
                    values[0] = new ItemValue((Opc.ItemIdentifier)subscription.Items[itemNum]);
                    values[0].Value = data;
                    Opc.IdentifiedResult[] idr = subscription.Write(values);
                    if (idr != null && idr.Length > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else 
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("opc函数WritenewBytes异常：" + ex.ToString());
            }
        }
    }
}
