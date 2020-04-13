using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//以下5个是所需要的命名空间
using ServiceStack.Redis;
using Day03_14.Redis.Models;
using System.Threading;
using System.Diagnostics;
using log4netDemo;//日志所需

namespace Day03_14.Redis.Controllers
{
    public class ProController : Controller
    {
        //显示
        public ActionResult Show(int currentpage = 1)
        {
            //打开redis数据库服务
            var processes = Process.GetProcessesByName("redis-server");
            if (processes.Length == 0)
            {
                //设置路径
                Process.Start(@"C:\Users\张凯\Desktop\技能所需文件\Redis所需文件\redis服务\redis-server.exe");
            }
            //创建Redis客户端
            using (IRedisClient redisClient = RedisManager.GetClient())
            {
                //获取Product表
                var products = redisClient.GetTypedClient<Products>();

                //用GetAll()获取所有数据 
                List<Products> pro = products.GetAll().ToList();

                //分页
                if (currentpage < 1)
                {
                    currentpage = 1;
                }
                if (pro.Count() % 2 == 0)
                {
                    ViewBag.totalpage = pro.Count() / 2;
                }
                else
                {
                    ViewBag.totalpage = pro.Count() / 2 + 1;
                }
                ViewBag.currentpage = currentpage;
                return View(pro.OrderByDescending(s => s.Upttime).Skip((currentpage - 1) * 2).Take(2).ToList());

            }
        }
        //查询
        [HttpPost]
        public ActionResult Show(string time1 = "", string time2 = "", string name = "", int currentpage = 1)
        {
            //打开redis数据库服务
            var processes = Process.GetProcessesByName("redis-server");

            if (processes.Length == 0)
            {
                Process.Start(@"C:\Users\张凯\Desktop\技能所需文件\Redis所需文件\redis服务\redis-server.exe");
            }

            using (IRedisClient redisClient = RedisManager.GetClient())
            {

                var products = redisClient.GetTypedClient<Products>();

                List<Products> pro = products.GetAll().ToList();

                if (!string.IsNullOrEmpty(name))
                {
                    pro = pro.Where(s => s.Pname.Contains(name)).ToList();
                }
                if (time1 != "")
                {
                    pro = pro.Where(s => s.Upttime >= Convert.ToDateTime(time1)).ToList();
                }
                if (time2 != "")
                {
                    pro = pro.Where(s => s.Upttime <= Convert.ToDateTime(time2)).ToList();
                }
                //分页
                if (currentpage < 1)
                {
                    currentpage = 1;
                }
                if (pro.Count() % 2 == 0)
                {
                    ViewBag.totalpage = pro.Count() / 2;
                }
                else
                {
                    ViewBag.totalpage = pro.Count() / 2 + 1;
                }
                ViewBag.currentpage = currentpage;
                return View(pro.OrderByDescending(s => s.Upttime).Skip((currentpage - 1) * 2).Take(2).ToList());

            }
        }
        //添加
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Products p)
        {
            try
            {
                using (IRedisClient redisClient = RedisManager.GetClient())
                {
                    var products = redisClient.GetTypedClient<Products>();

                    //添加必须需要
                    p.Id = Convert.ToInt32(products.GetNextSequence());

                    products.Store(p);

                    //打印日志
                    LogHelper.Default.WriteInfo("添加已完成！");

                }

                return Content("<script>alert('添加成功');location.href='/Pro/Show';</script>");
            }
            catch
            {
                return Content("<script>alert('添加失败');location.href='/Pro/Show';</script>");
            }
        }

        //反填
        public ActionResult Upt(int id)
        {
            using (IRedisClient redisClient = RedisManager.GetClient())
            {
                var products = redisClient.GetTypedClient<Products>();

                return View(products.GetById(id));
            }

        }

        //修改 可直接复制添加的方法
        [HttpPost]
        public ActionResult Upt(Products p)
        {
            try
            {
                using (IRedisClient redisClient = RedisManager.GetClient())
                {
                    var products = redisClient.GetTypedClient<Products>();

                    products.Store(p);

                    //添加日志
                    LogHelper.Default.WriteInfo("已修改！");
                }

                return Content("<script>alert('修改成功');location.href='/Pro/Show';</script>");
            }
            catch
            {
                return Content("<script>alert('修改失败');location.href='/Pro/Show';</script>");
            }
        }
        //删除
        public ActionResult Del(int id)
        {
            try
            {
                using (IRedisClient redisClient = RedisManager.GetClient())
                {
                    var products = redisClient.GetTypedClient<Products>();

                    products.DeleteById(id);

                    //打印日志
                    LogHelper.Default.WriteInfo("已删除！");
                }

                return Content("<script>alert('删除成功');location.href='/Pro/Show';</script>");

            }
            catch
            {
                return Content("<script>alert('删除失败');location.href='/Pro/Show';</script>");
            }
        }
    }
}
