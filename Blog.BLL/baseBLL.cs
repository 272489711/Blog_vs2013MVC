using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Domain;
using Blog.DAL;
using System.Linq.Expressions;

namespace Blog.BLL
{
    public class baseBLL<T> where T:MyBaseModel
    {
        /// <summary>
        /// 数据访问层接口对象
        /// </summary>
        BaseDAL<T> myDAL = new BaseDAL<T>();


        #region 1 增加数据 Add(T model)
        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="model">实体模型</param>
        public void Add(T model)
        {
            myDAL.Add(model);
        } 
        #endregion

        #region 2.1 删除bool Del(T model, bool IsSoftDel = true)
        /// <summary>
        /// 删除给出的模型
        /// </summary>
        /// <param name="model">模型对象</param>
        /// <param name="IsSoftDel">是否软删除(默认为true)</param>
        /// <returns></returns>
        public bool Del(T model, bool isAsTracking=true, bool IsSoftDel = true)
        {
            return myDAL.Del(model,isAsTracking, IsSoftDel);
        } 
        #endregion

        #region 2.2 删除bool Del(Expression<Func<T, bool>> delWhere, bool IsSoftDel = true)
        /// <summary>
        /// 根据lambda表达式条件来删除对象
        /// </summary>
        /// <param name="delWhere">对象的lambda表达式</param>
        /// <param name="IsSoftDel">是否软删除（默认为软删除）</param>
        /// <returns></returns>
        public bool Del(Expression<Func<T, bool>> delWhere,bool IsSoftDel = true)
        {
            return myDAL.Del(delWhere,IsSoftDel);
        } 
        #endregion

        #region 3.1 修改bool Mod(T model, params string[] Pros)
        /// <summary>
        /// 修改实体的某些属性
        /// </summary>
        /// <param name="model">已修改属性的实体</param>
        /// <param name="Pros">要修改的属性名称（动态数组）</param>
        /// <returns></returns>
        public bool Mod(T model, params string[] Pros)
        {
            return myDAL.Mod(model, Pros);
        } 
        #endregion

        #region 3.2 修改Mod(Expression<Func<T, bool>> modWhere, bool isModDelData, T model, params String[] pros)
        /// <summary>
        /// 根据条件选出目标对象进行某些属性批量修改
        /// </summary>
        /// <param name="modWhere">lambda表达式条件</param>
        /// <param name="isModDelData">已软删除的是否也修改</param>
        /// <param name="model">修改的对象模型</param>
        /// <param name="pros">要修改的对象属性名字符串</param>
        public void Mod(Expression<Func<T, bool>> modWhere, bool isModDelData, T model, params String[] pros)
        {
            myDAL.Mod(modWhere, isModDelData, model, pros);
        } 
        #endregion

        #region 4.1 查询IQueryable<T> GetList(Expression<Func<T, bool>> where, bool selectDel = false)
        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="selectDel">查询结果集中是否包含已软删除数据（默认为false）</param>
        /// <returns>IQueryable查询结果集</returns>
        public IQueryable<T> GetList(Expression<Func<T, bool>> where, bool selectDel = false,bool isAsNoTracking=true)
        {
            return myDAL.GetList(where, selectDel,isAsNoTracking);
        } 
        #endregion

        #region 4.2 查询IQueryable<T> GetList<Tkey>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLa, Expression<Func<T, Tkey>> orderByLa = null, bool isAsc = true, bool selectDel = false)
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
        /// <returns>IQueryable查询结果集</returns>
        public IQueryable<T> GetList<Tkey>(int pageIndex, int pageSize,out int total, Expression<Func<T, bool>> whereLa, Expression<Func<T, Tkey>> orderByLa = null, bool isAsc = true, bool selectDel = false,bool isAsNoTracking=true)
        {
            return myDAL.GetList(pageIndex, pageSize,out total, whereLa, orderByLa, isAsc, selectDel,isAsNoTracking);
        } 
        #endregion

        public int save(bool validRequired=true)
        {
            return myDAL.save(validRequired);
        }


    }
}
