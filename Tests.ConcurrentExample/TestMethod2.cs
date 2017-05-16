using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.ConcurrentExample
{

    public class TestMethod2
    {
        private static readonly object O = new object();
        private static List<Product> Products { get; set; }

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

        public static void AddProducts()
        {
            Parallel.For(0, 1000, (i) =>
            {
                lock (O)
                {
                    var product = new Product
                    {
                        Name = "name" + i,
                        Category = "Category" + i,
                        SellPrice = i
                    };
                    Products.Add(product);
                }
            });

            /*
             * 自C#2.0以来，LOCK是一直存在的.
             * 使用LOCK(互斥锁)是可以做到防止并发的.
             * 但是锁的引入，带来了一定的开销和性能的损耗，并降低了程序的扩展性，而且还会有死锁的发生(虽说概率不大，但也不能不防啊)，因此：使用LOCK进行并发编程显然不太适用.
             */
        }

    }
}
