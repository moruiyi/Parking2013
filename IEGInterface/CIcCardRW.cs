using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace IEGInterface
{
    interface CICCardRW
    {
        bool ConnectCOM();

        int disConnectCOM();

        int RequestICCard(ref short nCardType);

        int LoadKeyToEquip(int mode, int SectorNum, byte[] csKeyCode);

        int SelectCard(ref uint nCardSequenceNum);

        int ReadCard(Int16 p, Int16 p_2, ref byte[] Icdata);
    }

    class CIcCardRWOne:CICCardRW
    {
        #region  读写器操作动态库


        //1.
        //HANDLE __stdcall  rf_init(__int16 port,long baud);
        /// <summary>
        /// 初始化读写器
        /// </summary>
        /// <param name="commport">通讯端口号</param>
        /// <param name="baud">通信波特率</param>
        /// <returns>返回值</returns>
        [DllImport("mwrf32.dll", EntryPoint = "rf_init", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        protected static extern int rf_init(int commport, int baud);
        ////2.
        //__int16 __stdcall rf_exit(HANDLE icdev);
        /// <summary>
        /// 关闭串口，并保存PC机上的设置
        /// </summary>
        /// <param name="icdev">rf_init返回设备描述</param>
        /// <returns>返回值</returns>
        [DllImport("mwrf32.dll", EntryPoint = "rf_exit", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int rf_exit(int icdev);
        ////3.
        //__int16 __stdcall rf_config(HANDLE icdev,unsigned char _Mode,unsigned char _Baud);
        ////4.
        //__int16 __stdcall rf_request(HANDLE icdev,unsigned char _Mode,unsigned __int16 *TagType);
        /// <summary>
        /// 该函数向卡片发出寻卡命令，开始选择一张新卡片时需要执行该函数。
        /// </summary>
        /// <param name="icdev"></param>
        /// <param name="Mode">
        /// 寻卡模式:0 IDLE mode, 只有处在IDLE 状态的卡片才响应读写器的命令。1 ALL mode, 处在 IDLE 状态和HALT 状态的卡片都将响应读写器的命令。
        /// </param>
        /// <param name="tagtype">返回卡片类型</param>
        /// <returns>返回值</returns>
        [DllImport("mwrf32.dll", EntryPoint = "rf_request", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int rf_request(int icdev, int Mode, out Int16 tagtype);
        ////5.
        //__int16 __stdcall rf_request_std(HANDLE icdev,unsigned char _Mode,unsigned __int16 *TagType);

        //__int16 __stdcall rf_anticoll(HANDLE icdev,unsigned char _Bcnt,unsigned long *_Snr);
        /// <summary>
        /// 激活读写器的防冲突队列。如果有几张MIFARE 卡片在感应区内，将会选择一张卡片，并返回卡片的序列号供将来调用rf_select 函数时使用。
        /// </summary>
        /// <param name="icdev">rf_init()返回的设备描述符</param>
        /// <param name="_Bcnt">预选卡片使用的位, 标准调用时为bcnt=0.</param>
        /// <param name="_Snr">返回的卡片序列号</param>
        /// <returns>返回值</returns>
        [DllImport("mwrf32.dll", EntryPoint = "rf_anticoll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int rf_anticoll(int icdev, int _Bcnt, out uint _Snr);
        ////6.
        //__int16 __stdcall rf_select(HANDLE icdev,unsigned long _Snr,unsigned char *_Size);
        /// <summary>
        /// 用指定的序列号选择卡片，将卡片的容量返回给PC 机。
        /// </summary>
        /// <param name="icdev">返回的设备描述符</param>
        /// <param name="Snr">卡片的序列号</param>
        /// <param name="Size">卡片容量的地址指针，目前该值不能使用</param>
        /// <returns>返回值</returns>
        [DllImport("mwrf32.dll", EntryPoint = "rf_select", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int rf_select(int icdev, uint Snr, out byte Size);

        /// <summary>
        ///7.验证读写器中的密码与需要访问的卡片的同一扇区(0~15)的密码是否一致。如果读写器中选择的密码（可用rf_load_key 函数修改）与卡片的相匹配，密码验证通过，传输的数据将用以下的命令加密
        /// </summary>
        /// <param name="icdev">rf_init()返回的设备描述符</param>
        /// <param name="_Mode">验证密码类型：0 — 用KEY A 验证 4 — 用 KEY B 验证</param>
        /// <param name="_SecNr">将要访问的卡片扇区号(0~15)</param>
        /// <returns></returns>
        [DllImport("mwrf32.dll", EntryPoint = "rf_authentication", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int rf_authentication(int icdev, int _Mode, int _SecNr);
        ////
        //__int16 __stdcall rf_halt(HANDLE icdev);
        /// <summary>
        ///8.将一张选中的卡片设为“Halt”模式，只有当该卡再次复位或用ALL 模式调用request 函数时，读写器才能够再次操作它
        /// </summary>
        /// <param name="icdev">rf_init()返回的设备描述符</param>
        /// <returns></returns>
        [DllImport("mwrf32.dll", EntryPoint = "rf_halt", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int rf_halt(int icdev);
        ////9
        //__int16 __stdcall rf_read(HANDLE icdev,unsigned char _Adr,unsigned char *_Data);
        /// <summary>
        /// 从一张选定并通过密码验证的卡片读取一块共16个字节的数据。
        /// </summary>
        /// <param name="icdev">rf_init()返回的设备描述符</param>
        /// <param name="_Adr">读取数据的块号(0~63)</param>
        /// <param name="_Data">Data:读取的数据，PC 机上RAM 的地址空间由调用该函数来分配。</param>
        /// <returns></returns>
        [DllImport("mwrf32.dll", EntryPoint = "rf_read", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int rf_read(int icdev, int _Adr, [MarshalAs(UnmanagedType.LPArray)]byte[] _Data);
        ////10.
        //__int16 __stdcall rf_read_hex(HANDLE icdev,unsigned char _Adr, char *_Data);
        ////11.
        //__int16 __stdcall rf_write(HANDLE icdev,unsigned char _Adr,unsigned char *_Data);
        /// <summary>
        /// 11.将一块共16字节写入选定并验证通过的卡片中。
        /// </summary>
        /// <param name="icdev">rf_init()返回的设备描述符</param>
        /// <param name="_Adr">写入数据的块地址 (1~63)</param>
        /// <param name="_Data">写入数据,长度为16字节</param>
        /// <returns></returns>
        [DllImport("mwrf32.dll", EntryPoint = "rf_write", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int rf_write(int icdev, int _Adr, [MarshalAs(UnmanagedType.LPArray)]byte[] _Data);
        ////12.
        //__int16 __stdcall rf_write_hex(HANDLE icdev,unsigned char _Adr,char *_Data);
        ////13.
        //__int16 __stdcall rf_load_key(HANDLE icdev,unsigned char _Mode,unsigned char _SecNr,unsigned char *_NKey);
        [DllImport("mwrf32.dll", EntryPoint = "rf_load_key", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int rf_load_key(int icdev, int _Mode, int _SecNr, byte[] _NKey);
        ////14.
        //__int16 __stdcall rf_load_key_hex(HANDLE icdev,unsigned char _Mode,unsigned char _SecNr, char *_NKey);
        [DllImport("mwrf32.dll", EntryPoint = "rf_load_key_hex", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int rf_load_key_hex(int icdev, string _Mode, string _SecNr, ref string _NKey);
        ////15.
        //__int16 __stdcall rf_increment(HANDLE icdev,unsigned char _Adr,unsigned long _Value);
        ////16.
        //__int16 __stdcall rf_decrement(HANDLE icdev,unsigned char _Adr,unsigned long _Value);
        ////17
        //__int16 __stdcall rf_decrement_ml(HANDLE icdev,unsigned __int16 _Value);
        ////18.
        //__int16 __stdcall rf_restore(HANDLE icdev,unsigned char _Adr);
        ////19
        //__int16 __stdcall rf_transfer(HANDLE icdev,unsigned char _Adr);
        ////20.
        //__int16 __stdcall rf_card(HANDLE icdev,unsigned char _Mode,unsigned long *_Snr);
        ////21.
        //__int16 __stdcall rf_initval(HANDLE icdev,unsigned char _Adr,unsigned long _Value);
        ////22
        //__int16 __stdcall rf_initval_ml(HANDLE icdev,unsigned __int16 _Value);
        ////23.
        //__int16 __stdcall rf_readval(HANDLE icdev,unsigned char _Adr,unsigned long *_Value);
        ////24
        //__int16 __stdcall rf_readval_ml(HANDLE icdev,unsigned __int16 *_Value);
        ////25.
        //__int16 __stdcall rf_changeb3(HANDLE icdev,unsigned char _SecNr,unsigned char *_KeyA,unsigned char _B0,unsigned char _B1,unsigned char _B2,unsigned char _B3,unsigned char _Bk,unsigned char *_KeyB);
        ////26.
        //__int16 __stdcall rf_get_status(HANDLE icdev,unsigned char *_Status);
        ////27.
        //__int16 __stdcall rf_clr_control_bit(HANDLE icdev,unsigned char _b);
        ////28.
        //__int16 __stdcall rf_set_control_bit(HANDLE icdev,unsigned char _b);
        ////29.
        //__int16 __stdcall rf_reset(HANDLE icdev,unsigned __int16 _Msec);
        /// <summary>
        /// 将RF（射频）模块的能量释放几毫秒
        /// </summary>
        /// <param name="icdev">rf_init()返回的设备描述符</param>
        /// <param name="_Msec">复位时间 ( 0~ 500ms)</param>
        /// <returns></returns>
        [DllImport("mwrf32.dll", EntryPoint = "rf_reset", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int rf_reset(int icdev, int _Msec);
        ////30.
        //__int16 __stdcall rf_HL_decrement(HANDLE icdev,unsigned char _Mode,unsigned char _SecNr,unsigned long _Value,unsigned long _Snr,unsigned long *_NValue,unsigned long *_NSnr);
        ////31.
        //__int16 __stdcall rf_HL_increment(HANDLE icdev,unsigned char _Mode,unsigned char _SecNr,unsigned long _Value,unsigned long _Snr,unsigned long *_NValue,unsigned long *_NSnr);
        ////32.
        //__int16 __stdcall rf_HL_write(HANDLE icdev,unsigned char _Mode,unsigned char _Adr,unsigned long *_Snr,unsigned char *_Data);
        ////33.
        //__int16 __stdcall rf_HL_writehex(HANDLE icdev,unsigned char _Mode,unsigned char _Adr,unsigned long *_Snr, char *_Data);
        ////34
        //__int16 __stdcall rf_HL_read(HANDLE icdev,unsigned char _Mode,unsigned char _Adr,unsigned long _Snr,unsigned char *_Data,unsigned long *_NSnr);
        ////35
        //__int16 __stdcall rf_HL_readhex(HANDLE icdev,unsigned char _Mode,unsigned char _Adr,unsigned long _Snr, char *_Data,unsigned long *_NSnr);
        ////36.
        //__int16 __stdcall rf_HL_initval(HANDLE icdev,unsigned char _Mode,unsigned char _SecNr,unsigned long _Value,unsigned long *_Snr);

        /// <summary>
        ///37.蜂鸣几毫秒。
        /// </summary>
        /// <param name="icdev">rf_init()返回的设备描述符</param>
        /// <param name="_Msec">蜂鸣时间，单位：毫秒</param>
        /// <returns></returns>
        [DllImport("mwrf32.dll", EntryPoint = "rf_beep", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int rf_beep(int icdev, int _Msec);
        ////38.
        //__int16 __stdcall rf_disp8(HANDLE icdev,__int16 pt_mode,unsigned char* disp_str);
        ////39.
        //__int16 __stdcall rf_disp(HANDLE icdev,unsigned char pt_mode,unsigned short digit);
        ////40.
        //__int16 __stdcall rf_encrypt(unsigned char *key,unsigned char *ptrSource, unsigned __int16 msgLen,unsigned char *ptrDest);
        ////41.
        //__int16 __stdcall rf_decrypt(unsigned char *key,unsigned char *ptrSource, unsigned __int16 msgLen,unsigned char *ptrDest);
        ////42
        //__int16 __stdcall rf_HL_authentication(HANDLE icdev,unsigned char reqmode,unsigned long snr,unsigned char authmode,unsigned char secnr);
        ////43
        //__int16 __stdcall rf_srd_eeprom(HANDLE icdev,__int16 offset,__int16 lenth,unsigned char *rec_buffer);
        ////44
        //__int16 __stdcall rf_swr_eeprom(HANDLE icdev,__int16 offset,__int16 lenth,unsigned char* send_buffer);
        ////45
        //__int16 __stdcall rf_srd_snr(HANDLE icdev,__int16 lenth,unsigned char *rec_buffer);
        ////46
        //__int16 __stdcall rf_check_write(HANDLE icdev,unsigned long Snr,unsigned char authmode,unsigned char Adr,unsigned char * _data);
        ////47
        //__int16 __stdcall rf_check_writehex(HANDLE icdev,unsigned long Snr,unsigned char authmode,unsigned char Adr, char * _data);
        ////48
        //__int16 __stdcall rf_authentication_2(HANDLE icdev,unsigned char _Mode,unsigned char KeyNr,unsigned char Adr);
        ////49
        //__int16 __stdcall rf_decrement_transfer(HANDLE icdev,unsigned char Adr,unsigned long _Value);
        ////50
        //__int16 __stdcall rf_setport(HANDLE icdev,unsigned char _Byte);
        ////51
        //__int16 __stdcall rf_getport(HANDLE icdev,unsigned char *receive_data);
        ////52
        //__int16 __stdcall rf_gettime(HANDLE icdev,unsigned char *time);
        ////53
        //__int16 __stdcall rf_gettimehex(HANDLE icdev,char *time);
        ////54
        //__int16 __stdcall rf_settime(HANDLE icdev,unsigned char *time);
        ////55
        //__int16 __stdcall rf_settimehex(HANDLE icdev,char *time);
        ////56
        //__int16 __stdcall rf_setbright(HANDLE icdev,unsigned char bright);
        ////57
        //__int16 __stdcall rf_ctl_mode(HANDLE icdev,unsigned char mode);
        ////58
        //__int16 __stdcall rf_disp_mode(HANDLE icdev,unsigned char mode);
        ////59
        //__int16 __stdcall lib_ver(unsigned char *str_ver);
        ////60
        //__int16 __stdcall rf_comm_check(HANDLE icdev,unsigned char _Mode);
        ////61
        //__int16 __stdcall set_host_check(unsigned char _Mode);
        ////62
        //__int16 __stdcall set_host_485(unsigned char _Mode);
        ////63
        //__int16 __stdcall rf_set_485(HANDLE icdev,unsigned char _Mode);
        ////64
        //__int16 __stdcall hex_a(unsigned char *hex,char *a,unsigned char length);
        ////65
        //__int16 __stdcall a_hex(char *a,unsigned char *hex,unsigned char len);
        [DllImport("mwrf32.dll", EntryPoint = "a_hex", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int a_hex(string a, byte[] hex, string len);
        ////66
        ////__int16 __stdcall srd_alleeprom(HANDLE icdev,__int16 offset,__int16 len,unsigned char *receive_buffer);
        ////67
        ////__int16 __stdcall swr_alleeprom(HANDLE icdev,__int16 offset,__int16 len,unsigned char* send_buffer);
        ////68
        //__int16 __stdcall rf_swr_snr(HANDLE icdev,__int16 lenth,unsigned char* send_buffer);
        ////69
        //__int16 __stdcall rf_sam_rst(HANDLE icdev, unsigned char baud, unsigned char *samack);
        ////70
        //__int16 __stdcall rf_sam_trn(HANDLE icdev, unsigned char *samblock,unsigned char *recv);
        ////71
        //__int16 __stdcall rf_sam_off(HANDLE icdev);
        ////72
        //__int16 __stdcall mf2_protocol(HANDLE icdev,unsigned __int16 timeout,unsigned char slen,char *dbuff);
        ////73
        //__int16 __stdcall rf_cpu_rst(HANDLE icdev, unsigned char baud, unsigned char *cpuack);
        ////74
        //__int16 __stdcall rf_cpu_trn(HANDLE icdev, unsigned char *cpublock,unsigned char *recv);
        ////75
        //__int16 __stdcall rf_pro_rst(HANDLE icdev,unsigned char *_Data);
        ////76
        //__int16 __stdcall rf_pro_trn(HANDLE icdev,unsigned char *problock,unsigned char *recv);
        ////77
        //__int16 __stdcall rf_pro_halt(HANDLE icdev);
        ////78
        //void __stdcall Set_Reader_Mode(unsigned char _Mode);
        //__int16 __stdcall rf_get_snr(HANDLE icdev,unsigned char *_Snr);
        #endregion
        /// <summary>
        /// 读卡器所连接的ＣＯＭ口
        /// </summary>
        private int mnCommNum;
        /// <summary>
        /// 初始化时，返回的设备描述符
        /// </summary>
        private int mnEquipDesc;
        private int mnbantnum;
        /// <summary>
        /// 访卡的模式
        /// </summary>
        private int mstrRequestCardMode;
      
        private Int16 mnAuthKeyMode;     

        public CIcCardRWOne(int com, int nBaudRate, int strRequestCardMode, Int16 nAuthKeyMode)
        {
            mnCommNum = com;
            mnbantnum = nBaudRate;
            mstrRequestCardMode = strRequestCardMode;
            mnAuthKeyMode = nAuthKeyMode;
        }

        #region 操作函数集

        /// <summary>
        /// 初始化，建立与COM口的连接
        /// </summary>
        public bool ConnectCOM()
        {
            mnEquipDesc = rf_init(mnCommNum, mnbantnum);//mnBaudRate改为mnbantnum
            if (mnEquipDesc >=0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 断开COM口连接
        /// </summary>
        /// <returns>返回值，等于0成功，-1表示COM的连接没有建立。其他表示不成功。</returns>
        public int disConnectCOM()
        {
            int nback = -1;
            if (mnEquipDesc >= 0)
            {
                nback = rf_exit(mnEquipDesc);
                nback = rf_halt(mnEquipDesc);
            }
            return nback;
        }
        
        /// <summary>
        ///  寻卡，注意寻卡方式，0表示Idle方式；1表示all方式
        /// </summary>
        /// <param name="nCardType">返回寻到的卡的类型</param>
        /// <returns>返回值为0表成功</returns>
        public int RequestICCard(ref short nCardType)
        {
            int nback = -1;
            //int ntemp=-1;
            if (mnEquipDesc >= 0) //设备初始化了
            {
                nback = rf_reset(mnEquipDesc, 20);
                nback = rf_request(mnEquipDesc, mstrRequestCardMode, out nCardType);               
            }
            return nback;
        }

        /// <summary>
        /// 向读写器装载指定扇区的新密码（不与卡片进行通讯），读写器中有16个扇区的密码（0~15），每个扇区有两个密码(KEY A 和 KEY B)
        /// 该函数在与读写器建立连接后，初始化时需要写入
        /// </summary>
        /// <param name="mode">密码类型0 — KEY A 4 — KEY B</param>
        /// <param name="SectorNum">须装载密码的扇区号(0～15)</param>
        /// <param name="csKeyCode">写入读写器的6字节新密码(十六进制字符)</param>
        /// <returns>返回0表示成功，否则表示失败</returns>
        public int LoadKeyToEquip(int mode,int SectorNum,byte[] csKeyCode)
        {
            int nback = -1;
            nback = rf_load_key(mnEquipDesc, mode, SectorNum, csKeyCode);
            return nback;
        }

        /// <summary>
        /// 防冲突的情况下选卡，并选择其中停车卡
        /// </summary>
        /// <returns>返回值为0表示成功，-1表示防冲突函数失败，否则表示选卡不成功。</returns>
        public int SelectCard(ref uint nCardSequenceNum)
        {
            //int nCardSequenceNum;
            byte strtemp ;
            int nback = -1;
            nback = rf_anticoll(mnEquipDesc, 0, out nCardSequenceNum);//查找卡片
            if (nback != 0)
            {
                return -1;
            }
            uint temp = nCardSequenceNum;
            nback = rf_select(mnEquipDesc, temp, out strtemp);
            if (nback == 0)
            {
                rf_beep(mnEquipDesc, 50);
                //rf_halt(mnEquipDesc);
            }
            return nback;
        }

        /// <summary>
        /// 读取感应区内所选定卡的指定扇区（0~15）的指定数据块（0~3）
        /// </summary>
        /// <param name="nSector">指定扇区</param>
        /// <param name="nDBNum">指定扇区中的指定DB块</param>
        /// <param name="breturn">返回的数据（16字节）,十六进制数</param>
        /// <param name="nback">函数执行过程的返回值</param>
        /// <returns>函数返回值，-1表示验证密码失败，0表示操作成功，其他表示读失败</returns>
        public int ReadCard(Int16 nSector,Int16 nDBNum,ref byte[] breturn)
        {
            int nback = -1;
            byte[] bdata = new byte[16];
            short blocknum = (short)(nSector * 4 + nDBNum);
            nback = rf_authentication(mnEquipDesc,mnAuthKeyMode,nSector);
            if (nback != 0)
            {
                return -1;
            }
            nback = rf_read(mnEquipDesc, blocknum, bdata);

            breturn = bdata;
            return nback;
        }

        public int AuthenticatCard(int nSector)
        {
            int nback = -1;
            nback = rf_authentication(mnEquipDesc,(short)mnAuthKeyMode, (short)(nSector));
            return nback;
        }

        /// <summary>
        /// 向感应区内所选定卡的指定扇区（0~15）的指定数据块（0~3）写入数据（16字节或者6字节（密码）十六进制）
        /// </summary>
        /// <param name="nSector">指定扇区</param>
        /// <param name="nDBNum">指定扇区中的指定DB块</param>
        /// <param name="bwrite">写入的数据（16字节或6字节）,十六进制数</param>
        /// <param name="nback">函数执行过程的返回值</param>
        /// <returns>函数返回值，-1表示验证密码失败，0表示操作成功，其他表示读失败</returns>
        public int WriteCard(Int16 nSector, Int16 nDBNum,byte[] bwrite, ref int nback)
        {
            nback = -1;
            int blocknum = nSector * 4 + nDBNum;
            nback = rf_authentication(mnEquipDesc, mnAuthKeyMode, nSector);
            if (nback != 0)
            {
                return -1;
            }
            nback = rf_write(mnEquipDesc, (short)blocknum, bwrite);            
            return nback;
        }

        #endregion
    }  
}
