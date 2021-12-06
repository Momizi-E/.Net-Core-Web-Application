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
                // NLog�F�Z�b�g�A�b�v�G���[���L���b�`
                logger.Error(exception, "��O�̂��߂Ƀv���O�������~���܂����B");
                throw;
            }
            finally
            {
                // �A�v���P�[�V�������I������O�ɁA�����^�C�}�[/�X���b�h���t���b�V�����Ē�~����悤�ɂ��Ă�������
                // (Linux �ł̃Z�O�����e�[�V�����ᔽ��������Ă��������j
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
                    logging.ClearProviders();                 // NLog �ȊO�Őݒ肳�ꂽ Provider �̖�����.
                    logging.SetMinimumLevel(LogLevel.Trace);  // �ŏ����O���x���̐ݒ�
                })
                .UseNLog();  // NLog�F�ˑ��������̂��߂� NLog �̃Z�b�g�A�b�v;
    }
}