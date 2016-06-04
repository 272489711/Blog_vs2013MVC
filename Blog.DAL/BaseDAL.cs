using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Domain;
using System.Linq.Expressions;
using System.Reflection;
using System.Data.Entity;
using System.Runtime.Remoting.Messaging;

namespace Blog.DAL
{
    public class BaseDAL<T> where T : MyBaseModel
    {
        Model1Container _db;
        /// <summary>
        /// 设置db上下文属性，用session保证单一实例
        /// </summary>
        public Model1Container db
        {
            get
            {
                DbContext dbContext = CallContext.GetData(typeof(BaseDAL<T>).Name) as DbContext;
                if (dbContext == null)
                {
                    dbContext = new Model1Container();
                    //dbContext.Configuration.ValidateOnSaveEnabled = false;
                    //将新创建的 ef上下文对象 存入线程
                    CallContext.SetData(typeof(BaseDAL<T>).Name, dbContext);
                }
                return dbContext as Model1Container;
            }
        }

     

        #region 新增Add(T model)
        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="model">实体模型</param>
        public void Add(T model)
        {
            if (model.CreateTime == null||model.CreateTime<=DateTime.MinValue)
                model.CreateTime = DateTime.Now;
            model.UpTime = DateTime.Now;
            model.IsDel = false;
            db.Set<T>().Add(model);
        }
        #endregion

        #region 删除特定对象bool Del(model, IsSoftDel（可选参数）)
        /// <summary>
        /// 删除给出的模型
        /// </summary>
        /// <param name="model">模型对象</param>
        /// <param name="IsSoftDel">是否软删除(默认为true)</param>
        /// <returns></returns>
        public bool Del(T model,bool isAsTracking=true ,bool IsSoftDel = true)
        {
            if (!IsSoftDel)
            {
                db.Set<T>().Attach(model);
                db.Set<T>().Remove(model);
                db.SaveChanges();
            }
            else
            {
                if (!isAsTracking)
                {
                    model.UpTime = DateTime.Now;
                    model.IsDel = true;
                }
                else
                {
                    db.Configuration.ValidateOnSaveEnabled = false;
                    model.IsDel = true;
                    model.UpTime = DateTime.Now;
                    var m = db.Entry<T>(model);
                    m.State = System.Data.Entity.EntityState.Unchanged;
                    m.Property("UpTime").IsModified = true;
                    m.Property("IsDel").IsModified = true;
                    var result = db.SaveChanges();
                    db.Configuration.ValidateOnSaveEnabled = true;
                    return result >= 1;
                }

            }
            return true;
        }
        #endregion

        #region 由lambda条件来删除对象bool Del(delWhere（lambda）, bool IsSoftDel = true)
        /// <summary>
        /// 根据lambda表达式条件来删除对象
        /// </summary>
        /// <param name="delWhere">对象的lambda表达式</param>
        /// <param name="IsSoftDel">是否软删除（默认为软删除）</param>
        /// <returns></returns>
        public bool Del(Expression<Func<T, bool>> delWhere, bool IsSoftDel = true)
        {
            //查询所有满足条件的实体对象
            var modelS = db.Set<T>().AsNoTracking().Where(delWhere).ToList();   //注意这里要asnotracking，否则下面怎么改都是原来的数据
            if (!IsSoftDel)
            {
                modelS.ForEach(m =>
                {
                    //附加到上下文
                    db.Set<T>().Attach(m);
                    //标记为删除状态
                    db.Set<T>().Remove(m);
                });
            }
            else //进行软删除
            {
                db.Configuration.ValidateOnSaveEnabled = false;
                foreach (var mymodel in modelS)
                {
                    mymodel.IsDel = true;
                    mymodel.UpTime = DateTime.Now;
                    var entry = db.Entry<T>(mymodel);
                    entry.State = System.Data.Entity.EntityState.Unchanged;
                    entry.Property("IsDel").IsModified = true;
                    entry.Property("UpTime").IsModified = true;
                }
                var result = db.SaveChanges();
                db.Configuration.ValidateOnSaveEnabled = true;
                return result >= 1;
            }
            return true;
        } 
        #endregion

        #region 修改给出实体的某些属性bool Mod(T model, params string[] Pros)
        /// <summary>
        /// 修改实体的某些属性
        /// </summary>
        /// <param name="model">已修改属性的实体</param>
        /// <param name="Pros">要修改的属性名称（动态数组）</param>
        /// <returns></returns>
        public bool Mod(T model, params string[] Pros)
        {
            //关闭检查
            db.Configuration.ValidateOnSaveEnabled = false;
            //附加到上下文
            model.UpTime = DateTime.Now;
            var m = db.Entry<T>(model);
            //把全部属性标记为 没有修改
            m.State = System.Data.Entity.EntityState.Unchanged;
            for (int i = 0; i < Pros.Length; i++)
            {
                //标记要修改的属性                
                m.Property(Pros[i]).IsModified = true;
            }
            m.Property("UpTime").IsModified = true;
            int num = db.SaveChanges();
            //打开检查
            db.Configuration.ValidateOnSaveEnabled = true;
            return num >= 1;
        } 
        #endregion

        #region 批量修改对象的某些属性Mod(Expression<Func<T, bool>> modWhere, bool isModDelData, T model, params String[] pros)
        /// <summary>
        /// 根据条件选出目标对象进行某些属性批量修改
        /// </summary>
        /// <param name="modWhere">lambda表达式条件</param>
        /// <param name="isModDelData">已软删除的是否也修改</param>
        /// <param name="model">修改的对象模型</param>
        /// <param name="pros">要修改的对象属性名字符串</param>
        public void Mod(Expression<Func<T, bool>> modWhere, bool isModDelData, T model, params String[] pros)
        {
            model.UpTime = DateTime.Now;
            List<T> modList;
            //获取由lambda表达式筛选出的 要修改的对象集合
            if (isModDelData)
            {
                modList = db.Set<T>().Where(modWhere).ToList();
            }
            else
            {
                Expression<Func<T, bool>> delexp = w => w.IsDel == false;

                modList = db.Set<T>().Where(Helper.AddLinq.And(delexp, modWhere)).ToList();//拼接lambda表达式，筛选出未软删除的对象

            }
            Type type = typeof(T);
            //获取实体类T的所有公有属性
            List<PropertyInfo> proInfos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
            List<PropertyInfo> properties = new List<PropertyInfo>();
            //循环把pros包含的字符串属性以及UpTime属性转换成properties属性对象
            proInfos.ForEach(p =>
                {
                    for (int i = 0; i < pros.Length; ++i)
                        if (p.Name == pros[i].Trim() || p.Name == "UpTime")
                        {
                            properties.Add(p);
                            break;
                        }
                });
            //二重循环修改目标集合的 属性值
            foreach (PropertyInfo property in properties)
            {
                //从已修改对象中逐个取要修改的属性值
                var newValue = property.GetValue(model);
                foreach (T myModel in modList)
                {
                    //修改目标对象的属性值
                    property.SetValue(myModel, newValue);
                }
            }
        } 
        #endregion

        #region 根据lambda条件查询IQueryable<T> GetList(Expression<Func<T, bool>> where, bool selectDel = false)
        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="selectDel">查询结果集中是否包含已软删除数据（默认为false）</param>
        /// <returns>IQueryable查询结果集</returns>
        public IQueryable<T> GetList(Expression<Func<T, bool>> where, bool selectDel = false,bool isAsNoTracking=true)
        {
            if (!selectDel)
            {
                where = Helper.AddLinq.And(where, w => w.IsDel == false);
            }
            if (isAsNoTracking)
            {
                return db.Set<T>().AsNoTracking().Where(where);
            }
            else
            {
                return db.Set<T>().Where(where);
            }
        } 
        #endregion

        /// <summary>
        /// 根据lambda条件分页查询，兼排序功能
        /// </summary>
        /// <typeparam name="Tkey">排序属性的数据类型</typeparam>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页多少记录</param>
        /// <param name="whereLa">查询条件</param>
        /// <param name="orderByLa">排序属性（默认为null）</param>
        /// <param name="isAsc">是否升序（默认为true）</param>
        /// <param name="selectDel">查询结果是否包含已删除数据（默认为false）</param>
        /// <returns></returns>
        #region 分页兼排序功能查询IQueryable<T> GetList<Tkey>(int pageIndex,int pageSize,Expression<Func<T,bool>> whereLa,Expression<Func<T,Tkey>> orderByLa=null,bool isAsc=true,bool selectDel=false)
        public IQueryable<T> GetList<Tkey>(int pageIndex, int pageSize,out int total, Expression<Func<T, bool>> whereLa, Expression<Func<T, Tkey>> orderByLa = null, bool isAsc = true, bool selectDel = false,bool isAsNoTracking=true)
        {
            if (!selectDel)
            {
                whereLa = Helper.AddLinq.And(whereLa, w => w.IsDel == false);
            }
            IQueryable<T> t = null;
            if (isAsNoTracking)
            {
                t = db.Set<T>().AsNoTracking().Where(whereLa);
            }
            else
            {
                t = db.Set<T>().Where(whereLa);
            }
            if (orderByLa != null)
            {
                if (isAsc)
                    t = t.OrderBy(orderByLa);
                else
                    t = t.OrderByDescending(orderByLa);
            }
            int count = t.Count();
            total = count / pageSize + ((count % pageSize) > 0 ? 1 : 0);
            return t.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        } 
        #endregion


        #region 保存到数据库 int save（validRequired）
        /// <summary>
        /// 保存修改到数据库
        /// </summary>
        /// <returns></returns>
        public int save(bool validRequired=true)
        {
            if (validRequired == false)
            {
                db.Configuration.ValidateOnSaveEnabled = false;
                int result = db.SaveChanges();
                db.Configuration.ValidateOnSaveEnabled = true;
                return result;
            }
            else
                return db.SaveChanges();
        } 
        #endregion
    }
}
