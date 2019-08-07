using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIEVision.COMMON
{
    public class GlobalContext
    {
        private Hashtable context;
        private static GlobalContext mContext = null;
        public GlobalContext()
        {
            context = new Hashtable();
        }
        public static GlobalContext GetInstance()
        {
            if (mContext == null)
            {
                mContext = new GlobalContext();
            }
            return mContext;
        }
        /// <summary>
        /// 查找指定的全局变量实例是否在容器类
        /// </summary>
        /// <param name="globalInstance"></param>
        /// <returns></returns>
        public bool Contains(object globalInstance)
        {
            return context.Contains(globalInstance);
        }
        /// <summary>
        /// 查找指定的全局变量关键字是否在容器类存在实例
        /// </summary>
        /// <param name="globalName"></param>
        /// <returns></returns>
        public bool Contains(string globalName)
        {
            return context.ContainsKey(globalName);
        }

        /// <summary>
        ///  提供全局变量关键字和实例，加载到容器中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="globalName"></param>
        /// <param name="globalInstance"></param>
        public void Add<T>(string globalName, T globalInstance)
        {

            if (globalInstance != null)
            {
                if (!context.ContainsKey(globalName))
                {
                    context.Add(globalName, globalInstance);
                }
                else
                {
                    throw new ArgumentException("");
                }
            }
            else
            {
                throw new ArgumentException("");
            }

            //return userChild;
        }
        /// <summary>
        /// 指定全局变量关键字获得全局变量实例
        /// </summary>
        /// <param name="globalName"></param>
        /// <returns></returns>
        public object this[string globalName]
        {
            get
            {
                object fglFound = null;
                if (context.ContainsKey(globalName))
                {
                    fglFound = context[globalName];
                }
                return fglFound;
            }
        }
        /// <summary>
        /// 指定全局变量关键字在容器中删除
        /// </summary>
        /// <param name="globalName"></param>
        public void RemoveAt(string globalName)
        {
            if (Contains(globalName))
            {
                context.Remove(globalName);
            }
        }
    
    }
}
