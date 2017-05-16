using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tests.ConcurrentExample
{
    public class TestMethod3
    {
        private static readonly object O = new object();

        /* 定义 Queue */
        private static Queue<Product> Products { get; set; }

        /**
         * ConcurrentQueue 提供线程安全的先进先出集合.
         * ConcurrentQueue 是完全无锁的，能够支持并发的添加元素，先进先出.
         * 
         */
        private static ConcurrentQueue<Product> ConcurrenProducts { get; set; }

        public static void MockTest()
        {
            Thread.Sleep(1000);
            Products = new Queue<Product>();
            var swTask = new Stopwatch();     // 用于统计时间消耗的
            swTask.Start();

            /* 创建任务 t1  t1 执行 数据集合添加操作 */
            var t1 = Task.Factory.StartNew(AddProducts);

            /*创建任务 t2  t2 执行 数据集合添加操作 */
            var t2 = Task.Factory.StartNew(AddProducts);

            /*创建任务 t3  t3 执行 数据集合添加操作 */
            var t3 = Task.Factory.StartNew(AddProducts);

            Task.WaitAll(t1, t2, t3);
            swTask.Stop();

            Console.WriteLine("List<Product> 当前数据量为：" + Products.Count);
            Console.WriteLine("List<Product> 执行时间为：" + swTask.ElapsedMilliseconds);

            
            Thread.Sleep(1000);
            ConcurrenProducts = new ConcurrentQueue<Product>();
            var swTask1 = new Stopwatch();
            swTask1.Start();

            /* 创建任务 tk1  tk1 执行 数据集合添加操作 */
            var tk1 = Task.Factory.StartNew(AddConcurrenProducts);

            /*创建任务 tk2  tk2 执行 数据集合添加操作*/
            var tk2 = Task.Factory.StartNew(AddConcurrenProducts);

            /*创建任务 tk3  tk3 执行 数据集合添加操作*/
            var tk3 = Task.Factory.StartNew(AddConcurrenProducts);

            Task.WaitAll(tk1, tk2, tk3);
            swTask1.Stop();

            Console.WriteLine("ConcurrentQueue<Product> 当前数据量为：" + ConcurrenProducts.Count);
            Console.WriteLine("ConcurrentQueue<Product> 执行时间为：" + swTask1.ElapsedMilliseconds);

            /*
             * 从执行时间上来看，使用 ConcurrentQueue 相比 LOCK 明显快了很多！
             */
        }

        /// <summary>
        /// 执行集合数据添加操作
        /// </summary>
        public static void AddProducts()
        {
            Parallel.For(0, 30000, (i) =>
            {
                var product = new Product
                {
                    Name = "name" + i,
                    Category = "Category" + i,
                    SellPrice = i
                };
                lock (O)
                {
                    Products.Enqueue(product);
                }
            });

        }

        /// <summary>
        /// 执行集合数据添加操作
        /// </summary>
        public static void AddConcurrenProducts()
        {
            Parallel.For(0, 30000, (i) =>
            {
                var product = new Product
                {
                    Name = "name" + i,
                    Category = "Category" + i,
                    SellPrice = i
                };
                ConcurrenProducts.Enqueue(product);
            });
        }


    }
}
