using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace 百度API翻译
{
    public class  JsonTranslation
    {
        public string from;
        public string to;
        public List<Result> trans_result;//类类型需要泛型格式来容纳
    }

    public class Result
    {
        public string src;
        public string dst;
    }

    public class HotKey
    {
        /// <summary>
        /// 如果函数执行失败,返回0.若要得到扩展错误信息,调用GetLastError
        /// </summary>
        /// <param name="hWnd">要定义热键的窗口句柄</param>
        /// <param name="id">定义热键ID</param>
        /// <param name="fsModifiers">表示热键是否在按Alt,Ctrl,Shift,Windows等键时才会生效</param>
        /// <param name="vk">定义热键内容</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(
            IntPtr hWnd,
            UInt32 id,
            KeyModifiers fsModifiers,
            Keys vk
            );

        /// <summary>
        /// 退出时取消注册热键
        /// </summary>
        /// <param name="hWnd">要取消热键的窗口句柄</param>
        /// <param name="id">要取消热键的ID</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(
            IntPtr hWnd,
            UInt32 id
            );

        [DllImport("kernel32.dll")]
        public static extern UInt32 GlobalAddAtom(String lpString);

        [DllImport("kernel32.dll")]
        public static extern UInt32 GlobalDeleteAtom(UInt32 nAtom);

        /// <summary>
        /// 定义了辅助键的名称(将数字转变为字符以便于记忆,也可去除此枚举而直接使用数值)
        /// </summary>
        [Flags()]
        public enum KeyModifiers
        {
            None=0,
            Alt=1,
            Ctrl=2,
            Shift=4,
            WindowsKey=8
        }

    }
}
