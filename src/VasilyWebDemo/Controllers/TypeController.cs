using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VasilyWebDemo.Entity;
using Microsoft.AspNetCore.Mvc;
using Vasily.Driver;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VasilyWebDemo.Controllers
{
    /// <summary>
    /// 广告类型路由
    /// </summary>
    [Route("api/[controller]")]
    public class TypeController :VasilyController<AdType>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TypeController():base("AdSql")
        {
            
        }

        /// <summary>
        /// 获取整个类型集合
        /// </summary>
        /// <returns>实例集合</returns>
        [HttpGet("get_all")]
        public ReturnResult Get()
        {
            return Result(SqlHandler.Get());
        }

        /// <summary>
        /// 根据主键Id获取类型实例
        /// </summary>
        /// <param name="id">主键id</param>
        /// <returns>分类实例</returns>
        [HttpGet("get/{id}")]
        public ReturnResult Get(int id)
        {
            return Result(SqlHandler.GetByPrimaryKey(new { tid = id }));
        }

        /// <summary>
        /// 增加一个分类实例
        /// </summary>
        /// <param name="value">分类实例表单</param>
        [HttpPost("add")]
        public ReturnResult Add([FromBody]AdType value)
        {
            //校验模型
            if (ModelState.IsValid)
            {
                return Result(SqlHandler.Add(value));
            }
            else
            {
                return Result();
            }
        }



        /// <summary>
        /// 根据主键id删除一个分类实例
        /// </summary>
        /// <param name="id">主键id</param>
        [HttpGet("delete/{id}")]
        public ReturnResult Delete(int id)
        {
            return Result(SqlHandler.Delete(new { tid = id }));

        }


        /// <summary>
        /// 修改一个分类实例
        /// </summary>
        /// <param name="value">分类实例表单</param>
        [HttpPost("modify")]
        public ReturnResult Modeify([FromBody]AdType value)
        {
            if (ModelState.IsValid)
            {
                return Result(SqlHandler.Modify(value));

            }
            else
            {
                return Result();
            }
        }


        /*
         // <summary>
        /// 根据gid查询一组广告
        /// </summary>
        /// <param name="id">广告位id</param>
        /// <returns>广告集合</returns>
        [HttpGet("get_gid/{id}")]
        public ReturnResult GetGid(int id)
        {
            return Result(SqlHandler.GetCache("GetByGid", new { gid = id }));
        }


        /// <summary>
        /// 通过标题进行模糊查询
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>广告集合</returns>
        [HttpGet("get_subject/{key}")]
        public ReturnResult GetSubject(string key)
        {
            return Result(SqlHandler.GetCache("fuzzyQuery", new { subject = key }));
        }
         */
    }
}
