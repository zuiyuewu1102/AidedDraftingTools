using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClassLibrary
{
    public static class MLineTools
    {
        /// <summary>
        /// 用于创建一个新的多线样式
        /// </summary>
        /// <param name="db">图形数据库</param>
        /// <param name="styleName">所要创建的多线样式名</param>
        /// <param name="elements">要加入到多线样式的元素集合</param>
        /// <returns>新建多线样式的Id</returns>
        public static ObjectId CreateMLineStyle(this Database db,string styleName,List<MlineStyleElement> elements)
        {
            //打开当前数据库的多线样式字典
            DBDictionary dict = (DBDictionary)db.MLStyleDictionaryId.GetObject(OpenMode.ForRead);
            //如果已存在指定名称的多线样式、则返回该多线样式的Id
            if (dict.Contains(styleName)) return (ObjectId)dict[styleName];
            //创建一个多线样式对象
            MlineStyle mlineStyle = new MlineStyle
            {
                //设置多线样式的名称
                Name = styleName
            };
            //为多线样式添加新的元素
            foreach (var element in elements)
            {
                mlineStyle.Elements.Add(element, true);
            }
            //切换多线字典为写
            dict.UpgradeOpen();
            //在多线样式字典中加入新创建的多线样式对象，并指定搜索关键字为styleName
            dict.SetAt(styleName, mlineStyle);
            //通知事务处理完成多线样式对象的加入
            db.TransactionManager.AddNewlyCreatedDBObject(mlineStyle, true);
            //为了安全，将多线样式字典切换为读
            dict.DowngradeOpen();
            //返回该多线样式的Id
            return mlineStyle.ObjectId;
        }

        /// <summary>
        /// 删除多线样式
        /// </summary>
        /// <param name="db">图形数据库</param>
        /// <param name="styleName">要删除的多线样式名</param>
        public static void RemoveMLineStyle(this Database db,string styleName)
        {
            //打开当前数据库的多线样式字典
            DBDictionary dict = (DBDictionary)db.MLStyleDictionaryId.GetObject(OpenMode.ForRead);

            if (dict.Contains(styleName))
            {
                dict.UpgradeOpen();
                dict.Remove(styleName);
                dict.DowngradeOpen();
            }
        }
    }
}
