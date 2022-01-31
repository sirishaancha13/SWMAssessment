using Ninject;
using SWM.TechnicalAssessment.Services;
using System;
using System.Threading.Tasks;

namespace SWM.TechnicalAssessment
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var kernel = new StandardKernel();
            kernel.Bind<IClientService>().To<ClientService>();
            kernel.Bind<IUserService>().To<UserService>();
            
            var userService = kernel.Get<IUserService>();
            RunAsync(userService).GetAwaiter().GetResult();
        }

        public static async Task RunAsync(IUserService userService)
        {
            try
            {
                Console.WriteLine(await userService.GetUserDetails("sampletest", 42, 23));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();
        }
    }    
}
