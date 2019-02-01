using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBF模拟
{
    class Program
    {
        static string[] a;
        static void Main(string[] args)
        {
            a = args;
            runS();

            //double bestV = 0, bestBh = 0, ve = 0, vet = 0; // 最优解
            //for (double V = 5.0; V <= 15; V += 0.1)
            //{
            //    for (double B = 0.1; B <= 2.0; B += 0.01)
            //    {
            //        vet = bestB(0.2, .0000103, 0.013, 20, B, 0.01, V, 0.00001);
            //        if (vet > ve)
            //        {
            //            ve = vet;
            //            bestV = V; bestBh = B;
            //        }
            //    }
            //    Console.Write("({0:F4},{1:F4}),", bestV, bestBh);
            //    bestV = 0; bestV = 0;
            //}
            //Console.ReadKey(false);

        }

        static void runS()
        {
            if (a.Length == 0)
            {
                Console.Write("默认数据\n");
                move(0.2, .0000103, 0.013, 20, 0.86, 0.01, 7.4, 0.00001);
            }
            else
                if (a[0].Equals("?") == true)
                    Console.WriteLine("摩擦系数 空阻系数 质量 初速度 线圈磁场 轨道宽度 轨道电压 子弹电阻");
                else
                    move(Convert.ToDouble(a[0]), Convert.ToDouble(a[1]), Convert.ToDouble(a[2]), Convert.ToDouble(a[3]), Convert.ToDouble(a[4]), Convert.ToDouble(a[5]), Convert.ToDouble(a[6]), Convert.ToDouble(a[7]));
            Console.ReadKey(false);
        }

        static void move(double u ,double k_a ,double mass, double v0, double Bh, double w, double E0,  double Rl) // w: width  v0: initial velocity
        {
            double x = 0, l = 0.6, dt = 0.0000001, time = 0; double vo = v0;
            double Q = 0, We = 0, heat = 0, W = 0; //tempUp = 0;

            // int i = 0;

            double force = 0;
            double air_f = 0;
            double Er = 0;
            double cur = 0;
            double a = 0;
            while (x <= l)
            {
                Er = v0 * Bh * w; // 逆向电压
                cur = (E0 - Er) / Rl; // 电流
                air_f = k_a * v0 * v0; // 空气阻力

                force = cur * Bh * w - u * mass * 9.732 - air_f; // 合外力
                a = force / mass; //加速度

                x += v0 * dt; // 小量速度累加得位移
                v0 += a * dt; // 小量加速度累加得速度变化量
                Q += cur * dt; // 流过子弹的电荷量
                We += cur * (E0 - Er) * dt; // 电磁力做功
                W += cur * E0 * dt; // 导轨电压做功
                heat += cur * cur * Rl * dt; // 加热
                time += dt; // 时间计算

                //if (i == 100)
                //{
                //    Console.Write("({0},{1}),", time * 10000, Er);
                //    i = 0;
                //}
                //else i++;
            }
            // tempUp = heat / (880 * mass);

            double Ek = 0.5 * mass * v0 * v0;

            Console.Write("物理量单位遵循SI\n摩擦系数：{0} 空阻系数：{1} 质量：{2} 初速度：{3} \n线圈磁场：{4} 轨道宽度：{5} 轨道电压：{6} 子弹电阻：{7}", u, k_a, mass, vo, Bh, w, E0, Rl);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("\n位移：{0:F4}m\n最终速度：{1:F4}m/s\n最终动能：{2:F4}J\n效率：{3:F4}\n流经电荷：{4:F4}C\n子弹及导轨加热或总热量：{5:F4}J\n导轨电源做功：{6:F4}J\n用时：{7:F4}s\n",
                            x, v0, Ek, Ek / W, Q, heat, W, time);
            Console.ForegroundColor = ConsoleColor.White;

            //Console.WriteLine(":{0:F4}", v0);
        }

        static double bestB(double u, double k_a, double mass, double v0, double Bh, double w, double E0, double Rl)
        {
            double x = 0, l = 0.6, dt = 0.0000001, time = 0; double vo = v0;

            // int i = 0;

            double force = 0;
            double air_f = 0;
            double Er = 0;
            double cur = 0;
            double a = 0;
            while (x <= l)
            {
                Er = v0 * Bh * w; // 逆向电压
                cur = (E0 - Er) / Rl; // 电流
                air_f = k_a * v0 * v0; // 空气阻力

                force = cur * Bh * w - u * mass * 9.732 - air_f; // 合外力
                a = force / mass; //加速度

                x += v0 * dt; // 小量速度累加得位移
                v0 += a * dt; // 小量加速度累加得速度变化量
                time += dt; // 时间计算

                //if (i == 100)
                //{
                //    Console.Write("({0},{1}),", time * 10000, Er);
                //    i = 0;
                //}
                //else i++;
            }
            // tempUp = heat / (880 * mass);

            return v0;
        }
    }
}
