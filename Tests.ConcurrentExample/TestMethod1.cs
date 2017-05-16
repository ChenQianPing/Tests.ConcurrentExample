using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.ConcurrentExample
{
    public class TestMethod1
    {
        private static object _o = new object();
        private static List<Product> Products { get; set; }

        /*
         * 代码中 创建三个并发线程 来操作_Products 集合；
         * System.Collections.Generic.List 这个列表在多个线程访问下，
         * 不能保证是安全的线程，所以不能接受并发的请求，我们必须对ADD方法的执行进行串行化；
         * 
         * 开辟了三个线程，通过循环向集合中添加数据，每个线程执行1000次(三个线程之间的操作是同时进行的，也是并行的)，那么，理论上结果应该是3000.
         * 数组，集合确实不能保证线程安全，确实不能预防并发.
         */
        public static void MockTest()
        {
            Products = new List<Product>();

            /* 创建任务 t1  t1 执行 数据集合添加操作 */
            var t1 = Task.Factory.StartNew(AddProducts);
            /* 创建任务 t2  t2 执行 数据集合添加操作 */
            var t2 = Task.Factory.StartNew(AddProducts);
            /* 创建任务 t3  t3 执行 数据集合添加操作 */
            var t3 = Task.Factory.StartNew(AddProducts);

            Task.WaitAll(t1, t2, t3);

            Console.WriteLine(Products.Count);
        }

        /// <summary>
        /// 执行集合数据添加操作
        /// </summary>
        public static void AddProducts()
        {
            Parallel.For(0, 1000, (i) =>
            {
                var product = new Product
                {
                    Name = "name" + i,
                    Category = "Category" + i,
                    SellPrice = i
                };
                Products.Add(product);
            });
        }
    }
}



/**
 * 背景   
 * C#命名空间：System.Collenctions和System.Collenctions.Generic 中提供了很多列表、集合和数组。
 * 例如：List<T>集合，数组Int[]，String[] ......，Dictory<T,T>字典等等。
 * 但是这些列表、集合和数组的线程都不是安全的，不能接受并发请求。
  */
