using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading;
using WorkflowCore;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.Persistence.MySQL;
using WorkFlowTest.Check;
using WorkFlowTest.DependencyInjection;
using WorkFlowTest.Event;
using WorkFlowTest.Looping;
using WorkFlowTest.MutipleOutcomes;
using WorkFlowTest.ParallelForeach;
using WorkFlowTest.ParallelTask;
using WorkFlowTest.PassingData;
using WorkFlowTest.RoleWorkflow;
using WorkFlowTest.WhileLoop;
using DoSomething = WorkFlowTest.ParallelForeach.DoSomething;
using WorkflowCore.Services.DefinitionStorage;
using System.IO;
using System.Text;

namespace WorkFlowTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = ConfigureServices();
            var host = serviceProvider.GetService<IWorkflowHost>();
            var loader = serviceProvider.GetService<IDefinitionLoader>();

            var str = Directory.GetCurrentDirectory();
            string jsonString = File.ReadAllText("C:\\Users\\zhangle\\source\\repos\\WorkFlowTest\\WorkFlowTest\\JsonTest\\JsonTestFlow.json", Encoding.Default);
            var def = loader.LoadDefinition(jsonString, Deserializers.Json);

            host.Start();
            host.StartWorkflow("json", 1, new SetData { Value1 = "one" });
            //HelloWorld(host,serviceProvider);
            //MutipleOutcome(host);
            //PassingData(host);
            //ParallelFoeEach(host);
            //WhileLoop(host);
            //IfStatement(host);
            //ParallelTask(host);
            //DIWorkflow(host);
            //LoopingWorkflow(host);
            //EventWorkflow(host, serviceProvider);
            //RoleWorkflow(host, serviceProvider);

            var workflowController = serviceProvider.GetService<IWorkflowController>();
            var persistWorkflow = serviceProvider.GetService<IPersistenceProvider>();

            Console.ReadKey();
            host.Stop();
            
        }

        private static IServiceProvider ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddLogging(); // WorkflowCore需要用到logging service
            services.AddWorkflow();
            services.AddWorkflowDSL();
            //services.AddWorkflow();

            //services.AddTransient<DoSomething>();
            //services.AddTransient<IMyService, MyService>();
            services.AddTransient<User1>();
            services.AddTransient<PrintMessage>();
            services.AddScoped<DoSomething>();
            services.AddSingleton<RoleStep>();
            services.AddSingleton<IRoleSrvice, RoleService>();


            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }

        public static void HelloWorld(IWorkflowHost host,IServiceProvider provider)
        {

            var workflowController = provider.GetService<IWorkflowController>();
            var persist = provider.GetService<IPersistenceProvider>();
            host.RegisterWorkflow<UserWorkflow1, MyData>();
            host.RegisterWorkflow<OutcomeWorkflow, MutipleOutcomes.MyData>();
            host.Start();
            
            //persist.GetWorkflowInstance
            // Demo1:Hello World
            var id = host.StartWorkflow("workflow1", new MyData() { Message = "message" }).Result;
            //host.s()
           // Console.ReadKey();
           // var wid = host.StartWorkflow("outcome-sample", new MutipleOutcomes.MyData() { Value = 2, id = id}).Result;
            //Console.ReadKey();
            //var a = workflowController.ResumeWorkflow(result1).Result;
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}");
            
           
            
        }

        public static void MutipleOutcome(IWorkflowHost host)
        {
            host.RegisterWorkflow<OutcomeWorkflow, MutipleOutcomes.MyData>();
            host.Start();

            Console.WriteLine("Starting workflow...");
            host.StartWorkflow("outcome-sample", new MutipleOutcomes.MyData() { Value = 2 });
        }

        public static void PassingData(IWorkflowHost host)
        {
            host.RegisterWorkflow<PassingDataWorkflow, MyDataClass>();
            host.RegisterWorkflow<PassingDataWorkflow2, Dictionary<string, int>>();
            host.Start();

            var initialData = new MyDataClass
            {
                Value1 = 2,
                Value2 = 3
            };

            //host.StartWorkflow("PassingDataWorkflow", 1, initialData);


            var initialData2 = new Dictionary<string, int>
            {
                ["Value1"] = 7,
                ["Value2"] = 2
            };

            host.StartWorkflow("PassingDataWorkflow", 1, initialData);
        }
        public static void ParallelFoeEach(IWorkflowHost host)
        {

        }

        public static void WhileLoop(IWorkflowHost host)
        {
            host.RegisterWorkflow<WhileWorkflow, WhileLoop.MyData>();
            host.Start();

            Console.WriteLine("Starting workflow...");
            string workflowId = host.StartWorkflow("While", new WhileLoop.MyData() { Counter = 0 }).Result;
        }

        public static void CheckStatement(IWorkflowHost host)
        {
            host.RegisterWorkflow<CheckWorkflow, CheckData>();
            host.Start();

            Console.WriteLine("Starting Check...");
            string workflowId = host.StartWorkflow("check").Result;
        }

        public static void ParallelTask(IWorkflowHost host)
        {
            host.RegisterWorkflow<ParallelWorkflow, ParallelTask.MyData>();
            host.Start();

            Console.WriteLine("Starting workflow...");
            string workflowId = host.StartWorkflow("parallel-sample", new ParallelTask.MyData() { Counter = 4 }).Result;
        }

        public static void DIWorkflow(IWorkflowHost host)
        {
            host.RegisterWorkflow<DIWorkflow>();
            host.Start();

            host.StartWorkflow("DIWorkflow", 1, null, null);
        }

        public static void LoopingWorkflow(IWorkflowHost host)
        {
            host.RegisterWorkflow<LoopingWorkflow>();
            host.Start();

            host.StartWorkflow("LoopingWorkflow");
        }

        public static void EventWorkflow(IWorkflowHost host, IServiceProvider provider)
        {
            var workflowController = provider.GetService<IWorkflowController>();
            var persist = provider.GetService<IPersistenceProvider>();
            host.RegisterWorkflow<EventSampleWorkflow, EventFlowData>();
            host.Start();

            var initialData = new EventFlowData();
            var workflowId = host.StartWorkflow("EventSampleWorkflow", 1, initialData).Result;

            //var id = "EventSampleWorkflow";
         
            //var workflow = workflowController.SuspendWorkflow(workflowId).Result;
            Console.WriteLine("Enter value to publish");
            string value = Console.ReadLine();
            host.PublishEvent("MyEvent", workflowId, value);

        }

        public static void RoleWorkflow(IWorkflowHost host, IServiceProvider provider)
        {
            host.RegisterWorkflow<RoleSwitchWorkflow, RoleData>();
            var roleService = provider.GetService<IRoleSrvice>();
            roleService.SetStudentRole();
            host.Start();

            RoleData roleData = new RoleData() { branchId = 1, Role = Role.Student };
            host.StartWorkflow("role workflow", roleData);
        }
    }
}
