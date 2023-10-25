using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace InfinityReShuiQi
{
    /*
     * This code is modified from IC Card Reader (www.acs.com.hk)
     */
    public class IC
    {
        //常量定义
        public const byte BLOCK0_EN = 0x01;//操作第0块
        public const byte BLOCK1_EN = 0x02;//操作第1块
        public const byte BLOCK2_EN = 0x04;//操作第2块
        public const byte NEEDSERIAL = 0x08;//仅对指定序列号的卡操作
        public const byte EXTERNKEY = 0x10;
        public const byte NEEDHALT = 0x20;//读卡或写卡后顺便休眠该卡，休眠后，卡必须拿离开感应区，再放回感应区，才能进行第二次操作。

        //外部函数声明：让设备发出声响
        [DllImport("OUR_MIFARE.dll", EntryPoint = "pcdbeep", CallingConvention = CallingConvention.StdCall)]
        public static extern byte pcdbeep(UInt32 xms);//xms单位为毫秒 


        //只读卡号
        [DllImport("OUR_MIFARE.dll", EntryPoint = "piccrequest", CallingConvention = CallingConvention.StdCall)]
        public static extern byte piccrequest(byte[] serial);//devicenumber用于返回编号 

        //读取设备编号，可做为软件加密狗用,也可以根据此编号在公司网站上查询保修期限
        [DllImport("OUR_MIFARE.dll", EntryPoint = "pcdgetdevicenumber", CallingConvention = CallingConvention.StdCall)]
        public static extern byte pcdgetdevicenumber(byte[] devicenumber);//devicenumber用于返回编号 


        //轻松读卡
        [DllImport("OUR_MIFARE.dll", EntryPoint = "piccreadex", CallingConvention = CallingConvention.StdCall)]
        public static extern byte piccreadex(byte ctrlword, byte[] serial, byte area, byte keyA1B0, byte[] picckey, byte[] piccdata0_2);
        //参数：说明
        //ctrlword：控制字
        //serial：卡序列号数组，用于指定或返回卡序列号
        //area：指定读卡区号
        //keyA1B0：指定用A或B密码认证,一般是用A密码，只有特殊用途下才用B密码，在这不做详细解释。
        //picckey：指定卡密码，6个字节，卡出厂时的初始密码为6个0xff
        //piccdata0_2：用于返回卡该区第0块到第2块的数据，共48个字节.


        //轻松写卡
        [DllImport("OUR_MIFARE.dll", EntryPoint = "piccwriteex", CallingConvention = CallingConvention.StdCall)]
        public static extern byte piccwriteex(byte ctrlword, byte[] serial, byte area, byte keyA1B0, byte[] picckey, byte[] piccdata0_2);
        //参数：说明
        //ctrlword：控制字
        //serial：卡序列号数组，用于指定或返回卡序列号
        //area：指定读卡区号
        //keyA1B0：指定用A或B密码认证,一般是用A密码，只有特殊用途下才用B密码，在这不做详细解释。
        //picckey：指定卡密码，6个字节，卡出厂时的初始密码为6个0xff
        //piccdata0_2：用于返回卡该区第0块到第2块的数据，共48个字节.


        //修改卡单区的密码
        [DllImport("OUR_MIFARE.dll", EntryPoint = "piccchangesinglekey", CallingConvention = CallingConvention.StdCall)]
        public static extern byte piccchangesinglekey(byte ctrlword, byte[] serial, byte area, byte keyA1B0, byte[] piccoldkey, byte[] piccnewkey);
        //参数：说明
        //ctrlword：控制字
        //serial：卡序列号数组，用于指定或返回卡序列号
        //area：指定读卡区号
        //keyA1B0：指定用A或B密码认证,一般是用A密码，只有特殊用途下才用B密码，在这不做详细解释。
        //piccoldkey：//旧密码
        //piccnewkey：//新密码.

        //修改卡A/B密码及控制字
        [DllImport("OUR_MIFARE.dll", EntryPoint = "piccchangesinglekeyex", CallingConvention = CallingConvention.StdCall)]
        public static extern byte piccchangesinglekeyex(byte ctrlword, byte[] serial, byte area, byte keyA1B0, byte[] piccoldkey, byte[] piccnewkey);
        //参数：说明
        //ctrlword：控制字
        //serial：卡序列号数组，用于指定或返回卡序列号
        //area：指定读卡区号
        //keyA1B0：指定用A或B密码认证,一般是用A密码，只有特殊用途下才用B密码，在这不做详细解释。
        //piccoldkey：//旧密码
        //piccnewkey：//新密码.

        //发送显示内容到读卡器
        [DllImport("OUR_MIFARE.dll", EntryPoint = "lcddispfull", CallingConvention = CallingConvention.StdCall)]
        public static extern byte lcddispfull(string lcdstr);
        //参数：说明
        //lcdstr：显示内容


        //写设备存储区1
        [DllImport("OUR_MIFARE.dll", EntryPoint = "pcdsetcustomizedata1", CallingConvention = CallingConvention.StdCall)]
        public static extern byte pcdsetcustomizedata1(byte[] readerdata);//devicenumber用于传递写数据 

        //读设备存储区1
        [DllImport("OUR_MIFARE.dll", EntryPoint = "pcdgetcustomizedata1", CallingConvention = CallingConvention.StdCall)]
        public static extern byte pcdgetcustomizedata1(byte[] readerdata);//devicenumber用于传递写数据 

        //写设备存储区2
        [DllImport("OUR_MIFARE.dll", EntryPoint = "pcdsetcustomizedata2", CallingConvention = CallingConvention.StdCall)]
        public static extern byte pcdsetcustomizedata2(byte[] readerdata);//devicenumber用于传递写数据 

        //读设备存储区2
        [DllImport("OUR_MIFARE.dll", EntryPoint = "pcdgetcustomizedata2", CallingConvention = CallingConvention.StdCall)]
        public static extern byte pcdgetcustomizedata2(byte[] readerdata, byte[] devno);//devicenumber用于传递写数据 
    }
}
