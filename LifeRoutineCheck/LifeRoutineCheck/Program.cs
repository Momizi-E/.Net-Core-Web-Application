using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LifeRoutineCheck
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception exception)
            {
                // NLog：セットアップエラーをキャッチ
                logger.Error(exception, "例外のためにプログラムを停止しました。");
                throw;
            }
            finally
            {
                // アプリケーションを終了する前に、内部タイマー/スレッドをフラッシュして停止するようにしてください
                // (Linux でのセグメンテーション違反を回避してください）
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();                 // NLog 以外で設定された Provider の無効化.
                    logging.SetMinimumLevel(LogLevel.Trace);  // 最小ログレベルの設定
                })
                .UseNLog();  // NLog：依存性注入のための NLog のセットアップ;
    }
}
